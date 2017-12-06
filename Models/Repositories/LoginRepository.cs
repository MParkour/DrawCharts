using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Models.Utility;

namespace Models
{
    public class LoginRepository : ILogin
    {
        private readonly DrawChartsContext _context;
        public LoginRepository(DrawChartsContext context)
        {
            this._context = context;
        }

        public Message Add(Login_Log item)
        {
            try
            {
                if (CheckUserID(item.UserID))
                {
                    _context.Login_Logs.Add(item);
                    _context.SaveChanges();
                    return Message.Success;
                }
                return Message.UserNotFound;
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

        public Login_Log Find(int id)
        {
            try
            {
                return _context.Login_Logs.Find(id);
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

        public IQueryable<Login_Log> GetAll()
        {
            try
            {
                return _context.Login_Logs.AsQueryable();
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
                var loginLog = _context.Login_Logs.Find(id);
                if (loginLog != null)
                {
                    _context.Entry(loginLog).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Message.Success;
                }
                else
                    return Message.LogNotFound;
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

        public Message Remove(Login_Log item)
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

        public Message Update(Login_Log item)
        {
            try
            {
                _context.Login_Logs.Attach(item);
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
    }
}