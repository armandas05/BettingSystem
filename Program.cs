using BettingSystem.Services;
using BettingSystem.Data;
using Microsoft.EntityFrameworkCore;
using BettingSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<DeckService>();
builder.Services.AddSingleton<IBlackjackService, BlackjackService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserStateService, UserStateService>();
builder.Services.AddScoped<DiceGameService>();
builder.Services.AddScoped<IDiceService, DiceService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// DB SEEDER FOR TESTING PURPOSES
// COMMENT CODE IF NOT NEEDED

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.Run();
