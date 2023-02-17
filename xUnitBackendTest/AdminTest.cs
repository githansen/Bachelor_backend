using System.Net;
using Bachelor_backend.Controller;
using Bachelor_backend.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace xUnitBackendTest;

public class AdminTest
{
    private static readonly Mock<ITextRepository> mockTextRep = new();
    private static readonly Mock<ILogger<AdminController>> _logger = new();
    private readonly AdminController _adminController = new(mockTextRep.Object, _logger.Object);


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
}