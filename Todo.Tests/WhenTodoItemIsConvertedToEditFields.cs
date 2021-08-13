using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoItemIsConvertedToEditFields
    {
        private readonly TodoItem srcTodoItem;
        private readonly TodoItemEditFields resultFields;

        public WhenTodoItemIsConvertedToEditFields()
        {
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High, 0)
                    .Build();

            srcTodoItem = todoList.Items.First();

            resultFields = TodoItemEditFieldsFactory.Create(srcTodoItem);
        }

        [Fact]
        public void EqualTodoListId()
        {
            srcTodoItem.TodoListId.Should().Be(resultFields.TodoListId);
        }

        [Fact]
        public void EqualTitle()
        {
            srcTodoItem.Title.Should().Be(resultFields.Title);
        }

        [Fact]
        public void EqualRank()
        {
            srcTodoItem.Rank.Should().Be(resultFields.Rank);
        }

        [Fact]
        public void EqualImportance()
        {
            srcTodoItem.Importance.Should().Be(resultFields.Importance);
        }
    }
}