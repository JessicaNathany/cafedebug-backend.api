using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Banners;

public class Banner : Entity
{
    public string Name { get; private set; }
    public string UrlImage { get; private set; }
    public string? Url { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Active { get; private set; }
    public int Order { get; private set; }

    public Banner() { }
    public Banner(
        string name,
        string urlImage, 
        string url, 
        DateTime startDate, 
        DateTime endDate,
        bool active, 
        int ordem)
    {
        Code = Guid.NewGuid();
        Name = name;
        UrlImage = urlImage;
        Url = url;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = DateTime.Now;
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
        bool active, 
        int ordem)
    {
        Name = name;
        UrlImage = urlImage;
        Url = url;
        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTime.Now;
        Active = active;
        EndDateVerify(endDate);
        Order = ordem;
    }
    public void EndDateVerify(DateTime endDate)
    {
        if (endDate == DateTime.Now.AddDays(-1))
            Active = false;
    }
}