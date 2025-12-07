using FluentAssertions;
using Shared.DTOs;

namespace ERP.UnitTests.DTOs;

public class BaseFilterDtoTests
{
    [Fact]
    public void PageNumber_WhenSetToNegative_ShouldBeSetToOne()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageNumber = -5 };

        // Assert
        filter.PageNumber.Should().Be(1);
    }

    [Fact]
    public void PageNumber_WhenSetToZero_ShouldBeSetToOne()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageNumber = 0 };

        // Assert
        filter.PageNumber.Should().Be(1);
    }

    [Fact]
    public void PageNumber_WhenSetToPositive_ShouldKeepValue()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageNumber = 5 };

        // Assert
        filter.PageNumber.Should().Be(5);
    }

    [Fact]
    public void PageSize_WhenExceedsMax_ShouldBeSetToMax()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageSize = 500 };

        // Assert
        filter.PageSize.Should().Be(100); // MaxPageSize is 100
    }

    [Fact]
    public void PageSize_WhenSetToNegative_ShouldBeSetToDefault()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageSize = -5 };

        // Assert
        filter.PageSize.Should().Be(10); // Default is 10
    }

    [Fact]
    public void PageSize_WhenSetToZero_ShouldBeSetToDefault()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageSize = 0 };

        // Assert
        filter.PageSize.Should().Be(10); // Default is 10
    }

    [Fact]
    public void PageSize_WhenSetToValidValue_ShouldKeepValue()
    {
        // Arrange & Act
        var filter = new BaseFilterDto { PageSize = 50 };

        // Assert
        filter.PageSize.Should().Be(50);
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var filter = new BaseFilterDto();

        // Assert
        filter.PageNumber.Should().Be(1);
        filter.PageSize.Should().Be(10);
        filter.SearchTerm.Should().BeNull();
        filter.SortBy.Should().BeNull();
        filter.SortDescending.Should().BeFalse();
        filter.CreatedFrom.Should().BeNull();
        filter.CreatedTo.Should().BeNull();
        filter.IncludeDeleted.Should().BeFalse();
    }
}

