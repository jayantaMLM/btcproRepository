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
    public class KYCDocumentsController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/KYCDocuments
        public IQueryable<KYCDocument> GetKYCDocuments()
        {
            //return db.KYCDocuments;
            string user = "";
            user = User.Identity.Name;

            var reg = db.Registrations.SingleOrDefault(r => r.UserName.ToUpper() == user.ToUpper());
            return db.KYCDocuments.Where(t => t.RegistrationId == reg.Id).OrderByDescending(t => t.Id);
        }

        // GET: api/KYCDocuments/5
        [ResponseType(typeof(KYCDocument))]
        public async Task<IHttpActionResult> GetKYCDocument(long id)
        {
            KYCDocument kYCDocument = await db.KYCDocuments.FindAsync(id);
            if (kYCDocument == null)
            {
                return NotFound();
            }

            return Ok(kYCDocument);
        }

        // PUT: api/KYCDocuments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKYCDocument(long id, KYCDocument kYCDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kYCDocument.Id)
            {
                return BadRequest();
            }

            db.Entry(kYCDocument).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KYCDocumentExists(id))
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

        // POST: api/KYCDocuments
        [ResponseType(typeof(KYCDocument))]
        public async Task<IHttpActionResult> PostKYCDocument(KYCDocument kYCDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = User.Identity.Name;
            kYCDocument.Date = DateTime.Now;
            kYCDocument.DateString = DateTime.Now.ToLongDateString();
            var reg = db.Registrations.SingleOrDefault(t => t.UserName.ToUpper() == user.ToUpper());
            kYCDocument.RegistrationId = reg.Id;
            kYCDocument.isApproved = false;

            kYCDocument.Comment = "";
            db.KYCDocuments.Add(kYCDocument);
            await db.SaveChangesAsync();
            if (kYCDocument.LibraryId > 0)
            {
                var lib = db.LibraryDocuments.Where(l => l.Id == kYCDocument.LibraryId).FirstOrDefault();
                lib.ModuleId = (int)kYCDocument.Id;
                await db.SaveChangesAsync();
            }

            return CreatedAtRoute("DefaultApi", new { id = kYCDocument.Id }, kYCDocument);
        }

        // DELETE: api/KYCDocuments/5
        [ResponseType(typeof(KYCDocument))]
        public async Task<IHttpActionResult> DeleteKYCDocument(long id)
        {
            KYCDocument kYCDocument = await db.KYCDocuments.FindAsync(id);
            if (kYCDocument == null)
            {
                return NotFound();
            }

            db.KYCDocuments.Remove(kYCDocument);
            await db.SaveChangesAsync();

            return Ok(kYCDocument);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KYCDocumentExists(long id)
        {
            return db.KYCDocuments.Count(e => e.Id == id) > 0;
        }
    }
}