using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded) 
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                    {
                        string token = _tokenService.CreateToken(appUser);
                        System.Diagnostics.Debug.WriteLine(appUser.ToString);
                        return Ok(
                            new UserDto
                            {
                                Email = appUser.Email,
                                UserName = appUser.UserName,
                                Token = token,
                            }
                            );
                    }else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(400, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(501, ex);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if(!result.Succeeded)
                return Unauthorized("Invalid email or password");

            string token = _tokenService.CreateToken(user);
            return Ok(
                new UserDto
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Token = token,
                }
                );
        }
    }
}
