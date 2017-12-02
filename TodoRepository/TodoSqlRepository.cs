using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TodoRepository.Exceptions;
using TodoRepository.Interfaces;

namespace TodoRepository
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDb _todoDb;

        public TodoSqlRepository(TodoDb todoDb)
        {
            _todoDb = todoDb;
        }

        public void Add(TodoItem todoItem)
        {
            if (_todoDb.TodoItems.Where(i => (i.Id == todoItem.Id)).Count() > 0)
            {
                throw new DuplicateTodoItemException(todoItem);
            }
            _todoDb.TodoItems.Add(todoItem);
            _todoDb.SaveChanges();
        }

        public TodoItemLabel GenerateLabel(string label)
        {
            label = label.ToLower();
            TodoItemLabel todoLabel = _todoDb.TodoItemLabels.Where(l => l.Value == label).FirstOrDefault();
            if (todoLabel == null)
            {
                todoLabel = new TodoItemLabel(label);
            }
            return todoLabel;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem item = _todoDb.TodoItems.Include(i => i.Labels).Where(i => (i.Id == todoId)).FirstOrDefault();
            if (item == null)
            {
                return null;
            }
            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException(userId);
            }
            return item;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _todoDb.TodoItems.Include(i => i.Labels).Where(i => ((i.UserId == userId) && (!i.DateCompleted.HasValue))).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _todoDb.TodoItems.Include(i => i.Labels).Where(i => (i.UserId == userId)).OrderByDescending(i => i.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _todoDb.TodoItems.Include(i => i.Labels).Where(i => ((i.UserId == userId) && (i.DateCompleted.HasValue))).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _todoDb.TodoItems.Include(i => i.Labels).Where(i => ((i.UserId == userId) && (filterFunction(i)))).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if ((item == null) || (item.IsCompleted))
            {
                return false;
            }
            item.MarkAsCompleted();
            _todoDb.SaveChanges();
            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }
            _todoDb.TodoItems.Remove(item);
            _todoDb.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem item = Get(todoItem.Id, userId);
            if (item == null)
            {
                _todoDb.TodoItems.Add(todoItem);
            }
            else
            {
                _todoDb.Entry(item).CurrentValues.SetValues(todoItem);
            }
            _todoDb.SaveChanges();
        }
    }
}
