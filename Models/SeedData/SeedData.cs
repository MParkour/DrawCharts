using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Models.Utility;

namespace Models
{
    public static class SeedData
    {
        public static Message Initialize(IServiceProvider serviceprovider)
        {
            try
            {
                var UserContext = serviceprovider.GetRequiredService<IUser>();
                if (UserContext.FindByUserName("admin") != null)
                {
                    return Message.DbNotEmpty;
                }
                UserContext.Add(new User
                {
                    UserName = "admin",
                    UserPassword = "1234",
                    UserType = Utility.UserType.Admin,
                    StatusCode = Utility.UserStatus.Enable,
                    CreateDate = "1396/05/10"
                });
                return Utility.Message.Success;
            }
            catch (DbUpdateException ex)
            {
                Microsoft.Data.Sqlite.SqliteException sqliteEX = ex.GetBaseException() as Microsoft.Data.Sqlite.SqliteException;
                int ErrorCode = sqliteEX.SqliteErrorCode;
                return Message.unKnow;
            }

            // using (var context = new DrawChartsContext(serviceprovider.GetRequiredService<DbContextOptions<DrawChartsContext>>()))
            // {
            //     // Look for any Users.
            //     if (context.Users.Any())
            //     {
            //         return -1;   // DB has been seeded
            //     }
            //     context.Users.Add(new User
            //     {
            //         UserName = "baba",
            //         UserPassword = "1234",
            //         UserType = MyEnum.UserType.Admin,
            //         StatusCode = MyEnum.UserStatus.Free,
            //         CreateDate = "1396/05/10"
            //     });
            //     context.SaveChanges();
            //     return 50;
            // }
        }
    }
}
