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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace BtcProApp.Controllers
{
    public class RegistersController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/Registers
        public IQueryable<Register> GetRegistrations()
        {
            return db.Registrations;
        }

        // GET: api/Registers/5
        [ResponseType(typeof(Register))]
        public async Task<IHttpActionResult> GetRegister(long id)
        {
            Register register = await db.Registrations.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }

            return Ok(register);
        }

        // PUT: api/Registers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRegister(long id, Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != register.Id)
            {
                return BadRequest();
            }

            db.Entry(register).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisterExists(id))
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

        // POST: api/Registers
        [ResponseType(typeof(Register))]
        public async Task<IHttpActionResult> PostRegister(Register register)
        {
            register.WorkingLeg = "L";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Random rnd = new Random();
            string randNumber = rnd.Next(1001,9999).ToString();
            register.TrxPassword = randNumber;
            db.Registrations.Add(register);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = register.Id }, register);
        }

        // DELETE: api/Registers/5
        [ResponseType(typeof(Register))]
        public async Task<IHttpActionResult> DeleteRegister(long id)
        {
            Register register = await db.Registrations.FindAsync(id);
            if (register == null)
            {
                return NotFound();
            }

            db.Registrations.Remove(register);
            await db.SaveChangesAsync();

            return Ok(register);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegisterExists(long id)
        {
            return db.Registrations.Count(e => e.Id == id) > 0;
        }
    }
}