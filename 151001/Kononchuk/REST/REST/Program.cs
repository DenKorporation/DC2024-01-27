using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using REST.Data;
using REST.Models.Entities;
using REST.Repositories.Implementations.Memory;
using REST.Repositories.Interfaces;
using REST.Services.Implementations;
using REST.Services.Interfaces;
using REST.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<AppDbContext>(opt =>
//     opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddTransient<IEditorService, EditorService>();
builder.Services.AddTransient<IIssueService, IssueService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddTransient<ITagService, TagService>();

// Validators Registration

builder.Services.AddTransient<AbstractValidator<Editor>, EditorValidator>();
builder.Services.AddTransient<AbstractValidator<Issue>, IssueValidator>();
builder.Services.AddTransient<AbstractValidator<Note>, NoteValidator>();
builder.Services.AddTransient<AbstractValidator<Tag>, TagValidator>();

// Repository Registration

if (bool.Parse(builder.Configuration["UseInMemoryRepositories"] ?? "true"))
{
    builder.Services.AddSingleton<IEditorRepository<long>, EditorRepository>();
    builder.Services.AddSingleton<IIssueRepository<long>, IssueRepository>();
    builder.Services.AddSingleton<INoteRepository<long>, NoteRepository>();
    builder.Services.AddSingleton<ITagRepository<long>, TagRepository>();
}
else
{
    // builder.Services.AddSingleton<IRepository<long, Editor>, EditorRepository>();
    // builder.Services.AddSingleton<IRepository<long, Issue>, IssueRepository>();
    // builder.Services.AddSingleton<IRepository<long, Note>, NoteRepository>();
    // builder.Services.AddSingleton<IRepository<long, Tag>, TagRepository>();
    // builder.Services.AddSingleton<IIssueTagRepository<long>, IssueTagRepository>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();