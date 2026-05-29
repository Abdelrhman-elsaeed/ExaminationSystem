using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class ExamStudentServiceTests
    {
        private Mock<IRepository<ExamStudent>> _examStudentRepoMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<ExamStudent>(It.IsAny<CreateExamStudentDTO>()))
                .Returns((CreateExamStudentDTO src) => new ExamStudent
                {
                    ExamId = src.ExamId,
                    StudentId = src.StudentId
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _examStudentRepoMock = new Mock<IRepository<ExamStudent>>();
        }

        [Test]
        [Category("Happy")]
        public async Task AddAsync_NotAssignedBefore_ReturnsTrue()
        {
            // Arrange
            var createDto = new CreateExamStudentDTO { ExamId = 1, StudentId = 1 };
            var service = new ExamStudentService(_examStudentRepoMock.Object);

            _examStudentRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<ExamStudent, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            
            _examStudentRepoMock.Setup(r => r.AddAsync(It.IsAny<ExamStudent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExamStudent es, CancellationToken ct) => es);

            _examStudentRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _examStudentRepoMock.Verify(x => x.AddAsync(It.IsAny<ExamStudent>(), It.IsAny<CancellationToken>()), Times.Once);
            _examStudentRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Business")]
        public async Task AddAsync_AlreadyAssigned_ReturnsFalse()
        {
            // Arrange
            var createDto = new CreateExamStudentDTO { ExamId = 1, StudentId = 1 };
            var service = new ExamStudentService(_examStudentRepoMock.Object);

            _examStudentRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<ExamStudent, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _examStudentRepoMock.Verify(x => x.AddAsync(It.IsAny<ExamStudent>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        [Category("Happy")]
        public async Task IsStudentAssignedToExamAsync_Assigned_ReturnsTrue()
        {
            // Arrange
            var service = new ExamStudentService(_examStudentRepoMock.Object);

            _examStudentRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<ExamStudent, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.IsStudentAssignedToExamAsync(1, 1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
