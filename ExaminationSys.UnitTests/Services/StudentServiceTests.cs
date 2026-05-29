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
    public class StudentServiceTests
    {
        private Mock<IRepository<Student>> _studentRepoMock = null!;

        [SetUp]
        public void SetUp()
        {
            _studentRepoMock = new Mock<IRepository<Student>>();
        }

        [Test]
        [Category("Happy")]
        public async Task IsExistAsync_StudentExists_ReturnsTrue()
        {
            // Arrange
            var service = new StudentService(_studentRepoMock.Object);
            
            _studentRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.IsExistAsync(1, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _studentRepoMock.Verify(x => x.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [Category("Happy")]
        public async Task IsExistAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var service = new StudentService(_studentRepoMock.Object);
            
            _studentRepoMock.Setup(r => r.CheckExistsByConditionAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await service.IsExistAsync(99, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
