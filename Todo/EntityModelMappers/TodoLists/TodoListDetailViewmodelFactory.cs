using System.Collections.Generic;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool includeDone, string sortProperty = "", string sortDirection = "")
        {
            var items = includeDone
                ? todoList.Items.Select(TodoItemSummaryViewmodelFactory.Create).ToList()
                : todoList.Items.Where(item => item.IsDone == false).Select(TodoItemSummaryViewmodelFactory.Create).ToList();

            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, includeDone, sortProperty, sortDirection);
        }
    }
}