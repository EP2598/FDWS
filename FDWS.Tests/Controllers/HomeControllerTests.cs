using FDWS.Controllers;
using FDWS.Models;
using FDWS.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FDWS.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IBusinessLogic> _mockBusinessLogic;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            _mockBusinessLogic = new Mock<IBusinessLogic>();
            _homeController = new HomeController(_mockBusinessLogic.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithCorrectModel()
        {
            // Arrange
            var expectedData = new
            {
                Title = "Welcome to FDWS",
                Message = "Your application is running successfully!",
                Timestamp = System.DateTime.Now
            };
            _mockBusinessLogic.Setup(x => x.GetHomeDataAsync()).ReturnsAsync(expectedData);

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

            // Verify the business logic was called
            _mockBusinessLogic.Verify(x => x.GetHomeDataAsync(), Times.Once);
        }

        [Fact]
        public async Task GetResult_WithValidInput_ShouldReturnSuccessJson()
        {
            // Arrange
            var inputList = new List<int[]> { new int[] { 65, 2020 } };
            var mockResponse = new ResponseObject
            {
                Result = "15.5",
                Status = HttpStatusCode.OK.ToString()
            };
            _mockBusinessLogic.Setup(x => x.ValidateInputAsync(inputList)).ReturnsAsync(mockResponse);

            // Act
            var result = await _homeController.GetResult(inputList);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(true);
            messageProperty.GetValue(resultValue).Should().Be("15.5");

            _mockBusinessLogic.Verify(x => x.ValidateInputAsync(inputList), Times.Once);
        }

        [Fact]
        public async Task GetResult_WithInvalidInput_ShouldReturnErrorJson()
        {
            // Arrange
            var inputList = new List<int[]> { new int[] { 2020, 65 } }; // Invalid: year < age
            var mockResponse = new ResponseObject
            {
                Result = "Some of the input is invalid (Year of Death must be higher than Age of Death)",
                Status = HttpStatusCode.BadRequest.ToString()
            };
            _mockBusinessLogic.Setup(x => x.ValidateInputAsync(inputList)).ReturnsAsync(mockResponse);

            // Act
            var result = await _homeController.GetResult(inputList);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            
            var resultValue = jsonResult.Value;
            var resultType = resultValue.GetType();
            
            var successProperty = resultType.GetProperty("success");
            var messageProperty = resultType.GetProperty("message");
            
            successProperty.GetValue(resultValue).Should().Be(false);
            messageProperty.GetValue(resultValue).Should().Be("Some of the input is invalid (Year of Death must be higher than Age of Death)");

            _mockBusinessLogic.Verify(x => x.ValidateInputAsync(inputList), Times.Once);
        }

        [Fact]
        public async Task GetResult_WithNullInput_ShouldReturnErrorJson()
        {
            // Arrange
            var mockResponse = new ResponseObject
            {
                Result = "No input",
                Status = HttpStatusCode.BadRequest.ToString()
            };
            _mockBusinessLogic.Setup(x => x.ValidateInputAsync(null)).ReturnsAsync(mockResponse);

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

            _mockBusinessLogic.Verify(x => x.ValidateInputAsync(null), Times.Once);
        }
    }
}