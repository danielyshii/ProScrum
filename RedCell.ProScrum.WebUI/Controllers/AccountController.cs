using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using RedCell.ProScrum.WebUI.Filters;
using RedCell.ProScrum.WebUI.Models;

namespace RedCell.ProScrum.WebUI.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginModel review = TempData["LoginModel"] as LoginModel;

            ViewBag.ReturnUrl = returnUrl;
            return View(review);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            TempData["LoginModel"] = model;

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                

                return Redirect(returnUrl);
            }
            else if (WebSecurity.UserExists(model.UserName))
            {
            
            }
            else
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Name = model.UserName });
                WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
                return Redirect(returnUrl);
            }

            model.Password = null;

            return RedirectToAction("Login", new { @returnUrl = returnUrl });
        }

    }
}
