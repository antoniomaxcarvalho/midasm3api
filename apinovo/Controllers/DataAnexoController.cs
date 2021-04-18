using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataAnexoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_anexo> GetAllAnexoCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_anexo.Where(a => a.autonumeroCliente == autonumeroCliente) orderby p.descricao select p;
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarAnexo()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_anexo.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.tb_anexo.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]
        public string UploadFile()
        {
            var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();
            //try
            //{

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    var siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString().Trim();
                    var descricao = HttpContext.Current.Request.Form["descricao"].ToString().Trim();
                    var caminho = "~/UploadedFiles/" + siglaCliente + "/";

                    // Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);


                    using (var dc = new manutEntities())
                    {
                        var k = new tb_anexo
                        {
                            siglaCliente = siglaCliente,
                            autonumeroCliente = autonumeroCliente,
                            url = fileName,
                            descricao = descricao

                        };

                        dc.tb_anexo.Add(k);
                        dc.SaveChanges();
                        var auto = Convert.ToInt32(k.autonumero);

                        return auto.ToString("#######0");
                    }
                }

            }

            return message;


            //}
            //catch (DbEntityValidationException e)
            //{
            //    var sb = new StringBuilder();
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name,
            //            eve.Entry.State));
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName,
            //                ve.ErrorMessage));
            //        }
            //    }
            //    message = sb.ToString();
            //    //throw new DbEntityValidationException(sb.ToString(), e);
            //}
            //catch (Exception ex)
            //{
            //    message = ex.InnerException != null
            //        ? ex.InnerException.ToString().Substring(0, 130) + " -  SaveFiles"
            //        : ex.Message + " - DataprodutoController SaveFilesFoto";

            //}
            //return message;
        }

        [HttpGet]
        public HttpResponseMessage DownloadFile(string siglaCliente, string nomeArquivo)
        {

            var caminho = "~/UploadedFiles/" + siglaCliente + "/";

            // Criar a pasta se não existir ou devolver informação sobre a pasta
            //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));

            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), RemoveCaracteresEspeciais(nomeArquivo));

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fileSavePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileSavePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }


        public static string RemoveCaracteresEspeciais(string texto)
        {
            texto = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in texto.ToCharArray())
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            return RemoveAcentos_BH(sb.ToString());
        }

        public static string RemoveAcentos_BH(string _textoNAOFormatado)
        {

            var ret = Regex.Replace(_textoNAOFormatado, "[^0-9a-zA-Z .,]+", " ");
            return ret;
        }


    }
}
