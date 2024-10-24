using ContactManagement.Api.Extensions;
using ContactManagement.Api.Extensions.Models;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ContactManagement.Api.Controllers;

/// <summary>
/// Handles HTTP requests related to contact operations.
/// </summary>
[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ValidateModelStateFilter))]
public class ContactController : BaseController
{
    private readonly IContactServices _contactServices;

    /// <summary>
    /// private variable for the <see cref="IContactServices"/> class.
    /// </summary>
    /// <param name="contactServices">The repository for handling contact data.</param>
    public ContactController(IContactServices contactServices, INotificationService notificationServices): base(notificationServices)
    {
        _contactServices = contactServices;
    }

    /// <summary>
    /// Retrieves all contacts.
    /// </summary>
    /// <returns>A list of all contacts.</returns>
    [HttpGet]
    public async Task<ActionResult> GetAllContacts()
    {

        var contacts = await _contactServices.GetAllAsync();

        if (!contacts.Any()) return NotFound("Não existe contatos cadastrados");

        var contactsDtos = contacts.Select(contact => new ContactDto
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            AreaCode = contact.AreaCode,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
        }).ToList();

        return CustomResponse(HttpStatusCode.OK, contactsDtos);
    }

    /// <summary>
    /// Retrieves a contact by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact.</param>
    /// <returns>The contact with the specified identifier.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetContactById(int id)
    {
        var contact = await _contactServices.GetByIdAsync(id);

        if (contact == null) return NotFound("Não existe contato com o Id informado");

        var contactDto = new ContactDto
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            AreaCode = contact.AreaCode,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
        };
             
        return CustomResponse(HttpStatusCode.OK, contactDto);
    }

    /// <summary>
    /// Retrieves a list of contacts filtered by a specific area code.
    /// </summary>
    /// <param name="areaCode">The area code used to filter contacts.</param>
    /// <returns>A list of contacts that match the provided area code.</returns>
    [HttpGet("getContactsByAreaCode/{areaCode}")]
    public async Task<ActionResult> GetContactByAreaCode(int areaCode)
	{
	 var contacts = await _contactServices.GetByAreaCodeAsync(areaCode);

	 if (!contacts.Any()) return NotFound("Não existe contato com a AreaCode informada");

        var contactsDtos = contacts.Select(contact => new ContactDto
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            AreaCode = contact.AreaCode,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
        }).ToList();

        return CustomResponse(HttpStatusCode.OK, contactsDtos);
	}

	/// <summary>
	/// Adds a new contact.
	/// </summary>
	/// <param name="contactDto">The contact to add.</param>
	/// <returns>The identifier of the newly added contact.</returns>
	[HttpPost]
    public async Task<ActionResult> AddContact(ContactDto contactDto)
    {
        var contact = new Contact(0, contactDto.FirstName, contactDto.LastName, contactDto.AreaCode, contactDto.PhoneNumber, contactDto.Email);

        var id = await _contactServices.AddAsync(contact);

        if (id == 0) 
            return CustomResponse();

        return CustomResponse(HttpStatusCode.Created, contactDto);
    }

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="id">The identifier of the contact to update.</param>
    /// <param name="contactDto">The updated contact information.</param>
    /// <returns>A status indicating the result of the operation.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto contactDto)
    {
        var existingContact = await _contactServices.GetByIdAsync(id);

        if (existingContact == null) return NotFound("Não existe o contato a ser atualizado.");
        
        existingContact.UpdateContact(contactDto.FirstName, contactDto.LastName, contactDto.AreaCode, contactDto.PhoneNumber, contactDto.Email); 
        await _contactServices.UpdateAsync(existingContact);

        return CustomResponse(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Deletes a contact by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact to delete.</param>
    /// <returns>A status indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        await _contactServices.DeleteAsync(id);

        return CustomResponse(HttpStatusCode.NoContent);
    }
}