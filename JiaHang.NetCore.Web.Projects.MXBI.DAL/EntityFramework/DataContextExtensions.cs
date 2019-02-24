using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.NetCore.Web.Projects.MXBI.DAL.EntityFramework
{
    public static class DataContextExtensions
    {
        /// <summary>
        /// 异步执行带有参数的存储过程方法 获取信息集合以及返回空值处理
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public async static Task<T> ExecSpAsync<T>(this DataContext db, string sql, MySqlParameter[] sqlParams = null)
        {

            var connection = db.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand())
            {
                await db.Database.OpenConnectionAsync();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (sqlParams != null)
                    cmd.Parameters.AddRange(sqlParams);
                var dr = await cmd.ExecuteReaderAsync();
                //var columnSchema = dr.GetColumnSchema();
                DataTable dataTable = new DataTable();
                dataTable.Load(dr);
                dr.Close();
                dr.Dispose();
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dataTable)));
            }
        }

        //public async static Task<DataTable> Exec3SpAsync<T>(this DataContext db, string sql, MySqlParameter[] sqlParams)
        //{

        //    var connection = db.Database.GetDbConnection();
        //    using (var cmd = connection.CreateCommand())
        //    {
        //        await db.Database.OpenConnectionAsync();
        //        cmd.CommandText = sql;
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddRange(sqlParams);
        //        var dr = await cmd.ExecuteReaderAsync();
        //        //var columnSchema = dr.GetColumnSchema();
        //        DataTable dataTable = new DataTable();
        //        dataTable.Load(dr);
        //        dr.Close();
        //        dr.Dispose();
        //        return dataTable;
        //    }
        //}
    }
}
