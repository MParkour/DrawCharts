using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.AspNetCore.Http;
using System;

namespace DrawCharts.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUser _userContext;
        private readonly ILogin _loginContext;
        private readonly IFaildLogin _faildLoginContext;

        public LoginController(IUser userContext, ILogin login, IFaildLogin faildLogin)
        {
            _userContext = userContext;
            _loginContext = login;
            _faildLoginContext = faildLogin;
        }

        [HttpGet("[action]")]
        public IActionResult signIn(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return new JsonResult(new { Message = "نام کاربری یا کلمه عبور نمی تواند خالی باشد", context = "error", page = "/" });
            }
            Utility.Message message = _userContext.CheckUserPass(UserName, Password);
            //نام کاربری یا کلمه عبور نادرست است
            if (message.Equals(Utility.Message.WrongUserPass))
            {
                //faildLogs ذخیره در
                Faild_Log faild = new Faild_Log()
                {
                    DateTime_In = DateTime.Now.ToString(),
                    UserName = UserName,
                    Password = Password
                };
                _faildLoginContext.Add(faild);
                return new JsonResult(new { Message = Utility.GetMessage(message), Context = "error", Page = "/" });
            }

            //احراز هویت صحیح می باشد
            else if (message.Equals(Utility.Message.Success))
            {
                //loginLogs ذخیره در
                Login_Log login = new Login_Log()
                {
                    UserID = _userContext.FindByUserName(UserName).UserID,
                    DateIn = DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Year.ToString("00"),
                    TimeIn = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " " + (DateTime.Now.ToString().ToLower().Contains("am") ? "AM" : "PM")
                };
                _loginContext.Add(login);
                string Token = Utility.CreateToken(50);
                // HttpContext.Session.SetString(Token, _userContext.FindByUserName(UserName).UserID.ToString());

                Utility.UserType userType = _userContext.GetUserType(UserName, Password);
                if (userType.Equals(Utility.UserType.Admin))
                {
                    return new JsonResult(new { Page = "/AdminPage", token = Token });
                }
                else if (userType.Equals(Utility.UserType.User))
                {
                    return new JsonResult(new { Page = "/UserPage", token = Token, userID = _userContext.FindByUserName(UserName).UserID.ToString() });
                }
                else
                    return new JsonResult(new { Message = "خطای ناشناس", Context = "Warning", Page = "/" });
            }
            else
                return new JsonResult(new { Message = "خطای ناشناس", Contex = "Warning", Page = "/" });
        }

        [HttpGet("[action]")]
        public bool CheckToken(string token)
        {
            try
            {
                var data = HttpContext.Session.GetString(token);
                int userID = -1;
                if (data == null)
                    return false;
                userID = int.Parse(HttpContext.Session.GetString(token));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

//هنگامیکه اهراز هویت صحیح بود
//باید برای کاربر یک توکن ایجاد کنیم و توکن را برای کاربر ارسال کنیم
//توکن ارسال شده در تکست باکس مخفی ذخیره می شود
//هنگامیکه کاکپوننتی درخواست شد باید توکن را به سرور بفرستیم
//سرور توکن ارسالی را با توکن خودش مقایسه می کند
//اگر درست بود که هیچ اجازه ورود دارد
//اگر نه کاربر باید به صفحه لاگین هدایت شود