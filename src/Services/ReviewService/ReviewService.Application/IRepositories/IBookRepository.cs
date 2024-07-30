﻿using ReviewService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.IRepositories
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);
        Task<bool> BookExistsAsync(Guid bookId);
    }
}
