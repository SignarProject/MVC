using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Converters;
using AsignarDataAccessLayer.AzureADBModel;
using AsignarBusinessLayer.Services.ServiceInterfaces;
using AsignarBusinessLayer.SortEnum;

namespace AsignarBusinessLayer.Services
{
    public class UserService : IExtendedService<UserDTO>, IAuthenticationService<UserDTO>, IDisposable
    {
        private DTOConverter _converter;

        private AsignarDBModel _dbContext;

        private ConverterMDFive _hashConverter;

        public UserService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
            _hashConverter = new ConverterMDFive();
        }

        public bool AuthenticateUser(UserDTO user)
        {
            User seekingUser = _dbContext.Users.Single(u => u.Login.Equals(user.Login) && u.Email.Equals(user.Email));

            if (seekingUser.Password.Equals(_hashConverter.CalculateMD5Hash(user.Password)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateItem(UserDTO newItem)
        {
            User newUser = _converter.UserFromDTO(newItem);


            if (_dbContext.Users.Any(u => (u.Email.Equals(newItem.Email) || u.Login.Equals(newItem.Login)) ))
            {
                return false;
            }

            newUser.Password = _hashConverter.CalculateMD5Hash(newUser.Password);

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();


            return true;
        }


        public bool DeleteItem(int id)
        {
            User user = _dbContext.Users.Find(id);


            if (user.Bugs.Any())
            {
                return false;
            }


            _dbContext.Users.Remove(user);


            _dbContext.SaveChanges();


            return true;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public ICollection<UserDTO> GetAllItems()
        {
            ICollection<UserDTO> allUsersDTOs = new HashSet<UserDTO>();


            ICollection<User> allUsers = _dbContext.Users.ToList();


            foreach (var user in allUsers)
            {
                allUsersDTOs.Add(_converter.UserToDTO(user));
            }

            return allUsersDTOs;
        }


        public UserDTO GetItem(int id)
        {
            User user = _dbContext.Users.Find(id);


            UserDTO userDTO = _converter.UserToDTO(user);


            return userDTO;
        }


        public ICollection<UserDTO> GetPage(int pageNumber, SortBy sortValue)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<User> searchResult = _dbContext.Users.AsNoTracking().OrderBy(u => u.Name).Skip(9 * (pageNumber - 1)).Take(9).ToList();
                        ICollection<UserDTO> dtoResult = new HashSet<UserDTO>();

                        foreach (var user in searchResult)
                        {
                            dtoResult.Add(_converter.UserToDTO(user));
                        }

                        return dtoResult;
                    }
                default:
                    {
                        return null;
                    }
            }
        }


        public bool UpdateItem(UserDTO updatedItem)
        {
            User userToUpdate = _dbContext.Users.Find(updatedItem.UserID);


            userToUpdate.Name = updatedItem.Name;
            userToUpdate.Surname = updatedItem.Surname;
            userToUpdate.Email = updatedItem.Email;
            userToUpdate.AvatarImagePath = updatedItem.AvatarPath;
            userToUpdate.IsAdmin = updatedItem.IsAdmin;


            return true;
        }
    }
}
