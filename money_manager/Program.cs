using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using money_manager.Entities;
using money_manager.Repositories;
using money_manager.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AuthorizationPolicy authenticatedUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
});

//Repositorios
builder.Services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

//servicios
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IUserStore<UserEntity>, UserStore>();
builder.Services.AddIdentityCore<UserEntity>(option =>
{
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
})
    .AddUserManager<UserManager<UserEntity>>()
    .AddSignInManager<SignInManager<UserEntity>>()
    .AddErrorDescriber<ErrorMessageIdentity>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Login";
});

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/NotFoundPage");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();
