using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyshareDL.Entities;
using KeyShareDL.Entities;
using KeySharePL.DTOs.BookingDTOs;
using KeySharePL.DTOs.MessageDTOs;
using KeySharePL.DTOs.NotificationDTOs;
using KeySharePL.DTOs.PaymentDTOs;
using KeySharePL.DTOs.ReviewDTOs;
using KeySharePL.DTOs.UserDTOs;
using KeySharePL.DTOs.VehicleDTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KeySharePL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, BasicUserDTO>();
            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Vehicle
            CreateMap<Vehicle, BasicVehicleDTO>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()));
            CreateMap<CreateVehicleDTO, Vehicle>();

            // Booking
            CreateMap<Booking, BasicBookingDTO>()
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
                .ForMember(dest => dest.RenterName, opt => opt.MapFrom(src => src.Renter.Name));
            CreateMap<CreateBookingDTO, Booking>();

            // Review
            CreateMap<Review, BasicReviewDTO>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer.Name));
            CreateMap<CreateReviewDTO, Review>();

            // Message
            CreateMap<Message, BasicMessageDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.Name))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.Name));
            CreateMap<SendMessageDTO, Message>();

            // Payment
            CreateMap<Payment, BasicPaymentDTO>();

            // Notification
            CreateMap<Notification, BasicNotificationDTO>();
        }
    }
}
