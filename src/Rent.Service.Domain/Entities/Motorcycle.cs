using Rent.Service.Domain.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Service.Domain.Entities
{
    [Table("motorcycles")]
    public record Motorcycle : BaseEntity
    {
        #region Propriedades Públicas
        [Required]
        [Column("year")]
        public required short Year { get; init; }

        [Required]
        [Column("model")]
        public required string Model { get; init; }

        [Required]
        [Column("license_plate")]
        public required string LicensePlate { get; init; }
        #endregion
    }
}
