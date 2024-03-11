
namespace Server.Data.Repositories
{
    using Server.Data.Entities;

    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> roles = new List<Role>
        {
            new Role { Id = Guid.Parse("fe65031c-a784-44e2-aaf9-75a0c2f33c4c"), Name = "Admin" },
            new Role { Id = Guid.Parse("26e3f4b8-38ad-4171-9c33-8dc1c8d49176"), Name = "User" }
        };

        public Task<List<Role>> GetBulkAsync(IEnumerable<Guid> id)
        {
            var result = this.roles.Where(f => id.Contains(f.Id)).ToList();
            
            return Task.FromResult(result);
        }
    }
}