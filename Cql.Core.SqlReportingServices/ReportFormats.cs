// ReSharper disable CheckNamespace

namespace Cql.Core.ReportingServices.ReportExecution
{
    public enum ReportFormats
    {
        [EnumValue("")]
        NotSet = 0,

        /// <summary>
        /// XML file with report data
        /// </summary>
        [EnumValue("XML")]
        Xml = 1,

        /// <summary>
        /// CSV (comma delimited)
        /// </summary>
        [EnumValue("CSV")]
        Csv = 2,

        /// <summary>
        /// Data Feed (rss)
        /// </summary>
        [EnumValue("ATOM")]
        Atom = 3,

        /// <summary>
        /// PDF
        /// </summary>
        [EnumValue("PDF")]
        Pdf = 4,

        /// <summary>
        /// Remote GDI+ file
        /// </summary>
        [EnumValue("RGDI")]
        Rgdi = 5,

        /// <summary>
        /// HTML 4.0
        /// </summary>
        [EnumValue("HTML4.0")]
        Html4_0 = 6,

        /// <summary>
        /// MHTML (web archive)
        /// </summary>
        [EnumValue("MHTML")]
        Mhtml = 7,

        /// <summary>
        /// Excel 2003 (.xls)
        /// </summary>
        [EnumValue("EXCEL")]
        Excel = 8,

        /// <summary>
        /// Excel (.xlsx)
        /// </summary>
        [EnumValue("EXCELOPENXML")]
        ExcelOpenXml = 9,

        /// <summary>
        /// RPL Renderer
        /// </summary>
        [EnumValue("RPL")]
        Rpl = 10,

        /// <summary>
        /// TIFF file
        /// </summary>
        [EnumValue("IMAGE")]
        Image = 11,

        /// <summary>
        /// Word 2003 (.doc)
        /// </summary>
        [EnumValue("WORD")]
        Word = 12,

        /// <summary>
        /// Word (.docx)
        /// </summary>
        [EnumValue("WORDOPENXML")]
        WordOpenXml = 13
    }
}
