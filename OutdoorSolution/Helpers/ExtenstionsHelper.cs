using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutdoorSolution.Helpers
{
    public static class ExtenstionsHelper
    {
        private static readonly Dictionary<string, string> supportedContentTypes = new Dictionary<string, string>() { { "image/png", ".png" }, { "image/jpeg", ".jpg" }, { "image/pjpeg", ".jpg" } };

        public static bool IsImageTypeSupported(string mimeType)
        {
            return supportedContentTypes.ContainsKey(mimeType);
        }

        public static string GetImageExtension(string mimeType)
        {
            String ext = null;
            supportedContentTypes.TryGetValue(mimeType, out ext);
            return ext;
        }
    }
}