using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewMvc.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace NewMvc.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Document> documents = new List<Document>();

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM newuser", connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document doc = new Document();
                            doc.Id = reader.GetInt32("Id");
                            doc.FirstName = reader.GetString("FirstName");
                            documents.Add(doc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ViewBag.ErrorMessage = "An error occurred while retrieving data.";
                return View("Error");
            }
        }
        return View(documents);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
