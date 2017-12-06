using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DrawCharts.Controllers
{
    [Route("api/[controller]")]
    public class TemplateController : Controller
    {
        private readonly ITemplate _templateContext;
        public TemplateController(ITemplate templateContext)
        {
            _templateContext = templateContext;
        }

        [HttpGet("[action]")]
        public IActionResult CheckConnection(string data)
        {
            string[] Params = data.Split(',');
            string dbType = Params[0].Trim(), IP = Params[1].Trim(), userName = Params[2].Trim(), passWord = Params[3].Trim(), dbName = Params[4].Trim(), fileType = Params[5].Trim();
            string ConnectionString = "";
            if (dbType.ToLower() == "sqlserver")
            {
                if (userName.ToLower() != "" && passWord.ToLower() != "")
                    ConnectionString = $"Server={IP};Database={dbName};User={userName};Password={passWord};";
                else
                    ConnectionString = $"Server={IP};Database={dbName};";
            }
            else
            {
                ConnectionString = $"Data Source=\\{IP}\\{dbName}{fileType}";
                ConnectionString = ConnectionString.Replace("\\", "\\\\");
            }
            List<string> TableList = new List<string>();

            if (!Connect(ConnectionString, TableList, dbType.ToLower(), dbName) || TableList.Count == 0)
            {
                return new JsonResult(new { message = "برقراری اتصال امکان پذیر نمی باشد", context = "error", tableList = new { } });
            }
            return new JsonResult(new { message = "اتصال برقرار است", context = "success", tableList = TableList });
        }

        public bool Connect(string source, List<string> tableList, string dbType, string dbName)
        {
            if (dbType == "sqlite")
                using (SqliteConnection con = new SqliteConnection(source))
                {
                    try
                    {
                        con.Open();
                        string CommandText = "SELECT name FROM sqlite_master where type='table' and name<>'__EFMigrationsHistory' and name<> 'sqlite_sequence'";
                        using (SqliteCommand cmd = new SqliteCommand(CommandText, con))
                        {
                            using (SqliteDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                    tableList.Add(dr[0].ToString());
                            }
                        }
                        con.Close();
                        return true;
                    }
                    catch (DbUpdateException)
                    {
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            else
                using (SqlConnection con = new SqlConnection(source))
                {
                    try
                    {
                        con.Open();
                        string CommandText = $"SELECT TABLE_NAME FROM {dbName}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME <> 'sysdiagrams'";
                        using (SqlCommand cmd = new SqlCommand(CommandText, con))
                        {
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                    tableList.Add(dr[0].ToString());
                            }
                        }
                        con.Close();
                        return true;
                    }
                    catch (DbUpdateException)
                    {
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
        }

        [HttpPost("[action]")]
        public IActionResult RegisterTemplate([FromBody] Template template)
        {
            Utility.Message message = _templateContext.Add(template);
            string context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { Message = Utility.GetMessage(message), context = context });
        }

        [HttpGet("[action]")]
        public IActionResult SelectAll()
        {
            return new JsonResult(_templateContext.GetAll());
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteTemplate(int TemplateID)
        {
            Utility.Message message = _templateContext.Remove(TemplateID);
            string context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { Message = Utility.GetMessage(message), Context = context });
        }

        [HttpGet("[action]")]
        public IActionResult GetTableField(string data, string tableName)
        {
            string[] Params = data.Split(',');
            string dbType = Params[0].Trim(), IP = Params[1].Trim(), userName = Params[2].Trim(), passWord = Params[3].Trim(), dbName = Params[4].Trim(), fileType = Params[5].Trim();
            string ConnectionString = "";
            if (dbType.ToLower() == "sqlserver")
            {
                if (userName.ToLower() != "" && passWord.ToLower() != "")
                    ConnectionString = $"Server={IP};Database={dbName};User={userName};Password={passWord};";
                else
                    ConnectionString = $"Server={IP};Database={dbName};";
            }
            else
            {
                ConnectionString = $"Data Source=\\{IP}\\{dbName}{fileType}";
                ConnectionString = ConnectionString.Replace("\\", "\\\\");
            }

            List<string> FieldList = new List<string>();
            if (dbType.ToLower() == "sqlite")
                using (SqliteConnection con = new SqliteConnection(ConnectionString))
                {
                    con.Open();
                    string CommandText = "PRAGMA table_info(" + tableName + ")";
                    using (SqliteCommand cmd = new SqliteCommand(CommandText, con))
                    {
                        using (SqliteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                FieldList.Add(dr[1].ToString());
                        }
                    }
                    con.Close();
                }
            else
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string CommandText = $"SELECT COLUMN_NAME FROM {dbName}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{tableName}'";
                    using (SqlCommand cmd = new SqlCommand(CommandText, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                FieldList.Add(dr[0].ToString());
                        }
                    }
                    con.Close();
                }
            return new JsonResult(new { FieldList = FieldList });
        }

        public bool ConnectToSql(string source, List<string> tableList)
        {
            using (SqlConnection con = new SqlConnection(source))
            {
                try
                {
                    con.Open();
                    string CommandText = "select * from [dbo].[User]";
                    using (SqlCommand cmd = new SqlCommand(CommandText, con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                tableList.Add(dr[1].ToString());
                        }
                    }
                    con.Close();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}