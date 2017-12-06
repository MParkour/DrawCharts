using System.Collections.Generic;

namespace Models
{
    public interface IUser : IBase<User>
    {
        User FindByUserName(string UserName);

        Utility.Message CheckUserPass(string userName, string passWord);
        Utility.UserType GetUserType(string userName, string passWord);
        Utility.UserType GetUserType(int userID);
    }
}