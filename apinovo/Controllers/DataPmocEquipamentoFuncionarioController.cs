using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataPmocEquipamentoFuncionarioController : ApiController
    {

        [HttpGet]
        public IEnumerable<pmocequipamentofuncionario> GetCodigoPmocEquipamentoFuncionario(long autonumeroContrato, string anoMes)
        {
            anoMes = anoMes.Replace("/", "");
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.pmocequipamentofuncionario.Where(a => a.autonumeroContrato == autonumeroContrato && a.anoMes == anoMes && a.cancelado != "S") select p).OrderBy(p => p.nomeProfissao); ;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<pmocequipamentofuncionario> GetPmocEquipamentoFuncionarioAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.pmocequipamentofuncionario.Where(a => a.autonumero == autonumero && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<pmocequipamentofuncionario> GetAllPmocEquipamentoFuncionario(long autonumeroContrato)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.pmocequipamentofuncionario where p.autonumeroContrato == autonumeroContrato && p.cancelado != "S" select p).OrderBy(p => p.anoMes);
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarPmocEquipamentoFuncionario()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);

            var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
            var autonumeroFuncionario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString());
            var nomeFuncionario = HttpContext.Current.Request.Form["nomeFuncionario"].ToString().Trim();
            var autonumeroProfissao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroProfissao"].ToString());
            var nomeProfissao = HttpContext.Current.Request.Form["nomeProfissao"].ToString().Trim();
            var anoMes = HttpContext.Current.Request.Form["anoMes"].ToString();
            var totalHoras = TimeSpan.Parse(HttpContext.Current.Request.Form["totalHoras"].ToString().Trim());


            if (autonumero == 0)
            {


                using (var dc = new manutEntities())
                {
                    var k = new pmocequipamentofuncionario
                    {
                        nomeContrato = nomeContrato,
                        autonumeroContrato = autonumeroContrato,
                        autonumeroFuncionario = autonumeroFuncionario,
                        nomeFuncionario = nomeFuncionario,
                        autonumeroProfissao = autonumeroProfissao,
                        nomeProfissao = nomeProfissao,
                        anoMes = anoMes.Replace("/", ""),
                        totalHoras = totalHoras,
                        cancelado = "N"

                    };

                    dc.pmocequipamentofuncionario.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.pmocequipamentofuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.totalHoras = totalHoras;
                        dc.pmocequipamentofuncionario.AddOrUpdate(linha);
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarPmocEquipamentoFuncionario()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.pmocequipamentofuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.pmocequipamentofuncionario.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }


    }
}
