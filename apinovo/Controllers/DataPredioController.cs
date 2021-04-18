using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataPredioController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_predio> GetAllPredioCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_predio.Where(a => a.autonumeroCliente == autonumeroCliente) orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_predio> GetAllPredio()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_predio orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirPredio()
        {
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
            using (var dc = new manutEntities())
            {
                var k = new tb_predio
                {
                    nome = nomePredio,
                    autonumeroCliente = autonumeroCliente,
                };

                dc.tb_predio.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }
        [HttpDelete]
        public string CancelarPredio()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_predio.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null )
                {
                    dc.tb_predio.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }
    }
}
