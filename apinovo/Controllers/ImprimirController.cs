using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Web.Http.Cors;
using System.Web.Mvc;
using apinovo.Cors;

namespace apinovo.Controllers
{
    public class ImprimirController : Controller
    {


        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowCrossSite]
        public ActionResult ImprimirOs(string codigoOs)
        {

            var message = String.Empty;


            using (var rd = new ReportDocument())
            {

                //var local = HttpContext.Current.Server.MapPath(@"\\Rpt\\os.rpt");
                var local = Server.MapPath("../Rpt/os.rpt");

                if (! System.IO.File.Exists(local))
                {
                    local = @"D:\apiMidas\apiMIDAS\rpt\os.rpt";
                }

                rd.Load(local);
                rd.SetParameterValue("p1", codigoOs);


                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "os.pdf");
            }


        }

    }
}