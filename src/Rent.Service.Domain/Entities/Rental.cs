using Rent.Service.Domain.Entities.Abstractions;
using Rent.Service.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Service.Domain.Entities
{
    [Table("rentals")]
    public record Rental : BaseEntity
    {
        #region Public Properties
        [Required]
        [Column("delivery_person_id")]
        public required string DeliveryPersonId { get; init; }

        [Required]
        [Column("motorcycle_id")]
        public required string MotorcycleId { get; init; }

        [Required]
        [Column("start_date")]
        public required DateTimeOffset StartDate { get; init; }

        [Required]
        [Column("end_date")]
        public required DateTimeOffset EndDate { get; init; }

        [Required]
        [Column("expected_end_date")]
        public required DateTimeOffset ExpectedEndDate { get; init; }

        [Required]
        [Column("plan")]
        public required RentalPlanType Plan { get; init; }

        [Required]
        [Column("daily_value")]
        public required int DailyValue { get; init; }

        [Column("return_date")]
        public DateTimeOffset? ReturnDate { get; init; }
        #endregion
    }
}