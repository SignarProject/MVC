﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Converters;
using AsignarDataAccessLayer.AzureADBModel;
using AsignarBusinessLayer.Services.ServiceInterfaces;

namespace AsignarBusinessLayer.Services
{
    public class AttachmentService : IService<AttachmentDTO>, IDisposable
    {
        private DTOConverter _converter;

        private AsignarDBModel _dbContext;

        private ConverterMDFive _hashConverter;

        public AttachmentService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
        }

        public bool CreateItem(AttachmentDTO newItem)
        {
            Attachment newAttachment = _converter.AttachmentFromDTO(newItem);

            if (_dbContext.Attachments.Any(a => a.Name.Equals(newItem.Name)))
            {
                return false;
            }

            _dbContext.Attachments.Add(newAttachment);
            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteItem(int id)
        {
            Attachment attachment = _dbContext.Attachments.Find(id);

            if(attachment == null)
            {
                return false;
            }

            _dbContext.Attachments.Remove(attachment);
            _dbContext.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public ICollection<AttachmentDTO> GetAllItems()
        {
            ICollection<AttachmentDTO> attachmentsDTO = new HashSet<AttachmentDTO>();

            ICollection<Attachment> attachments = _dbContext.Attachments.ToList();

            foreach(var attachment in attachments)
            {
                attachmentsDTO.Add(_converter.AttachmentToDTO(attachment));
            }

            return attachmentsDTO;
        }

        public AttachmentDTO GetItem(int id)
        {
            Attachment attachment = _dbContext.Attachments.Find(id);

            if(attachment == null)
            {
                throw new InvalidOperationException("Attachment with this ID doesn't exist");
            }

            AttachmentDTO attachmentDTO = _converter.AttachmentToDTO(attachment);

            return attachmentDTO;
        }

        public bool UpdateItem(AttachmentDTO updatedItem)
        {
            Attachment attachment = _dbContext.Attachments.Find(updatedItem.AttachmentID);

            if(attachment == null)
            {
                return false;
            }

            attachment.Name = updatedItem.Name;
            attachment.ContentPath = updatedItem.ContentPath;

            return true;
        }
    }
}
