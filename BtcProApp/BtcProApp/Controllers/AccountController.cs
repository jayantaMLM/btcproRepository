using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BtcProApp.Models;
using System.Collections;
using System.Collections.Generic;

namespace BtcProApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private BtcProDB db = new BtcProDB();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UserLogin(string Username, string Password)
        {
            Boolean status = false;
            try {
                var result = await SignInManager.PasswordSignInAsync(Username, Password, false, shouldLockout: false);
                
                switch (result)
                {
                    case SignInStatus.Success:
                        status = true;
                        break;
                    case SignInStatus.Failure:
                        status = false;
                        break;
                }
            }
            catch (Exception e)
            {

            }
           
            return Json(new { Status = status }, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Adduser(string emailId,string username,string password)
        {
            string emailid = username + "@gmail.com";
            var user = new ApplicationUser { UserName = username, Email = emailid };
            string pwd = password;
            var result = UserManager.Create(user, pwd);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                //string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                ////UserManager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //Hashtable ht = new Hashtable();
                //ht.Add("FROM", emailId);
                //ht.Add("TO", emailId);
                //ht.Add("BODY", "Please confirm your new account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //ht.Add("SUBJECT", "You new account in www.btcpro.co");

                //Email email = new Email(ht);
                //email.SendEMail();

                return Json(new { flag = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { flag = "Failure" }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ResetMyPassword(ResetPasswordViewModel model)
        {
            Boolean IsOK = false;
            string usr = User.Identity.Name;
            model.UserName = usr;
            var user = await UserManager.FindByNameAsync(model.UserName);
            var result = await UserManager.ChangePasswordAsync(user.Id,model.Password, model.ConfirmPassword);
            if (result.Succeeded)
            {
                IsOK = true;
            }
            return Json(new { OK = IsOK }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["User"] = null; //it's my session variable
            Session.Clear();
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Indexmain", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Common Code

        [HttpPost]
        public JsonResult LedgerPosting(string Username, string DrCr, int WalletType, double Amount)
        {
            var currentusr = User.Identity.Name;
            var rec0 = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == currentusr.ToUpper().Trim());

            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == Username.ToUpper().Trim());
            Ledger ldgr = new Ledger();
            ldgr.RegistrationId = rec.Id;
            ldgr.WalletId = WalletType;
            ldgr.Date = DateTime.Now;
            if (DrCr == "D") { ldgr.Deposit = Amount; }
            if (DrCr == "W") { ldgr.Withdraw = Amount; }
            ldgr.TransactionTypeId = 1;
            ldgr.TransactionId = 0;
            ldgr.SubLedgerId = 3;
            ldgr.ToFromUser = rec0.Id;

            db.Ledgers.Add(ldgr);
            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LedgerPostingMember(string Username, int WalletType, double Amount)
        {
            var currentusr = User.Identity.Name;
            if (currentusr != null || currentusr != "")
            {
                var rec0 = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == currentusr.ToUpper().Trim());
                var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == Username.ToUpper().Trim());

                Ledger ldgr = new Ledger();
                ldgr.RegistrationId = rec0.Id;
                ldgr.WalletId = WalletType;
                ldgr.Date = DateTime.Now;
                ldgr.Withdraw = Amount;
                ldgr.Deposit = 0;
                ldgr.TransactionTypeId = 6;
                ldgr.TransactionId = 0;
                ldgr.SubLedgerId = 1;
                ldgr.ToFromUser = rec.Id;

                db.Ledgers.Add(ldgr);
                db.SaveChanges();

                Ledger ldgrT = new Ledger();
                ldgrT.RegistrationId = rec.Id;
                ldgrT.WalletId = WalletType;
                ldgrT.Date = DateTime.Now;
                ldgrT.Withdraw = 0;
                ldgrT.Deposit = Amount;
                ldgrT.TransactionTypeId = 7;
                ldgrT.TransactionId = 0;
                ldgrT.SubLedgerId = 1;
                ldgrT.ToFromUser = rec0.Id;

                db.Ledgers.Add(ldgrT);
                db.SaveChanges();
            }


            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeam(int? RegistrationId)   //Get Tree of a Id
        {
            long regId = 0;
            if (RegistrationId == null)
            {
                string username = User.Identity.Name;
                var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
                regId = rec.Id;
            }
            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.UplineRegistrationId == regId);
            members.AddRange(m);
            long lastId = 0;

            for (int i = 0; i < members.Count(); i++)
            {
                lastId = members[i].RegistrationId;
                var n = db.Members.Where(mm => mm.UplineRegistrationId == lastId);
                members.AddRange(n);
            }

            var MemList = members.Select(n => new { n.RegistrationId, n.Username, n.Defaultpackagecode, n.Country, Doj = n.Doj.ToShortDateString(), n.BinaryPosition }).OrderByDescending(n => n.Doj);

            return Json(new { Members = MemList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeamByNameFirstLevel(string Member)   //Get Tree of a name
        {
            var res = db.Members.Where(x => x.Username == Member).Single();
            long MemberId = res.Id;

            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.UplineRegistrationId == MemberId);
            members.AddRange(m);

            var MemList = members.Select(n => new { n.Id, n.Username, n.ReferrerRegistrationId, n.Referrerusername, n.Level });

            return Json(new { Members = MemList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeamByIdNextLevel(long Id)   //Get Tree of a name
        {
            long MemberId = Id;

            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.UplineRegistrationId == MemberId);
            members.AddRange(m);

            var MemList = members.Select(n => new { n.Id, n.Username, n.ReferrerRegistrationId, n.Referrerusername, n.Level });

            return Json(new { Members = MemList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeamByName(string Member)   //Get Tree of a name
        {
            if (Member == "")
            {
                Member = User.Identity.Name;
            }

            var res = db.Members.Where(x => x.Username == Member).Single();
            long MemberId = res.RegistrationId;

            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.UplineRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

            for (int i = 0; i < m.Count(); i++)
            {
                m[i].Kycdocuments = PackageImage((int)m[i].Defaultpackagecode);
            }

            if (m.Count() == 1)
            {
                if (m[0].BinaryPosition == "L")
                {
                    Member mmmm = new Member();
                    mmmm.Username = "Vacant";
                    mmmm.BinaryPosition = "R";
                    mmmm.RegistrationId = m[0].RegistrationId + 10000;
                    mmmm.UplineRegistrationId = m[0].UplineRegistrationId;
                    mmmm.Uplineusername = m[0].Uplineusername;
                    mmmm.Level = m[0].Level;
                    mmmm.Kycdocuments = PackageImage(0);

                    members.AddRange(m);
                    members.Add(mmmm);
                }
                if (m[0].BinaryPosition == "R")
                {
                    Member mmmm = new Member();
                    mmmm.Username = "Vacant";
                    mmmm.BinaryPosition = "L";
                    mmmm.RegistrationId = m[0].RegistrationId + 10000;
                    mmmm.UplineRegistrationId = m[0].UplineRegistrationId;
                    mmmm.Uplineusername = m[0].Uplineusername;
                    mmmm.Level = m[0].Level;
                    mmmm.Kycdocuments = PackageImage(0);
                    members.Add(mmmm);
                    members.AddRange(m);
                }

            }

            long lastId = 0;

            for (int i = 0; i < members.Count(); i++)
            {
                lastId = members[i].RegistrationId;
                var n = db.Members.Where(mm => mm.UplineRegistrationId == lastId).ToList();

                for (int c = 0; c < n.Count(); c++)
                {
                    n[c].Kycdocuments = PackageImage((int)n[c].Defaultpackagecode);
                }

                if (n.Count() == 1)
                {
                    if (n[0].BinaryPosition == "L")
                    {
                        Member mmmm = new Member();
                        mmmm.Username = "Vacant";
                        mmmm.BinaryPosition = "R";
                        mmmm.RegistrationId = n[0].RegistrationId + 10000;
                        mmmm.UplineRegistrationId = n[0].UplineRegistrationId;
                        mmmm.Uplineusername = n[0].Uplineusername;
                        mmmm.Level = n[0].Level;
                        mmmm.Kycdocuments = PackageImage(0);

                        members.AddRange(n);
                        members.Add(mmmm);
                    }
                    if (m[0].BinaryPosition == "R")
                    {
                        Member mmmm = new Member();
                        mmmm.Username = "Vacant";
                        mmmm.BinaryPosition = "L";
                        mmmm.RegistrationId = n[0].RegistrationId + 10000;
                        mmmm.UplineRegistrationId = n[0].UplineRegistrationId;
                        mmmm.Uplineusername = n[0].Uplineusername;
                        mmmm.Level = n[0].Level;
                        mmmm.Kycdocuments = PackageImage(0);
                        members.Add(mmmm);
                        members.AddRange(n);
                    }

                }
            }

            var MemList = members.Select(n => new { n.RegistrationId, n.Username, n.UplineRegistrationId, n.Uplineusername, n.Level, n.Kycdocuments });

            return Json(new { Members = MemList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBinaryTeamByTree(string Member, int levels, int? Id)   //Get Tree of a name
        {
            int levelsToSshow = levels;


            //Temporary valiables    
            string name = "";           //username
            int nameLength = 0;         //length of username
            int stringStartpos = 0;     //username starting position within html string
            int stringEndpos = 0;       //username ending position after </a> following username
            string tmphtml = "";        //intermediate stage during html construction of a username
            string firstPart = "";      //before html insert operation, store the <prefix> part
            string lastPart = "";       //before html insert operation store the <suffix> part
            string defaultPackage = "";

            if (Member != "")
            {
                var mm = db.Members.SingleOrDefault(i => i.Username == Member);
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }

            if (Member == "" && Id == null)
            {
                Member = User.Identity.Name;
                var mm = db.Members.SingleOrDefault(i => i.Username == Member);
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }
            if (Member == "" && Id != null)
            {
                var mm = db.Members.SingleOrDefault(i => i.Id == Id);
                Member = mm.Username;
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }

            string MemTree = "<ul><li><a uib-popover-html='<b>HTML</b>, <i>inline</i>'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + Member + "</a></li></ul>"; //the actual html string

            var res = db.Members.Where(x => x.Username == Member).Single();
            long MemberId = res.RegistrationId;
            int? currentMemberLevel = res.Level;
            int maximumlevel = (int)currentMemberLevel + levelsToSshow - 1;

            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.UplineRegistrationId == MemberId);
            members.AddRange(m);

            if (m.Count() > 0)
            {
                name = Member.Trim();
                nameLength = name.Length;
                stringStartpos = MemTree.IndexOf(name, 0);
                stringEndpos = stringStartpos + name.Length + 4;
                tmphtml = "<ul>";
                foreach (Member mem in m)
                {
                    if (mem.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                    if (mem.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                    if (mem.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                    if (mem.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                    tmphtml = tmphtml + "<li><a onmouseover='myFunction()'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + mem.Username + "</a></li>";
                }
                tmphtml = tmphtml + "</ul>";
                firstPart = MemTree.Substring(0, stringEndpos);
                lastPart = MemTree.Substring(stringEndpos);
                MemTree = firstPart + tmphtml + lastPart;
            }
            long lastId = 0;
            string lastIdUsername = "";

            for (int i = 0; i < members.Count(); i++)
            {
                if (members[i].Level < maximumlevel)
                {
                    lastId = members[i].RegistrationId;
                    lastIdUsername = members[i].Username;
                    var n = db.Members.Where(mm => mm.UplineRegistrationId == lastId);
                    members.AddRange(n);
                    if (n.Count() > 0)
                    {
                        name = lastIdUsername.Trim();
                        nameLength = name.Length;
                        stringStartpos = MemTree.IndexOf(name, 0);
                        stringEndpos = stringStartpos + name.Length + 4;
                        tmphtml = "<ul>";
                        foreach (Member mem in n)
                        {
                            if (mem.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                            if (mem.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                            if (mem.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                            if (mem.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                            tmphtml = tmphtml + "<li><a ng-click='clicksearch(" + mem.Id + ")'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + mem.Username + "</a></li>";
                        }
                        tmphtml = tmphtml + "</ul>";
                        firstPart = MemTree.Substring(0, stringEndpos);
                        lastPart = MemTree.Substring(stringEndpos);
                        MemTree = firstPart + tmphtml + lastPart;
                    }
                }
            }

            return Json(new { Members = MemTree }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnilevelTeamByTree(string Member, int levels, int? Id)   //Get Tree of a name
        {
            int levelsToSshow = levels;


            //Temporary valiables    
            string name = "";           //username
            int nameLength = 0;         //length of username
            int stringStartpos = 0;     //username starting position within html string
            int stringEndpos = 0;       //username ending position after </a> following username
            string tmphtml = "";        //intermediate stage during html construction of a username
            string firstPart = "";      //before html insert operation, store the <prefix> part
            string lastPart = "";       //before html insert operation store the <suffix> part
            string defaultPackage = "";

            if (Member != "")
            {
                var mm = db.Members.SingleOrDefault(i => i.Username == Member);
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }

            if (Member == "" && Id == null)
            {
                Member = User.Identity.Name;
                var mm = db.Members.SingleOrDefault(i => i.Username == Member);
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }
            if (Member == "" && Id != null)
            {
                var mm = db.Members.SingleOrDefault(i => i.Id == Id);
                Member = mm.Username;
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
            }

            string MemTree = "<ul><li><a uib-popover-html='<b>HTML</b>, <i>inline</i>'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + Member + "</a></li></ul>"; //the actual html string

            var res = db.Members.Where(x => x.Username == Member).Single();
            long MemberId = res.RegistrationId;
            int? currentMemberLevel = res.Level;
            int maximumlevel = (int)currentMemberLevel + levelsToSshow - 1;

            List<Member> members = new List<Member>();

            var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId);
            members.AddRange(m);

            if (m.Count() > 0)
            {
                name = Member.Trim();
                nameLength = name.Length;
                stringStartpos = MemTree.IndexOf(name, 0);
                stringEndpos = stringStartpos + name.Length + 4;
                tmphtml = "<ul>";
                foreach (Member mem in m)
                {
                    if (mem.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                    if (mem.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                    if (mem.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                    if (mem.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                    tmphtml = tmphtml + "<li><a onmouseover='myFunction()'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + mem.Username + "</a></li>";
                }
                tmphtml = tmphtml + "</ul>";
                firstPart = MemTree.Substring(0, stringEndpos);
                lastPart = MemTree.Substring(stringEndpos);
                MemTree = firstPart + tmphtml + lastPart;
            }
            long lastId = 0;
            string lastIdUsername = "";

            for (int i = 0; i < members.Count(); i++)
            {
                if (members[i].Level < maximumlevel)
                {
                    lastId = members[i].RegistrationId;
                    lastIdUsername = members[i].Username;
                    var n = db.Members.Where(mm => mm.ReferrerRegistrationId == lastId);
                    members.AddRange(n);
                    if (n.Count() > 0)
                    {
                        name = lastIdUsername.Trim();
                        nameLength = name.Length;
                        stringStartpos = MemTree.IndexOf(name, 0);
                        stringEndpos = stringStartpos + name.Length + 4;
                        tmphtml = "<ul>";
                        foreach (Member mem in n)
                        {
                            if (mem.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                            if (mem.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                            if (mem.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                            if (mem.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                            tmphtml = tmphtml + "<li><a ng-click='clicksearch(" + mem.Id + ")'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + mem.Username + "</a></li>";
                        }
                        tmphtml = tmphtml + "</ul>";
                        firstPart = MemTree.Substring(0, stringEndpos);
                        lastPart = MemTree.Substring(stringEndpos);
                        MemTree = firstPart + tmphtml + lastPart;
                    }
                }
            }

            return Json(new { Members = MemTree }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult IsUserNameExist(string UserName)
        {
            bool isExist = true;
            long Id = 0;
            Member mem = db.Members.SingleOrDefault(m => m.Username.ToUpper().Trim() == UserName.ToUpper().Trim());
            if (mem == null)
            {
                isExist = false;
            }
            else
            {
                Register reg = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
                Id = reg.Id;
            }
            return Json(new { Found = isExist, ReferrerId = Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult IsUserNameExist1(string UserName)
        {
            bool isExist = true;
            var mem = db.Registrations.Where(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
            if (mem == null || mem.Count() == 0)
            {
                isExist = false;
            }
            return Json(new { Found = isExist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult IsEmailPresent(string EmailId)
        {
            bool isExist = true;
            var mem = db.Registrations.Where(m => m.EmailId.ToLower().Trim() == EmailId.ToLower().Trim());
            if (mem == null || mem.Count() == 0)
            {
                isExist = false;
            }
            return Json(new { Found = isExist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MyWalletBalance(int WalletId)
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
            double sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id).Sum(i => i.Deposit);
            double sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id).Sum(i => i.Withdraw);
            double balance = sumdeposit - sumwithdrawal;
            return Json(new { Balance = balance }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> MyNewPurchase(int packageId, double investmentAmt)
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            //1...add a withdrawal entry in ledger
            //2...add an entry in purchase
            //3...update registration and ledger
            //4...add to member register
            //5...update left right count of uplines
            //6...weekly fixed income calculation
            //7...weekly binary income calculation
            //8...weekly sponsor income calculation
            //9...weekly generation income calculation
            //10..send confirmation email


            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            //1...
            Ledger ldgr = new Ledger();
            ldgr.RegistrationId = rec.Id;
            ldgr.WalletId = 2;
            ldgr.Date = DateTime.Now;
            ldgr.Deposit = 0;
            ldgr.Withdraw = investmentAmt;
            if (!rec.Joined) { ldgr.TransactionTypeId = 4; } else { ldgr.TransactionTypeId = 9; }
            ldgr.TransactionId = 0;
            ldgr.SubLedgerId = 1;
            ldgr.BatchNo = GuidString;

            db.Ledgers.Add(ldgr);
            //await db.SaveChangesAsync();
            db.SaveChanges();

            //2...
            Purchase pur = new Purchase();
            pur.Purchasedate = DateTime.Now;
            if (!rec.Joined) { pur.Packageid = packageId; } else { pur.Packageid = 5; packageId = 5; } //0==repurchase
            pur.RegistrationId = rec.Id;
            pur.Amount = investmentAmt;
            pur.Payreferenceno = GuidString;
            pur.Isapproved = true;

            db.Purchases.Add(pur);
            //await db.SaveChangesAsync();
            db.SaveChanges();

            //3...
            ldgr.TransactionId = pur.Id;
            Boolean isJoined = rec.Joined; //before update store last state of isJoined
            rec.Joined = true;
            //await db.SaveChangesAsync();
            db.SaveChanges();

            //4...
            long uplineId = 0;
            if (!isJoined)
            {
                try
                {
                    Member mem = new Member();
                    mem.Doj = rec.CreatedDate;
                    mem.Activationdate = DateTime.Now;
                    mem.Firstname = rec.FullName;
                    mem.Username = rec.UserName;
                    mem.Emailid = rec.EmailId;
                    mem.Isactive = true;
                    mem.RegistrationId = rec.Id;
                    mem.ReferrerRegistrationId = rec.ReferrerId;
                    mem.Referrerusername = rec.ReferrerName;
                    mem.Uplineusername = GetExtremeLeftRight(rec.ReferrerName, rec.BinaryPosition);
                    var temprec = db.Registrations.SingleOrDefault(b => b.UserName.ToUpper().Trim() == mem.Uplineusername.ToUpper().Trim());
                    mem.UplineRegistrationId = temprec.Id;
                    uplineId = (long)mem.UplineRegistrationId;
                    mem.Level = GetnewLevel(uplineId);
                    mem.BinaryPosition = rec.BinaryPosition;
                    mem.Defaultpackagecode = packageId;
                    mem.Totalmembers = 0;
                    mem.Rightmembers = 0;
                    mem.Leftmembers = 0;
                    mem.Country = "BD";
                    db.Members.Add(mem);
                    //await db.SaveChangesAsync();
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }

                //5...
                string searchname = rec.UserName;
                string searchpos = rec.BinaryPosition;
                var res0 = db.Members.Where(x1 => x1.Username == searchname).Single();
                long MemberId = res0.RegistrationId;
                Boolean keepgoing = true;

                do
                {
                    var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId);
                    if (res1 != null)
                    {
                        if (searchpos == "L") { res1.Leftmembers = (res1.Leftmembers == null) ? 1 : res1.Leftmembers + 1; }
                        if (searchpos == "R") { res1.Rightmembers = (res1.Rightmembers == null) ? 1 : res1.Rightmembers + 1; }
                        res1.Totalmembers = (res1.Totalmembers == null) ? 1 : res1.Totalmembers + 1;
                        //await db.SaveChangesAsync();
                        db.SaveChanges();
                        uplineId = (long)res1.UplineRegistrationId;
                    }
                    else
                    {
                        keepgoing = false;
                    }

                } while (keepgoing);
            }

            //6...

            //if repurchase, then % will be default package %
            double pc = 0;
            try
            {
                var pkg = db.Packages.SingleOrDefault(p => p.Id == packageId);
                if (pkg.Id == 5)
                {
                    var mmm = db.Members.Where(x => x.Username == rec.UserName).Single();
                    var pkgg = db.Packages.SingleOrDefault(p => p.Id == mmm.Defaultpackagecode);
                    pc = pkgg.Gauranteedreturn_percent;
                }
                else
                {
                    pc = pkg.Gauranteedreturn_percent;
                }
            }
            catch (Exception ex)
            {

            }

            await FixedIncomeCalculation(rec.Id, pc, investmentAmt, packageId, GuidString);

            //7...
            string searchname1 = rec.UserName;
            string searchpos1 = rec.BinaryPosition;
            var res = db.Members.Where(x2 => x2.Username == searchname1).Single();
            long MemberId1 = res.RegistrationId;
            Boolean keepgoing1 = true;
            long uplineId1 = (long)res.UplineRegistrationId;
            do
            {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId1);
                if (res1 != null)
                {
                    await BinaryIncomeCalculation(uplineId1, res1.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, GuidString);
                    uplineId1 = (long)res1.UplineRegistrationId;
                }
                else
                {
                    keepgoing1 = false;
                }

            } while (keepgoing1);

            //8...
            var res2 = db.Members.SingleOrDefault(m => m.RegistrationId == res.ReferrerRegistrationId);
            await SponsorIncomeCalculation(res.ReferrerRegistrationId, res2.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, GuidString);

            //9..
            string searchname2 = rec.UserName;
            string searchpos2 = rec.BinaryPosition;
            var res3 = db.Members.Where(x3 => x3.Username == searchname2).Single();
            long MemberId2 = res.RegistrationId;
            Boolean keepgoing2 = true;
            long uplineId2 = (long)res3.ReferrerRegistrationId;
            int level = 1;
            do
            {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId2);
                if (res1 != null)
                {
                    await GenerationIncomeCalculation(uplineId2, res1.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, (int)res1.Defaultpackagecode, level, GuidString);
                    uplineId2 = (long)res2.ReferrerRegistrationId;
                    if (level == 4) { keepgoing2 = false; }
                    level = level + 1;
                }
                else
                {
                    keepgoing2 = false;
                }

            } while (keepgoing2);

            //10..
            SendCandidateEmail(rec.EmailId);

            return Json(new { Success = "TRUE" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MyAddress()
        {
            string username = User.Identity.Name;
            var rec = db.Members.Select(r => new { Username = r.Username, Fullname = r.Firstname, AddressLine1 = r.Addressline1, AddressLine2 = r.Addressline2, City = r.City, State = r.State, Country = r.Country, EmailId = r.Emailid }).SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            return Json(new { Address = rec }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MyPurchases()
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            var purchases = (from p in db.Purchases
                             from s in db.Packages
                             where p.Packageid == s.Id && p.RegistrationId == rec.Id && p.Packageid > 0
                             orderby p.Purchasedate
                             select new
                             {
                                 PurchaseDate = p.Purchasedate.Day + "-" + p.Purchasedate.Month + "-" + p.Purchasedate.Year,
                                 ReferenceNo = p.Payreferenceno,
                                 Status = "Paid",
                                 Package = s.Name + " package",
                                 Quantity = 1,
                                 Amount = p.Amount,
                                 Paymode = "",
                                 MinPay = s.Minamount,
                                 MaxPay = s.Maxamount
                             }).ToList();
            return Json(new { Purchases = purchases }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyRePurchases()
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            var purchases = (from p in db.Purchases
                             from s in db.Packages
                             where p.Packageid == s.Id && p.RegistrationId == rec.Id && p.Packageid == 5
                             orderby p.Purchasedate
                             select new
                             {
                                 PurchaseDate = p.Purchasedate.Day + "-" + p.Purchasedate.Month + "-" + p.Purchasedate.Year,
                                 ReferenceNo = p.Payreferenceno,
                                 Status = "Paid",
                                 Package = s.Name + " package",
                                 Quantity = 1,
                                 Amount = p.Amount,
                                 Paymode = "",
                                 MinPay = s.Minamount,
                                 MaxPay = s.Maxamount
                             }).ToList();
            return Json(new { Purchases = purchases }, JsonRequestBehavior.AllowGet);
        }

        public string GetExtremeLeftRight(string referrerName, string position)
        {
            string searchname = referrerName;
            string searchpos = position;
            var res = db.Members.Where(x => x.Username.ToUpper().Trim() == referrerName.ToUpper().Trim()).Single();
            long MemberId = res.RegistrationId;
            Boolean keepgoing = true;
            do
            {
                var res1 = db.Members.SingleOrDefault(m => m.UplineRegistrationId == (long?)MemberId && m.BinaryPosition == position);
                if (res1 != null)
                {
                    MemberId = res1.RegistrationId;
                    searchname = res1.Username;
                }
                else
                {
                    keepgoing = false;
                }

            } while (keepgoing);

            return searchname;
        }

        public int GetnewLevel(long uplineId)
        {
            var res = db.Members.Where(x => x.RegistrationId == uplineId).Single();
            int MemberLevel = (int)res.Level + 1;

            return MemberLevel;
        }

        public async Task<bool> FixedIncomeCalculation(long RegistrationId, double Pc, double Investment, int PackageId, string GuidString)
        {
            DateTime duedt = DateTime.Now.AddDays(1);
            List<WeeklyIncome> weeklys = new List<WeeklyIncome>();
            for (int i = 1; i <= 52; i++)
            {
                WeeklyIncome wkin = new WeeklyIncome();
                wkin.RegistrationId = RegistrationId;
                wkin.WeekNo = i;
                wkin.Days = 7;
                wkin.Pc = Pc;
                wkin.Income = (Pc * Investment) / 100;
                wkin.PackageId = PackageId;
                WeekModel wkModel = GetCurrentWeek(DateTime.Now.AddDays(1), duedt);
                wkin.WeekStartDate = wkModel.WeekStartDate;
                wkin.WeekEndDate = wkModel.WeekEndDate;
                wkin.DueDate = duedt.AddDays(7);
                duedt = wkin.DueDate;
                wkin.CashWallet = 0;
                wkin.ReserveWallet = 0;
                wkin.FixedIncomeWallet = (Pc * Investment) / 100;
                wkin.FrozenWallet = 0;
                wkin.BatchNo = GuidString;

                weeklys.Add(wkin);
            }
            db.WeeklyIncomes.AddRange(weeklys);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BinaryIncomeCalculation(long RegistrationId, DateTime JoiningDate, double Amount, string Position, bool newJoining, long PurchaseRegistrationId, int packageId, string GuidString)
        {
            BinaryIncome newBusiness = new BinaryIncome();
            newBusiness.RegistrationId = RegistrationId;
            WeekModel wk = new WeekModel();
            wk = GetCurrentWeek(JoiningDate, DateTime.Now);
            newBusiness.WeekNo = wk.WeekNo;
            WeekModel wkc = new WeekModel();
            wkc = GetCurrentWeekBoundary(DateTime.Now);
            newBusiness.WeekStartDate = wkc.WeekStartDate;
            newBusiness.WeekEndDate = wkc.WeekEndDate;
            if (newJoining)
            {
                if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
            }
            else
            {
                newBusiness.LeftNewJoining = 0;
                newBusiness.RightNewJoining = 0;
            }
            if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
            if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

            if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
            if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

            newBusiness.TransactionDate = DateTime.Now;

            newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

            newBusiness.PackageId = packageId;

            double BinaryIncome = ((newBusiness.LeftNewBusinessAmount + newBusiness.LeftNewBusinessAmount) * 10) / 100;

            newBusiness.CashWallet = (BinaryIncome * 70) / 100;
            newBusiness.ReserveWallet = BinaryIncome - newBusiness.CashWallet;
            newBusiness.FixedIncomeWallet = 0;
            newBusiness.FrozenWallet = 0;

            newBusiness.BatchNo = GuidString;

            db.BinaryIncomes.Add(newBusiness);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SponsorIncomeCalculation(long RegistrationId, DateTime JoiningDate, double Amount, string Position, bool newJoining, long PurchaseRegistrationId, int packageId, string GuidString)
        {
            SponsorIncome newBusiness = new SponsorIncome();
            newBusiness.RegistrationId = RegistrationId;
            WeekModel wk = new WeekModel();
            wk = GetCurrentWeek(JoiningDate, DateTime.Now);
            newBusiness.WeekNo = wk.WeekNo;
            WeekModel wkc = new WeekModel();
            wkc = GetCurrentWeekBoundary(DateTime.Now);
            newBusiness.WeekStartDate = wkc.WeekStartDate;
            newBusiness.WeekEndDate = wkc.WeekEndDate;
            if (newJoining)
            {
                if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
            }
            else
            {
                newBusiness.LeftNewJoining = 0;
                newBusiness.RightNewJoining = 0;
            }
            if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
            if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

            if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
            if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

            newBusiness.TransactionDate = DateTime.Now;

            newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

            newBusiness.PackageId = packageId;

            newBusiness.IncomeAmount = (Amount * 10) / 100;

            newBusiness.CashWallet = (newBusiness.IncomeAmount * 70) / 100;
            newBusiness.ReserveWallet = newBusiness.IncomeAmount - newBusiness.CashWallet;
            newBusiness.FixedIncomeWallet = 0;
            newBusiness.FrozenWallet = 0;

            newBusiness.BatchNo = GuidString;

            db.SponsorIncomes.Add(newBusiness);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GenerationIncomeCalculation(long RegistrationId, DateTime JoiningDate, double Amount, string Position, bool newJoining, long PurchaseRegistrationId, int packageId, int DefaultSchemeId, int Level, string GuidString)
        {
            if (Level == 1)
            {
                if (DefaultSchemeId == 1 || DefaultSchemeId == 2 || DefaultSchemeId == 3 || DefaultSchemeId == 4)
                {
                    int interest = 5;
                    GenerationIncome newBusiness = new GenerationIncome();
                    newBusiness.RegistrationId = RegistrationId;
                    WeekModel wk = new WeekModel();
                    wk = GetCurrentWeek(JoiningDate, DateTime.Now);
                    newBusiness.WeekNo = wk.WeekNo;
                    WeekModel wkc = new WeekModel();
                    wkc = GetCurrentWeekBoundary(DateTime.Now);
                    newBusiness.WeekStartDate = wkc.WeekStartDate;
                    newBusiness.WeekEndDate = wkc.WeekEndDate;
                    if (newJoining)
                    {
                        if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                        if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
                    }
                    else
                    {
                        newBusiness.LeftNewJoining = 0;
                        newBusiness.RightNewJoining = 0;
                    }
                    if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

                    if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

                    newBusiness.TransactionDate = DateTime.Now;

                    newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

                    newBusiness.PackageId = packageId;

                    newBusiness.IncomeAmount = (Amount * interest) / 100;

                    newBusiness.CashWallet = (newBusiness.IncomeAmount * 70) / 100;
                    newBusiness.ReserveWallet = newBusiness.IncomeAmount - newBusiness.CashWallet;
                    newBusiness.FixedIncomeWallet = 0;
                    newBusiness.FrozenWallet = 0;

                    newBusiness.BatchNo = GuidString;

                    db.GenerationIncomes.Add(newBusiness);
                    await db.SaveChangesAsync();
                }
            }
            if (Level == 2)
            {
                if (DefaultSchemeId == 2 || DefaultSchemeId == 3 || DefaultSchemeId == 4)
                {
                    int interest = 7;
                    GenerationIncome newBusiness = new GenerationIncome();
                    newBusiness.RegistrationId = RegistrationId;
                    WeekModel wk = new WeekModel();
                    wk = GetCurrentWeek(JoiningDate, DateTime.Now);
                    newBusiness.WeekNo = wk.WeekNo;
                    newBusiness.WeekStartDate = wk.WeekStartDate;
                    newBusiness.WeekEndDate = wk.WeekEndDate;
                    if (newJoining)
                    {
                        if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                        if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
                    }
                    else
                    {
                        newBusiness.LeftNewJoining = 0;
                        newBusiness.RightNewJoining = 0;
                    }
                    if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

                    if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

                    newBusiness.TransactionDate = DateTime.Now;

                    newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

                    newBusiness.PackageId = packageId;

                    newBusiness.IncomeAmount = (Amount * interest) / 100;

                    db.GenerationIncomes.Add(newBusiness);
                    await db.SaveChangesAsync();
                }
            }
            if (Level == 3)
            {
                if (DefaultSchemeId == 3 || DefaultSchemeId == 4)
                {
                    int interest = 9;
                    GenerationIncome newBusiness = new GenerationIncome();
                    newBusiness.RegistrationId = RegistrationId;
                    WeekModel wk = new WeekModel();
                    wk = GetCurrentWeek(JoiningDate, DateTime.Now);
                    newBusiness.WeekNo = wk.WeekNo;
                    newBusiness.WeekStartDate = wk.WeekStartDate;
                    newBusiness.WeekEndDate = wk.WeekEndDate;
                    if (newJoining)
                    {
                        if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                        if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
                    }
                    else
                    {
                        newBusiness.LeftNewJoining = 0;
                        newBusiness.RightNewJoining = 0;
                    }
                    if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

                    if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

                    newBusiness.TransactionDate = DateTime.Now;

                    newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

                    newBusiness.PackageId = packageId;

                    newBusiness.IncomeAmount = (Amount * interest) / 100;

                    db.GenerationIncomes.Add(newBusiness);
                    await db.SaveChangesAsync();
                }
            }
            if (Level == 4)
            {
                if (DefaultSchemeId == 4)
                {
                    int interest = 10;
                    GenerationIncome newBusiness = new GenerationIncome();
                    newBusiness.RegistrationId = RegistrationId;
                    WeekModel wk = new WeekModel();
                    wk = GetCurrentWeek(JoiningDate, DateTime.Now);
                    newBusiness.WeekNo = wk.WeekNo;
                    newBusiness.WeekStartDate = wk.WeekStartDate;
                    newBusiness.WeekEndDate = wk.WeekEndDate;
                    if (newJoining)
                    {
                        if (Position == "L") { newBusiness.LeftNewJoining = 1; newBusiness.RightNewJoining = 0; }
                        if (Position == "R") { newBusiness.LeftNewJoining = 0; newBusiness.RightNewJoining = 1; }
                    }
                    else
                    {
                        newBusiness.LeftNewJoining = 0;
                        newBusiness.RightNewJoining = 0;
                    }
                    if (Position == "L") { newBusiness.LeftNewBusinessCount = 1; newBusiness.RightNewBusinessCount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessCount = 0; newBusiness.RightNewBusinessCount = 1; }

                    if (Position == "L") { newBusiness.LeftNewBusinessAmount = Amount; newBusiness.RightNewBusinessAmount = 0; }
                    if (Position == "R") { newBusiness.LeftNewBusinessAmount = 0; newBusiness.RightNewBusinessAmount = Amount; }

                    newBusiness.TransactionDate = DateTime.Now;

                    newBusiness.PurchaserRegistrationId = PurchaseRegistrationId;

                    newBusiness.PackageId = packageId;

                    newBusiness.IncomeAmount = (Amount * interest) / 100;

                    db.GenerationIncomes.Add(newBusiness);
                    await db.SaveChangesAsync();
                }
            }

            return true;
        }

        public WeekModel GetCurrentWeek(DateTime JoiningDate, DateTime Today)
        {
            int weekno = 0;
            DateTime jd = JoiningDate.AddDays(-1);
            Boolean keepdoing = true;

            do
            {
                if (Today >= jd && Today <= jd.AddDays(7))
                {
                    keepdoing = false;
                }
                jd = jd.AddDays(7);
                weekno = weekno + 1;
            } while (keepdoing);

            WeekModel wkmodel = new WeekModel();
            wkmodel.WeekNo = weekno;
            wkmodel.WeekStartDate = jd.AddDays(-6);
            wkmodel.WeekEndDate = jd;
            return wkmodel;
        }

        public WeekModel GetBoundaryWeek(DateTime JoiningDate, int WeekNo)
        {
            int weekno = 0;
            DateTime jd = JoiningDate.AddDays(-1);

            do
            {
                jd = jd.AddDays(7);
                weekno = weekno + 1;
            } while (weekno < WeekNo);

            WeekModel wkmodel = new WeekModel();
            wkmodel.WeekNo = weekno;
            wkmodel.WeekStartDate = jd.AddDays(-6);
            wkmodel.WeekEndDate = jd;

            return wkmodel;
        }

        public JsonResult MemberTransferHistory()
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
            var transfers = (from t in db.Ledgers
                             from r in db.Registrations
                             where t.ToFromUser == r.Id && t.RegistrationId == rec.Id && t.WalletId == 2
                             select new MemberTransferVM
                             {
                                 Id = t.Id,
                                 DateD = t.Date,
                                 Deposit = t.Deposit,
                                 Withdraw = t.Withdraw,
                                 Transfer = r.UserName,
                                 Balance = 0,
                             }).ToList();

            for (int i = 0; i < transfers.Count(); i++)
            {
                transfers[i].Date = transfers[i].DateD.ToLongDateString();
            }

            return Json(new { Transfers = transfers }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFixedIncomeIllustration(string Guid)
        {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            var FIarray = (from wk in db.WeeklyIncomes
                           from pkg in db.Packages
                           where wk.PackageId == pkg.Id &&
                           wk.RegistrationId == rec.Id &&
                           wk.BatchNo == Guid
                           orderby wk.WeekNo
                           select new FixedIncomeLedgerVM
                           {
                               WeekNo = wk.WeekNo,
                               WeekStartDate = wk.WeekStartDate,
                               WeekEndDate = wk.WeekEndDate,
                               DueDate = wk.DueDate,
                               Amount = wk.Income,
                               Package = pkg.Name
                           }).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
            {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                FIarray[i].sDueDate = FIarray[i].DueDate.ToLongDateString();
            }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyCurrentFixedIncome()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeek(rec.Doj, DateTime.Now);
            var FIarray = (from wk in db.WeeklyIncomes
                           from pkg in db.Packages
                           where wk.PackageId == pkg.Id && wk.RegistrationId == rec.RegistrationId && wk.WeekNo == weekly.WeekNo
                           orderby wk.WeekNo
                           select new FixedIncomeLedgerVM
                           {
                               WeekNo = wk.WeekNo,
                               WeekStartDate = wk.WeekStartDate,
                               WeekEndDate = wk.WeekEndDate,
                               DueDate = wk.DueDate,
                               Amount = wk.Income,
                               Package = pkg.Name
                           }).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
            {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                FIarray[i].sDueDate = FIarray[i].DueDate.ToLongDateString();
            }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyCurrentBinaryIncome()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);

            var BIncome = (from bi in db.BinaryIncomes
                           from pkg in db.Packages
                           from mem in db.Members
                           where bi.PackageId == pkg.Id && bi.RegistrationId == rec.RegistrationId &&
                           bi.WeekStartDate >= weekly.WeekStartDate && bi.WeekEndDate <= weekly.WeekEndDate
                           && mem.RegistrationId == bi.PurchaserRegistrationId
                           orderby bi.Id
                           select new BinaryIncomeLedgerVM
                           {
                               Id = bi.Id,
                               WeekNo = bi.WeekNo,
                               WeekStartDate = bi.WeekStartDate,
                               WeekEndDate = bi.WeekEndDate,
                               Date = bi.TransactionDate,
                               Purchaser = mem.Username,
                               Package = pkg.Name,
                               LeftSideAmount = bi.LeftNewBusinessAmount,
                               RightSideAmount = bi.RightNewBusinessAmount
                           }).ToList();

            double currleftamt = 0;
            double currrightamt = 0;
            for (int i = 0; i < BIncome.Count(); i++)
            {
                BIncome[i].sWeekStartDate = BIncome[i].WeekStartDate.ToLongDateString();
                BIncome[i].sWeekEndDate = BIncome[i].WeekEndDate.ToLongDateString();
                BIncome[i].sDate = BIncome[i].Date.ToLongDateString();
                currleftamt = currleftamt + BIncome[i].LeftSideAmount;
                currrightamt = currrightamt + BIncome[i].RightSideAmount;
            }

            double leftamt = 0;
            double rightamt = 0;
            try
            {
                double? leftamt0 = db.BinaryIncomes.Where(bi => bi.RegistrationId == rec.RegistrationId && bi.WeekNo < weekly.WeekNo).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                double? rightamt0 = db.BinaryIncomes.Where(bi => bi.RegistrationId == rec.RegistrationId && bi.WeekNo < weekly.WeekNo).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                if (leftamt0 != null) { leftamt = (double)leftamt0; }
                if (rightamt0 != null) { rightamt = (double)leftamt0; }
            }
            catch (Exception ex)
            {

            }


            double opLeftAmt = 0;
            double opRightAmt = 0;
            if (leftamt > rightamt) { opLeftAmt = leftamt - rightamt; opRightAmt = 0; }
            if (leftamt < rightamt) { opLeftAmt = 0; opRightAmt = rightamt - leftamt; }
            if (leftamt == rightamt) { opLeftAmt = 0; opRightAmt = 0; }


            double grandTotalLeft = opLeftAmt + currleftamt;
            double grandTotalRight = opRightAmt + currrightamt;

            double businesslimit = 0;

            double currentbusinessToConsider = 0;
            if (grandTotalLeft >= grandTotalRight) { currentbusinessToConsider = grandTotalRight; }
            if (grandTotalLeft < grandTotalRight) { currentbusinessToConsider = grandTotalLeft; }

            if (rec.Defaultpackagecode == 1 && currentbusinessToConsider <= 5000)
            {
                businesslimit = currentbusinessToConsider;
            }
            else if (rec.Defaultpackagecode == 1 && currentbusinessToConsider > 5000)
            {
                businesslimit = 5000;
            }

            if (rec.Defaultpackagecode == 2 && currentbusinessToConsider <= 10000)
            {
                businesslimit = currentbusinessToConsider;
            }
            else if (rec.Defaultpackagecode == 2 && currentbusinessToConsider <= 10000)
            {
                businesslimit = 10000;
            }
            if (rec.Defaultpackagecode == 3 && currentbusinessToConsider <= 30000)
            {
                businesslimit = currentbusinessToConsider;
            }
            else if (rec.Defaultpackagecode == 3 && currentbusinessToConsider <= 30000)
            {
                businesslimit = 30000;
            }

            if (rec.Defaultpackagecode == 4 && currentbusinessToConsider <= 50000)
            {
                businesslimit = currentbusinessToConsider;
            }
            else if (rec.Defaultpackagecode == 4 && currentbusinessToConsider <= 50000)
            {
                businesslimit = 50000;
            }

            double income = (businesslimit * 10) / 100;

            double cdLeftAmt = 0;
            double cdRightAmt = 0;
            if (grandTotalLeft > grandTotalRight) { cdLeftAmt = grandTotalLeft - grandTotalRight; cdRightAmt = 0; }
            if (grandTotalLeft < grandTotalRight) { cdLeftAmt = 0; cdRightAmt = grandTotalRight - grandTotalLeft; }
            if (grandTotalLeft == grandTotalRight) { cdLeftAmt = 0; cdRightAmt = 0; }

            BinaryIncomeLedgerVMTotals totalArr = new BinaryIncomeLedgerVMTotals();
            totalArr.opLeftAmount = opLeftAmt;
            totalArr.opRightAmount = opRightAmt;
            totalArr.CurrentLeftAmount = currleftamt;
            totalArr.CurrentRightAmount = currrightamt;
            totalArr.TotalLeftAmount = grandTotalLeft;
            totalArr.TotalRightAmount = grandTotalRight;
            totalArr.ConsiderationAmount = currentbusinessToConsider;
            totalArr.IncomeAmount = income;
            totalArr.cdLeftAmount = cdLeftAmt;
            totalArr.cdRightAmount = cdRightAmt;

            return Json(new { BIincomeArray = BIncome, TotalsArray = totalArr }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyCurrentSponsorIncome()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);

            var SpIncome = (from si in db.SponsorIncomes
                            from pkg in db.Packages
                            from mem in db.Members
                            where si.PackageId == pkg.Id && si.RegistrationId == rec.RegistrationId &&
                            si.WeekStartDate >= weekly.WeekStartDate && si.WeekEndDate <= weekly.WeekEndDate &&
                            mem.RegistrationId == si.PurchaserRegistrationId
                            orderby si.Id
                            select new BinaryIncomeLedgerVM
                            {
                                Id = si.Id,
                                WeekNo = si.WeekNo,
                                WeekStartDate = si.WeekStartDate,
                                WeekEndDate = si.WeekEndDate,
                                Date = si.TransactionDate,
                                Purchaser = mem.Username,
                                Package = pkg.Name,
                                LeftSideAmount = si.LeftNewBusinessAmount,
                                RightSideAmount = si.RightNewBusinessAmount
                            }).ToList();

            double currleftamt = 0;
            double currrightamt = 0;
            for (int i = 0; i < SpIncome.Count(); i++)
            {
                SpIncome[i].sWeekStartDate = SpIncome[i].WeekStartDate.ToLongDateString();
                SpIncome[i].sWeekEndDate = SpIncome[i].WeekEndDate.ToLongDateString();
                SpIncome[i].sDate = SpIncome[i].Date.ToLongDateString();
                currleftamt = currleftamt + SpIncome[i].LeftSideAmount;
                currrightamt = currrightamt + SpIncome[i].RightSideAmount;
            }


            return Json(new { SpIincomeArray = SpIncome }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyCurrentGenerationIncome()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);

            var GnIncome = (from gi in db.GenerationIncomes
                            from pkg in db.Packages
                            from mem in db.Members
                            where gi.PackageId == pkg.Id && gi.RegistrationId == rec.RegistrationId &&
                            gi.WeekStartDate >= weekly.WeekStartDate && gi.WeekEndDate <= weekly.WeekEndDate &&
                            mem.RegistrationId == gi.PurchaserRegistrationId
                            orderby gi.Id
                            select new BinaryIncomeLedgerVM
                            {
                                Id = gi.Id,
                                WeekNo = gi.WeekNo,
                                WeekStartDate = gi.WeekStartDate,
                                WeekEndDate = gi.WeekEndDate,
                                Date = gi.TransactionDate,
                                Purchaser = mem.Username,
                                Package = pkg.Name,
                                LeftSideAmount = gi.LeftNewBusinessAmount,
                                RightSideAmount = gi.RightNewBusinessAmount
                            }).ToList();

            double currleftamt = 0;
            double currrightamt = 0;
            for (int i = 0; i < GnIncome.Count(); i++)
            {
                GnIncome[i].sWeekStartDate = GnIncome[i].WeekStartDate.ToLongDateString();
                GnIncome[i].sWeekEndDate = GnIncome[i].WeekEndDate.ToLongDateString();
                GnIncome[i].sDate = GnIncome[i].Date.ToLongDateString();
                currleftamt = currleftamt + GnIncome[i].LeftSideAmount;
                currrightamt = currrightamt + GnIncome[i].RightSideAmount;
            }


            return Json(new { GnIincomeArray = GnIncome }, JsonRequestBehavior.AllowGet);
        }

        public WeekModel GetCurrentWeekBoundary(DateTime date)
        {
            WeekModel wk = new WeekModel();
            wk.WeekNo = 0;
            int dayOfToday = (int)date.DayOfWeek;
            if (dayOfToday == 0) { dayOfToday = 7; }
            wk.WeekStartDate = date.AddDays(-1 * (dayOfToday - 1));
            wk.WeekEndDate = date.AddDays(7 - dayOfToday);
            return wk;
        }

        public JsonResult GetCurrentWeekBoundaryTest(DateTime date)
        {
            WeekModel wk = new WeekModel();
            wk.WeekNo = 0;
            int dayOfToday = (int)date.DayOfWeek;
            if (dayOfToday == 0) { dayOfToday = 7; }
            wk.WeekStartDate = date.AddDays(-1 * (dayOfToday - 1));
            wk.WeekEndDate = date.AddDays(7 - dayOfToday);
            return Json(new { WeekBoundary = wk }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Dashboarddata()
        {
            DashboardModel dashdata = new DashboardModel();

            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());

            dashdata.TotalTeamMembers = (long)rec.Totalmembers;

            double totalbusinessL = db.BinaryIncomes.Where(s => s.RegistrationId == rec.RegistrationId).Select(n => n.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
            double totalbusinessR = db.BinaryIncomes.Where(s => s.RegistrationId == rec.RegistrationId).Select(n => n.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
            double totalbusinessSum = totalbusinessL + totalbusinessR;
            double totalselfbusiness = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId).Select(n => n.Amount).DefaultIfEmpty(0).Sum();
            dashdata.TotalTeamBusiness = totalbusinessSum + totalselfbusiness;

            dashdata.InitialInvestment = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId && p.Packageid != 0).Select(n => n.Amount).DefaultIfEmpty(0).Sum();

            dashdata.TotalRepurchase = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId && p.Packageid == 0).Select(n => n.Amount).DefaultIfEmpty(0).Sum();

            int plutocount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 1).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int jupitercount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 2).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int earthcount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 3).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int mercurycount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 4).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int totalcount = plutocount + jupitercount + earthcount + mercurycount;

            dashdata.PlutoPurchasePc = ((plutocount * 100) / totalcount);
            dashdata.JupiterPurchasePc = ((jupitercount * 100) / totalcount);
            dashdata.EarthPurchasePc = ((earthcount * 100) / totalcount);
            dashdata.MercuryPurchasePc = ((mercurycount * 100) / totalcount);
            dashdata.TotalPkgPurchaseCount = totalcount;

            dashdata.MyLeftCount = (long)rec.Leftmembers;
            dashdata.MyRightCount = (long)rec.Rightmembers;

            return Json(new { DashboardDataModel = dashdata }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyFixedIncomeWallet()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeek(rec.Doj, DateTime.Now);
            var FIarray = (from wk in db.WeeklyIncomes
                           from pkg in db.Packages
                           where wk.PackageId == pkg.Id && wk.RegistrationId == rec.RegistrationId && wk.WeekNo <= weekly.WeekNo
                           orderby wk.WeekNo
                           select new FixedIncomeLedgerVM
                           {
                               WeekNo = wk.WeekNo,
                               WeekStartDate = wk.WeekStartDate,
                               WeekEndDate = wk.WeekEndDate,
                               DueDate = wk.DueDate,
                               Amount = wk.Income,
                               Package = pkg.Name,
                               PaymentStatus = "",
                               PaymentDate = wk.PaidDate
                           }).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
            {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                FIarray[i].sDueDate = FIarray[i].DueDate.ToLongDateString();
                if (FIarray[i].PaymentDate == null) { FIarray[i].sPaymentDate = ""; }
                if (FIarray[i].PaymentDate != null)
                {
                    DateTime ddt = (DateTime)FIarray[i].PaymentDate;
                    FIarray[i].sPaymentDate = ddt.ToLongDateString();
                }
                if (FIarray[i].PaymentDate == null) { FIarray[i].PaymentStatus = ""; }
                if (FIarray[i].PaymentDate != null) { FIarray[i].PaymentStatus = "Paid"; }
            }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyCashIncomeWallet()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            WeekModel wm = GetCurrentWeekBoundary(DateTime.Now);
            var FIarray = (from wk in db.SponsorIncomes
                           from pkg in db.Packages
                           where wk.PackageId == pkg.Id &&
                           wk.RegistrationId == rec.RegistrationId &&
                           wk.WeekStartDate >= wm.WeekStartDate && wk.WeekEndDate <= wm.WeekEndDate
                           select new BinaryIncomeLedgerVM
                           {
                               WeekStartDate = wk.WeekStartDate,
                               WeekEndDate = wk.WeekEndDate,
                               Date = wk.TransactionDate,
                               Purchaser = rec.Username,
                               Package = pkg.Name,
                               Total = wk.LeftNewBusinessAmount + wk.RightNewBusinessAmount,
                               WalletAmount = (double)wk.CashWallet,
                               PaymentStatus = "",
                               PaymentDate = wk.PaidDate
                           }).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
            {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                if (FIarray[i].PaymentDate == null) { FIarray[i].sPaymentDate = ""; }
                if (FIarray[i].PaymentDate != null)
                {
                    DateTime ddt = (DateTime)FIarray[i].PaymentDate;
                    FIarray[i].sPaymentDate = ddt.ToLongDateString();
                }
                if (FIarray[i].PaymentDate == null) { FIarray[i].PaymentStatus = ""; }
                if (FIarray[i].PaymentDate != null) { FIarray[i].PaymentStatus = "Paid"; }
            }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyReserveIncomeWallet()
        {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            WeekModel wm = GetCurrentWeekBoundary(DateTime.Now);
            var FIarray = (from wk in db.SponsorIncomes
                           from pkg in db.Packages
                           where wk.PackageId == pkg.Id &&
                           wk.RegistrationId == rec.RegistrationId &&
                           wk.WeekStartDate >= wm.WeekStartDate && wk.WeekEndDate <= wm.WeekEndDate
                           select new BinaryIncomeLedgerVM
                           {
                               WeekStartDate = wk.WeekStartDate,
                               WeekEndDate = wk.WeekEndDate,
                               Date = wk.TransactionDate,
                               Purchaser = rec.Username,
                               Package = pkg.Name,
                               Total = wk.LeftNewBusinessAmount + wk.RightNewBusinessAmount,
                               WalletAmount = (double)wk.ReserveWallet,
                               PaymentStatus = "",
                               PaymentDate = wk.PaidDate
                           }).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
            {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                if (FIarray[i].PaymentDate == null) { FIarray[i].sPaymentDate = ""; }
                if (FIarray[i].PaymentDate != null)
                {
                    DateTime ddt = (DateTime)FIarray[i].PaymentDate;
                    FIarray[i].sPaymentDate = ddt.ToLongDateString();
                }
                if (FIarray[i].PaymentDate == null) { FIarray[i].PaymentStatus = ""; }
                if (FIarray[i].PaymentDate != null) { FIarray[i].PaymentStatus = "Paid"; }
            }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
        }

        public string PackageImage(int packageId)
        {
            string path = "";
            if (packageId == 1) { path = "<img src='../Content/pluto1.png' style='width:30px;height:30px'/><br/>"; }
            if (packageId == 2) { path = "<img src='../Content/Jupiter1.png' style='width:30px;height:30px'/><br/>"; }
            if (packageId == 3) { path = "<img src='../Content/Earth1.png' style='width:30px;height:30px'/><br/>"; }
            if (packageId == 4) { path = "<img src='../Content/Mercury1.png' style='width:30px;height:30px'/><br/>"; }
            return path;
        }

        public JsonResult MemberDetail()
        {
            string username = User.Identity.Name;
            var mem = db.Members.SingleOrDefault(m => m.Username == username);

            return Json(new { Member = mem }, JsonRequestBehavior.AllowGet);
        }

        private async Task<bool> SendEMail(string mail_to, string mail_cc, string subj, string desc, ArrayList arr)
        {
            string mail_from = "jc.chakrabarty@gmail.com";
            //string mail_cc = "";
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht.Add("FROM", mail_from);
            ht.Add("TO", mail_to);
            ht.Add("CC", mail_cc);
            //ht.Add("BCC", mail_cc);
            ht.Add("SUBJECT", subj);
            ht.Add("BODY", desc);
            ht.Add("ATTACHMENT", arr);

            try
            {
                Email mail = new Email(ht);
                mail.SendEMail();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<JsonResult> SendCandidateEmail(string email)
        {
            bool result = false;
            ArrayList array = new ArrayList();
            string body = "";
            body = @"<table style='width:100%'>
                            <tr>
                             <p style='text-align:left;font-size:14px;font-color:black;font-weight:600'> Dear,</p>
                           </tr> 
                           <tr>
                              <p style='text-align:justified;font-size:12px;'> This is a test email. </p>
                           </tr> 
                       </table>";
            result = await SendEMail(email, "", "Submission Confirmation", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TransactionPasswordExists()
        {
            var user = User.Identity.Name;
            var res = db.Members.Where(i => i.Username == user).Select(i => i.Transactionpassword).ToList();
            Boolean isExists = false;
            if (res[0] != "") { isExists = true; }
            return Json(new { isEXISTS = isExists }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TransactionPasswordMatch(string OldPassword)
        {
            var user = User.Identity.Name;
            var res = db.Members.Where(i => i.Username == user).Select(i => i.Transactionpassword).ToList();
            Boolean isMatching = false;
            if (res[0] == OldPassword) { isMatching = true; }
            return Json(new { isOK = isMatching }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PasswordMatch(string OldPassword)
        {
            var user = User.Identity.Name;
            var res = db.Registrations.Where(i => i.UserName == user).Select(i => i.Password).ToList();
            Boolean isMatching = false;
            if (res[0] == OldPassword) { isMatching = true; }
            return Json(new { isOK = isMatching }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrationDetail()
        {
            string username = User.Identity.Name;
            var mem = db.Registrations.SingleOrDefault(m => m.UserName == username);

            return Json(new { Member = mem }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

            
        }
        #endregion
    }
}