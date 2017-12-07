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
                return await Methods.PerformOracleQuery(Input, options);
            }catch(Exception ex)
            {
                if (options.ThrowErrorOnFailure)
                    throw ex;
                return new Output { Success = false, Message = ex.Message };
            }
        }
    }
}
