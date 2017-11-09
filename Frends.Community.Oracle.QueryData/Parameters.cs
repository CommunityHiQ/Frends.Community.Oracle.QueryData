using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.Community.Oracle.QueryData
{
    public class Parameters
    {
        public class Input
        {
            /// <summary>
            /// The connection string to the Oracle server
            /// </summary>
            [PasswordPropertyText(true)]
            [DefaultValue("\"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;\"")]
            public String ConnectionString { get; set; }

            /// <summary>
            /// The query to perform
            /// </summary>
            [DefaultValue("@\"SELECT NameColumn FROM TestTable\"")]
            public String Query { get; set; }
        }

        public class Options
        {
            /// <summary>
            /// The name of the root element of the resultset
            /// </summary>
            [DefaultValue("\"ROWSET\"")]
            public String RootElementName { get; set; }

            /// <summary>
            /// The name of the row element name of the resultset
            /// </summary>
            [DefaultValue("\"ROW\"")]
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
            public OracleQueryReturnType ReturnType { get; set; }
        }

        #region ParameterClassesAndEnums
        public class OracleParameter
        {
            public String Name { get; set; }
            public dynamic Value { get; set; }

            public ParameterDataType DataType { get; set; }

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

        public enum OracleQueryReturnType { XMLString, XMLDocument, XDocument, JSONString, Dynamic, JArray }
        #endregion
    }
}
