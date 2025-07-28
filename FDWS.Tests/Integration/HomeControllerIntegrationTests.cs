using FDWS.Controllers;
using FDWS.Models;
using FDWS.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FDWS.Tests.Integration
{
    public class HomeControllerIntegrationTests
    {
        private readonly HomeController _homeController;
        private readonly IBusinessLogic _businessLogic;
        // Use a real business logic service instead of a mock for integration testing
        public HomeControllerIntegrationTests()
        {
            // Use real business logic instead of mocks for integration testing
            _businessLogic = new BusinessLogicService();
            _homeController = new HomeController(_businessLogic);
        }

        [Fact]
        public async Task Index_WithRealBusinessLogic_ShouldReturnViewWithCorrectModel()
        {
            // Act
            var result = await _homeController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            
            viewResult.Model.Should().BeOfType<HomeViewModel>();
            var model = viewResult.Model as HomeViewModel;
            
            model.Title.Should().Be("Welcome to FDWS");
            model.Message.Should().Be("Let's calculate how many average villager dies");
            model.Timestamp.Should().BeCloseTo(System.DateTime.Now, precision: System.TimeSpan.FromMinutes(1));
        }

        [Fact]
        public async Task GetResult_WithRealBusinessLogic_ValidData_ShouldReturnSuccess()
        {
            // Arrange
            var inputData = new List<int[]>
            {
                new int[] { 65, 2020 },
                new int[] { 72, 2019 }
            };

            // Act
            var result = await _homeController.GetResult(inputData);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(true);
            messageProperty.GetValue(resultValue).Should().NotBeNull();

            // Verify the message contains a valid number (the calculated average)
            var messageValue = messageProperty.GetValue(resultValue).ToString();
            double.TryParse(messageValue, out double average).Should().BeTrue();
            average.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetResult_WithRealBusinessLogic_InvalidData_ShouldReturnError()
        {
            // Arrange
            var inputData = new List<int[]>
            {
                new int[] { 2020, 65 } // Invalid: year < age
            };

            // Act
            var result = await _homeController.GetResult(inputData);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(false);
            messageProperty.GetValue(resultValue).Should().Be("Some of the input is invalid (Year of Death must be higher than Age of Death)");
        }

        [Fact]
        public async Task GetResult_WithRealBusinessLogic_NullData_ShouldReturnError()
        {
            // Act
            var result = await _homeController.GetResult(null);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(false);
            messageProperty.GetValue(resultValue).Should().Be("No input");
        }

        [Fact]
        public async Task GetResult_WithRealBusinessLogic_EmptyData_ShouldReturnError()
        {
            // Arrange
            var inputData = new List<int[]>();

            // Act
            var result = await _homeController.GetResult(inputData);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(false);
            messageProperty.GetValue(resultValue).Should().Be("No input");
        }
    }
}