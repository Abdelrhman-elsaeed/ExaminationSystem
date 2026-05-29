using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.Exam;
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
    public class ExamServiceTests
    {
        private Mock<IRepository<Exam>> _examRepoMock = null!;
        private Mock<ICourseService> _courseServiceMock = null!;
        private Mock<IInstructorService> _instructorServiceMock = null!;
        private Mock<IExamQuestionService> _examQuestionServiceMock = null!;
        private Mock<IQuestionService> _questionServiceMock = null!;
        private Mock<IExamStudentService> _examStudentServiceMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<Exam>(It.IsAny<CreateExamDTO>()))
                .Returns((CreateExamDTO src) => new Exam
                {
                    Name = src.Name,
                    CourseId = src.CourseId,
                    InstructorId = src.InstructorId
                });
                
            mockMapper.Setup(m => m.Map<ExamViewDTO>(It.IsAny<Exam>()))
                .Returns((Exam src) => new ExamViewDTO
                {
                    Name = src.Name
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _examRepoMock = new Mock<IRepository<Exam>>();
            _courseServiceMock = new Mock<ICourseService>();
            _instructorServiceMock = new Mock<IInstructorService>();
            _examQuestionServiceMock = new Mock<IExamQuestionService>();
            _questionServiceMock = new Mock<IQuestionService>();
            _examStudentServiceMock = new Mock<IExamStudentService>();
        }

        [Test]
        [Category("Happy")]
        public async Task AddAsync_ValidExam_ReturnsSuccess()
        {
            // Arrange
            var createDto = new CreateExamDTO { Name = "Midterm", CourseId = 1, InstructorId = 1 };
            var service = new ExamService(
                _examRepoMock.Object, 
                _courseServiceMock.Object, 
                _instructorServiceMock.Object, 
                _examQuestionServiceMock.Object, 
                _questionServiceMock.Object, 
                _examStudentServiceMock.Object);

            _courseServiceMock.Setup(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _instructorServiceMock.Setup(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            
            _examRepoMock.Setup(r => r.AddAsync(It.IsAny<Exam>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Exam e, CancellationToken ct) => e);
            _examRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Exam added successfully");

            _examRepoMock.Verify(x => x.AddAsync(It.IsAny<Exam>(), It.IsAny<CancellationToken>()), Times.Once);
            _examRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Business")]
        public async Task AddAsync_CourseDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var createDto = new CreateExamDTO { Name = "Midterm", CourseId = 99, InstructorId = 1 };
            var service = new ExamService(
                _examRepoMock.Object, 
                _courseServiceMock.Object, 
                _instructorServiceMock.Object, 
                _examQuestionServiceMock.Object, 
                _questionServiceMock.Object, 
                _examStudentServiceMock.Object);

            _courseServiceMock.Setup(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Course not found");

            _instructorServiceMock.Verify(s => s.IsExist(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _examRepoMock.Verify(x => x.AddAsync(It.IsAny<Exam>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
