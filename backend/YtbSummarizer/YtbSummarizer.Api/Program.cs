using YtbSummarizer.Api;

var builder = WebApplication.CreateBuilder(args);

// Create an instance of the Startup class
var startup = new Startup(builder.Configuration);

// Invoke ConfigureServices from the Startup class
startup.ConfigureServices(builder.Services);

// Build the app
var app = builder.Build();

// Invoke Configure from the Startup class
startup.Configure(app, app.Environment);

// Run the app
app.Run();
