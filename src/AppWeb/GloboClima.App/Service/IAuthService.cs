using GloboClima.App.Configuration.Authentication.ViewModels;

namespace GloboClima.App.Service
{
    public interface IAuthService
    {
        Task<ApiResponse<UserLoginResponse>> Login(LoginUserViewModel viewModel);
        Task<ApiResponse<UserLoginResponse>> Register(RegisterUserViewModel viewModel);
        Task DoLogin(ApiResponse<UserLoginResponse> resposta);
    }
}
