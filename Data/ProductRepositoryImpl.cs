using WebApplication1.Models;

namespace WebApplication1.Data

{
    public class ProductRepositoryImpl : IProductRepository
    {
        List<Product> productList = new List<Product>
        {
            new Product{ ProductId=1,ProductName="Soccer Ball" , ProductCode ="SOB-BAL",Price=2000, Category="Soccer" ,ImageUrl=@"assets/images/socccerball.jpeg", Description="There are many variations of passages of Lorem Ipsum available",StarRating=4.5},
            new Product{ ProductId=2,ProductName="Kayak" , Price=10000,ProductCode ="WAT-KAK" ,Category="WaterSports" ,ImageUrl=@"assets/images/kayak.jpeg", Description="There are many variations of passages of Lorem Ipsum available",StarRating=3.7},
            new Product{ ProductId=3,ProductName="Life Jacket" , Price=800,ProductCode ="WAT-LJK" ,Category="Water Sports", ImageUrl=@"assets/images/lifeJacket.jpeg", Description="There are many variations of passages of Lorem Ipsum available",StarRating=2.5},
            new Product{ ProductId=4,ProductName="Chess Board" , Price=200,ProductCode ="CHS-BOD", Category="Indoor Games",ImageUrl=@"assets/images/chessboard.jpeg", Description="There are many variations of passages of Lorem Ipsum available",StarRating=4.3},
            new Product{ ProductId=5,ProductName="Carrom Coins" ,ProductCode ="CAR-CON" ,Price=700, Category="Soccer",ImageUrl=@"assets/images/socccerball.jpeg", Description="There are many variations of passages of Lorem Ipsum available",StarRating=3.5},


        };

        public Task<List<Product>> GetProducts()
        {
            return Task.Run(() => productList);
        }

        public Task<Product?> GetProductById(int id)
        {
            Task<Product?> pdtFound = Task.Run(() => productList.SingleOrDefault(x => x.ProductId == id));
            if (pdtFound != null)
                return pdtFound;
            else
                return null;
        }

        public async Task<Product?> UpdateProductPrice(int id, double newPrice)
        {
            Product? product = null;
            product = await GetProductById(id);
            if (product != null)
            {
                product.Price = newPrice;
            }
            return product;
        }

        public Task<List<Product>> GetProductsByPrice(int minPrice, int maxPrice)
        {
            List<Product> productsFound = null;
            productsFound = productList.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();
            return Task.FromResult(productsFound);
        }

        public Task<bool> AddProduct(Product product)
        {
            bool isAdded = false;
            if (product != null)
            {  //write logic to check the product has been added successfully and return true
                // l variable is created to check whether it's added or not.
                int length = productList.Count;
                // To add the product in the AddList.
                productList.Add(product);
                if (productList.Count == length + 1)
                {
                    isAdded = true;
                }

            }
            return Task.FromResult(isAdded);
        }

        public Task<List<Product>> GetProductsByName(string Name)
        {
            List<Product> productsFound = null;
            productsFound = productList.Where(x => x.ProductName.ToLower().Contains(Name.ToLower())).ToList();
            return Task.Run(() => productsFound);

        }public Task<Product?> GetProductsByCode(string productCode)
        {
            Product productFound = null;
            productFound = productList.FirstOrDefault(x=> x.ProductCode == productCode);
            return Task.FromResult(productFound);
        }
        
        public Task<List<Product?>> GetProductsByRating(double rating)
        {
            List<Product> productsFound = null;
            productsFound = productList.Where(x => x.StarRating == rating).ToList();
            return Task.FromResult(productsFound);
        }

        public async Task<bool> RemoveProduct(int id)
        {
            bool isRemoved = false;
            Product pdt = await GetProductById(id);
            if (pdt != null)
            {
                isRemoved = productList.Remove(pdt);
            };
            return isRemoved;
        }
    }
}