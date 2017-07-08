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
    public class ipnsController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/ipns
        public IQueryable<ipn> GetIpns()
        {
            return db.Ipns;
        }

        // GET: api/ipns/5
        [ResponseType(typeof(ipn))]
        public async Task<IHttpActionResult> Getipn(long id)
        {
            ipn ipn = await db.Ipns.FindAsync(id);
            if (ipn == null)
            {
                return NotFound();
            }

            return Ok(ipn);
        }

        // PUT: api/ipns/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putipn(long id, ipn ipn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ipn.id)
            {
                return BadRequest();
            }

            db.Entry(ipn).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ipnExists(id))
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

        // POST: api/ipns
        [ResponseType(typeof(ipn))]
        public async Task<IHttpActionResult> Postipn(ipn ipn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ipns.Add(ipn);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ipn.id }, ipn);
        }

        // DELETE: api/ipns/5
        [ResponseType(typeof(ipn))]
        public async Task<IHttpActionResult> Deleteipn(long id)
        {
            ipn ipn = await db.Ipns.FindAsync(id);
            if (ipn == null)
            {
                return NotFound();
            }

            db.Ipns.Remove(ipn);
            await db.SaveChangesAsync();

            return Ok(ipn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ipnExists(long id)
        {
            return db.Ipns.Count(e => e.id == id) > 0;
        }
    }
}