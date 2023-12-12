using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TodoManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var todoItem = await _context.TodoItems
                                        .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsComplete = true;
            _context.Update(todoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "TodoView");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var task = _context.TodoItems
                               .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (task != null)
            {
                _context.TodoItems.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "TodoView");
        }

        public IActionResult Edit(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var task = _context.TodoItems
                       .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem editedTask)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var existingTask = _context.TodoItems
                                      .FirstOrDefault(t => t.Id == editedTask.Id && t.UserId == userId);

            if (existingTask != null)
            {
                existingTask.Name = editedTask.Name;
                existingTask.IsComplete = editedTask.IsComplete;
                existingTask.CategoryId = editedTask.CategoryId;
                existingTask.ReminderDateTime = editedTask.ReminderDateTime;
                existingTask.StartDate = editedTask.StartDate;
                existingTask.EndDate = editedTask.EndDate;

                _context.SaveChanges();
                return RedirectToAction("Index", "TodoView");
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult Create(TodoItem newTask)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            newTask.UserId = userId; 

            _context.TodoItems.Add(newTask);
            _context.SaveChanges();
            return RedirectToAction("Index", "TodoView");
        }


        [HttpGet]
        public IActionResult AddReminder(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var todoItem = _context.TodoItems
                                   .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }


        [HttpPost]
        public async Task<IActionResult> AddReminder(int id, DateTime reminderDateTime)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var todoItem = await _context.TodoItems
                                         .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.ReminderDateTime = reminderDateTime;

            _context.Update(todoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "TodoView");
        }



    }
}