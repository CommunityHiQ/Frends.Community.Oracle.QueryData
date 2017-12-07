using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Frends.Community.Oracle.QueryData
{
    public class QueryData
    {
        /// <summary>
        /// Task for performing queries in Oracle databases. See documentation at https://github.com/CommunityHiQ/Frends.Community.Oracle.QueryData
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public async static Task<dynamic> PerformQuery(Input Input)
        {
            using (OracleConnection oracleConnection = new OracleConnection(Input.ConnectionString))
            {
                await oracleConnection.OpenAsync();

                try
                {
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

                        switch (Input.ReturnType)
                        {
                            case OracleQueryReturnType.XMLString:
                                return xmlDocument.OuterXml;
                            case OracleQueryReturnType.XMLDocument:
                                return xmlDocument;
                            case OracleQueryReturnType.JSONString:
                                return JsonConvert.SerializeXmlNode(xmlDocument);
                            case OracleQueryReturnType.XDocument:
                                XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument);
                                nodeReader.MoveToContent();
                                return XDocument.Load(nodeReader);
                            case OracleQueryReturnType.Dynamic:
                                dynamic root = new ExpandoObject();
                                XDocument outputDoc;
                                outputDoc = XDocument.Parse(xmlDocument.OuterXml, LoadOptions.PreserveWhitespace);
                                Methods.ParseToDynamic(root, outputDoc.Elements().First());
                                return root;
                            case OracleQueryReturnType.JArray:
                                root = JToken.Parse(JsonConvert.SerializeXmlNode(xmlDocument))[command.XmlQueryProperties.RootTag];
                                if (root == null)
                                    return new JArray();
                                return root[Input.RowElementName] is JArray ? (JArray)root[Input.RowElementName] : new JArray(root[Input.RowElementName]);
                            default:
                                return null;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    oracleConnection.Dispose();
                    oracleConnection.Close();
                    OracleConnection.ClearPool(oracleConnection);
                }
            }
        }
    }
}
