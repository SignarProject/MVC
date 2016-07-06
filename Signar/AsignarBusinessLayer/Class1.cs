using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarDataAccessLayer.AzureADBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer
{
    public class Class1
    {
        public enum SortBy
        {
            Title = 1,
            Status = 2,
            Priority = 3,
            Assignee = 4
        }


        public IEnumerable<ProjectDTO> PagingSolution(SortBy rule, int page)
        {
            var dbContext = new AsignarDBModel();

            switch(rule)
            {
                case SortBy.Title:
                    {
                        IEnumerable<Project> searchResult = dbContext.Projects.AsNoTracking().OrderBy(x => x.Name).Skip(9 * page - 1).Take(9).ToList();
                        List<ProjectDTO> dtoResult = new List<ProjectDTO>();

                        foreach(var project in searchResult)
                        {
                            var projectDTO = new ProjectDTO();

                            projectDTO.ProjectID = project.ProjectID;
                            projectDTO.Name = project.Name;
                            projectDTO.Prefix = project.Prefix;
                            projectDTO.IsDeleted = project.IsDeleted;
                            projectDTO.BugsAmount = project.Bugs.Count;
                            projectDTO.UsersAmount = project.UsersToProjects.Where(p => p.Project.Equals(project)).Select(u => u.User).Count();//Maybe Wrong !! Ask about it!

                            dtoResult.Add(projectDTO);
                        }

                        return dtoResult;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
