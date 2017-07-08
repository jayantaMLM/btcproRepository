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
    public class TicketsController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/Tickets
        public IQueryable<Ticket> GetTickets()
        {
            string user = "";
            user = User.Identity.Name;

            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == user.ToUpper());
            return db.Tickets.Where(t => t.RegistrationId == reg.Id).OrderByDescending(t => t.Id);
        }

        // GET: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> GetTicket(long id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTicket(long id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.Id)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = User.Identity.Name;
            ticket.Date = DateTime.Now;
            ticket.DateString = DateTime.Now.ToLongDateString();
            var reg = db.Registrations.SingleOrDefault(t => t.UserName.ToUpper() == user.ToUpper());
            ticket.RegistrationId = reg.Id;
            ticket.isApproved = false;
            
            ticket.Comment = "Open";
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();
            if (ticket.LibraryId >0)
            {
                var lib = db.LibraryDocuments.Where(l => l.Id == ticket.LibraryId).FirstOrDefault();
                lib.ModuleId = (int)ticket.Id;
                await db.SaveChangesAsync();
            }
            
            return CreatedAtRoute("DefaultApi", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> DeleteTicket(long id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(long id)
        {
            return db.Tickets.Count(e => e.Id == id) > 0;
        }
    }
}