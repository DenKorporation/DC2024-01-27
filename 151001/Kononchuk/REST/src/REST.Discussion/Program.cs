using Asp.Versioning;
using FluentValidation;
using REST.Discussion.Data;
using REST.Discussion.Exceptions.Handler;
using REST.Discussion.Models.Entities;
using REST.Discussion.Repositories.Implementations;
using REST.Discussion.Repositories.Interfaces;
using REST.Discussion.Services.Implementations;
using REST.Discussion.Services.Interfaces;
using REST.Discussion.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<CassandraContext>(_ =>
    new CassandraContext(builder.Configuration["Cassandra:connectionString"],
        builder.Configuration["Cassandra:keyspace"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Services Registration

builder.Services.AddTransient<INoteService, NoteService>();

// Validators Registration

builder.Services.AddTransient<AbstractValidator<Note>, NoteValidator>();

// Repository Registration

builder.Services.AddScoped<INoteRepository<NoteKey>, NoteRepository>();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program;