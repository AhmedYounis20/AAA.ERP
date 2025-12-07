using FluentAssertions;
using Shared.DTOs;

namespace ERP.UnitTests.DTOs;

public class PaginatedResultTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var items = new List<string> { "item1", "item2", "item3" };
        var totalCount = 10;
        var pageNumber = 1;
        var pageSize = 3;

        // Act
        var result = new PaginatedResult<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        result.Items.Should().BeEquivalentTo(items);
        result.TotalCount.Should().Be(totalCount);
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
    }

    [Fact]
    public void TotalPages_ShouldCalculateCorrectly()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>(new List<string>(), 25, 1, 10);

        // Assert
        result.TotalPages.Should().Be(3); // 25 / 10 = 2.5, ceiling = 3
    }

    [Fact]
    public void TotalPages_WhenPageSizeIsZero_ShouldReturnZero()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>
        {
            Items = new List<string>(),
            TotalCount = 10,
            PageNumber = 1,
            PageSize = 0
        };

        // Assert
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public void HasPreviousPage_WhenOnFirstPage_ShouldReturnFalse()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>(new List<string>(), 10, 1, 5);

        // Assert
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_WhenNotOnFirstPage_ShouldReturnTrue()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>(new List<string>(), 10, 2, 5);

        // Assert
        result.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_WhenOnLastPage_ShouldReturnFalse()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>(new List<string>(), 10, 2, 5); // 2 pages total, on page 2

        // Assert
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void HasNextPage_WhenNotOnLastPage_ShouldReturnTrue()
    {
        // Arrange & Act
        var result = new PaginatedResult<string>(new List<string>(), 10, 1, 5); // 2 pages total, on page 1

        // Assert
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Map_ShouldTransformItems()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };
        var source = new PaginatedResult<int>(items, 10, 1, 3);

        // Act
        var result = source.Map(x => x.ToString());

        // Assert
        result.Items.Should().BeEquivalentTo(new[] { "1", "2", "3" });
        result.TotalCount.Should().Be(10);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(3);
    }
}

