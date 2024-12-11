using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pos.Configs;
using Pos.Helpers;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "AllowAll", builder =>
    {
        builder
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(
options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("id-ID"),
        new("en-US"),
    };

    options.DefaultRequestCulture = new RequestCulture("id-ID");
    // Formatting numbers, dates, etc.
    options.SupportedCultures = supportedCultures;
    // UI strings that we have localized.
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
    {
        var languages = context.Request.Headers["Accept-Language"].ToString();
        var currentLanguage = languages.Split(',').FirstOrDefault();
        var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "id-ID" : currentLanguage;

        if (defaultLanguage != "id-ID" && defaultLanguage != "en-US")
        {
            defaultLanguage = "id-ID";
        }

        return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
    }));
});

builder.Services.AddControllers();

string dbConfig = builder.Configuration.GetConnectionString("PosDatabase");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(dbConfig, ServerVersion.AutoDetect(dbConfig));
});

string redisConfig = builder.Configuration.GetConnectionString("Redis") + ",abortConnect=false";

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfig;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Pos Web API",
        Description = ".NET Web API for Pos App",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "David Surya Fadilla",
            Email = string.Empty,
            Url = new Uri("https://google.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Web API for Pos App");
    });
}

app.UseCors("AllowAll");

app.Run();