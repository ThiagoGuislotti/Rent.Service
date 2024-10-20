using NetToolsKit.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Service.Domain.Entities.Abstractions
{
    public abstract record BaseEntity : IEntity
    {
        #region Propriedades Públicas
        [Key]
        [Required]
        [Column("id")]
        public required string Id { get; init; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; init; }
        #endregion
    }
}
