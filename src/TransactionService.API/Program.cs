using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Interfaces;
using TransactionService.Infrastructure.Data;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Persistence.UnitOfWork;
using TransactionService.Infrastructure.Repositories;
using TransactionService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventBus, RabbitMqEventBus>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(TransactionService.Application.Commands.CreateTransfer.CreateTransferCommand).Assembly));


var app = builder.Build();

app.UseGlobalExceptionHandling();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
