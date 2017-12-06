using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using static Models.Utility;

namespace Models
{
    public class UserRepository : IUser
    {
        private readonly DrawChartsContext _context;
        public UserRepository(DrawChartsContext context)
        {
            this._context = context;
        }

        public Message Add(User item)
        {
            try
            {
                //عملیات رمز کردن کلمه عبور
                item.UserPassword = GetHashString(item.UserPassword);
                _context.Users.Add(item);
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

        public Message Update(User item)
        {
            try
            {
                // item.UserPassword = GetHashString(item.UserPassword);
                _context.Users.Attach(item);
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

        public User Find(int id)
        {
            try
            {
                return _context.Users.Find(id);
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

        public IQueryable<User> GetAll()
        {
            try
            {
                return _context.Users.AsQueryable();
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
                User user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
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

        public Message Remove(User item)
        {
            try
            {
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
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

        public User FindByUserName(string UserName)
        {
            try
            {
                var user = (from v in _context.Users
                            where v.UserName.ToLower().Equals(UserName.ToLower())
                            select v).SingleOrDefault();
                return user;
            }
            catch (DbUpdateException)
            {
                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public Message CheckUserPass(string userName, string passWord)
        {
            try
            {
                passWord = Utility.GetHashString(passWord);
                var user = _context.Users.Where(p => p.UserName.ToLower() == userName.ToLower() && p.UserPassword == passWord).FirstOrDefault();
                if (user == null)
                {
                    return Message.WrongUserPass;
                }
                return Message.Success;
            }
            catch (DbUpdateException)
            {
                return Message.unKnow;
            }
            catch (System.Exception)
            {
                return Message.unKnow;
            }
        }

        public UserType GetUserType(string userName, string passWord)
        {
            try
            {
                passWord = Utility.GetHashString(passWord);
                var user = _context.Users.Where(p => p.UserName.ToLower() == userName.ToLower() && p.UserPassword == passWord).FirstOrDefault();
                if (user == null)
                {
                    return UserType.unKnow;
                }
                return user.UserType;
            }
            catch (DbUpdateException)
            {
                return UserType.unKnow;
            }
            catch (System.Exception)
            {
                return UserType.unKnow;
            }
        }

        public UserType GetUserType(int userID)
        {
            try
            {
                var user = _context.Users.Where(p => p.UserID == userID).SingleOrDefault();
                if (user == null)
                {
                    return UserType.unKnow;
                }
                return user.UserType;
            }
            catch (DbUpdateException)
            {
                return UserType.unKnow;
            }
            catch (System.Exception)
            {
                return UserType.unKnow;
            }
        }
    }
}