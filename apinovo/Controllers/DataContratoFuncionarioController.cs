using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace apinovo.Controllers
{
    public class DataContratoFuncionarioController : ApiController
    {

        [HttpGet]
        public IEnumerable<contratofuncionario> GetFuncionarioContratoAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.contratofuncionario.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<contratofuncionario> GetAllFuncionarioContrato(int autonumeroFuncionarioContrato)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.contratofuncionario.Where(a => a.cancelado != "S" && a.autonumeroFuncionario == autonumeroFuncionarioContrato) orderby p.nomeFuncionario select p;
                return user.ToList(); ;
            }

        }

        [HttpDelete]
        public string CancelarFuncionarioContrato()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.contratofuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.contratofuncionario.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }


        [HttpPost]
        public void IncluirAlterarFuncionarioContrato()
        {
            var c = 1;
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);

            using (var dc = new manutEntities())
            {
                var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
                var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString().Trim());

                var nomeFuncionario = HttpContext.Current.Request.Form["nomeFuncionario"].ToString().Trim();
                var autonumeroFuncionario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString().Trim());



                if (autonumero == 0)
                {
                    var Funcionario = new contratofuncionario
                    {
                        nomeFuncionario = nomeFuncionario,
                        autonumeroFuncionario = autonumeroFuncionario,
                        autonumeroContrato = autonumeroContrato,
                        nomeContrato = nomeContrato,
                        cancelado = "N"

                    };

                    dc.contratofuncionario.Add(Funcionario);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(Funcionario.autonumero);

                }
                else
                {

                    //var linha = dc.contratofuncionario.Find(autonumero); // sempre irá procurar pela chave primaria
                    //if (linha != null && linha.cancelado != "S")
                    //{
                    //    linha.nomeFuncionario = nomeFuncionario.Trim();
                    //    linha.autonumeroContrato = autonumeroContrato;
                    //    dc.contratofuncionario.AddOrUpdate(linha);
                    //    dc.SaveChanges();

                    //}
                }

            }

        }


    }
}
