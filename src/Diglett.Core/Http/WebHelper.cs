using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Diglett.Core.Http
{
    public static class WebHelper
    {
        private static HtmlEncoder? _htmlEncoder;

        public static HtmlEncoder HtmlEncoder
        {
            get
            {
                if (_htmlEncoder != null)
                    return _htmlEncoder;

                return HtmlEncoder.Create(new TextEncoderSettings(UnicodeRanges.All));
            }
            set => _htmlEncoder = value;
        }
    }
}
