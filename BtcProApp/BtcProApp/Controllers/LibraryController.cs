using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using BtcProApp.Models;

[RoutePrefix("api/Library")]
public class LibraryController : ApiController
{
    private BtcProDB db = new BtcProDB();

    [Route("Resource/{id}")]
    public HttpResponseMessage Get(int id)
    {
        var result = new HttpResponseMessage(HttpStatusCode.OK);
        var imgrec = db.LibraryDocuments.SingleOrDefault(i => i.Id == id);

        if (imgrec != null)
        {
            string path = WebConfigurationManager.AppSettings["LibraryPath"] + "/" + imgrec.Module + "/" + imgrec.UniqueImageName;
            String filePath = path;                               //here path is actual

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            result.Content = new ByteArrayContent(fileBytes);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = imgrec.OriginalImageName;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(imgrec.Remarks);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        }

        return result;

        //var result = new HttpResponseMessage(HttpStatusCode.OK);
        //var imgrec = db.LibraryDocuments.SingleOrDefault(i => i.Id == id);
        //if (imgrec != null)
        //{
        //    if (imgrec.ImageType == "IMAGE")
        //    {
        //        string path = WebConfigurationManager.AppSettings["LibraryPath"] + "/" + imgrec.Module + "/" + imgrec.UniqueImageName;
        //        String filePath = path;                               //here path is actual
        //        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        //        System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream);
        //        MemoryStream memoryStream = new MemoryStream();
        //        image.Save(memoryStream, ImageFormat.Jpeg);
        //        result.Content = new ByteArrayContent(memoryStream.ToArray());
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue(imgrec.Remarks);
        //        fileStream.Close();
        //    }
        //    if (imgrec.ImageType == "PDF")
        //    {
        //        string path = WebConfigurationManager.AppSettings["LibraryPath"] + "/" + imgrec.Module + "/" + imgrec.UniqueImageName;
        //        String filePath = path;                               //here path is actual
        //        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        //        //System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream);
        //        MemoryStream memoryStream = new MemoryStream();
        //        //image.Save(memoryStream, ImageFormat.Jpeg);
        //        result.Content = new ByteArrayContent(memoryStream.ToArray());
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //        fileStream.Close();
        //    }
        //}
        //return result;
    }

    [Route("Resource")]
    public async Task<HttpResponseMessage> Post(string module, int moduleId, string subModule, int subModuleId)
    {
        if (String.IsNullOrEmpty(module) || String.IsNullOrEmpty(subModule) ||
            String.IsNullOrEmpty(moduleId.ToString()) || String.IsNullOrEmpty(subModuleId.ToString()))
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Required parameter module/moduleId/subModule/subModuleId missing!");
        }

        if (!Request.Content.IsMimeMultipartContent())
        {
            return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
        }

        List<UploadResult> result = new List<UploadResult>();
        string documentPath = WebConfigurationManager.AppSettings["DocumentPath"] + "/";

        try
        {
            string fileSaveLocation = WebConfigurationManager.AppSettings["LibraryPath"] + "/" + module;
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);

            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    String fileName = file.Headers.ContentDisposition.Name;
                    String uniqueFileName = file.Headers.ContentDisposition.FileName;
                    String fileExt = uniqueFileName.Substring(uniqueFileName.LastIndexOf(".")).ToUpper();
                    String mediaType = file.Headers.ContentType.MediaType;

                    LibraryDocument newdoc = new LibraryDocument();
                    newdoc.Module = module;
                    newdoc.ModuleId = moduleId;
                    newdoc.SubModule = subModule;
                    newdoc.SubModuleId = subModuleId;
                    newdoc.OriginalImageName = fileName;
                    newdoc.UniqueImageName = uniqueFileName;
                    newdoc.UploadDate = DateTime.Now;
                    if (fileExt == ".JPG" || fileExt == ".JPEG" || fileExt == ".PNG" || fileExt == ".GIF" ||
                        fileExt == ".TIF" || fileExt == ".BMP" || fileExt == ".WMF" || fileExt == ".ICO")
                    {
                        newdoc.ImageType = "IMAGE";
                    }
                    if (fileExt == ".PDF")
                    {
                        newdoc.ImageType = "PDF";
                    }
                    if (fileExt == ".DOC" || fileExt == ".DOCX")
                    {
                        newdoc.ImageType = "WORD";
                    }
                    if (fileExt == ".XLS" || fileExt == ".XLSX")
                    {
                        newdoc.ImageType = "EXCEL";
                    }
                    if (fileExt == ".PPT" || fileExt == ".PPTX")
                    {
                        newdoc.ImageType = "POWERPOINT";
                    }
                    if (fileExt == ".TXT")
                    {
                        newdoc.ImageType = "TEXT";
                    }
                    if (fileExt == ".HTML")
                    {
                        newdoc.ImageType = "HTML";
                    }

                    newdoc.UploadedBy = "";
                    newdoc.Remarks = mediaType;
                    newdoc.UploadedBy = User.Identity.Name;

                    db.LibraryDocuments.Add(newdoc);

