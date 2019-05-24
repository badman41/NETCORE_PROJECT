using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Shared.ValueObject;
using OrderService.Infra.Data.Respository.Dapper;
using OrderService.Model;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSettings;
        private IAccountRepository accountRepository;
        public AccountController(IAccountRepository AccountRepository, IOptions<AppSettings> appSettings)
        {
            accountRepository = AccountRepository;
            _appSettings = appSettings.Value;
        }

        // GET: orderApi/Customer
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchAccountRequest request)
        {
            AccountModel account = new AccountModel()
            {
                UserName = request.Name,
                Role = request.Role,
            };
            var result = accountRepository.Search(account,request.Page,request.PageSize);
            GetAllAccountResponse response = new GetAllAccountResponse
            {
                Data = (IEnumerable<AccountModel>)result.Data,
                Message = "",
                Metadata = new Metadata()
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = result.PageTotal
                },
                Success = true
            };
            return Ok(response);
        }
        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest request)
        {
            try
            {
                LoginResponse response = new LoginResponse() {
                    Data = new DataLoginResponse()
                };
                var userdetails = accountRepository.get(request.Username, request.Password);

                if (userdetails != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, userdetails.UserTypeID.ToString()),
                        new Claim(ClaimTypes.UserData, userdetails.UserName.ToString()),
                        new Claim(ClaimTypes.UserData, request.Password)
                        }),
                        Expires = DateTime.UtcNow.AddDays(300),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    response.Data.Token = tokenHandler.WriteToken(token);

                    response.Data.User = userdetails;
                    response.Message = "";
                    response.Success = true;

                    return Ok(response);
                }
                else
                {
                    response.Data.Token = "";
                    response.Data.User = new Domain.ReadModels.AccountModel();
                    response.Success = false;
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LoginWithToken()
        {
            var accesToken = Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accesToken) as JwtSecurityToken;
            var id = Int32.Parse(jsonToken.Claims.First().Value);

            var userdetails = accountRepository.getByCustomer(id);
            if (id == 0)
            {
                string userName = jsonToken.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
                string passWord = jsonToken.Claims.Last(x => x.Type == ClaimTypes.UserData).Value;
                userdetails = accountRepository.get(userName, passWord);
            }
            try
            {
                LoginResponse response = new LoginResponse()
                {
                    Data = new DataLoginResponse()
                };
                

                if (userdetails != null)
                {
                    response.Data.Token = accesToken;
                    response.Data.User = userdetails;
                    response.Message = "";
                    response.Success = true;
                    return Ok(response);
                }
                else
                {
                    response.Data.Token = "";
                    response.Data.User = new Domain.ReadModels.AccountModel();
                    response.Success = false;
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LoginWareHouse([FromBody] LoginRequestViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var loginstatus = _users.AuthenticateUsers(value.UserName, EncryptionLibrary.EncryptText(value.Password));
                    var loginstatus = true;
                    if (loginstatus)
                    {
                        //var userdetails = _users.GetUserDetailsbyCredentials(value.UserName, EncryptionLibrary.EncryptText(value.Password));
                        var userdetails = new LoginResponse();
                        if (userdetails != null)
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                new Claim(ClaimTypes.Name, "1")
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            value.Token = tokenHandler.WriteToken(token);

                            // remove password before returning
                            value.Password = null;
                            value.Usertype = 2;

                            return Ok(value);
                        }
                        else
                        {
                            value.Password = null;
                            value.Usertype = 0;
                            return Ok(value);
                        }
                    }
                    value.Password = null;
                    value.Usertype = 0;
                    return Ok(value);
                }
                value.Password = null;
                value.Usertype = 0;
                return Ok(value);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNew(AddNewAccountRequest request)
        {
            AccountModel account = new AccountModel()
            {
                DisplayName = request.UserName,
                UserName = request.UserName,
                Password = request.Password,
                Role = 0,
                Status = 0,
                UpdatedAt = DateTime.Now
            };
            var result = accountRepository.Add(account);
            return Ok(result);
        }
        // PUT: api/Customer/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ChangePasswordRequest request)
        {
            var accesToken = Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accesToken) as JwtSecurityToken;
            var id = Int32.Parse(jsonToken.Claims.First().Value);

            AccountModel account = new AccountModel()
            {
                Password = request.NewPassword,
                UserTypeID = id,
            };
            bool result = accountRepository.ChangePassword(account,request.OldPassword);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}