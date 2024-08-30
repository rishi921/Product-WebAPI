using Npgsql;
using NpgsqlTypes;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.DAO
{
    public class ProductDaoImplementation : IProductDao
    {
        NpgsqlConnection _connection;
        public ProductDaoImplementation(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        //public async Task<List<Product>> GetProducts()
        //{
        //    //return Task.Run(() => productList);
        //    throw new NotImplementedException();

        //}




        public async Task<List<Product>> GetProducts()
        {
            string errormessage = null;
            string query = @"select * from practice.products";
            Product product = null;

            List<Product> productList = new List<Product>();

            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product = new Product();
                            product.ProductId = (int)reader.GetInt32(0);
                            product.ProductName = (string)reader.GetString(1);
                            product.Price = reader.GetDouble(2);
                            product.Category = reader["category"].ToString();
                            product.StarRating = Convert.ToInt32(reader["star_rating"]);
                            product.Description = reader["description"].ToString();
                            product.ProductCode = reader["product_code"].ToString();
                            productList.Add(product);
                        }
                    }
                    reader?.Close();
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                Console.WriteLine("-------Exception in getting products ---------" + errormessage);
            }
            return productList;
        }




        public async Task<Product> GetProductById(int id)
        {
            Product product = null;
            string errorMessage = string.Empty;
            string query = @"select * from practice.products where product_id=@pid";
            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@pid", id);
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product = new Product();
                            product.ProductId = reader.GetInt32(0);
                            product.ProductName = reader.GetString(1);
                            product.Price = reader.GetDouble(2);
                            product.Category = reader["Category"].ToString();
                            product.StarRating = Convert.ToInt32(reader["star_rating"]);
                            product.Description = reader["Description"]?.ToString();
                            product.ProductCode = (string)reader["ProductCode"];
                            product.ImageUrl = (string)reader["ImageURL"];

                        }
                    }
                    reader?.Close();
                }
                return product;
            }
            catch (NpgsqlException e)
            {
                errorMessage = e.Message;
                Console.WriteLine("------ Exception in adding a productr---------" + errorMessage);
            }
            return product;
        }

        public async Task<int> InsertProduct(Product p)
        {
            // create a connection onbject using connection string
            // open the connection
            // create a command object pass the connection
            // specify the command type
            // CREATE the  query , call the command object execute method. if you hava paraeter add the paramter
            // close the reader
            // close the connection

            int rowsInserted = 0;
            string message;
            string insertQuery = @$"insert into practice.products(ProductName, Price, Category, star_rating,
            Description, ProductCode, ImageURL) values
            ('{p.ProductName}', {p.Price}, '{p.Category}', {p.StarRating}, '{p.Description}', '{p.ProductCode}', '{p.ImageUrl}')";
            Console.WriteLine("QUery" + insertQuery);
            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, _connection);
                    insertCommand.CommandType = CommandType.Text;
                    rowsInserted = await insertCommand.ExecuteNonQueryAsync();
                }

            }
            catch (NpgsqlException e)
            {
                message = e.Message;
                Console.WriteLine("------ Exception ---------" + message);
            }

            return rowsInserted;

        }

        //Update Product price
        public async Task<int> UpdateProductPriceById(int id, decimal newPrice)
        {
            int rowsAffected;
            string query = $"update practice.products set price=@price where product_id= @pid; ";
            using (_connection)
            {
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                await _connection.OpenAsync(); command.CommandType = CommandType.Text;

                // Add the input parameter and set its properties.
                NpgsqlParameter priceParameter = new()
                {
                    ParameterName = "@price",
                    NpgsqlDbType = NpgsqlDbType.Numeric,
                    Direction = ParameterDirection.Input,
                    Value = newPrice
                };
                NpgsqlParameter idParameter = new()
                {
                    ParameterName = "@pid",
                    NpgsqlDbType = NpgsqlDbType.Integer,
                    Direction = ParameterDirection.Input,
                    Value = id
                };
                command.Parameters.Add(priceParameter);
                command.Parameters.Add(idParameter);
                rowsAffected = await command.ExecuteNonQueryAsync();

            }
            return rowsAffected;
        }


        public async Task<int> DeleteProductById(int id)
        {
            int rowsAffected = 8;
            string query = $"delete from practice.products where product_id = @pid";
            using (_connection)
            {
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                await _connection.OpenAsync();
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@pid", NpgsqlDbType.Integer).Value = id;
                rowsAffected = await command.ExecuteNonQueryAsync();

            }
            return rowsAffected;
        }


        //Get the total no. of products
        public async Task<int> GetTotalProductsCount()
        {
            int totalProduct = 0;
            string query = "select count(product_id) from practice.products";
            using (_connection)
            {
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                await _connection.OpenAsync();
                command.CommandType = CommandType.Text;
                var result = await command.ExecuteScalarAsync();
                totalProduct = Convert.ToInt32(result);
            }
            return totalProduct;
        }

        

        public async Task<List<Product>> GetProductsSortedByPrice()
        {
            List<Product> products = new List<Product>();
            string query = "select * from practice.products order by Price asc;";
            using (_connection)
            {
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                await _connection.OpenAsync();
                command.CommandType = CommandType.Text;
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Product product = new Product();
                        product.ProductId = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                        product.Price = reader.GetDouble(2);
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public async Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            List<Product> products = new List<Product>();
            string query = "select * from practice.products where Price >= @minPrice and Price <= @maxPrice order by Price asc;";
            using (_connection)
            {
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                await _connection.OpenAsync();
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@minPrice", NpgsqlDbType.Numeric).Value = minPrice;
                command.Parameters.Add("@maxPrice", NpgsqlDbType.Numeric).Value = maxPrice;
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Product product = new Product();
                        product.ProductId = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                        product.Price = reader.GetDouble(2);
                        products.Add(product);
                    }
                }
            }
            return products;
        }
    }
}