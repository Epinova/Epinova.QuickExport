using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using EPiServer.Enterprise;
using System.Web.Mvc;
using EPiServer.Cms.Shell.UI.Controllers.Internal;
using EPiServer.Core;
using EPiServer.Core.Transfer;
using EPiServer.Enterprise.Internal;
using EPiServer.Security;
using EPiServer.Shell.Web.Mvc;
using IOFile = System.IO.File;

namespace Epinova.QuickExport.Controllers
{
    [Authorize(Roles = "WebAdmins,Administrators")]
    public class QuickExportController : Controller
    {
        private readonly IDataImporter _dataImporter;
        private const string FileName = "ExportedFile.episerverdata";


        public QuickExportController(IDataImporter dataImporter)
        {
            _dataImporter = dataImporter;
        }

        public FileResult Download(int id = 0)
        {
            try
            {
                string key = "export" + id;
                string path = Session[key].ToString();
                byte[] contents = IOFile.ReadAllBytes(path);

                Session.Remove(key);
                IOFile.Delete(path);

                return File(contents, "binary/octet-stream", FileName);
            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult Upload(int id = 0)
        {
            var files = GetValidFiles(Request.Files).ToArray();

            if (files.Length == 0)
                throw new Exception("No valid files supplied");

            List<UploadFileResult> results = new List<UploadFileResult>();

            var options = new ImportOptions
            {
                KeepIdentity = false,
                //ValidateDestination = true,
                //EnsureContentNameUniqueness = true
            };

            ContentReference destination = new ContentReference(id);

            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];

                try
                {
                    _dataImporter.Import(file.InputStream, destination, options);
                }
                catch (Exception ex)
                {
                    results.Add(new UploadFileResult
                    {
                        Index = i,
                        FileName = file.FileName,
                        ErrorMessage = ex.Message
                    });

                    _dataImporter.Status.Log.Error(ex.Message, ex);
                }
            }

            return this.JsonData(results);
        }

        private IEnumerable<HttpPostedFileBase> GetValidFiles(HttpFileCollectionBase files)
        {
            if (files == null || files.Count == 0)
                yield break;

            for (var i = 0; i < files.Count; i++)
            {
                if (files[i] != null && files[i].ContentLength > 0)
                    yield return files[i];
            }
        }


        [HttpPost]
        public JsonResult Export(int id = 0)
        {
            var options = new ExportOptions
            {
                ExcludeFiles = false,
                RequiredSourceAccess = AccessLevel.Read,
            };

            try
            {
                string tempFileName = Path.GetTempFileName();

                using (var fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    var source = new ExportSource(new ContentReference(id), ExportSource.RecursiveLevelInfinity);

                    using (var exporter = new DefaultDataExporter())
                    {
                        exporter.SourceRoots.Add(source);

                        exporter.ExportPropertySettings = false;
                        exporter.IncludeImplicitContentDependencies = true;
                        exporter.IncludeImplicitContentTypeDependencies = false;
                        exporter.Export(fileStream, options);
                        exporter.Close();

                        Session["export" + id] = tempFileName;

                        return Json(new { success = true, id });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, id, message = ex.Message });
            }
        }
    }
}