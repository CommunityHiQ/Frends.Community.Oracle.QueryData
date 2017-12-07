using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Frends.Tasks.Attributes;

namespace Frends.Community.Oracle.QueryData
{
    public class QueryData
    {
        /// <summary>
        /// Task for performing queries in Oracle databases. See documentation at https://github.com/CommunityHiQ/Frends.Community.Oracle.QueryData
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Options"></param>
        /// <returns>Object { bool Success, string Message, dynamic Result }</returns>
        public async static Task<Output> PerformQuery([CustomDisplay(DisplayOption.Tab)]Input Input, [CustomDisplay(DisplayOption.Tab)]Options options)
        {
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(Input.ConnectionString))
                {
                    await oracleConnection.OpenAsync();

                    using (OracleCommand command = new OracleCommand(Input.Query, oracleConnection))
                    {
                        command.CommandTimeout = Input.TimeoutSeconds;
                        command.XmlCommandType = OracleXmlCommandType.Query;
                        command.BindByName = true;

                        command.XmlQueryProperties.MaxRows = Input.MaxmimumRows;
                        command.XmlQueryProperties.RootTag = Input.RootElementName;
                        command.XmlQueryProperties.RowTag = Input.RowElementName;

                        if (Input.Parameters != null) command.Parameters.AddRange(Input.Parameters.Select(x => Methods.CreateOracleParam(x)).ToArray());

                        XmlReader reader = command.ExecuteXmlReader();

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.PreserveWhitespace = (Input.ReturnType != OracleQueryReturnType.JArray);
                        xmlDocument.Load(reader);

                        if (!xmlDocument.HasChildNodes)
                        {
                            xmlDocument = new XmlDocument();
                            xmlDocument.LoadXml(String.Format("<{0}></{0}>", Input.RootElementName));
                        }

                        oracleConnection.Dispose();
                        oracleConnection.Close();
                        OracleConnection.ClearPool(oracleConnection);

                        dynamic result;
                        switch (Input.ReturnType)
                        {
                            case OracleQueryReturnType.XMLString:
                                result = xmlDocument.OuterXml;
                                break;
                            case OracleQueryReturnType.XMLDocument:
                                result = xmlDocument;
                                break;
                            case OracleQueryReturnType.JSONString:
                                result = JsonConvert.SerializeXmlNode(xmlDocument);
                                break;
                            case OracleQueryReturnType.XDocument:
                                XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument);
                                nodeReader.MoveToContent();
                                result = XDocument.Load(nodeReader);
                                break;
                            case OracleQueryReturnType.Dynamic:
                                dynamic root = new ExpandoObject();
                                XDocument outputDoc;
                                outputDoc = XDocument.Parse(xmlDocument.OuterXml, LoadOptions.PreserveWhitespace);
                                Methods.ParseToDynamic(root, outputDoc.Elements().First());
                                result = root;
                                break;
                            case OracleQueryReturnType.JArray:
                                root = JToken.Parse(JsonConvert.SerializeXmlNode(xmlDocument))[command.XmlQueryProperties.RootTag];
                                if (root == null)
                                    result = new JArray();
                                result = root[Input.RowElementName] is JArray ? (JArray)root[Input.RowElementName] : new JArray(root[Input.RowElementName]);
                                break;
                            default:
                                result = null;
                                break;
                        }

                        return new Output { Success = true, Result = result };
                    }
                }
            }catch(Exception ex)
            {
                if (options.ThrowErrorOnFailure)
                    throw ex;
                return new Output { Success = false, Message = ex.Message };
            }
        }
    }
}
