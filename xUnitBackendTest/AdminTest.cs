using System.Net;
using Bachelor_backend.Controller;
using Bachelor_backend.DAL.Repositories;
using Bachelor_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace xUnitBackendTest;

public class AdminTest
{
    private static readonly Mock<ITextRepository> mockTextRep = new();
    private static readonly Mock<IVoiceRepository> mockVoiceRep = new();
    private static readonly Mock<ISecurityRepository> mockSecurity = new();
    private static readonly Mock<ILogger<AdminController>> _logger = new();
    private readonly AdminController _adminController = new(mockTextRep.Object, mockVoiceRep.Object, mockSecurity.Object, _logger.Object);


    [Fact]
    public async Task CreateTagOk()
    {
        //Arrange
        mockTextRep.Setup(x => x.CreateTag(It.IsAny<string>())).ReturnsAsync(true);

        //Act
        var result = await _adminController.CreateTag("test") as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
        
    }
    
    [Fact]
    public async Task CreateTagFault()
    {
        //Arrange
        mockTextRep.Setup(x => x.CreateTag(It.IsAny<string>())).ReturnsAsync(false);

        //Act
        var result = await _adminController.CreateTag("test") as BadRequestObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }

    [Fact]
    public async Task GetTagOk()
    {
        //Arrange
        var tags = new List<Tag>
        {
            new Tag()
            {
                TagId = 1,
                TagText= "test"
            },
            new Tag()
            {
                TagId = 2,
                TagText= "test2"
                }
        };

        mockTextRep.Setup(x => x.GetAllTags()).ReturnsAsync(tags);

        //Act
        var result = await _adminController.GetTags() as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(tags, result.Value);
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
                new Tag()
                {
                    TagId = 1,
                    TagText = "test"
                }
            }
        };
        
        mockTextRep.Setup(x => x.CreateText(It.IsAny<Text>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.CreateText(text) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
    }
    
    [Fact]
    public async Task CreateTextFault()
    {
        //Arrange
        var text = new Text();

        mockTextRep.Setup(x => x.CreateText(It.IsAny<Text>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.CreateText(text) as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(false, result.Value);
    }

    [Fact]
    public async Task DeleteTextOk()
    {
        //Arrange
        mockTextRep.Setup(x => x.DeleteText(It.IsAny<int>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.DeleteText(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
    }
    
    [Fact]
    public async Task DeleteTextFault()
    {
        //Arrange
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
        mockTextRep.Setup(x => x.DeleteTag(It.IsAny<int>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.DeleteTag(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
    }
    
    [Fact]
    public async Task DeleteTagFault()
    {
        //Arrange
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
         mockTextRep.Setup(x => x.GetNumberOfTexts()).ReturnsAsync(10);
         
         //Act
         var result = await _adminController.GetNumberOfTexts() as OkObjectResult;
         
         //Assert
         Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
         Assert.Equal(10, result.Value);
    }
    
    [Fact]
    public async Task GetNumberOfTextsFault()
    {
        //Arrange
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
        mockTextRep.Setup(x => x.GetNumberOfUsers()).ReturnsAsync(10);
         
        //Act
        var result = await _adminController.GetNumberOfUsers() as OkObjectResult;
         
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(10, result.Value);
    }
    
    [Fact]
    public async Task GetNumberOfUsersFault()
    {
        //Arrange
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
        mockTextRep.Setup(x => x.GetOneText(It.IsAny<int>())).ReturnsAsync(text);
        
        //Act
        var result = await _adminController.GetOneText(1) as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(text, result.Value);
    }
    
    [Fact]
    public async Task GetOneTextFault()
    {
        //Arrange
        mockTextRep.Setup(x => x.GetOneText(It.IsAny<int>())).ReturnsAsync((Text) null);
        
        //Act
        var result = await _adminController.GetOneText(1) as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(null, result.Value);
    }

    [Fact]
    public async Task GetOneRecordingOk()
    {
        //Arrange
        var file = new FormFile(new MemoryStream(), 0, 0, "Data", "12345-12345-12345-12345.m4a");
        
        mockVoiceRep.Setup(x => x.GetOneRecording(It.IsAny<string>())).ReturnsAsync(file);
        
        //Act
        var result = await _adminController.GetOneRecording("12345-12345-12345-12345") as OkObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(file, result.Value);
    }   
    [Fact]
    public async Task GetOneRecordingFault()
    {
        //Arrange
        
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
                UserId = 1,
                AgeGroup = "18-29",
                Dialect = "Østlandsk",
                Gender = "Kvinne",
                NativeLanguage = "Norsk"
            },
            new()
            {
                UserId = 2,
                AgeGroup = "30-39",
                Dialect = "Trøndersk",
                Gender = "Mann",
                NativeLanguage = "Norsk"
            }
        };
        
        mockTextRep.Setup(x => x.GetAllUsers()).ReturnsAsync(users);
        //Act
        var result = await _adminController.GetAllUsers() as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(users, result.Value);
    }
    
    [Fact]
    public async Task GetAllUsersFault()
    {
        //Arrange
        mockTextRep.Setup(x => x.GetAllUsers()).ReturnsAsync((List<User>) null);
        
        //Act
        var result = await _adminController.GetAllUsers() as ObjectResult;
        
        //Assert
        Assert.Equal((int) HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(null, result.Value);
    }

    [Fact]
    public async Task EditTextOk()
    {
        //Arrange
        mockTextRep.Setup(x => x.EditText(It.IsAny<Text>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.EditText(new Text()) as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
    }

    [Fact]
    public async Task EditTextFault()
    {
        //Arrange
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
        mockTextRep.Setup(x => x.EditTag(It.IsAny<Tag>())).ReturnsAsync(true);
        
        //Act
        var result = await _adminController.EditTag(new Tag()) as OkObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(true, result.Value);
    }
    
    [Fact]
    public async Task EditTagFault()
    {
        //Arrange
        mockTextRep.Setup(x => x.EditTag(It.IsAny<Tag>())).ReturnsAsync(false);
        
        //Act
        var result = await _adminController.EditTag(new Tag()) as BadRequestObjectResult;

        //Assert
        Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(false, result.Value);
    }
}