using OutdoorSolution.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace OutdoorSolution.Providers
{
    public class MultipartImageStreamsProvider : MultipartMemoryStreamProvider
    {
        private int parsedImagesCount = 0;

        public MultipartImageStreamsProvider()
        {
            MaxImagesAmount = 1;
        }

        public int MaxImagesAmount { get; private set; }

        public override Stream GetStream(HttpContent parent, System.Net.Http.Headers.HttpContentHeaders headers)
        {
            // following line handles other form fields other than files.
            if (String.IsNullOrEmpty(headers.ContentDisposition.FileName)) return base.GetStream(parent, headers);
            
            // restrict what filetypes can be uploaded
            if (ExtenstionsHelper.IsImageTypeSupported(headers.ContentType.MediaType) && parsedImagesCount < MaxImagesAmount)
            {
                ++parsedImagesCount;
                return base.GetStream(parent, headers);
            }
            else
            {
                return Stream.Null;
            }
        }
    }
}