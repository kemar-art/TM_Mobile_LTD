global using OnDiSpotShop.Shared;
global using System.Net.Http.Json;
global using OnDiSpotShop.Client.Services.ProductServices;
global using OnDiSpotShop.Client.Services.CategoryServices;
global using OnDiSpotShop.Shared.DTOs;
global using OnDiSpotShop.Shared.Modles;
global using Blazored.LocalStorage;
global using OnDiSpotShop.Client;
global using OnDiSpotShop.Client.Services.CartServices;
global using OnDiSpotShop.Client.Services.AuthServices;
global using OnDiSpotShop.Client.Services.OrderServices;
global using OnDiSpotShop.Client.Services.AddressServices;
global using OnDiSpotShop.Client.Services.ProductTypeServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStareProvider>();


await builder.Build().RunAsync();
