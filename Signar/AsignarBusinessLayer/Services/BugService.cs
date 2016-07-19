using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Converters;
using AsignarDataAccessLayer.AzureADBModel;
using AsignarBusinessLayer.Services.ServiceInterfaces;
using AsignarDataAccessLayer.SerializationSignatures;
using AsignarBusinessLayer.SortEnum;

namespace AsignarBusinessLayer.Services
{
    public class BugService : IExtendedService<BugDTO>, IDisposable
    {
        private DTOConverter _converter;
        
        private AsignarDBModel _dbContext;

        private NotificationQueueService _notificationService;

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

        public int CopyTask(BugDTO newItem)
        {
            Bug newBug = _converter.BugFromDTO(newItem);
            if (newBug.AssigneeID == 0) newBug.AssigneeID = null;
            if ((newBug.Priority < 0
                || newBug.Priority > 3)
                && newBug.Project == null
                && (newBug.Subject == null
                || newBug.Subject.Equals(string.Empty)))
            {
                return 0;
            }

            _dbContext.Bugs.Add(newBug);
            _dbContext.SaveChanges();

            return newBug.BugID;
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

        public int GetBugStatus(int id)
        {
            Bug bug = _dbContext.Bugs.Find(id);
            BugDTO bugDTO = _converter.BugToDTO(bug);

            return (int)bugDTO.Status;
        }
        
        public ICollection<BugDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.Project.Prefix).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<BugDTO> dtoResult = new HashSet<BugDTO>();

                        foreach (var bug in searchResult)
                        {
                            dtoResult.Add(_converter.BugToDTO(bug));
                        }

                        return dtoResult;
                    }
                case SortBy.Assignee:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.User.Name).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<BugDTO> dtoResult = new HashSet<BugDTO>();

                        foreach (var bug in searchResult)
                        {
                            dtoResult.Add(_converter.BugToDTO(bug));
                        }

                        return dtoResult;
                    }
                case SortBy.Priority:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.Priority).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<BugDTO> dtoResult = new HashSet<BugDTO>();

                        foreach (var bug in searchResult)
                        {
                            dtoResult.Add(_converter.BugToDTO(bug));
                        }

                        return dtoResult;
                    }
                case SortBy.Status:
                    {
                        ICollection<Bug> searchResult = _dbContext.Bugs.AsNoTracking().OrderBy(b => b.BugStatus).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
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

            BugDTO transportBugInfo = _converter.BugToDTO(bugToUpdate);

            _notificationService = new NotificationQueueService();
            _notificationService.BugConditionChanged(transportBugInfo, new List<string>());

            _dbContext.SaveChanges();
            return true;
        }

        public bool SetAssignee(int BugID, int UserID)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(BugID);
            bugToUpdate.AssigneeID = (byte)UserID;

            BugDTO transportBugInfo = _converter.BugToDTO(bugToUpdate);

            _notificationService = new NotificationQueueService();
            _notificationService.BugReassigned(transportBugInfo, new List<string>());

            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateItem(BugDTO updatedItem)
        {
            Bug bugToUpdate = _dbContext.Bugs.Find(updatedItem.BugID);
            _notificationService = new NotificationQueueService();

            if (!bugToUpdate.AssigneeID.Equals(updatedItem.AssigneeID))
            {
                bugToUpdate.AssigneeID = updatedItem.AssigneeID;
                bugToUpdate.User = _dbContext.Users.Find(updatedItem.AssigneeID);
                updatedItem.Project = _converter.ProjectToDTO(bugToUpdate.Project, true);
                updatedItem.User = _converter.UserToDTO(bugToUpdate.User, true);
                _notificationService.BugReassigned(updatedItem, new List<string>());
            }

            bugToUpdate.Priority = (byte)updatedItem.Priority;
            bugToUpdate.Subject = updatedItem.Subject;
            bugToUpdate.Description = updatedItem.Description;
            bugToUpdate.ModificationDate = DateTime.Now;

            foreach (var attachmentDTO in updatedItem.Attachments)
            {
                var attachment = _converter.AttachmentFromDTO(attachmentDTO);

                if (!bugToUpdate.Attachments.Contains(attachment))
                {
                    bugToUpdate.Attachments.Add(attachment);
                }
            }

            bugToUpdate.BugStatus = (byte)updatedItem.Status;

            if ((bugToUpdate.Priority < 0
                || bugToUpdate.Priority > 3)
                && bugToUpdate.Project == null
                && (bugToUpdate.Subject == null
                || bugToUpdate.Subject.Equals(string.Empty)))
            {
                return false;
            }

            _dbContext.SaveChanges();

            if (bugToUpdate.BugStatus != (byte)updatedItem.Status
                || bugToUpdate.Priority != (byte)updatedItem.Priority
                || bugToUpdate.Description != updatedItem.Description)
            {
                _notificationService.BugConditionChanged(updatedItem, new List<string>());
            }
                        
            return true;
        }

        public ICollection<BugDTO> SearchBy(string value, ICollection<BugDTO> searchCollection)
        {
            ICollection<BugDTO> searchResult = searchCollection.Select(b => b).Where(b => b.Subject.Contains(value)).ToList();
            ICollection<BugDTO> dtoResult = new HashSet<BugDTO>();

            foreach (var bug in searchCollection)
            {
                dtoResult.Add(bug);
            }

            return dtoResult;
        }

        public ICollection<BugDTO> AdvancedSearch(FilterDTO searchingFilter)
        {
            ICollection<BugDTO> resultCollection;

            ICollection<BugDTO> searchCollection = GetAllItems().Where(b => b.Subject.Contains(searchingFilter.FilterSignarute.SearchString)).Select(b => b).ToList();

            var projectMatchList = new List<BugDTO>();

            if (searchingFilter.FilterSignarute.Projects.Count > 0)
            {                
                foreach (var project in searchingFilter.FilterSignarute.Projects)
                {
                    projectMatchList.Concat(searchCollection.Where(b => b.ProjectID.Equals(project.ProjectID)).Select(b => b).ToList());
                }
            }                  

            var assigneeMatchList = new List<BugDTO>();

            if (searchingFilter.FilterSignarute.Assignees.Count > 0)
            {                
                foreach (var assignee in searchingFilter.FilterSignarute.Assignees)
                {
                    assigneeMatchList.Concat(searchCollection.Where(b => b.AssigneeID.Equals(assignee.UserID)).Select(b => b).ToList());
                }
            }

            if (projectMatchList.Count > 0 && assigneeMatchList.Count > 0)
            {
               var intersectedList = projectMatchList.Intersect(assigneeMatchList);
            }

            var statusMatchList = new List<BugDTO>();

            if(searchingFilter.FilterSignarute.Statuses.Count != 0)
            {
                foreach (var status in searchingFilter.FilterSignarute.Statuses)
                {
                    statusMatchList.Concat(searchCollection.Where(b => b.Status.Equals((Status)status)).Select(b => b).ToList());
                }
            }

            var priorityMatchList = new List<BugDTO>();

            if(searchingFilter.FilterSignarute.Priorities.Count != 0)
            {

            }

            foreach (var priority in searchingFilter.FilterSignarute.Priorities)
            {

            }

            return null;
        }
    }
}
