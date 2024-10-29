using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.Api.Controllers;

/// <summary>
/// Handles HTTP requests related to contact operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
        private readonly IContactServices _contactServices;

        /// <summary>
        /// private variable for the <see cref="IContactServices"/> class.
        /// </summary>
        /// <param name="contactServices">The repository for handling contact data.</param>
        public ContactController(IContactServices contactServices)
        {
            _contactServices = contactServices;
        }

        /// <summary>
        /// Retrieves all contacts.
        /// </summary>
        /// <returns>A list of all contacts.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllContacts()
        {
            
            var contacts = await _contactServices.GetAllAsync();
            
            
            return Ok(contacts.Select(contact => new ContactDto
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                AreaCode = contact.AreaCode,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
            }).ToList());
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the contact.</param>
        /// <returns>The contact with the specified identifier.</returns>
        [HttpGet("{id}")]
    [Authorize(Roles = SystemPermission.Usuario)]
    public async Task<ActionResult<ContactDto>> GetContactById(int id)
        {
            var contact = await _contactServices.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var contactDto = new ContactDto
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                AreaCode = contact.AreaCode,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,

            };
                
                
            return Ok(contactDto);
        }


	    [HttpGet("/{areaCode}")]
    [Authorize(Roles = SystemPermission.Usuario)]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetContactByAreaCode(int areaCode)
	    {
		    var contacts = await _contactServices.GetByAreaCodeAsync(areaCode);
		    if (contacts == null)
		    {
			    return NotFound();
		    }

		    return Ok(contacts.Select(contact => new ContactDto
		    {
			    FirstName = contact.FirstName,
			    LastName = contact.LastName,
			    AreaCode = contact.AreaCode,
			    PhoneNumber = contact.PhoneNumber,
			    Email = contact.Email,
		    }).ToList());
	}

	/// <summary>
	/// Adds a new contact.
	/// </summary>
	/// <param name="contactDto">The contact to add.</param>
	/// <returns>The identifier of the newly added contact.</returns>
	[HttpPost]
    [Authorize(Roles = SystemPermission.Usuario)]
    public async Task<ActionResult<int>> AddContact(ContactDto contactDto)
        {
            var contact = new Contact(0, contactDto.FirstName, contactDto.LastName, contactDto.AreaCode, contactDto.PhoneNumber, contactDto.Email);

            var id = await _contactServices.AddAsync(contact);
            return CreatedAtAction(nameof(GetContactById), new { id }, contactDto);
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="id">The identifier of the contact to update.</param>
        /// <param name="contactDto">The updated contact information.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("{id}")]
    [Authorize(Roles = SystemPermission.Admin)]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto contactDto)
        {
            var existingContact = await _contactServices.GetByIdAsync(id);
            if (existingContact == null)
            {
                return NotFound();
            }
            
            existingContact.UpdateContact(contactDto.FirstName, contactDto.LastName, contactDto.AreaCode, contactDto.PhoneNumber, contactDto.Email); 
            await _contactServices.UpdateAsync(existingContact);
            return NoContent();
        }

        /// <summary>
        /// Deletes a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the contact to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
    [Authorize(Roles = SystemPermission.Admin)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            
            await _contactServices.DeleteAsync(id);
            return NoContent();
        }
}