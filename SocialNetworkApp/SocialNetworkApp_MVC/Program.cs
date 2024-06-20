using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.Hubs;
using SocialNetworkApp.Business.Services.Implements;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Implements;
using SocialNetworkApp.DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Database Connection
builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")
        )
    );

//Identity User
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredLength = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;

    opt.SignIn.RequireConfirmedEmail = true;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//SignalR
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(x => true)));
builder.Services.AddSignalR();

//Repository Scope
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserSettingRepository, UserSettingRepository>();
builder.Services.AddScoped<IUserLocationRepository, UserLocationRepository>();

//Service Scope
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserLocationService, UserLocationService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();





builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseCors();


app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


app.MapHub<ChatHub>("/chathub");



app.Run();

