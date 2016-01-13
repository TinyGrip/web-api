using OutdoorSolution.Services.Common;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Manages TinyGrip operations with file system
    /// </summary>
    public class FileSystemService : IFileSystemService
    {
        /// <summary>
        /// Creates directory stucture to save image, based on current date
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        private string CreateImageDirectoryStructure(out string relativePath)
        {
            string imagePath = Path.Combine(
                ConfigurationManager.AppSettings["ImagesRoot"], 
                ConfigurationManager.AppSettings["ImagesPathToSave"]
            );

            var currentTime = DateTime.Now;
            relativePath = Path.Combine(currentTime.Year.ToString(), currentTime.Month.ToString());
            imagePath = Path.Combine(imagePath, relativePath);

            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            return imagePath;
        }

        /// <summary>
        /// Deletes image, stored on disk.
        /// If image's path is a web address - no action performed
        /// </summary>
        /// <param name="relPath">Relative path of image from image root folder</param>
        public void DeleteImage(string relPath)
        {
            if (String.IsNullOrWhiteSpace(relPath))
                return;

            // in case path is web address
            if (Utils.IsHttpUrl(relPath))
                return;

            File.Delete(
                Path.Combine(
                    ConfigurationManager.AppSettings["ImagesRoot"], 
                    ConfigurationManager.AppSettings["ImagesPathToSave"], 
                    relPath
                )
            );
        }

        /// <summary>
        /// Saves image stream to file
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="extension"></param>
        /// <returns>Relative path file, from images root folder</returns>
        public string SaveImageStreamToFile(Stream imageStream, string extension)
        {
            string relativePath = null;
            var imageDir = CreateImageDirectoryStructure(out relativePath);
            var imageName = GenerateFileName(imageDir, extension);

            var imagePath = Path.Combine(imageDir, imageName);

            using (FileStream output = new FileStream(imagePath, FileMode.Create))
            {
                imageStream.CopyTo(output);
            }

            return Path.Combine(relativePath, imageName);
        }

        /// <summary>
        /// Generates unique file name in the directory, based on GUID
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string GenerateFileName(string directory, string extension)
        {
            bool fileNameExist;
            string fileName;
            do
            {
                fileName = Guid.NewGuid() + extension;
                fileNameExist = Directory.GetFiles(directory, fileName, SearchOption.TopDirectoryOnly).Count() > 0;
            }
            while (fileNameExist);

            return fileName;
        }
    }
}
