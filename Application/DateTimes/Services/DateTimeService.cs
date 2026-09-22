using Application.DateTimes.Interfaces;

namespace Application.DateTimes.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }
}
