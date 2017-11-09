using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.Community.Oracle.QueryData;
using System.Threading;

namespace Frends.Community.Oracle.Tests
{
    [TestClass]
    public class UnitTest1
    {
        string connectionString = "Data Source=localhost;User Id=<userid>;Password=<password>;Persist Security Info=True;";

        [TestMethod]
        public void QueryOracle()
        {
            // No way to automate this test without an Oracle instance. So it's just commented out.

            //string query = "SELECT * FROM XPNIS_SYSTEM_SYSDESCR;";
            
            //Parameters.Options Options = new Parameters.Options();
            //Parameters.Input Inputs = new Parameters.Input();

            //Inputs.Query = query;
            //Inputs.ConnectionString = connectionString;

            //Options.MaxmimumRows = 100;
            //Options.RootElementName = "root";
            //Options.RowElementName = "row";

            //var result = QueryData.QueryData.ExecuteQuery(Inputs, Options, new CancellationToken());

            //Assert.AreEqual(System.Threading.Tasks.TaskStatus.RanToCompletion, result.Status);
        }
    }
}
