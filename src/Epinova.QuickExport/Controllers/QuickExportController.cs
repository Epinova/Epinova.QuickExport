using System;
using System.IO;
using EPiServer.Enterprise;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Core.Transfer;
using EPiServer.Enterprise.Internal;
using EPiServer.Security;
using IOFile = System.IO.File;

namespace Epinova.QuickExport.Controllers
{
    [Authorize(Roles = "WebAdmins,Administrators")]
    public class QuickExportController : Controller
    {
        private const string FileName = "ExportedFile.episerverdata";


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
                        exporter.Stream = fileStream;
                        exporter.Export(options);
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