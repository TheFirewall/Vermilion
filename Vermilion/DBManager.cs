using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.SQLite;
using Chloe.MySql;
using Chloe.PostgreSQL;
using Chloe;
using System.Data;
using System.Data.Common;
using Chloe.Extensions;

namespace Vermilion
{
    public class DBManager
    {
        private DbContext dbContext { get; set; }

        public DbContext GetDB()
        {
            return dbContext;
        }

        public void SetDB(DbContext value)
        {
            dbContext = value;
        }

        public void Insert(string query, bool task)
        {
            if (task)
            {
                Task.Run(() =>
                {
                    dbContext.Session.ExecuteNonQuery(@query);
                });
            }
            else
            {
                dbContext.Session.ExecuteNonQuery(@query);
            }
        }

        public void Update(string query, bool task)
        {
            if (task)
            {
                Task.Run(() =>
                {
                    dbContext.Session.ExecuteNonQuery(@query);
                });
            }
            else
            {
                dbContext.Session.ExecuteNonQuery(@query);
            }
        }

        public DataTable ExecuteDataTable(string query)
        {
            return DbSessionExtension.ExecuteDataTable(dbContext.Session, query);
        }

        public Dictionary<string, string> Query(string sql)
        {
            DataTable dt = ExecuteDataTable(sql);
            if (dt != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>(dt.Columns.Count);
                foreach (DataColumn c in dt.Columns)
                {
                    dic.Add(c.ColumnName, "null");
                    dic[c.ColumnName] = dt.Rows[0].Field<Object>(c.ColumnName).ToString();
                }
                dt.Dispose();
                return dic.Count > 0 ? dic : null;
            }
            return null;
        }
    }
}
