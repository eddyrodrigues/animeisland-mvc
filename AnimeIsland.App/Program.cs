using AnimeIsland.App.Conf;
using AnimeIsland.Bussiness.AutoMapper;
using AnimeIsland.Data.Context;
using AnimeIsland.Data.Contracts;
using AnimeIsland.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AnimeIslandDbContext>(c =>
{
    //c.UseInMemoryDatabase("animeisland");
    c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<AnimeIslandDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAnimeRepository, AnimesRepository>();
builder.Services.AddScoped<IDiretoresRepository, DiretoresRepository>();
builder.Services.AddAutoMapper(c => c.AddProfile(new AnimeAutoMapperProfile()));
builder.Services.AddRazorPages();
//builder.Services.AddAuthorization(o =>
//{
//    o.AddPolicy("PodeLer", a =>
//    {
//        a.Requirements.Add(new PermiteAcessoFilter(new Claim("Animes", "Ler")));
//    });
//});

builder.Services.AddMvc().ConfigureApiBehaviorOptions(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.ConfigureApplicationCookie(options =>
         {
             options.LoginPath = $"/minha-conta/login";
             options.LogoutPath = $"/minha-conta/logout";
             //options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
         });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) //Production
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    //app.UseHsts();
}
else //Development
{
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
}
app.UseGlobalizationConfig();
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
