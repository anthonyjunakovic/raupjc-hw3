using System;
using System.Collections.Generic;

namespace TodoRepository
{
    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }

        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }        public TodoItemLabel()
        {
            LabelTodoItems = new List<TodoItem>();
        }
        public override bool Equals(object obj)
        {
            if (obj is TodoItemLabel)
            {
                TodoItemLabel item = (TodoItemLabel)obj;
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
            return Value;
        }

        public static bool operator ==(TodoItemLabel item1, TodoItemLabel item2)
        {
            if (item1 is null)
            {
                return (item2 is null);
            }
            return item1.Equals(item2);
        }

        public static bool operator !=(TodoItemLabel item1, TodoItemLabel item2)
        {
            if (item1 is null)
            {
                return (!(item2 is null));
            }
            return !item1.Equals(item2);
        }
    }
}
