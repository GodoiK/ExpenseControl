using ExpenseControl.Application.Dtos.Category;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Application.Usecases.Category
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _repository;

        public CreateCategoryUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request)
        {
            var category = new ExpenseControl.Domain.Entities.Category
            {
                Id = Guid.NewGuid(),
                Description = request.Description!,
                Purpose = (CategoryPurpose)request.Purpose

            };
            await _repository.AddAsync(category);

            return new CategoryResponse(
                category.Id,
                category.Description,
                (CategoryPurpose)(int)category.Purpose
            );
        }   
    }
    public class GetAllCategoriesUseCase
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoriesUseCase(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryResponse>> ExecuteAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(c =>
                new CategoryResponse(c.Id, c.Description, (CategoryPurpose)(int)c.Purpose)
            );
        }
    }
}
