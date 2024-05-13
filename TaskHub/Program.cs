using TaskHub.Business;
using TaskHub.Data;
using TaskHub.Extensions;

var builder = WebApplication.CreateBuilder(args);

var conf = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var env = conf["ASPNETCORE_ENVIRONMENT"];

//configure appsettings file
builder.WebHost.ConfigureAppConfiguration((a, config) =>
{
    config.AddJsonFile("appsettings.json");
    config.AddJsonFile(string.Format("appsettings.{0}.json", env));
});

//Add Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//Add cors policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


//Add Database
builder.Services.AddTaskHubDatabase(builder.Configuration, (string)System.Reflection.Assembly.GetExecutingAssembly().FullName);
//Add Repositories
builder.Services.AddTaskHubRepositories();
// Add Gateway
builder.Services.AddTaskHubGateway();

//Add JWT Token
builder.Services.AddTokenJWT(builder.Configuration);

// Add Business Services
builder.Services.AddTaskHubBusinessServices(builder.Configuration);

// Add others services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGenerationDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heldis Api V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
