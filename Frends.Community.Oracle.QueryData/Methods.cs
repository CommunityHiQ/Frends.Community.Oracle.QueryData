using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using OracleParam = Oracle.ManagedDataAccess.Client.OracleParameter;
using System.Data;
using System.Xml.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.Community.Oracle.QueryData
{
    public class Methods
    {
        public static OracleParam CreateOracleParam(OracleParameter parameter, ParameterDirection? direction = null)
        {
            var newParam = new OracleParam()
            {
                ParameterName = parameter.Name,
                Value = parameter.Value,
                OracleDbType = (OracleDbType)(int)parameter.DataType
            };

            if (direction.HasValue)
                newParam.Direction = direction.Value;

            return newParam;
        }

        public static void ParseToDynamic(dynamic parent, XElement node)
        {
            if (node.HasElements)
            {
                if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    var item = new ExpandoObject();
                    var list = new List<dynamic>();
                    foreach (var element in node.Elements())
                    {
                        ParseToDynamic(list, element);
                    }

                    AddProperty(item, node.Elements().First().Name.LocalName, list);
                    AddProperty(parent, node.Name.ToString(), item);
                }
                else
                {
                    var item = new ExpandoObject();
                    foreach (var attribute in node.Attributes())
                    {
                        AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                    }
                    foreach (var element in node.Elements())
                    {
                        ParseToDynamic(item, element);
                    }

                    AddProperty(parent, node.Name.ToString(), item);
                }
            }
            else
            {
                AddProperty(parent, node.Name.ToString(), node.Value.Trim());
            }
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else
            {
                (parent as IDictionary<String, object>)[name] = value;
            }
        }

        public async static Task<Output> PerformOracleQuery(Input Input, Options options)
        {
            using (OracleConnection oracleConnection = new OracleConnection(Input.ConnectionString))
            {
                try
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
                } catch(Exception e)
                {
                    throw e;
                }
                finally
                {
                    // Close connection
                    oracleConnection.Dispose();
                    oracleConnection.Close();
                    OracleConnection.ClearPool(oracleConnection);
                }
            }
        }
    }
}
