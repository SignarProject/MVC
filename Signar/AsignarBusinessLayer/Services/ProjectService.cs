using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Converters;
using AsignarDataAccessLayer.AzureADBModel;

namespace AsignarBusinessLayer.Services
{
    public class ProjectService
    {

        private DTOConverter _converter;


        public ProjectService()
        {
            _converter = new DTOConverter();
        }

        public bool CreateNewProject(ProjectDTO newProjectDTO)
        {
            Project newProject = _converter.ProjectFromDTO(newProjectDTO, true);

            using (var dbContext = new AsignarDBModel())
            {
                dbContext.Projects.Add(newProject);
                dbContext.SaveChanges();
            }

            return true;
        }


        public bool EditProject(ProjectDTO editedProjectDTO)
        {
            Project project = _converter.ProjectFromDTO(editedProjectDTO, false);

            project.Prefix = editedProjectDTO.Prefix;
            


            return true;
        }
    }
}
