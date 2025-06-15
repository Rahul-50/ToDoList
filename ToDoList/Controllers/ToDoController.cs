using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
//using System.Data.SqlClient;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TodoController : Controller
    {
        private readonly IConfiguration _configuration;

        public TodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<TodoItem> items = new();
            using SqlConnection conn = new(GetConnectionString());
            conn.Open();
            string sql = "SELECT * FROM TodoItems";
            using SqlCommand cmd = new(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                items.Add(new TodoItem
                {
                    Id = (int)reader["Id"],
                    Title = reader["Title"].ToString(),
                    IsCompleted = (bool)reader["IsCompleted"]
                });
            }

            return View(items);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title)
        {
            using SqlConnection conn = new(GetConnectionString());
            conn.Open();
            string sql = "INSERT INTO TodoItems (Title, IsCompleted) VALUES (@title, 0)";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            TodoItem item = null;

            using SqlConnection conn = new(GetConnectionString());
            conn.Open();
            string sql = "SELECT * FROM TodoItems WHERE Id = @id";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                item = new TodoItem
                {
                    Id = (int)reader["Id"],
                    Title = reader["Title"].ToString(),
                    IsCompleted = (bool)reader["IsCompleted"]
                };
            }

            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Update(TodoItem item)
        {
            using SqlConnection conn = new(GetConnectionString());
            conn.Open();
            string sql = "UPDATE TodoItems SET Title = @title, IsCompleted = @isCompleted WHERE Id = @id";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@title", item.Title);
            cmd.Parameters.AddWithValue("@isCompleted", item.IsCompleted);
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using SqlConnection conn = new(GetConnectionString());
            conn.Open();
            string sql = "DELETE FROM TodoItems WHERE Id = @id";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
    }
}
