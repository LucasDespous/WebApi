using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.UseCases;

public class ProductUseCase : IProductUseCase
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public ProductUseCase(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    public async Task<IReadOnlyCollection<ProductResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        => (await _products.GetAllAsync(cancellationToken)).Select(ToResponse).ToList();

    public async Task<IReadOnlyCollection<ProductResponseDto>> SearchAsync(string name, CancellationToken cancellationToken)
        => (await _products.SearchByNameAsync(name, cancellationToken)).Select(ToResponse).ToList();

    public async Task<ProductResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        return product is null ? null : ToResponse(product);
    }

    public async Task<ProductResponseDto?> CreateAsync(ProductRequestDto request, CancellationToken cancellationToken)
    {
        if (await _categories.GetByIdAsync(request.CategoryId, cancellationToken) is null) return null;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };

        await _products.AddAsync(product, cancellationToken);
        var created = await _products.GetByIdAsync(product.Id, cancellationToken);
        return created is null ? null : ToResponse(created);
    }

    public async Task<bool> UpdateAsync(Guid id, ProductRequestDto request, CancellationToken cancellationToken)
    {
        if (await _categories.GetByIdAsync(request.CategoryId, cancellationToken) is null) return false;
        var product = await _products.GetByIdForUpdateAsync(id, cancellationToken);
        if (product is null) return false;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        await _products.UpdateAsync(product, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdForUpdateAsync(id, cancellationToken);
        if (product is null) return false;
        await _products.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static ProductResponseDto ToResponse(Product product)
        => new(product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, product.Category?.Name ?? string.Empty);
}
