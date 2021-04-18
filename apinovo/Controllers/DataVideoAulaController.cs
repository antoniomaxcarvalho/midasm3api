using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class DataVideoAulaController : ApiController
    {

        [HttpGet]
        public IEnumerable<videoaula> GetAllVideoAula()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.videoaula orderby p.descricao select p;
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarVideoAula()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.videoaula.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.videoaula.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]
        public string UploadFile()
        {
            var c = 1;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
          

            //var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();

            //if (HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];


         
                if (httpPostedFile != null)
                {


                    var descricao = HttpContext.Current.Request.Form["descricao"].ToString().Trim();

                    var caminho = "~/UploadedFiles/VideoAula/";

                    // Criar a pasta se não existir ou devolver informação sobre a pasta
                    var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));

                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);


                    using (var dc = new manutEntities())
                    {
                        var k = new videoaula
                        {

                            url = fileName,
                            descricao = descricao,

                        };

                        dc.videoaula.Add(k);
                        dc.SaveChanges();
                        var auto = Convert.ToInt32(k.autonumero);

                        return auto.ToString("#######0");
                    }
                }



            }
          
            catch (Exception ex)
            {
                var c2 = ex;
            }

            return string.Empty;

        }


        //[HttpPost]
        //public string SaveFiles()
        //{
        //    // Faz 2 Coisas: Salva o arquivo na pasta ~/UploadedFiles e armazena sobre o arquivo no banco de dados


        //    var message = string.Empty;
        //    bool flag = false;
        //    if (HttpContext.Current.Request.Files != null)
        //    {
        //        var file = Request.Files[0];
        //        if (file != null)
        //        {
        //            string actualFileName = file.FileName;
        //            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //            int size = file.ContentLength;

        //            try
        //            {
        //                // salvar o arquivo na pasta da aplicacao
        //                file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), fileName));

        //                var f = new uploadedfiles
        //                {
        //                    fileName = actualFileName,
        //                    filePath = fileName,
        //                    descripition = description,
        //                    curso = curso,
        //                    periodo = periodo,
        //                    fileSize = size,
        //                    video = video

        //                };
        //                // Atualizar no BD ( tabela uploadedfiles )
        //                using (var dc = new comumEntities())
        //                {
        //                    dc.uploadedfiles.Add(f);
        //                    dc.SaveChanges();
        //                    message = "Sucesso Upload";
        //                    flag = true;
        //                }


        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                var sb = new StringBuilder();
        //                foreach (var eve in e.EntityValidationErrors)
        //                {
        //                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                                                    eve.Entry.Entity.GetType().Name,
        //                                                    eve.Entry.State));
        //                    foreach (var ve in eve.ValidationErrors)
        //                    {
        //                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
        //                                                    ve.PropertyName,
        //                                                    ve.ErrorMessage));
        //                    }
        //                }
        //                message = sb.ToString();
        //                //throw new DbEntityValidationException(sb.ToString(), e);
        //            }
        //            catch (Exception ex)
        //            {
        //                message = "Falha - Upload, Tente Outra Vez";

        //            }
        //            finally
        //            {
        //                var c = new JsonResult { Data = new { Message = message, Status = flag } };
        //            }
        //        }
        //    }
        //    return new JsonResult { Data = new { Message = message, Status = flag } };
        //}


        [HttpGet]
        public HttpResponseMessage DownloadFile(string nomeArquivo)
        {

            var caminho = "~/UploadedFiles/VideoAula/";
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
