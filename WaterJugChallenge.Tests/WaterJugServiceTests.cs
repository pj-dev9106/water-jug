using FluentAssertions;
using WaterJugChallenge.Models;
using WaterJugChallenge.Services;
using Xunit;

namespace WaterJugChallenge.Tests
{
    public class WaterJugServiceTests
    {
        private readonly WaterJugService _service;

        public WaterJugServiceTests()
        {
            _service = new WaterJugService();
        }

        [Fact]
        public void Solution_WithValidInput_ShouldReturnSteps()
        {
            // Arrange
            int xCapacity = 2;
            int yCapacity = 100;
            int targetAmount = 96;

            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            error.Should().BeNull();
            steps.Should().NotBeNull();
            steps.Should().NotBeEmpty();

            // Verify the solution reaches the target
            var lastStep = steps[steps.Count - 1];
            (lastStep.XAmount == targetAmount || lastStep.YAmount == targetAmount).Should().BeTrue();

            steps.Count.Should().Be(4);
        }

        [Theory]
        [InlineData(0, 5, 3, "Invalid input: Values must be positive integers.")]
        [InlineData(3, 0, 3, "Invalid input: Values must be positive integers.")]
        [InlineData(-1, 5, 3, "Invalid input: Values must be positive integers.")]
        [InlineData(3, -5, 3, "Invalid input: Values must be positive integers.")]
        public void Solution_WithInvalidInput_ShouldReturnError(int xCapacity, int yCapacity, int targetAmount, string expectedError)
        {
            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            steps.Should().BeNull();
            error.Should().Be(expectedError);
        }

        [Theory]
        [InlineData(3, 5, 7, "No solution possible.")]
        [InlineData(2, 6, 5, "No solution possible.")]
        public void Solution_WithImpossibleTarget_ShouldReturnError(int xCapacity, int yCapacity, int targetAmount, string expectedError)
        {
            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            steps.Should().BeNull();
            error.Should().Be(expectedError);
        }

        [Fact]
        public void Solution_WithCoprimeJugs_ShouldFindSolution()
        {
            // Arrange - 3 and 5 are coprime, so any target up to 5 should be possible
            int xCapacity = 3;
            int yCapacity = 5;
            int targetAmount = 4;

            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            error.Should().BeNull();
            steps.Should().NotBeNull();
            steps.Should().NotBeEmpty();
            
            // Verify the solution reaches the target
            var lastStep = steps[steps.Count - 1];
            (lastStep.XAmount == targetAmount || lastStep.YAmount == targetAmount).Should().BeTrue();
        }

        [Fact]
        public void Solution_WithTargetZero_ShouldReturnEmptySolution()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int targetAmount = 0;

            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            error.Should().BeNull();
            steps.Should().NotBeNull();
            steps.Should().BeEmpty(); // No steps needed for target 0
        }

        [Fact]
        public void Solution_StepsShouldBeValidSequence()
        {
            // Arrange
            int xCapacity = 2;
            int yCapacity = 100;
            int targetAmount = 96;

            // Act
            var (steps, error) = _service.Solution(xCapacity, yCapacity, targetAmount);

            // Assert
            error.Should().BeNull();
            steps.Should().NotBeNull();
            steps.Should().NotBeEmpty();

            // Verify each step is valid
            for (int i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                step.XAmount.Should().BeGreaterThanOrEqualTo(0);
                step.YAmount.Should().BeGreaterThanOrEqualTo(0);
                step.XAmount.Should().BeLessThanOrEqualTo(xCapacity);
                step.YAmount.Should().BeLessThanOrEqualTo(yCapacity);
                step.Action.Should().NotBeNullOrEmpty();
            }
        }
    }
} 