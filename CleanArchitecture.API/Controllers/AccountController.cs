﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CleanArchitecture.API.Helpers;
using CleanArchitecture.API.Models;
using CleanArchitecture.API.ViewModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace CleanArchitecture.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Guestbook> _repo;
        private readonly AppSettings _appSettings;
        private readonly IAppLogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IOptions<AppSettings> appSettings,
            IAppLogger<AccountController> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _appSettings = appSettings.Value;
            //_logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(user);
            }

            throw new ApplicationException(result.Errors.ToString());
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.RequiresTwoFactor)
            {
                //return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            }
            if (result.Succeeded)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                return await GenerateJwtToken(user);
            }

            throw new ApplicationException();
        }

        [Authorize]
        [HttpPost("AddClaim")]
        public async Task<object> AddClaims(IEnumerable<ClaimViewModel> claims)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            foreach (var claimViewModel in claims)
            {
                await _userManager.AddClaimAsync(user, new Claim(claimViewModel.Type, claimViewModel.Value));
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("ListClaims")]
        public async Task<object> GetClaims()
        {
            return Ok(HttpContext.User.Claims);
        }

        [Authorize]
        [HttpGet("SignOut")]
        public async Task<object> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout");
        }

        private async Task<IActionResult> GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("CompletedBasicTraining", ""),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Email,
                Token = tokenString
            });
        }
    }
}
