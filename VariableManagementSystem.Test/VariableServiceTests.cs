using Moq;
using System;
using System.Threading.Tasks;
using VariableManagementSystem.Models;
using VariableManagementSystem.Repositories;
using VariableManagementSystem.Services;
using Xunit;
using VariableManagementSystem.Enums;
using Microsoft.AspNetCore.SignalR;

namespace VariableManagementSystem.Tests
{
    public class VariableServiceTests
    {
        private readonly Mock<IVariableRepository> _mockRepository;
        private readonly Mock<IHubContext<VariableHub>> _mockHubContext; // Updated to use VariableHub
        private readonly VariableService _variableService;

        public VariableServiceTests()
        {
            // Mock the repository
            _mockRepository = new Mock<IVariableRepository>();

            // Mock the SignalR Hub context using VariableHub
            _mockHubContext = new Mock<IHubContext<VariableHub>>();

            // Mock repository methods to return completed tasks or fake data
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Variable>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Variable>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Variable());

            // Mock the SignalR Hub Clients method (assuming you want to send to all clients after operations)
            var mockClient = new Mock<IHubClients>();
            var mockClientCaller = new Mock<IClientProxy>();

            // You can simulate the behavior of the SendAsync method on IClientProxy
            mockClient.Setup(c => c.All).Returns(mockClientCaller.Object);
            _mockHubContext.Setup(h => h.Clients).Returns(mockClient.Object);

            // Initialize the service with mocked dependencies
            _variableService = new VariableService(_mockRepository.Object, _mockHubContext.Object);
        }

        [Fact]
        public async Task CreateVariableAsync_ShouldCreateVariable()
        {
            // Arrange
            var newVariable = new Variable
            {
                Id = Guid.NewGuid(),
                Identifier = "TestVar",
                Type = VariableType.String,
                Value = "TestValue",
            };

            // Act
            await _variableService.CreateVariableAsync(newVariable);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Variable>()), Times.Once);
        }

        [Fact]
        public async Task UpdateVariableAsync_ShouldUpdateVariable()
        {
            // Arrange
            var existingVariable = new Variable
            {
                Id = Guid.NewGuid(),
                Identifier = "TestVar",
                Type = VariableType.String,
                Value = "OldValue",
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingVariable);

            var newValue = "NewValue";

            // Act
            await _variableService.UpdateVariableAsync(existingVariable.Id, newValue);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Variable>(v => v.Value == newValue)), Times.Once);
        }

        [Fact]
        public async Task DeleteVariableAsync_ShouldDeleteVariable()
        {
            // Arrange
            var variableId = Guid.NewGuid();
            var existingVariable = new Variable
            {
                Id = variableId,
                Identifier = "TestVar",
                Type = VariableType.String,
                Value = "ValueToDelete",
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(variableId)).ReturnsAsync(existingVariable);

            // Act
            await _variableService.DeleteVariableAsync(variableId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(variableId), Times.Once);
        }
    }
}
