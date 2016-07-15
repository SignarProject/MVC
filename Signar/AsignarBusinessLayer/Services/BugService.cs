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
            if (newBug.AssigneeID == 0) newBug.AssigneeID = null;
            if ((newBug.Priority < 0
                || newBug.Priority > 3)
                && newBug.Project == null
                && (newBug.Subject == null
                || newBug.Subject.Equals(string.Empty)))
            {
                return false;
            }

            _dbContext.Bugs.Add(newBug);
            _dbContext.SaveChanges();
            
            return true;
        }


        public bool DeleteItem(int id)
        {
            Bug bug = _dbContext.Bugs.Find(id);

            _dbContext.Bugs.Remove(bug);
            _dbContext.SaveChanges();

            return true;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }


        public ICollection<BugDTO> GetAllItems()
        {
            ICollection<BugDTO> bugsDTO = new HashSet<BugDTO>();

            ICollection<Bug> bugs = _dbContext.Bugs.ToList();

            foreach(var bug in bugs)
            {
                if(!bug.Project.IsDeleted)
                {
                    bugsDTO.Add(_converter.BugToDTO(bug));
                }                
            }

            return bugsDTO;
        }


        public BugDTO GetItem(int id)
        {
            Bug bug = _dbContext.Bugs.Find(id);
            BugDTO bugDTO = _converter.BugToDTO(bug);

            return bugDTO;
        }


        public ICollection<BugDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.BugID).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
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

        public bool SetStatus(int BugID, int Status)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(BugID);
            bugToUpdate.BugStatus = (byte)Status;

            _dbContext.SaveChanges();
            return true;
        }

        public bool SetAssignee(int BugID, int UserID)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(BugID);
            bugToUpdate.AssigneeID = (byte)UserID;

            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateItem(BugDTO updatedItem)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(updatedItem.BugID);

            if (!bugToUpdate.AssigneeID.Equals(updatedItem.AssigneeID))
            {
                bugToUpdate.AssigneeID = updatedItem.AssigneeID;
                bugToUpdate.User = _dbContext.Users.Find(updatedItem.AssigneeID);
            }
            else return false;

            bugToUpdate.Priority = (byte) updatedItem.Priority;
            bugToUpdate.Subject = updatedItem.Subject;
            bugToUpdate.Description = updatedItem.Description;
            
            foreach(var attachmentDTO in updatedItem.Attachments)
            {
                var attachment = _converter.AttachmentFromDTO(attachmentDTO);

                if(!bugToUpdate.Attachments.Contains(attachment))
                {
                    bugToUpdate.Attachments.Add(attachment);
                }                
            }

            bugToUpdate.BugStatus = (byte) updatedItem.Status;
            
            if ((bugToUpdate.Priority < 0
                || bugToUpdate.Priority > 3)
                && bugToUpdate.Project == null
                && (bugToUpdate.Subject == null
                || bugToUpdate.Subject.Equals(string.Empty)))
            {
                return false;
            }
            
            return true;
        }
    }
}
