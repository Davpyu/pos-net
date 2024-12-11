using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pos.Configs;
using Pos.Entities;

namespace Pos.Helpers;

public class DataContext : DbContext
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Variant> Variants { get; set; }
    private AppSettings _appSettings { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DataContext(DbContextOptions<DataContext> options, IOptions<AppSettings> appSettings) : base(options)
    {
        _appSettings = appSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (_appSettings != null)
        {
            // Use logger when db logging is enabled
            if (_appSettings.EnableDBLogging)
            {
                options.EnableSensitiveDataLogging();
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

        options.AddInterceptors(new AutofillDateTimeInterceptor());
    }
}