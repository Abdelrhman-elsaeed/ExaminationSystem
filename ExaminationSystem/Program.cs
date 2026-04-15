
using ExaminationSystem.DataBase;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.Repo.RepositoryExtension;

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //dependency injection
            builder.Services.AddScoped<GenericRepository<Course>>();
            builder.Services.AddScoped<GenericRepository<Question>>();
            builder.Services.AddScoped<GenericRepository<Choice>>();
            builder.Services.AddScoped<QuestionRepo>();
            builder.Services.AddScoped<Context>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
