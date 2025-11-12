using BackEnd.DTOs;
using BackEnd.Entities;

namespace BackEnd.Extensions;

public static class StatusExtensions
{
    public static GetStatus ToDTO(this Status status)
    {
        return new GetStatus
        {
            Id = status.Id,
            Name = status.Name
        };
    }

    public static Status ToEntity(this PostStatus status)
    {
        return new Status()
        {
            Id = Guid.NewGuid(),
            Name = status.Name
        };
    }

    public static void Update(this Status status, PutStatus putStatus)
    {
        status.Name = putStatus.Name ?? status.Name;
    }
}