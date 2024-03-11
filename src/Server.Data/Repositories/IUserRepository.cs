namespace Server.Data.Repositories
{
    using System.Threading.Tasks;
    using Server.Data.Entities;

    public interface IUserRepository
    {
        Task<User> GetLoginAsync(string username, string passwrod);
    }
}