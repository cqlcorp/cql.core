namespace Cql.Core.Owin
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;

    public static class UploadFileUtil
    {
        public static string GetTempFilepath(MultipartFileStreamProvider provider)
        {
            var appDataPath = AppUtil.GetAppDataPath();

            var fileData = provider.FileData.First();

            var localFileName = fileData.LocalFileName;

            var fileName = GetFileName(fileData);

            var outputPath = Path.Combine(appDataPath, $"{DateTime.Now:yyyyMMdd-HHmmss}-{fileName}");

            File.Move(localFileName, outputPath);

            return outputPath;
        }

        private static string GetFileName(MultipartFileData fileData)
        {
            var fileName = fileData.Headers.ContentDisposition.FileName;

            foreach (var c in Path.GetInvalidPathChars())
            {
                fileName = fileName.Replace(c.ToString(), string.Empty);
            }

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c.ToString(), string.Empty);
            }

            return fileName;
        }
    }
}
