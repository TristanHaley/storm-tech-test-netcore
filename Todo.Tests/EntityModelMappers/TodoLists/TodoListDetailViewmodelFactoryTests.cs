using System;
using Bogus.Extensions;
using FluentAssertions;
using Todo.EntityModelMappers.TodoLists;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.EntityModelMappers.TodoLists
{
    public sealed class TodoListDetailViewmodelFactoryTests
    {
        [Fact]
        public void Create_IncludeDoneIsFalse_ReturnsItemsThatAreNotDone()
        {
            // Arrange
            var todoList = TodoFakerManager.GetTodoListFaker().Generate();
            var doneItems = TodoFakerManager.GetTodoItemFaker(todoList.Owner)
                                                 .RuleFor(todoItem => todoItem.IsDone, true)
                                                 .GenerateBetween(2, 3);
            var incompleteItems = TodoFakerManager.GetTodoItemFaker(todoList.Owner)
                                                  .RuleFor(todoItem => todoItem.IsDone, false)
                                                  .GenerateBetween(4, 5);

            doneItems.ForEach(todoItem => todoList.Items.Add(todoItem));
            incompleteItems.ForEach(todoItem => todoList.Items.Add(todoItem));

            // Act
            var todoListDetailViewModel = TodoListDetailViewmodelFactory.Create(todoList, false);

            // Assert
            todoListDetailViewModel.Items.Should()
                                   .HaveCount(incompleteItems.Count)
                                   .And.NotHaveCount(doneItems.Count);
        }
        
        [Fact]
        public void Create_IncludeDoneIsTrue_ReturnsAllItems()
        {
            // Arrange
            var todoList = TodoFakerManager.GetTodoListFaker().Generate();
            var doneItems = TodoFakerManager.GetTodoItemFaker(todoList.Owner)
                                            .RuleFor(todoItem => todoItem.IsDone, true)
                                            .GenerateBetween(2, 3);
            var incompleteItems = TodoFakerManager.GetTodoItemFaker(todoList.Owner)
                                                  .RuleFor(todoItem => todoItem.IsDone, false)
                                                  .GenerateBetween(4, 5);

            doneItems.ForEach(todoItem => todoList.Items.Add(todoItem));
            incompleteItems.ForEach(todoItem => todoList.Items.Add(todoItem));

            // Act
            var todoListDetailViewModel = TodoListDetailViewmodelFactory.Create(todoList, true);

            // Assert
            todoListDetailViewModel.Items.Should()
                                   .HaveCount(incompleteItems.Count + doneItems.Count);
        }
    }
}