﻿
using CatalogService.Domain.Entities;
using CatalogService.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogService.Infostructure.Repositories
{
    public class BookSellerRepository
        (CatalogDbContext dbContext) : IBookSellerRepository
    {
        private readonly CatalogDbContext _dbcontext = dbContext;

        public async Task CreateAsync(BookSeller bookSeller, CancellationToken token = default)
        {
            await _dbcontext.BookSellers.AddAsync(bookSeller, token);
        }
        public Task<BookSeller> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _dbcontext.BookSellers
                .Include(bs => bs.Book)
                .Include(bs => bs.Seller)
                .FirstOrDefaultAsync(bs => bs.Id.Equals(id), token); ;
        }

        public async Task UpdateAsync(BookSeller bookSeller, CancellationToken token = default)
        {
            await _dbcontext.BookSellers
                .Where(u => u.Id.Equals(bookSeller.Id))
                .ExecuteUpdateAsync(update => update
                    .SetProperty(u => u.Price, bookSeller.Price)
                    .SetProperty(u => u.Description, bookSeller.Description), token);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token = default)
        {
            await _dbcontext.BookSellers.Where(p => p.Id.Equals(id)).ExecuteDeleteAsync(token);
        }

        public async Task<List<BookSeller>> GetByTwinId(Guid bookId, Guid sellerId, CancellationToken token = default)
        {
            var bookSeller = await _dbcontext.BookSellers
                .Where(u => u.BookId.Equals(bookId))
                .Where(u => u.SellerId.Equals(sellerId)).ToListAsync();
            return bookSeller;
        }
        public Task<bool> AnyAsync(Guid id, CancellationToken token = default)
        {
            return _dbcontext.BookSellers.AnyAsync(prop => prop.Id.Equals(id));
        }
    }
}
