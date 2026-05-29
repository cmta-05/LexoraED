using LexoraED.Data;
using LexoraED.Models;
using LexoraED.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LexoraEDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LexoraEDConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<LexoraEDContext>()
.AddDefaultTokenProviders()
.AddClaimsPrincipalFactory<LexoraUserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddScoped<ActivityLogService>();
builder.Services.AddScoped<GamificationService>();
builder.Services.AddScoped<QuizScoringService>();
builder.Services.AddScoped<AdaptiveLearningPathService>();
builder.Services.AddScoped<LearningPathPresentationService>();
builder.Services.AddScoped<IdentityAdminService>();
builder.Services.AddScoped<TeacherInsightsService>();
builder.Services.AddScoped<LexoraReportService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LexoraEDContext>();
    await context.Database.MigrateAsync();
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);
    LexoraEDMinimalCurriculumSeed.Apply(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
