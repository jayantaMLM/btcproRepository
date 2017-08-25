using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcProApp.Models;
using System.Collections;
using System.Threading.Tasks;

namespace BtcProApp.Controllers
    {
    [Authorize]
    public class HomeController : Controller
        {
        private BtcProDB db = new BtcProDB();
        private dbviewDataContext dbView = new dbviewDataContext();

        [AllowAnonymous]
        public ActionResult IndexMain(string id)
            {
            ViewBag.Username = id;

            if (id != null)
                {
                var mem = db.Members.SingleOrDefault(m => m.Username.ToUpper() == id.ToUpper());
                if (mem == null)
                    {
                    return View("_SponsorError");
                    }
                else
                    {
                    ViewBag.Username = mem.Username;
                    ViewBag.Fullname = mem.Firstname;
                    return View("Registration");
                    }
                }
            else
                {
                ViewBag.Username = "";
                return View();
                }

            }
        //public ActionResult IndexMain()
        //{
        //    return View();
        //}
        public ActionResult Index()
            {

            return View();
            }

        public ActionResult Members()
            {
            return View();
            }

        public ActionResult MemberWallets()
            {
            return View();
            }

        public JsonResult MembersList()
            {
            var members = ( from m in db.Registrations
                            select new
                                {
                                Id = m.Id,
                                UserId = m.UserName,
                                UserName = m.FullName,
                                SponsorId = m.ReferrerName,
                                EmailId = m.EmailId,
                                JoiningDate = m.CreatedDate.ToString(),
                                CountryCode = m.CountryCode,
                                Status = ( m.Joined == true ? "Joined" : "Free Member" ),
                                } ).ToList();

            return Json(new { MemberList = members }, JsonRequestBehavior.AllowGet);

            }

        public JsonResult MemberAllWallets()
            {
            var memberWallets = ( from m in db.Members
                                  select new vmMemberWallet
                                      {
                                      RegistrationId = m.RegistrationId,
                                      MemberName = m.Username,
                                      CashWalletAmount = 0,
                                      ReserveWalletAmount = 0,
                                      ReturnWalletAmount = 0,
                                      FrozenWalletAmount = 0
                                      } ).ToList();
            try
                {
                for (int i = 0; i < memberWallets.Count(); i++)
                    {
                    memberWallets[i].CashWalletAmount = MemberWalletBalance(memberWallets[i].MemberName, 1);
                    memberWallets[i].ReserveWalletAmount = MemberWalletBalance(memberWallets[i].MemberName, 2);
                    memberWallets[i].ReturnWalletAmount = MemberWalletBalance(memberWallets[i].MemberName, 3);
                    }

                }
            catch (Exception e)
                {

                }

            return Json(new { Wallets = memberWallets }, JsonRequestBehavior.AllowGet);
            }
        [HttpPost]
        public JsonResult ResetCountry(long Id, string CountryCode)
            {
            Register reg = db.Registrations.SingleOrDefault(r => r.Id == Id);
            reg.CountryCode = CountryCode;

            Member mem = db.Members.SingleOrDefault(m => m.RegistrationId == Id);
            mem.Country = CountryCode;

            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult Country()
            {
            string user = User.Identity.Name;
            string homeCountryName = "";
            string homeCountryCode = db.Registrations.Where(r => r.UserName == user).Select(r => r.CountryCode).Single();
            if (homeCountryCode == "") { homeCountryName = ""; } else { homeCountryName = db.Countries.Where(c => c.CountryId == homeCountryCode).Select(c => c.Mapimagename).Single(); }

            ViewBag.ImagePath = Url.Content("~/images/flags/" + homeCountryName);

            return Json(new { CountryName = ViewBag.ImagePath }, JsonRequestBehavior.AllowGet);
            }

        [AllowAnonymous]
        public ActionResult Aboutus()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult Ethereum()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult BizOpportunity()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult Faq()
            {
            return View();
            }

        public ActionResult Contactus()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult Registration()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult Login()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult Contact()
            {

            return View("ContactUs");
            }
        public ActionResult TeamMembers()
            {
            ViewBag.Name = User.Identity.Name;
            return View();
            }

        public ActionResult MemberArea()
            {
            return View();
            }
        public ActionResult MemberBlockUnblock()
            {
            return View();
            }
        public ActionResult PayoutByAdmin()
            {
            return View();
            }

        public ActionResult ReferralLink()
            {
            return View();
            }
        public ActionResult ProcessSupportTickets()
            {
            return View();
            }
        public ActionResult Genealogy()
            {
            ViewBag.Name = User.Identity.Name;
            return View();
            }
        public ActionResult TeamStructure()
            {
            return View();
            }
        public ActionResult TeamStructureNew()
            {
            return View();
            }
        public ActionResult GenerationStructure()
            {
            return View();
            }
        public ActionResult Network()
            {
            return View();
            }
        public ActionResult MyPurchase()
            {
            return View();
            }
        public ActionResult MyRePurchase()
            {
            return View();
            }

        public ActionResult FreeJoining()
            {
            return View();
            }
        public ActionResult PackagesShop()
            {
            return View();
            }
        public ActionResult OrderHistory()
            {
            return View();
            }
        public ActionResult Invoices()
            {
            return View();
            }
        public ActionResult SalesHistory()
            {
            return View();
            }
        public ActionResult Illustration()
            {
            return View();
            }
        public ActionResult CashWallet()
            {
            return View();
            }
        public ActionResult ReturnWallet()
            {
            return View();
            }
        public ActionResult ReserveWallet()
            {
            return View();
            }
        public ActionResult FrozenWallet()
            {
            return View();
            }
        public ActionResult Earnings()
            {
            return View();
            }
        public ActionResult Withdrawals()
            {
            return View();
            }
        public ActionResult Transfers()
            {
            return View();
            }
        public ActionResult AdminTransfers()
            {
            return View();
            }
        public ActionResult UploadBalance()
            {
            return View();
            }
        public ActionResult WithdrawalRequests()
            {
            return View();
            }
        public ActionResult ChangeUserId()
            {
            return View();
            }
        public ActionResult TransferRequests()
            {
            return View();
            }

        public ActionResult MemberPage()
            {
            return View();
            }
        public ActionResult EditProfile()
            {
            return View();
            }
        public ActionResult UploadKYC()
            {
            return View();
            }
        public ActionResult AccountStatus()
            {
            return View();
            }
        public ActionResult ChangePassword()
            {
            return View();
            }
        public ActionResult ChangeTrnPassword()
            {
            return View();
            }

        public ActionResult ApproveBitcoinTransfers()
            {
            return View();
            }
        //public ActionResult DirectJoining()
        //{

        //}

        public ActionResult Tickets()
            {
            return View();
            }

        public ActionResult FixedPayout()
            {
            return View();
            }
        public ActionResult ManageNews()
            {
            return View();
            }
        public ActionResult Notifications()
            {
            return View();
            }
        public ActionResult Logout()
            {
            return View();
            }

        #region Common Code

        [AllowAnonymous]
        [HttpGet]
        public JsonResult IsUserNameExist(string UserName)
            {
            bool isExist = true;
            long Id = 0;
            string binarypos = "";
            Member mem = db.Members.SingleOrDefault(m => m.Username.ToUpper().Trim() == UserName.ToUpper().Trim());
            if (mem == null)
                {
                isExist = false;
                }
            else
                {
                Register reg = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
                Id = reg.Id;
                binarypos = reg.BinaryPosition;
                }
            return Json(new { Found = isExist, ReferrerId = Id, BinaryPos = binarypos, Sponsorname = mem.Firstname }, JsonRequestBehavior.AllowGet);
            }

        [HttpGet]
        public JsonResult CurrentUserName()
            {
            string user = User.Identity.Name;
            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == user.ToUpper());
            return Json(new { CurrentUser = user, UserId = reg.Id }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MyAccountNo(string WalletId)
            {
            string user = User.Identity.Name;
            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == user.ToUpper());
            reg.MyWalletAccount = WalletId;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult GetMyAccountNo()
            {
            string user = User.Identity.Name;
            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == user.ToUpper());
            return Json(new { WalletAc = reg.MyWalletAccount }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckIdentity(string username, string emailId)
            {
            bool status = false;
            string message = "";
            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == username.ToUpper());
            if (reg != null)
                {
                if (reg.EmailId == emailId)
                    {
                    //send email
                    string line0 = "<table style='width:100%'>";
                    string line1 = "<tr><td colspan='2'>Dear <b>" + username + "</b></td></tr>";
                    string line2 = "<tr style='line-height:28px'><td colspan='2'>You have requested to restore password of your account.</td></tr>";
                    string line3 = "<tr style='line-height:40px'><td colspan='2'>The details are as follows:</td></tr>";
                    string line4 = "<tr><td style='width:50px'>Username:</td><td><b>" + username + "</b></td></tr>";
                    string line5 = "<tr><td style='width:50px'>Password:</td><td><b>" + reg.Password + "</b></td></tr>";
                    string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
                    string line9 = "<tr><td colspan='2' style='line-height:48px'>You can change this password in your personal area</td><tr>";
                    string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
                    string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
                    string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
                    string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
                    string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
                    string line15 = "</table>";
                    string body = line0 + line1 + line2 + line3 + line4 + line5 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;


                    string mail_from = "noreply@btcpro.co";
                    string mail_to = reg.EmailId;
                    ArrayList arr = null;
                    string subj = "Forget Password mail";
                    string mail_cc = "info@btcpro.co";
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    ht.Add("FROM", mail_from);
                    ht.Add("TO", mail_to);
                    ht.Add("CC", mail_cc);
                    //ht.Add("BCC", mail_cc);
                    ht.Add("SUBJECT", subj);
                    ht.Add("BODY", body);
                    ht.Add("ATTACHMENT", arr);

                    try
                        {
                        Email mail = new Email(ht);
                        mail.SendEMail();

                        }
                    catch (Exception e)
                        {

                        }
                    status = true;
                    message = "An email has been sent to your registered email Id.";
                    return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
                    }
                else
                    {
                    status = false;
                    message = "ERROR!!! Username and registered email id did not match.";
                    return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
            else
                {
                status = false;
                message = "ERROR!!! Username not found.";
                return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
                }

            //return Json(JsonRequestBehavior.AllowGet);
            }


        [HttpPost]
        public JsonResult LedgerPosting(string Username, string DrCr, int WalletType, double Amount, string Comment)
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
            ldgr.Comment = Comment;

            db.Ledgers.Add(ldgr);
            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        private bool ledgerposting(string Username, string DrCr, int WalletType, double Amount, string Comment)
            {

            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == Username.ToUpper().Trim());
            Ledger ldgr = new Ledger();
            ldgr.RegistrationId = rec.Id;
            ldgr.WalletId = WalletType;
            ldgr.Date = DateTime.Now;
            if (DrCr == "D") { ldgr.Deposit = Amount; }
            if (DrCr == "W") { ldgr.Withdraw = Amount; }
            ldgr.TransactionTypeId = 2;
            ldgr.TransactionId = 0;
            ldgr.SubLedgerId = 2;
            ldgr.ToFromUser = 1;
            ldgr.Comment = Comment;

            db.Ledgers.Add(ldgr);
            db.SaveChanges();

            return true;
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

        //called when a member puts request for withdrawal
        public JsonResult WithdrawPostingMember(int WalletType, double Amount)
            {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            var currentusr = User.Identity.Name;
            if (currentusr != null || currentusr != "")
                {
                try
                    {
                    var rec0 = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == currentusr.ToUpper().Trim());
                    WithdrawalRequest req = new WithdrawalRequest();
                    req.RegistrationId = rec0.Id;
                    req.Date = DateTime.Now;
                    req.WalletId = WalletType;
                    req.Amount = Amount;
                    req.ServiceCharge = ( Amount * 5 ) / 100;
                    req.PaidOutAmount = Amount - req.ServiceCharge;
                    req.Status = "Under Process";
                    req.BatchNo = GuidString;
                    req.PaidBitCoinAccount = rec0.MyWalletAccount;
                    db.WithdrawalRequests.Add(req);
                    db.SaveChanges();

                    Ledger ldgr = new Ledger();
                    ldgr.RegistrationId = rec0.Id;
                    ldgr.WalletId = WalletType;
                    ldgr.Date = DateTime.Now;
                    ldgr.Withdraw = Amount;
                    ldgr.Deposit = 0;
                    ldgr.TransactionTypeId = 3;
                    ldgr.TransactionId = req.Id;
                    ldgr.SubLedgerId = 2;
                    ldgr.ToFromUser = 0;
                    ldgr.BatchNo = GuidString;

                    db.Ledgers.Add(ldgr);
                    db.SaveChanges();
                    }
                catch (Exception e)
                    {

                    }

                }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult CancelOrder(int Id, string remarks, string comment)
            {
            Boolean success = false;
            try
                {
                var withreqRec = db.WithdrawalRequests.SingleOrDefault(w => w.Id == Id);
                withreqRec.Status = remarks;
                withreqRec.Comment = comment;
                string btchno = withreqRec.BatchNo;

                var rec0 = db.Ledgers.SingleOrDefault(l => l.BatchNo == btchno);
                Ledger ldgr = new Ledger();
                ldgr.RegistrationId = rec0.RegistrationId;
                ldgr.WalletId = rec0.WalletId;
                ldgr.Date = DateTime.Now;
                ldgr.Withdraw = 0;
                ldgr.Deposit = rec0.Withdraw;
                ldgr.TransactionTypeId = rec0.TransactionTypeId;
                ldgr.TransactionId = rec0.TransactionId;
                ldgr.SubLedgerId = 2;
                ldgr.ToFromUser = 0;
                ldgr.BatchNo = rec0.BatchNo;

                db.Ledgers.Add(ldgr);
                db.SaveChanges();

                success = true;
                }
            catch (Exception e)
                {

                }

            return Json(new { Success = success }, JsonRequestBehavior.AllowGet);
            }

        [HttpGet]
        public JsonResult GetMyFreeMembers()
            {
            var Username = User.Identity.Name;
            var recs = db.Registrations.Where(r => r.ReferrerName == Username && r.Joined == false).ToList();

            for (int i = 0; i < recs.Count; i++)
                {
                recs[i].Password = recs[i].CreatedDate.ToLongDateString();
                }

            return Json(new { FreeMembers = recs }, JsonRequestBehavior.AllowGet);
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

            var m = db.Members.Where(mm => mm.ReferrerRegistrationId == regId);
            members.AddRange(m);
            long lastId = 0;

            //for (int i = 0; i < members.Count(); i++)
            //{
            //    lastId = members[i].RegistrationId;
            //    var n = db.Members.Where(mm => mm.UplineRegistrationId == lastId);
            //    members.AddRange(n);
            //}

            var MemList = members.Select(n => new { n.RegistrationId, n.Username, n.Firstname, n.Defaultpackagecode, n.Country, Doj = n.Doj.ToShortDateString(), n.BinaryPosition }).OrderByDescending(n => n.Doj);

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
        public JsonResult GetTeamByName(string Member)   //Get Binary Tree of a name
            {
            if (Member == "")
                {
                Member = User.Identity.Name;
                }

            long regId = 10000000;
            List<Member> members = new List<Member>();

            var res = db.Members.SingleOrDefault(x => x.Username == Member);

            if (res != null)
                {
                #region found

                long MemberId = res.RegistrationId;
                int pkgcode = 0;
                long regid = 0;

                //Add the first member to array first
                List<Treemodel> MemberList = new List<Treemodel>();
                Treemodel Mem0 = new Treemodel();
                Mem0.id = res.RegistrationId;
                Mem0.UplineRegistrationId = res.RegistrationId;
                Mem0.parentId = null;
                Mem0.Name = "";
                Mem0.pic = PackageImage((int) res.Defaultpackagecode);
                Mem0.isMember = true;
                Mem0.fullname = res.Firstname;
                Mem0.username = res.Username;
                Mem0.sponsorname = res.Referrerusername;
                Mem0.package = db.Packages.Where(p => p.Id == res.Defaultpackagecode).Select(p => p.Name).SingleOrDefault();
                Mem0.totalleft = (long) res.Leftmembers;
                Mem0.totalright = (long) res.Rightmembers;
                Mem0.totalbusinessleft = db.BinaryIncomes.Where(i => i.RegistrationId == res.RegistrationId).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                Mem0.totalbusinessright = db.BinaryIncomes.Where(i => i.RegistrationId == res.RegistrationId).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                BinaryIncomeLedgerVMTotals BI = GetCurrentBinaryIncome(res.Username);
                Mem0.businessleft = BI.CurrentLeftAmount;
                Mem0.businessright = BI.CurrentRightAmount;
                Mem0.achievement = res.Achievement1 == 1 ? "Diamond" : "-";
                MemberList.Add(Mem0);


                int param1 = 0;
                int param2 = 0;
                for (int lvl = 1; lvl <= 4; lvl++)
                    {
                    if (lvl == 1) { param1 = 0; param2 = 0; }
                    if (lvl == 2) { param1 = 1; param2 = 2; }
                    if (lvl == 3) { param1 = 3; param2 = 6; }
                    if (lvl == 4) { param1 = 7; param2 = 14; }

                    #region Level 1,2,3,4
                    for (int i = param1; i <= param2; i++)
                        {
                        try
                            {
                            MemberId = MemberList[i].UplineRegistrationId;
                            var m = db.Members.Where(mm => mm.UplineRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();
                            if (m.Count() == 0)
                                {
                                Treemodel Mem1 = new Treemodel();
                                regId = regId + 1;
                                Mem1.id = regId;
                                Mem1.UplineRegistrationId = regId;
                                Mem1.parentId = MemberList[i].id;
                                Mem1.Name = "L";
                                Mem1.isMember = false;
                                if (MemberList[i].isMember) { Mem1.pic = PackageImage(99); } else { Mem1.pic = ""; }
                                Mem1.position = "L";
                                MemberList.Add(Mem1);

                                Treemodel Mem2 = new Treemodel();
                                regId = regId + 1;
                                Mem2.id = regId;
                                Mem2.UplineRegistrationId = regId;
                                Mem2.parentId = MemberList[i].id;
                                Mem2.isMember = false;
                                Mem2.Name = "R";
                                if (MemberList[i].isMember) { Mem2.pic = PackageImage(99); } else { Mem2.pic = ""; }
                                Mem2.position = "R";
                                MemberList.Add(Mem2);
                                }

                            if (m.Count() == 1)
                                {
                                if (m[0].BinaryPosition == "L")
                                    {
                                    Treemodel Mem1 = new Treemodel();
                                    regId = regId + 1;
                                    Mem1.id = regId;
                                    Mem1.UplineRegistrationId = m[0].RegistrationId;
                                    Mem1.parentId = MemberList[i].id;
                                    Mem1.Name = "L";
                                    Mem1.isMember = true;
                                    Mem1.pic = PackageImage((int) m[0].Defaultpackagecode);
                                    Mem1.position = "L";
                                    Mem1.fullname = m[0].Firstname;
                                    Mem1.username = m[0].Username;
                                    Mem1.sponsorname = m[0].Referrerusername;
                                    pkgcode = (int) m[0].Defaultpackagecode;
                                    Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                    Mem1.totalleft = (long) m[0].Leftmembers;
                                    Mem1.totalright = (long) m[0].Rightmembers;
                                    regid = m[0].RegistrationId;
                                    Mem1.totalbusinessleft = db.BinaryIncomes.Where(y => y.RegistrationId == regid).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                    Mem1.totalbusinessright = db.BinaryIncomes.Where(z => z.RegistrationId == regid).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                    BinaryIncomeLedgerVMTotals BI1 = GetCurrentBinaryIncome(m[0].Username);
                                    Mem1.businessleft = BI1.CurrentLeftAmount;
                                    Mem1.businessright = BI1.CurrentRightAmount;
                                    Mem1.achievement = m[0].Achievement1 == 1 ? "Diamond" : "-";

                                    MemberList.Add(Mem1);

                                    Treemodel Mem2 = new Treemodel();
                                    regId = regId + 1;
                                    Mem2.id = regId;
                                    Mem2.UplineRegistrationId = regId;
                                    Mem2.parentId = MemberList[i].id;
                                    Mem2.Name = "R";
                                    Mem2.isMember = false;
                                    if (MemberList[i].isMember) { Mem2.pic = PackageImage(99); } else { Mem2.pic = ""; }
                                    Mem2.position = "R";
                                    MemberList.Add(Mem2);
                                    }
                                if (m[0].BinaryPosition == "R")
                                    {
                                    Treemodel Mem1 = new Treemodel();
                                    regId = regId + 1;
                                    Mem1.id = regId;
                                    Mem1.UplineRegistrationId = regId;
                                    Mem1.parentId = MemberList[i].id;
                                    Mem1.Name = "L";
                                    Mem1.isMember = false;
                                    if (MemberList[i].isMember) { Mem1.pic = PackageImage(99); } else { Mem1.pic = ""; }
                                    Mem1.position = "L";
                                    MemberList.Add(Mem1);

                                    Treemodel Mem2 = new Treemodel();
                                    regId = regId + 1;
                                    Mem2.id = regId;
                                    Mem2.UplineRegistrationId = m[0].RegistrationId;
                                    Mem2.parentId = MemberList[i].id;
                                    Mem2.Name = "R";
                                    Mem2.isMember = true;
                                    Mem2.pic = PackageImage((int) m[0].Defaultpackagecode);
                                    Mem2.position = "R";
                                    Mem2.fullname = m[0].Firstname;
                                    Mem2.username = m[0].Username;
                                    Mem2.sponsorname = m[0].Referrerusername;
                                    pkgcode = (int) m[0].Defaultpackagecode;
                                    Mem2.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                    Mem2.totalleft = (long) m[0].Leftmembers;
                                    Mem2.totalright = (long) m[0].Rightmembers;
                                    regid = m[0].RegistrationId;
                                    Mem2.totalbusinessleft = db.BinaryIncomes.Where(y => y.RegistrationId == regid).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                    Mem2.totalbusinessright = db.BinaryIncomes.Where(z => z.RegistrationId == regid).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                    BinaryIncomeLedgerVMTotals BI2 = GetCurrentBinaryIncome(m[0].Username);
                                    Mem2.businessleft = BI2.CurrentLeftAmount;
                                    Mem2.businessright = BI2.CurrentRightAmount;
                                    Mem2.achievement = m[0].Achievement1 == 1 ? "Diamond" : "-";

                                    MemberList.Add(Mem2);



                                    }
                                }
                            if (m.Count() == 2)
                                {
                                Treemodel Mem1 = new Treemodel();
                                regId = regId + 1;
                                Mem1.id = regId;
                                Mem1.UplineRegistrationId = m[0].RegistrationId;
                                Mem1.parentId = MemberList[i].id;
                                Mem1.Name = "L";
                                Mem1.isMember = true;
                                Mem1.pic = PackageImage((int) m[0].Defaultpackagecode);
                                Mem1.position = "L";
                                Mem1.fullname = m[0].Firstname;
                                Mem1.username = m[0].Username;
                                Mem1.sponsorname = m[0].Referrerusername;
                                pkgcode = (int) m[0].Defaultpackagecode;
                                Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                Mem1.totalleft = (long) m[0].Leftmembers;
                                Mem1.totalright = (long) m[0].Rightmembers;
                                regid = m[0].RegistrationId;
                                Mem1.totalbusinessleft = db.BinaryIncomes.Where(y => y.RegistrationId == regid).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                Mem1.totalbusinessright = db.BinaryIncomes.Where(z => z.RegistrationId == regid).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                BinaryIncomeLedgerVMTotals BI3 = GetCurrentBinaryIncome(m[0].Username);
                                Mem1.businessleft = BI3.CurrentLeftAmount;
                                Mem1.businessright = BI3.CurrentRightAmount;
                                Mem1.achievement = m[0].Achievement1 == 1 ? "Diamond" : "-";

                                MemberList.Add(Mem1);

                                Treemodel Mem2 = new Treemodel();
                                regId = regId + 1;
                                Mem2.id = regId;
                                Mem2.UplineRegistrationId = m[1].RegistrationId;
                                Mem2.parentId = MemberList[i].id;
                                Mem2.Name = "R";
                                Mem2.isMember = true;
                                Mem2.pic = PackageImage((int) m[1].Defaultpackagecode);
                                Mem2.position = "R";
                                Mem2.fullname = m[1].Firstname;
                                Mem2.username = m[1].Username;
                                Mem2.sponsorname = m[1].Referrerusername;
                                pkgcode = (int) m[1].Defaultpackagecode;
                                Mem2.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                Mem2.totalleft = (long) m[1].Leftmembers;
                                Mem2.totalright = (long) m[1].Rightmembers;
                                regid = m[1].RegistrationId;
                                Mem2.totalbusinessleft = db.BinaryIncomes.Where(y => y.RegistrationId == regid).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                Mem2.totalbusinessright = db.BinaryIncomes.Where(z => z.RegistrationId == regid).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
                                BinaryIncomeLedgerVMTotals BI4 = GetCurrentBinaryIncome(m[1].Username);
                                Mem2.businessleft = BI4.CurrentLeftAmount;
                                Mem2.businessright = BI4.CurrentRightAmount;
                                Mem2.achievement = m[1].Achievement1 == 1 ? "Diamond" : "-";

                                MemberList.Add(Mem2);

                                }
                            }
                        catch (Exception e)
                            {

                            }
                        }
                    #endregion
                    }

                return Json(new { Members = MemberList }, JsonRequestBehavior.AllowGet);

                #endregion
                }
            else
                {
                #region not found

                List<Treemodel> MemList = new List<Treemodel>();
                Treemodel Mem0 = new Treemodel();
                Mem0.id = 0;
                Mem0.parentId = null;
                Mem0.Name = "";
                Mem0.pic = "";
                MemList.Add(Mem0);

                Treemodel Mem1 = new Treemodel();
                Mem1.id = 1;
                Mem1.parentId = 0;
                Mem1.Name = "";
                Mem1.pic = "";
                MemList.Add(Mem1);
                Treemodel Mem2 = new Treemodel();
                Mem2.id = 2;
                Mem2.parentId = 0;
                Mem2.Name = "";
                Mem2.pic = "";
                MemList.Add(Mem2);

                Treemodel Mem3 = new Treemodel();
                Mem3.id = 3;
                Mem3.parentId = 1;
                Mem3.Name = "";
                Mem3.pic = "";
                MemList.Add(Mem3);
                Treemodel Mem4 = new Treemodel();
                Mem4.id = 4;
                Mem4.parentId = 1;
                Mem4.Name = "";
                Mem4.pic = "";
                MemList.Add(Mem4);
                Treemodel Mem5 = new Treemodel();
                Mem5.id = 5;
                Mem5.parentId = 2;
                Mem5.Name = "";
                Mem5.pic = "";
                MemList.Add(Mem5);
                Treemodel Mem6 = new Treemodel();
                Mem6.id = 6;
                Mem6.parentId = 2;
                Mem6.Name = "";
                Mem6.pic = "";
                MemList.Add(Mem6);

                Treemodel Mem7 = new Treemodel();
                Mem7.id = 7;
                Mem7.parentId = 3;
                Mem7.Name = "";
                Mem7.pic = "";
                MemList.Add(Mem7);
                Treemodel Mem8 = new Treemodel();
                Mem8.id = 8;
                Mem8.parentId = 3;
                Mem8.Name = "";
                Mem8.pic = "";
                MemList.Add(Mem8);
                Treemodel Mem9 = new Treemodel();
                Mem9.id = 9;
                Mem9.parentId = 4;
                Mem9.Name = "";
                Mem9.pic = "";
                MemList.Add(Mem9);
                Treemodel Mem10 = new Treemodel();
                Mem10.id = 10;
                Mem10.parentId = 4;
                Mem10.Name = "";
                Mem10.pic = "";
                MemList.Add(Mem10);
                Treemodel Mem11 = new Treemodel();
                Mem11.id = 11;
                Mem11.parentId = 5;
                Mem11.Name = "";
                Mem11.pic = "";
                MemList.Add(Mem11);
                Treemodel Mem12 = new Treemodel();
                Mem12.id = 12;
                Mem12.parentId = 5;
                Mem12.Name = "";
                Mem12.pic = "";
                MemList.Add(Mem12);
                Treemodel Mem13 = new Treemodel();
                Mem13.id = 13;
                Mem13.parentId = 6;
                Mem13.Name = "";
                Mem13.pic = "";
                MemList.Add(Mem13);
                Treemodel Mem14 = new Treemodel();
                Mem14.id = 14;
                Mem14.parentId = 6;
                Mem14.Name = "";
                Mem14.pic = "";
                MemList.Add(Mem14);

                Treemodel Mem15 = new Treemodel();
                Mem15.id = 15;
                Mem15.parentId = 7;
                Mem15.Name = "";
                Mem15.pic = "";
                MemList.Add(Mem15);
                Treemodel Mem16 = new Treemodel();
                Mem16.id = 16;
                Mem16.parentId = 7;
                Mem16.Name = "";
                Mem16.pic = "";
                MemList.Add(Mem16);

                Treemodel Mem17 = new Treemodel();
                Mem17.id = 17;
                Mem17.parentId = 8;
                Mem17.Name = "";
                Mem17.pic = "";
                MemList.Add(Mem17);
                Treemodel Mem18 = new Treemodel();
                Mem18.id = 18;
                Mem18.parentId = 8;
                Mem18.Name = "";
                Mem18.pic = "";
                MemList.Add(Mem18);

                Treemodel Mem19 = new Treemodel();
                Mem19.id = 19;
                Mem19.parentId = 9;
                Mem19.Name = "";
                Mem19.pic = "";
                MemList.Add(Mem19);
                Treemodel Mem20 = new Treemodel();
                Mem20.id = 20;
                Mem20.parentId = 9;
                Mem20.Name = "";
                Mem20.pic = "";
                MemList.Add(Mem20);

                Treemodel Mem21 = new Treemodel();
                Mem21.id = 21;
                Mem21.parentId = 10;
                Mem21.Name = "";
                Mem21.pic = "";
                MemList.Add(Mem21);
                Treemodel Mem22 = new Treemodel();
                Mem22.id = 22;
                Mem22.parentId = 10;
                Mem22.Name = "";
                Mem22.pic = "";
                MemList.Add(Mem22);
                Treemodel Mem23 = new Treemodel();
                Mem23.id = 23;
                Mem23.parentId = 11;
                Mem23.Name = "";
                Mem23.pic = "";
                MemList.Add(Mem23);
                Treemodel Mem24 = new Treemodel();
                Mem24.id = 24;
                Mem24.parentId = 11;
                Mem24.Name = "";
                Mem24.pic = "";
                MemList.Add(Mem24);
                Treemodel Mem25 = new Treemodel();
                Mem25.id = 25;
                Mem25.parentId = 12;
                Mem25.Name = "";
                Mem25.pic = "";
                MemList.Add(Mem25);
                Treemodel Mem26 = new Treemodel();
                Mem26.id = 26;
                Mem26.parentId = 12;
                Mem26.Name = "";
                Mem26.pic = "";
                MemList.Add(Mem26);
                Treemodel Mem27 = new Treemodel();
                Mem27.id = 27;
                Mem27.parentId = 13;
                Mem27.Name = "";
                Mem27.pic = "";
                MemList.Add(Mem27);
                Treemodel Mem28 = new Treemodel();
                Mem28.id = 28;
                Mem28.parentId = 13;
                Mem28.Name = "";
                Mem28.pic = "";
                MemList.Add(Mem28);
                Treemodel Mem29 = new Treemodel();
                Mem29.id = 29;
                Mem29.parentId = 14;
                Mem29.Name = "";
                Mem29.pic = "";
                MemList.Add(Mem29);
                Treemodel Mem30 = new Treemodel();
                Mem30.id = 30;
                Mem30.parentId = 14;
                Mem30.Name = "";
                Mem30.pic = "";
                MemList.Add(Mem30);

                return Json(new { Members = MemList }, JsonRequestBehavior.AllowGet);

                #endregion
                }

            }

        [HttpGet]
        public JsonResult GetBinaryTeamByTree(string Member, int levels, int? Id)   //Get Binary Tree of a name
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
                if (mm.Defaultpackagecode == 6) { defaultPackage = "amazing.png"; }
                if (mm.Defaultpackagecode == 7) { defaultPackage = "octa-core.png"; }
                }

            if (Member == "" && Id == null)
                {
                Member = User.Identity.Name;
                var mm = db.Members.SingleOrDefault(i => i.Username == Member);
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                if (mm.Defaultpackagecode == 6) { defaultPackage = "amazing.png"; }
                if (mm.Defaultpackagecode == 7) { defaultPackage = "octa-core.png"; }
                }
            if (Member == "" && Id != null)
                {
                var mm = db.Members.SingleOrDefault(i => i.Id == Id);
                Member = mm.Username;
                if (mm.Defaultpackagecode == 1) { defaultPackage = "pluto1.png"; }
                if (mm.Defaultpackagecode == 2) { defaultPackage = "Jupiter1.png"; }
                if (mm.Defaultpackagecode == 3) { defaultPackage = "Earth1.png"; }
                if (mm.Defaultpackagecode == 4) { defaultPackage = "Mercury1.png"; }
                if (mm.Defaultpackagecode == 6) { defaultPackage = "amazing.png"; }
                if (mm.Defaultpackagecode == 7) { defaultPackage = "octa-core.png"; }
                }

            string MemTree = "<ul><li><a uib-popover-html='<b>HTML</b>, <i>inline</i>'>" + "<img src='../Content/" + defaultPackage + "' class='imgclass'/><br/>" + Member + "</a></li></ul>"; //the actual html string

            var res = db.Members.Where(x => x.Username == Member).Single();
            long MemberId = res.RegistrationId;
            int? currentMemberLevel = res.Level;
            int maximumlevel = (int) currentMemberLevel + levelsToSshow - 1;

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
                    if (mem.Defaultpackagecode == 6) { defaultPackage = "amazing.png"; }
                    if (mem.Defaultpackagecode == 7) { defaultPackage = "octa-core.png"; }
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
                            if (mem.Defaultpackagecode == 6) { defaultPackage = "amazing.png"; }
                            if (mem.Defaultpackagecode == 7) { defaultPackage = "octa-core.png"; }
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

        public JsonResult GetUnilevelTeamByTree(string Member)   //Get Generation Tree of a name   ///called from GenerationStructureController.js
            {
            if (Member == "")
                {
                Member = User.Identity.Name;
                }

            long regId = 10000000;
            List<Member> members = new List<Member>();
            List<Treemodel> MemberList = new List<Treemodel>();

            var res = db.Members.SingleOrDefault(x => x.Username == Member);

            if (res != null)
                {
                #region found

                long MemberId = res.RegistrationId;
                int pkgcode = 0;
                long regid = 0;

                //Add the first member to array first

                Treemodel Mem0 = new Treemodel();
                Mem0.id = res.RegistrationId;
                Mem0.UplineRegistrationId = res.RegistrationId;
                Mem0.parentId = null;
                Mem0.Name = "";
                Mem0.pic = PackageImage((int) res.Defaultpackagecode);
                Mem0.isMember = true;
                Mem0.fullname = res.Firstname;
                Mem0.username = res.Username;
                Mem0.sponsorname = res.Referrerusername;
                Mem0.package = db.Packages.Where(p => p.Id == res.Defaultpackagecode).Select(p => p.Name).SingleOrDefault();
                Mem0.totalleft = 0;
                Mem0.totalright = 0;
                Mem0.businessleft = 0;
                Mem0.businessright = 0;
                MemberList.Add(Mem0);

                int param1 = 0;
                int param2 = 0;
                int param3 = 0;
                int param4 = 0;
                int param5 = 0;
                int param6 = 0;
                int param7 = 0;
                int param8 = 0;
                int param9 = 0;
                int param10 = 0;
                int param11 = 0;
                int param12 = 0;


                for (int lvl = 1; lvl <= 1; lvl++)
                    {
                    if (lvl == 1) { param1 = 0; param2 = 0; }

                    if (res.Defaultpackagecode == 1 || res.Defaultpackagecode == 2 || res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 1

                        for (int i = param1; i <= param2; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param3 = 1;
                                    param4 = m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[i].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 2 || res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 2
                        param5 = param4 + 1;
                        param6 = param4;
                        for (int i = param3; i <= param4; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param6 = param6 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[i].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 3
                        param7 = param6 + 1;
                        param8 = param6;
                        for (int j = param5; j <= param6; j++)
                            {
                            try
                                {
                                MemberId = MemberList[j].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param8 = param8 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[j].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 4
                        param9 = param8 + 1;
                        param10 = param8;

                        for (int i = param7; i <= param8; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {

                                    param10 = param10 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[i].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 5
                        param11 = param10 + 11;
                        param12 = param10;

                        for (int i = param9; i <= param10; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param12 = param12 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[i].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 7)
                        {
                        #region Level 6

                        for (int i = param11; i <= param12; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].UplineRegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        Treemodel Mem1 = new Treemodel();
                                        regId = regId + 1;
                                        Mem1.id = regId;
                                        Mem1.UplineRegistrationId = m[w].RegistrationId;
                                        Mem1.parentId = MemberList[i].id;
                                        Mem1.Name = "";
                                        Mem1.isMember = true;
                                        Mem1.pic = PackageImage((int) m[w].Defaultpackagecode);
                                        Mem1.position = "L";
                                        Mem1.fullname = m[w].Firstname;
                                        Mem1.username = m[w].Username;
                                        Mem1.sponsorname = m[w].Referrerusername;
                                        pkgcode = (int) m[w].Defaultpackagecode;
                                        Mem1.package = db.Packages.Where(p => p.Id == pkgcode).Select(p => p.Name).SingleOrDefault();
                                        Mem1.totalleft = 0;
                                        Mem1.totalright = 0;
                                        regid = m[w].RegistrationId;
                                        Mem1.businessleft = 0;
                                        Mem1.businessright = 0;
                                        MemberList.Add(Mem1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    }


                #endregion
                }
            return Json(new { Members = MemberList }, JsonRequestBehavior.AllowGet);
            }


        public JsonResult IsUserMember()
            {
            bool isExist = true;
            bool trnPwdExists = false;
            string UserName = User.Identity.Name;
            long Id = 0;
            Member mem = db.Members.SingleOrDefault(m => m.Username.ToUpper().Trim() == UserName.ToUpper().Trim());
            if (mem == null)
                {
                isExist = false;
                }
            else
                {
                isExist = true;
                string trnpwd = db.Registrations.Where(r => r.UserName == UserName).Select(r => r.TrxPassword).FirstOrDefault();
                trnPwdExists = ( trnpwd == null ? false : true );
                }
            return Json(new { Found = isExist, TrnPasswordExists = trnPwdExists }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult IsMemberACNOpresent()
            {
            bool isExist = true;
            string UserName = User.Identity.Name;
            Register mem = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
            if (mem.MyWalletAccount == "" || mem.MyWalletAccount == null)
                {
                isExist = false;
                }
            else
                {
                isExist = true;
                }
            return Json(new { Found = isExist }, JsonRequestBehavior.AllowGet);
            }

        [AllowAnonymous]
        public JsonResult IsUserNameExist1(string UserName)
            {
            bool isExist = true;
            var mem = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim() && !m.isBlocked);
            string name = "";
            string email = "";
            string countrycode = "";
            if (mem == null)
                {
                isExist = false;
                }
            else
                {
                name = mem.FullName;
                email = mem.EmailId;
                countrycode = mem.CountryCode;
                }
            return Json(new { Found = isExist, Name = name, Email = email, Countrycode = countrycode }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult IsUserNameExists2(string UserName)
            {
            bool isExist = true;
            var mem = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
            Boolean isBlocked = false;
            if (mem == null)
                {
                isExist = false;
                }
            else
                {
                isExist = true;
                isBlocked = mem.isBlocked;
                }
            return Json(new { Found = isExist, BlockStatus = isBlocked }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult IsUserNameExists2Update(string UserName, Boolean BlockStatus)
            {
            bool isExist = true;
            var mem = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper().Trim() == UserName.ToUpper().Trim());
            Boolean isBlocked = false;
            if (mem == null)
                {
                isExist = false;
                }
            else
                {
                isExist = true;
                mem.isBlocked = BlockStatus;
                db.SaveChanges();
                isBlocked = BlockStatus;
                }
            return Json(new { Found = isExist, BlockStatus = isBlocked }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public JsonResult UpdateUserID(string oldid, string newid)
            {
            if (User.Identity.Name.ToUpper() == "SUPERADMIN" || User.Identity.Name.ToUpper() == "ADMINISTRATOR")
                {
                var reg = db.Registrations.SingleOrDefault(r => r.UserName == oldid);
                reg.UserName = newid;
                db.SaveChanges();

                var reg1 = db.Registrations.Where(r => r.ReferrerName == oldid);
                foreach (Register line in reg1)
                    {
                    line.ReferrerName = newid;
                    }
                db.SaveChanges();

                var mem1 = db.Members.SingleOrDefault(m => m.Username == oldid);
                mem1.Username = newid;
                db.SaveChanges();

                var mem2 = db.Members.Where(m => m.Referrerusername == oldid);
                foreach (Member mem in mem2)
                    {
                    mem.Referrerusername = newid;
                    }
                db.SaveChanges();

                var mem3 = db.Members.Where(m => m.Uplineusername == oldid);
                foreach (Member mem in mem3)
                    {
                    mem.Uplineusername = newid;
                    }
                db.SaveChanges();

                var aspnetuser = dbView.AspNetUsers.SingleOrDefault(a => a.UserName == oldid);
                aspnetuser.UserName = newid;
                dbView.SubmitChanges();

                }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        [AllowAnonymous]
        [HttpGet]
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
            double balance = 0;
            try
                {
                string username = User.Identity.Name;
                var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
                double sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == WalletId).Select(i => i.Deposit).DefaultIfEmpty(0).Sum();
                double sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == WalletId).Select(i => i.Withdraw).DefaultIfEmpty(0).Sum(); ;
                balance = sumdeposit - sumwithdrawal;
                }
            catch (Exception e)
                {
                Console.WriteLine("Exception caught: {0}", e);
                }

            return Json(new { Balance = balance }, JsonRequestBehavior.AllowGet);
            }

        private double MemberWalletBalance(string username, int WalletId)
            {
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
            double sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == WalletId).Select(i => i.Deposit).DefaultIfEmpty(0).Sum();
            double sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == WalletId).Select(i => i.Withdraw).DefaultIfEmpty(0).Sum(); ;
            double balance = sumdeposit - sumwithdrawal;

            return balance;
            }

        [HttpGet]
        public JsonResult UserWalletBalance(string username)
            {
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            double cash_sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 1).Select(bi => bi.Deposit).DefaultIfEmpty(0).Sum();
            double cash_sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 1).Select(bi => bi.Withdraw).DefaultIfEmpty(0).Sum();
            double cash_balance = cash_sumdeposit - cash_sumwithdrawal;

            double reserve_sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 2).Select(bi => bi.Deposit).DefaultIfEmpty(0).Sum();
            double reserve_sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 2).Select(bi => bi.Withdraw).DefaultIfEmpty(0).Sum();
            double reserve_balance = reserve_sumdeposit - reserve_sumwithdrawal;

            double invreturn_sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 3).Select(bi => bi.Deposit).DefaultIfEmpty(0).Sum();
            double invreturn_sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 3).Select(bi => bi.Withdraw).DefaultIfEmpty(0).Sum();
            double invreturn_balance = invreturn_sumdeposit - invreturn_sumwithdrawal;

            double frozen_sumdeposit = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 4).Select(bi => bi.Deposit).DefaultIfEmpty(0).Sum();
            double frozen_sumwithdrawal = db.Ledgers.Where(i => i.RegistrationId == rec.Id && i.WalletId == 4).Select(bi => bi.Withdraw).DefaultIfEmpty(0).Sum();
            double frozen_balance = frozen_sumdeposit - frozen_sumwithdrawal;

            return Json(new { Cash_Balance = cash_balance, Reserve_Balance = reserve_balance, Invreturn_Balance = invreturn_balance, Frozen_Balance = frozen_balance }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        //called from PackageShop when user is self purchasing 
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
            //------------------------------------------old code when package upgradation was not allowed------------------//
            //if (!rec.Joined) { pur.Packageid = packageId; } else { pur.Packageid = 5; packageId = 5; } //0==repurchase

            //-----------------------------------------new code when package upgradation was opened-------------------------//
            pur.Packageid = packageId;

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
                    var sponsorrec = db.Registrations.SingleOrDefault(s => s.UserName == rec.ReferrerName);
                    mem.Uplineusername = GetExtremeLeftRight(rec.ReferrerName, sponsorrec.WorkingLeg);
                    var temprec = db.Registrations.SingleOrDefault(b => b.UserName.ToUpper().Trim() == mem.Uplineusername.ToUpper().Trim());
                    mem.UplineRegistrationId = temprec.Id;
                    uplineId = (long) mem.UplineRegistrationId;
                    mem.Level = GetnewLevel(uplineId);
                    mem.BinaryPosition = sponsorrec.WorkingLeg;
                    mem.Defaultpackagecode = packageId;
                    mem.Totalmembers = 0;
                    mem.Rightmembers = 0;
                    mem.Leftmembers = 0;
                    mem.Country = temprec.CountryCode;
                    mem.Achievement1Date = DateTime.Now;
                    db.Members.Add(mem);

                    //club determination of new member
                    if (packageId == 6 || packageId == 7)
                        {
                        mem.Achievement1 = 1; //is a default diamond club achiever
                        mem.Achievement1Date = DateTime.Now;
                        }

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
                        if (searchpos == "L") { res1.Leftmembers = ( res1.Leftmembers == null ) ? 1 : res1.Leftmembers + 1; }
                        if (searchpos == "R") { res1.Rightmembers = ( res1.Rightmembers == null ) ? 1 : res1.Rightmembers + 1; }
                        res1.Totalmembers = ( res1.Totalmembers == null ) ? 1 : res1.Totalmembers + 1;
                        searchpos = res1.BinaryPosition;
                        //club determination of old member
                        if (res1.Achievement1 == 0) //not a club member yet
                            {
                            Boolean club = IsClubMember(res1.RegistrationId, searchpos, investmentAmt);
                            if (club)
                                {
                                res1.Achievement1 = 1; //is a default diamond club achiever
                                res1.Achievement1Date = DateTime.Now;
                                }
                            }
                        db.SaveChanges();
                        uplineId = (long) res1.UplineRegistrationId;
                        }
                    else
                        {
                        keepgoing = false;
                        }

                    } while (keepgoing);
                }

            //6...

            //if repurchase, then % will be default package % (condition changed on 15th Aug 2017)
            double pc = 0;
            try
                {
                var pkg = db.Packages.SingleOrDefault(p => p.Id == packageId);
                if (pkg.Id == 5)
                    {
                    //#region added on 15/08/2017, default package code to change as per accumulated investment, check on every repurchase
                    ////----------------what is the total investment so far including today?
                    //double totalinvestment = db.Purchases.Where(p => p.RegistrationId == rec.Id).Select(p => p.Amount).DefaultIfEmpty(0).Sum();
                    ////----------------what should be the package code for this amount of investment?-------// 
                    //var ranges = db.Packages.Where(p => p.Minamount <= totalinvestment && p.Maxamount >= totalinvestment && p.Isactive == true && p.Id!=5).OrderBy(p => p.Id).FirstOrDefault();
                    //if (ranges != null)
                    //    {
                    //    int eligibleDefaultpackageCode = ranges.Id;
                    //    //----------------update the defaultpackage code and also keep track in PackageCodeHistory field-----//
                    //    var mmm0 = db.Members.Where(x => x.Username == rec.UserName).Single();
                    //    if (mmm0.PackageCodeHistory == null || mmm0.PackageCodeHistory == "")
                    //        {
                    //        mmm0.PackageCodeHistory = "From " + mmm0.Defaultpackagecode.ToString() + " on " + DateTime.Now.ToShortDateString();
                    //        }
                    //    else
                    //        {
                    //        mmm0.PackageCodeHistory = mmm0.PackageCodeHistory + "| From " + mmm0.Defaultpackagecode.ToString() + " on " + DateTime.Now.ToShortDateString();

                    //        }
                    //    mmm0.Defaultpackagecode = eligibleDefaultpackageCode;

                    //    db.SaveChanges();
                    //    }
                    ////----------------------------------------------------------------------//
                    //#endregion

                    var mmm = db.Members.Where(x => x.Username == rec.UserName).Single();
                    var pkgg = db.Packages.SingleOrDefault(p => p.Id == mmm.Defaultpackagecode);
                    pc = pkgg.Gauranteedreturn_percent;
                    }
                else
                    {
                    var mmm = db.Members.Where(x => x.Username == rec.UserName).Single();
                    mmm.Defaultpackagecode = packageId;
                    db.SaveChanges();
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
            long uplineId1 = (long) res.UplineRegistrationId;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId1);
                if (res1 != null)
                    {
                    await BinaryIncomeCalculation(uplineId1, res1.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, GuidString);
                    uplineId1 = (long) res1.UplineRegistrationId;
                    searchpos1 = res1.BinaryPosition;
                    }
                else
                    {
                    keepgoing1 = false;
                    }

                } while (keepgoing1);

            //8...
            var res2 = db.Members.SingleOrDefault(m => m.RegistrationId == res.ReferrerRegistrationId);
            await SponsorIncomeCalculation(res.ReferrerRegistrationId, res2.Doj, investmentAmt, res2.BinaryPosition, isJoined, rec.Id, packageId, GuidString);

            //9..
            string searchname2 = rec.UserName;
            string searchpos2 = rec.BinaryPosition;
            var res3 = db.Members.Where(x3 => x3.Username == searchname2).Single();
            long MemberId2 = res.RegistrationId;
            Boolean keepgoing2 = true;
            long uplineId2 = (long) res3.ReferrerRegistrationId;
            int level = 1;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId2);
                if (res1 != null)
                    {
                    await GenerationIncomeCalculation(uplineId2, res1.Doj, investmentAmt, res1.BinaryPosition, isJoined, rec.Id, packageId, (int) res1.Defaultpackagecode, level, GuidString);
                    uplineId2 = (long) res2.ReferrerRegistrationId;
                    if (level == 4) { keepgoing2 = false; }
                    level = level + 1;
                    }
                else
                    {
                    keepgoing2 = false;
                    }

                } while (keepgoing2);

            //10..
            //SendCandidateEmail(rec.EmailId);


            return Json(new { Success = "TRUE" }, JsonRequestBehavior.AllowGet);
            }

        public async Task<JsonResult> AutoPurchase(string username, long UplineId, int packageId, double investmentAmt)
        //called from tree Binarytree when sponsor is direct joining //called in case of FREE MEMBER JOINING also
            {
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

                    if (UplineId == 0)
                        {
                        var SponsorRec = db.Registrations.SingleOrDefault(s => s.UserName == rec.ReferrerName);
                        mem.Uplineusername = GetExtremeLeftRight(rec.ReferrerName, SponsorRec.WorkingLeg);
                        var temprec = db.Registrations.SingleOrDefault(b => b.UserName == mem.Uplineusername);
                        mem.UplineRegistrationId = temprec.Id;
                        mem.Country = temprec.CountryCode;
                        mem.BinaryPosition = SponsorRec.WorkingLeg;
                        }
                    if (UplineId > 0)
                        {
                        var temprec = db.Registrations.SingleOrDefault(b => b.Id == UplineId);
                        mem.UplineRegistrationId = temprec.Id;
                        mem.Uplineusername = temprec.UserName;
                        mem.Country = temprec.CountryCode;
                        mem.BinaryPosition = rec.BinaryPosition;
                        }

                    uplineId = (long) mem.UplineRegistrationId;
                    mem.Level = GetnewLevel(uplineId);

                    mem.Defaultpackagecode = packageId;
                    mem.Totalmembers = 0;
                    mem.Rightmembers = 0;
                    mem.Leftmembers = 0;

                    //club determination of new member
                    if (packageId == 6 || packageId == 7)
                        {
                        mem.Achievement1 = 1; //is a default diamond club achiever
                        mem.Achievement1Date = DateTime.Now;
                        }
                    mem.Achievement1Date = DateTime.Now;
                    db.Members.Add(mem);
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
                        if (searchpos == "L") { res1.Leftmembers = ( res1.Leftmembers == null ) ? 1 : res1.Leftmembers + 1; }
                        if (searchpos == "R") { res1.Rightmembers = ( res1.Rightmembers == null ) ? 1 : res1.Rightmembers + 1; }
                        res1.Totalmembers = ( res1.Totalmembers == null ) ? 1 : res1.Totalmembers + 1;
                        searchpos = res1.BinaryPosition;
                        //club determination of old member
                        if (res1.Achievement1 == 0) //not a club member yet
                            {
                            Boolean club = IsClubMember(res1.RegistrationId, searchpos, investmentAmt);
                            if (club)
                                {
                                res1.Achievement1 = 1; //is a default diamond club achiever
                                res1.Achievement1Date = DateTime.Now;
                                }
                            }
                        db.SaveChanges();
                        uplineId = (long) res1.UplineRegistrationId;
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

            if (User.Identity.Name.ToUpper() != "SUPERADMIN")      //i.e. not a FREE JOINING
                {
                await FixedIncomeCalculation(rec.Id, pc, investmentAmt, packageId, GuidString);
                }

            //7...
            string searchname1 = rec.UserName;
            string searchpos1 = rec.BinaryPosition;
            var res = db.Members.Where(x2 => x2.Username == searchname1).Single();
            long MemberId1 = res.RegistrationId;
            Boolean keepgoing1 = true;
            long uplineId1 = (long) res.UplineRegistrationId;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId1);
                if (res1 != null)
                    {
                    await BinaryIncomeCalculation(uplineId1, res1.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, GuidString);
                    uplineId1 = (long) res1.UplineRegistrationId;
                    searchpos1 = res1.BinaryPosition;
                    }
                else
                    {
                    keepgoing1 = false;
                    }

                } while (keepgoing1);

            //8...
            var res2 = db.Members.SingleOrDefault(m => m.RegistrationId == res.ReferrerRegistrationId);
            await SponsorIncomeCalculation(res.ReferrerRegistrationId, res2.Doj, investmentAmt, res2.BinaryPosition, isJoined, rec.Id, packageId, GuidString);

            //9..
            string searchname2 = rec.UserName;
            string searchpos2 = rec.BinaryPosition;
            var res3 = db.Members.Where(x3 => x3.Username == searchname2).Single();
            long MemberId2 = res.RegistrationId;
            Boolean keepgoing2 = true;
            long uplineId2 = (long) res3.ReferrerRegistrationId;
            int level = 1;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId2);
                if (res1 != null)
                    {
                    if (User.Identity.Name.ToUpper() != "SUPERADMIN")      //i.e. not a FREE JOINING
                        {
                        await GenerationIncomeCalculation(uplineId2, res1.Doj, investmentAmt, res1.BinaryPosition, isJoined, rec.Id, packageId, (int) res1.Defaultpackagecode, level, GuidString);
                        }
                    uplineId2 = (long) res2.ReferrerRegistrationId;
                    if (level == 4) { keepgoing2 = false; }
                    level = level + 1;
                    }
                else
                    {
                    keepgoing2 = false;
                    }

                } while (keepgoing2);

            //10..
            //SendCandidateEmail(rec.EmailId);


            return Json(new { Success = "TRUE" }, JsonRequestBehavior.AllowGet);
            }

        private async Task<Boolean> autoPurchase(string username, long UplineId, int packageId, double investmentAmt)
            {
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

                    if (UplineId == 0)
                        {
                        var SponsorRec = db.Registrations.SingleOrDefault(s => s.UserName == rec.ReferrerName);
                        mem.Uplineusername = GetExtremeLeftRight(rec.ReferrerName, SponsorRec.WorkingLeg);
                        var temprec = db.Registrations.SingleOrDefault(b => b.UserName == mem.Uplineusername);
                        mem.UplineRegistrationId = temprec.Id;
                        mem.Country = temprec.CountryCode;
                        mem.BinaryPosition = SponsorRec.WorkingLeg;
                        }
                    if (UplineId > 0)
                        {
                        var temprec = db.Registrations.SingleOrDefault(b => b.Id == UplineId);
                        mem.UplineRegistrationId = temprec.Id;
                        mem.Uplineusername = temprec.UserName;
                        mem.Country = temprec.CountryCode;
                        mem.BinaryPosition = rec.BinaryPosition;
                        }

                    uplineId = (long) mem.UplineRegistrationId;
                    mem.Level = GetnewLevel(uplineId);
                    mem.Defaultpackagecode = packageId;
                    mem.Totalmembers = 0;
                    mem.Rightmembers = 0;
                    mem.Leftmembers = 0;

                    //club determination of new member
                    if (packageId == 6 || packageId == 7)
                        {
                        mem.Achievement1 = 1; //is a default diamond club achiever
                        mem.Achievement1Date = DateTime.Now;
                        }
                    mem.Achievement1Date = DateTime.Now;
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
                        if (searchpos == "L") { res1.Leftmembers = ( res1.Leftmembers == null ) ? 1 : res1.Leftmembers + 1; }
                        if (searchpos == "R") { res1.Rightmembers = ( res1.Rightmembers == null ) ? 1 : res1.Rightmembers + 1; }
                        res1.Totalmembers = ( res1.Totalmembers == null ) ? 1 : res1.Totalmembers + 1;
                        searchpos = res1.BinaryPosition;

                        //club determination of old member
                        if (res1.Achievement1 == 0) //not a club member yet
                            {
                            Boolean club = IsClubMember(res1.RegistrationId, searchpos, investmentAmt);
                            if (club)
                                {
                                res1.Achievement1 = 1; //is a default diamond club achiever
                                res1.Achievement1Date = DateTime.Now;
                                }
                            }

                        db.SaveChanges();
                        uplineId = (long) res1.UplineRegistrationId;
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

            if (User.Identity.Name.ToUpper() != "SUPERADMIN")      //i.e. not a FREE JOINING
                {
                await FixedIncomeCalculation(rec.Id, pc, investmentAmt, packageId, GuidString);
                }

            //7...
            string searchname1 = rec.UserName;
            string searchpos1 = rec.BinaryPosition;
            var res = db.Members.Where(x2 => x2.Username == searchname1).Single();
            long MemberId1 = res.RegistrationId;
            Boolean keepgoing1 = true;
            long uplineId1 = (long) res.UplineRegistrationId;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId1);
                if (res1 != null)
                    {
                    await BinaryIncomeCalculation(uplineId1, res1.Doj, investmentAmt, searchpos1, isJoined, rec.Id, packageId, GuidString);
                    uplineId1 = (long) res1.UplineRegistrationId;
                    searchpos1 = res1.BinaryPosition;
                    }
                else
                    {
                    keepgoing1 = false;
                    }

                } while (keepgoing1);

            //8...
            var res2 = db.Members.SingleOrDefault(m => m.RegistrationId == res.ReferrerRegistrationId);
            await SponsorIncomeCalculation(res.ReferrerRegistrationId, res2.Doj, investmentAmt, res2.BinaryPosition, isJoined, rec.Id, packageId, GuidString);

            //9..
            string searchname2 = rec.UserName;
            string searchpos2 = rec.BinaryPosition;
            var res3 = db.Members.Where(x3 => x3.Username == searchname2).Single();
            long MemberId2 = res.RegistrationId;
            Boolean keepgoing2 = true;
            long uplineId2 = (long) res3.ReferrerRegistrationId;
            int level = 1;
            do
                {
                var res1 = db.Members.SingleOrDefault(m => m.RegistrationId == uplineId2);
                if (res1 != null)
                    {
                    if (User.Identity.Name.ToUpper() != "SUPERADMIN")      //i.e. not a FREE JOINING
                        {
                        await GenerationIncomeCalculation(uplineId2, res1.Doj, investmentAmt, res1.BinaryPosition, isJoined, rec.Id, packageId, (int) res1.Defaultpackagecode, level, GuidString);
                        }
                    uplineId2 = (long) res2.ReferrerRegistrationId;
                    if (level == 4) { keepgoing2 = false; }
                    level = level + 1;
                    }
                else
                    {
                    keepgoing2 = false;
                    }

                } while (keepgoing2);

            //10..
            //SendCandidateEmail(rec.EmailId);


            return true;
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

            var purchases = ( from p in db.Purchases
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
                                  MaxPay = s.Maxamount,
                                  PackageId=p.Packageid
                                  } ).ToList();
            return Json(new { Purchases = purchases }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MyRePurchases()
            {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            var purchases = ( from p in db.Purchases
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
                                  } ).ToList();
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
                var res1 = db.Members.SingleOrDefault(m => m.UplineRegistrationId == (long?) MemberId && m.BinaryPosition == position);
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
            int MemberLevel = (int) res.Level + 1;

            return MemberLevel;
            }

        public async Task<bool> FixedIncomeCalculation(long RegistrationId, double Pc, double Investment, int PackageId, string GuidString)
            {
            //double bonusamt = 0;
            //##################### Bonus 10th July - 13th August 2017 #####################//
            //added on 10th July
            //if (PackageId == 3 || PackageId == 5)
            //    {
            //    bonusamt = ( Investment * 5 ) / 100;
            //    Investment = Investment + bonusamt;
            //    }
            //##############################################################################//

            if (PackageId == 1 || PackageId == 2 || PackageId == 3 || PackageId==5)
                {
                #region Weekly payout
                DateTime duedt = DateTime.Now.AddDays(1);
                List<WeeklyIncome> weeklys = new List<WeeklyIncome>();
                for (int i = 1; i <= 52; i++)
                    {
                    WeeklyIncome wkin = new WeeklyIncome();
                    wkin.RegistrationId = RegistrationId;
                    wkin.WeekNo = i;
                    wkin.Days = 7;
                    wkin.Pc = Pc;

                    wkin.Income = ( Pc * Investment ) / 100;
                    wkin.PackageId = PackageId;
                    WeekModel wkModel = GetCurrentWeek(DateTime.Now.AddDays(1), duedt);
                    wkin.WeekStartDate = wkModel.WeekStartDate;
                    wkin.WeekEndDate = wkModel.WeekEndDate;
                    wkin.DueDate = duedt.AddDays(7);
                    duedt = wkin.DueDate;
                    wkin.CashWallet = 0;
                    wkin.ReserveWallet = 0;
                    wkin.FixedIncomeWallet = ( Pc * Investment ) / 100;
                    wkin.FrozenWallet = 0;
                    wkin.BatchNo = GuidString;

                    weeklys.Add(wkin);
                    }
                db.WeeklyIncomes.AddRange(weeklys);
                await db.SaveChangesAsync();
                #endregion
                }

            if (PackageId == 6 || PackageId == 7)
                {
                #region Monthly payout
                DateTime duedt = DateTime.Now.AddDays(1);
                List<WeeklyIncome> monthly = new List<WeeklyIncome>();
                for (int i = 1; i <= 36; i++)
                    {
                    WeeklyIncome mnthin = new WeeklyIncome();
                    mnthin.RegistrationId = RegistrationId;
                    mnthin.WeekNo = i;
                    mnthin.Days = 30;
                    mnthin.Pc = Pc;

                    mnthin.Income = ( Pc * Investment ) / 100;
                    mnthin.PackageId = PackageId;
                    WeekModel wkModel = GetCurrentMonth(DateTime.Now.AddDays(1), duedt);
                    mnthin.WeekStartDate = wkModel.WeekStartDate;
                    mnthin.WeekEndDate = wkModel.WeekEndDate;
                    mnthin.DueDate = duedt.AddDays(30);
                    duedt = mnthin.DueDate;
                    mnthin.CashWallet = 0;
                    mnthin.ReserveWallet = 0;
                    mnthin.FixedIncomeWallet = ( Pc * Investment ) / 100;
                    mnthin.FrozenWallet = 0;
                    mnthin.BatchNo = GuidString;

                    monthly.Add(mnthin);
                    }
                db.WeeklyIncomes.AddRange(monthly);
                await db.SaveChangesAsync();
                #endregion
                }


            return true;
            }

        //called during Joining
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

            double BinaryIncome = ( ( newBusiness.LeftNewBusinessAmount + newBusiness.RightNewBusinessAmount ) * 10 ) / 100;

            newBusiness.CashWallet = ( BinaryIncome * 70 ) / 100;
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

            newBusiness.IncomeAmount = ( Amount * 10 ) / 100;

            newBusiness.CashWallet = ( newBusiness.IncomeAmount * 70 ) / 100;
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

                    newBusiness.IncomeAmount = ( Amount * interest ) / 100;

                    newBusiness.CashWallet = ( newBusiness.IncomeAmount * 70 ) / 100;
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

                    newBusiness.IncomeAmount = ( Amount * interest ) / 100;

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

                    newBusiness.IncomeAmount = ( Amount * interest ) / 100;

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

                    newBusiness.IncomeAmount = ( Amount * interest ) / 100;

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

        public WeekModel GetCurrentMonth(DateTime JoiningDate, DateTime Today)
            {
            int monthno = 0;
            DateTime jd = JoiningDate.AddDays(-1);
            Boolean keepdoing = true;

            do
                {
                if (Today >= jd && Today <= jd.AddDays(30))
                    {
                    keepdoing = false;
                    }
                jd = jd.AddDays(30);
                monthno = monthno + 1;
                } while (keepdoing);

            WeekModel wkmodel = new WeekModel();
            wkmodel.WeekNo = monthno;
            wkmodel.WeekStartDate = jd.AddDays(-29);
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
            var transfers = ( from t in db.Ledgers
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
                                  } ).ToList();

            for (int i = 0; i < transfers.Count(); i++)
                {
                transfers[i].Date = transfers[i].DateD.ToLongDateString();
                }

            return Json(new { Transfers = transfers }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MemberWithdrawalHistory()
            {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());
            try
                {
                var transfers = ( from t in db.WithdrawalRequests
                                  from w in db.Wallets
                                  from r in db.Registrations
                                  where t.RegistrationId == rec.Id && t.WalletId == w.Id && t.RegistrationId == r.Id
                                  select new MemberWithdrawVM
                                      {
                                      Id = t.Id,
                                      Date = t.Date,
                                      WalletName = w.WalletName,
                                      BitCoinAccount = t.PaidBitCoinAccount,
                                      Amount = t.Amount,
                                      Payable = t.PaidOutAmount,
                                      AdministrativeChg = t.ServiceCharge,
                                      Approved_Date = t.Approved_Date,
                                      Status = t.Status,
                                      Comment = t.Comment
                                      } ).ToList();

                for (int i = 0; i < transfers.Count(); i++)
                    {
                    transfers[i].sDate = transfers[i].Date.ToLongDateString();
                    if (transfers[i].Approved_Date == null || transfers[i].Approved_Date == DateTime.Parse("01-01-0001"))
                        {
                        transfers[i].sApproved_Date = "";
                        }
                    else
                        {
                        transfers[i].sApproved_Date = ( (DateTime) transfers[i].Approved_Date ).ToLongDateString();
                        }
                    }

                return Json(new { Transfers = transfers }, JsonRequestBehavior.AllowGet);
                }
            catch (Exception e)
                {
                return Json(new { Transfers = "" }, JsonRequestBehavior.AllowGet);
                }

            }

        public JsonResult GetFixedIncomeIllustration(string Guid)
            {
            string username = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper().Trim() == username.ToUpper().Trim());

            var FIarray = ( from wk in db.WeeklyIncomes
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
                                Package = pkg.Name,
                                PaymentDate = wk.PaidDate
                                } ).ToList();

            for (int i = 0; i < FIarray.Count(); i++)
                {
                FIarray[i].sWeekStartDate = FIarray[i].WeekStartDate.ToLongDateString();
                FIarray[i].sWeekEndDate = FIarray[i].WeekEndDate.ToLongDateString();
                FIarray[i].sDueDate = FIarray[i].DueDate.ToLongDateString();
                if (FIarray[i].PaymentDate != null || FIarray[i].PaymentDate == DateTime.Parse("01-01-0001"))
                    {
                    FIarray[i].sPaymentDate = ( (DateTime) FIarray[i].PaymentDate ).ToLongDateString();
                    }
                }

            return Json(new { FixedIncomeArray = FIarray }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult GetMyCurrentFixedIncome()
            {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeek(rec.Doj, DateTime.Now);
            var FIarray = ( from wk in db.WeeklyIncomes
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
                                } ).ToList();

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

            var BIncome = ( from bi in db.BinaryIncomes
                            from pkg in db.Packages
                            from mem in db.Members
                            where bi.PackageId == pkg.Id && bi.RegistrationId == rec.RegistrationId
                            //&& bi.WeekStartDate <= DateTime.Now && bi.WeekEndDate >= DateTime.Now
                            && mem.RegistrationId == bi.PurchaserRegistrationId && bi.ProcessId == null
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
                                } ).ToList();

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
                if (leftamt0 != null) { leftamt = (double) leftamt0; }
                if (rightamt0 != null) { rightamt = (double) leftamt0; }
                }
            catch (Exception ex)
                {

                }

            double opLeftAmt = 0;
            double opRightAmt = 0;
            BinaryOpening opline = db.BinaryOpenings.Where(bi => bi.RegistrationId == rec.RegistrationId).OrderByDescending(bi => bi.Id).FirstOrDefault();

            if (opline != null)
                {
                //new code 
                //double totalbusinessleft = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum())-leftamt;
                //double totalbusinessright = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum())-rightamt;
                //if (totalbusinessleft< totalbusinessright) { opLeftAmt = 0; opRightAmt = totalbusinessright - totalbusinessleft; }
                //if (totalbusinessleft > totalbusinessright) { opLeftAmt = totalbusinessleft-totalbusinessright; opRightAmt=0; }
                //if (totalbusinessleft == totalbusinessright) { opLeftAmt = 0;opRightAmt = 0; }
                //
                opLeftAmt = opline.LeftSideCd;
                opRightAmt = opline.RightSideCd;

                }

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
            else if (rec.Defaultpackagecode == 2 && currentbusinessToConsider > 10000)
                {
                businesslimit = 10000;
                }
            if (rec.Defaultpackagecode == 3 && currentbusinessToConsider <= 30000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 3 && currentbusinessToConsider > 30000)
                {
                businesslimit = 30000;
                }

            if (rec.Defaultpackagecode == 4 && currentbusinessToConsider <= 50000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 4 && currentbusinessToConsider > 50000)
                {
                businesslimit = 50000;
                }

            if (rec.Defaultpackagecode == 6 && currentbusinessToConsider <= 200000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 6 && currentbusinessToConsider > 200000)
                {
                businesslimit = 200000;
                }

            if (rec.Defaultpackagecode == 7 && currentbusinessToConsider <= 300000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 7 && currentbusinessToConsider > 300000)
                {
                businesslimit = 300000;
                }

            double income = ( businesslimit * 10 ) / 100;

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

        public BinaryIncomeLedgerVMTotals GetCurrentBinaryIncome(string username)
            {
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);

            var BIncome = ( from bi in db.BinaryIncomes
                            from pkg in db.Packages
                            from mem in db.Members
                            where bi.PackageId == pkg.Id && bi.RegistrationId == rec.RegistrationId
                            //&& bi.WeekStartDate <= DateTime.Now && bi.WeekEndDate >= DateTime.Now
                            && mem.RegistrationId == bi.PurchaserRegistrationId && bi.ProcessId == null
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
                                } ).ToList();

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
                if (leftamt0 != null) { leftamt = (double) leftamt0; }
                if (rightamt0 != null) { rightamt = (double) leftamt0; }
                }
            catch (Exception ex)
                {

                }

            double opLeftAmt = 0;
            double opRightAmt = 0;
            BinaryOpening opline = db.BinaryOpenings.Where(bi => bi.RegistrationId == rec.RegistrationId).OrderByDescending(bi => bi.Id).FirstOrDefault();

            if (opline != null)
                {
                //new code 
                //double totalbusinessleft = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum()) - leftamt;
                //double totalbusinessright = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum()) - rightamt;
                //if (totalbusinessleft < totalbusinessright) { opLeftAmt = 0; opRightAmt = totalbusinessright - totalbusinessleft; }
                //if (totalbusinessleft > totalbusinessright) { opLeftAmt = totalbusinessleft - totalbusinessright; opRightAmt = 0; }
                //if (totalbusinessleft == totalbusinessright) { opLeftAmt = 0; opRightAmt = 0; }
                //
                opLeftAmt = opline.LeftSideCd;
                opRightAmt = opline.RightSideCd;
                //opLeftAmt = totalbusinessleft;
                //opRightAmt = totalbusinessright;
                }

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
            else if (rec.Defaultpackagecode == 2 && currentbusinessToConsider > 10000)
                {
                businesslimit = 10000;
                }
            if (rec.Defaultpackagecode == 3 && currentbusinessToConsider <= 30000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 3 && currentbusinessToConsider > 30000)
                {
                businesslimit = 30000;
                }

            if (rec.Defaultpackagecode == 4 && currentbusinessToConsider <= 50000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 4 && currentbusinessToConsider > 50000)
                {
                businesslimit = 50000;
                }
            if (rec.Defaultpackagecode == 6 && currentbusinessToConsider <= 200000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 6 && currentbusinessToConsider > 200000)
                {
                businesslimit = 200000;
                }

            if (rec.Defaultpackagecode == 7 && currentbusinessToConsider <= 300000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 7 && currentbusinessToConsider > 300000)
                {
                businesslimit = 300000;
                }
            double income = ( businesslimit * 10 ) / 100;

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

            return totalArr;
            }

        public JsonResult GetMyCurrentSponsorIncome()
            {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);
            try
                {
                var SpIncome = ( from si in db.SponsorIncomes
                                 from pkg in db.Packages
                                 from mem in db.Members
                                 where si.PackageId == pkg.Id && si.RegistrationId == rec.RegistrationId
                                 //&& si.TransactionDate > weekly.WeekStartDate && si.TransactionDate < weekly.WeekEndDate
                                 //&& si.WeekStartDate <= DateTime.Now && si.WeekEndDate >= DateTime.Now
                                 && mem.RegistrationId == si.PurchaserRegistrationId && si.ProcessId == null
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
                                     RightSideAmount = si.RightNewBusinessAmount,
                                     WalletAmount = si.IncomeAmount
                                     } ).ToList();
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

            catch (Exception e)
                {

                }

            return null;
            }

        private List<BinaryIncomeLedgerVM> GetMyCurrentGenerationIncomes(string member)
            {
            if (member == "")
                {
                member = User.Identity.Name;
                }
            var weekly = GetCurrentWeekBoundary(DateTime.Now);
            List<BinaryIncomeLedgerVM> MemberList = new List<BinaryIncomeLedgerVM>();


            var res = db.Members.SingleOrDefault(x => x.Username == member);

            if (res != null)
                {
                #region found

                long MemberId = res.RegistrationId;
                BinaryIncomeLedgerVM Gen0 = new BinaryIncomeLedgerVM();
                Gen0.RegistrationId = res.RegistrationId;
                Gen0.WeekNo = weekly.WeekNo;
                Gen0.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                Gen0.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                Gen0.Purchaser = res.Username;
                Gen0.Level = 1;
                Gen0.Total = 0;
                MemberList.Add(Gen0);

                int param1 = 0;
                int param2 = 0;
                int param3 = 0;
                int param4 = 0;
                int param5 = 0;
                int param6 = 0;
                int param7 = 0;
                int param8 = 0;
                int param9 = 0;
                int param10 = 0;
                int param11 = 0;
                int param12 = 0;

                for (int lvl = 1; lvl <= 1; lvl++)
                    {
                    if (lvl == 1) { param1 = 0; param2 = 0; }

                    if (res.Defaultpackagecode == 1 || res.Defaultpackagecode == 2 || res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 1

                        for (int i = param1; i <= param2; i++)
                            {
                            try
                                {
                                MemberId = res.RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param3 = 1;
                                    param4 = m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen1 = new BinaryIncomeLedgerVM();
                                        Gen1.RegistrationId = m[w].RegistrationId;
                                        Gen1.WeekNo = weekly.WeekNo;
                                        Gen1.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen1.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen1.Purchaser = m[w].Username;
                                        Gen1.Level = 1;
                                        Gen1.Total = GetBinaryIncome(Gen1.Purchaser);
                                        if (Gen1.Level == 1) { Gen1.WalletAmount = ( Gen1.Total * 5 ) / 100; }
                                        if (Gen1.Level == 2) { Gen1.WalletAmount = ( Gen1.Total * 7 ) / 100; }
                                        if (Gen1.Level == 3) { Gen1.WalletAmount = ( Gen1.Total * 9 ) / 100; }
                                        if (Gen1.Level == 4) { Gen1.WalletAmount = ( Gen1.Total * 10 ) / 100; }
                                        MemberList.Add(Gen1);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 2 || res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 2
                        param5 = param4 + 1;
                        param6 = param4;
                        for (int i = param3; i <= param4; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param6 = param6 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen2 = new BinaryIncomeLedgerVM();
                                        Gen2.RegistrationId = m[w].RegistrationId;
                                        Gen2.WeekNo = weekly.WeekNo;
                                        Gen2.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen2.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen2.Purchaser = m[w].Username;
                                        Gen2.Level = 2;
                                        Gen2.Total = GetBinaryIncome(Gen2.Purchaser);
                                        if (Gen2.Level == 1) { Gen2.WalletAmount = ( Gen2.Total * 5 ) / 100; }
                                        if (Gen2.Level == 2) { Gen2.WalletAmount = ( Gen2.Total * 7 ) / 100; }
                                        if (Gen2.Level == 3) { Gen2.WalletAmount = ( Gen2.Total * 9 ) / 100; }
                                        if (Gen2.Level == 4) { Gen2.WalletAmount = ( Gen2.Total * 10 ) / 100; }
                                        MemberList.Add(Gen2);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 3 || res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 3
                        param7 = param6 + 1;
                        param8 = param6;
                        for (int j = param5; j <= param6; j++)
                            {
                            try
                                {
                                MemberId = MemberList[j].RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param8 = param8 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen3 = new BinaryIncomeLedgerVM();
                                        Gen3.RegistrationId = m[w].RegistrationId;
                                        Gen3.WeekNo = weekly.WeekNo;
                                        Gen3.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen3.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen3.Purchaser = m[w].Username;
                                        Gen3.Level = 3;
                                        Gen3.Total = GetBinaryIncome(Gen3.Purchaser);
                                        if (Gen3.Level == 1) { Gen3.WalletAmount = ( Gen3.Total * 5 ) / 100; }
                                        if (Gen3.Level == 2) { Gen3.WalletAmount = ( Gen3.Total * 7 ) / 100; }
                                        if (Gen3.Level == 3) { Gen3.WalletAmount = ( Gen3.Total * 9 ) / 100; }
                                        if (Gen3.Level == 4) { Gen3.WalletAmount = ( Gen3.Total * 10 ) / 100; }
                                        MemberList.Add(Gen3);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 4 || res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 4
                        param9 = param8 + 1;
                        param10 = param8;
                        for (int i = param7; i <= param8; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param10 = param10 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen4 = new BinaryIncomeLedgerVM();
                                        Gen4.RegistrationId = m[w].RegistrationId;
                                        Gen4.WeekNo = weekly.WeekNo;
                                        Gen4.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen4.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen4.Purchaser = m[w].Username;
                                        Gen4.Level = 4;
                                        Gen4.Total = GetBinaryIncome(Gen4.Purchaser);
                                        if (Gen4.Level == 1) { Gen4.WalletAmount = ( Gen4.Total * 5 ) / 100; }
                                        if (Gen4.Level == 2) { Gen4.WalletAmount = ( Gen4.Total * 7 ) / 100; }
                                        if (Gen4.Level == 3) { Gen4.WalletAmount = ( Gen4.Total * 9 ) / 100; }
                                        if (Gen4.Level == 4) { Gen4.WalletAmount = ( Gen4.Total * 10 ) / 100; }
                                        MemberList.Add(Gen4);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 6 || res.Defaultpackagecode == 7)
                        {
                        #region Level 5
                        param11 = param10 + 1;
                        param12 = param10;
                        for (int i = param9; i <= param10; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {
                                    param12 = param12 + m.Count();

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen5 = new BinaryIncomeLedgerVM();
                                        Gen5.RegistrationId = m[w].RegistrationId;
                                        Gen5.WeekNo = weekly.WeekNo;
                                        Gen5.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen5.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen5.Purchaser = m[w].Username;
                                        Gen5.Level = 5;
                                        Gen5.Total = GetBinaryIncome(Gen5.Purchaser);
                                        if (Gen5.Level == 1) { Gen5.WalletAmount = ( Gen5.Total * 5 ) / 100; }
                                        if (Gen5.Level == 2) { Gen5.WalletAmount = ( Gen5.Total * 7 ) / 100; }
                                        if (Gen5.Level == 3) { Gen5.WalletAmount = ( Gen5.Total * 9 ) / 100; }
                                        if (Gen5.Level == 4) { Gen5.WalletAmount = ( Gen5.Total * 10 ) / 100; }
                                        if (Gen5.Level == 5) { Gen5.WalletAmount = ( Gen5.Total * 15 ) / 100; }

                                        MemberList.Add(Gen5);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    if (res.Defaultpackagecode == 7)
                        {
                        #region Level 6

                        for (int i = param11; i <= param12; i++)
                            {
                            try
                                {
                                MemberId = MemberList[i].RegistrationId;
                                var m = db.Members.Where(mm => mm.ReferrerRegistrationId == MemberId).OrderBy(mm => mm.BinaryPosition).ToList();

                                if (m.Count() > 0)
                                    {

                                    for (int w = 0; w < m.Count(); w++)
                                        {
                                        BinaryIncomeLedgerVM Gen6 = new BinaryIncomeLedgerVM();
                                        Gen6.RegistrationId = m[w].RegistrationId;
                                        Gen6.WeekNo = weekly.WeekNo;
                                        Gen6.sWeekStartDate = weekly.WeekStartDate.ToLongDateString();
                                        Gen6.sWeekEndDate = weekly.WeekEndDate.ToLongDateString();
                                        Gen6.Purchaser = m[w].Username;
                                        Gen6.Level = 6;
                                        Gen6.Total = GetBinaryIncome(Gen6.Purchaser);
                                        if (Gen6.Level == 1) { Gen6.WalletAmount = ( Gen6.Total * 5 ) / 100; }
                                        if (Gen6.Level == 2) { Gen6.WalletAmount = ( Gen6.Total * 7 ) / 100; }
                                        if (Gen6.Level == 3) { Gen6.WalletAmount = ( Gen6.Total * 9 ) / 100; }
                                        if (Gen6.Level == 4) { Gen6.WalletAmount = ( Gen6.Total * 10 ) / 100; }
                                        if (Gen6.Level == 5) { Gen6.WalletAmount = ( Gen6.Total * 15 ) / 100; }
                                        if (Gen6.Level == 6) { Gen6.WalletAmount = ( Gen6.Total * 20 ) / 100; }

                                        MemberList.Add(Gen6);
                                        }
                                    }

                                }
                            catch (Exception e)
                                {

                                }
                            }
                        #endregion
                        }
                    }
                #endregion
                }
            return MemberList;
            }

        public JsonResult GetMyCurrentGenerationIncome(string member)
            {
            var MemberList = GetMyCurrentGenerationIncomes(member);
            return Json(new { GnIincomeArray = MemberList }, JsonRequestBehavior.AllowGet);

            }

        private int GetGeneration(string user, string purchaser)
            {
            int ctr = 0;
            Boolean keepgoing = true;

            do
                {
                var mem = db.Members.SingleOrDefault(d => d.Username.ToUpper() == purchaser.ToUpper());
                ctr = ctr + 1;
                if (mem.Referrerusername.ToUpper() == user.ToUpper())
                    {
                    keepgoing = false;
                    }
                else
                    {
                    purchaser = mem.Referrerusername;
                    }
                } while (keepgoing);

            return ctr;
            }

        private double GetBinaryIncome(string username)
            {
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            var weekly = GetCurrentWeekBoundary(DateTime.Now);

            var BIncome = ( from bi in db.BinaryIncomes
                            from pkg in db.Packages
                            from mem in db.Members
                            where bi.PackageId == pkg.Id && bi.RegistrationId == rec.RegistrationId
                            //&& bi.WeekStartDate >= weekly.WeekStartDate && bi.WeekEndDate <= weekly.WeekEndDate
                            && mem.RegistrationId == bi.PurchaserRegistrationId && bi.ProcessId == null
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
                                } ).ToList();

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
                if (leftamt0 != null) { leftamt = (double) leftamt0; }
                if (rightamt0 != null) { rightamt = (double) leftamt0; }
                }
            catch (Exception ex)
                {

                }


            double opLeftAmt = 0;
            double opRightAmt = 0;
            BinaryOpening opline = db.BinaryOpenings.Where(bi => bi.RegistrationId == rec.RegistrationId).OrderByDescending(bi => bi.Id).FirstOrDefault();
            if (opline != null)
                {
                //new code 
                //double totalbusinessleft = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum()) - leftamt;
                //double totalbusinessright = (db.BinaryIncomes.Where(i => i.RegistrationId == rec.RegistrationId).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum()) - rightamt;
                //if (totalbusinessleft < totalbusinessright) { opLeftAmt = 0; opRightAmt = totalbusinessright - totalbusinessleft; }
                //if (totalbusinessleft > totalbusinessright) { opLeftAmt = totalbusinessleft - totalbusinessright; opRightAmt = 0; }
                //if (totalbusinessleft == totalbusinessright) { opLeftAmt = 0; opRightAmt = 0; }
                //
                opLeftAmt = opline.LeftSideCd;
                opRightAmt = opline.RightSideCd;

                }

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
            else if (rec.Defaultpackagecode == 2 && currentbusinessToConsider > 10000)
                {
                businesslimit = 10000;
                }
            if (rec.Defaultpackagecode == 3 && currentbusinessToConsider <= 30000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 3 && currentbusinessToConsider > 30000)
                {
                businesslimit = 30000;
                }

            if (rec.Defaultpackagecode == 4 && currentbusinessToConsider <= 50000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 4 && currentbusinessToConsider > 50000)
                {
                businesslimit = 50000;
                }

            if (rec.Defaultpackagecode == 6 && currentbusinessToConsider <= 200000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 6 && currentbusinessToConsider > 200000)
                {
                businesslimit = 200000;
                }

            if (rec.Defaultpackagecode == 7 && currentbusinessToConsider <= 300000)
                {
                businesslimit = currentbusinessToConsider;
                }
            else if (rec.Defaultpackagecode == 7 && currentbusinessToConsider > 300000)
                {
                businesslimit = 300000;
                }

            double income = ( businesslimit * 10 ) / 100;

            return income;
            }

        public WeekModel GetCurrentWeekBoundary(DateTime date)
            {
            WeekModel wk = new WeekModel();
            wk.WeekNo = 0;
            int dayOfToday = (int) date.DayOfWeek;
            if (dayOfToday == 0) { dayOfToday = 7; }
            wk.WeekStartDate = date.AddDays(-1 * ( dayOfToday - 1 ));
            wk.WeekEndDate = date.AddDays(7 - dayOfToday);
            wk.WeekStartDate = wk.WeekStartDate.AddDays(-1);
            wk.WeekEndDate = wk.WeekEndDate.AddDays(1);
            return wk;
            }

        public JsonResult GetCurrentWeekBoundaryTest(DateTime date)
            {
            WeekModel wk = new WeekModel();
            wk.WeekNo = 0;
            int dayOfToday = (int) date.DayOfWeek;
            if (dayOfToday == 0) { dayOfToday = 7; }
            wk.WeekStartDate = date.AddDays(-1 * ( dayOfToday - 1 ));
            wk.WeekEndDate = date.AddDays(7 - dayOfToday);
            return Json(new { WeekBoundary = wk }, JsonRequestBehavior.AllowGet);
            }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult Dashboarddata()
            {
            DashboardModel dashdata = new DashboardModel();

            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());

            dashdata.TotalTeamMembers = (long) rec.Totalmembers;

            double totalbusinessL = db.BinaryIncomes.Where(s => s.RegistrationId == rec.RegistrationId).Select(n => n.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
            double totalbusinessR = db.BinaryIncomes.Where(s => s.RegistrationId == rec.RegistrationId).Select(n => n.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
            double totalbusinessSum = totalbusinessL + totalbusinessR;
            double totalselfbusiness = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId).Select(n => n.Amount).DefaultIfEmpty(0).Sum();
            dashdata.TotalTeamBusiness = totalbusinessSum + totalselfbusiness;

            dashdata.InitialInvestment = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId && p.Packageid != 0).Select(n => n.Amount).DefaultIfEmpty(0).Sum();

            //dashdata.TotalRepurchase = db.Purchases.Where(p => p.RegistrationId == rec.RegistrationId && p.Packageid == 0).Select(n => n.Amount).DefaultIfEmpty(0).Sum();
            dashdata.TotalRepurchase = db.WithdrawalRequests.Where(w => w.RegistrationId == rec.RegistrationId && w.Approved_Date == null && w.Status == "Under Process").Select(w => w.Amount).DefaultIfEmpty(0).Sum();

            int plutocount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 1).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int jupitercount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 2).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int earthcount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 3).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int mercurycount = db.BinaryIncomes.Where(b => b.RegistrationId == rec.RegistrationId && b.PackageId == 4).Select(n => n.PackageId).DefaultIfEmpty(0).Count();
            int totalcount = plutocount + jupitercount + earthcount + mercurycount;

            dashdata.PlutoPurchasePc = ( ( plutocount * 100 ) / totalcount );
            dashdata.JupiterPurchasePc = ( ( jupitercount * 100 ) / totalcount );
            dashdata.EarthPurchasePc = ( ( earthcount * 100 ) / totalcount );
            dashdata.MercuryPurchasePc = ( ( mercurycount * 100 ) / totalcount );
            dashdata.TotalPkgPurchaseCount = totalcount;

            dashdata.MyLeftCount = (long) rec.Leftmembers;
            dashdata.MyRightCount = (long) rec.Rightmembers;

            double amt1 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 1) ).Select(b => b.Deposit).DefaultIfEmpty(0).Sum();
            double amt2 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 1) ).Select(b => b.Withdraw).DefaultIfEmpty(0).Sum();
            dashdata.CashWalletBalance = amt1 - amt2;

            double amt3 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 2) ).Select(b => b.Deposit).DefaultIfEmpty(0).Sum();
            double amt4 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 2) ).Select(b => b.Withdraw).DefaultIfEmpty(0).Sum();
            dashdata.ReserveWalletBalance = amt3 - amt4;

            double amt5 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 3) ).Select(b => b.Deposit).DefaultIfEmpty(0).Sum();
            double amt6 = (double) ( db.Ledgers.Where(b => b.RegistrationId == rec.RegistrationId && b.WalletId == 3) ).Select(b => b.Withdraw).DefaultIfEmpty(0).Sum();
            dashdata.ReturnWalletBalance = amt5 - amt6;

            dashdata.FrozenWalletBalance = 0;

            return Json(new { DashboardDataModel = dashdata }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MyFixedIncomeWallet()
            {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            WeekModel wm = GetCurrentWeekBoundary(DateTime.Now);

            var fixedwallet = ( from l in db.Ledgers
                                from t in db.TransactionTypes
                                from s in db.SubLedgers
                                where l.TransactionTypeId == t.Id &&
                                l.SubLedgerId == s.Id &&
                                l.RegistrationId == rec.RegistrationId &&
                                l.WalletId == 3
                                orderby l.Id
                                select new LedgerVM
                                    {
                                    Id = l.Id,
                                    Date = l.Date,
                                    Deposit = l.Deposit,
                                    Withdraw = l.Withdraw,
                                    Balance = 0,
                                    TransactionType = t.TransactionName,
                                    Ledger = s.SubLedgerName,
                                    Remarks = l.Comment
                                    } ).ToList();

            for (int x = 0; x < fixedwallet.Count(); x++)
                {
                fixedwallet[x].sDate = fixedwallet[x].Date.ToLongDateString();
                }
            return Json(new { FixedWallet = fixedwallet }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MyCashIncomeWallet()
            {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            WeekModel wm = GetCurrentWeekBoundary(DateTime.Now);

            var cashwallet = ( from l in db.Ledgers
                               from t in db.TransactionTypes
                               from s in db.SubLedgers
                               where l.TransactionTypeId == t.Id &&
                               l.SubLedgerId == s.Id &&
                               l.RegistrationId == rec.RegistrationId &&
                               l.WalletId == 1
                               orderby l.Id
                               select new LedgerVM
                                   {
                                   Id = l.Id,
                                   Date = l.Date,
                                   Deposit = l.Deposit,
                                   Withdraw = l.Withdraw,
                                   Balance = 0,
                                   TransactionType = t.TransactionName,
                                   Ledger = s.SubLedgerName,
                                   BatchNo = l.BatchNo,
                                   Remarks = l.Comment
                                   } ).ToList();

            for (int x = 0; x < cashwallet.Count(); x++)
                {
                cashwallet[x].sDate = cashwallet[x].Date.ToLongDateString();
                if (cashwallet[x].TransactionType == "BitCoin Withdrawal" && cashwallet[x].Ledger == "BitCoin Transaction")
                    {
                    string btchno = cashwallet[x].BatchNo;
                    //check the status of the transaction from Withdrawal request
                    var trnrec = db.WithdrawalRequests.SingleOrDefault(w => w.BatchNo == btchno);
                    if (trnrec.Approved_Date == null && trnrec.Status != "Cancelled by Member")
                        {
                        cashwallet[x].Remarks = "Under Process";
                        }
                    else if (trnrec.Approved_Date == null && trnrec.Status == "Cancelled by Member")
                        {
                        if (cashwallet[x].Deposit > 0)
                            {
                            cashwallet[x].Remarks = "Refunded on request";
                            }
                        if (cashwallet[x].Withdraw > 0)
                            {
                            cashwallet[x].Remarks = "Request Cancelled";
                            }
                        }
                    }
                }
            return Json(new { CashWallet = cashwallet }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult MyReserveIncomeWallet()
            {
            string username = User.Identity.Name;
            var rec = db.Members.SingleOrDefault(r => r.Username.ToUpper().Trim() == username.ToUpper().Trim());
            WeekModel wm = GetCurrentWeekBoundary(DateTime.Now);

            var reservewallet = ( from l in db.Ledgers
                                  from t in db.TransactionTypes
                                  from s in db.SubLedgers
                                  where l.TransactionTypeId == t.Id &&
                                  l.SubLedgerId == s.Id &&
                                  l.RegistrationId == rec.RegistrationId &&
                                  l.WalletId == 2
                                  orderby l.Id
                                  select new LedgerVM
                                      {
                                      Id = l.Id,
                                      Date = l.Date,
                                      Deposit = l.Deposit,
                                      Withdraw = l.Withdraw,
                                      Balance = 0,
                                      TransactionType = t.TransactionName,
                                      Ledger = s.SubLedgerName,
                                      Remarks = l.Comment
                                      } ).ToList();

            for (int x = 0; x < reservewallet.Count(); x++)
                {
                reservewallet[x].sDate = reservewallet[x].Date.ToLongDateString();
                }
            return Json(new { ReserveWallet = reservewallet }, JsonRequestBehavior.AllowGet);
            }

        public string PackageImage(int packageId)
            {
            string path = "";
            if (packageId == 1) { path = "https://btcpro.co/BtcLibrary/Assets/pluto.png"; }
            if (packageId == 2) { path = "https://btcpro.co/BtcLibrary/Assets/jupiter.png"; }
            if (packageId == 3) { path = "https://btcpro.co/BtcLibrary/Assets/earth.png"; }
            if (packageId == 4) { path = "https://btcpro.co/BtcLibrary/Assets/mercury.png"; }
            if (packageId == 6) { path = "https://btcpro.co/BtcLibrary/Assets/amazing.png"; }
            if (packageId == 7) { path = "https://btcpro.co/BtcLibrary/Assets/octa-core.png"; }

            if (packageId == 99) { path = "https://btcpro.co/BtcLibrary/Assets/plus-sign.png"; }
            return path;
            }

        public JsonResult MemberDetail(string Member)
            {
            string username = "";
            if (Member == "")
                {
                username = User.Identity.Name;
                }
            else
                {
                username = Member;
                }

            var mem = db.Members.SingleOrDefault(m => m.Username == username);

            return Json(new { Member = mem }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult ActiveSupportTickets()
            {
            //var tickets = db.Tickets.Where(t => t.isApproved == false);
            var tickets = ( from t in db.Tickets
                            from r in db.Registrations
                            where t.RegistrationId == r.Id && t.isApproved == false
                            select new
                                {
                                Id = t.Id,
                                DateString = t.DateString,
                                TicketCategory = t.TicketCategory,
                                UserId = r.UserName,
                                Subject = t.Subject,
                                Message = t.Message,
                                Status = t.Status
                                } );

            return Json(new { Tickets = tickets }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public JsonResult TicketFeedback(Ticket ticket)
            {
            Ticket objrec = db.Tickets.SingleOrDefault(t => t.Id == ticket.Id);
            objrec.Status = ticket.Status;
            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        [HttpPost]
        public JsonResult TicketClose(Ticket ticket)
            {
            Ticket objrec = db.Tickets.SingleOrDefault(t => t.Id == ticket.Id);
            objrec.Status = ticket.Status;
            objrec.Comment = "Approved on " + DateTime.Now.ToLongDateString();
            objrec.ApprovedWhen = DateTime.Now;
            objrec.isApproved = true;
            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        private async Task<bool> SendEMail(string mail_to, string mail_cc, string subj, string desc, ArrayList arr)
            {
            string mail_from = "noreply@btcpro.co";
            string mail_ccc = "info@btcpro.co";
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht.Add("FROM", mail_from);
            ht.Add("TO", mail_to);
            ht.Add("CC", mail_ccc);
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> SendContactUsEMail(string fullname, string phone, string country, string mailfrom, string mail_to, string mail_cc, string subj, string desc)
            {
            string bodytext = "<table><tr><td>Full name</td><td>" + fullname + "</td></tr><tr><td>Phone No.</td><td>" + phone + "</td></tr><tr><td>Email</td><td>" + mailfrom + "</td></tr><tr><td>Country</td><td>" + country + "</td></tr><tr><td>Message</td><td>" + desc + "</td></tr></table>";
            string mail_from = mailfrom;
            mail_to = "info@btcpro.co";
            ArrayList arr = null;
            //string mail_cc = "";
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht.Add("FROM", mail_from);
            ht.Add("TO", mail_to);
            ht.Add("CC", mail_cc);
            //ht.Add("BCC", mail_cc);
            ht.Add("SUBJECT", subj);
            ht.Add("BODY", bodytext);
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
        public async Task<JsonResult> SendMyRegistrationEmail(string Username)
            {
            var reg = db.Registrations.SingleOrDefault(r => r.UserName == Username);

            bool result = false;
            ArrayList array = new ArrayList();
            string body = "";
            body = @"<table style='width:40%;border:1px solid grey;padding:25px'>
                            <tr>
                             <td colspan='2'>
                               <p style='text-align:left;font-size:14px;font-color:black;font-weight:600'> Greetings from BTC PRO INC!!!</p></td>
                           </tr> 
                           <tr>
                              <td colspan='2'>
                                <p style='text-align:justified;font-size:12px;'> Thanks for your interest in www.BtcPro.co. We wish you a prosperous association with us. The following are your credentials to get you started !!! </p>
                              </td>
                           </tr>
                           <tr>
                             <td style='text-align:right;font-weight:600'>
                                Date of Joining:
                             </td>
                             <td>" + reg.CreatedDate.ToLongDateString() + "</td>" +
                           "</tr>" +
                           "<tr>" +
                             "<td style='text-align:right;font-weight:600'>Username:" +
                              "</td>" +
                                "<td>" + reg.UserName + "</td></tr><tr><td style='text-align:right;font-weight:600'>Password:</td><td> " + reg.Password + " </td> " +
                                "</tr>" +
                                 "<tr><td style='text-align:right;font-weight:600'>Transaction Password:</td><td> " + reg.TrxPassword + " </td> " +
                                "</tr><tr><td colspan='2' style='padding-top:15px'>All the best !!!</td></tr><tr><td colspan='2'>BtcPro Team</td></tr>" +
                                "</table>";
            result = await SendEMail(reg.EmailId, "", "Registration Confirmation", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }
        [HttpPost]
        public JsonResult sendMyTransactionPassword()
            {
            var reg = db.Registrations.Where(r => r.UserName.ToUpper() == User.Identity.Name.ToUpper()).SingleOrDefault();
            Boolean status = false;
            if (reg != null)
                {
                //send email
                string line0 = "<table style='width:100%'>";
                string line1 = "<tr><td colspan='2'>Dear <b>" + reg.FullName + "</b></td></tr>";
                string line2 = "<tr style='line-height:28px'><td colspan='2'>You have requested to restore transaction password of your account.</td></tr>";
                string line3 = "<tr style='line-height:40px'><td colspan='2'>The details are as follows:</td></tr>";
                string line4 = "<tr><td style='width:50px'>Username:</td><td><b>" + reg.UserName + "</b></td></tr>";
                string line5 = "<tr><td style='width:50px'>Password:</td><td><b>" + reg.TrxPassword + "</b></td></tr>";
                string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
                string line9 = "<tr><td colspan='2' style='line-height:48px'>You can change this password in your personal area</td><tr>";
                string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
                string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
                string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
                string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
                string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
                string line15 = "</table>";
                string body = line0 + line1 + line2 + line3 + line4 + line5 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;


                string mail_from = "noreply@btcpro.co";
                string mail_to = reg.EmailId;
                ArrayList arr = null;
                string subj = "Forget Transaction Password mail";
                string mail_cc = "info@btcpro.co";
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                ht.Add("FROM", mail_from);
                ht.Add("TO", mail_to);
                ht.Add("CC", mail_cc);
                //ht.Add("BCC", mail_cc);
                ht.Add("SUBJECT", subj);
                ht.Add("BODY", body);
                ht.Add("ATTACHMENT", arr);

                try
                    {
                    Email mail = new Email(ht);
                    mail.SendEMail();

                    }
                catch (Exception e)
                    {

                    }
                status = true;

                return Json(new { Status = status }, JsonRequestBehavior.AllowGet);
                }
            return Json(new { Status = status }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public async Task<JsonResult> SendMyWithdrawalRequestemail(string Username, double amount, string status)
            {
            if (Username == "")
                {
                Username = User.Identity.Name;
                }
            var reg = db.Registrations.SingleOrDefault(r => r.UserName == Username);
            bool result = false;
            ArrayList array = new ArrayList();

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + Username + "</b></td></tr>";
            string line2 = "<tr style='line-height:28px'><td colspan='2'>We have received your withdrawal request.</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details are as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Amount:</td><td><b>$ " + amount.ToString() + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Address:</td><td><b>" + reg.MyWalletAccount + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Status:</td><td><b>Under process</b></td></tr>";
            string line7 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line8 = "<tr><td colspan='2' style='line-height:48px'>If you have any queries, please do not hesitate to contact us.</td><tr>";
            string line9 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line10 = "<tr><td colspan='2'>Best regards,</td><tr>";
            string line11 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
            string line12 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
            string line13 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
            string line14 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14;

            result = await SendEMail(reg.EmailId, "", "Withdrawal Request mail", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public async Task<JsonResult> NotifyAdminAboutBalanceTransfer(string ToUsername, double Amount)
            {
            string Username = User.Identity.Name;
            ArrayList array = new ArrayList();

            string body =
            @"<table>
            <tr><td><b>Dear Administrator,</b></td></tr>
            <tr><td><b>"+ Username + @"</b> has transferred <b>$" + Amount + @"</b> to <b>" + ToUsername + @".</b></td></tr>
            <tr><td>This is a system generated message.</td></tr>
            </table>";

            bool result = false;
            result = await SendEMail("noreply@btcpro.co", "", "Reserve Wallet Transfer Notification", body, array);

            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public async Task<JsonResult> NotifyAdminAboutPackagePurchase(int PackageId, double Amount)
            {
            string Username = User.Identity.Name;
            string Packagename = db.Packages.Where(p => p.Id == PackageId).Select(p => p.Name).FirstOrDefault();
            ArrayList array = new ArrayList();

            string body =
            @"<table>
            <tr><td><b>Dear Administrator,</b></td></tr>
            <tr><td><b>" + Username + @"</b> has just invested <b>$" + Amount + @"</b> in package <b>" + Packagename + @".</b></td></tr>
            <tr><td>This is a system generated message.</td></tr>
            </table>";

            bool result = false;
            result = await SendEMail("noreply@btcpro.co", "", "New Package Investment Notification", body, array);

            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public async Task<JsonResult> SendMyWithdrawalConfirmationemail(long RegistrationId, double amount, string status)
            {

            var reg = db.Registrations.SingleOrDefault(r => r.Id == RegistrationId);
            string Username = reg.UserName;
            bool result = false;
            ArrayList array = new ArrayList();

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + Username + "</b></td></tr>";
            string line2 = "<tr style='line-height:28px'><td colspan='2'>Your withdrawal request has been approved!.</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details are as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Amount:</td><td><b>$ " + amount.ToString() + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Address:</td><td><b>" + reg.MyWalletAccount + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Status:</td><td><b>Approved</b></td></tr>";
            string line7 = "<tr><td style='width:50px'>Notes:</td><td><b>" + status + "</b></td></tr>";
            string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line9 = "<tr><td colspan='2' style='line-height:48px'>If you have any queries, please do not hesitate to contact us.</td><tr>";
            string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
            string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
            string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
            string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
            string line15 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;

            result = await SendEMail(reg.EmailId, "", "Withdrawal payment approval mail", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        public async Task<JsonResult> SendMyWithdrawalCancellationemail(long RegistrationId, double amount, string status)
            {

            var reg = db.Registrations.SingleOrDefault(r => r.Id == RegistrationId);
            string Username = reg.UserName;
            bool result = false;
            ArrayList array = new ArrayList();

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + Username + "</b></td></tr>";
            string line2 = "<tr style='line-height:28px'><td colspan='2'>Your withdrawal request has been declined.</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details are as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Amount:</td><td><b>$ " + amount.ToString() + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Address:</td><td><b>" + reg.MyWalletAccount + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Status:</td><td><b>Declined</b></td></tr>";
            string line7 = "<tr><td style='width:50px'>Notes:</td><td><b>" + status + "</b></td></tr>";
            string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line9 = "<tr><td colspan='2' style='line-height:48px'>If you have any queries, please do not hesitate to contact us.</td><tr>";
            string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
            string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
            string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
            string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
            string line15 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;

            result = await SendEMail(reg.EmailId, "", "Withdrawal payment status mail", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        public async Task<JsonResult> SendMyWithdrawalCommentUpdateemail(long RegistrationId, double amount, string status)
            {

            var reg = db.Registrations.SingleOrDefault(r => r.Id == RegistrationId);
            string Username = reg.UserName;
            bool result = false;
            ArrayList array = new ArrayList();

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + Username + "</b></td></tr>";
            string line2 = "<tr style='line-height:28px'><td colspan='2'>Admin has commented on your withdrawal request.</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details of the withdrawal request as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Amount:</td><td><b>$ " + amount.ToString() + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Address:</td><td><b>" + reg.MyWalletAccount + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Status:</td><td><b>Approved</b></td></tr>";
            string line7 = "<tr><td style='width:50px'>Notes:</td><td><b>" + status + "</b></td></tr>";
            string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line9 = "<tr><td colspan='2' style='line-height:48px'>If you have any queries, please do not hesitate to contact us.</td><tr>";
            string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
            string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
            string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
            string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
            string line15 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;

            result = await SendEMail(reg.EmailId, "", "Withdrawal request update mail", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        public async Task<JsonResult> SendSupportTicketemail(string subject, string message, string attachment, string category)
            {
            string user = User.Identity.Name;
            var reg = db.Registrations.SingleOrDefault(i => i.UserName == user);
            string Username = reg.UserName;
            string Attachment = attachment == "" ? "No" : "Yes";
            ArrayList array = new ArrayList();
            bool result = false;

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + "Administrator" + "</b></td></tr>";
            string line2 = "<tr style='line-height:28px'><td colspan='2'>" + Username + " has created a support ticket.</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details of the support ticket as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Ticket Type:</td><td><b>$ " + category + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Subject:</td><td><b>" + subject + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Message:</td><td><b>" + message + "</b></td></tr>";
            string line7 = "<tr><td style='width:50px'>Has Attachment:</td><td><b>" + Attachment + "</b></td></tr>";
            string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line9 = "<tr><td colspan='2' style='line-height:48px'></td><tr>";
            string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line11 = "<tr><td colspan='2'></td><tr>";
            string line12 = "<tr><td colspan='2'>This is a system generated email.</td><tr>";
            string line13 = "<tr><td colspan='2'><a></a></td><tr>";
            string line14 = "<tr><td colspan='2'><a></a></td><tr>";
            string line15 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;

            result = await SendEMail(reg.EmailId, "", "New support ticket by " + Username + "!!!", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        public async Task<JsonResult> SendPostingUpdateemail(string Username, string DrCr, int WalletType, double Amount, string Comment)
            {

            var reg = db.Registrations.SingleOrDefault(r => r.UserName == Username);
            string wallet = db.Wallets.Where(w => w.Id == WalletType).Select(w => w.WalletName).FirstOrDefault();
            bool result = false;
            ArrayList array = new ArrayList();

            string line0 = "<table style='width:100%'>";
            string line1 = "<tr><td colspan='2'>Dear <b>" + Username + "</b></td></tr>";
            string text = "";
            string status = "";
            if (DrCr == "D") //Deposit
                {
                text = "Balance transfer by Admin to your account";
                status = "Deposit";
                }
            if (DrCr == "W") //Withdraw
                {
                text = "Balance transfer by Admin from your account";
                status = "Withdraw";
                }
            string line2 = "<tr style='line-height:28px'><td colspan='2'>" + text + "</td></tr>";
            string line3 = "<tr style='line-height:40px'><td colspan='2'>The details of the transfer are as follows:</td></tr>";
            string line4 = "<tr><td style='width:50px'>Amount:</td><td><b>$ " + Amount.ToString() + "</b></td></tr>";
            string line5 = "<tr><td style='width:50px'>Wallet:</td><td><b>" + wallet + "</b></td></tr>";
            string line6 = "<tr><td style='width:50px'>Event:</td><td><b>" + status + "</b></td></tr>";
            string line7 = "<tr><td style='width:50px'>Notes:</td><td><b>" + Comment + "</b></td></tr>";
            string line8 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line9 = "<tr><td colspan='2' style='line-height:48px'>If you have any queries, please do not hesitate to contact us.</td><tr>";
            string line10 = "<tr style='line-height:48px'><td colspan='2'></td><tr>";
            string line11 = "<tr><td colspan='2'>Best regards,</td><tr>";
            string line12 = "<tr><td colspan='2'>BTC Pro Inc.</td><tr>";
            string line13 = "<tr><td colspan='2'><a>support@btcpro.co</a></td><tr>";
            string line14 = "<tr><td colspan='2'><a>www.btcpro.co</a></td><tr>";
            string line15 = "</table>";
            string body = line0 + line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11 + line12 + line13 + line14 + line15;

            result = await SendEMail(reg.EmailId, "", "Balance transfer by BTC Pro Admin", body, array);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult TransactionPasswordExists()
            {
            var user = User.Identity.Name;
            string txpwd = db.Registrations.Where(i => i.UserName == user).Select(i => i.TrxPassword).FirstOrDefault();
            Boolean isExists = false;
            if (txpwd != null) { isExists = true; } else { isExists = false; }
            return Json(new { isEXISTS = isExists }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult TransactionPasswordMatch(string OldPassword)
            {
            var user = User.Identity.Name;
            string txpwd = db.Registrations.Where(i => i.UserName == user).Select(i => i.TrxPassword).FirstOrDefault();
            Boolean isMatching = false;
            if (txpwd == OldPassword) { isMatching = true; } else { isMatching = false; }
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

        public JsonResult ResetPasswordInRegistration(string OldPassword)
            {
            var user = User.Identity.Name;
            var res = db.Registrations.Where(i => i.UserName == user).Single();
            res.Password = OldPassword;
            db.SaveChanges();
            Boolean isMatching = true;
            return Json(new { isOK = isMatching }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult RegistrationDetail()
            {
            string username = User.Identity.Name;
            var mem = db.Registrations.SingleOrDefault(m => m.UserName == username);

            return Json(new { Member = mem }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult RegistrationDetail1(string username)
            {
            var mem = db.Registrations.SingleOrDefault(m => m.UserName == username);

            return Json(new { Member = mem }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult AdminTransferLedger(string Username)
            {
            var admin = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper() == "SUPERADMIN");
            if (Username != "")
                {
                var mem = db.Registrations.SingleOrDefault(m => m.UserName.ToUpper() == Username);

                var transfers = ( from t in db.Ledgers
                                  from r in db.Registrations
                                  from w in db.Wallets
                                  where t.ToFromUser == admin.Id && t.RegistrationId == mem.Id && t.RegistrationId == r.Id
                                  && t.WalletId == w.Id
                                  orderby t.Id descending
                                  select new MemberTransferVM
                                      {
                                      Id = t.Id,
                                      DateD = t.Date,
                                      Deposit = t.Deposit,
                                      Withdraw = t.Withdraw,
                                      Transfer = r.UserName,
                                      Walletname = w.WalletName,
                                      Balance = 0,
                                      Comment = t.Comment
                                      } ).ToList();

                for (int i = 0; i < transfers.Count(); i++)
                    {
                    transfers[i].Date = transfers[i].DateD.ToLongDateString();
                    }
                //var ledger = db.Ledgers.Where(m => m.RegistrationId == mem.Id && m.ToFromUser == admin.Id).OrderByDescending(m => m.Date);
                return Json(new { Ledger = transfers }, JsonRequestBehavior.AllowGet);
                }
            else
                {
                var transfers = ( from t in db.Ledgers
                                  from r in db.Registrations
                                  from w in db.Wallets
                                  where t.ToFromUser == admin.Id && t.WalletId == w.Id && t.RegistrationId == r.Id
                                  orderby t.Id descending
                                  select new MemberTransferVM
                                      {
                                      Id = t.Id,
                                      DateD = t.Date,
                                      Deposit = t.Deposit,
                                      Withdraw = t.Withdraw,
                                      Transfer = r.UserName,
                                      Walletname = w.WalletName,
                                      Balance = 0,
                                      Comment = t.Comment
                                      } ).ToList();

                for (int i = 0; i < transfers.Count(); i++)
                    {
                    transfers[i].Date = transfers[i].DateD.ToLongDateString();
                    }

                //var ledger = db.Ledgers.Where(m => m.ToFromUser == admin.Id).OrderBy(m => m.Date);
                return Json(new { Ledger = transfers }, JsonRequestBehavior.AllowGet);
                }
            }

        public JsonResult UpdateMember(long id, Member member)
            {
            try
                {
                Member mem = db.Members.SingleOrDefault(m => m.Id == id);
                mem.Firstname = member.Firstname;
                mem.Emailid = member.Emailid;
                mem.Addressline1 = member.Addressline1;
                mem.Addressline2 = member.Addressline2;
                mem.City = member.City;
                mem.Postcode = member.Postcode;
                mem.State = member.State;
                mem.Country = member.Country;
                mem.Mobileno = member.Mobileno;
                mem.Secretquestion = member.Secretquestion;
                mem.Secretpassword = member.Secretpassword;
                mem.Transactionpassword = member.Transactionpassword;

                Register reg = db.Registrations.SingleOrDefault(r => r.UserName == mem.Username);
                reg.FullName = mem.Firstname;
                reg.EmailId = mem.Emailid;
                reg.CountryCode = mem.Country;
                try
                    {
                    db.SaveChanges();
                    }
                catch (Exception e)
                    {
                    Console.WriteLine(e.InnerException);
                    }


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            catch (Exception e)
                {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }

        public JsonResult GetCountry(string countryCode)
            {
            string Countryname = db.Countries.Where(c => c.CountryId == countryCode).Select(c => c.CountryName).Single();
            if (Countryname == "" || Countryname == null)
                {
                Countryname = "hidden";
                }

            return Json(new { CountryName = Countryname }, JsonRequestBehavior.AllowGet);
            }

        [AllowAnonymous]
        public ActionResult CalculatePayout()
            {
            //Set the Process Id
            string processId = "PRC-12";
            var PP = db.PayoutProcess.ToList().OrderByDescending(pp => pp.Id).FirstOrDefault();

            //post the Fixed Incomes first
            var allrecords = db.Members.ToList();
            foreach (Member mem in allrecords)
                {
                #region //post the Binary Incomes

                BinaryIncomeLedgerVMTotals BI = GetCurrentBinaryIncome(mem.Username);
                double binaryincome = BI.IncomeAmount;
                List<Ledger> postingsBI = new List<Ledger>();

                BinaryOpening closingrec = new BinaryOpening();
                closingrec.RegistrationId = mem.RegistrationId;
                closingrec.Week = PP.FromDate.ToShortDateString() + " - " + PP.ToDate.ToShortDateString();
                closingrec.ProcessId = processId;
                closingrec.LeftSideOp = BI.opLeftAmount;
                closingrec.RightSideOp = BI.opRightAmount;
                closingrec.LeftSideCurrent = BI.CurrentLeftAmount;
                closingrec.RightSideCurrent = BI.CurrentRightAmount;
                closingrec.LeftSideGrandTotal = BI.TotalLeftAmount;
                closingrec.RightSideGrandTotal = BI.TotalRightAmount;
                if (closingrec.LeftSideGrandTotal >= closingrec.RightSideGrandTotal)
                    {
                    closingrec.Binary = closingrec.RightSideGrandTotal;
                    }
                if (closingrec.LeftSideGrandTotal < closingrec.RightSideGrandTotal)
                    {
                    closingrec.Binary = closingrec.LeftSideGrandTotal;
                    }
                closingrec.CappingAmount = BI.ConsiderationAmount;
                closingrec.Income = BI.IncomeAmount;
                closingrec.LeftSideCd = BI.cdLeftAmount;
                closingrec.RightSideCd = BI.cdRightAmount;
                closingrec.IsCurrent = true;
                db.BinaryOpenings.Add(closingrec);

                if (binaryincome > 0)
                    {
                    Ledger postBIcash = new Ledger();
                    postBIcash.RegistrationId = mem.RegistrationId;
                    postBIcash.WalletId = 1;
                    postBIcash.Date = DateTime.Now;
                    postBIcash.Deposit = ( binaryincome * 70 ) / 100;
                    postBIcash.Withdraw = 0;
                    postBIcash.TransactionTypeId = 10;
                    postBIcash.TransactionId = 0;
                    postBIcash.SubLedgerId = 5;
                    postBIcash.ToFromUser = 0;
                    postBIcash.BatchNo = "";
                    postBIcash.ProcessId = processId;
                    postBIcash.Leftside_cd = 0;
                    postBIcash.Rightside_cd = 0;
                    postingsBI.Add(postBIcash);

                    Ledger postBIreserve = new Ledger();
                    postBIreserve.RegistrationId = mem.RegistrationId;
                    postBIreserve.WalletId = 2;
                    postBIreserve.Date = DateTime.Now;
                    postBIreserve.Deposit = ( binaryincome * 30 ) / 100;
                    postBIreserve.Withdraw = 0;
                    postBIreserve.TransactionTypeId = 10;
                    postBIreserve.TransactionId = 0;
                    postBIreserve.SubLedgerId = 5;
                    postBIreserve.ToFromUser = 0;
                    postBIreserve.BatchNo = "";
                    postBIreserve.ProcessId = processId;
                    postBIreserve.Leftside_cd = 0;
                    postBIreserve.Rightside_cd = 0;
                    postingsBI.Add(postBIreserve);

                    db.Ledgers.AddRange(postingsBI);
                    db.SaveChanges();
                    }

                #endregion

                #region //post sponsor incomes
                var sponsorpostings = db.SponsorIncomes.Where(r => r.RegistrationId == mem.RegistrationId && r.ProcessId == null).ToList();
                if (sponsorpostings.Count() > 0)
                    {
                    List<Ledger> SPpostings = new List<Ledger>();
                    for (int k = 0; k < sponsorpostings.Count(); k++)
                        {
                        Ledger SPpostcash = new Ledger();
                        SPpostcash.RegistrationId = mem.RegistrationId;
                        SPpostcash.WalletId = 1;
                        SPpostcash.Date = DateTime.Now;
                        SPpostcash.Deposit = (double) sponsorpostings[k].CashWallet;
                        SPpostcash.Withdraw = 0;
                        SPpostcash.TransactionTypeId = 10;
                        SPpostcash.TransactionId = 0;
                        SPpostcash.SubLedgerId = 6;
                        SPpostcash.ToFromUser = sponsorpostings[k].RegistrationId;
                        SPpostcash.BatchNo = "";
                        SPpostcash.ProcessId = processId;
                        SPpostcash.Leftside_cd = 0;
                        SPpostcash.Rightside_cd = 0;
                        SPpostings.Add(SPpostcash);

                        Ledger SPpostreserve = new Ledger();
                        SPpostreserve.RegistrationId = mem.RegistrationId;
                        SPpostreserve.WalletId = 2;
                        SPpostreserve.Date = DateTime.Now;
                        SPpostreserve.Deposit = (double) sponsorpostings[k].ReserveWallet;
                        SPpostreserve.Withdraw = 0;
                        SPpostreserve.TransactionTypeId = 10;
                        SPpostreserve.TransactionId = 0;
                        SPpostreserve.SubLedgerId = 6;
                        SPpostreserve.ToFromUser = sponsorpostings[k].RegistrationId;
                        SPpostreserve.BatchNo = "";
                        SPpostreserve.ProcessId = processId;
                        SPpostreserve.Leftside_cd = 0;
                        SPpostreserve.Rightside_cd = 0;
                        SPpostings.Add(SPpostreserve);

                        sponsorpostings[k].ProcessId = processId;
                        }
                    db.Ledgers.AddRange(SPpostings);
                    db.SaveChanges();
                    }
                #endregion

                #region post generation income
                var generationpostings = GetMyCurrentGenerationIncomes(mem.Username);
                if (generationpostings.Count() > 0)
                    {
                    List<Ledger> GPpostings = new List<Ledger>();
                    for (int k = 0; k < generationpostings.Count(); k++)
                        {
                        double earning = generationpostings[k].WalletAmount;
                        if (earning > 0)
                            {
                            Ledger GPpostcash = new Ledger();
                            GPpostcash.RegistrationId = mem.RegistrationId;
                            GPpostcash.WalletId = 1;
                            GPpostcash.Date = DateTime.Now;
                            GPpostcash.Deposit = ( earning * 70 ) / 100;
                            GPpostcash.Withdraw = 0;
                            GPpostcash.TransactionTypeId = 10;
                            GPpostcash.TransactionId = 0;
                            GPpostcash.SubLedgerId = 7;
                            GPpostcash.ToFromUser = generationpostings[k].RegistrationId;
                            GPpostcash.BatchNo = "";
                            GPpostcash.ProcessId = processId;
                            GPpostcash.Leftside_cd = 0;
                            GPpostcash.Rightside_cd = 0;
                            GPpostings.Add(GPpostcash);

                            Ledger GPpostreserve = new Ledger();
                            GPpostreserve.RegistrationId = mem.RegistrationId;
                            GPpostreserve.WalletId = 2;
                            GPpostreserve.Date = DateTime.Now;
                            GPpostreserve.Deposit = ( earning * 30 ) / 100;
                            GPpostreserve.Withdraw = 0;
                            GPpostreserve.TransactionTypeId = 10;
                            GPpostreserve.TransactionId = 0;
                            GPpostreserve.SubLedgerId = 7;
                            GPpostreserve.ToFromUser = generationpostings[k].RegistrationId;
                            GPpostreserve.BatchNo = "";
                            GPpostreserve.ProcessId = processId;
                            GPpostreserve.Leftside_cd = 0;
                            GPpostreserve.Rightside_cd = 0;
                            GPpostings.Add(GPpostreserve);
                            }
                        }
                    db.Ledgers.AddRange(GPpostings);

                    db.SaveChanges();
                    }
                #endregion
                }
            var binaryrecs = db.BinaryIncomes.Where(b => b.ProcessId == null).ToList();
            for (int z = 0; z < binaryrecs.Count(); z++)
                {
                binaryrecs[z].ProcessId = processId;
                }
            var generationrecs = db.GenerationIncomes.Where(b => b.ProcessId == null).ToList();
            for (int z = 0; z < generationrecs.Count(); z++)
                {
                generationrecs[z].ProcessId = processId;
                }
            db.SaveChanges();

            return null;
            }
        public JsonResult TodayPendingInvestmentReturnprocess()
            {
            double totalamount = 0;

            var records = ( from w in db.WeeklyIncomes
                            from r in db.Registrations
                            from p in db.Packages
                            where w.RegistrationId == r.Id && w.PackageId == p.Id && w.DueDate <= DateTime.Now && w.ProcessId == null
                            orderby w.DueDate
                            select new WeeklyIncomeVM
                                {
                                DueDate = w.DueDate,
                                Username = r.UserName,
                                PackageName = p.Name,
                                InvestmentAmount = db.Purchases.Where(p => p.Payreferenceno == w.BatchNo).Select(t => t.Amount).FirstOrDefault(),
                                FixedIncomeWallet = w.FixedIncomeWallet,
                                Status = "Unpaid"
                                } ).ToList();

            for (int i = 0; i < records.Count(); i++)
                {
                totalamount = totalamount + (double) records[i].FixedIncomeWallet;
                records[i].sDueDate = records[i].DueDate.ToLongDateString();
                }

            return Json(new { ToProcess = records, TotalPayout = totalamount }, JsonRequestBehavior.AllowGet);
            }
        [HttpPost]
        public JsonResult CalculateDailyPayout()
            {
            long counter = 0;
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            //Set the Process Id
            string processId = "PRC-" + GuidString;
            int processcount = 0;
            //post the Fixed Incomes first
            var allrecords = db.Members.ToList();
            foreach (Member mem in allrecords)
                {
                #region //post the Fixed Incomes first
                var fixedpostings = db.WeeklyIncomes.Where(r => r.RegistrationId == mem.RegistrationId && r.DueDate <= DateTime.Now && r.ProcessId == null).ToList();
                processcount = fixedpostings.Count();
                if (fixedpostings.Count() > 0)
                    {
                    List<Ledger> postings = new List<Ledger>();
                    for (int k = 0; k < fixedpostings.Count(); k++)
                        {
                        Ledger post = new Ledger();
                        post.RegistrationId = mem.RegistrationId;
                        post.WalletId = 3;
                        post.Date = DateTime.Now;
                        post.Deposit = (double) fixedpostings[k].FixedIncomeWallet;
                        post.Withdraw = 0;
                        post.TransactionTypeId = 10;
                        post.TransactionId = 0;
                        post.SubLedgerId = 4;
                        post.ToFromUser = 0;
                        post.BatchNo = "";
                        post.ProcessId = processId;
                        post.Leftside_cd = 0;
                        post.Rightside_cd = 0;
                        postings.Add(post);

                        fixedpostings[k].ProcessId = processId;
                        fixedpostings[k].PaidDate = DateTime.Now;

                        counter = counter + 1;
                        }
                    db.Ledgers.AddRange(postings);
                    db.SaveChanges();
                    }

                #endregion
                }
            return Json(new { Success = true, Postings = counter }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult UpdatePaymentRequest(int id, string comment)
            {
            WithdrawalRequest recToUpdate = db.WithdrawalRequests.SingleOrDefault(w => w.Id == id);
            recToUpdate.Approved_Date = DateTime.Now;
            recToUpdate.Status = "Paid";
            recToUpdate.sDate = recToUpdate.Date.ToLongTimeString();
            recToUpdate.ReferenceNo = ( 100000 + recToUpdate.Id ).ToString();
            recToUpdate.Comment = comment;
            try
                {
                db.SaveChanges();
                }
            catch (Exception e)
                {

                }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult UpdatePaymentRequestComment(int id, string comment)
            {
            WithdrawalRequest recToUpdate = db.WithdrawalRequests.SingleOrDefault(w => w.Id == id);
            recToUpdate.Comment = comment;
            try
                {
                db.SaveChanges();
                }
            catch (Exception e)
                {

                }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult Membername()
            {
            string user = User.Identity.Name;
            return Json(new { CurrentUser = user }, JsonRequestBehavior.AllowGet);
            }

        [AllowAnonymous]
        public JsonResult GetWorkingLeg(string user)
            {
            if (user == "")
                {
                user = User.Identity.Name;
                }

            var rec = db.Registrations.SingleOrDefault(r => r.UserName == user);
            string leg = rec.WorkingLeg;
            return Json(new { Position = leg }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult SetWorkingLeg(string leg)
            {
            string user = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName == user);
            rec.WorkingLeg = leg;
            db.SaveChanges();
            return Json(new { Position = rec.WorkingLeg }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult UpdateTransactionPassword(string TxPassword)
            {
            string user = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName == user);
            rec.TrxPassword = TxPassword;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        public JsonResult MatchTrnPassword(string TxPassword)
            {
            Boolean isMatched = false;
            string user = User.Identity.Name;
            var rec = db.Registrations.SingleOrDefault(r => r.UserName == user);
            isMatched = ( rec.TrxPassword == TxPassword ? true : false );
            return Json(new { Success = isMatched }, JsonRequestBehavior.AllowGet);
            }
        public ActionResult PayCryptoCurrency(string username, long UplineId, int packageId, double investmentAmt, string cointype)
            {
            if (username == null) { username = User.Identity.Name; }
            string packagename = db.Packages.Where(p => p.Id == packageId).Select(p => p.Name).FirstOrDefault();
            if (packageId == 0)
                {
                packagename = "Repurchase";
                }
            CoinPayments cnp = new CoinPayments("1396f745295c35c328aD3381EfFb833d39524E59e7Ae333C9Db0e96a03c2Cd93", "247d9b36d83aa885f5d9ac79d695ae3fcc4bf7a368910e1843d2cfb3cdd3b98d");
            //cnp.CallAPI("get_tx_info");
            var retobj = cnp.CallAPI("create_transaction", username, packagename, investmentAmt, cointype);
            try
                {
                CryptoCurrrency objcurrency = new CryptoCurrrency();
                Dictionary<string, object> newobj = new Dictionary<string, object>();
                newobj = (Dictionary<string, object>) retobj.Values.ElementAt(1);
                foreach (KeyValuePair<string, object> kv in newobj)
                    {
                    string key = kv.Key;
                    object value = kv.Value;
                    if (key == "amount") { objcurrency.ConvertedAmount = value.ToString(); }
                    if (key == "txn_id") { objcurrency.Transactionid = value.ToString(); }
                    if (key == "address") { objcurrency.TargetWalletAccount = value.ToString(); }
                    if (key == "status_url") { objcurrency.Status_url = value.ToString(); }
                    if (key == "qrcode_url") { objcurrency.Qrcode_url = value.ToString(); }
                    }
                objcurrency.Username = username;
                objcurrency.Transactiondate = DateTime.Now;
                objcurrency.PackageId = packageId;
                objcurrency.PackageAmt = investmentAmt;
                objcurrency.UplineId = UplineId;
                objcurrency.Paying_Currency = "USD";
                objcurrency.Target_Currency = cointype;

                db.CryptoCurrencies.Add(objcurrency);
                db.SaveChanges();
                return Json(new { objTransaction = objcurrency }, JsonRequestBehavior.AllowGet);

                }
            catch (Exception e)
                {
                return null;
                }

            }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ipn(ipn IPN)
            {
            db.Ipns.Add(IPN);
            var cryptoObj = db.CryptoCurrencies.Where(t => t.Transactionid == IPN.txn_id).FirstOrDefault();
            if (cryptoObj != null)
                {
                cryptoObj.Paymentstatus = System.Convert.ToInt16(IPN.status);
                cryptoObj.StatusRemarks = IPN.status_text;
                if (cryptoObj.Paymentstatus >= 100)
                    {
                    cryptoObj.FullyPaid = true;
                    cryptoObj.PaymentDate = DateTime.Now;
                    db.SaveChanges();
                    //time for crediting the amount in member ledger first
                    ledgerposting(cryptoObj.Username, "D", 2, cryptoObj.PackageAmt, "Package purchase with BitCoin");
                    //yes-- ready for autopurchase now
                    await autoPurchase(cryptoObj.Username, cryptoObj.UplineId, cryptoObj.PackageId, cryptoObj.PackageAmt);
                    }
                }
            await db.SaveChangesAsync();
            return null;
            }

        [HttpPost]
        public JsonResult NewsReportsEdit(NewsReport NewsReport)
            {
            if (User.Identity.Name != null)
                {
                var rpt = db.NewsReports.SingleOrDefault(n => n.Id == NewsReport.Id);
                rpt.NewsItemTitle = NewsReport.NewsItemTitle;
                rpt.NewsItemBody = NewsReport.NewsItemBody;
                rpt.ImageFileName = NewsReport.ImageFileName;
                rpt.NewsAuthor = NewsReport.NewsAuthor;
                rpt.UpdatedDate = DateTime.Now;
                rpt.UpdatedByUser = User.Identity.Name;

                db.SaveChanges();
                }
            return Json(new { Success = "OK" }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult GetCountries()
            {
            var list = db.Countries.ToList();
            return Json(new { Countries = list }, JsonRequestBehavior.AllowGet);
            }

        public ActionResult BinaryOpenings()
            {
            return View();
            }

        public JsonResult BinaryReport(string userId)
            {
            var rec = db.Members.Where(r => r.Username == userId).FirstOrDefault();
            string packagename = db.Packages.Where(p => p.Id == rec.Defaultpackagecode).Select(p => p.Name).Single();
            List<vmBinaryOpening> list = new List<vmBinaryOpening>();
            try
                {
                var binaryopenings = ( from row in db.BinaryIncomes
                                       where row.RegistrationId == rec.RegistrationId && row.ProcessId != null
                                       group row by row.ProcessId into groupblock
                                       select new vmBinaryOpening
                                           {
                                           RegistrationId = groupblock.FirstOrDefault().RegistrationId,
                                           ProcessId = groupblock.FirstOrDefault().ProcessId,
                                           LeftSideCurrent = groupblock.Sum(_ => _.LeftNewBusinessAmount),
                                           RightSideCurrent = groupblock.Sum(_ => _.RightNewBusinessAmount),
                                           } ).ToList();
                double opleft = 0;
                double opright = 0;
                string processid = "";
                int index = 1;
                foreach (vmBinaryOpening bo in binaryopenings)
                    {
                    bo.Id = index;
                    processid = bo.ProcessId;
                    var processrow = db.PayoutProcess.Single(p => p.ProcessNo == processid);
                    bo.Week = processrow.FromDate.ToShortDateString() + " - " + processrow.ToDate.ToShortDateString();
                    bo.LeftSideOp = opleft;
                    bo.RightSideOp = opright;
                    bo.LeftSideGrandTotal = bo.LeftSideOp + bo.LeftSideCurrent;
                    bo.RightSideGrandTotal = bo.RightSideOp + bo.RightSideCurrent;
                    if (bo.LeftSideGrandTotal > bo.RightSideGrandTotal)
                        {
                        bo.Binary = bo.RightSideGrandTotal;
                        bo.LeftSideCd = bo.LeftSideGrandTotal - bo.RightSideGrandTotal;
                        bo.RightSideCd = 0;
                        }
                    if (bo.LeftSideGrandTotal == bo.RightSideGrandTotal)
                        {
                        bo.Binary = bo.LeftSideGrandTotal;
                        bo.LeftSideCd = 0;
                        bo.RightSideCd = 0;
                        }
                    if (bo.LeftSideGrandTotal < bo.RightSideGrandTotal)
                        {
                        bo.Binary = bo.LeftSideGrandTotal;
                        bo.LeftSideCd = 0;
                        bo.RightSideCd = bo.RightSideGrandTotal - bo.LeftSideGrandTotal;
                        }
                    opleft = bo.LeftSideCd;
                    opright = bo.RightSideCd;

                    int capamt = 0;
                    if (rec.Defaultpackagecode == 1 && bo.Binary <= 5000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 5000;
                        }
                    else if (rec.Defaultpackagecode == 1 && bo.Binary > 5000)
                        {
                        bo.CappingAmount = 5000;
                        capamt = 5000;
                        }

                    if (rec.Defaultpackagecode == 2 && bo.Binary <= 10000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 10000;
                        }
                    else if (rec.Defaultpackagecode == 2 && bo.Binary > 10000)
                        {
                        bo.CappingAmount = 10000;
                        capamt = 10000;
                        }
                    if (rec.Defaultpackagecode == 3 && bo.Binary <= 30000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 30000;
                        }
                    else if (rec.Defaultpackagecode == 3 && bo.Binary > 30000)
                        {
                        bo.CappingAmount = 30000;
                        capamt = 30000;
                        }

                    if (rec.Defaultpackagecode == 4 && bo.Binary <= 50000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 50000;
                        }
                    else if (rec.Defaultpackagecode == 4 && bo.Binary > 50000)
                        {
                        bo.CappingAmount = 50000;
                        capamt = 50000;
                        }

                    if (rec.Defaultpackagecode == 6 && bo.Binary <= 200000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 200000;
                        }
                    else if (rec.Defaultpackagecode == 6 && bo.Binary > 200000)
                        {
                        bo.CappingAmount = 200000;
                        capamt = 200000;
                        }

                    if (rec.Defaultpackagecode == 7 && bo.Binary <= 500000)
                        {
                        bo.CappingAmount = bo.Binary;
                        capamt = 500000;
                        }
                    else if (rec.Defaultpackagecode == 7 && bo.Binary > 500000)
                        {
                        bo.CappingAmount = 500000;
                        capamt = 500000;
                        }
                    bo.Income = ( bo.CappingAmount * 10 ) / 100;
                    bo.CappingAmount = capamt;

                    index = index + 1;
                    }
                list = binaryopenings;
                }
            catch (Exception e)
                {

                }
            return Json(new { BinaryReport = list, Fullname = rec.Firstname, Packagename = packagename }, JsonRequestBehavior.AllowGet);
            }


        //used by developer for maintenance support purpose only
        //TRUNCATE the binaryopenings table before running this process
        public void BinaryIncomeTableReConstruction()
            {
            var members = db.Members.ToList();
            foreach (Member rec in members)
                {
                string packagename = db.Packages.Where(p => p.Id == rec.Defaultpackagecode).Select(p => p.Name).Single();
                List<vmBinaryOpening> list = new List<vmBinaryOpening>();
                try
                    {
                    var binaryopenings = ( from row in db.BinaryIncomes
                                           where row.RegistrationId == rec.RegistrationId && row.ProcessId != null
                                           group row by row.ProcessId into groupblock
                                           select new vmBinaryOpening
                                               {
                                               RegistrationId = groupblock.FirstOrDefault().RegistrationId,
                                               ProcessId = groupblock.FirstOrDefault().ProcessId,
                                               LeftSideCurrent = groupblock.Sum(_ => _.LeftNewBusinessAmount),
                                               RightSideCurrent = groupblock.Sum(_ => _.RightNewBusinessAmount),
                                               } ).ToList();
                    double opleft = 0;
                    double opright = 0;
                    string processid = "";
                    foreach (vmBinaryOpening bo in binaryopenings)
                        {
                        processid = bo.ProcessId;
                        var processrow = db.PayoutProcess.Single(p => p.ProcessNo == processid);
                        bo.Week = processrow.FromDate.ToShortDateString() + " - " + processrow.ToDate.ToShortDateString();
                        bo.LeftSideOp = opleft;
                        bo.RightSideOp = opright;
                        bo.LeftSideGrandTotal = bo.LeftSideOp + bo.LeftSideCurrent;
                        bo.RightSideGrandTotal = bo.RightSideOp + bo.RightSideCurrent;
                        if (bo.LeftSideGrandTotal > bo.RightSideGrandTotal)
                            {
                            bo.Binary = bo.RightSideGrandTotal;
                            bo.LeftSideCd = bo.LeftSideGrandTotal - bo.RightSideGrandTotal;
                            bo.RightSideCd = 0;
                            }
                        if (bo.LeftSideGrandTotal == bo.RightSideGrandTotal)
                            {
                            bo.Binary = bo.LeftSideGrandTotal;
                            bo.LeftSideCd = 0;
                            bo.RightSideCd = 0;
                            }
                        if (bo.LeftSideGrandTotal < bo.RightSideGrandTotal)
                            {
                            bo.Binary = bo.LeftSideGrandTotal;
                            bo.LeftSideCd = 0;
                            bo.RightSideCd = bo.RightSideGrandTotal - bo.LeftSideGrandTotal;
                            }
                        opleft = bo.LeftSideCd;
                        opright = bo.RightSideCd;

                        if (rec.Defaultpackagecode == 1 && bo.Binary <= 5000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 1 && bo.Binary > 5000)
                            {
                            bo.CappingAmount = 5000;
                            }

                        if (rec.Defaultpackagecode == 2 && bo.Binary <= 10000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 2 && bo.Binary > 10000)
                            {
                            bo.CappingAmount = 10000;
                            }
                        if (rec.Defaultpackagecode == 3 && bo.Binary <= 30000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 3 && bo.Binary > 30000)
                            {
                            bo.CappingAmount = 30000;
                            }

                        if (rec.Defaultpackagecode == 4 && bo.Binary <= 50000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 4 && bo.Binary > 50000)
                            {
                            bo.CappingAmount = 50000;
                            }

                        if (rec.Defaultpackagecode == 6 && bo.Binary <= 200000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 6 && bo.Binary > 200000)
                            {
                            bo.CappingAmount = 200000;
                            }

                        if (rec.Defaultpackagecode == 7 && bo.Binary <= 500000)
                            {
                            bo.CappingAmount = bo.Binary;
                            }
                        else if (rec.Defaultpackagecode == 7 && bo.Binary > 500000)
                            {
                            bo.CappingAmount = 500000;
                            }

                        bo.Income = ( bo.CappingAmount * 10 ) / 100;

                        BinaryOpening record = new BinaryOpening();
                        record.Binary = bo.Binary;
                        record.CappingAmount = bo.CappingAmount;
                        record.Id = bo.Id;
                        record.Income = bo.Income;
                        record.IsCurrent = bo.IsCurrent;
                        record.LeftSideCd = bo.LeftSideCd;
                        record.LeftSideCurrent = bo.LeftSideCurrent;
                        record.LeftSideGrandTotal = bo.LeftSideGrandTotal;
                        record.LeftSideOp = bo.LeftSideOp;
                        record.ProcessId = bo.ProcessId;
                        record.RegistrationId = bo.RegistrationId;
                        record.RightSideCd = bo.RightSideCd;
                        record.RightSideCurrent = bo.RightSideCurrent;
                        record.RightSideGrandTotal = bo.RightSideGrandTotal;
                        record.RightSideOp = bo.RightSideOp;
                        record.Week = bo.Week;

                        db.BinaryOpenings.Add(record);
                        db.SaveChanges();

                        }
                    }
                catch (Exception e)
                    {

                    }
                }

            return;
            }
        public ActionResult BitCoinDeposit()
            {
            return View();
            }

        public JsonResult BitCoinTransactions()
            {
            var list = ( from i in db.CryptoCurrencies
                         where i.ConvertedAmount != "0"
                         select new
                             {
                             Id = i.Id,
                             TransactionDate = i.Transactiondate.ToString(),
                             TransactionId = i.Transactionid,
                             UserId = i.Username,
                             PackageBought = db.Packages.Where(p => p.Id == i.PackageId).Select(p => p.Name).FirstOrDefault(),
                             InvestAmount = i.PackageAmt,
                             BitcoinAmount = i.ConvertedAmount,
                             PaymentStatus = i.StatusRemarks

                             } ).ToList();


            return Json(new { DepositList = list }, JsonRequestBehavior.AllowGet);
            }

        public JsonResult GetBinaryBreakup(long RegId, string PrcId)
            {
            var list = ( from bi in db.BinaryIncomes
                         from rg in db.Registrations
                         from pk in db.Packages
                         where bi.PurchaserRegistrationId == rg.Id && bi.PackageId == pk.Id && bi.RegistrationId == RegId && bi.ProcessId == PrcId
                         select new
                             {
                             TransactionDate = bi.TransactionDate.ToString(),
                             PurchaserId = rg.UserName,
                             Package = pk.Name,
                             LeftSideAmount = bi.LeftNewBusinessAmount,
                             RightSideAmount = bi.RightNewBusinessAmount
                             } ).ToList();

            return Json(new { DetailData = list }, JsonRequestBehavior.AllowGet);

            }

        private Boolean IsClubMember(long RegistrationId, string searchpos, double investamt)   //Club determination of existing member
            {
            double totalbusinessleft = db.BinaryIncomes.Where(i => i.RegistrationId == RegistrationId).Select(bi => bi.LeftNewBusinessAmount).DefaultIfEmpty(0).Sum();
            double totalbusinessright = db.BinaryIncomes.Where(i => i.RegistrationId == RegistrationId).Select(bi => bi.RightNewBusinessAmount).DefaultIfEmpty(0).Sum();
            if (searchpos == "L") { totalbusinessleft = totalbusinessleft + investamt; }
            if (searchpos == "R") { totalbusinessright = totalbusinessright + investamt; }

            if (totalbusinessleft >= 25000 && totalbusinessright >= 25000)
                {
                return true;
                }
            else
                {
                return false;
                }
            }
        #endregion
        }
    }

