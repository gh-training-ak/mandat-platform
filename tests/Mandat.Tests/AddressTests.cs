using FluentAssertions;
using Mandat.Domain.Entities;

namespace Mandat.Tests;

public sealed class AddressTests
{
    private static readonly Address Belfast = new("City Hall", "Belfast", "BT1 5GS", 54.5967, -5.9301);
    private static readonly Address Bucharest = new("Piata Unirii", "Bucuresti", "030167", 44.4268, 26.1025);

    [Fact]
    public void DistanceToItself_IsZero()
        => Belfast.DistanceKmTo(Belfast).Should().BeApproximately(0, 0.001);

    [Fact]
    public void DistanceIsSymmetric()
        => Belfast.DistanceKmTo(Bucharest).Should().BeApproximately(Bucharest.DistanceKmTo(Belfast), 0.001);

    [Fact]
    public void BelfastToBucharest_IsAboutTwoThousandFiveHundredKilometres()
        => Belfast.DistanceKmTo(Bucharest).Should().BeInRange(2400, 2600);
}
