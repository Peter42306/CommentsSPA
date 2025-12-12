using CommentsSpaApi.Data;
using CommentsSpaApi.Services.Attachments;
using CommentsSpaApi.Services.Captcha;
using CommentsSpaApi.Services.Html;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICaptchaService, InMemoryCaptchaService>();
builder.Services.AddSingleton<IHtmlSanitizerService, HtmlSanitizerService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

var app = builder.Build();

var webRoot = app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
var uploadsRoot = Path.Combine(webRoot, "uploads");

Directory.CreateDirectory(uploadsRoot);
Directory.CreateDirectory(Path.Combine(uploadsRoot, "images"));
Directory.CreateDirectory(Path.Combine(uploadsRoot, "text"));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}


app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();