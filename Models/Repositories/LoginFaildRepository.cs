using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Models.Utility;

namespace Models
{
    public class LoginFaildRepository : IFaildLogin
    {
        private readonly DrawChartsContext _context;

        public LoginFaildRepository(DrawChartsContext context)
        {
            this._context = context;
        }

        public Message Add(Faild_Log item)
        {
            try
            {
                _context.Faild_Logs.Add(item);
                _context.SaveChanges();
                return Message.Success;
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return Message.unKnow;
            }
        }

        public Faild_Log Find(int id)
        {
            try
            {
                return _context.Faild_Logs.Find(id);
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

        public IQueryable<Faild_Log> GetAll()
        {
            try
            {
                return _context.Faild_Logs.AsQueryable();
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

        public Message Remove(Faild_Log item)
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

        public Message Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Message Update(Faild_Log item)
        {
            throw new NotImplementedException();
        }
    }
}