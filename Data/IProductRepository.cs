using WebApplication1.Models;
namespace WebApplication1.Data
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProducts();
        public Task<Product?> GetProductById(int Id);
        public Task<List<Product>> GetProductsByName(string name);
        public Task<List<Product?>> GetProductsByRating(double rating);
        public Task<Product?> GetProductsByCode(string productCode);
        public Task<Product?> UpdateProductPrice(int Id, double price);
        public Task<List<Product>> GetProductsByPrice(int minPrice, int maxPrice);
        public Task<bool> AddProduct(Product product);
        public Task<bool> RemoveProduct(int id);
    }
}