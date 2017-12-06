using System.Collections.Generic;

namespace Models
{
    public interface IUser_Temp : IBase<User_Temp>
    {
        bool CheckTemplateID(int TempID);

        bool CheckUserID(int UserID);

        Dictionary<string, string> getTempID_TempName();

        Dictionary<string, string> getTempID_TempName(int userID);

        Dictionary<string, string> getUserID_UserName();
        
        Utility.Message RemoveAllTemplate(int userID);
    }
}