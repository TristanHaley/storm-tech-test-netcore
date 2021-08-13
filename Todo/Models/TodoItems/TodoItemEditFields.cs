using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemEditFields
    {
        public int    TodoListId    { get; set; }
        public string Title         { get; set; }
        public string TodoListTitle { get; set; }
        public int    TodoItemId    { get; set; }
        
        [Range(1, 10)]
        public int    Rank          { get; set; }
        [DisplayName("Complete?")]
        public bool   IsDone        { get; set; }
        
        [DisplayName("Assignee")]
        public string ResponsiblePartyId { get; set; }
        public Importance Importance { get; set; }

        public static IEnumerable<string> ValidSortFields 
        {
            get
            {
                return new[]
                {
                    "Importance",
                    "Title",
                    "Rank",
                    "IsDone"
                };
            }
        }
        
        public TodoItemEditFields() { }

        public TodoItemEditFields(int todoListId, string todoListTitle, int todoItemId, string title, bool isDone, string responsiblePartyId, Importance importance, int rank)
        {
            TodoListId         = todoListId;
            TodoListTitle      = todoListTitle;
            TodoItemId         = todoItemId;
            Title              = title;
            IsDone             = isDone;
            ResponsiblePartyId = responsiblePartyId;
            Importance         = importance;
            Rank               = rank;
        }
    }
}