using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxonomyPrj2.Models;
using TaxonomyPrj2.ViewModels;

namespace TaxonomyPrj2.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private RoleManager<IdentityRole> _roleManager;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        [HttpGet]
        public IActionResult Register() //+
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) //+
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Login, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);

                    // роль по умолчанию
                    List<string> roles = new List<string>();
                    if (_userManager.Users.Count() == 0)
                    {
                        roles.Add("Admin");
                    }
                    else 
                    {
                        roles.Add("CommonUser");
                    }
                   
                    await _userManager.AddToRolesAsync(user, roles);
                    //------------------

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null) //+
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) //+
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                       
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> SaveRole(string login)
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == login);
            var userRoles = await _userManager.GetRolesAsync(elem);
            return Json(new { role = userRoles.First() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() // + 
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users() // + 
        {
            var model = _userManager.Users.ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartialUsers() // +
        {

            var model = new List<UserEditViewModel>();
            var listUser = _userManager.Users.ToList();
            foreach (var item in listUser)
            {
                var user = new UserEditViewModel();
                var userRoles = await _userManager.GetRolesAsync(item);
                user.Role = userRoles.First();
                user.Login = item.UserName;
                user.EMail = item.Email;
                user.Year = item.Year;
                model.Add(user);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> PartialEdit(string login) //+
        {
            var model = new UserEditViewModel();

            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == login);
            var userRoles = await _userManager.GetRolesAsync(elem);
            
            model.Role = userRoles.First();
            model.Login = elem.UserName;
            model.EMail = elem.Email;
            model.Year = elem.Year;
            model.AllRoles = _roleManager.Roles.ToList();

            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PartialEdit(string login, string eMail, int year, string role)  // + 
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == login);
            var newRole = _roleManager.Roles.FirstOrDefault(x => x.Name == role);
            if (elem == null)
            {
                return Json(new { save = false, error = "Ошибка редактирования. Пользователь не найден" });
            }
            else if (newRole == null)
            {
                return Json(new { save = false, error = "Ошибка редактирования. Роль пользователя не найдена" });
            }
            else
            {
               
                elem.Email = eMail;
                elem.Year = year;
                var result = await _userManager.UpdateAsync(elem);
                if (result.Succeeded)
                {
                    //--------------- как проверить результат изменения роли? или хватит проверки выше?
                    /*var roles = await _userManager.GetRolesAsync(elem); // 
                    roles.Clear();*/
                    List<string> roles = new List<string>();
                    roles.Add(role);
                    // получем список ролей пользователя
                    var userRoles = await _userManager.GetRolesAsync(elem);
                    // получаем все роли
                    var allRoles = _roleManager.Roles.ToList();
                    // получаем список ролей, которые были добавлены
                    var addedRoles = roles.Except(userRoles);
                    // получаем роли, которые были удалены
                    var removedRoles = userRoles.Except(roles);

                    await _userManager.AddToRolesAsync(elem, addedRoles);

                    await _userManager.RemoveFromRolesAsync(elem, removedRoles);

                   // var проверка =  await _userManager.GetRolesAsync(elem);
                    //--------------

                    return Json(new { save = true });
                }
                else
                {
                    return Json(new { save = false, error = "Ошибка редактирования. Изменения не сохранились" });
                }

            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PartialDelete(string login)  // + 
        {
            var model = new QuestionUsersViewModel();
            model.Login = login;
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string login)  //+
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == login);
            if (elem == null)
            {
                return Json(new { save = false, error = "Ошибка удаления. Пользователь не найден" });
            }
            var result = await _userManager.DeleteAsync(elem);
            if (result.Succeeded)
            {
                return Json(new { save = true });
            }
            else
            {
                return Json(new { save = false, error = "Ошибка удаления. Результат удаления не был сохранен" });
            }

            
        }

        public async Task<IActionResult> returnRole(string role) // проверка на неизменность роли  +
        {
            var elem = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var userRoles = await _userManager.GetRolesAsync(elem);
            if (userRoles.First() == role)
            {
                return Json(new { result = true });
            }
            else
            {
                await _signInManager.SignOutAsync();
                return Json(new { result = false });
            }

        }
    }
}
