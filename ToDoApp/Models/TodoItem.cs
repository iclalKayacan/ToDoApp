namespace ToDoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; } // Kullanıcı ID'sini ekleyin

        public string FormattedStartDate => StartDate.ToShortDateString();
        public string FormattedEndDate => EndDate.ToShortDateString();
        public bool IsReminderDue =>
        ReminderDateTime.HasValue &&
        ReminderDateTime.Value.Date >= DateTime.Today.AddDays(7);

        public bool IsEndDateApproaching =>
            EndDate.Date >= DateTime.Today &&
            EndDate.Date <= DateTime.Today.AddDays(7);
        public void SetDefaultReminder()
        {
            if (EndDate >= DateTime.Today.AddDays(7))
            {
                ReminderDateTime = EndDate.AddDays(-7);
            }
        }


    }

}
