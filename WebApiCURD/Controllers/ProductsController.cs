using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using WebApiCURD.Models;

namespace WebApiCURD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string connectionString;
        public ProductsController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDB"] ?? "";
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductDtu productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO products (name, brand, category, price, description) VALUES (@name, @brand, @category, @price, @description)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the product.", error = ex.Message });
            }
            return Ok(new { message = "Product created successfully." });
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM products";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Brand = reader.GetString(2),
                                    Category = reader.GetString(3),
                                    Price = reader.GetDecimal(4).ToString(), // Read as Decimal and convert to String
                                    Description = reader.GetString(5),
                                    Created = reader.GetDateTime(6)
                                };

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the products.", error = ex.Message });
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = new Product();
            try
            {
                using(var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM products where id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using(var reader = command.ExecuteReader()) { 
                            if(reader.Read())
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4).ToString(); // Read as Decimal and convert to String
                                product.Description = reader.GetString(5);
                                product.Created = reader.GetDateTime(6);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the specified products.", error = ex.Message });
            }

            return Ok(product);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDtu productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE products SET name=@name, brand=@brand, category=@category, price=@price, description=@description WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);
                        command.Parameters.AddWithValue("@id", id);  // Add this line to set the id parameter

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
            return Ok(new { message = "Product updated successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FRom products where id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the product.", error = ex.Message });
            }
            return Ok();
        }


    }
}
