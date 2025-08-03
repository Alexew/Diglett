#nullable disable

using Diglett.Core.Collections;
using Diglett.Web.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.TagHelpers
{
    [OutputElementHint("nav")]
    [HtmlTargetElement("pagination", Attributes = ListItemsAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class PaginationTagHelper : BaseTagHelper
    {
        const string ListItemsAttributeName = "dt-list-items";
        const string ShowPaginatorAttributeName = "dt-show-paginator";
        const string MaxPagesToDisplayAttributeName = "dt-max-pages";
        const string ContentClassNameAttribute = "dt-content-class";
        const string QueryParamNameAttributeName = "dt-query-param";

        [HtmlAttributeName(ListItemsAttributeName)]
        public IPageable ListItems { get; set; }

        [HtmlAttributeName(ShowPaginatorAttributeName)]
        public bool ShowPaginator { get; set; } = true;

        [HtmlAttributeName(MaxPagesToDisplayAttributeName)]
        public int MaxPagesToDisplay { get; set; } = 11;

        [HtmlAttributeName(ContentClassNameAttribute)]
        public string ContentCssClass { get; set; }

        [HtmlAttributeName(QueryParamNameAttributeName)]
        public string QueryParamName { get; set; } = "page";

        protected override void ProcessCore(TagHelperContext context, TagHelperOutput output)
        {
            if (!ShowPaginator || ListItems == null || ListItems.TotalCount == 0 || ListItems.TotalPages <= 1)
            {
                output.SuppressOutput();
                return;
            }

            if (QueryParamName.IsEmpty())
            {
                QueryParamName = "page";
            }

            var items = CreateItemList();

            output.Attributes.Add("aria-label", "Page navigation");

            var itemsUl = new TagBuilder("ul");
            itemsUl.AppendCssClass("pagination mb-0");

            if (ContentCssClass.HasValue())
            {
                itemsUl.AppendCssClass(ContentCssClass);
            }

            var itemsCount = items.Count;
            foreach (var item in items)
            {
                AppendItem(itemsUl, item, itemsCount);
            }

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml(itemsUl);
        }

        protected virtual List<PagerItem> CreateItemList()
        {
            if (!ShowPaginator || ListItems.TotalPages <= 1)
                return [];

            var currentPage = ListItems.PageNumber;
            var totalPages = ListItems.TotalPages;
            var maxPages = Math.Max(5, MaxPagesToDisplay);
            var items = new List<PagerItem>
            {
                new(currentPage - 1, "Previous", GenerateUrl(currentPage - 1), PagerItemType.PreviousPage)
                {
                    State = (currentPage > 1) ? PagerItemState.Normal : PagerItemState.Disabled
                }
            };

            if (maxPages > 0)
            {
                var numPages = Math.Min(totalPages, maxPages);

                // xl range
                var (start, end) = CalculateItemRange(maxPages);

                var hasStartGap = start > 1;
                var hasEndGap = end < totalPages;

                // <= lg range (max 9 items)
                var (startLg, endLg) = numPages > 9 ? CalculateItemRange(9, hasStartGap, hasEndGap) : (start, end);

                // <= sm range (max 7 items)
                var (startSm, endSm) = numPages > 7 ? CalculateItemRange(7, hasStartGap, hasEndGap) : (startLg, endLg);

                // xs range (max 5 items)
                var (startXs, endXs) = numPages > 5 ? CalculateItemRange(5, hasStartGap, hasEndGap) : (startSm, endSm);

                if (start > 1)
                {
                    items.Add(new PagerItem(1, "1", GenerateUrl(1)));

                    if (hasStartGap)
                    {
                        items.Add(new PagerItem(start - 1, "...", GenerateUrl(start - 1), PagerItemType.Gap));
                    }
                }

                for (int i = start; i <= end; i++)
                {
                    var displayBreakpointUp = (string?)null;

                    if (i != currentPage && i > 1 && i < totalPages)
                    {
                        if (i < startLg || i > endLg)
                        {
                            displayBreakpointUp = "xl";
                        }
                        else if (i < startSm || i > endSm)
                        {
                            displayBreakpointUp = "md";
                        }
                        else if (i < startXs || i > endXs)
                        {
                            displayBreakpointUp = "sm";
                        }
                    }

                    var state = PagerItemState.Normal;
                    if (i == currentPage || (currentPage <= 0 && i == 1))
                    {
                        state = PagerItemState.Selected;
                    }

                    items.Add(new PagerItem(i, i.ToString(), GenerateUrl(i))
                    {
                        State = state,
                        DisplayBreakpointUp = displayBreakpointUp
                    });
                }

                if (hasEndGap)
                {
                    items.Add(new PagerItem(end + 1, "...", GenerateUrl(end + 1), PagerItemType.Gap));

                    if (end < totalPages - 2)
                    {
                        items.Add(new PagerItem(totalPages, totalPages.ToString(), GenerateUrl(totalPages)));
                    }
                }
            }

            items.Add(new PagerItem(currentPage + 1, "Next", GenerateUrl(currentPage + 1), PagerItemType.NextPage)
            {
                State = (currentPage == totalPages) ? PagerItemState.Disabled : PagerItemState.Normal,
            });

            return items;
        }

        private (int start, int end) CalculateItemRange(int maxPages, bool? hasStartGap = null, bool? hasEndGap = null)
        {
            var totalPages = ListItems.TotalPages;
            var currentPage = ListItems.PageNumber;
            var start = 1;
            var end = totalPages;

            if (totalPages > maxPages)
            {
                var middle = (int)Math.Ceiling(maxPages / 2d) - 1;
                var below = (currentPage - middle);
                var above = (currentPage + middle);

                if (below < 2)
                {
                    above = maxPages;
                    below = 1;
                }
                else if (above > (totalPages - 2))
                {
                    above = totalPages;
                    below = totalPages - maxPages + 1;
                }

                start = below;
                end = above;
            }

            if (hasStartGap.HasValue)
            {
                if (hasStartGap == true)
                {
                    start += 2;
                }
                else if (start > 1)
                {
                    start++;
                }
            }
            else
            {
                if (start > 1)
                {
                    start += 2;
                }
            }

            if (hasEndGap.HasValue)
            {
                if (hasEndGap == true)
                {
                    end -= 2;
                }
                else if (end < totalPages)
                {
                    end--;
                }
                else if (currentPage == start - 1)
                {
                    end -= 2;
                }
            }
            else
            {
                if (end < totalPages)
                {
                    end -= 2;
                }
            }

            return (start, end);
        }

        protected virtual void AppendItem(TagBuilder itemsUl, PagerItem item, int itemsCount)
        {
            var itemLi = new TagBuilder("li");

            using var classList = itemLi.GetClassList();

            classList.Add("page-item");

            if (item.State == PagerItemState.Disabled)
            {
                classList.Add("disabled");
            }
            else if (item.State == PagerItemState.Selected)
            {
                classList.Add("active");
            }

            if (item.CssClass.HasValue())
            {
                classList.Add(item.CssClass!.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            }

            var isResponsive = itemsCount >= 6;
            if (isResponsive && item.DisplayBreakpointUp.HasValue())
            {
                classList.Add("d-none", $"d-{item.DisplayBreakpointUp}-inline-block");
            }

            classList.Dispose();

            var isClickable = item.Type is PagerItemType.Page or PagerItemType.Gap;
            var isNavOrClickable = isClickable || item.IsNavButton;
            var innerAOrSpan = new TagBuilder(isNavOrClickable ? "a" : "span");

            if (isNavOrClickable)
            {
                innerAOrSpan.Attributes.Add("href", item.Url);

                if (item.State == PagerItemState.Selected)
                {
                    innerAOrSpan.Attributes.Add("aria-current", "page");
                }
                if (item.State == PagerItemState.Disabled)
                {
                    innerAOrSpan.Attributes.Add("aria-disabled", "true");
                    innerAOrSpan.Attributes.Add("tabindex", "-1");
                }

                if (item.Type == PagerItemType.PreviousPage)
                {
                    innerAOrSpan.Attributes.Add("rel", "prev");
                }
                else if (item.Type == PagerItemType.NextPage)
                {
                    innerAOrSpan.Attributes.Add("rel", "next");
                }

                if (item.IsNavButton)
                {
                    innerAOrSpan.Attributes.Add("title", item.Text.AttributeEncode());
                    innerAOrSpan.Attributes.Add("aria-label", item.Text.AttributeEncode());
                }
                else
                {
                    if (item.Type == PagerItemType.Page)
                    {
                        innerAOrSpan.Attributes.Add("aria-label", $"Page {item.Text}");
                    }
                    else if (item.Type == PagerItemType.Gap)
                    {
                        innerAOrSpan.Attributes.Add("aria-label", $"Page {item.Index}");
                    }
                }
            }

            innerAOrSpan.AddCssClass("page-link");
            itemLi.InnerHtml.AppendHtml(GetItemInnerContent(item, innerAOrSpan));
            itemsUl.InnerHtml.AppendHtml(itemLi);
        }

        protected virtual TagBuilder GetItemInnerContent(PagerItem item, TagBuilder inner)
        {
            // TODO: add icon
            var iconI = new TagBuilder("span");
            iconI.Attributes.Add("aria-hidden", "true");

            switch (item.Type)
            {
                case PagerItemType.PreviousPage:
                    iconI.InnerHtml.Append("<");
                    break;
                case PagerItemType.NextPage:
                    iconI.InnerHtml.Append(">");
                    break;
                default:
                    inner.InnerHtml.AppendHtml(item.Text);
                    break;
            }

            if (item.IsNavButton)
            {
                inner.InnerHtml.AppendHtml(iconI);
            }

            return inner;
        }

        protected virtual string GenerateUrl(int pageNumber)
        {
            var request = ViewContext.HttpContext.Request;
            var path = request.PathBase + request.Path;
            var queryPart = request.Query.Merge($"?{QueryParamName}={pageNumber}");
            var url = path.Add(queryPart);

            return url;
        }
    }
}
