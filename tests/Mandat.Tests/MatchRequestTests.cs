using FluentAssertions;
using Mandat.Domain.Entities;

namespace Mandat.Tests;

public sealed class MatchRequestTests
{
    [Fact]
    public void NewRequest_IsPending()
        => new MatchRequest().Status.Should().Be(MatchStatus.Pending);

    [Fact]
    public void Accept_SetsStatusAndTimestamp()
    {
        var request = new MatchRequest();
        request.Accept();

        request.Status.Should().Be(MatchStatus.Accepted);
        request.AnsweredAt.Should().NotBeNull();
    }

    [Fact]
    public void AnsweringTwice_Throws()
    {
        var request = new MatchRequest();
        request.Accept();

        var act = request.Reject;

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*already answered*");
    }
}
