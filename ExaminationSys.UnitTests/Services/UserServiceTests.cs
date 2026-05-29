using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.User;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock = null!;
        private Mock<IAuthService> _authServiceMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns((User src) => new UserDto
                {
                    ID = src.Id,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Username = src.UserName
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _authServiceMock = new Mock<IAuthService>();
        }

        [Test]
        [Category("Happy")]
        public async Task IsExistAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var service = new UserService(_userManagerMock.Object, _authServiceMock.Object);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

            // Act
            var result = await service.IsExistAsync("123", CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        [Category("Happy")]
        public async Task GetByIdAsync_UserExists_ReturnsUserDto()
        {
            // Arrange
            var service = new UserService(_userManagerMock.Object, _authServiceMock.Object);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User { Id = "123", FirstName = "John" });

            // Act
            var result = await service.GetByIdAsync("123", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.FirstName.Should().Be("John");
        }

        [Test]
        [Category("Business")]
        public async Task GetByIdAsync_UserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var service = new UserService(_userManagerMock.Object, _authServiceMock.Object);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            // Act
            var result = await service.GetByIdAsync("123", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }
    }
}
