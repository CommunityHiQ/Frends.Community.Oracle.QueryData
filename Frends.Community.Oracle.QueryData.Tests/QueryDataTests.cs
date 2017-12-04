using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.Community.Oracle.QueryData;
using System.Threading;

namespace Frends.Community.Oracle.Tests
{
    [TestClass]
    public class UnitTest1
    {
        string connectionString = "Data Source=localhost;User Id=<userid>;Password=<salasana>;Persist Security Info=True;";

        [TestMethod]
        public void QueryOracle()
        {
            // No way to automate this test without an Oracle instance.So it's just commented out.

            //string query = "SELECT * FROM XPNIS_SYSTEM_SYSDESCR;";

            //Input Inputs = new Input();

            //Inputs.Query = query;
            //Inputs.ConnectionString = connectionString;

            //Inputs.MaxmimumRows = 100;
            //Inputs.RootElementName = "root";
            //Inputs.RowElementName = "row";

            //var result = QueryData.QueryData.PerformQuery(Inputs);

            //Assert.AreEqual(System.Threading.Tasks.TaskStatus.RanToCompletion, result.Status);
        }
    }
}
