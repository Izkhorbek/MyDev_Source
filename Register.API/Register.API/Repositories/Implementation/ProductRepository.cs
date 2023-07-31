using Microsoft.EntityFrameworkCore;
using Register.API.Data;
using Register.API.Models.Domain;
using Register.API.Repositories.Interface;

namespace Register.API.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await appDbContext.Product.ToListAsync();   
        }
    }
}
