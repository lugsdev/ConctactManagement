
namespace ContactManagement.Application.Dtos;

public class ContactDto
{ 
    /// <summary>
    /// Gets the first name of the Contact.
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Gets the last name of the Contact.
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Gests the area code of the Contact
    /// </summary>
    public string AreaCode { get; set; }
    
    /// <summary>
    /// Gets the phone number of the Contact.
    /// </summary>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets the email of the Contact.
    /// </summary>
    public string Email { get; set; }
    
}