using System.Net;
using Bachelor_backend.Controller;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace xUnitBackendTest
{
    public class UserTest
    {
        private const string _loggedIn = "UserSession";
        private const string _notLoggedIn = "";
        
        private static readonly Mock<IVoiceRepository> mockVoiceRep = new Mock<IVoiceRepository>();
        private static readonly Mock<ITextRepository> mockTextRep = new Mock<ITextRepository>();
        private static readonly Mock<ILogger<UserController>> _logger = new Mock<ILogger<UserController>>();
        private readonly UserController _userController = new UserController(mockVoiceRep.Object, mockTextRep.Object, _logger.Object);

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SaveFileOk()
        {
            //Arrange
            mockVoiceRep.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).ReturnsAsync("76185658-eb35-4095-8491-08daffb158a5");

            mockSession[_loggedIn] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.SaveFile(It.IsAny<IFormFile>()) as OkObjectResult;


            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("76185658-eb35-4095-8491-08daffb158a5", result.Value);

        }
        [Fact]
        public async Task SaveFileFault()
        {
            //Arrange
            mockVoiceRep.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).ReturnsAsync("");
            
            mockSession[_loggedIn] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.SaveFile(It.IsAny<IFormFile>()) as BadRequestObjectResult;


            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Voice recording is not saved", result.Value);

        }
        
        [Fact]
        public async Task SaveFileNotLoggedIn()
        {
            //Arrange
            
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.SaveFile(It.IsAny<IFormFile>()) as UnauthorizedObjectResult;


            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Not logged in", result.Value);

        }

        [Fact]
        public async Task DeleteFileOk()
        {
            //Arrange
            mockVoiceRep.Setup(x => x.DeleteFile(It.IsAny<string>())).ReturnsAsync(true);
            
            //Act
            var result = await _userController.DeleteFile(It.IsAny<string>()) as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Voice recording is deleted", result.Value);
        }
        
        [Fact]
        public async Task DeleteFileFault()
        {
            //Arrange
            mockVoiceRep.Setup(x => x.DeleteFile(It.IsAny<string>())).ReturnsAsync(false);
            
            //Act
            var result = await _userController.DeleteFile(It.IsAny<string>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Voice recording is not deleted", result.Value);
        }
    }
}