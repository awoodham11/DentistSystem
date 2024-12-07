using BlazorApp1.Identity;
using BlazorApp1.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri("https://localhost:5070/")
});





if (builder.Environment.IsDevelopment())
{
	var handler = new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
	};
	builder.Services.AddHttpClient("MyClient", client =>
	{
		client.BaseAddress = new Uri("https://localhost:5070");
	}).ConfigurePrimaryHttpMessageHandler(() => handler);
}
else
{
	builder.Services.AddHttpClient("MyClient", client =>
	{
		client.BaseAddress = new Uri("https://localhost:5070");
	});
}




builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
