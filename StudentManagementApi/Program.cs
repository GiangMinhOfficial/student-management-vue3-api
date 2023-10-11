using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.Services.StudentService;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var StudentManagementUI = "_enablecors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: StudentManagementUI,
        policy =>
        {
            policy.WithOrigins("http://localhost:8080", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

//apply CONTEXT POOLING by changing from adddbcontext to AddDbContextPool
builder.Services.AddDbContextPool<StudentManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagementContext")
        ?? throw new InvalidOperationException("Connection string 'StudentManagementContext' not found.")));
builder.Services.AddScoped<IStudentService, StudentService>();

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(StudentManagementUI);

app.UseAuthorization();

app.MapControllers();

app.Run();
