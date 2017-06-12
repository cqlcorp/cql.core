// ReSharper disable CheckNamespace

namespace Cql.Core.ReportingServices.ReportExecution
{
    public enum ReportFormats
    {
        [EnumValue("")]
        NotSet,

        /// <summary>
        /// XML file with report data
        /// </summary>
        [EnumValue("XML")]
        Xml,

        /// <summary>
        /// CSV (comma delimited)
        /// </summary>
        [EnumValue("CSV")]
        Csv,

        /// <summary>
        /// Data Feed (rss)
        /// </summary>
        [EnumValue("ATOM")]
        Atom,

        /// <summary>
        /// PDF
        /// </summary>
        [EnumValue("PDF")]
        Pdf,

        /// <summary>
        /// Remote GDI+ file
        /// </summary>
        [EnumValue("RGDI")]
        Rgdi,

        /// <summary>
        /// HTML 4.0
        /// </summary>
        [EnumValue("HTML4.0")]
        Html4_0,

        /// <summary>
        /// MHTML (web archive)
        /// </summary>
        [EnumValue("MHTML")]
        Mhtml,

        /// <summary>
        /// Excel 2003 (.xls)
        /// </summary>
        [EnumValue("EXCEL")]
        Excel,

        /// <summary>
        /// Excel (.xlsx)
        /// </summary>
        [EnumValue("EXCELOPENXML")]
        ExcelOpenXml,

        /// <summary>
        /// RPL Renderer
        /// </summary>
        [EnumValue("RPL")]
        Rpl,

        /// <summary>
        /// TIFF file
        /// </summary>
        [EnumValue("IMAGE")]
        Image,

        /// <summary>
        /// Word 2003 (.doc)
        /// </summary>
        [EnumValue("WORD")]
        Word,

        /// <summary>
        /// Word (.docx)
        /// </summary>
        [EnumValue("WORDOPENXML")]
        WordOpenXml,
    }
}
