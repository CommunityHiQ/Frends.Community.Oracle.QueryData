using Frends.Community.Oracle.QueryData;
using NUnit.Framework;

namespace Frends.Community.Oracle.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        string connectionString = "Data Source=localhost;User Id=<userid>;Password=<salasana>;Persist Security Info=True;";

        [Test]
        [Ignore("No way to automate this test without an Oracle instance")]
        public void QueryOracle()
        {
            string query = "SELECT * FROM XPNIS_SYSTEM_SYSDESCR;";

            var inputs = new Input();
            var options = new Options { ThrowErrorOnFailure = true };

            inputs.Query = query;
            inputs.ConnectionString = connectionString;

            inputs.MaxmimumRows = 100;
            inputs.RootElementName = "root";
            inputs.RowElementName = "row";

            var result = QueryData.QueryData.PerformQuery(inputs, options);

            Assert.AreEqual(System.Threading.Tasks.TaskStatus.RanToCompletion, result.Status);
        }
    }
}
