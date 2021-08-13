using System;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.Views.TodoList
{
    public sealed class TodoListDetailViewmodelTests
    {
        [Fact]
        public void TodoListDetailViewmodel_ItemsAreNull_ViewModelCreatedWithEmptyList()
        {
            // Arrange
            var faker = new Faker();

            // Act
            Action act = () =>
            {
                var _ = new TodoListDetailViewmodel(faker.Random.Int(), faker.Lorem.Sentence(), null, true);
            };

            // Assert
            act.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void TodoListDetailViewmodel_ItemsNotNull_ViewModelCreatedWithSortedList()
        {
            // Arrange
            var todoList = TodoFakerManager.GetTodoListFaker().Generate();
            todoList.Items = TodoFakerManager.GetTodoItemFaker(todoList.Owner).GenerateBetween(2, 10);

            // Act
            var todoListDetailViewModel = TodoListDetailViewmodelFactory.Create(todoList, true);

            // Assert
            todoListDetailViewModel.Items.Should().BeInAscendingOrder(todoItem => todoItem.Importance);
        }
    }
}