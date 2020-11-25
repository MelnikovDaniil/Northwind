using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.ConsoleClient
{
    class Program
    {
        public const string Category = "/category";
        public const string Product = "/product";
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            // Set up configuration sources.
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true)
                .Build();


            var responce = await client.GetAsync(config["ApiUrl"] + Product);
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(content);
                Console.WriteLine("Categories");
                foreach (var product in products)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Id: {product.ProductId}");
                    Console.WriteLine($"Name: {product.ProductName}");
                    Console.WriteLine($"Is discounted: {product.Discontinued}");
                    Console.WriteLine($"UnitPrice: {product.UnitPrice}");
                    Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                }
            }

            responce = await client.GetAsync(config["ApiUrl"] + Category);
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(content);
                Console.WriteLine("Products");
                foreach (var category in categories)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Id  : {category.CategoryId}");
                    Console.WriteLine($"Name: {category.CategoryName}");
                    Console.WriteLine($"Desc: {category.Description}");
                }

            }
            Console.ReadLine();
        }
    }
}
