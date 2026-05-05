using AutoMapper;
using ExaminationSystem.DataBase;
using ExaminationSystem.Helper;
using ExaminationSystem.Helper.JWT;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //=================================================================
            //                              jwt  
            //=================================================================

            // 1.jwt Settings (Option Pattern)
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

            // 2.Secret Key 
            var keyBytes = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            // 3.Authentication Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,

                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

            // 4.Authorization Configuration
            builder.Services.AddAuthorization();

            //=================================================================
            //=================================================================


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

            app.UseAuthentication();
            app.UseAuthorization();

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
