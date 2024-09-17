using Cassandra;
using SmartHealthMonitoring.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register the PatientSensorService as a singleton
builder.Services.AddSingleton<PatientSensorService>();

builder.Services.AddSingleton<Cassandra.ISession>(provider =>
{
    // Define your Cassandra cluster
    var cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1") // Replace with your Cassandra instance IP or hostname
        .WithPort(9042) // Default port for Cassandra
        .Build();

    // Create and return the session
    return cluster.Connect("your_keyspace_name"); // Replace with your keyspace name
});

builder.Services.AddSingleton<AggregatedSensorDataService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
