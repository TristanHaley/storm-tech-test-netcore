using System.Linq;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Todo.Services;
using Todo.Tests.Builders;
using Xunit;

namespace Todo.Tests.Services
{
    public sealed class ApplicationDbContextConvenienceTests
    {
        [Fact]
        public void RelevantTodoLists_MultipleOwnedListsFound_ReturnsAllOwnedListsAsQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;
            var user    = new IdentityUser(faker.Person.Email);

            var relevantLists = TodoFakerManager.GetTodoListFaker(user).GenerateBetween(3, 5);

            context.TodoLists.AddRange(relevantLists);
            context.TodoLists.AddRange(TodoFakerManager.GetTodoListFaker().GenerateBetween(3, 5));
            context.SaveChanges();

            // Act
            var result = context.RelevantTodoLists(user.Id);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.HaveCount(relevantLists.Count)
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Owner.Id == user.Id));
        }

        [Fact]
        public void RelevantTodoLists_OwnedItemInUnownedList_ReturnsUnownedListContainingOwnedItemsAsQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;
            var user    = new IdentityUser(faker.Person.Email);

            var ownedTask   = TodoFakerManager.GetTodoItemFaker(user).Generate();
            var unownedList = TodoFakerManager.GetTodoListFaker().Generate();

            unownedList.Items.Add(ownedTask);

            context.TodoLists.Add(unownedList);
            context.SaveChanges();

            // Act
            var result = context.RelevantTodoLists(user.Id);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.HaveCount(1)
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Owner.Id == unownedList.Owner.Id))
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Owner.Id != user.Id))
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Items.Any(todoItem => todoItem.ResponsibleParty.Id == user.Id)));
        }

        [Fact]
        public void RelevantTodoLists_OwnedItemsInMultipleUnownedLists_ReturnsUnownedListsContainingOwnedItemsAsQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;
            var user    = new IdentityUser(faker.Person.Email);

            var unownedLists    = TodoFakerManager.GetTodoListFaker().GenerateBetween(3, 4);
            var unownedListWithUnownedTask = TodoFakerManager.GetTodoListFaker().Generate();

            unownedLists.ForEach(unownedList => unownedList.Items.Add(TodoFakerManager.GetTodoItemFaker(user).Generate()));
            unownedListWithUnownedTask.Items.Add(TodoFakerManager.GetTodoItemFaker().Generate());

            context.TodoLists.AddRange(unownedLists);
            context.TodoLists.Add(unownedListWithUnownedTask);
            context.SaveChanges();

            // Act
            var result = context.RelevantTodoLists(user.Id);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.HaveCount(unownedLists.Count)
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Owner.Id != user.Id))
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Items.Any(todoItem => todoItem.ResponsibleParty.Id == user.Id)));
        }

        [Fact]
        public void RelevantTodoLists_SingleOwnedListFound_ReturnsSingleListAsQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;
            var user    = new IdentityUser(faker.Person.Email);

            var relevantList = TodoFakerManager.GetTodoListFaker(user).Generate();

            context.TodoLists.Add(relevantList);
            context.TodoLists.AddRange(TodoFakerManager.GetTodoListFaker().GenerateBetween(3, 5));
            context.SaveChanges();

            // Act
            var result = context.RelevantTodoLists(user.Id);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.HaveCount(1)
                  .And.Match(todoLists => todoLists.All(todoList => todoList.Owner.Id == user.Id));
        }

        [Fact]
        public void RelevantTodoLists_TableEmpty_ReturnsEmptyQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;

            // Act
            var result = context.RelevantTodoLists(faker.Person.Email);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.BeEmpty();
        }

        [Fact]
        public void RelevantTodoLists_TableHasEntriesButNoneMatching_ReturnsEmptyQueryable()
        {
            // Arrange
            using var applicationDbContext = new ScopedApplicationDbContext();

            var faker   = new Faker();
            var context = applicationDbContext.Context;

            context.TodoLists.AddRange(TodoFakerManager.GetTodoListFaker().GenerateBetween(3, 5));
            context.SaveChanges();

            // Act
            // Using username will ensure the seeds do not clash from above generation
            var result = context.RelevantTodoLists(faker.Person.UserName);

            // Assert
            result.Should()
                  .NotBeNull()
                  .And.BeEmpty();
        }
    }
}