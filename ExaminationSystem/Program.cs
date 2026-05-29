using ExaminationSystem.BLL.AutoMapper.Profiles;
using ExaminationSystem.BLL.Helper.JWT;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Implementaiton;

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

            // **JWT Configuration**
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<Context>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;

                // Token Validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            // Register Context
            builder.Services.AddDbContext<Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //dependency injection
            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IChoiceService, ChoiceService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IExamQuestionService, ExamQuestionService>();
            builder.Services.AddScoped<IExamStudentService, ExamStudentService>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IStudnetCourseService, StudnetCourseService>();
            builder.Services.AddScoped<User>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // We use a specific type 'AuthProfile' to get a reference to the BLL Assembly.
            // This registers all AutoMapper profiles inside the BLL layer in a single scan.
            // Architectural Trade-off: This approach avoids the performance overhead and 
            // lazy-loading bugs caused by scanning all loaded assemblies using 'AppDomain.CurrentDomain.GetAssemblies()'.
            builder.Services.AddAutoMapper(typeof(AuthProfile).Assembly);

            // Error Handler
            builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();
            builder.Services.AddScoped<TransactionMiddleware>();


            var app = builder.Build();

            // Error Handler Middleware
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            // Transaction Middleware
            app.UseMiddleware<TransactionMiddleware>();

            //AutoMapper Configuration
            AutoMapperHelper.Mapper = app.Services.GetRequiredService<IMapper>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
