﻿using AutoMapper;
using CatalogService.Application.Dto;
using CatalogService.Application.UseCases;
using CatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBookCommand, Book>()
                .ForMember(dest => dest.BookCategories, opt => opt.Ignore())
                .ForMember(dest => dest.BookAuthors, opt => opt.Ignore())
                .ForMember(dest => dest.BookSellers, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.Sellers, opt => opt.Ignore());
            CreateMap<UpdateBookCommand, Book>();

            CreateMap<CreateAuthorCommand, Author>();
            CreateMap<UpdateAuthorCommand, Author>();

            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<UpdateCategoryCommand, Category>();

            CreateMap<CreatePublisherCommand, Publisher>();
            CreateMap<UpdatePublisherCommand, Publisher>();

            CreateMap<CreateBookSellerCommand, BookSeller>();
            CreateMap<UpdateBookSellerCommand, BookSeller>();

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id)))
                .ForMember(dest => dest.AuthorIds, opt => opt.MapFrom(src => src.Authors.Select(a => a.Id)));

            CreateMap<Author, AuthorDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Publisher, PublisherDto>();
            
        }
    }
}
