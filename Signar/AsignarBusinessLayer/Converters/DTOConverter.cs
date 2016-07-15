using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarDataAccessLayer.AzureADBModel;
using AsignarDataAccessLayer.SerializationSignatures;
using AsignarDataAccessLayer.AzureASModel;

namespace AsignarBusinessLayer.Converters
{
    public class DTOConverter
    {
        private AsignarDBModel _dbContext;

        private XMLConverter _xmlConvert;

        private AsignarBlobModel _storageContext;

        public DTOConverter(AsignarDBModel dbContext)
        {
            this._dbContext = dbContext;
        }
        
        public AttachmentDTO AttachmentToDTO(Attachment attachment)
        {
            var attachmentDTO = new AttachmentDTO();

            attachmentDTO.AttachmentID = attachment.AttachmentID;
            attachmentDTO.BugID = attachment.BugID;
            attachmentDTO.Name = attachment.Name;
            attachmentDTO.ContentPath = attachment.ContentPath;

            return attachmentDTO;
        }
        
        public Attachment AttachmentFromDTO(AttachmentDTO attachmentDTO)
        {
            _storageContext = new AsignarBlobModel();

            var newAttachment = new Attachment();

            newAttachment.Bug = _dbContext.Bugs.Find(attachmentDTO.BugID);
            newAttachment.BugID = attachmentDTO.BugID;
            newAttachment.Name = attachmentDTO.Name;

            Bug bug = _dbContext.Bugs.Find(attachmentDTO.BugID);
            string containerName = _storageContext.GetOrCreateBlobBugContainer(string.Concat(bug.Project.Prefix, "-", bug.BugID));
            _storageContext.UploadBlob(containerName, attachmentDTO.Name, attachmentDTO.FileStream);
            newAttachment.ContentPath = _storageContext.GetBlobSasUri(containerName, attachmentDTO.Name);

            return newAttachment;
        }
        
        public BugDTO BugToDTO(Bug bug)
        {
            var bugDTO = new BugDTO();

            bugDTO.BugID = bug.BugID;
            bugDTO.ProjectID = bug.ProjectID;
            bugDTO.Project = ProjectToDTO(bug.Project, true);

            if(bug.AssigneeID != null)
            {
                bugDTO.AssigneeID = bug.AssigneeID;
                bugDTO.User = UserToDTO(_dbContext.Users.Find(bugDTO.AssigneeID), true);
            }            

            bugDTO.Subject = bug.Subject;
            bugDTO.Prefix = string.Concat(bug.Project.Prefix, "-", bug.BugID);
            bugDTO.Description = bug.Description;
            bugDTO.CreationDate = bug.CreationDate;
            bugDTO.ModificationDate = bug.ModificationDate;
            bugDTO.Priority = (Priority)bug.Priority;
            bugDTO.Status = (Status)bug.BugStatus;

            foreach (var comment in bug.Comments)
            {
                bugDTO.Comments.Add(CommentToDTO(comment));
            }

            foreach (var attachment in bug.Attachments)
            {
                bugDTO.Attachments.Add(AttachmentToDTO(attachment));
            }

            return bugDTO;
        }
        
        public Bug BugFromDTO(BugDTO bugDTO)
        {
            var newBug = new Bug();

            newBug.ProjectID = bugDTO.ProjectID;
            newBug.Project = _dbContext.Projects.Find(bugDTO.ProjectID);

            if (bugDTO.AssigneeID.HasValue)
            {
                newBug.AssigneeID = bugDTO.AssigneeID;
                newBug.User = _dbContext.Users.Find(bugDTO.AssigneeID);
            }

            newBug.Subject = bugDTO.Subject;
            newBug.Description = bugDTO.Description;
            newBug.CreationDate = DateTime.Now;
            newBug.ModificationDate = DateTime.Now;
            newBug.BugStatus = (byte)bugDTO.Status;
            newBug.Priority = (byte)bugDTO.Priority;

            foreach (var attachment in bugDTO.Attachments)
            {
                newBug.Attachments.Add(AttachmentFromDTO(attachment));
            }

            return newBug;
        }
        
        public CommentDTO CommentToDTO(Comment comment)
        {
            var commentDTO = new CommentDTO();

            commentDTO.CommentID = comment.CommentID;
            commentDTO.BugID = comment.BugID;
            commentDTO.UserID = comment.UserID;
            commentDTO.Text = comment.Text;
            commentDTO.CreationDate = comment.CreationDate;
            commentDTO.ModificationDate = comment.ModificationDate;

            return commentDTO;
        }
        
