using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.DTOs;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IMapper _mapper;
        
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandsRepo,

        IGenericRepository<ProductType> productTypesRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productTypesRepo = productTypesRepo;
            _productBrandsRepo = productBrandsRepo;
            _productsRepo = productsRepo;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecificationParameters productParameters)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productParameters);   

            var countSpec = new ProductWithFiltersForCountSpecification(productParameters);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsyncWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParameters.PageIndex, productParameters.PageSize, totalItems, data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product is null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        
        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandsRepo.ListAllAsync());
        }
        
        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _productTypesRepo.ListAllAsync());
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromForm] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.PictureUrl = await CopyFileToServerAsync(productDto.Image!);

            _unitOfWork.Repository<Product>().Add(product);
            var result = await _unitOfWork.Complete();
            
            return Ok(result <= 0 ? null : product);
        }
        
        [HttpPut]
        public async Task<ActionResult<Product>> Update([FromForm] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.PictureUrl = await CopyFileToServerAsync(productDto.Image!);
        
            await DeleteFileFromServer(product.PictureUrl);
        
            _unitOfWork.Repository<Product>().Update(product);
            var result = await _unitOfWork.Complete();
        
            return Ok(result <= 0 ? null : product);
        }
        private static async Task<string> CopyFileToServerAsync(IFormFile image)
        {
            var imageFolderName = Path.Combine("Resources", "ProductsImages");
            var imageUrl = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var pathToSaveImage = Path.Combine(imageFolderName, imageUrl);

            await using var streamImage = new FileStream((pathToSaveImage), FileMode.Create);
            await image.CopyToAsync(streamImage);
            return imageUrl;
        }
        private async static Task DeleteFileFromServer(string pictureUrl)
        {
            var imageFolderName = Path.Combine("Resources", "ProductImages");
            var pathToDeleteImage = Path.Combine(imageFolderName, pictureUrl);

            System.IO.File.Delete(pathToDeleteImage);
        }
    }
}