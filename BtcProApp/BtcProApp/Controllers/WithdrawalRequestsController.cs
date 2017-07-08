using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BtcProApp.Models;

namespace BtcProApp.Controllers
{
    public class WithdrawalRequestsController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/WithdrawalRequests
        public List<WithdrawalRequestVM> GetWithdrawalRequests()
        {
            var withdrawalrequests = (from w in db.WithdrawalRequests
                                      from r in db.Registrations
                                      from l in db.Wallets
                                      where w.RegistrationId == r.Id && w.WalletId == l.Id &&
                                      w.ReferenceNo == null && !w.Status.Contains("Cancelled")
                                      select new WithdrawalRequestVM
                                      {
                                          Id = w.Id,
                                          RegistrationId = w.RegistrationId,
                                          Username = r.UserName,
                                          WalletId = w.WalletId,
                                          Walletname = l.WalletName,
                                          Date = w.Date,
                                          Amount = w.Amount,
                                          BitcoinAcNo = w.PaidBitCoinAccount,
                                          PaidOutAmount = w.PaidOutAmount,
                                          ServiceCharge = w.ServiceCharge,
                                          isApproved = false,
                                          Comment=w.Comment
                                      }).ToList();
            for (int i=0;i< withdrawalrequests.Count(); i++)
            {
                withdrawalrequests[i].sDate = withdrawalrequests[i].Date.ToShortDateString();
            }
            return withdrawalrequests;
        }

        // GET: api/WithdrawalRequests
        public List<WithdrawalRequestVM> GetWithdrawalRequests(string mode)
        {
            var withdrawalrequests = (from w in db.WithdrawalRequests
                                      from r in db.Registrations
                                      from l in db.Wallets
                                      where w.RegistrationId == r.Id && w.WalletId == l.Id &&
                                      w.Status=="Paid"
                                      select new WithdrawalRequestVM
                                      {
                                          Id = w.Id,
                                          RegistrationId = w.RegistrationId,
                                          Username = r.UserName,
                                          WalletId = w.WalletId,
                                          Walletname = l.WalletName,
                                          Date = w.Date,
                                          Approved_Date=w.Approved_Date,
                                          Amount = w.Amount,
                                          BitcoinAcNo = w.PaidBitCoinAccount,
                                          PaidOutAmount = w.PaidOutAmount,
                                          ServiceCharge = w.ServiceCharge,
                                          isApproved = false,
                                          Comment=w.Comment
                                      }).ToList();
            for (int i = 0; i < withdrawalrequests.Count(); i++)
            {
                withdrawalrequests[i].sDate = withdrawalrequests[i].Date.ToShortDateString();
                withdrawalrequests[i].sApproved_Date= ((DateTime)withdrawalrequests[i].Approved_Date).ToShortDateString();
            }
            return withdrawalrequests;
        }

        // GET: api/WithdrawalRequests/5
        [ResponseType(typeof(WithdrawalRequest))]
        public async Task<IHttpActionResult> GetWithdrawalRequest(long id)
        {
            WithdrawalRequest withdrawalRequest = await db.WithdrawalRequests.FindAsync(id);
            if (withdrawalRequest == null)
            {
                return NotFound();
            }

            return Ok(withdrawalRequest);
        }

        // PUT: api/WithdrawalRequests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWithdrawalRequest(long id, WithdrawalRequest withdrawalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != withdrawalRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(withdrawalRequest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WithdrawalRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WithdrawalRequests
        [ResponseType(typeof(WithdrawalRequest))]
        public async Task<IHttpActionResult> PostWithdrawalRequest(WithdrawalRequest withdrawalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WithdrawalRequests.Add(withdrawalRequest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = withdrawalRequest.Id }, withdrawalRequest);
        }

        // DELETE: api/WithdrawalRequests/5
        [ResponseType(typeof(WithdrawalRequest))]
        public async Task<IHttpActionResult> DeleteWithdrawalRequest(long id)
        {
            WithdrawalRequest withdrawalRequest = await db.WithdrawalRequests.FindAsync(id);
            if (withdrawalRequest == null)
            {
                return NotFound();
            }

            db.WithdrawalRequests.Remove(withdrawalRequest);
            await db.SaveChangesAsync();

            return Ok(withdrawalRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WithdrawalRequestExists(long id)
        {
            return db.WithdrawalRequests.Count(e => e.Id == id) > 0;
        }
    }
}