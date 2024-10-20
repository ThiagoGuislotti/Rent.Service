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
            CreateMap<Rental, RentalView>()
                .ForMember(d => d.DailyValue, s => s.MapFrom(src => Math.Round((decimal)src.DailyValue / 100, 2)));
            CreateMap<CreateRentalCommand, Rental>()
                .ReverseMap();
        }
    }
}