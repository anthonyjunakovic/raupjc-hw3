using System.Data.Entity;

namespace TodoRepository
{
    public class TodoDb : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoItemLabels { get; set; }

        public TodoDb(string cnnstr) : base(cnnstr)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItem>().Property(i => i.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCompleted).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateDue).IsOptional();

            modelBuilder.Entity<TodoItemLabel>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(i => i.Value).IsRequired();

            modelBuilder.Entity<TodoItem>().HasMany(i => i.Labels).WithMany(i => i.LabelTodoItems);
        }
    }
}
