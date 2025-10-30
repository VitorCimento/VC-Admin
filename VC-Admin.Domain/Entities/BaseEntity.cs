namespace VC_Admin.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; set; }
    }
}
