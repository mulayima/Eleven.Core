
namespace Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Entities
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Entity<TId> other = obj as Entity<TId>;
            if (other != null)
                return Id.Equals(other.Id);
            return false;
        }
    }
}
