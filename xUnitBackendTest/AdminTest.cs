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
    private static readonly Mock<ILogger<AdminController>> _logger = new();
    private readonly AdminController _adminController = new(mockTextRep.Object, mockVoiceRep.Object, _logger.Object);


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
}