﻿using System;
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
    public class CommentService : IService<CommentDTO>, IDisposable
    {
        private DTOConverter _converter;

        private AsignarDBModel _dbContext;
        
        public CommentService()
        {
            _dbContext = new AsignarDBModel();
            _converter = new DTOConverter(_dbContext);
        }

        public bool CreateItem(CommentDTO newItem)
        {
            Comment newComment = _converter.CommentFromDTO(newItem);

            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteItem(int id)
        {
            Comment com = _dbContext.Comments.Find(id);

            _dbContext.Comments.Remove(com);
            _dbContext.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public ICollection<CommentDTO> GetAllItems()
        {
            ICollection<CommentDTO> commentsDTO = new HashSet<CommentDTO>();

            ICollection<Comment> comments = _dbContext.Comments.ToList();

            foreach(var comment in comments)
            {
                commentsDTO.Add(_converter.CommentToDTO(comment));
            }

            return commentsDTO;
        }

        public CommentDTO GetItem(int id)
        {
            Comment comment = _dbContext.Comments.Find(id);

            CommentDTO commentDTO = _converter.CommentToDTO(comment);

            return commentDTO;
        }

        public bool UpdateItem(CommentDTO updatedItem)
        {
            Comment comment = _dbContext.Comments.Find(updatedItem.CommentID);

            if(comment == null)
            {
                return false;
            }

            comment.Text = updatedItem.Text;
            comment.ModificationDate = updatedItem.ModificationDate;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
