namespace KE_Service.Repositories
{        // Repositories/IUserRepository.cs
    using KE_Service.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllAsync();
        Task<Folder> GetByIdAsync(int id);
        Task<Folder> AddAsync(Folder user);
        Task<Folder> UpdateAsync(Folder user);
        Task<bool> DeleteAsync(int id);
    }


}
