using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataPmocCivilContingenciaController : ApiController
    {
        [HttpGet]
        public IEnumerable<pmoccivilcontingencia> GetCodigoPmocCivilContingencia(long codigoPmoc)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.pmoccivilcontingencia.Where(a => a.codigoPmoc == codigoPmoc && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<pmoccivilcontingencia> GetPmocCivilContingenciaAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.pmoccivilcontingencia.Where(a => a.autonumero == autonumero && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }



        [HttpGet]
        public IEnumerable<pmoccivilcontingencia> GetAllPmocCivilContingencia(long autonumeroContrato)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.pmoccivilcontingencia where p.autonumeroContrato == autonumeroContrato && p.cancelado != "S" select p).OrderBy(p => p.codigoPmoc);
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<pmoccivilcontingencia> GetAllPmocCivilContingenciaAnoMes(long autonumeroContrato, string anoMes)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.pmoccivilcontingencia where p.autonumeroContrato == autonumeroContrato && p.anoMes == anoMes && p.cancelado != "S" select p).OrderBy(p => p.codigoPmoc);
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarPmocCivilContingencia()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var autonumeroContrato = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
            var codigoPmoc = Convert.ToInt64(HttpContext.Current.Request.Form["codigoPmoc"].ToString());
            var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
            var obs = HttpContext.Current.Request.Form["obs"].ToString().Trim();
            var status = HttpContext.Current.Request.Form["status"].ToString().Trim();
            var nomeLocal = HttpContext.Current.Request.Form["nomeLocal"].ToString().Trim();

            if (autonumero == 0)
            {


                using (var dc = new manutEntities())
                {
                    var k = new pmoccivilcontingencia
                    {
                        nomeContrato = nomeContrato,
                        autonumeroContrato = autonumeroContrato,
                        codigoPmoc = codigoPmoc,
                        obs = obs,
                        status = status,
                        anoMes = anoMes.Replace("/", ""),
                        nomeLocal = nomeLocal,
                        cancelado = "N"

                    };

                    dc.pmoccivilcontingencia.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.pmoccivilcontingencia.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.nomeLocal = nomeLocal;
                        linha.obs = obs;
                        linha.status = status;
                        dc.pmoccivilcontingencia.AddOrUpdate(linha);
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarPmocCivilContingencia()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.pmoccivilcontingencia.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.pmoccivilcontingencia.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

    }
}
