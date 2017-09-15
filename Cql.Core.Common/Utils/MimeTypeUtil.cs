// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="MimeTypeUtil.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.IO;

    using Cql.Core.Common.Extensions;

    using JetBrains.Annotations;

    /// <summary>
    /// Class MimeTypeUtil.
    /// </summary>
    public static class MimeTypeUtil
    {
        /// <summary>
        /// The default content type
        /// </summary>
        public const string DefaultContentType = "application/octet-stream";

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The content type value such as &quot;image/jpeg&quot;.</returns>
        [NotNull]
        public static string GetContentType([CanBeNull] string fileName)
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

        /// <summary>
        /// Gets the file extension for the specified <paramref name="contentType"/>.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The file extension for the specified content type if known, or an empty string</returns>
        [NotNull]
        public static string GetFileExtension([CanBeNull] string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return ".jpg";
                case "image/gif":
                    return ".gif";
                case "image/png":
                    return ".png";
                case "application/pdf":
                    return ".pdf";
                case "application/msword":
                    return ".doc";
                case "application/vnd.ms-powerpoint":
                    return ".ppt";
                case "application/vnd.ms-excel":
                    return ".xls";
                case "video/mp4":
                    return ".mp4";
                case "text/csv":
                    return ".csv";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "xlsx";
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Determines whether the specified content type is an image content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns><c>true</c> if the specified content type contains "image"; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The contentType cannot be null</exception>
        public static bool IsImage([NotNull] string contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            return contentType.Contains("image", StringComparison.OrdinalIgnoreCase);
        }
    }
}
