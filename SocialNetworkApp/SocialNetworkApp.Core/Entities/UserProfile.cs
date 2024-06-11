﻿namespace SocialNetworkApp.Core.Entities;

public class UserProfile : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Gender { get; set; }
    public string? About { get; set; }
    public string? CurrentWorkAt { get; set; }
    public string? CurrentStudyUni {  get; set; }

    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public string? UserLocationId { get; set; }
    public UserLocation? UserLocation { get; set; }

    public ICollection<UserSettings>? UserProfiles { get; set; }


}