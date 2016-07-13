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
using System.IO;
using AsignarDataAccessLayer.AzureASModel;

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
            try
            {
                User seekingUser = _dbContext.Users.Single(u => (u.Login.Equals(user.Login) || u.Email.Equals(user.Login)));
                if (seekingUser.Password.Equals(_hashConverter.CalculateMD5Hash(user.Password)))
                {
                    user.Name = seekingUser.Name;
                    user.Surname = seekingUser.Surname;
                    user.Email = seekingUser.Email;
                    user.Login = seekingUser.Login;
                    user.AvatarPath = seekingUser.AvatarImagePath;
                    user.IsAdmin = seekingUser.IsAdmin;
                    user.UserID = seekingUser.UserID;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
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
                allUsersDTOs.Add(_converter.UserToDTO(user, false));
            }
            return allUsersDTOs;
        }


        public UserDTO GetItem(int id)
        {
            UserDTO userDTO;
            try
            {
                User user = _dbContext.Users.Find(id);
                userDTO = _converter.UserToDTO(user, false);
            }

            catch(Exception ex)
            {
                return null;
            }

            return userDTO;
        }


        public ICollection<UserDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<User> searchResult = _dbContext.Users.AsNoTracking().OrderBy(u => u.Name).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<UserDTO> dtoResult = new HashSet<UserDTO>();

                        foreach (var user in searchResult)
                        {
                            dtoResult.Add(_converter.UserToDTO(user, false));
                        }

                        return dtoResult;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public bool UpdatePassword(string OldPassword, string NewPassword, int id)
        {
            try
            {
                User seekingUser = _dbContext.Users.Single(u => u.UserID.Equals(id));
                if (seekingUser.Password.Equals(_hashConverter.CalculateMD5Hash(OldPassword)))
                {
                    seekingUser.Password = _hashConverter.CalculateMD5Hash(NewPassword);
                    _dbContext.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool UpdateItem(UserDTO updatedItem)
        {
            User userToUpdate = _dbContext.Users.Find(updatedItem.UserID);

            userToUpdate.Name = updatedItem.Name;
            userToUpdate.Surname = updatedItem.Surname;
            userToUpdate.Email = updatedItem.Email;
            userToUpdate.IsAdmin = updatedItem.IsAdmin;

            _dbContext.SaveChanges();

            return true;
        }

        public bool EditUserPhoto(UserDTO userDTO, string fileName, Stream fileStream)
        {
            AsignarBlobModel storageContext = new AsignarBlobModel();

            User user = _dbContext.Users.Find(userDTO.UserID);
                    
            if(storageContext.UploadBlob(storageContext.GetUserPhotosContainerName(), fileName, fileStream))
            {
                userDTO.AvatarPath = storageContext.GetBlobSasUri(storageContext.GetUserPhotosContainerName(), fileName);
                user.AvatarImagePath = userDTO.AvatarPath;

                _dbContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }            
        }

        public bool ClearUserPhoto(UserDTO userDTO)
        {
            AsignarBlobModel storageContext = new AsignarBlobModel();

            User user = _dbContext.Users.Find(userDTO.UserID);

            user.AvatarImagePath = storageContext.GetBlobSasUri(storageContext.GetUserPhotosContainerName(), "avatar-default.jpg");

            _dbContext.SaveChanges();

            return true;
        }
    }
}
