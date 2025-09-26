using FluentAssertions;
using Mandat.Domain.Entities;

namespace Mandat.Tests;

// Regression cover for #71: short distances were rounding to zero.
public sealed class DistanceRegressionTests
{
    [Fact]
    public void ShortDistancesAreNotRoundedToZero()
    {
        var a = new Address("Queens University", "Belfast", "BT7 1NN", 54.5844, -5.9344);
        var b = new Address("Botanic Gardens", "Belfast", "BT7 1LP", 54.5826, -5.9335);

        a.DistanceKmTo(b).Should().BeGreaterThan(0.1).And.BeLessThan(0.5);
    }
}
