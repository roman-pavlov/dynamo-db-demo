using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDbDemo.Persistence
{
    public interface IProductReviewRepository
    {
        public Task AddAsync(ProductReviewItem reviewItem);

        Task<IEnumerable<ProductReviewItem>> GetAllAsync();
        Task<IEnumerable<ProductReviewItem>> GetUserReviewsAsync(int userId);
        Task<ProductReviewItem> GetReviewAsync(int userId, string productName);
    }
}