using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public ICollection<TodoItemSummaryViewmodel> Items;
        public int                                   TodoListId    { get; }
        public string                                Title         { get; }
        [DisplayName("Include Done?")]
        public bool                                  IncludeDone   { get; }
        [DisplayName("Sort Order")]
        public string                                SortDirection { get; set; }
        [DisplayName("Sort By")]
        public string                                SortProperty  { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool includeDone, string sortProperty = "", string sortDirection = "")
        {
            TodoListId    = todoListId;
            Title         = title;
            IncludeDone   = includeDone;
            SortProperty  = sortProperty;
            SortDirection = sortDirection;
            
            items ??= new List<TodoItemSummaryViewmodel>();
            
            Items = SearchParametersValid()
                ? items.AsQueryable().OrderBy($"{SortProperty} {SortDirection}").ToList()
                : items;
        }
        
        private bool SearchParametersValid ()
        {
            // Guard: No sort property / direction
            if (string.IsNullOrWhiteSpace(SortProperty) || string.IsNullOrWhiteSpace(SortDirection)) return false;
            
            // Guard: Property does not exist or is invalid
            if (TodoItemEditFields.ValidSortFields.All(validField => validField != SortProperty)) return false;

            // Guard: Invalid sort direction
            if (SortDirection != "asc" && SortDirection != "desc") return false;    
                
            return true;
        }
    }
}