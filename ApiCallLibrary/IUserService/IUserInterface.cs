

using ApiCallLibrary.DTO;

namespace ApiCallLibrary.IUserService
{
    public interface IUserInterface
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>?> GetAllUsersAsync(int pageNumber);
    }
}
