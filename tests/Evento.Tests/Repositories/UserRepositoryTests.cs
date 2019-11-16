using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Ifrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Evento.Tests.Repositories
{
   public class UserRepositoryTests
    {
        [Fact]
        public async Task WhenAddingNewUserThanItBeAddedCorrectlyToTheList()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "user", "NewUser", "NewUSer@poczta.pl" , "tajneHasło 54%$");
            IUserRepository repo = new UserRepository();

            // Act
            await repo.AddAsync(user);
            var existingUser = await repo.GetAsync(user.Id);

            // Assert
            Assert.Equal(user, existingUser);
        }
    }
}
