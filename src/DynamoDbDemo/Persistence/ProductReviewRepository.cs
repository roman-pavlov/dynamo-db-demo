using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbDemo.Persistence
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly DynamoDBContext _context;

        public ProductReviewRepository(IAmazonDynamoDB dynamoDbClient)
        {
            if (dynamoDbClient == null) throw new ArgumentNullException(nameof(dynamoDbClient));
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task AddAsync(ProductReviewItem reviewItem)
        {
            await _context.SaveAsync(reviewItem);
        }

        public async Task<IEnumerable<ProductReviewItem>> GetAllAsync()
        {
            return await _context.ScanAsync<ProductReviewItem>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<IEnumerable<ProductReviewItem>> GetUserReviewsAsync(int userId)
        {
            return await _context.QueryAsync<ProductReviewItem>(userId).GetRemainingAsync();
        }

        public async Task<ProductReviewItem> GetReviewAsync(int userId, string productName)
        {
            return await _context.LoadAsync<ProductReviewItem>(userId, productName);
        }
    }
}