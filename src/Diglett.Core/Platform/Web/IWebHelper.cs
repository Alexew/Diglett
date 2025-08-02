namespace Diglett.Core.Web
{
    public interface IWebHelper
    {
        string ModifyQueryString(string? url, string? queryModification, string? removeParamName = null, string? anchor = null);
    }
}
