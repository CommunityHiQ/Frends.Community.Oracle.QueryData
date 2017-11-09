using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Frends.Community.Oracle.QueryData;
using System.Threading;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Frends.Community.Oracle.QueryData
{
    public class QueryData
    {
        public async static Task<dynamic> PerformQuery(Parameters.Input Input, Parameters.Options Options, CancellationToken cancellationToken)
        {
            using (OracleConnection oracleConnection = new OracleConnection(Input.ConnectionString))
            {
                await oracleConnection.OpenAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                using (OracleCommand command = new OracleCommand(Input.Query, oracleConnection))
                {
                    command.CommandTimeout = Options.TimeoutSeconds;
                    command.XmlCommandType = OracleXmlCommandType.Query;
                    command.BindByName = true;

                    command.XmlQueryProperties.MaxRows = Options.MaxmimumRows;
                    command.XmlQueryProperties.RootTag = Options.RootElementName;
                    command.XmlQueryProperties.RowTag = Options.RowElementName;

                    if(Options.Parameters != null) command.Parameters.AddRange(Options.Parameters.Select(x => Methods.CreateOracleParam(x)).ToArray());

                    XmlReader reader = command.ExecuteXmlReader();
                    cancellationToken.ThrowIfCancellationRequested();

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.PreserveWhitespace = (Options.ReturnType != Parameters.OracleQueryReturnType.JArray);
                    xmlDocument.Load(reader);

                    if (!xmlDocument.HasChildNodes)
                    {
                        xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(String.Format("<{0}></{0}>", Options.RootElementName));
                    }

                    oracleConnection.Dispose();
                    oracleConnection.Close();
                    OracleConnection.ClearPool(oracleConnection);

                    switch (Options.ReturnType)
                    {
                        case Parameters.OracleQueryReturnType.XMLString:
                            return xmlDocument.OuterXml;
                        case Parameters.OracleQueryReturnType.XMLDocument:
                            return xmlDocument;
                        case Parameters.OracleQueryReturnType.JSONString:
                            return JsonConvert.SerializeXmlNode(xmlDocument);
                        case Parameters.OracleQueryReturnType.XDocument:
                            XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument);
                            nodeReader.MoveToContent();
                            return XDocument.Load(nodeReader);
                        case Parameters.OracleQueryReturnType.Dynamic:
                            dynamic root = new ExpandoObject();
                            XDocument outputDoc;
                            outputDoc = XDocument.Parse(xmlDocument.OuterXml, LoadOptions.PreserveWhitespace);
                            Methods.ParseToDynamic(root, outputDoc.Elements().First());
                            return root;
                        case Parameters.OracleQueryReturnType.JArray:
                            root = JToken.Parse(JsonConvert.SerializeXmlNode(xmlDocument))[command.XmlQueryProperties.RootTag];
                            if (root == null)
                                return new JArray();
                            return root[Options.RowElementName] is JArray ? (JArray)root[Options.RowElementName] : new JArray(root[Options.RowElementName]);
                        default:
                            return null;
                    }
                }
            }
        }
    }
}
