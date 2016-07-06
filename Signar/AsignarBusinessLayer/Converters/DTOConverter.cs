using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarDataAccessLayer.AzureADBModel;


namespace AsignarBusinessLayer.Converters
{
    public class DTOConverter
    {

        public AttachmentDTO AttachmentToDTO(Attachment attachment)
        {
            var attachmentDTO = new AttachmentDTO();

            attachmentDTO.AttachmentID = attachment.AttachmentID;
            attachmentDTO.BugID = attachment.BugID;
            attachmentDTO.Name = attachment.Name;
            attachmentDTO.ContentPath = attachment.ContentPath;

            return attachmentDTO;
        }


        public Attachment AttachmentFromDTO(AttachmentDTO attachmentDTO, bool isNewEntity)
        {

            using (var dbContext = new AsignarDBModel())
            {

                if (isNewEntity)
                {
                    var newAttachment = new Attachment();

                    newAttachment.Bug = dbContext.Bugs.Single(b => b.BugID.Equals(attachmentDTO.BugID));
                    newAttachment.BugID = attachmentDTO.BugID;
                    newAttachment.Name = attachmentDTO.Name;
                    newAttachment.ContentPath = attachmentDTO.ContentPath;

                    return newAttachment;
                }
                else
                {
                    Attachment attachment = dbContext.Attachments.Single(a => a.AttachmentID.Equals(attachmentDTO.AttachmentID));

                    return attachment;
                }
            }
        }


        public BugDTO BugToDTO(Bug bug)
        {
            var bugDTO = new BugDTO();

            bugDTO.BugID = bug.BugID;
            bugDTO.ProjectID = bug.ProjectID;
            bugDTO.AssigneeID = bug.AssigneeID;
            bugDTO.Subject = bug.Subject;
            bugDTO.BugName = string.Concat(bug.Project.Prefix, "-", bug.BugID);
            bugDTO.Description = bug.Description;
            bugDTO.CreationDate = bug.CreationDate;
            bugDTO.ModificationDate = bug.ModificationDate;
            bugDTO.Priority = (sbyte) bug.Priority;
            bugDTO.BugStatus = (sbyte) bug.BugStatus;
            
            foreach(var comment in bug.Comments)
            {
                bugDTO.Comments.Add(CommentToDTO(comment));
            }

            foreach(var attachment in bug.Attachments)
            {
                bugDTO.Attachments.Add(AttachmentToDTO(attachment));
            }

            return bugDTO;
        }


