using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.Enums;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Exceptions.UserProfile;
using SocialNetworkApp.Business.Extensions;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;


namespace SocialNetworkApp.Business.Services.Implements;

public class UserProfileService(IUserProfileRepository _repo, AppDbContext _context) : IUserProfileService
{
    public Task<bool> CheckImage(ChangableUserProfileVM userProfileVM)
    {
        if (userProfileVM.ProfilePhoto != null)
        {
            if (!userProfileVM.ProfilePhoto.IsValidType("image"))
            {
                throw new InvalidTypeException();
            }
            if (!userProfileVM.ProfilePhoto.IsValidSize(200))
            {
                throw new InvalidSizeException();
            }
        }

        return null;
    }



    public async Task CreateAsync(ChangableUserProfileVM userProfileVM, string userId, string fileName)
    {
        var photoURL = default(string);
        if (fileName == "imgs/profilePhoto/download.png")
        {
            photoURL = fileName;
        }
        else
        {
            photoURL = Path.Combine("imgs", "profilePhoto", fileName);
        }

        UserProfile user = new UserProfile
        {
            Id = userProfileVM.Id,
            Name = userProfileVM.Name,
            Surname = userProfileVM.Surname,
            Gender = userProfileVM.Gender.ToString(),
            ProfilePhoto = photoURL,
            UserId = userId
        };

        await _repo.CreateAsync(user);
        await _repo.SaveAsync();
    }

    public async Task<SettingsCurier> GetUserProfileById(string UserId)
    {
        if(UserId == null) throw new NotFountException("Bad Request!");

        SettingsCurier curier = new SettingsCurier();

        var user = await _repo.GetFirstAsync(x => x.UserId == UserId);

        if (user == null) throw new NotFountException("User Not Found!");

        curier.GetUserProfileUpdateVM.Name = user.Name;
        curier.GetUserProfileUpdateVM.Surname = user.Surname;
        curier.GetUserProfileUpdateVM.About = user.About;
        curier.GetUserProfileUpdateVM.Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), user.Gender);
        curier.GetUserProfileUpdateVM.CurrentWorkAt = user.CurrentWorkAt;
        curier.GetUserProfileUpdateVM.CurrentStudyUni = user.CurrentStudyUni;

        return curier;

    }

    public async Task UpdateUserProfilePhotoById(string UserId, string? fileName)
    {
        if (UserId == null) throw new NotFountException("Bad Request!");

        var user = await _repo.GetFirstAsync(u => u.UserId == UserId);

        if (user == null) throw new NotFountException("User Not Found!");

        var photoURL = default(string);

        if (fileName == null)
        {
            photoURL = "~/imgs/profilePhoto/download.png";
            user.ProfilePhoto = photoURL;
        }
        else if(fileName != user.ProfilePhoto)
        {
            photoURL = Path.Combine("imgs", "profilePhoto", fileName);
            user.ProfilePhoto = photoURL;
        }

        await _repo.SaveAsync();
    }

    public async Task<SettingsCurier> UpdateUserProfileById(SettingsCurier curier, string UserId)
    {
        if (UserId == null) throw new NotFountException("Bad Request!");

        var user = await _repo.GetFirstAsync(u => u.UserId == UserId);

        if (user == null) throw new NotFountException("User Not Found!");

        if(curier.UserProfileVM.About != null)
        {
            user.Name = curier.UserProfileVM.Name;
            user.Surname = curier.UserProfileVM.Surname;
            user.About = curier.UserProfileVM.About;
            user.Gender = curier.UserProfileVM.Gender.ToString();
            user.CurrentWorkAt = curier.UserProfileVM.CurrentWorkAt;
            user.CurrentStudyUni = curier.UserProfileVM.CurrentStudyUni;
        }
        else
        {
            user.Name = curier.UserProfileVM.Name;
            user.Surname = curier.UserProfileVM.Surname;
            user.Gender = curier.UserProfileVM.Gender.ToString();
            user.CurrentWorkAt = curier.UserProfileVM.CurrentWorkAt;
            user.CurrentStudyUni = curier.UserProfileVM.CurrentStudyUni;
        }

        

        await _repo.SaveAsync();

        return curier;
    }

    public async Task UpdateUserLocationById(SettingsCurier curier, string UserId)
    {
        if (UserId == null) throw new NotFountException("Bad Request!");

        var user = await _context.userLocations.FirstOrDefaultAsync(u => u.UserId == UserId);

        if (user == null) throw new NotFountException("User Not Found!");

        user.Country = curier.ChangableUserLocationVM.Country;
        user.State = curier.ChangableUserLocationVM.State;
        user.City = curier.ChangableUserLocationVM.City;

        await _repo.SaveAsync();


    }
}
