using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static Models.Utility;

namespace Models
{
    public class TemplateRepository : ITemplate
    {
        private readonly DrawChartsContext _context;
        public TemplateRepository(DrawChartsContext context)
        {
            this._context = context;
        }

        public Message Add(Template item)
        {
            try
            {
                _context.Templates.Add(item);
                _context.SaveChanges();
                return Message.Success;
            }
            catch (DbUpdateException ex)
            {
                SqliteException sqliteEX = ex.GetBaseException() as SqliteException;
                Message message;
                switch (sqliteEX.SqliteErrorCode)
                {
                    case 19: message = Message.DuplicateTemplateName; break;
                    default: message = Message.unKnow; break;
                }
                return message;
            }
             catch (Exception)
            {
                return Message.unKnow;
            }
        }

        public Template Find(int id)
        {
            try
            {
                return _context.Templates.Find(id);
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

        public IQueryable<Template> GetAll()
        {
            try
            {
                return _context.Templates.AsQueryable();
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
                var Template = _context.Templates.Find(id);
                if (Template != null)
                {
                    _context.Entry(Template).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Message.Success;
                }
                else
                    return Message.TemplateNotFound;
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

        public Message Remove(Template item)
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

        public Message Update(Template item)
        {
            try
            {
                _context.Templates.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Message.Success;
            }
            catch (DbUpdateException ex)
            {
                SqliteException sqliteEX = ex.GetBaseException() as SqliteException;
                Message message;
                switch (sqliteEX.SqliteErrorCode)
                {
                    case 19: message = Message.DuplicateUserName; break;
                    default: message = Message.unKnow; break;
                }
                return message;
            }
            catch (Exception)
            {
                return Message.unKnow;
            }
        }
    }
}