using System;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Entities;

namespace Eleven.Core.ApplicationCore.Model.Domain.Entities
{
    public class ApplicationUser : Entity<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        public ApplicationUser(Guid id) : base(id) { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LoginName { get; set; }

        public string Email { get; set; }

        public Guid IdStatus { get; set; }

        public Guid IdOwner { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }

    }
}
