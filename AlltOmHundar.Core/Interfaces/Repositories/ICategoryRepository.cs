using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithTopicsAsync();
        Task<Category?> GetCategoryWithTopicsAsync(int categoryId);
    }
}