using Microsoft.AspNetCore.Http;
using ExpenseControl.Application.Interfaces.Repositories;
using ExpenseControl.Application.UseCase.Users;
using ExpenseControl.Application.Usecases.Category;
using ExpenseControl.Application.Usecases.FinancialRecords;
using ExpenseControl.Application.UseCases;
using ExpenseControl.Application.UseCases.FinancialRecord;
using ExpenseControl.Application.UseCases.Users;
using ExpenseControl.Application.UseCases.FinancialRecords;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Infrastructure.Data;
using ExpenseControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFinancialRecordRepository, FinancialRecordRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetUserSummaryUseCase>();

builder.Services.AddScoped<CreateFinancialRecordUseCase>();
builder.Services.AddScoped<GetAllFinancialRecordsUseCase>();
builder.Services.AddScoped<GetFinancialRecordsByUserUseCase>();

builder.Services.AddScoped<CreateCategoryUseCase>();
builder.Services.AddScoped<GetAllCategoriesUseCase>();
builder.Services.AddScoped<GetCategorySummaryUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Front");

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ExpenseControl.Domain.Exceptions.BusinessException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();