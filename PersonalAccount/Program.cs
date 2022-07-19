using AspNetCoreCsvImportExport.Formatters;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using PersonalAccount;
using PersonalAccount.Data;
using PersonalAccount.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

var builder = WebApplication.CreateBuilder(args);

var csvFormatterOptions = new CsvFormatterOptions();
csvFormatterOptions.CsvDelimiter = ",";

builder.Services.AddResponseCompression(options => 
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/csv" });
});


// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
    options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
}).AddNewtonsoftJson();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPersonalAccService, PersonalAccService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AirCompaniesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TicketContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHsts();

app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .Build();
});

app.MapControllers();

app.Run();