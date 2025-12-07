using ERP.Application.Services.Caching;
using ERP.Infrastracture.Services.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ERP.UnitTests.Services;

public class CacheServiceTests
{
    private readonly ICacheService _cacheService;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<ILogger<CacheService>> _loggerMock;

    public CacheServiceTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _loggerMock = new Mock<ILogger<CacheService>>();
        _cacheService = new CacheService(_memoryCache, _loggerMock.Object);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreValue()
    {
        // Arrange
        var key = "test-key";
        var value = new TestDto { Id = Guid.NewGuid(), Name = "Test" };

        // Act
        await _cacheService.SetAsync(key, value);
        var result = await _cacheService.GetAsync<TestDto>(key);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(value.Id);
        result.Name.Should().Be(value.Name);
    }

    [Fact]
    public async Task GetAsync_WhenKeyNotExists_ShouldReturnNull()
    {
        // Arrange
        var key = "non-existent-key";

        // Act
        var result = await _cacheService.GetAsync<TestDto>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValue()
    {
        // Arrange
        var key = "remove-test-key";
        var value = new TestDto { Id = Guid.NewGuid(), Name = "Test" };
        await _cacheService.SetAsync(key, value);

        // Act
        await _cacheService.RemoveAsync(key);
        var result = await _cacheService.GetAsync<TestDto>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrCreateAsync_WhenNotCached_ShouldCallFactory()
    {
        // Arrange
        var key = "get-or-create-key";
        var factoryCallCount = 0;
        var expectedValue = new TestDto { Id = Guid.NewGuid(), Name = "Created" };

        // Act
        var result = await _cacheService.GetOrCreateAsync(key, async () =>
        {
            factoryCallCount++;
            return await Task.FromResult(expectedValue);
        });

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedValue.Id);
        factoryCallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetOrCreateAsync_WhenCached_ShouldNotCallFactory()
    {
        // Arrange
        var key = "cached-get-or-create-key";
        var cachedValue = new TestDto { Id = Guid.NewGuid(), Name = "Cached" };
        await _cacheService.SetAsync(key, cachedValue);
        var factoryCallCount = 0;

        // Act
        var result = await _cacheService.GetOrCreateAsync(key, async () =>
        {
            factoryCallCount++;
            return await Task.FromResult(new TestDto { Id = Guid.NewGuid(), Name = "New" });
        });

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(cachedValue.Id);
        factoryCallCount.Should().Be(0);
    }

    [Fact]
    public async Task ExistsAsync_WhenKeyExists_ShouldReturnTrue()
    {
        // Arrange
        var key = "exists-test-key";
        await _cacheService.SetAsync(key, new TestDto { Id = Guid.NewGuid(), Name = "Test" });

        // Act
        var exists = await _cacheService.ExistsAsync(key);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenKeyNotExists_ShouldReturnFalse()
    {
        // Arrange
        var key = "not-exists-test-key";

        // Act
        var exists = await _cacheService.ExistsAsync(key);

        // Assert
        exists.Should().BeFalse();
    }

    private class TestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

