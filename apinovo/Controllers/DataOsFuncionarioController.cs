using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataOsFuncionarioController : ApiController
    {

        [HttpGet]
        public IEnumerable<osfuncionario> GetCodigoOsFuncionario(long autonumeroOs)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.osfuncionario.Where(a => a.autonumeroOs == autonumeroOs && a.cancelado != "S") select p).OrderBy(p => p.nomeProfissao); ;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<osfuncionario> GetOsFuncionarioAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.osfuncionario.Where(a => a.autonumero == autonumero && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<osfuncionario> GetAllOsFuncionario(long autonumeroOs)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.osfuncionario where p.autonumeroOs == autonumeroOs && p.cancelado != "S" select p).OrderBy(p => p.nomeFuncionario);
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarOsFuncionario()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();

            var autonumeroOs = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOs"].ToString());
            var codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
            var autonumeroFuncionario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString());
            var nomeFuncionario = HttpContext.Current.Request.Form["nomeFuncionario"].ToString().Trim();
            var autonumeroProfissao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroProfissao"].ToString());
            var nomeProfissao = HttpContext.Current.Request.Form["nomeProfissao"].ToString().Trim();

            // Pegar Horas maior que 24H ----------------------------------------------------------
            string input = HttpContext.Current.Request.Form["totalHoras"].ToString().Trim();
            var parts = input.Split(':');
            var hours = Int32.Parse(parts[0]);
            var minutes = Int32.Parse(parts[1]);
            var totalHoras = new TimeSpan(hours, minutes, 0);
            // Fim - Pegar Horas maior que 24H ----------------------------------------------------------

            // Calcular o Nro de Segundos da hora  ----------------------------------------------------------
            input = input + ":00";
            int[] ssmmhh = { 0, 0, 0 };
            var hhmmss = input.ToString().Split(':');
            var reversed = hhmmss.Reverse();
            int i = 0;
            reversed.ToList().ForEach(x => ssmmhh[i++] = int.Parse(x));
            var seconds = (int)(new TimeSpan(ssmmhh[2], ssmmhh[1], ssmmhh[0])).TotalSeconds;
            // Fim - Calcular o Nro de Segundos da hora  ----------------------------------------------------------



            if (autonumero == 0)
            {


                using (var dc = new manutEntities())
                {
                    var k = new osfuncionario
                    {
                        autonumeroContrato = autonumeroContrato,
                        nomeContrato = nomeContrato,
                        autonumeroOs = autonumeroOs,
                        codigoOs = codigoOs,
                        autonumeroFuncionario = autonumeroFuncionario,
                        nomeFuncionario = nomeFuncionario,
                        autonumeroProfissao = autonumeroProfissao,
                        nomeProfissao = nomeProfissao,
                        totalHoras = totalHoras,
                        totalSegundos = seconds,
                        cancelado = "N"

                    };

                    dc.osfuncionario.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.osfuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.totalHoras = totalHoras;
                        dc.osfuncionario.AddOrUpdate(linha);
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";



        }


        [HttpDelete]
        public string CancelarOsFuncionario()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.osfuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.osfuncionario.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

    }
}
