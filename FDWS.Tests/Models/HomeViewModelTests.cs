using FDWS.Models;
using FluentAssertions;
using System;
using Xunit;

namespace FDWS.Tests.Models
{
    public class HomeViewModelTests
    {
        [Fact]
        public void HomeViewModel_ShouldInitializeWithNullValues()
        {
            // Act
            var model = new HomeViewModel();

            // Assert
            model.Title.Should().BeNull();
            model.Message.Should().BeNull();
            model.Timestamp.Should().Be(default(DateTime));
        }

        [Fact]
        public void HomeViewModel_ShouldSetAndGetProperties()
        {
            // Arrange
            var expectedTitle = "Test Title";
            var expectedMessage = "Test Message";
            var expectedTimestamp = DateTime.Now;

            // Act
            var model = new HomeViewModel
            {
                Title = expectedTitle,
                Message = expectedMessage,
                Timestamp = expectedTimestamp
            };

            // Assert
            model.Title.Should().Be(expectedTitle);
            model.Message.Should().Be(expectedMessage);
            model.Timestamp.Should().Be(expectedTimestamp);
        }

        [Theory]
        [InlineData("Welcome", "Hello World")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void HomeViewModel_ShouldHandleDifferentStringValues(string title, string message)
        {
            // Arrange
            var timestamp = DateTime.Now;

            // Act
            var model = new HomeViewModel
            {
                Title = title,
                Message = message,
                Timestamp = timestamp
            };

            // Assert
            model.Title.Should().Be(title);
            model.Message.Should().Be(message);
            model.Timestamp.Should().Be(timestamp);
        }
    }
}