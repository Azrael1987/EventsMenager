using System;
using System.Threading.Tasks;
using Evento.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using Evento.Core.Repositories;

namespace Evento.Ifrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly ISet<User> _users = new HashSet<User>();
        public async Task<User> GetAsync(Guid id)
            => await Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

        public async Task<User> GetForEmailAsync(string email)
            => await Task.FromResult(_users.SingleOrDefault(x => x.Email.ToLowerInvariant() == email.ToLowerInvariant()));

        public async Task AddAsync(User @user)
        {
            _users.Add(@user);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User @user)
        {
            await Task.CompletedTask;
            //tu bedzie kiedys zapytanie SQL
        }
        public async Task DeleteAsync(User @user)
        {
            _users.Remove(@user);
            await Task.CompletedTask;
        }

    }
}