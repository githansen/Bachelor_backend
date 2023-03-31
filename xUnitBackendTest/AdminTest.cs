using System.Net;
using Bachelor_backend.Controller;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace xUnitBackendTest;

public class AdminTest
{
    private const string _loggedIn = "AdminSession";
    private const string _notLoggedIn = "";
    
    private static readonly Mock<ITextRepository> mockTextRep = new();
    private static readonly Mock<IVoiceRepository> mockVoiceRep = new();
    private static readonly Mock<ISecurityRepository> mockSecurity = new();
    private static readonly Mock<ILogger<AdminController>> _logger = new();

    private readonly AdminController _adminController = new(mockTextRep.Object, mockVoiceRep.Object, mockSecurity.Object, _logger.Object);
    
    
    private readonly Mock<HttpContext> mockHttpContext = new();
    private readonly MockHttpSession mockSession = new();


    [Fact]
    public async Task LogInOk()
    {
        //Arrange
        var user = new AdminUser()
        {
            Username = "admin",
            Password = "admin"
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockSecurity.Setup(x => x.Login(It.IsAny<AdminUser>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.LogIn(user) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result!.Value!);
    }
    
    [Fact]
    public async Task LogInFailed()
    {
        //Arrange
        mockSecurity.Setup(x => x.Login(It.IsAny<AdminUser>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.LogIn(It.IsAny<AdminUser>()) as UnauthorizedObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result!.StatusCode);
        Assert.Equal("Wrong username or password" , result!.Value!);
    }
    
    [Fact]
    public async Task LogInInputFault()
    {
        //Arrange
        _adminController.ModelState.AddModelError("Username", "Fault in input");
      
        //Act
        var result = await _adminController.LogIn(It.IsAny<AdminUser>()) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result!.StatusCode);
        Assert.Equal("Fault in input" , result!.Value!);
    }

    [Fact]
    public async Task LogOutOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        
        //Act
        var result = await _adminController.LogOut() as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result!.Value!);
        Assert.Equal("", mockSession[_loggedIn]);
    }

    [Fact]
    public async Task RegisterLoggedInOk()
    {
        //Arrange
        var user = new AdminUser()
        {
            Username = "person",
            Password = "password"
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockSecurity.Setup(x => x.Register(It.IsAny<AdminUser>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.RegisterAdmin(user) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result!.Value!);
    }
    
    [Fact]
    public async Task RegisterLoggedInFailed()
    {
        //Arrange
        var user = new AdminUser()
        {
            Username = "person",
            Password = "password"
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockSecurity.Setup(x => x.Register(It.IsAny<AdminUser>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.RegisterAdmin(user) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result!.StatusCode);
        Assert.Equal("Failed to register new admin" , result!.Value!);
    }
    
    [Fact]
    public async Task RegisterLoggedInInputFault()
    {
        //Arrange
        var user = new AdminUser()
        {
            Username = "1234",
            Password = "1234"
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        _adminController.ModelState.AddModelError("Username", "Fault in input");
        mockSecurity.Setup(x => x.Register(It.IsAny<AdminUser>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.RegisterAdmin(user) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result!.StatusCode);
        Assert.Equal("Fault in input" , result!.Value!);
    }
    
    [Fact]
    public async Task RegisterNotLoggedIn()
    {
        //Arrange
        var user = new AdminUser()
        {
            Username = "person",
            Password = "password"
        };
        
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.RegisterAdmin(user) as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result!.StatusCode);
    }

    [Fact]
    public async Task CreateTagOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.CreateTag(It.IsAny<string>())).ReturnsAsync(true);

        //Act
        var result = await _adminController.CreateTag("test") as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task CreateTagNotLoggedIn()
    {
        //Arrange
        
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        

        //Act
        var result = await _adminController.CreateTag("test") as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result!.StatusCode);
    }
    
    [Fact]
    public async Task CreateTagFault()
    {
        //Arrange
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.CreateTag(It.IsAny<string>())).ReturnsAsync(false);

        //Act
        var result = await _adminController.CreateTag("test") as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result!.StatusCode);
        Assert.False((bool) result.Value!);
    }

    [Fact]
    public async Task GetTagOk()
    {
        //Arrange
        
        var tags = new List<Tag>
        {
            new()
            {
                TagId = 1,
                TagText= "test"
            },
            new()
            {
                TagId = 2,
                TagText= "test2"
                }
        };

        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetAllTags()).ReturnsAsync(tags);

        //Act
        var result = await _adminController.GetTags() as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.Equal(tags, result.Value);
    }

