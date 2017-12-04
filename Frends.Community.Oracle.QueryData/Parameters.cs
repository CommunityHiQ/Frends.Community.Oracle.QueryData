using Frends.Tasks.Attributes;
using System;
using System.ComponentModel;

namespace Frends.Community.Oracle.QueryData
{
    /// <summary>
    /// Input class for QueryData component
    /// </summary>
    public class Input
    {
        /// <summary>
        /// The connection string to the Oracle server
        /// </summary>
        [DefaultValue("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;")]
        [DefaultDisplayType(DisplayType.Text)]
        public String ConnectionString { get; set; }

        /// <summary>
        /// The query to perform
        /// </summary>
        [DefaultValue("SELECT NameColumn FROM TestTable")]
        [DefaultDisplayType(DisplayType.Text)]
        public String Query { get; set; }

        /// <summary>
        /// The name of the root element of the resultset
        /// </summary>
        [DefaultValue("ROWSET")]
        [DefaultDisplayType(DisplayType.Text)]
        public String RootElementName { get; set; }

        /// <summary>
        /// The name of the row element name of the resultset
        /// </summary>
        [DefaultValue("ROW")]
        [DefaultDisplayType(DisplayType.Text)]
        public String RowElementName { get; set; }

        /// <summary>
        /// The maximum amount of rows to return; defaults to -1 eg. no limit
        /// </summary>
        [DefaultValue(-1)]
        public Int32 MaxmimumRows { get; set; }

        /// <summary>
        /// The timeout value in seconds
        /// </summary>
        [DefaultValue(30)]
        public Int32 TimeoutSeconds { get; set; }

        /// <summary>
        /// Parameters for the database query
        /// </summary>
        public OracleParameter[] Parameters { get; set; }

        /// <summary>
        /// In what format to return the results of the query
        /// </summary>
        [DefaultValue(OracleQueryReturnType.XDocument)]
        public OracleQueryReturnType ReturnType { get; set; }
    }

    #region ParameterClassesAndEnums
    /// <summary>
    /// Class representing an Oracle query parameter
    /// </summary>
    public class OracleParameter
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        [DefaultValue("ParameterName")]
        [DefaultDisplayType(DisplayType.Text)]
        public String Name { get; set; }

        /// <summary>
        /// The value of the parameter
        /// </summary>
        [DefaultValue("Parameter value")]
        [DefaultDisplayType(DisplayType.Text)]
        public dynamic Value { get; set; }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        [DefaultValue(ParameterDataType.NVarchar2)]
        public ParameterDataType DataType { get; set; }

        /// <summary>
        /// Enumerator representing oracle parameter data types
        /// </summary>
        public enum ParameterDataType
        {
            BFile = 101,
            Blob = 102,
            Byte = 103,
            Char = 104,
            Clob = 105,
            Date = 106,
            Decimal = 107,
            Double = 108,
            Long = 109,
            LongRaw = 110,
            Int16 = 111,
            Int32 = 112,
            Int64 = 113,
            IntervalDS = 114,
            IntervalYM = 115,
            NClob = 116,
            NChar = 117,
            NVarchar2 = 119,
            Raw = 120,
            RefCursor = 121,
            Single = 122,
            TimeStamp = 123,
            TimeStampLTZ = 124,
            TimeStampTZ = 125,
            Varchar2 = 126,
            XmlType = 127,
            BinaryDouble = 132,
            BinaryFloat = 133
        }

    }

    /// <summary>
    /// Enumerator for return types
    /// </summary>
    public enum OracleQueryReturnType { XMLString, XMLDocument, XDocument, JSONString, Dynamic, JArray }
    #endregion
}
