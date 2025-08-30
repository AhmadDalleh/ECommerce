using AutoMapper;
using ECommerce.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Catalog
{
    public class CategoryAppService : 
        CrudAppService<Category,
            CategoryDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateUpdateCategoryDto>, ICategoryAppService
    {

        private IRepository<Category, int> _categoryRepo;
        private IMapper _mapper;
        public CategoryAppService(IRepository<Category, int> repository, IMapper mapper) : base(repository)
        {
            _categoryRepo = repository;
            _mapper = mapper;
        }

        
    }
}
