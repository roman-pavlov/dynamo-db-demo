using System;
using Amazon.DynamoDBv2.DataModel;
using DynamoDbDemo.Models;

namespace DynamoDbDemo.Persistence
{
    [DynamoDBTable("ProductReview")]
    public class ProductReviewItem
    {
        [DynamoDBHashKey] public int UserId { get; set; }

        [DynamoDBRangeKey] public string ProductName { get; set; }

        public StarRank Rank { get; set; }
        public string Review { get; set; }
        public DateTime ReviewOn { get; set; }
    }
}