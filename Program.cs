using AzureLabStorageProject.Models;
using AzureLabStorageProject.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddTransient<IFileService, FileShareService>();
builder.Services.AddTransient<IBlobStorage, BlobStorageService>();
builder.Services.AddTransient<IQueueService, QueueService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroceryStoreAPI V1"));
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
