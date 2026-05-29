var builder = WebApplication.CreateBuilder(args);

// create service and configuration variables 
var services = builder.Services; 
var configurations = builder.Configuration;




services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();                    //Enables authentication middleware
app.UseAuthorization();
app.MapControllers();

app.Run();
