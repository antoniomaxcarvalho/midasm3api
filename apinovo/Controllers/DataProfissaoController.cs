using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataProfissaoController : ApiController
    {


        [HttpGet]
        public IEnumerable<profissao> GetProfissaoAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.profissao.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList();
            }
        }


        [HttpGet]
        public IEnumerable GetAllProfissao()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.profissao.Where(a => a.cancelado != "S") orderby p.nome select p;
                return user.ToList();

            }
            //using (var dc = new fmaxEntities())
            //{
            //    var user = (from i in dc.curso

            //                where i.cancelado != "S"
            //                group i by new { i.habilitacao } into g
            //                select new
            //                {
            //                    g.Key.habilitacao

            //                }).OrderBy(i => i.habilitacao);
            //    return user.ToList(); ;
            //}
        }



        [HttpPost]
        public string IncluirAlterarProfissao()
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

                using (var dc = new manutEntities())
                {
                    var k = new profissao
                    {
                        nome = nome,
                        cancelado = "N"
                    };


                    dc.profissao.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.profissao.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroProfissao = linha.autonumero;

                        linha.nome = nome;
                        dc.profissao.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.funcionario.Where(x => x.autonumeroProfissao == autonumeroProfissao && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeProfissao = nome;
                        });
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarProfissao()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.profissao.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.profissao.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

    }
}
