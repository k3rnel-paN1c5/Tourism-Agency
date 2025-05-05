using System;

namespace Application.DTOs.Trip;

public class GetTripDTO
{
    public int Id { get; set; }
    public string? Name {get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsPrivate { get; set; }

}
