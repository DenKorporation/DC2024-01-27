using Microsoft.AspNetCore.Mvc;
using Moq;
using REST.Controllers;
using REST.Models.DTOs.Request;
using REST.Models.DTOs.Response;
using REST.Services.Interfaces;

namespace REST.Tests.Controllers;

public class EditorControllerTests
{
    [Fact]
    public void Create_ValidEditorRequestDto_ReturnsCreatedStatusCodeWithEditorResponseDto()
    {
        EditorRequestDto requestDto = new EditorRequestDto()
            { Login = "test", Password = "12345678", FirstName = "test", LastName = "test" };
        EditorResponseDto responseDto = new EditorResponseDto()
            { Id = 1, Login = "test", FirstName = "test", LastName = "test" };

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.Create(It.IsAny<EditorRequestDto>())).Returns(responseDto);

        var controller = new EditorController(mockService.Object);


        var response = controller.Create(requestDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(response);
        var actualResult = Assert.IsType<EditorResponseDto>(createdResult.Value);
        Assert.Equal(responseDto.Id, actualResult.Id);
    }

    // TODO: after fixes for EditorController write correct test
    // [Fact]
    // public void Create_InValidEditorRequestDto_ReturnsCreatedStatusCodeWithEditorResponseDto()
    // {
    //     EditorRequestDto requestDto = new EditorRequestDto()
    //         { Login = "test", Password = "12345678", FirstName = "test", LastName = "test" };
    //     EditorResponseDto responseDto = new EditorResponseDto()
    //         { Id = 1, Login = "test", FirstName = "test", LastName = "test" };
    //     
    //     var mockService = new Mock<IEditorService>();
    //     mockService.Setup(service => service.Create(It.IsAny<EditorRequestDto>())).Returns(responseDto);
    //
    //     var controller = new EditorController(mockService.Object);
    //
    //
    //     var response = controller.Create(requestDto);
    //
    //     Assert.IsType<CreatedAtActionResult>(response);
    // }

    [Fact]
    public void GetAll_EmptyRepository_ReturnsOkStatusCodeWithEmptyArray()
    {
        List<EditorResponseDto> editors = new List<EditorResponseDto>();

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.GetAll()).Returns(editors);

        var controller = new EditorController(mockService.Object);


        var response = controller.GetAll();

        var okObjectResult = Assert.IsType<OkObjectResult>(response);
        var actualResult = Assert.IsType<List<EditorResponseDto>>(okObjectResult.Value);
        Assert.Empty(actualResult);
    }

    [Fact]
    public void GetAll_NotEmptyRepository_ReturnsOkStatusCodeWithNotEmptyArray()
    {
        EditorResponseDto responseDto = new EditorResponseDto()
            { Id = 1, Login = "test", FirstName = "test", LastName = "test" };
        List<EditorResponseDto> editors = [responseDto, responseDto];

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.GetAll()).Returns(editors);

        var controller = new EditorController(mockService.Object);


        var response = controller.GetAll();

        var okObjectResult = Assert.IsType<OkObjectResult>(response);
        var actualResult = Assert.IsType<List<EditorResponseDto>>(okObjectResult.Value);
        Assert.NotEmpty(actualResult);
    }

    [Fact]
    public void GetById_EditorNotExist_ReturnsNotFoundStatusCode()
    {
        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.GetById(It.IsAny<long>())).Returns((EditorResponseDto?)null);

        var controller = new EditorController(mockService.Object);


        var response = controller.GetById(-1);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public void GetById_EditorExist_ReturnsOkStatusCodeWithEditorResponseDto()
    {
        EditorResponseDto responseDto = new EditorResponseDto()
            { Id = 1, Login = "test", FirstName = "test", LastName = "test" };

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.GetById(It.IsAny<long>())).Returns(responseDto);

        var controller = new EditorController(mockService.Object);


        var response = controller.GetById(1);

        var okObjectResult = Assert.IsType<OkObjectResult>(response);
        var actualResult = Assert.IsType<EditorResponseDto>(okObjectResult.Value);
        Assert.Equal(responseDto.Id, actualResult.Id);
    }

    [Fact]
    public void Update_EditorNotExist_ReturnsNotFoundStatusCode()
    {
        EditorRequestDto requestDto = new EditorRequestDto()
            { Id = -1, Login = "test", Password = "12345678", FirstName = "test", LastName = "test" };

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.Update(It.IsAny<long>(), It.IsAny<EditorRequestDto>()))
            .Returns((EditorResponseDto?)null);

        var controller = new EditorController(mockService.Object);


        var response = controller.Update(requestDto);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public void Update_EditorExist_ReturnsOkStatusCodeWithEditorResponseDto()
    {
        EditorRequestDto requestDto = new EditorRequestDto()
            { Id = 1, Login = "updated", Password = "12345678", FirstName = "test", LastName = "test" };
        EditorResponseDto responseDto = new EditorResponseDto()
            { Id = 1, Login = "updated", FirstName = "test", LastName = "test" };

        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.Update(It.IsAny<long>(), It.IsAny<EditorRequestDto>()))
            .Returns(responseDto);

        var controller = new EditorController(mockService.Object);


        var response = controller.Update(requestDto);

        var okObjectResult = Assert.IsType<OkObjectResult>(response);
        var actualResult = Assert.IsType<EditorResponseDto>(okObjectResult.Value);
        Assert.Equal(responseDto.FirstName, actualResult.FirstName);
    }

    [Fact]
    public void Delete_EditorExist_ReturnsNoContentStatusCode()
    {
        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.Delete(It.IsAny<long>())).Returns(true);

        var controller = new EditorController(mockService.Object);


        var response = controller.Delete(1);

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public void Delete_EditorNotExist_ReturnsNotFoundStatusCode()
    {
        var mockService = new Mock<IEditorService>();
        mockService.Setup(service => service.Delete(It.IsAny<long>())).Returns(false);

        var controller = new EditorController(mockService.Object);


        var response = controller.Delete(-1);

        Assert.IsType<NotFoundResult>(response);
    }
}