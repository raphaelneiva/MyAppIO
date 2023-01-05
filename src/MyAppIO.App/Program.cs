using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MyAppIO.App.Data;
using MyAppIO.App.Extensions;
using MyAppIO.Business.Interfaces;
using MyAppIO.Data.Context;
using MyAppIO.Data.Repository;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Guardando a connection string do arquivo appSettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adicionando o suporte ao acesso ao DB do Identity via EF
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<MyAppIOContext>(options =>
    options.UseSqlServer(connectionString));

// Adicionando a tela de erro de banco de dados (para desenvolvimento)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adicionando o Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adicionando o MVC

builder.Services.AddControllersWithViews();
builder.Services.AddMvc(o =>
{
    string invalidValueMsg = "O valor preenchido é inválido para este campo.";
    string beNumericMsg = "O campo deve ser numérico.";
    string requiredValueMsg = "Este campo precisa ser preenchido.";
    string bodyRequiredMsg = "É necessário que o body na requisição não esteja vazio.";

    o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => invalidValueMsg);
    o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => requiredValueMsg);
    o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => requiredValueMsg);
    o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => bodyRequiredMsg);
    o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => invalidValueMsg);
    o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => invalidValueMsg);
    o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => beNumericMsg);
    o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => invalidValueMsg);
    o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => invalidValueMsg);
    o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => beNumericMsg);
    o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => requiredValueMsg);
});

builder.Services.AddScoped<MyAppIOContext>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();


var app = builder.Build();

var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
