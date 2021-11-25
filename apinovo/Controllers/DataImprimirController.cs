//using CrystalDecisions.CrystalReports.Engine;
//using System.IO;
//using System.Web.Http;
//using System.Web.Mvc;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{

    public class DataImprimirController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage ImprimirOs(string codigoOs, string modelo, string valor, string semBdioS)
        {
            var c = 1;
            try
            {
                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;
                    var local = HttpContext.Current.Server.MapPath("~/rpt/os.rpt");

                    if (semBdioS == "S")
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osSemBdi.rpt");
                    }

                    if (Convert.ToDecimal(valor) == 0m)
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osSemValor.rpt");
                    }

                    //var filtro = " {tb_os_itens1.cancelado} <> 'S' ";

                    rd.Load(local);
                    rd.SetParameterValue("p1", codigoOs);
                    rd.SetParameterValue("p2", modelo);

                    //rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }


        [HttpGet]
        public HttpResponseMessage ImprimirOsItensImportados(string codigoOs, string modelo, string valor)
        {
            try
            {

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/osItemInformado.rpt");

                    if (Convert.ToDecimal(valor) == 0m)
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osSemValor.rpt");
                    }

                    //var filtro = " {tb_os_itens1.cancelado} <> 'S' ";

                    rd.Load(local);
                    rd.SetParameterValue("p1", codigoOs);
                    rd.SetParameterValue("p2", modelo);

                    //rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }


        //[HttpGet]
        //public HttpResponseMessage ImprimirOsTodas(string data1, string data2, Int64 autonumeroCliente, Int64 autonumeroPrioridade, Int64 autonumeroServico,
        //         Int64 autonumeroStatus, Int64 autonumeroUsuario, Int64 autonumeroPredio, Int64 autonumeroSetor, Int64 autonumeroLocalFisico, 
        //         Int64 autonumeroSistema, Int64 autonumeroSubSistema, Int64 autonumeroEquipe)

        [HttpPost]
        public HttpResponseMessage ImprimirOsTodas()

        {
            var message = String.Empty;

            try
            {

                DataOsController.AcertarValoresNaOs(string.Empty, 0);

                var vv = HttpContext.Current.Request.Form["autonumeroCliente"].ToString();

                var modelo = HttpContext.Current.Request.Form["modelo"].ToString();

                //modelo 
                //"C" -> S.S Completa
                //"R"->  S.S Resumida
                //"F"->  S.S Com Foto
                //"S"->  S.S Sem Valor
                //"X"->  S.S Cancelada
                //"L"->  S.S Por Com setor,status,local
                //"semMedicao"

                var data1 = HttpContext.Current.Request.Form["data1"].ToString();
                var data2 = HttpContext.Current.Request.Form["data2"].ToString();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var autonumeroPrioridade = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPrioridade"].ToString());
                var x = HttpContext.Current.Request.Form["autonumeroServico"].ToString();
                var autonumeroServico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroServico"].ToString());
                var nomeStatus = HttpContext.Current.Request.Form["nomeStatus"].ToString();
                var autonumeroUsuario = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                var autonumeroPredio = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                //var autonumeroLocalAtendido = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalAtendido"].ToString());
                var autonumeroSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                var autonumeroSubSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                var tipoVencto = HttpContext.Current.Request.Form["tipoVencto"].ToString();
                var obs = HttpContext.Current.Request.Form["obs"].ToString();
                var contrato = HttpContext.Current.Request.Form["contrato"].ToString();
                var semBdioS = HttpContext.Current.Request.Form["semBdioS"].ToString();
                //var autonumeroFuncionario = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString());

                var p1 = Convert.ToDateTime(data1).ToString("dd/MM/yyyy") +
                     @" Até " + Convert.ToDateTime(data2).ToString("dd/MM/yyyy");

                var campo = "{tb_os1.dataInicio}";
                if (tipoVencto.Equals("F")) // Data Emissao ou Fechamwnto
                {
                    nomeStatus = "Fechada";
                    campo = "{tb_os1.dataTermino}";
                }

                if (tipoVencto.Equals("S")) // Data Emissao ou Fechamwnto
                {
                    nomeStatus = "Fechada";
                    campo = "{tb_os1.dataSolicitacao}";
                }

                if (modelo.Equals(2))
                {
                    p1 = "Os No Intervalo : " + Convert.ToDateTime(data1).ToString("dd/MM/yyyy") +
                      @" Até " + Convert.ToDateTime(data2).ToString("dd/MM/yyyy");
                }


                var filtro = campo + " >= DATE (" +
                     Convert.ToDateTime(data1).Year.ToString("####") + ", " +
                     Convert.ToDateTime(data1).Month.ToString("####") + ", " +
                     Convert.ToDateTime(data1).Day.ToString("####") + ") AND " +
                     campo + " <= DATE (" +
                     Convert.ToDateTime(data2).Year.ToString("####") + ", " +
                     Convert.ToDateTime(data2).Month.ToString("####") + ", " +
                     Convert.ToDateTime(data2).Day.ToString("####") + ") ";

                if (modelo.Equals("X"))  // so as  cancelada
                {
                    filtro = filtro + " AND {tb_os1.cancelado} = 'S' ";
                }
                else
                {
                    filtro = filtro + " AND { tb_os1.cancelado} <> 'S' ";
                }

                if (modelo.Equals("C")) // Completa
                {
                    filtro = filtro + " AND { tb_os_itens1.cancelado} <> 'S'  ";
                }


                if (autonumeroCliente > 0) { filtro = filtro + " and {tb_os1.autonumeroCliente} = " + autonumeroCliente; }
                if (autonumeroPrioridade > 0) { filtro = filtro + " and {tb_os1.autonumeroPrioridade} = " + autonumeroPrioridade; }
                if (autonumeroServico > 0) { filtro = filtro + " and {tb_os1.autonumeroServico} = " + autonumeroServico; }
                if (autonumeroUsuario > 0) { filtro = filtro + " and {tb_os1.autonumeroUsuario} = " + autonumeroUsuario; }
                if (autonumeroPredio > 0) { filtro = filtro + " and {tb_os1.autonumeroPredio} = " + autonumeroPredio; }
                if (autonumeroSetor > 0) { filtro = filtro + " and {tb_os1.autonumeroSetor} = " + autonumeroSetor; }
                if (autonumeroLocalFisico > 0) { filtro = filtro + " and {tb_os1.autonumeroLocalFisico} = " + autonumeroLocalFisico; }
                if (autonumeroSistema > 0) { filtro = filtro + " and {tb_os1.autonumeroSistema} = " + autonumeroSistema; }
                if (autonumeroSubSistema > 0) { filtro = filtro + " and {tb_os1.autonumeroSubSistema} = " + autonumeroSubSistema; }

                if (!string.IsNullOrEmpty(nomeStatus))
                {
                    filtro = filtro + " and {tb_os1.nomeStatus} in '" + nomeStatus + "' ";
                }
                else
                {
                    nomeStatus = "Todas";
                }

                if (modelo.Equals("S")) // S.S Sem Valor
                {
                    filtro = "IF (not isnull({ tb_os_itens1.cancelado})) then " + filtro + " AND { tb_os_itens1.cancelado} <> 'S' ELSE " + filtro;
                }

                if (modelo.Equals("semMedicao")) // S.S Sem Medição
                {
                    filtro = filtro + " AND ( {tb_os1.etapa} = '' OR {tb_os1.medicao} = '' ) ";
                }


                //if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_os1.autonumeroEquipe} = " + autonumeroEquipe; }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/osTodas.rpt");

                    if (modelo.Equals("X"))
                    {

                        local = HttpContext.Current.Server.MapPath("~/rpt/osTodasCancelada.rpt");
                        //if (semBdioS == "S")
                        //{
                        //    local = HttpContext.Current.Server.MapPath("~/rpt/osComFiltrosemBdi.rpt");
                        //}
                    }
                    if (modelo.Equals("L"))
                    {

                        local = HttpContext.Current.Server.MapPath("~/rpt/osTodasComLocal.rpt");
                        //if (semBdioS == "S")
                        //{
                        //    local = HttpContext.Current.Server.MapPath("~/rpt/osComFiltrosemBdi.rpt");
                        //}
                    }
                    if (modelo.Equals("A"))
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osTodasAcompanhamento.rpt");
                        //filtro = filtro + " and { tb_os_acompanhamento1.cancelado} <> 'S' ";


                        //if (semBdioS == "S")
                        //{
                        //    local = HttpContext.Current.Server.MapPath("~/rpt/osComFiltrosemBdi.rpt");
                        //}
                    }
                    if (modelo.Equals("C"))
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osComFiltro.rpt");
                        if (semBdioS == "S")
                        {
                            local = HttpContext.Current.Server.MapPath("~/rpt/osComFiltrosemBdi.rpt");
                        }
                    }

                    if (modelo.Equals("S"))
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osValorZerado.rpt");
                    }

                    if (modelo.Equals("F")) // foto
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osFoto.rpt");
                    }

                    if (modelo.Equals("semMedicao")) // S.S Sem Medição
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/osSemMedicao.rpt");
                    }

                    rd.Load(local);

                    if (modelo == "F") // foto
                    {
                        filtro = filtro + " and ({tb_os1.url} <> '' or {tb_os1.url1} <> '') ";
                    }
                    else
                    {
                        rd.SetParameterValue("p1", p1);
                        rd.SetParameterValue("p2", filtro);

                        if (modelo.Equals("semMedicao")) // S.S Sem Medição
                        {
                            filtro = filtro + " AND ( {tb_os1.etapa} = '' OR {tb_os1.medicao} = '' ) ";
                        }
                        else
                        {
                            rd.SetParameterValue("p3", nomeStatus);
                            if (!(modelo.Equals("C") || modelo.Equals("S"))) // osTodas.rpt
                            {
                                rd.SetParameterValue("p4", obs);
                                rd.SetParameterValue("p5", contrato);

                            }
                        }
                    }
                    rd.RecordSelectionFormula = filtro;

                    //var outputpath = local;

                    //var stream = new FileStream(outputpath, FileMode.Open);

                    //var result = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new StreamContent(stream)
                    //};
                    //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    ////use attachment to force download
                    //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    //{
                    //    FileName = "myEticket.pdf"
                    //};
                    //return result;




                    var caminho = "~/UploadedFiles/planilhas";

                    // Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    //var extension = "xls";
                    //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    var fileName = "Planilha.xls";


                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }


                    // Declare variables and get the export options.
                    ExportOptions exportOpts = new ExportOptions();
                    ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                    DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    exportOpts = rd.ExportOptions;

                    // Set the excel format options.
                    excelFormatOpts.ExcelUseConstantColumnWidth = true;
                    excelFormatOpts.ExcelTabHasColumnHeadings = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;
                    exportOpts.FormatOptions = excelFormatOpts;

                    // Set the disk file options and export.
                    exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    diskOpts.DiskFileName = fileSavePath;
                    exportOpts.DestinationOptions = diskOpts;

                    rd.Export();




                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;




        }


        [HttpPost]
        public void EnviarEmail()
        {
            var endFrom = HttpContext.Current.Request.Form["endFrom"].ToString().Trim();
            var endNomeFrom = HttpContext.Current.Request.Form["endNomeFrom"].ToString().Trim();

            //var endTo = HttpContext.Current.Request.Form["endTo"].ToString();
            //var endNomeTo = HttpContext.Current.Request.Form["endNomeTo"].ToString().Trim();

            var Subject = HttpContext.Current.Request.Form["Subject"].ToString().Trim();

            var nroOs = HttpContext.Current.Request.Form["nroOs"].ToString().Trim();
            var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
            var descricaoOs = HttpContext.Current.Request.Form["descricaoOs"].ToString().Trim();

            var siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString().Trim();
            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();

            string htmlBody = string.Empty;
            //using streamreader for reading my htmltemplate   

            var local = HttpContext.Current.Server.MapPath("~/rpt/m3Email.html");

            using (StreamReader reader = new StreamReader(local))
            {
                htmlBody = reader.ReadToEnd();
            }

            htmlBody = htmlBody.Replace(System.Environment.NewLine, "");
            htmlBody = Regex.Replace(htmlBody, @"\t", "");

            htmlBody = Regex.Replace(htmlBody, @"\t", "");
            htmlBody = Regex.Replace(htmlBody, "@nroOs", nroOs);
            htmlBody = Regex.Replace(htmlBody, "@nomeUsuario", nomeUsuario);
            htmlBody = Regex.Replace(htmlBody, "@descricaoOs", descricaoOs);
            htmlBody = Regex.Replace(htmlBody, "@nomeCliente", nomeCliente);

            // -- Incluir Logo  <img src=\"cid:Pic1\"> -> Marcacao do lugar do Logo dentro do Arq.html   ------------------------------------------------------
            //
            //AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            //    (htmlBody, null, MediaTypeNames.Text.Html);

            // // Create a LinkedResource object for each embedded image
            // LinkedResource pic1 = new LinkedResource(@"C:\SistemasWEB\MidasM3Api\apinovo\apinovo\rpt\logo.png", MediaTypeNames.Image.Jpeg);
            // pic1.ContentId = "Pic1";
            // avHtml.LinkedResources.Add(pic1);


            // Add the alternate views instead of using MailMessage.Body
            // var m = new System.Net.Mail.MailMessage();
            //m.AlternateViews.Add(avHtml);

            // -- FIM Incluir Logo ------------------------------------------------------------------------------------------------------------------------------

            // Sem Incluir Logo -------------------------------------------------------------
            var m = new System.Net.Mail.MailMessage();
            m.Body = htmlBody;
            m.IsBodyHtml = true;
            // Fim Sem Incluir Logo -------------------------------------------------------------

            // Address and send the message
            m.From = new MailAddress(endFrom, endNomeFrom);

            m.Subject = Subject;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";

            smtp.EnableSsl = true;

            NetworkCredential NetworkCred = new System.Net.NetworkCredential();

            //NetworkCred.UserName = "antoniomaxcarvalho@gmail.com";
            //NetworkCred.Password = "xyufxyyiqalpfpcp"; // antonio max

            NetworkCred.UserName = "m3programa01@gmail.com";
            NetworkCred.Password = "bmuppdyxiqvzqsse"; // m3

            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;

            var itens = new DataUsuarioController();
            var p = itens.GetUsuarioCliente(siglaCliente);
            if (p != null)
            {
                foreach (var value in p)
                {
                    if (IsValidEmail(value.login))
                    {
                        m.To.Add(new MailAddress(value.login, value.nome));
                        smtp.Send(m);
                    }

                }

            }

        }

        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }


        [HttpPost]
        public HttpResponseMessage ImprimirEquipamentos()

        {
            var message = String.Empty;

            try
            {
                var tipoData = HttpContext.Current.Request.Form["tipoData"].ToString().Trim();
                var data1 = HttpContext.Current.Request.Form["data1"].ToString().Trim();
                var data2 = HttpContext.Current.Request.Form["data2"].ToString();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var autonumeroPredio = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var autonumeroLocalAtendido = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalAtendido"].ToString());
                var autonumeroSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                var autonumeroSubSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                var autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString());


                var p1 = string.Empty;

                var filtro = "{tb_cadastro1.cancelado} <> 'S'  ";

                var campo = "";
                var nomeStatus = "";
                if (tipoData.Equals("C"))
                {
                    nomeStatus = "Compra";
                    campo = "{tb_cadastro1.dataCompra}";
                }
                if (tipoData.Equals("G"))
                {
                    nomeStatus = "Garantia";
                    campo = "{tb_cadastro1.dataGarantia}";
                }
                if (tipoData.Equals("P"))
                {
                    nomeStatus = "Prevista";
                    campo = "{tb_cadastro1.dataPrevista}";
                }




                if (!string.IsNullOrEmpty(nomeStatus))
                {

                    if (!string.IsNullOrEmpty(data1))
                    {
                        filtro = filtro + " and " + campo + " >= DATE (" +
                                 Convert.ToDateTime(data1).Year.ToString("####") + ", " +
                                 Convert.ToDateTime(data1).Month.ToString("####") + ", " +
                                 Convert.ToDateTime(data1).Day.ToString("####") + ") AND " +
                                 campo + " <= DATE (" +
                                 Convert.ToDateTime(data2).Year.ToString("####") + ", " +
                                 Convert.ToDateTime(data2).Month.ToString("####") + ", " +
                                 Convert.ToDateTime(data2).Day.ToString("####") + ")  ";

                        p1 = "Equipamento No Intervalo *" + nomeStatus + "* : " + Convert.ToDateTime(data1).ToString("dd/MM/yyyy") +
                        @" Até " + Convert.ToDateTime(data2).ToString("dd/MM/yyyy");

                    }
                }

                if (autonumeroCliente > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroCliente} = " + autonumeroCliente; }
                if (autonumeroPredio > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroPredio} = " + autonumeroPredio; }
                if (autonumeroSetor > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSetor} = " + autonumeroSetor; }
                if (autonumeroLocalFisico > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroLocalFisico} = " + autonumeroLocalFisico; }
                if (autonumeroLocalAtendido > 0) { filtro = filtro + " and {tb_cadastro1.local} = " + autonumeroLocalAtendido; }
                if (autonumeroSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSistema} = " + autonumeroSistema; }
                if (autonumeroSubSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSubSistema} = " + autonumeroSubSistema; }
                if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }
                //if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    var local = HttpContext.Current.Server.MapPath("~/rpt/componente.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpPost]
        public HttpResponseMessage ImprimirFotoOs()

        {
            var message = String.Empty;

            try
            {
                var p1 = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/osFoto.rpt");

                    rd.Load(local);

                    //rd.SetParameterValue("p1", p1);

                    rd.RecordSelectionFormula = " {tb_os1.codigoOs} = '" + p1.Trim() + "'";

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpPost]
        public HttpResponseMessage ImprimirEstatistica()

        {
            var message = String.Empty;

            try
            {

                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString().Trim());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                var osFechada = HttpContext.Current.Request.Form["osFechada"].ToString();

                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString();
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString();

                var p1 = @"Data *ABERTURA* : " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                if (osFechada == "S")
                {
                    p1 = @"Data *FECHAMENTO*: " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                }

                var d1 = new SqlParameter("@d1", System.Data.SqlDbType.DateTime)
                {
                    Value = data1
                };

                var d2 = new SqlParameter("@d2", System.Data.SqlDbType.DateTime)
                {
                    Value = data2
                };

                if (autonumeroCliente > 0)
                {
                    var auto = new SqlParameter("@auto", System.Data.SqlDbType.BigInt)
                    {
                        Value = autonumeroCliente
                    };
                    p1 = p1 + " - " + nomeCliente;
                }



                if (!string.IsNullOrEmpty(nomeSetor))
                {
                    p1 = string.Concat(p1, "\r\n", "Setor: ", nomeSetor);
                }

                if (!string.IsNullOrEmpty(nomeLocalFisico))
                {
                    p1 = string.Concat(p1, "\r\n", "Local: ", nomeLocalFisico);
                }

                var csql = new StringBuilder();

                csql.Append("DROP TABLE IF EXISTS `tb_fechamento`; ");
                csql.Append("CREATE table tb_fechamento( ");
                csql.Append("SELECT nomeSistema, coalesce(sum(nomeStatus = 'Aberta'), 0) aberta, ");
                csql.Append("coalesce(sum(nomeStatus = 'Fechada'), 0)  fechada, ");
                csql.Append("coalesce(sum(nomeStatus = 'Autorizado'), 0)  autorizado, ");
                csql.Append("coalesce(sum(nomeStatus = 'Rejeitado'), 0)  rejeitado, ");
                csql.Append("coalesce(sum(nomeStatus = 'O.S. Medida'), 0)  osMedida, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera material/mo'), 0)  esperaMaterialMo, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera Fiscal'), 0)  esperaFiscal, ");
                csql.Append("coalesce(sum(nomeStatus != 'xxxx'), 0) total ");

                if (osFechada == "S")
                {
                    if (autonumeroCliente > 0)
                    {
                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2}  group by nomeSistema) ");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor > 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} and autonumeroSetor = {3} group by nomeSistema)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroSetor });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} and autonumeroLocalFisico = {3} group by nomeSistema)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1}  group by nomeSistema)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }
                else
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2}  group by nomeSistema) ");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor > 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} and autonumeroSetor = {3} group by nomeSistema)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroSetor });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} and autonumeroLocalFisico = {3} group by nomeSistema)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1}  group by nomeSistema)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }


                }

                var obra = String.Empty;
                var processo = String.Empty;

                if (autonumeroCliente > 0)
                {
                    var cl = new DataClienteController();
                    var cliente = cl.GetCliente((int)autonumeroCliente);
                    foreach (var item in cliente)
                    {
                        obra = string.Concat("OBRA: ", item.obra);
                        processo = string.Concat("PROCESSO: ", item.processo);
                    }
                }


                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/estatistica.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1);
                    rd.SetParameterValue("@obra", obra);
                    rd.SetParameterValue("@processo", processo);

                    rd.RecordSelectionFormula = string.Empty;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;

        }

        [HttpPost]
        public HttpResponseMessage ImprimirEstatisticaSetor()

        {
            var message = String.Empty;

            try
            {

                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString().Trim());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                var osFechada = HttpContext.Current.Request.Form["osFechada"].ToString();


                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString();
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString();


                var p1 = @"Data *ABERTURA* : " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                if (osFechada == "S")
                {
                    p1 = @"Data *FECHAMENTO*: " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                }

                var d1 = new SqlParameter("@d1", System.Data.SqlDbType.DateTime)
                {
                    Value = data1
                };

                var d2 = new SqlParameter("@d2", System.Data.SqlDbType.DateTime)
                {
                    Value = data2
                };
                if (autonumeroCliente > 0)
                {
                    var auto = new SqlParameter("@auto", System.Data.SqlDbType.BigInt)
                    {
                        Value = autonumeroCliente
                    };
                    p1 = p1 + " - " + nomeCliente;
                }

                p1 = string.Concat(p1, " - Por SETOR");

                if (!string.IsNullOrEmpty(nomeSetor))
                {
                    p1 = string.Concat(p1, "\r\n", "Setor: ", nomeSetor);
                }


                var csql = new StringBuilder();

                csql.Append("DROP TABLE IF EXISTS `tb_fechamento`; ");
                csql.Append("CREATE table tb_fechamento( ");
                csql.Append("SELECT nomeSetor, coalesce(sum(nomeStatus = 'Aberta'), 0) aberta, ");
                csql.Append("coalesce(sum(nomeStatus = 'Fechada'), 0)  fechada, ");
                csql.Append("coalesce(sum(nomeStatus = 'Autorizado'), 0)  autorizado, ");
                csql.Append("coalesce(sum(nomeStatus = 'Rejeitado'), 0)  rejeitado, ");
                csql.Append("coalesce(sum(nomeStatus = 'O.S. Medida'), 0)  osMedida, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera material/mo'), 0)  esperaMaterialMo, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera Fiscal'), 0)  esperaFiscal, ");
                csql.Append("coalesce(sum(nomeStatus != 'xxxx'), 0) total ");

                if (osFechada == "S")
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} group by nomeSetor)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor > 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} and autonumeroSetor = {3} group by nomeSetor)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroSetor });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1}  group by nomeSetor)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }
                else
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} group by nomeSetor)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor > 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} and autonumeroSetor = {3} group by nomeSetor)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroSetor });
                            }
                        }


                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1}  group by nomeSetor)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/estatisticaSetor.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1.Trim());

                    rd.RecordSelectionFormula = string.Empty;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }


            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpPost]
        public HttpResponseMessage ImprimirEstatisticaLocalFisico()

        {
            var message = String.Empty;

            try
            {

                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString().Trim());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                var osFechada = HttpContext.Current.Request.Form["osFechada"].ToString();


                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString();
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString();


                var p1 = @"Data *ABERTURA* : " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                if (osFechada == "S")
                {
                    p1 = @"Data *FECHAMENTO*: " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                }

                var d1 = new SqlParameter("@d1", System.Data.SqlDbType.DateTime)
                {
                    Value = data1
                };

                var d2 = new SqlParameter("@d2", System.Data.SqlDbType.DateTime)
                {
                    Value = data2
                };
                if (autonumeroCliente > 0)
                {
                    var auto = new SqlParameter("@auto", System.Data.SqlDbType.BigInt)
                    {
                        Value = autonumeroCliente
                    };
                    p1 = p1 + " - " + nomeCliente;
                }

                p1 = string.Concat(p1, " - Por LOCAL");

                if (!string.IsNullOrEmpty(nomeLocalFisico))
                {
                    p1 = string.Concat(p1, "\r\n", "Local: ", nomeLocalFisico);
                }


                var csql = new StringBuilder();

                csql.Append("DROP TABLE IF EXISTS `tb_fechamento`; ");
                csql.Append("CREATE table tb_fechamento( ");
                csql.Append("SELECT nomeLocalFisico as nomeSetor, coalesce(sum(nomeStatus = 'Aberta'), 0) aberta, ");
                csql.Append("coalesce(sum(nomeStatus = 'Fechada'), 0)  fechada, ");
                csql.Append("coalesce(sum(nomeStatus = 'Autorizado'), 0)  autorizado, ");
                csql.Append("coalesce(sum(nomeStatus = 'Rejeitado'), 0)  rejeitado, ");
                csql.Append("coalesce(sum(nomeStatus = 'O.S. Medida'), 0)  osMedida, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera material/mo'), 0)  esperaMaterialMo, ");
                csql.Append("coalesce(sum(nomeStatus = 'Espera Fiscal'), 0)  esperaFiscal, ");
                csql.Append("coalesce(sum(nomeStatus != 'xxxx'), 0) total ");

                if (osFechada == "S")
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} group by nomeLocalFisico)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and autonumeroCliente = {2} and autonumeroLocalFisico = {3} group by nomeLocalFisico)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1}  group by nomeLocalFisico)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }
                else
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} group by nomeLocalFisico)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("FROM tb_os WHERE cancelado != 'S' and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} and autonumeroLocalFisico = {3} group by nomeLocalFisico)");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("FROM tb_os WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1}  group by nomeLocalFisico)");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/estatisticaSetor.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1.Trim());

                    rd.RecordSelectionFormula = string.Empty;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }


            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpPost]
        public HttpResponseMessage ImprimirEstatisticaSetorHora()

        {
            var message = String.Empty;

            try
            {

                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString().Trim());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();

                var p1 = @"Data *Fechamento* : " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");

                p1 = @"Data *FECHAMENTO*: " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/estatisticaSetorHora.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1);
                    rd.SetParameterValue("@nomeCliente", nomeCliente);
                    rd.SetParameterValue("@autonumeroCliente", autonumeroCliente);
                    rd.SetParameterValue("@data1", data1);
                    rd.SetParameterValue("@data2", data2);

                    rd.RecordSelectionFormula = string.Empty;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }


        [HttpPost]
        public HttpResponseMessage ImprimirOsPMOC()

        {
            var message = String.Empty;

            try
            {
                var tipoData = HttpContext.Current.Request.Form["tipoData"].ToString().Trim();
                var data1 = HttpContext.Current.Request.Form["data1"].ToString().Trim();
                var data2 = HttpContext.Current.Request.Form["data2"].ToString();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var autonumeroPredio = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var autonumeroLocalAtendido = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalAtendido"].ToString());
                var autonumeroSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                var autonumeroSubSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                var autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString());

                var p1 = string.Empty;
                var p2 = string.Empty;

                var filtro = "{tb_cadastro1.cancelado} <> 'S'  ";

                var campo = "";
                var nomeStatus = "";
                if (tipoData.Equals("C"))
                {
                    nomeStatus = "Compra";
                    campo = "{tb_cadastro1.dataCompra} ";
                }
                if (tipoData.Equals("G"))
                {
                    nomeStatus = "Garantia";
                    campo = "{tb_cadastro1.dataGarantia} ";
                }
                if (tipoData.Equals("P"))
                {
                    nomeStatus = "Prevista";
                    campo = "{tb_cadastro1.dataPrevista} ";
                }

                if (!string.IsNullOrEmpty(nomeStatus))
                {

                    if (!string.IsNullOrEmpty(data1))
                    {
                        filtro = filtro + " and " + campo + " >= DATE (" +
                                 Convert.ToDateTime(data1).Year.ToString("####") + ", " +
                                 Convert.ToDateTime(data1).Month.ToString("####") + ", " +
                                 Convert.ToDateTime(data1).Day.ToString("####") + ") AND " +
                                 campo + " <= DATE (" +
                                 Convert.ToDateTime(data2).Year.ToString("####") + ", " +
                                 Convert.ToDateTime(data2).Month.ToString("####") + ", " +
                                 Convert.ToDateTime(data2).Day.ToString("####") + ")  ";
                        //p1 = "Equipamento No Intervalo *" + nomeStatus + "* : " + Convert.ToDateTime(data1).ToString("dd/MM/yyyy") +
                        //@" Até " + Convert.ToDateTime(data2).ToString("dd/MM/yyyy");

                    }
                }

                if (autonumeroCliente > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroCliente} = " + autonumeroCliente; }
                if (autonumeroPredio > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroPredio} = " + autonumeroPredio; }
                if (autonumeroSetor > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSetor} = " + autonumeroSetor; }
                if (autonumeroLocalFisico > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroLocalFisico} = " + autonumeroLocalFisico; }
                if (autonumeroLocalAtendido > 0) { filtro = filtro + " and {tb_cadastro1.local} = " + autonumeroLocalAtendido; }
                if (autonumeroSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSistema} = " + autonumeroSistema; }
                if (autonumeroSubSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSubSistema} = " + autonumeroSubSistema; }
                if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }
                //if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }


                using (var dc = new manutEntities())
                {
                    using (var rd = new ReportDocument())
                    {
                        var Response = HttpContext.Current.ApplicationInstance.Response;

                        // PEGAR o relatorio ----------------------------------------
                        var local = ArquivoRelatorio(autonumeroSubSistema);
                        // FIM - PEGAR o relatorio ----------------------------------

                        rd.Load(local);

                        //rd.SetParameterValue("@parametro1", p1);
                        //rd.SetParameterValue("@parametro2", p2);

                        rd.RecordSelectionFormula = filtro;

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        ////75 is my print job limit.
                        //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                        //return CreateReport(reportClass);

                        rd.Close();
                        rd.Dispose();

                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                        resp.Content = new StreamContent(stream);
                        return resp;
                    }

                }


            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;





        }

        [HttpPost]
        public HttpResponseMessage ImprimirPMOCDepoisCalcular()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

                var autonumeroSubSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());

                var p1 = string.Empty;
                var p2 = string.Empty;

                var filtro = "{tb_cadastro1.cancelado} <> 'S' and not IsNULL({tb_cadastro1.dataPrevista}) ";

                if (autonumeroCliente > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroCliente} = " + autonumeroCliente; }
                if (autonumeroSubSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSubSistema} = " + autonumeroSubSistema; }

                //if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }


                using (var dc = new manutEntities())
                {
                    using (var rd = new ReportDocument())
                    {
                        var Response = HttpContext.Current.ApplicationInstance.Response;

                        // PEGAR o relatorio ----------------------------------------
                        var local = ArquivoRelatorio(autonumeroSubSistema);
                        // FIM - PEGAR o relatorio ----------------------------------

                        if (string.IsNullOrEmpty(local))
                        {
                            message = string.Format("Não foi possível encontrar o Relatorio para o SubSistema: " + autonumeroSubSistema);
                            HttpError err = new HttpError(message);
                            return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        }
                        rd.Load(local);

                        //rd.SetParameterValue("@parametro1", p1);
                        //rd.SetParameterValue("@parametro2", p2);

                        rd.RecordSelectionFormula = filtro;

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        ////75 is my print job limit.
                        //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                        //return CreateReport(reportClass);

                        rd.Close();
                        rd.Dispose();

                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                        resp.Content = new StreamContent(stream);
                        return resp;
                    }

                }


            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }

            return null;

        }


        [HttpPost]
        public HttpResponseMessage ImprimirPMOCComCheckList()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

                var autonumeroSubSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());

                var p1 = string.Empty;
                var p2 = string.Empty;

                var filtro = "{tb_cadastro1.cancelado} <> 'S' and not IsNULL({tb_cadastro1.dataPrevista}) ";

                if (autonumeroCliente > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroCliente} = " + autonumeroCliente; }
                if (autonumeroSubSistema > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroSubSistema} = " + autonumeroSubSistema; }

                //if (autonumeroEquipe > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroEquipe} = " + autonumeroEquipe; }


                using (var dc = new manutEntities())
                {
                    using (var rd = new ReportDocument())
                    {
                        var Response = HttpContext.Current.ApplicationInstance.Response;

                        // PEGAR o relatorio ----------------------------------------

                        var local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamento.rpt");
                        // FIM - PEGAR o relatorio ----------------------------------

                        //if (string.IsNullOrEmpty(local))
                        //{
                        //    message = string.Format("Não foi possível encontrar o Relatorio para o SubSistema: " + autonumeroSubSistema);
                        //    HttpError err = new HttpError(message);
                        //    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                        //}
                        rd.Load(local);

                        //rd.SetParameterValue("@parametro1", p1);
                        //rd.SetParameterValue("@parametro2", p2);

                        rd.RecordSelectionFormula = filtro;

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        ////75 is my print job limit.
                        //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                        //return CreateReport(reportClass);

                        rd.Close();
                        rd.Dispose();

                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                        resp.Content = new StreamContent(stream);
                        return resp;
                    }

                }


            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }

            return null;

        }




        private string ArquivoRelatorio(Int64? autonumeroSubSistema)
        {

            var local = string.Empty;

            switch (autonumeroSubSistema)
            {
                case 53: /*Ar Condicionado de parede*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/arParede.rpt");
                    break;
                case 54: /*Ar Portatil */
                    local = HttpContext.Current.Server.MapPath("~/rpt/arPortatil.rpt");
                    break;
                case 125: /*Bebedouro */
                    local = HttpContext.Current.Server.MapPath("~/rpt/bebedouro.rpt");
                    break;
                case 44: /*Bomba */
                    local = HttpContext.Current.Server.MapPath("~/rpt/Bomba.rpt");
                    break;


                case 139: /*Bomba de agua de condensação */
                    local = HttpContext.Current.Server.MapPath("~/rpt/BombaAguaCondensacao.rpt");
                    break;
                case 140: /*Bomba de agua de gelada */
                    local = HttpContext.Current.Server.MapPath("~/rpt/BombaAguaGelada.rpt");
                    break;



                case 122: /*Camara Frigorífica */
                    local = HttpContext.Current.Server.MapPath("~/rpt/camaraEBalcaoFrigorifico.rpt");
                    break;


                case 148: /*Cassete */
                    local = HttpContext.Current.Server.MapPath("~/rpt/cassete.rpt");
                    break;



                case 50: /*Centrífuga */
                    local = HttpContext.Current.Server.MapPath("~/rpt/centrifuga.rpt");
                    break;
                case 41: /*Chiller Elétrico a ar*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/chiller.rpt");
                    break;
                case 137: /*Chiller Elétrico a Agua*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/chiller.rpt");
                    break;
                case 124: /*Coifa e exaustor de cozinha*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/coifaExautorCozinha.rpt");
                    break;
                case 126: /*Cortina de ar*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/cortinaDeAr.rpt");
                    break;
                case 51: /*Exaustor de ar*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/cortinaDeAr.rpt");
                    break;
                case 43: /*Fancoil*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/fancoil.rpt");
                    break;
                case 127: /*Freezer*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/Freezer.rpt");
                    break;
                case 128: /*frigobar*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/frigobar.rpt");
                    break;
                case 129: /*geladeira*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/geladeira.rpt");
                    break;
                case 37: /*gerador*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/gerador.rpt");
                    break;
                case 131: /*Maquina de gelo*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/maquinaDeGelo.rpt");
                    break;
                case 130: /*Purificador de agua*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/purificadorDeAgua.rpt");
                    break;
                case 49: /*Roof Top*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/roofTop.rpt");
                    break;




                case 149: /*Refresqueira */
                    local = HttpContext.Current.Server.MapPath("~/rpt/refresqueira.rpt");
                    break;



                case 136: /*Self Contained Condensador Incorporado*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/selfContained.rpt");
                    break;
                case 145: /*Self Contained Condensador Incorporado*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/selfContainedRemoto.rpt");
                    break;
                case 52: /*Split System*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/spltSystem.rpt");
                    break;
                case 147: /*Split System VRF*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/spltSystemVRF.rpt");
                    break;
                case 42: /*torre*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/torre.rpt");
                    break;
                case 38: /*transformador*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/transformador.rpt");
                    break;
                case 47: /*Ventilador doméstico*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/ventiladorDeAr.rpt");
                    break;
                case 46: /*Ventilador de parede*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/ventiladorDeParede.rpt");
                    break;
                case 45: /*ventiladorDeTeto*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/ventiladorDeTeto.rpt");
                    break;
                case 48: /*ventiladorDeTeto*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/ventiladorDomestico.rpt");
                    break;
                case 141: /*camara mortuaria*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/camaraMortuaria.rpt");
                    break;
                /* ----------------------------------------------------------------------------------- */
                case 17: /*interiores*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/interiores.rpt");
                    break;
                case 19: /*subestacao*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/subEstacao.rpt");
                    break;
                case 61: /*Bomba hidraulica */
                    local = HttpContext.Current.Server.MapPath("~/rpt/bombaHidraulica.rpt");
                    break;
                case 75: /*Bombas*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/bombas.rpt");
                    break;
                case 72: /*Reservatórios de agua */
                    local = HttpContext.Current.Server.MapPath("~/rpt/reservatorioAgua.rpt");
                    break;
                case 20: /*Quadros elétricos*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/quadroEletrico.rpt");
                    break;
                case 33: /*Quadros Luz*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/quadroDeLuz.rpt");
                    break;
                case 34: /*Quadros Forca*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/quadroForca.rpt");
                    break;
                case 35: /*Quadros Luz e Forcas*/
                    local = HttpContext.Current.Server.MapPath("~/rpt/quadroLuzEForca.rpt");
                    break;


                default:
                    break;
            }

            return local;

        }

        [HttpGet]
        public HttpResponseMessage ImprimirOrcamento(string autonumero, string comPlanilhaFechada)
        {
            try
            {
                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/orcamento.rpt");


                    if (comPlanilhaFechada == "S")
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/orcamentoEPlanilhaFechada.rpt");
                    }

                    var filtro = "{tb_orcamento_itens1.cancelado} <> 'S' and  {tb_orcamento1.autonumero} = " + autonumero;

                    rd.Load(local);
                    //rd.SetParameterValue("p1", codigoOs);
                    //rd.SetParameterValue("p2", modelo);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }

        [HttpGet]
        public HttpResponseMessage ImprimirItensCompatibilizado(string dataInicio, string dataFim, Int64 autonumeroCliente,
                                   Int64 autonumeroSistema, Int32 autonumeroServico)
        {
            try
            {

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/itensCompatibilizado.rpt");
                    var tipoServico = "CORRETIVO";
                    if (autonumeroServico == 1)
                    {
                        tipoServico = "PREVENTIVO";
                    }
                    if (autonumeroServico == 2)
                    {
                        tipoServico = "CORRETIVO";
                    }

                    var servico = "";
                    if (autonumeroServico > 0)
                    {
                        servico = " and { tb_os1.autonumeroServico} = " + autonumeroServico;
                    }

                    var campo = "{tb_os1.dataTermino}";

                    var filtro = campo + " >= DATE (" +
                                     Convert.ToDateTime(dataInicio).Year.ToString("####") + ", " +
                                     Convert.ToDateTime(dataInicio).Month.ToString("####") + ", " +
                                     Convert.ToDateTime(dataInicio).Day.ToString("####") + ") AND " +
                                     campo + " <= DATE (" +
                                     Convert.ToDateTime(dataFim).Year.ToString("####") + ", " +
                                     Convert.ToDateTime(dataFim).Month.ToString("####") + ", " +
                                     Convert.ToDateTime(dataFim).Day.ToString("####") + ") AND {tb_os1.cancelado} <> 'S' ";

                    filtro = filtro + "AND {tb_os1.nomeStatus} = 'Fechada' and {tb_os1.cancelado} <> 'S' and {tb_os_itens1.cancelado} <> 'S' and  {tb_os1.autonumeroCliente} = " + autonumeroCliente + " and {tb_os1.autonumeroSistema} = " + autonumeroSistema + servico;

                    var intervalo = "PERÍODO DE " + dataInicio + " ATÉ " + dataFim;

                    rd.Load(local);
                    rd.SetParameterValue("p1", intervalo);
                    rd.SetParameterValue("p2", tipoServico);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);
                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }

        [HttpPost]
        public HttpResponseMessage ImprimirPlanilhaFechada()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());


                using (var dc = new manutEntities())
                {
                    using (var rd = new ReportDocument())
                    {
                        var Response = HttpContext.Current.ApplicationInstance.Response;

                        var local = HttpContext.Current.Server.MapPath("~/rpt/planilhaFechada.rpt");
                        rd.Load(local);

                        //rd.SetParameterValue("@parametro1", p1);
                        //rd.SetParameterValue("@parametro2", p2);

                        var filtro = " {tb_planilhafechada1.autonumeroCliente} = " + autonumeroCliente;

                        rd.RecordSelectionFormula = filtro;

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        ////75 is my print job limit.
                        //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                        //return CreateReport(reportClass);

                        rd.Close();
                        rd.Dispose();

                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                        resp.Content = new StreamContent(stream);
                        return resp;
                    }

                }

            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, ex.InnerException.ToString().Length);
                }
                message = message + ex.Message + " ---- " + c;
                message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, ex.InnerException.ToString().Length);
                }
                message = message + ex.Message + " ---- " + c;
                message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                message = ex.InnerException != null ? ex.InnerException.ToString().Substring(0, ex.InnerException.ToString().Length) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                message = ex.InnerException != null ? ex.InnerException.ToString().Substring(0, ex.InnerException.ToString().Length) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            //return null;

            HttpResponseMessage mes = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            mes.Content = new StringContent(message);
            throw new HttpResponseException(mes);





        }

        [HttpPost]
        public HttpResponseMessage ImprimirOrdemServico()

        {
            var message = String.Empty;

            try
            {
                var codigoOrdemServico = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString().Trim());
                var custoFinal = HttpContext.Current.Request.Form["custoFinal"].ToString().Trim();

                int qtde = 0;
                int qtdeItens = 0;
                var valorOs = 0m;

                using (var dc = new manutEntities())
                {
                    var ordem = (from p in dc.tb_os.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.codigoOrdemServico == codigoOrdemServico.Trim())
                                 select p).ToList();

                    qtde = ordem.Count();
                    //Debug.WriteLine(qtde);
                    valorOs = Convert.ToDecimal((from p in dc.tb_os.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.codigoOrdemServico == codigoOrdemServico.Trim())
                                                 select p).ToList().Sum(p => p.valor));
                }

                //Debug.WriteLine(valorOs);

                using (var dc = new manutEntities())
                {
                    var ordem = (from p in dc.tb_os_itens.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.codigoOrdemServico == codigoOrdemServico.Trim())
                                 select p).ToList();

                    qtdeItens = ordem.Count();

                }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/ordemServico.rpt");

                    if (custoFinal == "S")
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/oServicoCFinal.rpt");

                        //if (qtdeItens > 30)
                        //{
                        local = HttpContext.Current.Server.MapPath("~/rpt/oServicoCFinalMuitosItens.rpt");
                        //}

                    }
                    if (custoFinal == "F")
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/oServicoCFinalComQtdeMaoObra.rpt");
                    }

                    if (valorOs > 0)
                    {
                        if (qtde > 27)
                        {
                            local = HttpContext.Current.Server.MapPath("~/rpt/ordemServicoComVariasSS.rpt");

                            if (custoFinal == "S")
                            {
                                local = HttpContext.Current.Server.MapPath("~/rpt/oServicoComVariasSSCFinal.rpt");
                            }


                            if (custoFinal == "F")
                            {
                                local = HttpContext.Current.Server.MapPath("~/rpt/ordemSComVariasSSComMaoObra.rpt");
                            }

                        }

                        var xx = " {tb_os_itens1.quantidadePF} > 0 and {tb_ordemservico1.cancelado}  <> 'S'  and {tb_os_itens1.cancelado} <> 'S' and  {tb_ordemservico1.codigoOs} = '" + codigoOrdemServico.Trim() + "' and {tb_ordemservico1.autonumeroCliente} = " + autonumeroCliente.ToString();

                        //Debug.WriteLine(xx);

                        rd.Load(local);

                        rd.SetParameterValue("p1", qtde.ToString());
                        //rd.OpenSubreport("SCO").RecordSelectionFormula = "";

                        //*Debug.WriteLine(local);
                        rd.RecordSelectionFormula = " {tb_os_itens1.quantidadePF} > 0 and {tb_ordemservico1.cancelado}  <> 'S'  and {tb_os_itens1.cancelado} <> 'S' and  {tb_ordemservico1.codigoOs} = '" + codigoOrdemServico.Trim() + "' and {tb_ordemservico1.autonumeroCliente} = " + autonumeroCliente.ToString();

                        //  var filtroDoSubReport = "{tb_os_itens1.autonumeroCliente} = {?Pm-tb_ordemservico1.autonumeroCliente} and " +
                        //" { tb_os_itens1.codigoOrdemServico} = {?Pm-tb_os_itens1.codigoOrdemServico} " +
                        //" and { tb_os_itens1.codigoInsumoServico} <> { tb_os_itens1.codigoPF} and { tb_os_itens1.cancelado} <> 'S' and { tb_os_itens1.totalPF} > 0 ";




                    }
                    else
                    {
                        local = HttpContext.Current.Server.MapPath("~/rpt/ordemServicoZerada.rpt");
                        rd.Load(local);

                        rd.SetParameterValue("p1", qtde.ToString());

                        var xxx = "  { tb_ordemservico1.cancelado }  <> 'S' and  { tb_ordemservico1.codigoOs} = '" + codigoOrdemServico.Trim() + "' and { tb_ordemservico1.autonumeroCliente} = " + autonumeroCliente.ToString();
                        //Debug.WriteLine(xxx);
                        //Debug.WriteLine(local);
                        rd.RecordSelectionFormula = " {tb_ordemservico1.cancelado}  <> 'S' and  {tb_ordemservico1.codigoOs} = '" + codigoOrdemServico.Trim() + "' and {tb_ordemservico1.autonumeroCliente} = " + autonumeroCliente.ToString();


                    }
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }

            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpPost]
        public HttpResponseMessage ImprimirCorretivas()

        {
            var message = String.Empty;

            try
            {

                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString().Trim());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                var osFechada = HttpContext.Current.Request.Form["osFechada"].ToString();


                var autonumeroSetor = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString();
                var autonumeroLocalFisico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                var nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString();

                var sistemaOrcamento = HttpContext.Current.Request.Form["sistemaOrcamento"].ToString();

                var p1 = @"Data *ABERTURA* : " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                if (osFechada == "S")
                {
                    p1 = @"Data *FECHAMENTO*: " + data1.ToString("dd/MM/yyyy") + @" Até " + data2.ToString("dd/MM/yyyy");
                }

                var d1 = new SqlParameter("@d1", System.Data.SqlDbType.DateTime)
                {
                    Value = data1
                };

                var d2 = new SqlParameter("@d2", System.Data.SqlDbType.DateTime)
                {
                    Value = data2
                };
                if (autonumeroCliente > 0)
                {
                    var auto = new SqlParameter("@auto", System.Data.SqlDbType.BigInt)
                    {
                        Value = autonumeroCliente
                    };
                    p1 = p1 + " - " + nomeCliente;
                }

                //p1 = string.Concat(p1, " - Por LOCAL");

                if (!string.IsNullOrEmpty(nomeLocalFisico))
                {
                    p1 = string.Concat(p1, "\r\n", "Local: ", nomeLocalFisico);
                }


                var csql = new StringBuilder();

                csql.Append("DROP TABLE IF EXISTS `corretivastemp`; ");
                csql.Append("CREATE table corretivastemp( ");

                if (osFechada == "S")
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                            csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> ''  and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                            if (sistemaOrcamento == "S")
                            {
                                csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                            }

                            csql.Append("and nomeSistema != '' and cast(dataEmissao as date) >= {0} and cast(dataEmissao as date) <= {1} and  tb_ordemservico.autonumeroCliente = {2} ");
                            csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");


                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                            csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> '' and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                            if (sistemaOrcamento == "S")
                            {
                                csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                            }
                            csql.Append("and nomeSistema != '' and cast(dataEmissao as date) >= {0} and cast(dataEmissao as date) <= {1} and  tb_ordemservico.autonumeroCliente = {2} and  tb_ordemservico.autonumeroLocalFisico = {3}  ");
                            csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");
                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {


                        csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                        csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> ''  and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                        if (sistemaOrcamento == "S")
                        {
                            csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                        }
                        csql.Append("and nomeSistema != '' and cast(dataEmissao as date) >= {0} and cast(dataEmissao as date) <= {1}  ");
                        csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");

                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }
                else
                {
                    if (autonumeroCliente > 0)
                    {

                        if (autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                        {

                            csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                            csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> '' and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                            if (sistemaOrcamento == "S")
                            {
                                csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                            }
                            csql.Append("and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and tb_ordemservico.autonumeroCliente = {2} ");
                            csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");


                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente });
                            }
                        }

                        if (autonumeroSetor == 0 && autonumeroLocalFisico > 0)
                        {
                            csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                            csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> '' and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                            if (sistemaOrcamento == "S")
                            {
                                csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                            }
                            csql.Append("and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1} and autonumeroCliente = {2} and autonumeroLocalFisico = {3}  ");
                            csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");

                            using (var dc = new manutEntities())
                            {
                                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2, autonumeroCliente, autonumeroLocalFisico });
                            }
                        }

                    }
                    else
                    {
                        csql.Append("SELECT  sequencia,medicao,tb_ordemservico.etapa,codigoOs,nomeSistema,(valor) as total,(quantidadeSS) as qtdeSS ,siglaitem,resumoServico  FROM manut.tb_ordemservico,manut.tb_etapa ");
                        csql.Append("where cancelado <> 'S' and SUBSTRING(codigoOs, 12, 3) = 'C' and siglaItem <> 'CO' and medicao <> '' and tb_ordemservico.etapa = tb_etapa.etapa and tb_ordemservico.autonumeroCliente = tb_etapa.autonumeroCliente ");

                        if (sistemaOrcamento == "S")
                        {
                            csql.Append("and nomeSistema = 'ORÇAMENTO' ");
                        }
                        csql.Append("and nomeSistema != '' and cast(dataInicio as date) >= {0} and cast(dataInicio as date) <= {1}  ");
                        csql.Append("order by codigoOs, medicao,sequencia, nomeSistema, resumoservico ) ;  ");
                        using (var dc = new manutEntities())
                        {
                            var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
                        }
                    }
                }

                var obra = String.Empty;
                var processo = String.Empty;

                if (autonumeroCliente > 0)
                {
                    var cl = new DataClienteController();
                    var cliente = cl.GetCliente((int)autonumeroCliente);
                    foreach (var item in cliente)
                    {
                        obra = string.Concat("OBRA: ", item.obra);
                        processo = string.Concat("PROCESSO: ", item.processo);
                    }
                }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/corretivas.rpt");

                    rd.Load(local);

                    rd.SetParameterValue("p1", p1.Trim());
                    rd.SetParameterValue("@obra", obra);
                    rd.SetParameterValue("@processo", processo);

                    rd.RecordSelectionFormula = string.Empty;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }


            }


            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;


        }

        [HttpGet]
        public HttpResponseMessage ImprimirTramitacao(int autonumeroGrupoTramitacao, int autonumeroCliente)
        {
            var c = 1;
            try
            {
                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    var local = HttpContext.Current.Server.MapPath("~/rpt/tramitacao.rpt");

                    var filtro = "{tramitacao1.cancelado} <> 'S' ";

                    if (autonumeroGrupoTramitacao > 0)
                    {
                        filtro = filtro + "AND {tramitacao1.autonumeroGrupoTramitacao} = " + autonumeroGrupoTramitacao;
                    }

                    if (autonumeroCliente > 0)
                    {
                        filtro = filtro + "AND {tramitacao1.autonumeroCliente} = " + autonumeroCliente;
                    }

                    rd.Load(local);
                    //rd.SetParameterValue("p1", codigoOs);
                    //rd.SetParameterValue("p2", modelo);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }

        [HttpGet]
        public HttpResponseMessage DownloadFile(string nomeArquivo)
        {

            var caminho = "~/UploadedFiles/planilhas/";

            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), nomeArquivo);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        //[HttpPost]
        //public string GerarPmocCivil()

        //{
        //    var message = String.Empty;

        //    try
        //    {

        //        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //        var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //        var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
        //        var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
        //        var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
        //        var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
        //        var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();

        //        using (var dc = new manutEntities())
        //        {
        //            var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
        //            if (cli == null)
        //            {
        //                throw new ArgumentException("Erro: Tabela Contrato Não Encontrada");
        //            }

        //            var contador = cli.contadorPmocCivil;

        //            if (autonumeroSetor > 0)
        //            {
        //                dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //                {
        //                    x.contadorPmocCivil = contador;
        //                    contador++;
        //                });
        //                dc.SaveChanges();

        //            }
        //            else
        //            {
        //                if (autonumeroPredio > 0)
        //                {
        //                    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //                    {
        //                        x.contadorPmocCivil = contador;
        //                        contador++;
        //                    });
        //                    dc.SaveChanges();

        //                }
        //                else
        //                {
        //                    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //                    {
        //                        x.contadorPmocCivil = contador;
        //                        contador++;
        //                    });
        //                    dc.SaveChanges();
        //                }
        //            }

        //            var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
        //            if (linha != null)
        //            {
        //                linha.contadorPmocCivil = contador;
        //                dc.tb_cliente.AddOrUpdate(linha);
        //                dc.SaveChanges();
        //            }

        //        }

        //        var ano = Convert.ToInt32(anoMes.Substring(0, 4));
        //        var mes = Convert.ToInt32(anoMes.Substring(5, 2));

        //        var dlc = new DataCheckListHistoricoCivilController();
        //        dlc.SalvarPmocCivil(autonumeroCliente, ano, mes);

        //    }
        //    catch (Exception ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();

        //    }
        //    return "";
        //}


        [HttpPost]
        public string GerarPmocCivil()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
                var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
                var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();

                using (var dc = new manutEntities())
                {
                    var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (cli == null)
                    {
                        throw new ArgumentException("Erro: Tabela Contrato Não Encontrada");
                    }

                    var contador = cli.contadorPmocCivil;

                    //if (autonumeroSetor > 0)
                    //{
                    //    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                    //    {
                    //        x.contadorPmocCivil = contador;
                    //        contador++;
                    //    });
                    //    dc.SaveChanges();

                    //}
                    //else
                    //{
                    //if (autonumeroPredio > 0)
                    //{
                    //    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                    //    {
                    //        x.contadorPmocCivil = contador;
                    //        contador++;
                    //    });
                    //    dc.SaveChanges();

                    //}
                    //else
                    //{
                    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                    {
                        x.contadorPmocCivil = contador;
                        contador++;
                    });
                    dc.SaveChanges();
                    //}
                    //}

                    var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linha != null)
                    {
                        linha.contadorPmocCivil = contador;
                        dc.tb_cliente.AddOrUpdate(linha);
                        dc.SaveChanges();
                    }

                }

                var ano = Convert.ToInt32(anoMes.Substring(0, 4));
                var mes = Convert.ToInt32(anoMes.Substring(5, 2));
                var dlc = new DataCheckListHistoricoCivilController();
                dlc.SalvarPmocCivil(autonumeroCliente, ano, mes);

            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return "";
        }



        [HttpPost]
        public HttpResponseMessage ImprimirPmocCivil()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
                var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
                var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();


                //var filtro = "{local1.cancelado} <> 'S' ";

                var local = HttpContext.Current.Server.MapPath("~/rpt/PmocCivilLocal.rpt");

                //if (autonumeroCliente > 0) { filtro = filtro + " and {local1.autonumeroCliente} = " + autonumeroCliente; }
                //if (autonumeroPredio > 0) { filtro = filtro + " and {local1.autonumeroPredio} = " + autonumeroPredio; }
                //if (autonumeroSetor > 0) { filtro = filtro + " and {local1.autonumeroSetor} = " + autonumeroSetor; }



                var filtro = "{checklisthistoricocivil1.cancelado} <> 'S' and {checklisthistoricocivilitem1.anoMes} = {checklisthistoricocivil1.anoMes}   and {checklisthistoricocivil1.anoMes} = '" + anoMes.Replace("/", "") + "'  and {checklisthistoricocivil1.autonumeroCliente} = " + autonumeroCliente;


                if (autonumeroPredio > 0) { filtro = filtro + " and {checklisthistoricocivil1.autonumeroPredio} = " + autonumeroPredio; }
                if (autonumeroSetor > 0) { filtro = filtro + " and {checklisthistoricocivil1.autonumeroSetor} = " + autonumeroSetor; }






                using (var dc = new manutEntities())
                {
                    var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (cli == null)
                    {
                        throw new ArgumentException("Erro: Tabela Contrato Não Encontrada");
                    }

                    var contador = cli.contadorPmocCivil;

                    if (autonumeroSetor > 0)
                    {
                        dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                        {
                            x.contadorPmocCivil = contador;
                            contador++;
                        });
                        dc.SaveChanges();

                    }
                    else
                    {
                        if (autonumeroPredio > 0)
                        {
                            dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                            {
                                x.contadorPmocCivil = contador;
                                contador++;
                            });
                            dc.SaveChanges();

                        }
                        else
                        {
                            dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
                            {
                                x.contadorPmocCivil = contador;
                                contador++;
                            });
                            dc.SaveChanges();
                        }
                    }

                    var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linha != null)
                    {
                        linha.contadorPmocCivil = contador;
                        dc.tb_cliente.AddOrUpdate(linha);
                        dc.SaveChanges();
                    }

                }

                //var ano = Convert.ToInt32(anoMes.Substring(0, 4));
                //var mes = Convert.ToInt32(anoMes.Substring(5, 2));

                //var dlc = new DataCheckListHistoricoCivilController();
                //dlc.SalvarPmocCivil(autonumeroCliente, ano, mes);


                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    rd.Load(local);
                    //rd.SetParameterValue("p1", codigoOs);
                    //rd.SetParameterValue("p2", modelo);


                    //Debug.WriteLine(99999);


                    rd.RecordSelectionFormula = filtro;



                    //var caminho = string.Concat("~/UploadedFiles/PmocCivil/", sigla);

                    ////// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    ////var extension = "pdf";
                    ////var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    ////var fileName = string.Concat("PmocCivil-", anoMes.Replace("/", ""), ".pdf");
                    //var fileName = string.Concat("PmocCivil-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
                    //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    //if (File.Exists(fileSavePath))
                    //{
                    //    File.Delete(fileSavePath);
                    //}

                    //// Declare variables and get the export options.
                    //ExportOptions exportOpts = new ExportOptions();
                    //PdfFormatOptions pdfFormatOpts = new PdfFormatOptions();
                    //DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    //exportOpts = rd.ExportOptions;

                    //exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //exportOpts.FormatOptions = pdfFormatOpts;

                    //// Set the disk file options and export.
                    //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    //diskOpts.DiskFileName = fileSavePath;
                    //exportOpts.DestinationOptions = diskOpts;

                    //rd.Export();


                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;
                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;
        }


        //[HttpPost]
        //public HttpResponseMessage ImprimirPmocCivil()

        //{
        //    var message = String.Empty;

        //    try
        //    {

        //        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //        var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //        var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
        //        var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
        //        var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
        //        var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
        //        var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();

        //        var filtro = "{checklisthistoricocivil1.cancelado} <> 'S' and {checklisthistoricocivilitem1.anoMes} = {checklisthistoricocivil1.anoMes}   and {checklisthistoricocivil1.anoMes} = '" + anoMes.Replace("/", "") + "'  and {checklisthistoricocivil1.autonumeroCliente} = " + autonumeroCliente;

        //        var local = HttpContext.Current.Server.MapPath("~/rpt/PmocCivilLocal.rpt");

        //        if (autonumeroPredio > 0) { filtro = filtro + " and {checklisthistoricocivil1.autonumeroPredio} = " + autonumeroPredio; }
        //        if (autonumeroSetor > 0) { filtro = filtro + " and {checklisthistoricocivil1.autonumeroSetor} = " + autonumeroSetor; }

        //        using (var rd = new ReportDocument())
        //        {
        //            var Response = HttpContext.Current.ApplicationInstance.Response;


        //            rd.Load(local);


        //            Debug.WriteLine(99999);


        //            rd.RecordSelectionFormula = filtro;



        //            //var caminho = string.Concat("~/UploadedFiles/PmocCivil/", sigla);

        //            ////// Criar a pasta se não existir ou devolver informação sobre a pasta
        //            //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


        //            ////var fileName = string.Concat("PmocEquipamento-", anoMes.Replace("/", ""), ".pdf");
        //            //var fileName = string.Concat("PmocCivil-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
        //            //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);

        //            //if (File.Exists(fileSavePath))
        //            //{
        //            //    File.Delete(fileSavePath);
        //            //}


        //            //// Declare variables and get the export options.
        //            //ExportOptions exportOpts = new ExportOptions();
        //            //PdfFormatOptions pdfFormatOpts = new PdfFormatOptions();
        //            //DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
        //            //exportOpts = rd.ExportOptions;


        //            //exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            //exportOpts.FormatOptions = pdfFormatOpts;

        //            //// Set the disk file options and export.
        //            //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
        //            //diskOpts.DiskFileName = fileSavePath;
        //            //exportOpts.DestinationOptions = diskOpts;

        //            //rd.Export();



        //            Response.Buffer = false;
        //            Response.ClearContent();
        //            Response.ClearHeaders();

        //            var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
        //            stream.Seek(0, SeekOrigin.Begin);

        //            ////75 is my print job limit.
        //            //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
        //            //return CreateReport(reportClass);

        //            rd.Close();
        //            rd.Dispose();

        //            var resp = Request.CreateResponse(HttpStatusCode.OK);
        //            resp.Content = new StreamContent(stream);
        //            return resp;
        //        }
        //    }

        //    catch (LogOnException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "Incorrect Logon Parameters. Check your user name and password";
        //    }
        //    catch (DataSourceException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "An error has occurred while connecting to the database.";
        //    }
        //    catch (EngineException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();

        //    }
        //    return null;
        //}




        [HttpGet]
        public string GetVerificarPmocCivilFoiGerado(string anoMes, string sigla)
        {
            var caminho = string.Concat("~/UploadedFiles/PmocCivil/", sigla);

            var fileName = string.Concat("PmocCivil-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
            if (File.Exists(fileSavePath))
            {
                return "S";
            }
            return "N";

        }


        [HttpGet]
        public IEnumerable GetPmocCivilArquivoGerado(string sigla)
        {
            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocCivil/", sigla));
            return new DirectoryInfo(caminho).GetFiles().Select(o => o.Name).ToList();

        }



        [HttpGet]
        public string GetVerificarPmocEquipamentoFoiGerado(string anoMes, long autonumeroContrato)
        {

            anoMes = anoMes.Replace("/", "");
            using (var dc = new manutEntities())
            {
                var q = dc.checklisthistorico.Where(x => x.autonumeroCliente == autonumeroContrato && x.anoMes == anoMes).ToList().Count();
                if (q > 0)
                {
                    return "S";
                }
                return "N";
            }

            //var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/", sigla);

            //var fileName = string.Concat("PmocEquipamento-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
            //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
            //if (File.Exists(fileSavePath))
            //{
            //    return "S";
            //}
            //return "N";

        }




        [HttpGet]
        public IEnumerable GetPmocEquipamentoArquivoGerado(string sigla)
        {
            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocEquipamento/", sigla));
            return new DirectoryInfo(caminho).GetFiles().Select(o => o.Name).ToList();

        }


        [HttpGet]
        public HttpResponseMessage DownloadPmocCivil(string nomeArquivo, string sigla)
        {

            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocCivil/", sigla));

            var fileSavePath = Path.Combine(caminho, nomeArquivo);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }


        [HttpGet]
        public HttpResponseMessage DownloadPmocEquipamento(string nomeArquivo, string sigla)
        {

            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocEquipamento/", sigla));

            var fileSavePath = Path.Combine(caminho, nomeArquivo);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        //[HttpPost]
        //public HttpResponseMessage ImprimirPmocEquipamento()

        //{
        //    var message = String.Empty;

        //    try
        //    {

        //        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //        //var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //        //var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
        //        //var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
        //        //var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
        //        var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
        //        var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();


        //        var filtro = "{tb_cadastro1.cancelado} <> 'S' ";

        //        var local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamento.rpt");

        //        if (autonumeroCliente > 0) { filtro = filtro + " and {tb_cadastro1.autonumeroCliente} = " + autonumeroCliente; }
        //        //if (autonumeroPredio > 0) { filtro = filtro + " and {local1.autonumeroPredio} = " + autonumeroPredio; }
        //        //if (autonumeroSetor > 0) { filtro = filtro + " and {local1.autonumeroSetor} = " + autonumeroSetor; }

        //        using (var dc = new manutEntities())
        //        {
        //            //var cli = dc.cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
        //            //if (cli == null)
        //            //{
        //            //    throw new ArgumentException("Erro: Tabela Contrato Não Encontrada");
        //            //}

        //            //var contador = cli.contadorPmocEquipamento;

        //            //if (autonumeroSetor > 0)
        //            //{
        //            //    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //            //    {
        //            //        x.contadorPmocEquipamento = contador;
        //            //        contador++;
        //            //    });
        //            //    dc.SaveChanges();

        //            //}
        //            //else
        //            //{
        //            //if (autonumeroPredio > 0)
        //            //{
        //            //    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente && p.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //            //    {
        //            //        x.contadorPmocEquipamento = contador;
        //            //        contador++;
        //            //    });
        //            //    dc.SaveChanges();

        //            //}
        //            //else
        //            //{
        //            //    dc.local.Where(p => p.cancelado != "S" && p.autonumeroCliente == autonumeroCliente).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nome).ToList().ForEach(x =>
        //            //    {
        //            //        x.contadorPmocEquipamento = contador;
        //            //        contador++;
        //            //    });
        //            //    dc.SaveChanges();
        //            //}
        //            //}

        //            //var linha = dc.cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
        //            //if (linha != null)
        //            //{
        //            //    linha.contadorPmocEquipamento = contador;
        //            //    dc.cliente.AddOrUpdate(linha);
        //            //    dc.SaveChanges();
        //            //}

        //        }


        //        using (var rd = new ReportDocument())
        //        {
        //            var Response = HttpContext.Current.ApplicationInstance.Response;


        //            rd.Load(local);



        //            rd.RecordSelectionFormula = filtro;



        //            var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/", sigla);

        //            //// Criar a pasta se não existir ou devolver informação sobre a pasta
        //            var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


        //            //var extension = "pdf";
        //            //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

        //            //var fileName = string.Concat("PmocEquipamento-", anoMes.Replace("/", ""), ".pdf");
        //            var fileName = string.Concat("PmocEquipamento-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
        //            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
        //            if (File.Exists(fileSavePath))
        //            {
        //                File.Delete(fileSavePath);
        //            }


        //            // Declare variables and get the export options.
        //            ExportOptions exportOpts = new ExportOptions();
        //            PdfFormatOptions pdfFormatOpts = new PdfFormatOptions();
        //            DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
        //            exportOpts = rd.ExportOptions;


        //            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            exportOpts.FormatOptions = pdfFormatOpts;

        //            // Set the disk file options and export.
        //            exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
        //            diskOpts.DiskFileName = fileSavePath;
        //            exportOpts.DestinationOptions = diskOpts;

        //            rd.Export();


        //            Response.Buffer = false;
        //            Response.ClearContent();
        //            Response.ClearHeaders();

        //            var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
        //            stream.Seek(0, SeekOrigin.Begin);

        //            ////75 is my print job limit.
        //            //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
        //            //return CreateReport(reportClass);

        //            rd.Close();
        //            rd.Dispose();

        //            var resp = Request.CreateResponse(HttpStatusCode.OK);
        //            resp.Content = new StreamContent(stream);
        //            return resp;
        //        }
        //    }

        //    catch (LogOnException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "Incorrect Logon Parameters. Check your user name and password";
        //    }
        //    catch (DataSourceException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "An error has occurred while connecting to the database.";
        //    }
        //    catch (EngineException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();

        //    }
        //    return null;
        //}




        //[HttpPost]
        //public HttpResponseMessage ImprimirPmocEquipamento()

        //{
        //    var message = String.Empty;

        //    try
        //    {

        //        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //        //var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //        //var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
        //        //var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
        //        //var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
        //        var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
        //        var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
        //        var modelo = HttpContext.Current.Request.Form["modelo"].ToString().Trim();


        //        var filtro = "{checklisthistorico1.cancelado} <> 'S'  and {checklisthistorico1.anoMes} = '" + anoMes + "' ";

        //        var filtroDoSubReport = "{checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and {checklisthistitemnrofolha1.d} = 'N'  and {checklisthistitemnrofolha1.e} = 'N' and {checklisthistitemnrofolha1.q} = 'N' ";

        //        var local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamento.rpt");
        //        var csql = new StringBuilder();

        //        csql.Append("DROP TABLE IF EXISTS `tempSubSistema`; ");
        //        csql.Append("CREATE table tempSubSistema( ");

        //        if (modelo == "Diário")
        //        {
        //            csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.d = 'S' and ch.autonumeroCliente = {0} and ");
        //            csql.Append("ch.anoMes = {1} ) ");

        //            filtroDoSubReport = "{checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.d} = 'S' ) ";
        //            local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
        //        }

        //        if (modelo == "Semanal")
        //        {
        //            csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.e = 'S' and ch.autonumeroCliente = {0} and ");
        //            csql.Append("ch.anoMes = {1} ) ");

        //            filtroDoSubReport = "{checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.e} = 'S' ) ";
        //            local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
        //        }

        //        if (modelo == "Quinzenal")
        //        {
        //            csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.q = 'S' and ch.autonumeroCliente = {0} and ");
        //            csql.Append("ch.anoMes = {1} ) ");

        //            filtroDoSubReport = "{checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.q} = 'S' ) ";
        //            local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
        //        }

        //        if (modelo != "Mensal")
        //        {
        //            using (var dc = new manutEntities())
        //            {
        //                var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { autonumeroCliente, anoMes });
        //            }
        //        }

        //        if (autonumeroCliente > 0) { filtro = filtro + " and {checklisthistorico1.autonumeroCliente} = " + autonumeroCliente; }



        //        using (var rd = new ReportDocument())
        //        {
        //            var Response = HttpContext.Current.ApplicationInstance.Response;


        //            rd.Load(local);
        //            rd.SetParameterValue("@modelo", modelo);
        //            rd.OpenSubreport("checklist").RecordSelectionFormula = filtroDoSubReport;

        //            rd.RecordSelectionFormula = filtro;




        //            //var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/", sigla);

        //            ////// Criar a pasta se não existir ou devolver informação sobre a pasta
        //            //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


        //            ////var extension = "pdf";
        //            //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

        //            //var fileName = string.Concat("PmocEquipamento-", anoMes.Replace("/", ""), ".pdf");
        //            //var fileName = string.Concat("PmocEquipamento-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
        //            //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);

        //            //if (File.Exists(fileSavePath))
        //            //{
        //            //    File.Delete(fileSavePath);
        //            //}


        //            //// Declare variables and get the export options.
        //            //ExportOptions exportOpts = new ExportOptions();
        //            //PdfFormatOptions pdfFormatOpts = new PdfFormatOptions();
        //            //DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
        //            //exportOpts = rd.ExportOptions;


        //            //exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            //exportOpts.FormatOptions = pdfFormatOpts;

        //            //// Set the disk file options and export.
        //            //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
        //            //diskOpts.DiskFileName = fileSavePath;
        //            //exportOpts.DestinationOptions = diskOpts;

        //            //rd.Export();



        //            Response.Buffer = false;
        //            Response.ClearContent();
        //            Response.ClearHeaders();

        //            var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
        //            stream.Seek(0, SeekOrigin.Begin);

        //            ////75 is my print job limit.
        //            //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
        //            //return CreateReport(reportClass);

        //            rd.Close();
        //            rd.Dispose();

        //            var resp = Request.CreateResponse(HttpStatusCode.OK);
        //            resp.Content = new StreamContent(stream);
        //            return resp;

        //        }
        //    }

        //    catch (LogOnException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "Incorrect Logon Parameters. Check your user name and password";
        //    }
        //    catch (DataSourceException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = "An error has occurred while connecting to the database.";
        //    }
        //    catch (EngineException ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        var c = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            c = ex.InnerException.ToString().Substring(0, 130);
        //        }
        //        message = message + ex.Message + " ---- " + c;
        //        //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();

        //    }
        //    return null;
        //}


        [HttpPost]
        public HttpResponseMessage ImprimirPmocEquipamento()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                var autonumeroLocal = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocal"].ToString());

                var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
                var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
                var modelo = HttpContext.Current.Request.Form["modelo"].ToString().Trim();



                var filtroLocal = "";
                if (autonumeroLocal > 0)
                {
                    filtroLocal = " and {checklisthistorico1.autonumeroLocalFisico} = " + autonumeroLocal;
                }
                else
                {
                    if (autonumeroSetor > 0)
                    {
                        filtroLocal = " and {checklisthistorico1.autonumeroSetor} = " + autonumeroSetor;
                    }
                    else
                    {
                        if (autonumeroPredio > 0)
                        {
                            filtroLocal = " and {checklisthistorico1.autonumeroPredio} = " + autonumeroPredio;
                        }
                    }

                }


                var filtro = "{checklisthistorico1.cancelado} <> 'S'  and {checklisthistorico1.anoMes} = '" + anoMes + "' ";

                var filtroDoSubReport = "({ checklisthistoriconrofolha1.mensal} = 1 and  { checklisthistitemnrofolha1.m} ='S' or " +
                                        "{ checklisthistoriconrofolha1.bimestral} = 1 and  { checklisthistitemnrofolha1.b} ='S' or " +
                                        "{ checklisthistoriconrofolha1.trimestral} = 1 and  { checklisthistitemnrofolha1.t} ='S' or " +
                                        "{ checklisthistoriconrofolha1.semestral} = 1 and  { checklisthistitemnrofolha1.s} ='S' or " +
                                        "{ checklisthistoriconrofolha1.anual} = 1 and  { checklisthistitemnrofolha1.a} ='S' ) ";


                filtroDoSubReport = filtroDoSubReport + " and {checklisthistoriconrofolha1.autonumeroCliente} = " + autonumeroCliente.ToString() + " and {checklisthistoriconrofolha1.autonumeroCliente} =  {subsistemacliente1.autonumeroCliente} and {checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and {checklisthistitemnrofolha1.d} = 'N'  and {checklisthistitemnrofolha1.e} = 'N' and {checklisthistitemnrofolha1.q} = 'N' ";

                var local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamento.rpt");
                var csql = new StringBuilder();

                var executarConsulta = false;

                csql.Append("DROP TABLE IF EXISTS `tempSubSistema`; ");
                csql.Append("CREATE table tempSubSistema( ");

                if (modelo == "Diário")
                {
                    executarConsulta = true;
                    csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.d = 'S' and ch.autonumeroCliente = {0} and ");
                    csql.Append("ch.anoMes = {1} ) ");

                    filtroDoSubReport = "{checklisthistoriconrofolha1.autonumeroCliente} = " + autonumeroCliente.ToString() + " and {checklisthistoriconrofolha1.autonumeroCliente} =  {subsistemacliente1.autonumeroCliente} and {checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.d} = 'S' ) ";
                    local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
                }

                if (modelo == "Semanal")
                {
                    executarConsulta = true;
                    csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.e = 'S' and ch.autonumeroCliente = {0} and ");
                    csql.Append("ch.anoMes = {1} ) ");

                    filtroDoSubReport = "{checklisthistoriconrofolha1.autonumeroCliente} = " + autonumeroCliente.ToString() + " and {checklisthistoriconrofolha1.autonumeroCliente} =  {subsistemacliente1.autonumeroCliente} and {checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.e} = 'S' ) ";
                    local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
                }

                if (modelo == "Quinzenal")
                {
                    executarConsulta = true;
                    csql.Append("SELECT distinct autonumeroSubSistema from checklisthistitemnrofolha as ch where ch.q = 'S' and ch.autonumeroCliente = {0} and ");
                    csql.Append("ch.anoMes = {1} ) ");

                    filtroDoSubReport = "{checklisthistoriconrofolha1.autonumeroCliente} = " + autonumeroCliente.ToString() + " and {checklisthistoriconrofolha1.autonumeroCliente} =  {subsistemacliente1.autonumeroCliente} and {checklisthistoriconrofolha1.contadorPmocEquipamento} = {?Pm-checklisthistorico1.contadorPmocEquipamento} and ({checklisthistitemnrofolha1.q} = 'S' ) ";
                    local = HttpContext.Current.Server.MapPath("~/rpt/PmocEquipamentoDSQ.rpt");
                }

                if (executarConsulta)
                {
                    using (var dc = new manutEntities())
                    {
                        var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { autonumeroCliente, anoMes });
                    }
                }
                else
                {
                    var c = 1;

                    using (var dc = new manutEntities())
                    {

                        if (modelo != "Mensal Todas Tarefas")
                        {

                            dc.checklisthistorico.Where(x => x.autonumeroCliente == autonumeroCliente && x.anoMes == anoMes && x.cancelado != "S").ToList().ForEach(x =>
                            {
                                x.imprimir = "N";
                            });

                            dc.SaveChanges();

                            var lista4 = (from x in dc.checklisthistorico.Where(x => x.autonumeroCliente == autonumeroCliente && x.anoMes == anoMes && x.cancelado != "S") select x).ToList();
                            var lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == 20000).ToList();


                            if (modelo == "Mensal")
                            {
                                lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes && p.m == "S").ToList();
                                filtroDoSubReport = filtroDoSubReport + " and {checklisthistitemnrofolha1.m} = 'S'  ";
                            }
                            if (modelo == "Bimestral")
                            {
                                lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes && p.b == "S").ToList();
                                filtroDoSubReport = filtroDoSubReport + " and {checklisthistitemnrofolha1.b} = 'S' ";
                            }
                            if (modelo == "Trimestral")
                            {
                                lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes && p.t == "S").ToList();
                                filtroDoSubReport = filtroDoSubReport + " and {checklisthistitemnrofolha1.t} = 'S' ";
                            }
                            if (modelo == "Semestral")
                            {
                                lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes && p.s == "S").ToList();
                                filtroDoSubReport = filtroDoSubReport + " and {checklisthistitemnrofolha1.s} = 'S' ";
                            }
                            if (modelo == "Anual")
                            {
                                lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes && p.a == "S").ToList();
                                filtroDoSubReport = filtroDoSubReport + " and {checklisthistitemnrofolha1.a} = 'S' ";
                            }


                            filtro = filtro + " and { checklisthistorico1.imprimir} = 'S' ";

                            var lista5 = (from k in lista2
                                          join o in lista4 on k.autonumeroSubSistema equals o.autonumeroSubSistema
                                          select new
                                          {
                                              o
                                          }).ToList();

                            lista5.ForEach(x =>
                            {
                                x.o.imprimir = "S";
                            });
                            dc.SaveChanges();
                        }


                    }

                }

                if (autonumeroCliente > 0) { filtro = filtro + filtroLocal + " and {checklisthistorico1.autonumeroCliente} = " + autonumeroCliente; }


                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    rd.Load(local);
                    rd.SetParameterValue("@modelo", modelo);
                    rd.OpenSubreport("checklist").RecordSelectionFormula = filtroDoSubReport;

                    rd.RecordSelectionFormula = filtro;




                    var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/", sigla);

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    //var extension = "pdf";
                    //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    //var fileName = string.Concat("PmocEquipamento-", anoMes.Replace("/", ""), ".pdf");
                    //var fileName = string.Concat("PmocEquipamento-", sigla, "-", anoMes.Replace("/", ""), ".pdf");
                    //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);

                    //if (File.Exists(fileSavePath))
                    //{
                    //    File.Delete(fileSavePath);
                    //}


                    //// Declare variables and get the export options.
                    //ExportOptions exportOpts = new ExportOptions();
                    //PdfFormatOptions pdfFormatOpts = new PdfFormatOptions();
                    //DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    //exportOpts = rd.ExportOptions;


                    //exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //exportOpts.FormatOptions = pdfFormatOpts;

                    //// Set the disk file options and export.
                    //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    //diskOpts.DiskFileName = fileSavePath;
                    //exportOpts.DestinationOptions = diskOpts;

                    //rd.Export();



                    Response.Buffer = false;
                    //Response.ClearContent();
                    //Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;
        }

        [HttpPost]
        public HttpResponseMessage ImprimirPmocEquipamentoGeral()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
                var modelo = HttpContext.Current.Request.Form["modelo"].ToString().Trim();

                var filtro = "{checklisthistorico1.cancelado} <> 'S'  and {checklisthistorico1.anoMes} = '" + anoMes + "' ";
                var local = HttpContext.Current.Server.MapPath("~/rpt/PmocGeral.rpt");

                if (modelo == "SubSistema")
                {
                    local = HttpContext.Current.Server.MapPath("~/rpt/PmocGeralSubSistema.rpt");
                }

                if (autonumeroCliente > 0) { filtro = filtro + " and {checklisthistorico1.autonumeroCliente} = " + autonumeroCliente; }
                var cc = anoMes.Substring(4, 2);
                var ano = Convert.ToInt32(anoMes.Substring(0, 4));
                var mes = Convert.ToInt32(anoMes.Substring(4, 2));

                string nomeMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes).ToUpper();

                var p1 = "PMOC REFERENTE AO MÊS DE " + nomeMes + " / " + ano.ToString();

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    rd.Load(local);
                    rd.SetParameterValue("@p1", p1);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    //Response.ClearContent();
                    //Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;
        }




        [HttpGet]
        public IEnumerable GetPmocEquipamentoImagem(string sigla, string ano, string mes)
        {
            try
            {
                var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
                var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocEquipamento/Imagem/", sigla, "/", anoMes));

                return new DirectoryInfo(caminho).GetFiles().Select(o => o.Name).OrderBy(s => s).ToList();
            }
            catch (Exception)
            {
                return "";

            }

        }

        [HttpGet]
        public string RemoverPmocEquipamentoImagem(string arquivo, string sigla, string ano, string mes)
        {
            var message = "";

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
            var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/Imagem/", sigla, "/", anoMes, "/");


            try
            {
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), arquivo);
                if (File.Exists(fileSavePath))
                {
                    File.Delete(fileSavePath);
                }

                return message;


            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130)
                    : ex.Message;

            }

            return message;
        }


        [HttpPost]
        public string UploadImagemPmoc()
        {

            var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();

            try
            {

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                    var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
                    var ano = HttpContext.Current.Request.Form["ano"].ToString().Trim();
                    var mes = HttpContext.Current.Request.Form["mes"].ToString().Trim();

                    var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

                    var caminho = string.Concat("~/UploadedFiles/PmocEquipamento/Imagem/", sigla, "/", anoMes);

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));

                    if (httpPostedFile == null)
                    {
                        message = "Erro Upload 1";
                        return message;
                    }

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                    var fileName = httpPostedFile.FileName;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);

                }

                return message;


            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130) + " - DataprodutoController SaveFilesFoto"
                    : ex.Message + " - DataprodutoController SaveFilesFoto";

            }

            return message;
        }


        [HttpGet]
        public HttpResponseMessage DownloadImagemPmoc(string nomeArquivo, string sigla, string ano, string mes)
        {
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocEquipamento/Imagem/", sigla, "/", anoMes, "/"));

            //Debug.WriteLine(caminho);

            var fileSavePath = Path.Combine(caminho, nomeArquivo);

            //Debug.WriteLine(fileSavePath);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }



        [HttpGet]
        public string RemoverPmocCivilImagem(string arquivo, string sigla, string ano, string mes)
        {
            var message = "";

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
            var caminho = string.Concat("~/UploadedFiles/PmocCivil/Imagem/", sigla, "/", anoMes, "/");

            try
            {
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), arquivo);
                if (File.Exists(fileSavePath))
                {
                    File.Delete(fileSavePath);
                }

                return message;


            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130)
                    : ex.Message;

            }

            return message;
        }


        [HttpPost]
        public string UploadImagemPmocCivil()
        {

            var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();

            try
            {

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                    var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
                    var ano = HttpContext.Current.Request.Form["ano"].ToString().Trim();
                    var mes = HttpContext.Current.Request.Form["mes"].ToString().Trim();

                    var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

                    var caminho = string.Concat("~/UploadedFiles/PmocCivil/Imagem/", sigla, "/", anoMes);

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));

                    if (httpPostedFile == null)
                    {
                        message = "Erro Upload 1";
                        return message;
                    }

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    //var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                    var fileName = httpPostedFile.FileName;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);

                }

                return message;


            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130) + " - DataprodutoController SaveFilesFoto"
                    : ex.Message + " - DataprodutoController SaveFilesFoto";

            }

            return message;
        }

        [HttpGet]
        public IEnumerable GetPmocCivilImagem(string sigla, string ano, string mes)
        {
            try
            {
                var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
                var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocCivil/Imagem/", sigla, "/", anoMes));

                return new DirectoryInfo(caminho).GetFiles().Select(o => o.Name).OrderBy(s => s).ToList();
            }
            catch (Exception)
            {
                return "";

            }

        }

        [HttpGet]
        public HttpResponseMessage DownloadImagemPmocCivil(string nomeArquivo, string sigla, string ano, string mes)
        {
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            var caminho = HttpContext.Current.Server.MapPath(string.Concat("~/UploadedFiles/PmocCivil/Imagem/", sigla, "/", anoMes, "/"));

            //Debug.WriteLine(caminho);

            var fileSavePath = Path.Combine(caminho, nomeArquivo);

            //Debug.WriteLine(fileSavePath);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        [HttpPost]
        public HttpResponseMessage ImprimirPmocQtdeTarefaMes()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();


                var filtro = "{subsistemaqtdepmoc1.anoMes} = '" + anoMes + "' and ({subsistemaqtdepmoc1.mensal} + {subsistemaqtdepmoc1.bimestral} + {subsistemaqtdepmoc1.trimestral} + {subsistemaqtdepmoc1.semestral} + {subsistemaqtdepmoc1.anual} ) > 0 ";
                var local = HttpContext.Current.Server.MapPath("~/rpt/PmocQtdeTarefaMes.rpt");


                if (autonumeroCliente > 0) { filtro = filtro + " and {subsistemaqtdepmoc1.autonumeroCliente} = " + autonumeroCliente; }
                var cc = anoMes.Substring(4, 2);
                var ano = Convert.ToInt32(anoMes.Substring(0, 4));
                var mes = Convert.ToInt32(anoMes.Substring(4, 2));

                string nomeMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes).ToUpper();

                var linha2 = "PLANO DE MANUTENÇÃO, OPERAÇÃO E CONTROLE - PMOC - QTDE  DE TAREFAS REFERENTE AO MÊS DE " + nomeMes + " / " + ano.ToString();


                //var empresa = "TEKNO Sistema de Engenharia Ltda.";
                //var endereco = "End: Estrada dos Bandeirantes, 8592  -  Bairro: Camorim - Cidade: Rio de Janeiro - RJ.  CNPJ: 01.017.610/0001-60";

                var empresa = "";
                var endereco = "";

                using (var dc = new manutEntities())
                {
                    var e = dc.tb_empresa.Find(1); // sempre irá procurar pela chave primaria
                    if (e != null)
                    {
                        empresa = e.nome;
                        endereco = e.endereco;
                    }
                }

                using (var rd = new ReportDocument())
                {

                    var Response = HttpContext.Current.ApplicationInstance.Response;

                    rd.Load(local);
                    rd.SetParameterValue("@empresa", empresa);
                    rd.SetParameterValue("@linha1", endereco);
                    rd.SetParameterValue("@linha2", linha2);


                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    //Response.ClearContent();
                    //Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;

                }
            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;
        }


    }
}
