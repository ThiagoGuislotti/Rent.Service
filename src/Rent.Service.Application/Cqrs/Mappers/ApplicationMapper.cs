using AutoMapper;
using Rent.Service.Application.Cqrs.Commands;
using Rent.Service.Application.Cqrs.Views;
using Rent.Service.Domain.Entities;

namespace Rent.Service.Application.Cqrs.Mappers
{
    public sealed class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // DeliveryPerson
            CreateMap<DeliveryPerson, DeliveryPersonView>();
            CreateMap<CreateDeliveryPersonCommand, DeliveryPerson>()
                .ReverseMap();

            // Motorcycle
            CreateMap<Motorcycle, MotorcycleView>();
            CreateMap<CreateMotorcycleCommand, Motorcycle>()
                .ReverseMap();

            // Rental
            CreateMap<Rental, RentalView>();
            CreateMap<CreateRentalCommand, Rental>()
                .ReverseMap();
        }
    }
}