using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataEtapaController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_etapa> GetAllEtapaCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_etapa.Where(a => a.autonumeroCliente == autonumeroCliente) orderby p.sequencia select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<tb_etapa> GetAllEtapa()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_etapa orderby p.sequencia select p;
                return user.ToList(); ;
            }
        }
        [HttpPost]
        public string IncluirEtapa()
        {
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var etapa = HttpContext.Current.Request.Form["etapa"].ToString().Trim();
            var valorGlobal = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobal"].ToString());
            var sequencia = Convert.ToInt32(HttpContext.Current.Request.Form["sequencia"].ToString());
            using (var dc = new manutEntities())
            {
                var k = new tb_etapa
                {
                    etapa = etapa,
                    autonumeroCliente = autonumeroCliente,
                    valorGlobal = valorGlobal,
                    sequencia = sequencia
                };

                dc.tb_etapa.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }
        [HttpDelete]
        public string CancelarEtapa()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_etapa.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.tb_etapa.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }
    }
}
