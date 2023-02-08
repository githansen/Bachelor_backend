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
            mockVoiceRep.Setup(x => x.SaveFile(It.IsAny<IFormFile>(),It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync("76185658-eb35-4095-8491-08daffb158a5");

            mockSession[_loggedIn] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.SaveFile(It.IsAny<IFormFile>(),1) as OkObjectResult;


            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("76185658-eb35-4095-8491-08daffb158a5", result.Value);

        }
        [Fact]
        public async Task SaveFileFault()
        {
            //Arrange
            mockVoiceRep.Setup(x => x.SaveFile(It.IsAny<IFormFile>(),It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync("");
            
            mockSession[_loggedIn] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.SaveFile(It.IsAny<IFormFile>(),1) as BadRequestObjectResult;


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
            var result = await _userController.SaveFile(It.IsAny<IFormFile>(), 1) as UnauthorizedResult;


            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);

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

        [Fact]
        public async Task GetTextOk()
        {
            //Arrange
            var tags = new List<Tag>
            {
                new()
                {
                    TagId = 1,
                    TagText = "Lorem ipsum"
                },
                new()
                {
                    TagId = 2,
                    TagText = "Random text"
                }
            };

            var text = new Text
            {
                TextId = 1,
                TextText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus blandit dolor mi, et hendrerit ipsum rhoncus ac. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam vitae neque quis dolor lacinia elementum faucibus in lacus. Vivamus eleifend fringilla justo sit amet vulputate. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam hendrerit eu lacus quis finibus. Integer fringilla justo vel massa pellentesque, ac semper dolor aliquam. Curabitur congue lorem sit amet felis luctus tincidunt. Suspendisse ipsum orci, euismod varius pellentesque vitae, fermentum a tortor. Etiam pulvinar facilisis tempus. Suspendisse risus odio, convallis in eros sit amet, efficitur congue ipsum. Curabitur tincidunt urna at nunc imperdiet, non accumsan tellus dignissim.",
                Tags = tags,
                Active = true
            };
            mockTextRep.Setup(x => x.GetText(It.IsAny<User>())).ReturnsAsync(text);
            
            mockSession[_loggedIn] = "1";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            
            var result = await _userController.GetText() as OkObjectResult;
            
            
            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(text, result.Value);
        }
        
        [Fact]
        public async Task GetTextNotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            
            var result = await _userController.GetText() as UnauthorizedResult;
            
            
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task RegisterUserInfoOk()
        {
            //Arrange
            var user = new User()
            {
                UserId = 1,
                NativeLanguage = "Norwegian",
                AgeGroup = "18-20",
                Dialect = "Østlandsk"
            };
            
            mockTextRep.Setup(x => x.RegisterUserInfo(It.IsAny<User>())).ReturnsAsync(user);

            mockSession[_loggedIn] = user.UserId;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.RegisterUserInfo(It.IsAny<User>()) as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Ok", result.Value);
        }
        [Fact]
        public async Task RegisterUserInfoNull()
        {
            //Arrange
            var user = new User()
            {
                UserId = 1,
                NativeLanguage = null,
                AgeGroup = null,
                Dialect = null
            };
            
            mockTextRep.Setup(x => x.RegisterUserInfo(It.IsAny<User>())).ReturnsAsync(user);

            mockSession[_loggedIn] = user.UserId;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _userController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _userController.RegisterUserInfo(It.IsAny<User>()) as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Ok", result.Value);
        }
    }
}