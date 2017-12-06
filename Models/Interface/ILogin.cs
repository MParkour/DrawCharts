namespace Models
{
    public interface ILogin:IBase<Login_Log>
    {
        bool CheckUserID(int UserID);
    }
}