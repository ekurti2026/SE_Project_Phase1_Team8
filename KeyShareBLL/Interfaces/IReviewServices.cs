using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.ReviewDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface IReviewServices
    {
        BasicReviewDTO GetReview(int id);
        List<BasicReviewDTO> GetReviewsByVehicle(int vehicleId);
        List<BasicReviewDTO> GetReviewsByUser(int userId);
        BasicReviewDTO CreateReview(int reviewerId, CreateReviewDTO dto);
        BasicReviewDTO UpdateReview(int reviewId, int reviewerId, UpdateReviewDTO dto);
        BasicReviewDTO DeleteReview(int reviewId, int reviewerId);
    }
}