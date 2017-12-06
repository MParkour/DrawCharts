using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Models.Utility;

namespace Models
{
    public class User_TempRepository : IUser_Temp
    {
        private readonly DrawChartsContext _context;
        public User_TempRepository(DrawChartsContext context)
        {
            this._context = context;
        }

        public Message Add(User_Temp item)
        {
            try
            {
                if (CheckUserID(item.UserID))
                    if (CheckTemplateID(item.TemplateID))
                    {
                        //عملیات ثبت اطلاعات
                        _context.User_Temps.Add(item);
                        _context.SaveChanges();
                        return Message.Success;
                    }
                    else
                    {
                        //چنین الگویی به ثبت نرسیده است
                        return Message.TemplateNotFound;
                    }
                else
                {
                    //چنین کاربری وجود ندارد
                    return Message.UserNotFound;
                }
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return Message.unKnow;
            }
            catch (Exception)
            {
                return Message.unKnow;
            }
        }

        public bool CheckTemplateID(int TempID)
        {
            try
            {
                var data = _context.Templates.Find(TempID);
                if (data == null)
                    return false;
                return true;
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckUserID(int UserID)
        {
            try
            {
                var data = _context.Users.Find(UserID);
                if (data == null) return false;
                return true;
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User_Temp Find(int id)
        {
            try
            {
                return _context.User_Temps.Find(id);
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

        public IQueryable<User_Temp> GetAll()
        {
            try
            {
                return _context.User_Temps.AsQueryable();
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

        public Message Remove(int id)
        {
            try
            {
                var User_Temp = _context.User_Temps.Find(id);
                if (User_Temp != null)
                {
                    _context.Entry(User_Temp).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Message.Success;
                }
                else
                    return Message.UserNotFound;
            }
            catch (DbUpdateException)
            {
                return Message.ErrorInRemove;
            }
            catch (Exception)
            {
                return Message.ErrorInRemove;
            }
        }

        public Message Remove(User_Temp item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Deleted;
                _context.SaveChanges();
                return Message.Success;
            }
            catch (DbUpdateException)
            {
                return Message.ErrorInRemove;
            }
            catch (Exception)
            {
                return Message.ErrorInRemove;
            }
        }

        public Message Update(User_Temp item)
        {
            try
            {
                _context.User_Temps.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Message.Success;
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return Message.unKnow;
            }
            catch (Exception)
            {
                return Message.unKnow;
            }
        }

        public Dictionary<string, string> getTempID_TempName()
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                var List = _context.Templates.Select(p => new { p.TemplateID, p.Title }).ToList();
                foreach (var item in List)
                {
                    data.Add(item.TemplateID.ToString(), item.Title.ToString());
                }
                return data;
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

        public Dictionary<string, string> getTempID_TempName(int userID)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                var list = (from t in _context.Templates
                            join ut in _context.User_Temps on t.TemplateID equals ut.TemplateID
                            where ut.UserID.Equals(userID)
                            select new { tempID = t.TemplateID, tempName = t.Title });
                foreach (var item in list)
                {
                    data.Add(item.tempID.ToString(), item.tempName.ToString());
                }
                return data;
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

        public Dictionary<string, string> getUserID_UserName()
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                var List = _context.Users.Select(p => new { p.UserID, p.UserName }).ToList();
                foreach (var item in List)
                {
                    data.Add(item.UserID.ToString(), item.UserName.ToString());
                }
                return data;
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

        public Utility.Message RemoveAllTemplate(int userID)
        {
            try
            {
                var itemsToDelete = _context.User_Temps.Where(p => p.UserID == userID);
                _context.User_Temps.RemoveRange(itemsToDelete);
                _context.SaveChanges();
                return Utility.Message.Success;
            }
            catch (DbUpdateException)
            {
                return Utility.Message.unKnow;
            }
            catch (Exception)
            {
                return Utility.Message.unKnow;
            }
        }
    }
}