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
                allProjectDTOs.Add(_converter.ProjectToDTO(project));
            }

            return allProjectDTOs;
        }


        public ProjectDTO GetItem(int id)
        {
            Project project = _dbContext.Projects.Find(id);


            ProjectDTO projectDTO = _converter.ProjectToDTO(project);


            return projectDTO;
        }


        public ICollection<ProjectDTO> GetPage(int pageNumber, SortBy sortValue)
        {

            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Project> searchResult = _dbContext.Projects.AsNoTracking().OrderBy(p => p.Name).Skip(9 * (pageNumber - 1)).Take(9).ToList();
                        ICollection<ProjectDTO> dtoResult = new HashSet<ProjectDTO>();

                        foreach (var project in searchResult)
                        {
                            dtoResult.Add(_converter.ProjectToDTO(project));
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
