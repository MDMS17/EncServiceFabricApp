using Load837Meditrac.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Meditrac.Extensions
{
    public static class ContextExtension
    {
        public static int IntFromSQL(this MeditracContext context, string sql)
        {
            int count;
            using (var connection = context.Database.GetDbConnection())
            {
                connection.ConnectionString = Common.cn_Meditrac;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    string result = command.ExecuteScalar().ToString();

                    int.TryParse(result, out count);
                }
            }
            return count;
        }
    }
}
