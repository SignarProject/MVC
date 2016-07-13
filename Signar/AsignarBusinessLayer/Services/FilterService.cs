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
    public class FilterService : IExtendedService<FilterDTO>, IDisposable
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
            throw new NotImplementedException();
        }

        public bool DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ICollection<FilterDTO> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public FilterDTO GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<FilterDTO> GetPage(int pageNumber, SortBy sortValue, int itemAtOnce)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(FilterDTO updatedItem)
        {
            throw new NotImplementedException();
        }
    }
}
