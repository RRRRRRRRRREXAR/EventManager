using EventManager.DAL.Entities;
using EventManager.DAL.Interfaces;
using EventMangerBLL.DTO;
using EventMangerBLL.Infrastructure;
using EventMangerBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Uow;
        public UserService(IUnitOfWork work)
        {
            Uow = work;
        }
        public void ConfirmEmail(UserDTO user)
        {
            Uow.Users.Update(new User { Id=user.Id, IsEmailConfirmed=true, Email=user.Email, FirstName=user.FirstName, LastName=user.LastName, Password=user.Password, RoleId=user.RoleId});
        }

        public void Dispose()
        {
            Uow.Dispose();
        }

        public UserDTO SignIn(Func<User, bool> predicate)
        {
            try
            {
                var finded = Uow.Users.Find(predicate).First();
                return new UserDTO { Id= finded.Id, Email=finded.Email, FirstName=finded.FirstName, LastName= finded.LastName, Password= finded.Password, RoleId=finded.RoleId, IsEmailConfirmed=finded.IsEmailConfirmed };
            }
            catch
            {
                return null;
            }
        }

        public void SingUp(UserDTO user)
        {

            Uow.Users.Create(new EventManager.DAL.Entities.User { Email=user.Email, FirstName= user.FirstName, IsEmailConfirmed=false, LastName= user.LastName, Password= user.Password});
            new Emailer().SendConfirmationEmail(user);
            throw new NotImplementedException();
        }
    }
}
