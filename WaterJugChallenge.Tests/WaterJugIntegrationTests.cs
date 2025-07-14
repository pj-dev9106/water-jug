using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using WaterJugChallenge.Models;
using Xunit;

namespace WaterJugChallenge.Tests
{
    public class WaterJugIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WaterJugIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Solve_WithValidRequest_ShouldReturnOkWithSteps()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 2,
                YCapacity = 100,
                TargetAmount = 96
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("steps");
            responseContent.Should().NotContain("error");
            
            // Verify the response structure
            var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);
            responseData.TryGetProperty("steps", out var steps).Should().BeTrue();
            steps.GetArrayLength().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Solve_WithInvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 0, // Invalid: zero capacity
                YCapacity = 5,
                TargetAmount = 4
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("error");
            responseContent.Should().Contain("Invalid input: Values must be positive integers.");
        }

        [Fact]
        public async Task Solve_WithImpossibleTarget_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 2,
                YCapacity = 6,
                TargetAmount = 5 // Impossible target
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("error");
            responseContent.Should().Contain("No solution possible");
        }

        [Fact]
        public async Task Solve_WithMalformedJson_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var incorrectJson = "{ \"XCapacity\": 3, \"YCapacity\": 5, \"TargetAmount\": 4"; // Missing closing brace
            var content = new StringContent(incorrectJson, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Solve_WithMissingProperties_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var incompleteJson = "{ \"XCapacity\": 3 }"; // Missing YCapacity and TargetAmount
            var content = new StringContent(incompleteJson, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Solve_WithNegativeTarget_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                TargetAmount = -1 // Negative target
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("error");
            responseContent.Should().Contain("Invalid input: Values must be positive integers.");
        }

        [Fact]
        public async Task Solve_WithTargetEqualToJugCapacity_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                TargetAmount = 5 // Equal to Y capacity
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("steps");
            responseContent.Should().NotContain("error");
        }

        [Fact]
        public async Task Solve_WithTargetZero_ShouldReturnEmptySteps()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                TargetAmount = 0
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("steps");
            responseContent.Should().NotContain("error");
            
            // Verify it's an empty solution (no steps needed for target 0)
            var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var steps = responseData.GetProperty("steps");
            steps.GetArrayLength().Should().Be(0);
        }

        [Fact]
        public async Task Solve_WithEmptyBody_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var noContent = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", noContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Solve_WithNullBody_ShouldReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var nullContent = new StringContent("null", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/water-jug", nullContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
} 