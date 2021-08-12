using Bogus;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;

namespace Todo.Tests.Builders
{
    public static class TodoFakerManager
    {
        public static Faker<TodoItem> GetTodoItemFaker(IdentityUser responsibleParty = null)
        {
            responsibleParty ??= GenerateIdentityUser();
            
            return new Faker<TodoItem>()
                  .CustomInstantiator(faker => new TodoItem(faker.Random.Int(), responsibleParty.Email, faker.Lorem.Sentence(), faker.PickRandom<Importance>()))
                  .RuleFor(todoItem => todoItem.ResponsibleParty, responsibleParty);
        }

        private static IdentityUser GenerateIdentityUser()
        {
            var faker = new Faker();
            return new IdentityUser(faker.Person.Email);
        }
        
        public static Faker<TodoList> GetTodoListFaker(IdentityUser responsibleParty = null)
        {
            responsibleParty ??= GenerateIdentityUser();

            return new Faker<TodoList>()
               .CustomInstantiator(faker => new TodoList(responsibleParty, faker.Company.Bs()));
        }
    }
}