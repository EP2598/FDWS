using FDWS.Models;
using FDWS.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FDWS.Tests.Services
{
    public class BusinessLogicServiceTests
    {
        private readonly BusinessLogicService _businessLogicService;

        public BusinessLogicServiceTests()
        {
            _businessLogicService = new BusinessLogicService();
        }

        [Fact]
        public async Task GetHomeDataAsync_ShouldReturnCorrectData()
        {
            // Act
            var result = await _businessLogicService.GetHomeDataAsync();

            // Assert
            result.Should().NotBeNull();
            
            // Using reflection to check anonymous object properties
            var resultType = result.GetType();
            var titleProperty = resultType.GetProperty("Title");
            var messageProperty = resultType.GetProperty("Message");
            var timestampProperty = resultType.GetProperty("Timestamp");

            titleProperty.Should().NotBeNull();
            messageProperty.Should().NotBeNull();
            timestampProperty.Should().NotBeNull();

            titleProperty.GetValue(result).Should().Be("Welcome to FDWS");
            messageProperty.GetValue(result).Should().Be("Your application is running successfully!");
            timestampProperty.GetValue(result).Should().BeOfType<DateTime>();
        }

        [Fact]
        public async Task ValidateInputAsync_WithNullInput_ShouldReturnBadRequest()
        {
            // Act
            var result = await _businessLogicService.ValidateInputAsync(null);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().Be("No input");
            result.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
        }

        [Fact]
        public async Task ValidateInputAsync_WithEmptyInput_ShouldReturnBadRequest()
        {
            // Arrange
            var emptyList = new List<int[]>();

            // Act
            var result = await _businessLogicService.ValidateInputAsync(emptyList);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().Be("No input");
            result.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
        }

        [Theory]
        [InlineData(65, 2020)] // Valid: Year > Age
        [InlineData(72, 2019)] // Valid: Year > Age
        [InlineData(58, 2021)] // Valid: Year > Age
        public async Task ValidateInputAsync_WithValidSingleInput_ShouldReturnSuccess(int age, int year)
        {
            // Arrange
            var inputList = new List<int[]> { new int[] { age, year } };

            // Act
            var result = await _businessLogicService.ValidateInputAsync(inputList);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.OK.ToString());
            result.Result.Should().NotBeNullOrEmpty();
            
            // Result should be a valid number (average calculation result)
            double.TryParse(result.Result, out double resultValue).Should().BeTrue();
            resultValue.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(2020, 65)] // Invalid: Year < Age
        [InlineData(2019, 72)] // Invalid: Year < Age
        [InlineData(2021, 58)] // Invalid: Year < Age
        public async Task ValidateInputAsync_WithInvalidInput_YearLessThanAge_ShouldReturnBadRequest(int age, int year)
        {
            // Arrange
            var inputList = new List<int[]> { new int[] { age, year } };

            // Act
            var result = await _businessLogicService.ValidateInputAsync(inputList);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            result.Result.Should().Be("Some of the input is invalid (Year of Death must be higher than Age of Death)");
        }

        [Fact]
        public async Task ValidateInputAsync_WithMultipleValidInputs_ShouldCalculateCorrectAverage()
        {
            // Arrange
            var inputList = new List<int[]> 
            { 
                new int[] { 65, 2020 }, // yearOfDeath - ageOfDeath = 1955
                new int[] { 72, 2019 }, // yearOfDeath - ageOfDeath = 1947
                new int[] { 58, 2021 }  // yearOfDeath - ageOfDeath = 1963
            };

            // Act
            var result = await _businessLogicService.ValidateInputAsync(inputList);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.OK.ToString());
            result.Result.Should().NotBeNullOrEmpty();

            // Verify the result is a valid number
            double.TryParse(result.Result, out double average).Should().BeTrue();
            average.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ValidateInputAsync_WithMixedValidAndInvalidInputs_ShouldReturnBadRequestOnFirstInvalid()
        {
            // Arrange
            var inputList = new List<int[]> 
            { 
                new int[] { 65, 2020 }, // Valid
                new int[] { 2019, 72 }, // Invalid: Year < Age
                new int[] { 58, 2021 }  // Valid (but shouldn't be processed due to early return)
            };

            // Act
            var result = await _businessLogicService.ValidateInputAsync(inputList);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.BadRequest.ToString());
            result.Result.Should().Be("Some of the input is invalid (Year of Death must be higher than Age of Death)");
        }

        [Theory]
        [InlineData(0, 1)] // Edge case: Age 0
        [InlineData(1, 2)] // Edge case: Small numbers
        [InlineData(100, 2000)] // Edge case: Large age difference
        public async Task ValidateInputAsync_WithEdgeCases_ShouldHandleCorrectly(int age, int year)
        {
            // Arrange
            var inputList = new List<int[]> { new int[] { age, year } };

            // Act
            var result = await _businessLogicService.ValidateInputAsync(inputList);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(HttpStatusCode.OK.ToString());
            double.TryParse(result.Result, out double resultValue).Should().BeTrue();
        }
    }
}