    [Fact]
    public async Task GetTagNotLoggedIn()
    {
        //Arrange

        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;

        //Act
        var result = await _adminController.GetTags() as UnauthorizedResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetTagFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetAllTags()).ReturnsAsync((List<Tag>) null);

        //Act
        var result = await _adminController.GetTags() as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task CreateTextOk()
    {
        //Arrange
        var text = new Text()
        {
            Active = true,
            TextId = 1,
            TextText = "test",
            Tags = new List<Tag>()
            {
                new()
                {
                    TagId = 1,
                    TagText = "test"
                }
            }
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.CreateText(It.IsAny<Text>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.CreateText(text) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task CreateTextNotLoggedIn()
    {
        //Arrange
        var text = new Text()
        {
            Active = true,
            TextId = 1,
            TextText = "test",
            Tags = new List<Tag>()
            {
                new()
                {
                    TagId = 1,
                    TagText = "test"
                }
            }
        };
        
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;

        //Act
        var result = await _adminController.CreateText(text) as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task CreateTextFault()
    {
        //Arrange
        var text = new Text();

        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.CreateText(It.IsAny<Text>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.CreateText(text) as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result!.StatusCode);
        Assert.False((bool) result!.Value);
    }
    
    [Fact]
    public async Task CreateTextInputFault()
    {
        //Arrange
        var text = new Text();

        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        _adminController.ModelState.AddModelError("Text", "Fault in input");
        mockTextRep.Setup(x => x.CreateText(It.IsAny<Text>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.CreateText(text) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result!.StatusCode);
        Assert.Equal("Fault in input", result.Value);
    }

    [Fact]
    public async Task DeleteTextOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.DeleteText(It.IsAny<int>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.DeleteText(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task DeleteTextNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.DeleteText(1) as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTextFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.DeleteText(It.IsAny<int>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.DeleteText(1) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }
    
    [Fact]
    public async Task DeleteTagOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.DeleteTag(It.IsAny<int>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.DeleteTag(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task DeleteNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.DeleteTag(1) as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTagFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.DeleteTag(It.IsAny<int>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.DeleteTag(1) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }

    [Fact]
    public async Task GetNumberOfTextsOk()
    { 
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
       
        mockTextRep.Setup(x => x.GetNumberOfTexts()).ReturnsAsync(10);
         
        //Act
        var result = await _adminController.GetNumberOfTexts() as OkObjectResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(10, result.Value);
    }
    
    [Fact]
    public async Task GetNumberOfTextsNotLoggedIn()
    { 
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.GetNumberOfTexts() as UnauthorizedResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetNumberOfTextsFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetNumberOfTexts()).ReturnsAsync(-5);
         
        //Act
        var result = await _adminController.GetNumberOfTexts() as ObjectResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(-1, result.Value);
    }
    
    [Fact]
    public async Task GetNumberOfUsersOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetNumberOfUsers()).ReturnsAsync(10);
         
        //Act
        var result = await _adminController.GetNumberOfUsers() as OkObjectResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(10, result.Value);
    }
    
    [Fact]
    public async Task GetNumberOfUsersNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.GetNumberOfUsers() as UnauthorizedResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetNumberOfUsersFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetNumberOfUsers()).ReturnsAsync(-5);
         
        //Act
        var result = await _adminController.GetNumberOfUsers() as ObjectResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(-1, result.Value);
    }
    
    [Fact]
    public async Task GetOneTextOk()
    {
        //Arrange
        var text = new Text()
        {
            Active = true,
            TextId = 1,
            TextText = "test",
            Tags = new List<Tag>()
            {
                new()
                {
                    TagId = 1,
                    TagText = "test"
                }
            }
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetOneText(It.IsAny<int>())).ReturnsAsync(text);
        
        //Act
        var result = await _adminController.GetOneText(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(text, result.Value);
    }
    
    [Fact]
    public async Task GetOneTextNotLoggedIn()
    {
        //Arrange

        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.GetOneText(1) as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetOneTextFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetOneText(It.IsAny<int>())).ReturnsAsync((Text) null);
        
        //Act
        var result = await _adminController.GetOneText(1) as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task GetOneRecordingOk()
    {
        //Arrange
        var file = new FormFile(new MemoryStream(), 0, 0, "Data", "12345-12345-12345-12345.m4a");
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockVoiceRep.Setup(x => x.GetOneRecording(It.IsAny<string>())).ReturnsAsync(file);
        
        //Act
        var result = await _adminController.GetOneRecording("12345-12345-12345-12345") as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(file, result.Value);
    }
    
    [Fact]
    public async Task GetOneRecordingNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.GetOneRecording("12345-12345-12345-12345") as UnauthorizedResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetOneRecordingFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockVoiceRep.Setup(x => x.GetOneRecording(It.IsAny<string>())).ReturnsAsync((FormFile) null);
        
        //Act
        var result = await _adminController.GetOneRecording("12345-12345-12345-12345") as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Recording not found",result.Value);
    }

    [Fact]
    public async Task GetAllUsersOk()
    {
        //Arrange
        var users = new List<User>()
        {
            new()
            {
                UserId = new Guid(),
                AgeGroup = "18-29",
                Dialect = "Østlandsk",
                Gender = "Kvinne",
                NativeLanguage = "Norsk"
            },
            new()
            {
                UserId = new Guid(),
                AgeGroup = "30-39",
                Dialect = "Trøndersk",
                Gender = "Mann",
                NativeLanguage = "Norsk"
            }
        };
        
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetAllUsers()).ReturnsAsync(users);
        //Act
        var result = await _adminController.GetAllUsers() as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(users, result.Value);
    }
    
    [Fact]
    public async Task GetAllUsersNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.GetAllUsers() as UnauthorizedResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task GetAllUsersFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.GetAllUsers()).ReturnsAsync((List<User>) null);
        
        //Act
        var result = await _adminController.GetAllUsers() as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task EditTextOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.EditText(It.IsAny<Text>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.EditText(It.IsAny<Text>()) as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task EditTextNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.EditText(new Text()) as UnauthorizedResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task EditTextFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.EditText(It.IsAny<Text>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.EditText(new Text()) as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }

    [Fact]
    public async Task EditTagOk()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.EditTag(It.IsAny<Tag>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.EditTag(It.IsAny<Tag>()) as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result!.StatusCode);
        Assert.True((bool) result.Value!);
    }
    
    [Fact]
    public async Task EditTagNotLoggedIn()
    {
        //Arrange
        mockSession[_loggedIn] = _notLoggedIn;
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        //Act
        var result = await _adminController.EditTag(It.IsAny<Tag>()) as UnauthorizedResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task EditTagFault()
    {
        //Arrange
        mockSession[_loggedIn] = "admin";
        mockHttpContext.Setup(s => s.Session).Returns(mockSession);
        _adminController.ControllerContext.HttpContext = mockHttpContext.Object;
        
        mockTextRep.Setup(x => x.EditTag(It.IsAny<Tag>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.EditTag(It.IsAny<Tag>()) as BadRequestObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }
}