using DotNetEnv;
using Training.Api.Configurations;
using Training.Api.Middleware;
using Training.BusinessLogic;
using Training.BusinessLogic.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Đăng kí global exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCoreDependencies(config);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

// Cấu hình Exception Handling
app.UseExceptionHandler();
app.UseStaticFiles();

// Sử dụng CORS
app.UseCors("AllowAllOrigins"); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://0.0.0.0:8000");

app.UseAuthorization();

app.UseMiddleware<GlobalMiddleware>();
TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
DateTimeOffset.Now.ToOffset(vietnamTimeZone.BaseUtcOffset);

app.UseRouting();
app.MapControllers();

app.RunMigration();

app.Run();
