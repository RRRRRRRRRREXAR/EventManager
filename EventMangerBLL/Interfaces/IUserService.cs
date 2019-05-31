using EventManager.DAL.Entities;
using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Interfaces
{
    public interface IUserService
    {
        UserDTO SignIn(Func<User, bool> predicate);
        void SingUp(UserDTO user);
        void ConfirmEmail(UserDTO user);
        UserDTO Get(int id);
        void Dispose();
    }
}
