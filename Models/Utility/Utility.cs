using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using static Models.Utility.Message;
namespace Models
{
    public static class Utility
    {
        public enum UserStatus
        {
            Enable = 1,
            Disable = 0
        }

        public enum UserType
        {
            unKnow = -1,
            Admin = 1,
            User = 0
        }

        public enum Message
        {
            Success,
            DuplicateUserName,
            UserNotFound,
            ErrorInRemove,
            TemplateNotFound,
            LogNotFound,
            DbNotEmpty,
            WrongUserPass,
            DuplicateTemplateName,
            UnsuccessfulOperation,
            unKnow,
            Morethan6characters,
            Specialcharacters,
            Weak,
            Medium,
            Strong,
            VeryStrong
        }

        public static string GetMessage(Message message)
        {
            string Message = "";
            switch (message)
            {
                case Success: Message = "عملیات با موفقیت انجام شد"; break;
                case DuplicateUserName: Message = "نام کاربری تکراری است"; break;
                case UserNotFound: Message = "اطلاعاتی برای این کاربر یافت نشد"; break;
                case ErrorInRemove: Message = "خطا در حذف اطلاعات"; break;
                case TemplateNotFound: Message = "اطلاعاتی برای این الگو یافت نشد"; break;
                case LogNotFound: Message = "اطلاعاتی برای این لاگ یافت نشد"; break;
                case WrongUserPass: Message = "اطلاعات وارد شده صحیح نمی باشد"; break;
                case DuplicateTemplateName: Message = "نام الگو تکراری است"; break;
                case UnsuccessfulOperation: Message = "عملیات ناموفق بود"; break;
                case unKnow: Message = "خطای ناشناس"; break;
                case Morethan6characters: Message = "کلمه عبور باید بیش از 6 کاراکتر باشد"; break;
                case Specialcharacters: Message = "کلمه عبور باید شامل حرف بزرگ، حرف کوچک ،عدد و کاراکتر خاص باشد"; break;

                default:
                    Message = "خطای ناشناس"; break;
            }
            return Message;
        }

        public static string CreateToken(int length)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static Message ValidatePass(string Item)
        {
            if (String.IsNullOrEmpty(Item) || Item.Length < 6)
            {
                return Message.Morethan6characters;
            }

            var regex = new List<string>();
            regex.Add("[A-Z]");
            regex.Add("[a-z]");
            regex.Add("[0-9]");
            regex.Add("[$@$!%*#?&]");

            var passed = 0;

            for (var i = 0; i < regex.Count; i++)
                if (Regex.IsMatch(Item, regex[i]))
                    passed++;

            return (passed == 4 ? Message.Success : Message.Specialcharacters);
        }
    }
}

// color = "red";
// color = "darkorange";
// color = "green";
// color = "darkgreen";