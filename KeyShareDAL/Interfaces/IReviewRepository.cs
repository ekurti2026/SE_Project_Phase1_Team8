using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface IReviewRepository
    {
        Review? GetReviewById(int id);
        List<Review> GetReviewsByVehicle(int vehicleId);
        List<Review> GetReviewsByUser(int userId);
        Review AddReview(Review review);
        Review UpdateReview(Review review);
        Review DeleteReview(Review review);
    }
}
