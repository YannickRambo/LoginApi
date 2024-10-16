
using System.Net;
using MySql.Data.MySqlClient;

public class UserConflictEmailFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        User user = context.GetArgument<User>(0);
        UserRepository userRepository = context.GetArgument<UserRepository>(1);

        try
        {
            User searchedUser = userRepository.GetUserByEmail(user);

            if (searchedUser != null)
            {
                return Results.Conflict(new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Conflict,
                    ErrorMessages = ["E-mail already registered!"]
                });
            }

            return await next(context);
        }
        catch (MySqlException)
        {
            return Results.Problem("Couldn't be possible to find the user");
        }
    }
}