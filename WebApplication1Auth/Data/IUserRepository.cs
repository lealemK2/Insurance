using System.Threading.Tasks;
using Insurance.Model;

namespace Insurance.Data
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task LoginAsync(User user);
        Task RemoveAsync(long id);
        Task UpdateAsync(User user);
    }
}
