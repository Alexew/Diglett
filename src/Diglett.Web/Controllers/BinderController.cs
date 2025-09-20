using Diglett.Core;
using Diglett.Core.Catalog.Collection;
using Diglett.Core.Content.Media;
using Diglett.Core.Data;
using Diglett.Web.Models.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web.Controllers
{
    [Authorize]
    public class BinderController : DiglettController
    {
        private readonly DiglettDbContext _db;
        private readonly IWorkContext _workContext;
        private readonly IMediaService _mediaService;

        public BinderController(
            DiglettDbContext db,
            IWorkContext workContext,
            IMediaService mediaService)
        {
            _db = db;
            _workContext = workContext;
            _mediaService = mediaService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            var binders = await _db.Binders
                .AsNoTracking()
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .ToListAsync();

            var model = new BinderListModel
            {
                Binders = binders.Select(PrepareBinderModel).ToList()
            };

            return View(model);
        }

        public IActionResult CreateBinderModal()
        {
            return PartialView(PrepareBinderModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BinderModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(List));

            var binder = MapToBinderEntity(model);

            _db.Binders.Add(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> EditBinderModal(int id)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (binder == null)
                return NotFound();

            var model = PrepareBinderModel(binder);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BinderModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(List));

            var binder = await _db.Binders
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            if (binder == null)
                return NotFound();

            MapToBinderEntity(model, binder);

            _db.Binders.Update(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (binder == null)
                return NotFound();

            var model = PrepareBinderModel(binder);
            var page = await PrepareBinderPageModel(binder);

            model.Page = page;

            return View(model);
        }

        public async Task<IActionResult> DeleteBinderModal(int id)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (binder == null)
                return NotFound();

            var model = PrepareBinderModel(binder);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var binder = await _db.Binders
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (binder == null)
                return NotFound();

            _db.Binders.Remove(binder);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> LoadPage(int id, int page)
        {
            var binder = await _db.Binders
                .AsNoTracking()
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (binder == null)
                return NotFound();

            var model = await PrepareBinderPageModel(binder, page);

            return Json(new
            {
                html = await InvokePartialViewAsync("Partials/Binder.Page", model),
                page
            });
        }

        public async Task<IActionResult> AddCard(int binderId, int cardVariantId, int slot)
        {
            var binder = await _db.Binders
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == binderId);

            if (binder == null)
                return NotFound();

            if (slot < 1 || slot > binder.PageCount * binder.PocketSize)
                return BadRequest();

            var binderItem = await _db.BinderItems
                .ApplyBinderFilter(binder.Id)
                .ApplySlotFilter(slot)
                .SingleOrDefaultAsync();
            var isNew = binderItem == null;

            binderItem ??= new BinderItem
            {
                BinderId = binder.Id,
                Slot = slot
            };

            binderItem.CardVariantId = cardVariantId;

            if (isNew)
                _db.BinderItems.Add(binderItem);
            else
                _db.BinderItems.Update(binderItem);

            await _db.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> RemoveCard(int binderId, int slot)
        {
            var binder = await _db.Binders
                .ApplyUserFilter(_workContext.CurrentUser!.Id)
                .SingleOrDefaultAsync(x => x.Id == binderId);

            if (binder == null)
                return NotFound();

            if (slot < 1 || slot > binder.PageCount * binder.PocketSize)
                return BadRequest();

            var binderItem = await _db.BinderItems
                .ApplyBinderFilter(binder.Id)
                .ApplySlotFilter(slot)
                .SingleOrDefaultAsync();

            if (binderItem == null)
                return NotFound();

            _db.BinderItems.Remove(binderItem);
            await _db.SaveChangesAsync();

            return Ok();
        }

        private BinderModel PrepareBinderModel(Binder? binder = null)
        {
            var model = new BinderModel();

            if (binder != null)
            {
                model.Id = binder.Id;
                model.Name = binder.Name;
                model.PageCount = binder.PageCount;
            }

            return model;
        }

        private async Task<BinderPageModel> PrepareBinderPageModel(Binder binder, int page = 1)
        {
            Guard.NotNull(binder);

            var result = new BinderPageModel
            {
                Page = page,
                PocketSize = binder.PocketSize
            };

            var items = await _db.BinderItems
                .AsNoTracking()
                .IncludeCards()
                .ApplyBinderFilter(binder.Id)
                .ApplyPageFilter(page, binder.PocketSize)
                .OrderBy(x => x.Slot)
                .ToListAsync();

            foreach (var item in items)
            {
                var itemModel = new BinderItemModel
                {
                    Id = item.Id,
                    Slot = item.Slot
                };

                if (item.CardVariant?.Card != null)
                {
                    itemModel.CardVariantId = item.CardVariant.Id;
                    itemModel.Name = item.CardVariant.Card.Name;
                    itemModel.ImageUrl = _mediaService.GetImageUrl(item.CardVariant.Card);
                }

                result.Items[itemModel.Slot] = itemModel;
            }

            return result;
        }

        private Binder MapToBinderEntity(BinderModel model, Binder? entity = null)
        {
            Guard.NotNull(model);

            entity ??= new Binder
            {
                UserId = _workContext.CurrentUser!.Id
            };

            entity.Name = model.Name;
            entity.PageCount = model.PageCount;

            return entity;
        }
    }
}
