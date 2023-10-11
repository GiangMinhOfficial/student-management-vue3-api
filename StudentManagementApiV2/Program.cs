﻿using Microsoft.EntityFrameworkCore;
using StudentManagementApiV2.Data;
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

builder.Services.AddDbContext<StudentManagementApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagementApiContext") ?? throw new InvalidOperationException("Connection string 'StudentManagementApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
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
