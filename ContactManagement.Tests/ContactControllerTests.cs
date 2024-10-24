using ContactManagement.Api.Controllers;
using ContactManagement.Api.Extensions.Models;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace ContactManagement.Tests;

public class ContactControllerTests
{
    private readonly Mock<IContactServices> _mockServices;
        private readonly Mock<INotificationService> _mockNotificationServices;
    private readonly ContactController _contactController;
    public ContactControllerTests()
    {
        _mockServices = new Mock<IContactServices>();
        _mockNotificationServices = new Mock<INotificationService>();
        _contactController = new ContactController(_mockServices.Object, _mockNotificationServices.Object);
    }

    [Fact]
    public async Task GetAllContacts_ReturnsOk_WhenContactsExist()
    {
        
        var contactListDto = new List<ContactDto>()
        {new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                AreaCode = "33",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com"
            },
            
            new ContactDto
            {
                FirstName = "Mary",
                LastName = "Doe",
                AreaCode = "33",
                PhoneNumber = "1234567899",
                Email = "may.doe@example.com"
            }
        };
        var contactList = new List<Contact>()
        {
            new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com"),
            new Contact(1, "Mary", "Doe", "33", "1234567899", "mary.doe@example.com")
        };
        _mockServices.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(contactList);
        
        var result = await _contactController.GetAllContacts();
        
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(2, contactList.Count);
    }

    [Fact]
    public async Task GetContactById_ReturnsOk_WhenContactExists()
    {
        
        var contactDto = new ContactDto
        {
            FirstName = "John",
            LastName = "Doe",
            AreaCode = "33",
            PhoneNumber = "1234567890",
            Email = "john.doe@example.com"
        };
        
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
       
        _mockServices.Setup(repo => repo.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);
        
        var result = await _contactController.GetContactById(contact.Id);
        
      
        var okResult = Assert.IsType<ObjectResult>(result);
        
        Assert.IsType<ContactDto>(okResult.Value);
    }

    [Fact]
    public async Task GetContactById_ReturnsNotFound_WhenContactDoesNotExist()
    {
        var contactId = 2;
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
        
        _mockServices.Setup(repo => repo.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);
        
        var result = await _contactController.GetContactById(contactId);
        
        Assert.IsType<ObjectResult>(result);
        Assert.NotEqual(contact.Id, contactId);
        
    }

    [Fact]
    public async Task AddContact_ReturnsCreated_WhenContactsExist()
    {
        var contactDto = new ContactDto
        {
            FirstName = "John",
            LastName = "Doe",
            AreaCode = "33",
            PhoneNumber = "1234567890",
            Email = "john.doe@example.com"
        };

        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");

        _mockServices.Setup(repo => repo.AddAsync(contact))
            .ReturnsAsync(1);
        
        var result = await _contactController.AddContact(contactDto);
        Assert.IsType<ObjectResult>(result);
    }

    [Fact]
    public async Task UpdateContact_ReturnsOk_WhenContactsExist()
    {
        var contactDto = new ContactDto
        {
            FirstName = "John",
            LastName = "Doe",
            AreaCode = "33",
            PhoneNumber = "1234567890",
            Email = "john.doe@example.com"
        };

        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");

        _mockServices.Setup(repo => repo.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);

        _mockServices.Setup(repo => repo.UpdateAsync(contact))
            .Returns(Task.CompletedTask);

        var result = await _contactController.UpdateContact(contact.Id, contactDto);

        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal((int)HttpStatusCode.NoContent, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        var contactDto = new ContactDto
        {
            FirstName = "John",
            LastName = "Doe",
            AreaCode = "33",
            PhoneNumber = "1234567890",
            Email = "john.doe@example.com"
        };
        var invalidId = 2;

        _mockServices.Setup(repo => repo.GetByIdAsync(invalidId))
            .ReturnsAsync((Contact)null);

        var result = await _contactController.UpdateContact(invalidId, contactDto);

        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        var errorResponse = Assert.IsType<ErrorResponse>(objectResult.Value);
        Assert.Single(errorResponse.Errors);
        Assert.Equal("Não existe o contato a ser atualizado.", errorResponse.Errors.First());
    }


    [Fact]
    public async Task DeleteContact_ReturnsNoContent_WhenContactIsDeleted()
    {
        var contactId = 1;
        _mockServices.Setup(repo => repo.DeleteAsync(contactId));
        
        var result = await _contactController.DeleteContact(contactId);
        
        Assert.IsType<NoContentResult>(result);

    }
}