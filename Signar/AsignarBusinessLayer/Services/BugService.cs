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
    public class BugService : IService<BugDTO>, IPagingService<BugDTO>, IDisposable
    {

        private DTOConverter _converter;


        private AsignarDBModel _dbContext;

        
        public BugService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
        }


        public bool CreateItem(BugDTO newItem)
        {
            Bug newBug = _converter.BugFromDTO(newItem);
            newBug.BugStatus = 0;


            if(newBug.Priority > 3 
                && newBug.Project.Equals(null)
                && (newBug.Subject.Equals(null) 
                || newBug.Equals(string.Empty)))
            {
                return false;
            }

            _dbContext.Bugs.Add(newBug);
            _dbContext.SaveChanges();


            return true;
        }


        public bool DeleteItem(int id)
        {
            _dbContext.Bugs.Find(id).BugStatus = 4;
            return true;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }


        public ICollection<BugDTO> GetAllItems()
        {
            throw new NotImplementedException();
        }


        public BugDTO GetItem(int id)
        {
            Bug bug = _dbContext.Bugs.Find(id);
            BugDTO bugDTO = _converter.BugToDTO(bug);

            return bugDTO;
        }


        public ICollection<BugDTO> GetPage(int pageNumber, SortBy sortValue)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.BugID).Skip(9 * (pageNumber - 1)).Take(9).ToList();
                        ICollection<BugDTO> dtoResult = new HashSet<BugDTO>();

                        foreach (var bug in searchResult)
                        {
                            dtoResult.Add(_converter.BugToDTO(bug));
                        }

                        return dtoResult;
                    }
                default:
                    {
                        return null;
                    }
            }
        }


        public bool UpdateItem(BugDTO updatedItem)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(updatedItem.BugID);


            bugToUpdate.Priority = (byte) updatedItem.Priority;
            bugToUpdate.Subject = updatedItem.Subject;
            bugToUpdate.AssigneeID = updatedItem.AssigneeID;
            bugToUpdate.Description = updatedItem.Description;
            
            foreach(var attachment in updatedItem.Attachments)
            {
                bugToUpdate.Attachments.Add(_converter.AttachmentFromDTO(attachment));
            }

            bugToUpdate.BugStatus = (byte) updatedItem.Status;


            if (bugToUpdate.Priority > 3
                && bugToUpdate.Project.Equals(null)
                && (bugToUpdate.Subject.Equals(null)
                || bugToUpdate.Equals(string.Empty)))
            {
                return false;
            }


            return true;
        }
    }
}
