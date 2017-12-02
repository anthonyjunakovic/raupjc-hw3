using System;
using System.Collections.Generic;

namespace TodoRepository
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public bool IsCompleted => DateCompleted.HasValue;
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateDue { get; set; }

        public TodoItem()
        {
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Text = text;
            Labels = new List<TodoItemLabel>();
            DateCreated = DateTime.UtcNow;
        }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is TodoItem)
            {
                TodoItem item = (TodoItem)obj;
                return (item.Id == this.Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
        }

        public static bool operator ==(TodoItem item1, TodoItem item2)
        {
            if (item1 is null)
            {
                return (item2 is null);
            }
            return item1.Equals(item2);
        }

        public static bool operator !=(TodoItem item1, TodoItem item2)
        {
            if (item1 is null)
            {
                return (!(item2 is null));
            }
            return !item1.Equals(item2);
        }
    }
}
