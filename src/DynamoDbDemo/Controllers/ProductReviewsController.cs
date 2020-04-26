using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamoDbDemo.Models;
using DynamoDbDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamoDbDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductReviewsController : ControllerBase
    {
        private readonly ILogger<ProductReviewsController> _logger;
        private readonly IProductReviewService _productReviewService;

        public ProductReviewsController(ILogger<ProductReviewsController> logger,
            IProductReviewService productReviewService)
        {
            _logger = logger;
            _productReviewService = productReviewService;
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> AddProductReview(int userId, ProductReviewRequest request)
        {
            await _productReviewService.AddAsync(userId, request);

            return Created(
                Url.Link("GetUserProductReview", new
                {
                    userId,
                    productName = request.ProductName
                }), null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductReviews()
        {
            var reviews = await _productReviewService.GetAllReviewsAsync();
            return ToActionResult(reviews);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserProductReviews(int userId)
        {
            var reviews = await _productReviewService.GetUserReviewsAsync(userId);
            return ToActionResult(reviews);
        }

        [HttpGet]
        [Route("{userId}/{productName}", Name = "GetUserProductReview")]
        public async Task<IActionResult> GetUserProductReview(int userId, string productName)
        {
            var result = await _productReviewService.GetReviewAsync(userId, productName);
            return result is null ? (IActionResult) NotFound(null) : Ok(result);
        }

        private IActionResult ToActionResult(IEnumerable<ProductReviewResponse> reviews)
        {
            return reviews.Any() ? Ok(reviews) : (IActionResult) NotFound(null);
        }
    }
}