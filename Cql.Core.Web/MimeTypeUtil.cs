using System;
using System.IO;

using Cql.Core.Common.Extensions;

namespace Cql.Core.Web
{
    public static class MimeTypeUtil
    {
        public const string DefaultContentType = "application/octet-stream";

        public static string GetContentType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return DefaultContentType;
            }

            var ext = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(ext))
            {
                return DefaultContentType;
            }

            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".mp4":
                    return "video/mp4";
                case ".csv":
                    return "text/csv";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".docm":
                    return "application/vnd.ms-word.document.macroEnabled.12";
                case ".dotx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case ".dotm":
                    return "application/vnd.ms-word.template.macroEnabled.12";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xlsm":
                    return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case ".xltx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case ".xltm":
                    return "application/vnd.ms-excel.template.macroEnabled.12";
                case ".xlsb":
                    return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case ".xlam":
                    return "application/vnd.ms-excel.addin.macroEnabled.12";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".pptm":
                    return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".ppsx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".ppsm":
                    return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                case ".potx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case ".potm":
                    return "application/vnd.ms-powerpoint.template.macroEnabled.12";
                case ".ppam":
                    return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case ".sldx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slide";
                case ".sldm":
                    return "application/vnd.ms-powerpoint.slide.macroEnabled.12";
                case ".one":
                case ".onetoc2":
                case ".onetmp":
                case ".onepkg":
                    return "application/msonenote";
                case ".thmx":
                    return "application/vnd.ms-officetheme";
                default:
                    return DefaultContentType;
            }
        }

        public static bool IsImage(string fileName)
        {
            var contentType = GetContentType(fileName);

            return contentType.Contains("image", StringComparison.OrdinalIgnoreCase);
        }
    }
}
