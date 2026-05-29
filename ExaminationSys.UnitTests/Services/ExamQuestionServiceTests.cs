using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class ExamQuestionServiceTests
    {
        private Mock<IRepository<ExamQuestion>> _examQuestionRepoMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<ExamQuestion>(It.IsAny<AssignQuestionToExamDTO>()))
                .Returns((AssignQuestionToExamDTO src) => new ExamQuestion
                {
                    ExamId = src.ExamId,
                    QuestionId = src.QuestionId,
                    Grade = src.Grade
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _examQuestionRepoMock = new Mock<IRepository<ExamQuestion>>();
        }

        [Test]
        [Category("Happy")]
        public async Task AddAsync_ValidAssignment_ReturnsTrue()
        {
            // Arrange
            var createDto = new AssignQuestionToExamDTO { ExamId = 1, QuestionId = 1, Grade = 5 };
            var service = new ExamQuestionService(_examQuestionRepoMock.Object);

            _examQuestionRepoMock.Setup(r => r.AddAsync(It.IsAny<ExamQuestion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExamQuestion eq, CancellationToken ct) => eq);
            _examQuestionRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _examQuestionRepoMock.Verify(x => x.AddAsync(It.IsAny<ExamQuestion>(), It.IsAny<CancellationToken>()), Times.Once);
            _examQuestionRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Happy")]
        public async Task IsExist_QuestionExists_ReturnsTrue()
        {
            // Arrange
            var service = new ExamQuestionService(_examQuestionRepoMock.Object);

            _examQuestionRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<ExamQuestion, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.IsExist(1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        [Category("Business")]
        public async Task DeleteQuestionFromExam_DoesNotExist_ReturnsFalse()
        {
            // Arrange
            var service = new ExamQuestionService(_examQuestionRepoMock.Object);

            _examQuestionRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<ExamQuestion, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await service.DeleteQuestionFromExam(99, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _examQuestionRepoMock.Verify(x => x.SoftDelete(It.IsAny<ExamQuestion>()), Times.Never);
        }
    }
}
