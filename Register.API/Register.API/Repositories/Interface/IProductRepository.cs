using Register.API.Models.Domain;

namespace Register.API.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
    }
}
