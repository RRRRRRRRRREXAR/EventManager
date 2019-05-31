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

        public UserDTO Get(int id)
        {
            var s = Uow.Users.Get(id);
            return new UserDTO {Id=s.Id, Email=s.Email, FirstName= s.FirstName, IsEmailConfirmed=s.IsEmailConfirmed, LastName=s.LastName, Password=s.Password, RoleId=s.RoleId };
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
                throw new ValidationException("Неправильно введена почта или пароль","");
            }
        }

        public void SingUp(UserDTO user)
        {
            Uow.Users.Create(new EventManager.DAL.Entities.User { Email=user.Email, FirstName= user.FirstName, IsEmailConfirmed=false, LastName= user.LastName, Password= user.Password});
            Func<User, bool> f = d => d.Email == user.Email;
            var s = Uow.Users.Find(f).First();
            new Emailer().SendConfirmationEmail(new UserDTO {Id=s.Id, Email=s.Email, FirstName=s.FirstName,IsEmailConfirmed=s.IsEmailConfirmed, LastName=s.LastName, Password=s.Password, RoleId=s.RoleId});
        }
    }
}
