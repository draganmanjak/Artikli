using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.UserManagement.Services
{
    public class ServiceResponse<T>
    {
        public T Model { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = null;
    }
}
