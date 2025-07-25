using GestionTareas.API.Consumer;
using GestionTareas.Modelos.DTOs;

namespace GestionTareas.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiUrl = builder.Configuration["ApiUrl"] ?? "https://localhost:7149/";

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<Crud<UsuarioDTOs>>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                var baseUrl = "https://localhost:7149";
                var endpoint = "autorizaciones";
                return new Crud<UsuarioDTOs>(httpClient, baseUrl, endpoint);
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
