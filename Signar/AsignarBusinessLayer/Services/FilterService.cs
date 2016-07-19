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
    public class FilterService : IExtendedService<FilterDTO>, ISearchService<FilterDTO>, IDisposable
    {
        private DTOConverter _converter;

        private AsignarDBModel _dbContext;

        public FilterService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
        }

        public bool CreateItem(FilterDTO newItem)
        {
            Filter newFilter = _converter.FilterFromDTO(newItem);

            _dbContext.Filters.Add(newFilter);
            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteItem(int id)
        {
            Filter filter = _dbContext.Filters.Find(id);

            _dbContext.Filters.Remove(filter);
            _dbContext.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public ICollection<FilterDTO> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public ICollection<FilterDTO> GetAllItemsByUserID(int id)
        {
            ICollection<FilterDTO> filtersDTO = new HashSet<FilterDTO>();

            ICollection<Filter> filters = _dbContext.Filters.Where(f => f.UserID.Equals(id)).Select(f => f).ToList();

            foreach (var filter in filters)
            {
                filtersDTO.Add(_converter.FilterToDTO(filter));
            }

            return filtersDTO;
        }

        public FilterDTO GetItem(int id)
        {
            Filter filter = _dbContext.Filters.Find(id);

            FilterDTO filterDTO = _converter.FilterToDTO(filter);

            return filterDTO;
        }

        public ICollection<FilterDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {
            switch (sortValue)
            {
                case SortBy.Title:
                    {
                        ICollection<Filter> searchResult = _dbContext.Filters.AsNoTracking().OrderBy(f => f.Title).Skip(itemAtOnce * (pageNumber - 1)).Take(itemAtOnce).ToList();
                        ICollection<FilterDTO> dtoResult = new HashSet<FilterDTO>();

                        foreach (var filter in searchResult)
                        {
                            dtoResult.Add(_converter.FilterToDTO(filter));
                        }

                        return dtoResult;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid SortBy parameters!");
                    }
            }
        }

        public ICollection<FilterDTO> SearchBy(string value, ICollection<FilterDTO> searhCollection)
        {
            ICollection<Filter> searchResult = _dbContext.Filters.Select(f => f).Where(f => f.Title.Contains(value)).ToList();
            ICollection<FilterDTO> dtoResult = new HashSet<FilterDTO>();

            foreach (var filter in searchResult)
            {
                dtoResult.Add(_converter.FilterToDTO(filter));
            }

            return dtoResult;
        }

        public bool UpdateItem(FilterDTO updatedItem)
        {
            Filter filter = _dbContext.Filters.Find(updatedItem.FilterID);

            filter.Title = updatedItem.Title;
            filter.FilterContent = _converter.FilterFromDTO(updatedItem).FilterContent;

            return true;
        }
    }
}
