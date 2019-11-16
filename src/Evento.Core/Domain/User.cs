using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Evento.Core.Domain
{
    public class User : Entity
    {
        private static List<string> _roles = new List<string>
        {
            "user",
            "admin",
            "moderator"
        };

        public string Role { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public DateTime CreatesAt { get; protected set; }

        protected User() { }

        public User(Guid id, string role, string name, string email, string password)
        {
            Id = id;
            Role = role;
            SetName(name);
            SetEmail(email);
            SetPassword(password);
            CreatesAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"You can not have an empty name.");
            }
            Name = name;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception($"You can not have an empty email.");
            }
            else if (email.Contains("@") && email.Contains("."))
            {
                Email = email;
            }
            else
            {
                throw new Exception($"Email need have a special signs '@' and '.'");
            }
        }
        public void SeTRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new Exception($"User can not have empty role");
            }
            role = role.ToLowerInvariant();
            if (!_roles.Contains(role))
            {
                throw new Exception($"User can not have role '{role}'");
            }
            Role = role;
        }

        public void SetPassword(string password)
        {
            var regex = new Regex("[^a-zA-Z0-9]*$");
            Match matchForSpecialSigns = regex.Match(password);

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"You must set password");
            }
            else if (password.Length <= 8 && password.Length > 25)
            {
                throw new Exception($"Your password must have number of signs beeteen 8 and 24");
            }
            else if (!matchForSpecialSigns.Success)
            {
                throw new Exception($"Your password must have special sign");
            }
            Password = password;
        }
    }
}