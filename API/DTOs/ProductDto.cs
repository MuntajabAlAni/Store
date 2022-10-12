﻿namespace API.DTOs;

public class ProductDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = null! ;
    public string Description { get; set; } = null! ;
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; } = null;
    public int ProductTypeId { get; set; }
    public int ProductCollectionId { get; set; }
}