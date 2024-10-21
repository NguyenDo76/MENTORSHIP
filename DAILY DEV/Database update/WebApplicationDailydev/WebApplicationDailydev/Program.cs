//using WebApplicationDailydev.Repository;
//using Quartz;
//using Quartz.Impl;
//using Quartz.Spi;

//var builder = WebApplication.CreateBuilder(args);
//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddHttpClient();
//builder.Services.AddScoped<SourceRepository>();
//builder.Services.AddScoped<CategoriesRepository>();

//builder.Services.AddScoped<NewsRepository>(provider => new NewsRepository(connectionString));
//builder.Services.AddControllers();
//var app = builder.Build();



//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using WebApplicationDailydev.Repository;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using static WebApplicationDailydev.Repository.NewsRepository;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

// Add Quartz services
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddScoped<RssFetchJob>(); // Đổi sang Scoped
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(RssFetchJob),
    cronExpression: "0 0/10 * * * ?")); // Run every 10 minutes

builder.Services.AddHostedService<QuartzHostedService>();
builder.Services.AddScoped<SourceRepository>();
builder.Services.AddScoped<CategoriesRepository>();
builder.Services.AddScoped<NewsRepository>(provider => new NewsRepository(connectionString, provider.GetRequiredService<HttpClient>()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();