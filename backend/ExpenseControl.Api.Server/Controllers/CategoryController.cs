using ExpenseControl.Application.Dtos.Category;
using ExpenseControl.Application.Usecases.Category;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Server.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CreateCategoryUseCase _createUseCase;
        private readonly GetAllCategoriesUseCase _getAllUseCase;
        private readonly GetCategorySummaryUseCase _summaryUseCase;

        public CategoryController(
            CreateCategoryUseCase createUseCase,
            GetAllCategoriesUseCase getAllUseCase,
            GetCategorySummaryUseCase summaryUseCase)
        {
            _createUseCase = createUseCase;
            _getAllUseCase = getAllUseCase;
            _summaryUseCase = summaryUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var result = await _createUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllUseCase.ExecuteAsync();
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _summaryUseCase.ExecuteAsync();
            return Ok(result);
        }
    }
}
