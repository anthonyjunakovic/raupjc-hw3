using System;

namespace TodoRepository.Exceptions
{
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(Guid id) : base($"duplicate id: {id}")
        {

        }

        public DuplicateTodoItemException(TodoItem item) : this(item.Id)
        {

        }
    }

    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(Guid userId) : base($"access denied to user id: {userId}")
        {

        }
    }
}
