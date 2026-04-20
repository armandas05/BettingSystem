using BettingSystem.Services;
using BettingSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<DeckService>();
builder.Services.AddSingleton<BlackjackService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserStateService>();
builder.Services.AddScoped<DiceGameService>();
builder.Services.AddScoped<DiceService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<AnalyticsService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRazorPages();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

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


app.Run();

// DB SEEDER FOR TESTING PURPOSES
// COMMENT CODE IF NOT NEEDED

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();
//    DbSeeder.Seed(db);
//}