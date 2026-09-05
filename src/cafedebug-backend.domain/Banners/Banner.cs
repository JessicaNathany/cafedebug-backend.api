using cafedebug_backend.domain.Shared;
using static cafedebug_backend.domain.Banners.BannerStatus;

namespace cafedebug_backend.domain.Banners;

public class Banner : Entity
{
    private TimeProvider _timeProvider = TimeProvider.System;

    public string Name { get; private set; }
    public string UrlImage { get; private set; }
    public string? Url { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool Active { get; private set; }
    public int Order { get; private set; }

    public BannerStatus Status
    {
        get => field == Draft || field == Archived
            ? field
            : _timeProvider.GetLocalNow().DateTime < StartDate ? Published : Scheduled;
        set;
    }

    private Banner() { }
    public Banner(
        string name,
        string urlImage, 
        string url, 
        DateTime startDate, 
        DateTime endDate,
        BannerStatus status,
        bool active, 
        int ordem,
        TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
        var now = _timeProvider.GetLocalNow().DateTime;
        Name = name;
        UrlImage = urlImage;
        Url = url;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        CreatedAt = now;
        Active = active;
        EndDateVerify(endDate);
        Order = ordem;
    }

    public void Update(
        string name, 
        string urlImage, 
        string url, 
        DateTime startDate, 
        DateTime endDate,
        BannerStatus status,
        bool active, 
        int ordem,
        TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
        var now = _timeProvider.GetLocalNow().DateTime;
        Name = name;
        UrlImage = urlImage;
        Url = url;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        UpdatedAt = now;
        Active = active;
        EndDateVerify(endDate);
        Order = ordem;
    }
    public void EndDateVerify(DateTime endDate)
    {
        if (endDate == _timeProvider.GetLocalNow().DateTime.AddDays(-1))
            Active = false;
    }
}
