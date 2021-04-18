using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace apinovo.Controllers
{
    public class DataDocumentoContratoController : ApiController
    {
        [HttpGet]
        public IEnumerable<documentocontrato> GetDocumentoContratoAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.documentocontrato.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<documentocontrato> GetAllDocumentoContrato(int autonumeroContrato)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.documentocontrato.Where(a => a.autonumeroContrato == autonumeroContrato) orderby p.nome select p;
                return user.ToList(); ;
            }

        }


        [HttpDelete]
        public string CancelarDocumentoContrato()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.documentocontrato.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {

                    dc.documentocontrato.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        public void IncluirDocumentoContrato()
        {

            using (var dc = new manutEntities())
            {


                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
                var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString().Trim());


                var Funcionario = new documentocontrato
                {
                    autonumeroContrato = autonumeroContrato,
                    nomeContrato = nomeContrato,
                    nome = nome,

                };

                dc.documentocontrato.Add(Funcionario);
                dc.SaveChanges();
                var auto = Convert.ToInt32(Funcionario.autonumero);



            }

        }

    }
}
