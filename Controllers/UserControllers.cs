using System.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public static class UserControllers
{
    public static IResult RegisterUser([FromBody] User user, [FromServices] UserRepository userRepository)
    {
        user.Name = user.Name.Trim();
        user.Email = user.Email.Trim();

        try
        {
            bool result = userRepository.InsertUser(user);

            return Results.Ok(new ApiResponse()
            {
                IsSuccess = true,
                Result = result,
                StatusCode = HttpStatusCode.Created,
            });
        }
        catch (MySqlException)
        {
            return Results.Problem("Couldn't be possible to register the user");
        }
    }

    public static IResult LoginUser([FromBody] User user, [FromServices] UserRepository userRepository, IConfiguration configuration)
    {
        try
        {
            User searchedUser = userRepository.GetUserByEmailAndPassword(user);

            if (searchedUser == null)
            {
                return Results.NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = ["Wrong e-mail or password"]
                });
            }

            string token = Token.Sign(configuration, searchedUser);

            return Results.Ok(new ApiResponse()
            {
                IsSuccess = true,
                Result = token,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (MySqlException)
        {
            return Results.Problem("Couldn't be possible to log-in the user");
        }
    }

    public static IResult GetUser(HttpContext httpContext)
    {
        string name = httpContext.User.Identity?.Name ?? string.Empty;

        return Results.Ok(new ApiResponse()
        {
            IsSuccess = true,
            Result = name,
            StatusCode = HttpStatusCode.OK
        });
    }
}