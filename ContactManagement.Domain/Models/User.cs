using ContactManagement.Domain.Enums;

namespace ContactManagement.Domain.Models;
/// <summary>
///  This Record is created to simulate an user and will not be save in database
/// </summary>
/// <param name="Id">Unique identifier for the user</param>
/// <param name="Email">User emai</param>
/// <param name="Password">User passwor</param>
/// <param name="Roles">User Role</param>
/// 
//Lista que vamos usar em memoria para simular a consultada de usuario
public static class ListaUsuario
{
    public static IList<User> users { get; set; }
}


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public SystemPermissionType SystemPermission { get; set; }

}