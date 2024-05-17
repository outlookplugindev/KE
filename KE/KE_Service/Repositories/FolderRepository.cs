using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KE_Service.Model;
namespace KE_Service.Repositories
{

    public class FolderRepository : IFolderRepository
    {
        private List<Folder> _folders = new List<Folder>();

        public FolderRepository()
        {
            // For demonstration purposes, let's add some dummy data
            _folders.Add(new Folder { Id = 1, Name = "John Doe", Email = "john@example.com" });
            _folders.Add(new Folder { Id = 2, Name = "Jane Smith", Email = "jane@example.com" });
        }

        public Task<IEnumerable<Folder>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Folder>>(_folders);
        }

        public Task<Folder> GetByIdAsync(int id)
        {
            return Task.FromResult(_folders.FirstOrDefault(u => u.Id == id));
        }

        public async Task<Folder> AddAsync(Folder user)
        {
            user.Id = _folders.Count + 1; // In a real application, you'd likely use a database-generated ID
            _folders.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<Folder> UpdateAsync(Folder user)
        {
            var existingUser = await GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("User not found");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var folder = await GetByIdAsync(id);
            if (folder == null)
                return false;

            _folders.Remove(folder);
            return true;
        }
    }
}
