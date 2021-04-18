using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataPredioCivilController : ApiController
    {
        [HttpGet]
        public IEnumerable<predio> GetAllPredioCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.predio.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<predio> GetPredioAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.predio.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }



        [HttpGet]
        public IEnumerable<predio> GetAllPredio()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.predio where p.cancelado != "S" orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarPredio()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
            var codigoMunicipioIBGE = HttpContext.Current.Request.Form["codigoMunicipioIBGE"].ToString();
            var nomeMunicipio = HttpContext.Current.Request.Form["nomeMunicipio"].ToString().Trim();
            var preventiva = Convert.ToDecimal(HttpContext.Current.Request.Form["preventiva"].ToString());
            var corretiva = Convert.ToDecimal(HttpContext.Current.Request.Form["corretiva"].ToString());
            var inspecao = Convert.ToDecimal(HttpContext.Current.Request.Form["inspecao"].ToString());
            var endereco = HttpContext.Current.Request.Form["endereco"].ToString();

            if (autonumero == 0)
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();

                using (var dc = new manutEntities())
                {
                    var k = new predio
                    {
                        nome = nome,

                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        codigoMunicipioIBGE = codigoMunicipioIBGE,
                        nomeMunicipio = nomeMunicipio,

                        cancelado = "N",
                        preventiva = preventiva,
                        corretiva = corretiva,
                        inspecao = inspecao,
                        endereco = endereco,
                        contadorPmocCivil = 0,
                    };

                    dc.predio.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.predio.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroPredio = linha.autonumero;
                        linha.codigoMunicipioIBGE = codigoMunicipioIBGE;
                        linha.nomeMunicipio = nomeMunicipio;
                        linha.nome = nome;
                        linha.corretiva = corretiva;
                        linha.preventiva = preventiva;
                        linha.inspecao = inspecao;
                        linha.endereco = endereco;
                        dc.predio.AddOrUpdate(linha);
                        dc.SaveChanges();

                        //dc.os.Where(x => x.autonumeroPredio == autonumeroPredio && x.cancelado != "S" && (x.nomePredio != nome || x.nomeMunicipio != nomeMunicipio || x.endereco != endereco)).ToList().ForEach(x =>
                        //{
                        //    x.nomePredio = nome;
                        //    x.nomeMunicipio = nomeMunicipio;
                        //    x.endereco = endereco;
                        //});
                        //dc.SaveChanges();

                        //dc.local.Where(x => x.autonumeroPredio == autonumeroPredio && x.cancelado != "S" && x.nomePredio != nome).ToList().ForEach(x =>
                        //{
                        //    x.nomePredio = nome;
                        //});
                        //dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarPredio()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.predio.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.predio.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }



    }
}
