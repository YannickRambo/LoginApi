var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Connection>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/register", UserControllers.RegisterUser).AddEndpointFilter<UserDataFilter>().AddEndpointFilter<UserConflictEmailFilter>();

app.MapPost("/login", UserControllers.LoginUser);

app.MapGet("/home", UserControllers.GetUser).RequireAuthorization();

app.Run();
