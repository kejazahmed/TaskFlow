
using TaskFlow.WebAPI.Infrastructure;
using TaskFlow.WebAPI.Middleware;
using TaskFlow.WebAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Ensure the logs directory exists.
var logDirectory = Path.Combine(
    builder.Environment.ContentRootPath, "logs");

Directory.CreateDirectory(logDirectory);

// Configure log4net.
builder.Logging.AddLog4Net("log4net.config");

// Add services.
const string TaskFlowCorsPolicy = "TaskFlow-Policy";

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
    options.AddPolicy(TaskFlowCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure middleware.
app.UseExceptionHandler();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors(TaskFlowCorsPolicy);

app.MapControllers();

// Verify that logging works.
app.Logger.LogInformation("Task Flow Web API started.");

app.Run();
