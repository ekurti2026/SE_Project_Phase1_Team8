using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly KeyShareContext _context;

        public ReviewRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Review? GetReviewById(int id)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Vehicle)
                .Include(r => r.Booking)
                .FirstOrDefault(r => r.ReviewId == id);
        }

        public List<Review> GetReviewsByVehicle(int vehicleId)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Where(r => r.VehicleId == vehicleId)
                .ToList();
        }

        public List<Review> GetReviewsByUser(int userId)
        {
            return _context.Reviews
                .Include(r => r.Vehicle)
                .Where(r => r.ReviewerId == userId)
                .ToList();
        }

        public Review AddReview(Review review)
        {
            _context.Reviews.Add(review);
            return review;
        }

        public Review UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            return review;
        }

        public Review DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            return review;
        }
    }
}
