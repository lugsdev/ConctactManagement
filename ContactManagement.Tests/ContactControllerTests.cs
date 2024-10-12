using ContactManagement.Api.Controllers;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactManagement.Tests;

public class ContactControllerTests
{
    private readonly Mock<IContactRepository> _mockRepository;
    private readonly ContactController _contactController;
    public ContactControllerTests()
    {
        _mockRepository = new Mock<IContactRepository>();
        _contactController = new ContactController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllContacts_ReturnsOk_WhenContactsExist()
    {
        var contactList = new List<Contact>()
        {
            new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com"),
            new Contact(1, "Mary", "Doe", "33", "1234567899", "mary.doe@example.com")
        };
        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(contactList);
        
        var result = await _contactController.GetAllContacts();
        
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(2, contactList.Count);
    }

    [Fact]
    public async Task GetContactById_ReturnsOk_WhenContactExists()
    {
        
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
        _mockRepository.Setup(repo => repo.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);
        
        var result = await _contactController.GetContactById(contact.Id);
        
      
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.IsType<Contact>(okResult.Value);
    }

    [Fact]
    public async Task GetContactById_ReturnsNotFound_WhenContactDoesNotExist()
    {
        var contactId = 2;
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(contact.Id))
            .ReturnsAsync(contact);
        
        var result = await _contactController.GetContactById(contactId);
        
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.NotEqual(contact.Id, contactId);
        
    }

    [Fact]
    public async Task AddContact_ReturnsCreated_WhenContactsExist()
    {
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");

        _mockRepository.Setup(repo => repo.AddAsync(contact))
            .ReturnsAsync(1);
        
        var result = await _contactController.AddContact(contact);
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task UpdateContact_ReturnsOk_WhenContactsExist()
    {
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
        _mockRepository.Setup(repo => repo.UpdateAsync(contact))
            .Returns(Task.CompletedTask);
        
        var result = await _contactController.UpdateContact(contact.Id, contact);
        Assert.IsType<NoContentResult>(result);
            
    }
    
    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        var contact = new Contact(1, "John", "Doe", "33", "1234567890", "john.doe@example.com");
        var invalidId = 2;
        
        _mockRepository.Setup(repo => repo.UpdateAsync(contact))
            .Returns(Task.CompletedTask);
        
        var result = await _contactController.UpdateContact(invalidId, contact);
        Assert.IsType<BadRequestResult>(result);
            
    }   
    
    
    [Fact]
    public async Task DeleteContact_ReturnsNoContent_WhenContactIsDeleted()
    {
        var contactId = 1;
        _mockRepository.Setup(repo => repo.DeleteAsync(contactId));
        
        var result = await _contactController.DeleteContact(contactId);
        
        Assert.IsType<NoContentResult>(result);

    }
}