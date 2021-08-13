using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Views.TodoItem
{
    public static class SelectListConvenience
    {
        public static readonly SelectListItem[] ImportanceSelectListItems =
        {
            new SelectListItem {Text = "High", Value = Importance.High.ToString()},
            new SelectListItem {Text = "Medium", Value = Importance.Medium.ToString()},
            new SelectListItem {Text = "Low", Value = Importance.Low.ToString()},
        };

        public static readonly SelectListItem[] TodoListSortPropertySelectListItems =
        {
            new SelectListItem
            {
                Text  = "Done",
                Value = "IsDone"
            },
            new SelectListItem
            {
                Text  = "Importance",
                Value = "Importance"
            },
            new SelectListItem
            {
                Text  = "Name",
                Value = "Title"
            },
            new SelectListItem
            {
                Text  = "Rank",
                Value = "Rank"
            }
        };
        
        public static readonly SelectListItem[] SortDirectionSelectListItems =
        {
             new SelectListItem
            {
                Text  = "Ascending",
                Value = "asc"
                
            },
            new SelectListItem
            {
                Text  = "Descending",
                Value = "desc"
            }
        };

        public static List<SelectListItem> UserSelectListItems(this ApplicationDbContext dbContext)
        {
            return dbContext.Users.Select(u => new SelectListItem {Text = u.UserName, Value = u.Id}).ToList();
        }
    }
}