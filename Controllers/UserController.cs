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
    public class UserController : Controller
    {
        private readonly IUser _userContext;
        public UserController(IUser userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("[action]")]
        public ActionResult SelectAll()
        {
            var list = _userContext.GetAll();
            return new JsonResult(list);
        }

        [HttpPost("[action]")]
        public ActionResult AddUser([FromBody] User user)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            var persianDate = pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");
            user.CreateDate = persianDate;

            Utility.Message message = Models.Utility.ValidatePass(user.UserPassword);
            if (message.Equals(Utility.Message.Success))
                message = _userContext.Add(user);
            string context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { Message = Utility.GetMessage(message), context = context });
        }

        [HttpPost("[action]")]
        public IActionResult UpdateUser([FromBody] User Puser, int userID, bool Pass)
        {
            User user = _userContext.Find(userID);
            if (user == null)
            {
                return NotFound();
            }
            if (!Pass)
            {
                user.UserName = Puser.UserName;
                user.Description = Puser.Description;
                user.StatusCode = Puser.StatusCode;
                user.UserType = Puser.UserType;
            }
            else
                user.UserPassword = Utility.GetHashString(Puser.UserPassword);

            Utility.Message message = _userContext.Update(user);
            string context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { Message = Utility.GetMessage(message), context = context });
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteUser(int UserCode)
        {
            Utility.Message message = _userContext.Remove(UserCode);
            string context = message == Utility.Message.Success ? "success" : "error";
            return new JsonResult(new { Message = Utility.GetMessage(message), Context = context });
        }
    }
}