using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ExaminationSystem.BLL.Services.Implementaiton;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repo;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExaminationSys.UnitTests.Services
{
    [TestFixture]
    public class InstructorServiceTests
    {
        private Mock<IRepository<Instructor>> _instructorRepoMock = null!;

        [SetUp]
        public void SetUp()
        {
            _instructorRepoMock = new Mock<IRepository<Instructor>>();
        }

        [Test]
        [Category("Happy")]
        public async Task IsExist_InstructorExists_ReturnsTrue()
        {
            // Arrange
            var service = new InstructorService(_instructorRepoMock.Object);
            
            _instructorRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.IsExist(1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _instructorRepoMock.Verify(x => x.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Happy")]
        public async Task IsExist_InstructorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var service = new InstructorService(_instructorRepoMock.Object);
            
            _instructorRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await service.IsExist(99, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
