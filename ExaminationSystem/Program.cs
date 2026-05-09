using ExaminationSystem.BLL.AutoMapper.Profiles;
using ExaminationSystem.Middlewares;

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

                    ClockSkew = TimeSpan.Zero,

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
            builder.Services.AddScoped<TokenGenerator>();
            builder.Services.AddScoped<RoleFeature>();
            builder.Services.AddScoped<RoleFeatureService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<User>();

            // We use a specific type 'AuthProfile' to get a reference to the BLL Assembly.
            // This registers all AutoMapper profiles inside the BLL layer in a single scan.
            // Architectural Trade-off: This approach avoids the performance overhead and 
            // lazy-loading bugs caused by scanning all loaded assemblies using 'AppDomain.CurrentDomain.GetAssemblies()'.
            builder.Services.AddAutoMapper(typeof(AuthProfile).Assembly);

            // Error Handler
            builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();

            var app = builder.Build();

            // Error Handler Middleware
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();


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
