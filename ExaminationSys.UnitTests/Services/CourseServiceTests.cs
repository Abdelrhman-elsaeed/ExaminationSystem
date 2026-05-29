using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.DTOs.Common;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ExaminationSystem.BLL.ViewModels;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class CourseServiceTests
    {
        private Mock<IRepository<Course>> _courseRepoMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<Course>(It.IsAny<CreateCourseDTO>()))
                .Returns((CreateCourseDTO src) => new Course
                {
                    Name = src.Name,
                    Description = src.Description,
                    Hours = src.Hours
                });

            mockMapper.Setup(m => m.Map<GetCourseDTO>(It.IsAny<Course>()))
                .Returns((Course src) => new GetCourseDTO
                {
                    ID = src.ID,
                    Name = src.Name,
                    Description = src.Description,
                    Hours = src.Hours
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _courseRepoMock = new Mock<IRepository<Course>>();
        }

        [Test]
        [Category("Happy")]
        public async Task AddCourseAsync_ValidCourse_ReturnsSuccess()
        {
            // Arrange
            var createDto = new CreateCourseDTO { Name = "C# Basics", Description = "Learn C#", Hours = 40 };
            var service = new CourseService(_courseRepoMock.Object);

            _courseRepoMock.Setup(r => r.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Course c, CancellationToken ct) => c);
            _courseRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddCourseAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Course added successfully");
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be(createDto.Name);

            _courseRepoMock.Verify(x => x.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()), Times.Once);
            _courseRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Business")]
        public async Task AddCourseAsync_SaveFails_ReturnsFailure()
        {
            // Arrange
            var createDto = new CreateCourseDTO { Name = "C# Basics", Description = "Learn C#", Hours = 40 };
            var service = new CourseService(_courseRepoMock.Object);

            _courseRepoMock.Setup(r => r.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Course c, CancellationToken ct) => c);
            _courseRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await service.AddCourseAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Fail to add Course!");
        }

        [Test]
        [Category("Happy")]
        public async Task GetCourseByIdAsync_CourseExists_ReturnsCourse()
        {
            // Arrange
            var service = new CourseService(_courseRepoMock.Object);
            var course = new Course { ID = 1, Name = "C# Basics", Description = "Learn C#", Hours = 40 };

            _courseRepoMock.Setup(r => r.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(course);

            // Act
            var result = await service.GetCourseByIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.ID.Should().Be(1);
        }

        [Test]
        [Category("Business")]
        public async Task GetCourseByIdAsync_CourseDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var service = new CourseService(_courseRepoMock.Object);

            _courseRepoMock.Setup(r => r.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Course?)null);

            // Act
            var result = await service.GetCourseByIdAsync(99, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Course not found!");
            result.Data.Should().BeNull();
        }
    }
}
