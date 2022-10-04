using Business.Abstract;
using Eticaret.WebUI.EmailServices;
using Eticaret.WebUI.Extensions;
using Eticaret.WebUI.Identity;
using Eticaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eticaret.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> _userManeger;
        private SignInManager<User> _signInManeger;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        public AccountController(UserManager<User> userManeger, SignInManager<User> signInManeger,IEmailSender emailSender, ICartService cartService)
        {
            _userManeger = userManeger;
            _signInManeger = signInManeger;
            _emailSender = emailSender;
            _cartService = cartService;
        }
      [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManeger.FindByNameAsync(model.UserName);
            if (user==null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış");
                return View(model);
            }

            if (!await _userManeger.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Email adresinize gelen link ile üyelik onay işlemini gerçekleştiriniz");
                return View(model);
            }

            var result = await _signInManeger.PasswordSignInAsync(user, model.Password,true,false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("", "Girilen Kullanıcı adı veya parola hatalıdır");
            return View(model);
        }

      [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManeger.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //generate token
                var code = await _userManeger.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                //email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız", $"Lütfen email hesabınızı onaylamak için <a href='https://localhost:44344{url}'>linke</a> tıklayınız");
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen bir hata.Lütfen tekrar deneyiniz");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManeger.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                Title ="Oturum Kapatıldı",
                Message="Hesabınız güvenli bir şekilde kapatıldı",
                AlertType="warning"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId ==null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title ="Geçersiz Token",
                    Message = "Geçersiz Token",
                    AlertType = "danger"
                });     
                return View();
            }
            var user = await _userManeger.FindByIdAsync(userId);
            if (user!=null)
            {
                var result = await _userManeger.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //cart objesi
                    _cartService.InitializeCart(user.Id);


                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız Onaylandı",
                        Message = "Hesabınız Onaylandı",
                        AlertType = "success"
                    });
                  
                   
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınız Onaylanmadı",
                Message = "Hesabınız Onaylanmadı",
                AlertType = "warning"
            });  
            return View();
        }
       
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManeger.FindByIdAsync(Email);
            if (user ==null)
            {
                return View();
            }

            var code = await _userManeger.GeneratePasswordResetTokenAsync(user);

            //generate token
           
            var url = Url.Action("ReserPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });
            //email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"parolanızı yenilemek için <a href='https://localhost:44344{url}'>linke</a> tıklayınız");
            return View();
        }

        public IActionResult ResetPassword(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel { Token = token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManeger.FindByEmailAsync(model.Email);
            if (user==null)
            {
                return RedirectToAction("Home", "Index");
            }
            var result = await _userManeger.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
