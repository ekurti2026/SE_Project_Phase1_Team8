using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Entities;
using KeyShareDL.Enums;
using KeySharePL.DTOs.ReviewDTOs;

namespace KeyShareBLL.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BasicReviewDTO GetReview(int id)
        {
            var review = _unitOfWork.Reviews.GetReviewById(id);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {id} not found.");

            return _mapper.Map<BasicReviewDTO>(review);
        }

        public List<BasicReviewDTO> GetReviewsByVehicle(int vehicleId)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            var reviews = _unitOfWork.Reviews.GetReviewsByVehicle(vehicleId);
            return _mapper.Map<List<BasicReviewDTO>>(reviews);
        }

        public List<BasicReviewDTO> GetReviewsByUser(int userId)
        {
            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var reviews = _unitOfWork.Reviews.GetReviewsByUser(userId);
            return _mapper.Map<List<BasicReviewDTO>>(reviews);
        }

        public BasicReviewDTO CreateReview(int reviewerId, CreateReviewDTO dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var booking = _unitOfWork.Bookings.GetBookingById(dto.BookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {dto.BookingId} not found.");

            if (booking.RenterId != reviewerId)
                throw new InvalidOperationException("You can only review bookings you made.");

            if (booking.Status != BookingStatus.Completed &&
                booking.Status != BookingStatus.Reviewed)
                throw new InvalidOperationException("You can only review completed bookings.");

            if (booking.Review != null)
                throw new InvalidOperationException("This booking has already been reviewed.");

            var vehicle = _unitOfWork.Vehicles.GetVehicleById(dto.VehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {dto.VehicleId} not found.");

            if (booking.VehicleId != dto.VehicleId)
                throw new ArgumentException("Vehicle ID does not match the booking.");

            var review = new Review
            {
                ReviewerId = reviewerId,
                VehicleId = dto.VehicleId,
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Reviews.AddReview(review);

            // Update booking status to Reviewed
            booking.Status = BookingStatus.Reviewed;
            _unitOfWork.Bookings.UpdateBooking(booking);

            _unitOfWork.Save();

            return _mapper.Map<BasicReviewDTO>(review);
        }

        public BasicReviewDTO UpdateReview(int reviewId, int reviewerId, UpdateReviewDTO dto)
        {
            var review = _unitOfWork.Reviews.GetReviewById(reviewId);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");

            if (review.ReviewerId != reviewerId)
                throw new InvalidOperationException("You can only edit your own reviews.");

            bool hasChanges = false;

            if (dto.Rating.HasValue)
            {
                if (dto.Rating.Value < 1 || dto.Rating.Value > 5)
                    throw new ArgumentException("Rating must be between 1 and 5.");

                review.Rating = dto.Rating.Value;
                hasChanges = true;
            }

            if (dto.Comment != null && dto.Comment != review.Comment)
            {
                review.Comment = dto.Comment;
                hasChanges = true;
            }

            if (!hasChanges)
                throw new ArgumentException("Nothing to update.");

            _unitOfWork.Reviews.UpdateReview(review);
            _unitOfWork.Save();

            return _mapper.Map<BasicReviewDTO>(review);
        }

        public BasicReviewDTO DeleteReview(int reviewId, int reviewerId)
        {
            var review = _unitOfWork.Reviews.GetReviewById(reviewId);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");

            if (review.ReviewerId != reviewerId)
                throw new InvalidOperationException("You can only delete your own reviews.");

            _unitOfWork.Reviews.DeleteReview(review);
            _unitOfWork.Save();

            return _mapper.Map<BasicReviewDTO>(review);
        }
    }
}