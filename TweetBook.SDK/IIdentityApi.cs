using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Request.Auth;
using TweetBook.Contracts.V1.Responses.Auth;

namespace TweetBook.SDK
{
    public interface IIdentityApi
    {
        [Post("/" + ApiRoutes.Identity.Register)]
        Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);

        [Post("/" + ApiRoutes.Identity.Login)]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest loginRequest);

        [Post("/" + ApiRoutes.Identity.Refresh)]
        Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest refreshRequest);
    }
}
