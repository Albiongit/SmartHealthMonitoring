using Cassandra;
using IoT_Health_Monitoring.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Cassandra.ISession>(provider =>
{
    var cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1")
        .Build();
    var session = cluster.Connect();
    return cluster.Connect("health_data");
});

builder.Services.AddSingleton<DataGeneratorService>();
builder.Services.AddSingleton<SimulatorService>();
builder.Services.AddSingleton<DataService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.MapControllers();
app.Run();