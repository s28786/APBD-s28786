using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;
using Task6.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Task6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        //If warehouseDbContext is used
        /*
        private readonly WarehouseDbContext _context;
        public WarehouseController(WarehouseDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Amount has to be greater than zero");
            }

            var productExists = _context.Products.Any(p => p.Id == request.ProductId);
            if (!productExists)
            {
                return NotFound($"No product found with Id {request.ProductId}");
            }

            //check whether warehouseExists

            var order = _context.Orders
                .Where(o => o.ProductId == request.ProductId && o.CreatedAt < request.CreatedAt)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();
        */

        // If dbcontext is not used

        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;TrustServerCertificate=True";

        [HttpPost]
        public IActionResult AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Amount has to be greater than zero");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    string query = $"SELECT COUNT(*) FROM Product WHERE IdProduct = {request.ProductId}";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        if ((int)myCommand.ExecuteScalar() == 0)
                        {
                            transaction.Rollback();
                            return NotFound($"No product found with Id {request.ProductId}");
                        }
                    }

                    query = $"SELECT COUNT(*) FROM Warehouse WHERE IdWarehouse = {request.WarehouseId}";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        if ((int)myCommand.ExecuteScalar() == 0)
                        {
                            transaction.Rollback();
                            return NotFound($"No warehouse found with Id {request.WarehouseId}");
                        }
                    }
                    query = $"SELECT COUNT(*) FROM [Order] WHERE IdProduct = {request.ProductId} AND Amount = {request.Amount} AND CreatedAt < CONVERT(datetime, '{request.CreatedAt}', 126);";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        Console.WriteLine("Here 1");
                        if ((int)myCommand.ExecuteScalar() == 0)
                        {
                            transaction.Rollback();
                            return NotFound($"No order found matched");
                        }
                    }
                    int orderId = 0;
                    //check if order is completed
                    query = $"SELECT COUNT(*) FROM [Order] WHERE IdProduct = {request.ProductId} AND Amount = {request.Amount} AND CreatedAt < CONVERT(datetime, '{request.CreatedAt}', 126) AND FulfilledAt IS NULL";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        Console.WriteLine("Here 2");
                        if ((int)myCommand.ExecuteScalar() == 0)
                        {
                            transaction.Rollback();
                            return NotFound($"Order is already completed");
                        }
                    }

                    //get order id from idproduct and amount
                    query = $"SELECT IdOrder FROM [Order] WHERE IdProduct = {request.ProductId} AND Amount = {request.Amount} AND CreatedAt < CONVERT(datetime, '{request.CreatedAt}', 126) AND FulfilledAt IS NULL";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        orderId = (int)myCommand.ExecuteScalar();
                    }

                    //check if product-warehouse has the orderid
                    query = $"SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = {orderId}";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        if ((int)myCommand.ExecuteScalar() != 0)
                        {
                            transaction.Rollback();
                            return NotFound($"Product-warehouse existed with order id {orderId}");
                        }
                    }
                    //update the Order table FullfilledAt column of the order with the current date and time
                    query = $"UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = {orderId}";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        myCommand.ExecuteNonQuery();
                    }
                    //get price from product with id = request.ProductId
                    decimal price;
                    query = $"SELECT Price FROM Product WHERE IdProduct = {request.ProductId}";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        price = (decimal)myCommand.ExecuteScalar();
                    }
                    price = price * request.Amount;
                    //insert a record into the Product_Warehouse table

                    query = $"INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES ({request.WarehouseId}, {request.ProductId}, {orderId}, {request.Amount}, {price}, GETDATE())";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        myCommand.ExecuteNonQuery();
                    }
                    //get the primary key of the record inserted into the Product_Warehouse table
                    int maxId;
                    query = "SELECT MAX(IdProductWarehouse) FROM Product_Warehouse";
                    using (SqlCommand myCommand = new SqlCommand(query, connection, transaction))
                    {
                        maxId = (int)myCommand.ExecuteScalar();
                    }
                    //As a result of the operation, we return the value of the primary key
                    //generated for the record inserted into the Product_Warehouse table.

                    transaction.Commit();
                    return Ok(maxId);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"An error occured: {ex.Message}");
                }
            }
        }

        // ExtraTask
        // Adding using StoredProcedures

        [HttpPost("AddUsingProcedure")]
        public IActionResult AddProductToWarehouseUsingProcedure([FromBody] ProductWarehouseRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Amount must be greater than zero.");
            }
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddProductToWarehouse", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // set up the parameters
                    command.Parameters.Add(new SqlParameter("@IdProduct", request.ProductId));
                    command.Parameters.Add(new SqlParameter("@IdWarehouse", request.WarehouseId));
                    command.Parameters.Add(new SqlParameter("@Amount", request.Amount));
                    command.Parameters.Add(new SqlParameter("@CreatedAt", request.CreatedAt));
                    try
                    {
                        command.ExecuteNonQuery();
                        return Ok("product added via stored procvedure successfully.");
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occured: {ex.Message}");
                    }
                }
            }
        }
    }
}