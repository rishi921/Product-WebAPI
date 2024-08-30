using WebApplication1.Models;

namespace WebApplication1.DAO
{
    public interface IProductDao
    {
        Task<int> InsertProduct(Product P);
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task<int> UpdateProductPriceById(int Id, decimal price);
        Task<int> DeleteProductById(int id);
        Task<int> GetTotalProductsCount();
        Task<List<Product>> GetProductsSortedByPrice();
        Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    }
}
