using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Api.Database;

namespace YtbSummarizer.Api.Installers;

public static class Database
{
	public static void InstallDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<UoW>(options => options.UseNpgsql(connectionString));
	}
}