using FDWS.Models;
using FluentAssertions;
using System;
using System.Net;
using Xunit;

namespace FDWS.Tests.Models
{
    public class ResponseObjectTests
    {
        [Fact]
        public void ResponseObject_ShouldInitializeWithNullValues()
        {
            // Act
            var response = new ResponseObject();

            // Assert
            response.Result.Should().BeNull();
            response.Status.Should().BeNull();
        }

        [Fact]
        public void ResponseObject_ShouldSetAndGetProperties()
        {
            // Arrange
            var expectedResult = "Test result";
            var expectedStatus = HttpStatusCode.OK.ToString();

            // Act
            var response = new ResponseObject
            {
                Result = expectedResult,
                Status = expectedStatus
            };

            // Assert
            response.Result.Should().Be(expectedResult);
            response.Status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData("Success", "OK")]
        [InlineData("Error", "BadRequest")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ResponseObject_ShouldHandleDifferentStringValues(string result, string status)
        {
            // Act
            var response = new ResponseObject
            {
                Result = result,
                Status = status
            };

            // Assert
            response.Result.Should().Be(result);
            response.Status.Should().Be(status);
        }
    }
}
