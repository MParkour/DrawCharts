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
    public class UserPageController : Controller
    {
        private readonly IUser_Temp _userTempContext;
        private readonly ITemplate _tempContext;
        public UserPageController(IUser_Temp userTemp, ITemplate template)
        {
            _userTempContext = userTemp;
            _tempContext = template;
        }

        [HttpGet("[action]")]
        public IActionResult GetUserRole(int userID)
        {
            try
            {
                var Data = _userTempContext.getTempID_TempName(userID).Select(p => new { tempID = p.Key, tempName = p.Value });
                return new JsonResult(Data);
            }
            catch (DbUpdateException)
            {
                return NoContent();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetTempData(int tempID)
        {
            try
            {
                var Data = _tempContext.Find(tempID);
                string ConnectionString = "";
                if (Data.dbType.ToLower() == "sqlserver")
                {
                    if (Data.UserName.ToLower() != "" && Data.Password.ToLower() != "")
                        ConnectionString = $"Server={Data.IP};Database={Data.dbName};User={Data.UserName};Password={Data.Password};";
                    else
                        ConnectionString = $"Server={Data.IP};Database={Data.dbName};";
                }
                else
                {
                    ConnectionString = string.Format("Data Source=\\{0}\\{1}", Data.IP, Data.dbName);
                    ConnectionString = ConnectionString.Replace("\\", "\\\\");
                }

                var list = GetData(ConnectionString, Data.TableName, Data.Field1, Data.Field2, Data.Calculation, "", Data.dbType);

                return Json(list);
            }
            catch (DbUpdateException)
            {
                return NoContent();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        public List<info> GetData(string source, string tblName, string Field1, string Field2, string Calculation, string CustomCondition, string dbType)
        {
            if (dbType.ToLower() == "sqlite")
                using (SqliteConnection con = new SqliteConnection(source))
                {
                    try
                    {
                        con.Open();
                        List<info> tableList = new List<info>();
                        if (CustomCondition.Length <= 6)
                            CustomCondition = "";
                        string CommandText = $"select {Field1}, {Calculation}({Field2}) from {tblName} {CustomCondition} group by {Field1}";
                        using (SqliteCommand cmd = new SqliteCommand(CommandText, con))
                        {
                            using (SqliteDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                    tableList.Add(new info { Field1 = dr[0].ToString(), Field2 = dr[1].ToString(), Calculation = Calculation, AxisX_Name = Field1, AxisY_Name = Field2 });
                            }
                        }
                        con.Close();
                        return tableList;
                    }
                    catch (DbUpdateException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            else
                using (SqlConnection con = new SqlConnection(source))
                {
                    try
                    {
                        con.Open();
                        List<info> tableList = new List<info>();
                        if (CustomCondition.Length <= 6)
                            CustomCondition = "";
                        string CommandText = $"select {Field1}, {Calculation}({Field2}) from [{tblName}] {CustomCondition} group by {Field1}";
                        using (SqlCommand cmd = new SqlCommand(CommandText, con))
                        {
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                    tableList.Add(new info { Field1 = dr[0].ToString(), Field2 = dr[1].ToString(), Calculation = Calculation, AxisX_Name = Field1, AxisY_Name = Field2 });
                            }
                        }
                        con.Close();
                        return tableList;
                    }
                    catch (DbUpdateException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
        }

        [HttpGet("[action]")]
        public IActionResult SetFilter(int tempID, bool Check1, bool Check2, bool Baze1, bool Baze2, string listFilter1, string listFilter2)
        {
            if (!Check1 && !Check2)
            {
                //در این قسمت فقط یک واکشی ساده داریم بدون فیلتر
                return GetTempData(tempID);
            }
            string CustomCondition = "where ";
            var Data = _tempContext.Find(tempID);
            string ConnectionString = "";
            if (Data.dbType.ToLower() == "sqlserver")
            {
                if (Data.UserName.ToLower() != "" && Data.Password.ToLower() != "")
                    ConnectionString = $"Server={Data.IP};Database={Data.dbName};User={Data.UserName};Password={Data.Password};";
                else
                    ConnectionString = $"Server={Data.IP};Database={Data.dbName};";
            }
            else
            {
                ConnectionString = string.Format("Data Source=\\{0}\\{1}", Data.IP, Data.dbName);
                ConnectionString = ConnectionString.Replace("\\", "\\\\");
            }

            List<string> AxisX = new List<string>();
            if (!string.IsNullOrEmpty(listFilter1))
                AxisX = listFilter1.Split(',').ToList();
            if (Check1 && AxisX.Count != 0)
            {
                if (Baze1)
                {
                    if (!string.IsNullOrEmpty(AxisX[0]) && !string.IsNullOrEmpty(AxisX[1]))
                        CustomCondition += $"({Data.Field1}>='{AxisX[0].Trim()}' and {Data.Field1}<='{AxisX[1].Trim()}')";
                    else if (!string.IsNullOrEmpty(AxisX[0]))
                        CustomCondition += $"({Data.Field1}>='{AxisX[0].Trim()}')";
                    else if (!string.IsNullOrEmpty(AxisX[1]))
                        CustomCondition += $"({Data.Field1}<='{AxisX[1].Trim()}')";
                }
                else
                {
                    CustomCondition += "(";
                    bool flag = false;
                    for (int i = 0; i < AxisX.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(AxisX[i]))
                        {
                            var Type = Data.dbType.ToLower() == "sqlserver" ? "N" : "";
                            CustomCondition += $"{Data.Field1}={Type}'{AxisX[i].Trim()}' or ";
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        CustomCondition = CustomCondition.Substring(0, CustomCondition.Length - 1);
                    }
                    //where ==6 char
                    if (CustomCondition.Length > 6)
                        CustomCondition = CustomCondition.Substring(0, CustomCondition.Length - 4);
                    if (flag)
                        CustomCondition += ")";
                }
            }

            List<string> AxisY = new List<string>();
            if (!string.IsNullOrEmpty(listFilter2))
                AxisY = listFilter2.Split(',').ToList();
            if (Check2 && AxisY.Count != 0)
            {
                if (CustomCondition.Length > 6)
                    CustomCondition += " and ";
                if (Baze2)
                {
                    if (!string.IsNullOrEmpty(AxisY[0]) && !string.IsNullOrEmpty(AxisY[1]))
                        CustomCondition += $"({Data.Field2}>='{AxisY[0].Trim()}' and {Data.Field2}<='{AxisY[1].Trim()}')";
                    else if (!string.IsNullOrEmpty(AxisY[0]))
                        CustomCondition += $"({Data.Field2}>='{AxisY[0].Trim()}')";
                    else if (!string.IsNullOrEmpty(AxisY[1]))
                        CustomCondition += $"({Data.Field2}<='{AxisY[1].Trim()}')";
                }
                else
                {
                    CustomCondition += "(";
                    bool flag = false;
                    for (int i = 0; i < AxisY.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(AxisY[i]))
                        {
                            var Type = Data.dbType.ToLower() == "sqlserver" ? "N" : "";
                            CustomCondition += $"{Data.Field2}={Type}'{AxisY[i].Trim()}' or ";
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        CustomCondition = CustomCondition.Substring(0, CustomCondition.Length - 1);
                    }
                    //where ==6 char
                    if (CustomCondition.Length > 6)
                        CustomCondition = CustomCondition.Substring(0, CustomCondition.Length - 4);
                    if (flag)
                        CustomCondition += ")";
                }
            }
            if (CustomCondition.EndsWith("and "))
            {
                CustomCondition = CustomCondition.Substring(0, CustomCondition.Length - 5);
            }
            var list = GetData(ConnectionString, Data.TableName, Data.Field1, Data.Field2, Data.Calculation, CustomCondition, Data.dbType);
            return Json(list);

        }
    }

    public class info
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Calculation { get; set; }
        public string AxisX_Name { get; set; }
        public string AxisY_Name { get; set; }
    }
}