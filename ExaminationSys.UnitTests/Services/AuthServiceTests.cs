using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.Auth;
using ExaminationSystem.BLL.Helper.JWT;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock = null!;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock = null!;
        private IOptions<JWT> _jwtOptions = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<User>(It.IsAny<RegisterDto>()))
                .Returns((RegisterDto src) => new User
                {
                    UserName = src.Username,
                    Email = src.Email
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            _jwtOptions = Options.Create(new JWT
            {
                Key = "super_secret_key_that_is_at_least_32_bytes_long_for_hmac_sha256",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                DurationInDays = 1
            });
        }

        [Test]
        [Category("Business")]
        public async Task RegisterAsync_EmailAlreadyRegistered_ReturnsFailure()
        {
            // Arrange
            var service = new AuthService(_userManagerMock.Object, _jwtOptions, _roleManagerMock.Object);
            var model = new RegisterDto { Email = "test@test.com", Username = "testuser", Password = "Password1!" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(new User());

            // Act
            var result = await service.RegisterAsync(model, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email is already registered");
        }

        [Test]
        [Category("Happy")]
        public async Task GetTokenAsync_ValidCredentials_ReturnsAuthDto()
        {
            // Arrange
            var service = new AuthService(_userManagerMock.Object, _jwtOptions, _roleManagerMock.Object);
            var model = new TokenRequestDto { Email = "test@test.com", Password = "Password1!" };
            var user = new User { Id = "123", UserName = "testuser", Email = "test@test.com" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(new List<System.Security.Claims.Claim>());
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Student" });

            // Act
            var result = await service.GetTokenAsync(model, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Username.Should().Be("testuser");
            result.Data.Email.Should().Be("test@test.com");
            result.Data.Token.Should().NotBeNullOrEmpty();
        }

        [Test]
        [Category("Business")]
        public async Task GetTokenAsync_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var service = new AuthService(_userManagerMock.Object, _jwtOptions, _roleManagerMock.Object);
            var model = new TokenRequestDto { Email = "test@test.com", Password = "WrongPassword!" };
            var user = new User { Id = "123", UserName = "testuser", Email = "test@test.com" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(false);

            // Act
            var result = await service.GetTokenAsync(model, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid Email or Password");
        }
    }
}
