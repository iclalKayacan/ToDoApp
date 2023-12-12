using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TodoViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoViewController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? SelectedCategoryId)
        {
            var viewModel = new TodoViewModel();
            viewModel.Categories = await _context.Categories.ToListAsync();

            int userId = HttpContext.Session.GetInt32("UserID") ?? -1; 
            viewModel.SelectedCategoryId = SelectedCategoryId;

            if (SelectedCategoryId.HasValue)
            {
                viewModel.TodoItems = await _context.TodoItems
                    .Where(t => t.CategoryId == SelectedCategoryId && t.UserId == userId) 
                    .Include(t => t.Category)
                    .ToListAsync();
            }
            else
            {
                viewModel.TodoItems = await _context.TodoItems
                    .Where(t => t.UserId == userId) 
                    .Include(t => t.Category)
                    .ToListAsync();
            }

            return View(viewModel);
        }
        public IActionResult PendingTasks()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1; 
            var viewModel = new TodoViewModel
            {
                TodoItems = _context.TodoItems
                                    .Where(t => !t.IsComplete && t.UserId == userId) 
                                    .Include(t => t.Category)
                                    .ToList(),
                Categories = _context.Categories.ToList()
            };
            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1; 

            var viewModel = new TodoViewModel
            {
                Categories = await _context.Categories.ToListAsync()
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel.TodoItems = await _context.TodoItems
                    .Where(t => t.Name.Contains(searchString) && t.UserId == userId) 
                    .Include(t => t.Category)
                    .ToListAsync();
            }
            else
            {
                viewModel.TodoItems = await _context.TodoItems
                    .Where(t => t.UserId == userId) 
                    .Include(t => t.Category)
                    .ToListAsync();
            }

            return View("Index", viewModel);
        }

        public IActionResult UpcomingTasks()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? -1;
            var upcomingDeadline = DateTime.Now.AddDays(7);
            var viewModel = new TodoViewModel
            {
                TodoItems = _context.TodoItems
                                    .Where(t => t.ReminderDateTime.HasValue &&
                                                t.ReminderDateTime.Value <= upcomingDeadline &&
                                                t.UserId == userId) 
                                    .Include(t => t.Category)
                                    .ToList(),
                Categories = _context.Categories.ToList()
            };

            return View("Index", viewModel);
        }


    }
}
