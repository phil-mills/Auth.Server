namespace Server.Data.Repositories
{
    using System.Threading.Tasks;
    using Server.Data.Entities;

    public interface IRoleRepository
    {
        public Task<List<Role>> GetBulkAsync(IEnumerable<Guid> id);
    }
}