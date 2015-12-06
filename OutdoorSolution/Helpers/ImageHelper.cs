using OutdoorSolution.Services.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OutdoorSolution.Helpers
{
    public static class ImageHelper
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

        public static Uri GetImageUri(string path, Uri requestUri)
        {
            if (String.IsNullOrWhiteSpace(path))
                return null;

            String url;

            if (Utils.IsHttpUrl(path))
            {
                url = path;
            }
            else
            {
                // construct image path relatively to server
                var pathProcessed = path.Replace("\\", "/");
                url =  requestUri.Scheme + "://" + 
                       requestUri.Host + ":" + requestUri.Port + "/" +
                       HttpContext.Current.Request.ApplicationPath + "/" +
                       ConfigurationManager.AppSettings["ImagesPathToSave"] + "/" +
                       pathProcessed;
            }

            return new Uri(url);
        }
    }
}