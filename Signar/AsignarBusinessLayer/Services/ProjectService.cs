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
    public class ProjectService : IExtendedService<ProjectDTO>, IDisposable
    {

        private DTOConverter _converter;


        private AsignarDBModel _dbContext;


        public ProjectService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
        }


        public bool CreateItem(ProjectDTO newItem)
        {
            Project newProject = _converter.ProjectFromDTO(newItem);


            if(_dbContext.Projects.Any(p => p.Prefix.Equals(newItem.Prefix)))
            {
                return false;
            }


            _dbContext.Projects.Add(newProject);
            _dbContext.SaveChanges();


            return true;
        }


        public bool DeleteItem(int id)
        {
            Project project = _dbContext.Projects.Find(id);


            project.IsDeleted = true;

            foreach(var bug in project.Bugs)
            {
                bug.AssigneeID = null;
            }

            _dbContext.SaveChanges();


            return true;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }


        public ICollection<ProjectDTO> GetAllItems()
        {
            ICollection<ProjectDTO> allProjectDTOs = new HashSet<ProjectDTO>();


            ICollection<Project> allProjects = _dbContext.Projects.ToList();


            foreach (var project in allProjects)
            {
                allProjectDTOs.Add(_converter.ProjectToDTO(project, false));
            }

            return allProjectDTOs;
        }

        public ICollection<ProjectDTO> GetAllProjectsByUserId(int userID)
        {
            if (_dbContext.Users.Find(userID) == null) return null;
            ICollection<ProjectDTO> allProjectDTOsOfUser = new HashSet<ProjectDTO>();
            ICollection<Project> allProjectsOfUser = _dbContext.UsersToProjects.Where(r => r.UserID.Equals(userID)).Select(u => u.Project).ToList();


            foreach (var project in allProjectsOfUser)
            {
                allProjectDTOsOfUser.Add(_converter.ProjectToDTO(project, false));
            }

            return allProjectDTOsOfUser;
        }

        public ProjectDTO GetItem(int id)
        {
            ProjectDTO projectDTO;
            try
            {
                Project project = _dbContext.Projects.Find(id);
                projectDTO = _converter.ProjectToDTO(project, false);
            }
            catch(Exception ex)
            {
                return null;
            }
            return projectDTO;
        }


        public ICollection<ProjectDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {

            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Project> searchResult = _dbContext.Projects.AsNoTracking().OrderBy(p => p.Name).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<ProjectDTO> dtoResult = new HashSet<ProjectDTO>();

                        foreach (var project in searchResult)
                        {
                            dtoResult.Add(_converter.ProjectToDTO(project, false));
                        }

                        return dtoResult;
                    }
                default:
                    {
                        return null;
                    }
            }
        }


        public bool UpdateItem(ProjectDTO updatedItem)
        {
            Project projectToUpdate = _dbContext.Projects.Find(updatedItem.ProjectID);


            projectToUpdate.Name = updatedItem.Name;


            return true;
        }
    }
}
