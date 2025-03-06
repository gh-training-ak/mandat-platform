using Mandat.Application.Abstractions;
using Mandat.Application.Services;
using Mandat.Infrastructure.Persistence;
using Mandat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MandatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Mandat")));

builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<MentorSearchService>();
builder.Services.AddScoped<MatchRequestService>();
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddDbContextCheck<MandatDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();

public partial class Program;
