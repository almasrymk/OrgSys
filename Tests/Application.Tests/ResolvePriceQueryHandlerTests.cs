using Catalog.Application.Pricing.Queries;
using Catalog.Contracts.Pricing;
using Catalog.Domain;
using Moq;
using OrgSys.SharedKernel;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

public class ResolvePriceQueryHandlerTests
{
    [Fact]
    public async Task Handle_NoMatchingEntry_FallsBackToProductDefaultPrice()
    {
        var product = new Product { Id = 1, Name = "Widget", Price = 42m };
        var entryRepository = new Mock<IRepository<PriceListEntry>>();
        entryRepository
            .Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<PriceListEntry, bool>>>()))
            .ReturnsAsync((IEnumerable<PriceListEntry>?)null);
        var productRepository = new Mock<IRepository<Product>>();
        productRepository
            .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(product);

        var handler = new ResolvePriceQueryHandler(entryRepository.Object, productRepository.Object);

        var result = await handler.Handle(new ResolvePriceQuery(1, null, 1, null, DateTime.Today), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Response);
        Assert.Equal(42m, result.Response!.Price);
        Assert.Equal("ProductDefaultPrice", result.Response.AppliedRule);
    }

    [Fact]
    public async Task Handle_ActiveEntryWithinValidityWindow_UsesEntryPrice()
    {
        var entry = new PriceListEntry
        {
            Id = 1,
            ProductId = 1,
            PriceListId = 7,
            Price = 35m,
            IsActive = true,
            ValidFrom = DateTime.Today.AddDays(-1),
            ValidTo = DateTime.Today.AddDays(1)
        };
        var entryRepository = new Mock<IRepository<PriceListEntry>>();
        entryRepository
            .Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<PriceListEntry, bool>>>()))
            .ReturnsAsync(new List<PriceListEntry> { entry });
        var productRepository = new Mock<IRepository<Product>>();

        var handler = new ResolvePriceQueryHandler(entryRepository.Object, productRepository.Object);

        var result = await handler.Handle(new ResolvePriceQuery(1, 7, 1, null, DateTime.Today), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Response);
        Assert.Equal(35m, result.Response!.Price);
        Assert.Equal(7, result.Response.PriceListId);
        Assert.Equal("PriceListEntry", result.Response.AppliedRule);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsNotFound()
    {
        var entryRepository = new Mock<IRepository<PriceListEntry>>();
        entryRepository
            .Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<PriceListEntry, bool>>>()))
            .ReturnsAsync((IEnumerable<PriceListEntry>?)null);
        var productRepository = new Mock<IRepository<Product>>();
        productRepository
            .Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        var handler = new ResolvePriceQueryHandler(entryRepository.Object, productRepository.Object);

        var result = await handler.Handle(new ResolvePriceQuery(999, null, 1, null, DateTime.Today), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Null(result.Response);
    }
}
