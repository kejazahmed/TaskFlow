using TaskFlow.WebAPI.Infrastructure;
using TaskFlow.WebAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string TaskFlowCorsPolicy = "TaskFlow-Policy";

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options => options.AddPolicy(TaskFlowCorsPolicy, policy =>
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod()));


builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors(TaskFlowCorsPolicy);
app.MapControllers();
app.Run();

