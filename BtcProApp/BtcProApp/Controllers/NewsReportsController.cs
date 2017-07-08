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
    public class NewsReportsController : ApiController
    {
        private BtcProDB db = new BtcProDB();

        // GET: api/NewsReports
        public IQueryable<NewsReport> GetNewsReports()
        {
            return db.NewsReports.OrderByDescending(t=>t.Id);
        }

        // GET: api/NewsReports/5
        [ResponseType(typeof(NewsReport))]
        public async Task<IHttpActionResult> GetNewsReport(long id)
        {
            NewsReport newsReport = await db.NewsReports.FindAsync(id);
            if (newsReport == null)
            {
                return NotFound();
            }

            return Ok(newsReport);
        }

        // PUT: api/NewsReports/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNewsReport(long id, NewsReport newsReport)
        {
            if (!ModelState.IsValid || User.Identity.Name==null)
            {
                return BadRequest(ModelState);
            }

            if (id != newsReport.Id)
            {
                return BadRequest();
            }
            newsReport.UpdatedByUser = User.Identity.Name;
            db.Entry(newsReport).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsReportExists(id))
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

        // POST: api/NewsReports
        [ResponseType(typeof(NewsReport))]
        public async Task<IHttpActionResult> PostNewsReport(NewsReport newsReport)
        {
            if (!ModelState.IsValid || User.Identity.Name==null)
            {
                return BadRequest(ModelState);
            }

            newsReport.CreatedDate = DateTime.Now;
            newsReport.UpdatedDate = DateTime.Now;
            newsReport.CreatedByUser = User.Identity.Name;
            newsReport.UpdatedByUser = User.Identity.Name;
            db.NewsReports.Add(newsReport);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            

            return CreatedAtRoute("DefaultApi", new { id = newsReport.Id }, newsReport);
        }

        // DELETE: api/NewsReports/5
        [ResponseType(typeof(NewsReport))]
        public async Task<IHttpActionResult> DeleteNewsReport(long id)
        {
            NewsReport newsReport = await db.NewsReports.FindAsync(id);
            if (newsReport == null)
            {
                return NotFound();
            }

            db.NewsReports.Remove(newsReport);
            await db.SaveChangesAsync();

            return Ok(newsReport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsReportExists(long id)
        {
            return db.NewsReports.Count(e => e.Id == id) > 0;
        }
    }
}