using System;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Entities;

namespace Eleven.Core.ApplicationCore.Model.Domain.Entities
{
    //Extend This Class
    public class ElevenUser : Entity<Guid>
    {
        public ElevenUser()
        {
            Id = Guid.NewGuid();
        }
        public ElevenUser(Guid id) : base(id) { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }

        public int IdStatus { get; set; }
    }
}
