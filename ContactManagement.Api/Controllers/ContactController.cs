using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Interfaces;
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
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="contactRepository">The repository for handling contact data.</param>
        public ContactController(IContactServices contactServices)
        {
            _contactServices = contactServices;
        }

        /// <summary>
        /// Retrieves all contacts.
        /// </summary>
        /// <returns>A list of all contacts.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            return Ok(await _contactServices.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the contact.</param>
        /// <returns>The contact with the specified identifier.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContactById(int id)
        {
            var contact = await _contactServices.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        /// <summary>
        /// Adds a new contact.
        /// </summary>
        /// <param name="contact">The contact to add.</param>
        /// <returns>The identifier of the newly added contact.</returns>
        [HttpPost]
        public async Task<ActionResult<int>> AddContact(Contact contact)
        {
            var id = await _contactServices.AddAsync(contact);
            return CreatedAtAction(nameof(GetContactById), new { id }, contact);
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="id">The identifier of the contact to update.</param>
        /// <param name="contact">The updated contact information.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            
            await _contactServices.UpdateAsync(contact);
            return NoContent();
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
            return NoContent();
        }
}