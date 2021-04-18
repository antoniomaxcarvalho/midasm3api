using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataSetorCivilController : ApiController
    {
        [HttpGet]
        public IEnumerable<setor> GetAllSetorPredio(int autonumeroPredio)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.setor.Where(a => a.autonumeroPredio == autonumeroPredio && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<setor> GetSetorAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.setor.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }



        [HttpGet]
        public IEnumerable<setor> GetAllSetor()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.setor where p.cancelado != "S" orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarSetor()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();

            if (autonumero == 0)
            {
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();

                using (var dc = new manutEntities())
                {
                    var k = new setor
                    {
                        nome = nome,
                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        autonumeroPredio = autonumeroPredio,
                        nomePredio = nomePredio,
                        cancelado = "N"

                    };

                    dc.setor.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.setor.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroSetor = linha.autonumero;
                        linha.nome = nome;
                        dc.setor.AddOrUpdate(linha);
                        dc.SaveChanges();

                        //dc.tb_os.Where(x => x.autonumeroSetor == autonumeroSetor && x.cancelado != "S" && x.nomeSetor != nome).ToList().ForEach(x =>
                        //{
                        //    x.nomeSetor = nome;
                        //});
                        //dc.SaveChanges();


                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarSetor()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.setor.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.setor.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

    }
}
