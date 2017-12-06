using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace DrawCharts.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IUser_Temp _UserTempContext;
        public RoleController(IUser_Temp UserTempContext)
        {
            _UserTempContext = UserTempContext;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllUsers()
        {
            var userList = _UserTempContext.getUserID_UserName().Where(p => p.Value != "admin").Select(p => new { userName = p.Value, userID = p.Key });
            return new JsonResult(userList);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllTemplates()
        {
            var TempList = _UserTempContext.getTempID_TempName().Select(p => new { tempName = p.Value, tempID = p.Key });
            return new JsonResult(TempList);
        }

        [HttpPost("[action]")]
        public IActionResult Register(string listTempID, int userID)
        {
            Utility.Message message = _UserTempContext.RemoveAllTemplate(userID);
            string context = message == Utility.Message.Success ? "success" : "error";
            if (string.IsNullOrEmpty(listTempID))
            {
                return new JsonResult(new { message = Utility.GetMessage(message), context = context });
            }
            string[] roleID = listTempID.Split(',');
            foreach (var tempID in roleID)
            {
                if (tempID != "")
                {
                    User_Temp user_temp = new User_Temp()
                    {
                        UserID = userID,
                        TemplateID = int.Parse(tempID),
                        StartDate = "",
                        EndDate = ""
                    };
                    message = _UserTempContext.Add(user_temp);
                }
            }
            context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { message = Utility.GetMessage(message), context = context });
        }

        [HttpGet("[action]")]
        public IActionResult GetUserTemp(int userID)
        {
            var List = _UserTempContext.getTempID_TempName(userID).Select(p => new { tempID = p.Key, tempName = p.Value }).ToList();
            return new JsonResult(List);
        }
    }
}