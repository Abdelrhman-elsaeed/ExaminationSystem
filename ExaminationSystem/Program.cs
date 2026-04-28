using AutoMapper;
using ExaminationSystem.DataBase;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.Services;

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
            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<QuestionService>();
            builder.Services.AddScoped<ChoiceService>();
            builder.Services.AddScoped<CourseService>();
            builder.Services.AddScoped<InstructorService>();
            builder.Services.AddScoped<ExamQuestionService>();
            builder.Services.AddScoped<ExamStudentService>();
            builder.Services.AddScoped<ExamService>();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            var app = builder.Build();

            //AutoMapper Configuration
            AutoMapperHelper.Mapper = app.Services.GetRequiredService<IMapper>();

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