        public Comment CommentFromDTO(CommentDTO commentDTO)
        {
            var newComment = new Comment();

            newComment.BugID = commentDTO.BugID;
            newComment.Bug = _dbContext.Bugs.Find(commentDTO.BugID);
            newComment.UserID = commentDTO.UserID;
            newComment.User = _dbContext.Users.Find(commentDTO.UserID);
            newComment.Text = commentDTO.Text;
            newComment.CreationDate = commentDTO.CreationDate;
            newComment.ModificationDate = null;

            return newComment;
        }
        
        public FilterDTO FilterToDTO(Filter filter)
        {
            var filterDTO = new FilterDTO();

            _xmlConvert = new XMLConverter();

            filterDTO.FilterID = filter.FilterID;
            filterDTO.UserID = filter.UserID;
            filterDTO.Title = filter.Title;
            filterDTO.FilterSignarute = _xmlConvert.DeserializeFilter(filter.FilterContent);

            return filterDTO;
        }
        
        public Filter FilterFromDTO(FilterDTO filterDTO)
        {
            var newFilter = new Filter();

            newFilter.UserID = filterDTO.UserID;
            newFilter.User = _dbContext.Users.Find(filterDTO.UserID);
            newFilter.Title = filterDTO.Title;
            newFilter.FilterContent = _xmlConvert.SerializeFilter(filterDTO.FilterSignarute);

            return newFilter;
        }
        
        public ProjectDTO ProjectToDTO(Project project, bool isCollectionItem)
        {
            var projectDTO = new ProjectDTO();

            projectDTO.ProjectID = project.ProjectID;
            projectDTO.Name = project.Name;
            projectDTO.Prefix = project.Prefix;
            projectDTO.IsDeleted = project.IsDeleted;

            if(!isCollectionItem)
            {
                var usersOfProject = project.UsersToProjects.Where(r => r.ProjectID.Equals(project.ProjectID)).Select(r => r).ToList();

                foreach (var record in usersOfProject)
                {
                    /*if(record.User != null)*/projectDTO.Users.Add(UserToDTO(record.User, true));
                }

                foreach (var bug in project.Bugs)
                {
                    projectDTO.Bugs.Add(BugToDTO(bug));
                }
            }

            projectDTO.UsersAmount = project.UsersToProjects.Where(r => r.ProjectID.Equals(project.ProjectID)).Count();
                        
            projectDTO.BugsAmount = project.Bugs.Count;

            return projectDTO;
        }
        
        public Project ProjectFromDTO(ProjectDTO projectDTO)
        {
            var newProject = new Project();

            newProject.Name = projectDTO.Name;
            newProject.Prefix = projectDTO.Prefix;
            newProject.IsDeleted = false;

            return newProject;
        }
        
        public UserDTO UserToDTO(User user, bool isCollectionItem)
        {
            var userDTO = new UserDTO();

            userDTO.UserID = user.UserID;
            userDTO.Name = user.Name;
            userDTO.Surname = user.Surname;
            userDTO.Email = user.Email;
            userDTO.AvatarPath = user.AvatarImagePath;
            userDTO.Login = user.Login;
            userDTO.Password = user.Password;
            userDTO.IsAdmin = user.IsAdmin;

            if(!isCollectionItem)
            {
                var userProjects = user.UsersToProjects.Where(u => u.UserID.Equals(user.UserID)).Select(p => p.Project).ToList();

                foreach (var project in userProjects)
                {
                        userDTO.Projects.Add(ProjectToDTO(project, true));                 
                }

                foreach (var bug in user.Bugs)
                {
                    userDTO.Bugs.Add(BugToDTO(bug));
                }
            }
                                
            foreach (var comment in user.Comments)
            {
                userDTO.Comments.Add(CommentToDTO(comment));
            }

            foreach (var filter in user.Filters)
            {
                userDTO.Filters.Add(FilterToDTO(filter));
            }

            return userDTO;
        }
        
        public User UserFromDTO(UserDTO userDTO)
        {
            _storageContext = new AsignarBlobModel();
            var newUser = new User();

            newUser.Name = userDTO.Name;
            newUser.Surname = userDTO.Surname;
            newUser.Email = userDTO.Email;
            newUser.AvatarImagePath = _storageContext.GetDefaultAvatarSasUri();
            newUser.Login = userDTO.Login;
            newUser.Password = userDTO.Password;
            newUser.IsAdmin = userDTO.IsAdmin;

            return newUser;
        }
        
        public UserToProjectDTO UsersToProjectsToDTO(UsersToProject userToProject)
        {
            var userToProjectDTO = new UserToProjectDTO();


            userToProjectDTO.ProjectID = userToProject.ProjectID;
            userToProjectDTO.UserID = userToProject.UserID;


            return userToProjectDTO;
        }        
    }
}
