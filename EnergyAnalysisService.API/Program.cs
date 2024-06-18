using System.Text;
using EnergyAnalysisService.API;
using EnergyAnalysisService.DataAccess.Context;
using EnergyAnalysisService.DataAccess.Repository;
using EnergyAnalysisService.DataAccess.Repository.IRepository;
using EnergyAnalysisService.Models.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<EnergyAnalysisServiceContext>();
builder.Services.AddAuthorization();


var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = true,
        ValidIssuer= "https://energymaster-api.com",
        ValidAudience= "dotnetmastery.com",
        ValidateAudience = true,
        ClockSkew= TimeSpan.Zero,
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddDbContext<EnergyAnalysisServiceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

