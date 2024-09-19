using BookAPIDemo.Items;
using BookAPIDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          

                      });
});

builder.Services.AddDbContext<BookDbContext>(options => options.UseSqlServer(
 builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:\to-delete"),
    RequestPath = "/StaticFiles"
});




app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.Run();
