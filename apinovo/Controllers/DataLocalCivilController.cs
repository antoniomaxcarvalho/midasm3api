using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataLocalCivilController : ApiController
    {
        [HttpGet]
        public IEnumerable<local> GetAllLocalSetor(int autonumeroSetor)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.local.Where(a => a.autonumeroSetor == autonumeroSetor && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<local> GetLocalAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.local.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }



        [HttpGet]
        public IEnumerable<local> GetAllLocal()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.local where p.cancelado != "S" orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarLocalCivil()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
            var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
            var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
            var autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
            var nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString().Trim();
            var autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
            var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
            var autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
            var nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString().Trim();

            if (autonumero == 0)
            {

                using (var dc = new manutEntities())
                {
                    var k = new local
                    {
                        nome = nome,
                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        autonumeroPredio = autonumeroPredio,
                        nomePredio = nomePredio,
                        autonumeroSetor = autonumeroSetor,
                        nomeSetor = nomeSetor,
                        autonumeroSistema = autonumeroSistema,
                        nomeSistema = nomeSistema,
                        autonumeroSubSistema = autonumeroSubSistema,
                        nomeSubSistema = nomeSubSistema,
                        cancelado = "N"

                    };

                    dc.local.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.local.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        //var autonumeroSetor = linha.autonumero;
                        linha.nomeSistema = nomeSistema;
                        linha.autonumeroSistema = autonumeroSistema;
                        linha.nomeSubSistema = nomeSubSistema;
                        linha.autonumeroSubSistema = autonumeroSubSistema;
                        linha.nome = nome;
                        dc.local.AddOrUpdate(linha);
                        dc.SaveChanges();

                        //dc.os.Where(x => x.autonumeroSetor == autonumeroSetor && x.cancelado != "S" && x.nomeSetor != nome).ToList().ForEach(x =>
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
        public string CancelarLocal()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.local.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.local.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }


        [HttpPost]
        public string CopiarCheckListCivil()
        {
            var c = 1;



            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
            var autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
            var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
            var autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
            var nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString().Trim();

            using (var dc = new manutEntities())
            {
                dc.local.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                {
                    x.autonumeroSistema = autonumeroSistema;
                    x.nomeSistema = nomeSistema;
                    x.autonumeroSubSistema = autonumeroSubSistema;
                    x.nomeSubSistema = nomeSubSistema;
                });
                dc.SaveChanges();

                return "";

            }

        }

    }
}
