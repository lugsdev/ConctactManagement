using ContactManagement.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace ContactManagement.Domain.Entities;

public static class ListaUsuario
{
	public static IList<User> users { get; set; }
}

/// <summary>
/// Represents a User in the system.
/// </summary>
public class User
{
	[SwaggerSchema("Unique identifier for the user")]
	/// <summary>
	/// Unique identifier for the user.
	/// </summary>
	public int Id { get; set; }

	[SwaggerSchema("Username for the user")]
	/// <summary>
	/// The username of the user.
	/// </summary>
	public string Username { get; set; }

	[SwaggerSchema("Password for the user")]
	/// <summary>
	/// The password for the user.
	/// </summary>
	public string Password { get; set; }

	[SwaggerSchema("The system permission level for the user")]
	/// <summary>
	/// The system permission level for the user.
	/// </summary>

	public SystemPermissionType SystemPermission { get; set; }
}