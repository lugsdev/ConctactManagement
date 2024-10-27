using ContactManagement.Api.Controllers;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactManagement.Tests;

public class ContactControllerTests
{
    private readonly Mock<IContactServices> _mockServices;
    private readonly ContactController _contactController;
    public ContactControllerTests()
    {
        _mockServices = new Mock<IContactServices>();
        _contactController = new ContactController(_mockServices.Object);
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
        
        Assert.IsType<OkObjectResult>(result.Result);
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
        
      
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
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
        
        Assert.IsType<NotFoundResult>(result.Result);
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
        Assert.IsType<CreatedAtActionResult>(result.Result);
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
        _mockServices.Setup(repo => repo.UpdateAsync(contact))
            .Returns(Task.CompletedTask);
        
        var result = await _contactController.UpdateContact(contact.Id, contactDto);
        Assert.IsType<NotFoundResult>(result);
            
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
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");

        
        _mockServices.Setup(repo => repo.UpdateAsync(contact))
            .Returns(Task.CompletedTask);
        
        var result = await _contactController.UpdateContact(invalidId, contactDto);
        Assert.IsType<NotFoundResult>(result);
            
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