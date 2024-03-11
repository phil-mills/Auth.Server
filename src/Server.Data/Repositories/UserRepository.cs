namespace Server.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Server.Data.Entities;

    public class UserRepository : IUserRepository
    {
        private readonly IEnumerable<User> Users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "string",
                Password = "string",
                Roles = new List<Guid> 
                {
                    Guid.Parse("fe65031c-a784-44e2-aaf9-75a0c2f33c4c"),
                    Guid.Parse("26e3f4b8-38ad-4171-9c33-8dc1c8d49176")
                }
               
            }
        };

        public async Task<User> GetLoginAsync(string username, string password)
        {
            return this.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}