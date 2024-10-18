namespace ContactManagement.Domain.Models;
/// <summary>
///  This Record is created to simulate an user and will not be save in database
/// </summary>
/// <param name="Id">Unique identifier for the user</param>
/// <param name="Email">User emai</param>
/// <param name="Password">User passwor</param>
/// <param name="Roles">User Role</param>
public record User(
    int Id,
    string Email,
    string Password,
    string[] Roles
);