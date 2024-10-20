using Rent.Service.Domain.Entities.Abstractions;
using Rent.Service.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Service.Domain.Entities
{
    [Table("delivery_persons")]
    public record DeliveryPerson : BaseEntity
    {
        #region Propriedades Públicas
        [Required]
        [Column("name")]
        public required string Name { get; init; }

        [Required]
        [Column("cnpj")]
        public required string Cnpj { get; init; }

        [Required]
        [Column("date_of_birth")]
        public required DateTime DateOfBirth { get; init; }

        [Required]
        [Column("driver_license_number")]
        public required string DriverLicenseNumber { get; init; }

        [Required]
        [Column("driver_license_category")]
        public required DriverLicenseCategory DriverLicenseCategory { get; init; }

        [Required]
        [Column("with_driver_license_image")]
        public bool WithDriverLicenseImage { get; init; } = false;
        #endregion
    }
}
