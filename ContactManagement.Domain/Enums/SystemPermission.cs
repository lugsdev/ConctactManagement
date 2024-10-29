using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Enums
{
    public enum SystemPermissionType
    {
        Admin,
        Usuario,
        Convidado
    }
    public static class SystemPermission
    {
        public const string Admin = "Administrador";
        public const string Usuario = "Usuario";
        public const string Convidado = "Convidado";
    }
}
