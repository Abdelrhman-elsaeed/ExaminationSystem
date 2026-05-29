using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.DTOs.Choice;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class ChoiceServiceTests
    {
        private Mock<IRepository<Choice>> _choiceRepoMock = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<Choice>(It.IsAny<UpdateChoiceDTO>()))
                .Returns((UpdateChoiceDTO src) => new Choice
                {
                    ID = src.ID,
                    Text = src.Text,
                    IsCorrectChoice = src.IsCorrectChoice
                });

            AutoMapperHelper.Mapper = mockMapper.Object;
        }

        [SetUp]
        public void SetUp()
        {
            _choiceRepoMock = new Mock<IRepository<Choice>>();
        }

        [Test]
        [Category("Happy")]
        public async Task DeleteByQuestionIdAsync_ChoicesFound_ReturnsTrue()
        {
            // Arrange
            var service = new ChoiceService(_choiceRepoMock.Object);
            var choices = new List<Choice> { new Choice { ID = 1 }, new Choice { ID = 2 } };

            _choiceRepoMock.Setup(r => r.GetAllByConditionAsync(It.IsAny<Expression<Func<Choice, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(choices);
            _choiceRepoMock.Setup(r => r.DeleteRange(choices));
            _choiceRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.DeleteByQuestionIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _choiceRepoMock.Verify(x => x.DeleteRange(choices), Times.Once);
            _choiceRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Happy")]
        public async Task DeleteByQuestionIdAsync_ChoicesNotFound_ReturnsFalse()
        {
            // Arrange
            var service = new ChoiceService(_choiceRepoMock.Object);

            _choiceRepoMock.Setup(r => r.GetAllByConditionAsync(It.IsAny<Expression<Func<Choice, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Choice>());

            // Act
            var result = await service.DeleteByQuestionIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _choiceRepoMock.Verify(x => x.DeleteRange(It.IsAny<IEnumerable<Choice>>()), Times.Never);
        }

        [Test]
        [Category("Happy")]
        public async Task UpdateChoiceAsync_UpdatesAndSaves_ReturnsResultOfSave()
        {
            // Arrange
            var service = new ChoiceService(_choiceRepoMock.Object);
            var updateDto = new UpdateChoiceDTO { ID = 1, Text = "New Text", IsCorrectChoice = true };

            _choiceRepoMock.Setup(r => r.UpdateInclude(It.IsAny<Choice>(), It.IsAny<string[]>()));
            _choiceRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.UpdateChoiceAsync(updateDto, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _choiceRepoMock.Verify(x => x.UpdateInclude(It.IsAny<Choice>(), nameof(Choice.Text), nameof(Choice.IsCorrectChoice)), Times.Once);
            _choiceRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Happy")]
        public async Task AnyAsync_ChoiceExists_ReturnsTrue()
        {
            // Arrange
            var service = new ChoiceService(_choiceRepoMock.Object);

            _choiceRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Choice, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AnyAsync(1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
