using WebApplication1.Data;
using Newtonsoft.Json;
using Npgsql;
using WebApplication1.DAO;
namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("PostgreDB");
            builder.Services.AddScoped((provider) => new NpgsqlConnection(connectionString));
            builder.Services.AddScoped<IProductDao, ProductDaoImplementation>();

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                options.AddPolicy("FrontEndClient", builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
            });

            //If you are not getting the changes display change the scope to 'AddSingleton' from 'AddScoped'.
            // Singleton ----> Ek hi object create karta hai aur agar woh object exist karega
            // toh usi mein changes kardega


            // AddScoped----> It creates Instances Every time you execute the code,
            // that's why the changes are not displayed.

            //Add productrepositoryimple to service container
            //builder.Services.AddSingleton<IProductRepository, ProductRepositoryImpl>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("FrontEndClient");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}