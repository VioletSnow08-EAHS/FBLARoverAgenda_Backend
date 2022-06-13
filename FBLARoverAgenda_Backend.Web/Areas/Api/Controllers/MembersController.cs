using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FBLARoverAgenda_Backend.Domain.DTOs.Authentication;
using FBLARoverAgenda_Backend.Web.Areas.Api.Models;
using System.Threading.Tasks;
using FBLARoverAgenda_Backend.Infrastructure.Authentication.Services;


namespace FBLARoverAgenda_Backend.Web.Areas.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Area("Api")]
public class MembersController : Controller
{
    private readonly IUserService _userService;

    public MembersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Authenticate a member
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ApiResponse> Authenticate(AuthenticateRequest model)
    {
        var member = await _userService.Authenticate(model);

        if (member == null)
            return new ApiResponse(System.Net.HttpStatusCode.NotFound, model,"Username or password is incorrect");

        return new ApiResponse(System.Net.HttpStatusCode.OK, member);
    }
}