        public Bug BugFromDTO(BugDTO bugDTO, bool isNewEntity)
        {
            using (var dbContext = new AsignarDBModel())
            {

                if(isNewEntity)
                {
                    var newBug = new Bug();

                    newBug.ProjectID = bugDTO.ProjectID;
                    newBug.Project = dbContext.Projects.Single(p => p.ProjectID.Equals(bugDTO.ProjectID));

                    if(bugDTO.AssigneeID.HasValue)
                    {
                        newBug.AssigneeID = bugDTO.AssigneeID;
                        newBug.User = dbContext.Users.Single(u => u.UserID.Equals(bugDTO.AssigneeID));
                    }

                    newBug.Subject = bugDTO.Subject;
                    newBug.Description = bugDTO.Description;
                    newBug.CreationDate = bugDTO.CreationDate;
                    newBug.BugStatus = (byte) bugDTO.BugStatus;
                    newBug.Priority = (byte) bugDTO.Priority;

                    foreach(var attachment in bugDTO.Attachments)
                    {
                        newBug.Attachments.Add(AttachmentFromDTO(attachment, true));
                    }

                    return newBug;
                }
                else
                {
                    Bug bug = dbContext.Bugs.Single(b => b.BugID.Equals(bugDTO.BugID));

                    return bug;
                }

            }
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


        public Comment CommentFromDTO(CommentDTO commentDTO, bool isNewEntity)
        {
            using (var dbContext = new AsignarDBModel())
            {

                if (isNewEntity)
                {
                    var newComment = new Comment();

                    newComment.BugID = commentDTO.BugID;
                    newComment.Bug = dbContext.Bugs.Single(b => b.BugID.Equals(commentDTO.BugID));
                    newComment.UserID = commentDTO.UserID;
                    newComment.User = dbContext.Users.Single(u => u.UserID.Equals(commentDTO.UserID));
                    newComment.Text = commentDTO.Text;
                    newComment.CreationDate = commentDTO.CreationDate;
                    newComment.ModificationDate = null;

                    return newComment;
                }
                else
                {
                    Comment comment = dbContext.Comments.Single(c => c.CommentID.Equals(commentDTO.CommentID));

                    return comment;
                }

            }
        }


        public FilterDTO FilterToDTO(Filter filter)
        {
            var filterDTO = new FilterDTO();

            filterDTO.FilterID = filter.FilterID;
            filterDTO.UserID = filter.UserID;
            filterDTO.Title = filter.Title;
            filterDTO.FilterContent = filter.FilterContent;

            return filterDTO;
        }


        public Filter FilterFromDTO(FilterDTO filterDTO, bool isNewEntity)
        {
            using (var dbContext = new AsignarDBModel())
            {

                if (isNewEntity)
                {
                    var newFilter = new Filter();

                    newFilter.UserID = filterDTO.UserID;
                    newFilter.User = dbContext.Users.Single(u => u.UserID.Equals(filterDTO.UserID));
                    newFilter.Title = filterDTO.Title;
                    newFilter.FilterContent = filterDTO.FilterContent;

                    return newFilter;
                }
                else
                {
                    Filter filter = dbContext.Filters.Single(f => f.FilterID.Equals(filterDTO.FilterID));

                    return filter;
                }

            }
        }


        public ProjectDTO ProjectToDTO(Project project)
        {
            var projectDTO = new ProjectDTO();

            projectDTO.ProjectID = project.ProjectID;
            projectDTO.Name = project.Name;
            projectDTO.Prefix = project.Prefix;
            projectDTO.IsDeleted = project.IsDeleted;
            projectDTO.BugsAmount = project.Bugs.Count;
            projectDTO.UsersAmount = project.UsersToProjects.Where(r => r.ProjectID.Equals(project.ProjectID)).Count();

            return projectDTO;
        }


        public Project ProjectFromDTO(ProjectDTO projectDTO, bool isNewEntity)
        {

            if (isNewEntity)
            {
                var newProject = new Project();

                newProject.Name = projectDTO.Name;
                newProject.Prefix = projectDTO.Prefix;
                newProject.IsDeleted = projectDTO.IsDeleted;

                return newProject;
            }
            else
            {
                using (var dbContext = new AsignarDBModel())
                {
                    Project project = dbContext.Projects.Single(p => p.ProjectID.Equals(projectDTO.ProjectID));

                    return project;
                }
            }

        }


        public UserDTO UserToDTO(User user)
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
            
            foreach(var bug in user.Bugs)
            {
                userDTO.Bugs.Add(BugToDTO(bug));
            }

            foreach(var comment in user.Comments)
            {
                userDTO.Comments.Add(CommentToDTO(comment));
            }

            foreach(var filter in user.Filters)
            {
                userDTO.Filters.Add(FilterToDTO(filter));
            }

            return userDTO;
        }


        public User UserFromDTO(UserDTO userDTO, bool isNewEntity)
        {
            using (var dbContext = new AsignarDBModel())
            {

                if(isNewEntity)
                {
                    var newUser = new User();

                    newUser.Name = userDTO.Name;
                    newUser.Surname = userDTO.Surname;
                    newUser.Email = userDTO.Email;
                    newUser.AvatarImagePath = userDTO.AvatarPath;
                    newUser.Login = userDTO.Login;
                    newUser.Password = userDTO.Password;
                    newUser.IsAdmin = userDTO.IsAdmin;

                    return newUser;
                }
                else
                {
                    User user = dbContext.Users.Single(u => u.UserID.Equals(userDTO.UserID));

                    return user;
                }

            }
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
