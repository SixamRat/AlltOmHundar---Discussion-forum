using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithTopicsAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(string name, string? description, int displayOrder);
        Task<bool> UpdateCategoryAsync(int id, string name, string? description, int displayOrder);
        Task<bool> DeleteCategoryAsync(int id);
    }
}