                    try
                    {
                        db.SaveChanges();

                        UploadResult res = new UploadResult();
                        res.fileName = newdoc.OriginalImageName;
                        res.fileId = newdoc.Id;
                        res.fileInternalName = newdoc.UniqueImageName;
                        res.fileType = newdoc.ImageType;
                        res.filePath = documentPath + module + "/";
                        res.filesubModule = newdoc.SubModule;
                        res.filesubModuleId = newdoc.SubModuleId;
                        result.Add(res);
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.NotImplemented, "Files not saved. Upload again!");
                    }
                }

                // Send OK Response along with saved file names to the client.
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        catch (Exception e)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    [Route("Index")]
    [ResponseType(typeof(List<LibraryDocument>))]
    public HttpResponseMessage Get()
    {
        List<LibraryDocument> recs = db.LibraryDocuments.ToList();
        return Request.CreateResponse(HttpStatusCode.OK, recs);
    }

    [Route("Index")]
    [ResponseType(typeof(UploadResult))]
    public HttpResponseMessage Get(int id, string module)
    {
        LibraryDocument rec = db.LibraryDocuments.Find(id); ;
        string documentPath = WebConfigurationManager.AppSettings["DocumentPath"] + "/";
        UploadResult res = new UploadResult();
        if (rec != null)
        {
            res.fileName = rec.OriginalImageName;
            res.fileInternalName = rec.UniqueImageName;
            res.fileType = rec.ImageType;
            res.fileId = rec.Id;
            res.filePath = documentPath + module + "/";
            res.filesubModule = rec.SubModule;
            res.filesubModuleId = rec.SubModuleId;
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
        else
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

    }

    [Route("Index")]
    [ResponseType(typeof(List<UploadResult>))]
    public HttpResponseMessage Get(string module, int moduleId)
    {
        //List<LibraryDocument> recs = db.LibraryDocuments.Where(i => i.Module == module && i.ModuleId == moduleId).ToList();
        //return Request.CreateResponse(HttpStatusCode.OK, recs);
        List<UploadResult> recs = new List<UploadResult>();
        string documentPath = WebConfigurationManager.AppSettings["DocumentPath"] + "/";
        recs = (from l in db.LibraryDocuments
                where l.Module == module && l.ModuleId == moduleId
                select new UploadResult
                {
                    fileName = l.OriginalImageName,
                    fileInternalName = l.UniqueImageName,
                    fileType = l.ImageType,
                    fileId = l.Id,
                    filePath = documentPath + module + "/",
                    filesubModule = l.SubModule,
                    filesubModuleId = l.SubModuleId
                }).ToList();

        UploadResult2 result = new UploadResult2();
        result.url = documentPath;
        result.uploadresults = recs;
        return Request.CreateResponse(HttpStatusCode.OK, result);
    }

    [Route("Index")]
    [ResponseType(typeof(List<LibraryDocument>))]
    public HttpResponseMessage Get(string module, int moduleId, string subModule, int subModuleId)
    {
        //List<LibraryDocument> recs = db.LibraryDocuments.Where(i => i.Module == module && i.ModuleId == moduleId && i.SubModule == subModule && i.SubModuleId == subModuleId).ToList();
        //return Request.CreateResponse(HttpStatusCode.OK, recs);
        List<UploadResult> recs = new List<UploadResult>();
        string documentPath = WebConfigurationManager.AppSettings["DocumentPath"] + "/";
        recs = (from l in db.LibraryDocuments
                where l.Module == module && l.ModuleId == moduleId && l.SubModule == subModule && l.SubModuleId == subModuleId
                select new UploadResult
                {
                    fileName = l.OriginalImageName,
                    fileInternalName = l.UniqueImageName,
                    fileType = l.ImageType,
                    fileId = l.Id,
                    filePath = documentPath + module + "/",
                    filesubModule = l.SubModule,
                    filesubModuleId = l.SubModuleId
                }).ToList();

        UploadResult2 result = new UploadResult2();
        result.url = documentPath;
        result.uploadresults = recs;
        return Request.CreateResponse(HttpStatusCode.OK, result);

    }

    [Route("Resource/{id}")]
    public IHttpActionResult Delete(int id)
    {
        var imgrec = db.LibraryDocuments.SingleOrDefault(i => i.Id == id);
        if (imgrec != null)
        {
            string path = WebConfigurationManager.AppSettings["LibraryPath"] + "/" + imgrec.Module + "/" + imgrec.UniqueImageName;
            string filePath = path;
            db.LibraryDocuments.Remove(imgrec);
            File.Delete(filePath);
            db.SaveChanges();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    // We implement MultipartFormDataStreamProvider to override the filename of File which
    // will be stored on server, or else the default name will be of the format like Body-
    // Part_{GUID}. In the following implementation we simply get the FileName from 
    // ContentDisposition Header of the Request Body.
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            String fileName = headers.ContentDisposition.FileName;
            fileName = fileName.Substring(1);
            fileName = fileName.Substring(0, fileName.Length - 1);
            fileName = fileName.Replace("\"", "");

            headers.ContentDisposition.Name = fileName;  //store original name here;

            String fileExt = fileName.Substring(fileName.IndexOf(".")).ToUpper();

            //-------------------------------------------------------------------
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");
            GuidString = GuidString.Replace("\\", "");
            GuidString = GuidString.Replace("-", "");
            //-------------------------------------------------------------------

            string uniqueFileName = GuidString + fileExt;
            return headers.ContentDisposition.FileName = uniqueFileName;
        }
    }
}