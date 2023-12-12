namespace ToDoApp.Models
{
    public class TodoViewModel
    {
        public int? SelectedCategoryId { get; set; }
        public List<TodoItem> TodoItems { get; set; }
        public List<Category> Categories { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public bool ShowWarning { get; set; }


    }
}
