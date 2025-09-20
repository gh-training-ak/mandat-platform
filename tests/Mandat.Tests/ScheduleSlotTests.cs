using FluentAssertions;
using Mandat.Domain.Entities;

namespace Mandat.Tests;

public sealed class ScheduleSlotTests
{
    private static ScheduleSlot Slot(int fromHour, int toHour, DayOfWeek day = DayOfWeek.Monday)
        => new() { Day = day, StartsAt = new TimeOnly(fromHour, 0), EndsAt = new TimeOnly(toHour, 0) };

    [Fact]
    public void AdjacentSlotsDoNotOverlap()
        => Slot(9, 10).Overlaps(Slot(10, 11)).Should().BeFalse();

    [Fact]
    public void PartiallyOverlappingSlotsOverlap()
        => Slot(9, 11).Overlaps(Slot(10, 12)).Should().BeTrue();

    [Fact]
    public void SlotsOnDifferentDaysNeverOverlap()
        => Slot(9, 11).Overlaps(Slot(9, 11, DayOfWeek.Tuesday)).Should().BeFalse();
}
