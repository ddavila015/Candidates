using DataAccess;
using DataAccess.Data;
using DataAccess.DataInterface;
using Domain;
using Domain.DomainInterface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DEPENDENCIAS
builder.Services.AddScoped<ICandidateData, CandidateData>();
builder.Services.AddScoped<ICandidateDomain, CandidateDomain>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CandidateContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CandidateConnection"));
});

 var app = builder.Build();

//DESCOMENTAR ANTES DE ENVIAR
//using (var scope = app.Services.CreateScope()) 
//{
//    var dataContext = scope.ServiceProvider.GetRequiredService<CandidateContext>();
//    dataContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
