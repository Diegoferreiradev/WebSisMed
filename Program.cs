using FluentValidation;
using WebSisMed.Models.Contexts;
using WebSisMed.Validators.Consultas;
using WebSisMed.Validators.Medicos;
using WebSisMed.Validators.MonitoramentoPaciente;
using WebSisMed.Validators.Pacientes;
using WebSisMed.ViewModels.Consultas;
using WebSisMed.ViewModels.Medicos;
using WebSisMed.ViewModels.MonitoramentoPaciente;
using WebSisMed.ViewModels.Pacientes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddDbContext<SisMedContext>();
builder.Services.AddScoped<IValidator<AdicionarMedicoViewModel>, AdicionarMedicoValidator>();
builder.Services.AddScoped<IValidator<EditarMedicoViewModel>, EditarMedicoValidator>();
builder.Services.AddScoped<IValidator<AdicionarPacienteViewModel>, AdicionarPacienteValidator>();
builder.Services.AddScoped<IValidator<EditarPacienteViewModel>, EditarPacienteValidator>();
builder.Services.AddScoped<IValidator<AdicionarMonitoramentoViewModel>, AdicionarMonitoramentoValidator>();
builder.Services.AddScoped<IValidator<EditarMonitoramentoViewModel>, EditarMonitoramentoValidator>();
builder.Services.AddScoped<IValidator<AdicionarConsultaViewModel>, AdicionarConsultaValidator>();
builder.Services.AddScoped<IValidator<EditarConsultaViewModel>, EditarConsultaValidator>();


var app = builder.Build();

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
