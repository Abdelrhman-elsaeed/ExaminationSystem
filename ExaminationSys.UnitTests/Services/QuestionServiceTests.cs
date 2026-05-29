using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.Question;
using ExaminationSystem.BLL.Helper.BusinessExceptions;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using ExaminationSystem.DAL.Enums.Question;
using ExaminationSystem.BLL.ViewModels;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class QuestionServiceTests
    {
        private Mock<IRepository<Question>> _questionRepoMock = null!;
        private Mock<IChoiceService> _choiceServiceMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<GetQuestionDTO>(It.IsAny<Question>()))
                .Returns((Question src) => new GetQuestionDTO
                {
                    ID = src.ID,
                    Title = src.Title,
                    Level = src.Level
                });
            
            mockMapper.Setup(m => m.Map<Question>(It.IsAny<CreateQuestionDTO>()))
                .Returns((CreateQuestionDTO src) => new Question
                {
                    Title = src.Title,
                    Level = src.Level,
                    CourseId = src.CourseId
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _questionRepoMock = new Mock<IRepository<Question>>();
            _choiceServiceMock = new Mock<IChoiceService>();
        }

        [Test]
        [Category("Happy")]
        public async Task AddAsync_ValidQuestion_ReturnsSuccessResponse()
        {
            // Arrange
            var createDto = new CreateQuestionDTO { Title = "What is C#?", Level = QuestionLevel.Easy, CourseId = 1 };
            var service = new QuestionService(_questionRepoMock.Object, _choiceServiceMock.Object);

            _questionRepoMock.Setup(r => r.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Question q, CancellationToken c) => q);
            _questionRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Question Added Successfully");
            result.Data.Should().NotBeNull();
            result.Data!.Title.Should().Be(createDto.Title);

            _questionRepoMock.Verify(x => x.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()), Times.Once);
            _questionRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Business")]
        public async Task AddAsync_SaveFails_ReturnsFailureResponse()
        {
            // Arrange
            var createDto = new CreateQuestionDTO { Title = "What is C#?", Level = QuestionLevel.Easy, CourseId = 1 };
            var service = new QuestionService(_questionRepoMock.Object, _choiceServiceMock.Object);

            _questionRepoMock.Setup(r => r.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Question q, CancellationToken c) => q);
            _questionRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await service.AddAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Failed to save the question. Please try again");
            result.Data.Should().BeNull();
        }

        [Test]
        [Category("Business")]
        public async Task DeleteQuestionAndChoicesAsync_QuestionNotFound_ThrowsBusinessException()
        {
            // Arrange
            var service = new QuestionService(_questionRepoMock.Object, _choiceServiceMock.Object);

            _questionRepoMock.Setup(r => r.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Question?)null);

            // Act
            var act = async () => await service.DeleteQuestionAndChoicesAsync(99, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Failed to delete Question");
        }
    }
}
