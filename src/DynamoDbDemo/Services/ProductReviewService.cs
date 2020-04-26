using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamoDbDemo.Models;
using DynamoDbDemo.Persistence;

namespace DynamoDbDemo.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository ??
                                       throw new ArgumentNullException(nameof(productReviewRepository));
        }

        public async Task AddAsync(int userId, ProductReviewRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));


            var reviewItem = ToProductReviewItem(userId, request);
            await _productReviewRepository.AddAsync(reviewItem);
        }

        public async Task<IEnumerable<ProductReviewResponse>> GetAllReviewsAsync()
        {
            var results = await _productReviewRepository.GetAllAsync();
            return ToProductReviewResponses(results);
        }

        public async Task<IEnumerable<ProductReviewResponse>> GetUserReviewsAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            var results = await _productReviewRepository.GetUserReviewsAsync(userId);
            return ToProductReviewResponses(results);
        }

        public async Task<ProductReviewResponse> GetReviewAsync(int userId, string productName)
        {
            if (productName == null) throw new ArgumentNullException(nameof(productName));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            var result = await _productReviewRepository.GetReviewAsync(userId, productName);
            return ToProductReviewResponse(result);
        }

        private static IEnumerable<ProductReviewResponse> ToProductReviewResponses(
            IEnumerable<ProductReviewItem> results)
        {
            return results?.Select(ToProductReviewResponse);
        }

        private static ProductReviewResponse ToProductReviewResponse(ProductReviewItem item)
        {
            if (item is null)
                return default;

            return new ProductReviewResponse
            {
                ProductName = item.ProductName,
                UserId = item.UserId,
                Rank = item.Rank,
                Review = item.Review,
                ReviewOn = item.ReviewOn
            };
        }

        private static ProductReviewItem ToProductReviewItem(int userId, ProductReviewRequest request)
        {
            return new ProductReviewItem
            {
                UserId = userId,
                ProductName = request.ProductName,
                Rank = request.Rank,
                Review = request.Review,
                ReviewOn = request.ReviewOn ?? DateTime.UtcNow
            };
        }
    }
}