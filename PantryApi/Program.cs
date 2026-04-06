var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Serve static files from frontend folder
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
