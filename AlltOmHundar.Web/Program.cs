using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Infrastructure.Repositories;
using AlltOmHundar.Services;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlltOmHundar.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IGroupMessageRepository, GroupMessageRepository>();

            // Register Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IReactionService, ReactionService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPrivateMessageService, PrivateMessageService>();
            builder.Services.AddScoped<IGroupService, GroupService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
        }
    }
}