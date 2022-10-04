using Business.Abstract;
using Business.Concreate;
using Data;
using Data.Abstract;
using Data.Concrete.EFCore;
using Eticaret.WebUI.EmailServices;
using Eticaret.WebUI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eticaret.WebUI
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlConnection")));
            services.AddDbContext<ShopContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlConnection")));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber =false;
            });
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".Eticare.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });
            //      services.AddScoped<IProductRepository,EfCoreProductRepository>();
            //services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            //      services.AddScoped<ICartRepository, EfCoreCartRepository>();
            //      services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductService,ProductManager>();
            services.AddScoped<ICategoryService,CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
            new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                _configuration["EmailSender:Username"],
                _configuration["EmailSender:Password"]
                )
            );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager,RoleManager<IdentityRole> roleManager,ICartService cartService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
            name: "orders",
            pattern: "orders",
            defaults: new { controller = "Cart", action = "GetOrders" });

                endpoints.MapControllerRoute(
            name: "checkout",
            pattern: "checkout",
            defaults: new { controller = "Cart", action = "Checkout" });

                endpoints.MapControllerRoute(
            name: "cart",
            pattern: "cart",
            defaults: new { controller = "Cart", action = "Index" });

                //Admin User
                endpoints.MapControllerRoute(
              name: "adminuseredit",
              pattern: "admin/user/{id?}",
              defaults: new { controller = "Admin", action = "UserEdit" });

                endpoints.MapControllerRoute(
            name: "adminusers",
            pattern: "admin/user/list",
            defaults: new { controller = "Admin", action = "UserList" });

                //admin Roles
                endpoints.MapControllerRoute(
                name: "adminroles",
                pattern: "admin/role/list",
                defaults: new { controller = "Admin", action = "RoleList" });

                endpoints.MapControllerRoute(
               name: "adminrolecreate",
               pattern: "admin/role/create",
               defaults: new { controller = "Admin", action = "RoleCreate" });

                endpoints.MapControllerRoute(
             name: "adminroleedit",
             pattern: "admin/role/{id?}",
             defaults: new { controller = "Admin", action = "RoleEdit" });

                //admin product category
                endpoints.MapControllerRoute(
                 name: "adminproducts",
                 pattern: "admin/products",
                 defaults: new { controller = "Admin", action = "ProductList" });

                endpoints.MapControllerRoute(
             name: "adminproductcreate",
             pattern: "admin/products/create",
             defaults: new { controller = "Admin", action = "ProductCreate" });

                endpoints.MapControllerRoute(
                name: "adminproductedit",
                pattern: "admin/products/{id?}",
                defaults: new { controller = "Admin", action = "ProductEdit" });

                endpoints.MapControllerRoute(
                name: "admincategories",
                pattern: "admin/categories",
                defaults: new { controller = "Admin", action = "CategoryList" });

                endpoints.MapControllerRoute(
               name: "admincategorycreate",
               pattern: "admin/categories/create",
               defaults: new { controller = "Admin", action = "CategoryCreate" });

                endpoints.MapControllerRoute(
              name: "admincategoryedit",
              pattern: "admin/categories/{id?}",
              defaults: new { controller = "Admin", action = "CategoryEdit" });

                //local/search
                endpoints.MapControllerRoute(
                   name: "search",
                   pattern: "search",
                   defaults: new { controller = "Shop", action = "search" });

                endpoints.MapControllerRoute(
                   name: "productsdetails",
                   pattern: "{url}",
                   defaults: new { controller = "Shop", action = "details" });

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "list" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            SeedIdentity.Seed(userManager,roleManager,cartService,configuration).Wait();
        }
    }
}
