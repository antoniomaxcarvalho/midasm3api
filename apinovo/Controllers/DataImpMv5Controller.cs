using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http.Cors;
using System.Web.Mvc;
//using Adm.Cors;

namespace apinovo.Controllers
{
    public class DataImpMv5Controller : Controller
    {
        [HttpGet]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[AllowCrossSite]
        public ActionResult ImprimirEstoque(string modelo)
        {
            // Quando for Criar uma Pasta para colocar o relatorios, será necessario configurar o IIS7
            // para poder ler a Pasta RPT.


            var message = String.Empty;


            using (var rd = new ReportDocument())
            {

                var local = Server.MapPath("../Rpt/EstoqueWeb.rpt");
                if (System.IO.File.Exists(local))
                {

                    switch (modelo)
                    {
                        case "G":
                            {
                                local = Server.MapPath("../Rpt/EstoqueWeb.rpt");
                                break;
                            }
                        case "S":
                            {
                                local = Server.MapPath("../Rpt/EstoqueWebSetorSintetico.rpt");
                                break;
                            }
                        case "F":
                            {
                                local = Server.MapPath("../Rpt/EstoqueWebFornecedor.rpt");
                                break;
                            }

                    }
                }





                rd.Load(local);




                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "pedido.pdf");

            }
        }

    }
}