namespace ToDoApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // TodoItem ilişkisi
        public ICollection<TodoItem> TodoItems { get; set; }
    }
}
