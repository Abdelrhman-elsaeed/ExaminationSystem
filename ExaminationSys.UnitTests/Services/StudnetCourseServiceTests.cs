using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.StudentCourse;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class StudnetCourseServiceTests
    {
        private Mock<IRepository<StudentCourse>> _studentCourseRepoMock = null!;
        private Mock<IStudentService> _studentServiceMock = null!;
        private Mock<ICourseService> _courseServiceMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<StudentCourse>(It.IsAny<AssignStudentToCourseDTO>()))
                .Returns((AssignStudentToCourseDTO src) => new StudentCourse
                {
                    StudentID = src.StudentID,
                    CourseID = src.CourseID
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _studentCourseRepoMock = new Mock<IRepository<StudentCourse>>();
            _studentServiceMock = new Mock<IStudentService>();
            _courseServiceMock = new Mock<ICourseService>();
        }

        [Test]
        [Category("Happy")]
        public async Task AssignStudentToCourseAsync_ValidAssignment_ReturnsSuccess()
        {
            // Arrange
            var createDto = new AssignStudentToCourseDTO { StudentID = 1, CourseID = 1 };
            var service = new StudnetCourseService(_studentCourseRepoMock.Object, _studentServiceMock.Object, _courseServiceMock.Object);

            _studentServiceMock.Setup(s => s.IsExistAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _courseServiceMock.Setup(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _studentCourseRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<StudentCourse, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            
            _studentCourseRepoMock.Setup(r => r.AddAsync(It.IsAny<StudentCourse>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((StudentCourse sc, CancellationToken ct) => sc);
            _studentCourseRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await service.AssignStudentToCourseAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Student assigned to course successfully");

            _studentCourseRepoMock.Verify(x => x.AddAsync(It.IsAny<StudentCourse>(), It.IsAny<CancellationToken>()), Times.Once);
            _studentCourseRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Business")]
        public async Task AssignStudentToCourseAsync_StudentDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var createDto = new AssignStudentToCourseDTO { StudentID = 99, CourseID = 1 };
            var service = new StudnetCourseService(_studentCourseRepoMock.Object, _studentServiceMock.Object, _courseServiceMock.Object);

            _studentServiceMock.Setup(s => s.IsExistAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await service.AssignStudentToCourseAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Student not found!");
            
            _courseServiceMock.Verify(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _studentCourseRepoMock.Verify(x => x.AddAsync(It.IsAny<StudentCourse>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        [Category("Business")]
        public async Task AssignStudentToCourseAsync_AlreadyAssigned_ReturnsFailure()
        {
            // Arrange
            var createDto = new AssignStudentToCourseDTO { StudentID = 1, CourseID = 1 };
            var service = new StudnetCourseService(_studentCourseRepoMock.Object, _studentServiceMock.Object, _courseServiceMock.Object);

            _studentServiceMock.Setup(s => s.IsExistAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _courseServiceMock.Setup(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _studentCourseRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<StudentCourse, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await service.AssignStudentToCourseAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Student assigned before tho this course");
            
            _studentCourseRepoMock.Verify(x => x.AddAsync(It.IsAny<StudentCourse>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
