using NetToolsKit.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Service.Domain.Entities
{
    [Table("motorcycle_events")]
    public record MotorcycleEvent : IEntity
    {
        #region Propriedades Públicas
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [Column("motorcycle_id")]
        public required string MotorcycleId { get; init; }

        [Required]
        [Column("event_type")]
        public required string EventType { get; init; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        #endregion
    }
}
