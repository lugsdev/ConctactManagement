using System.Threading.Tasks;
using ContactManagement.Domain.Models;

namespace ContactManagement.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken(User user); // Método deve ser assíncrono
    }
}
