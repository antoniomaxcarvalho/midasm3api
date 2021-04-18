using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataProfissaoCursoController : ApiController
    {


        [HttpGet]
        public IEnumerable<profissaocurso> GetProfissaoCursoAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.profissaocurso.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<profissaocurso> GetAllProfissaoCurso(int autonumeroProfissao)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.profissaocurso.Where(a => a.autonumeroProfissao == autonumeroProfissao) orderby p.nomeProfissao select p;
                return user.ToList(); ;
            }

        }


        [HttpDelete]
        public string CancelarProfissaoCurso()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.profissaocurso.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {

                    dc.profissaocurso.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        public void IncluirProfissaoCurso()
        {

            using (var dc = new manutEntities())
            {


                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var nomeProfissao = HttpContext.Current.Request.Form["nomeProfissao"].ToString().Trim();
                var autonumeroProfissao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroProfissao"].ToString().Trim());


                var Funcionario = new profissaocurso
                {
                    autonumeroProfissao = autonumeroProfissao,
                    nomeProfissao = nomeProfissao,
                    nome = nome,

                };

                dc.profissaocurso.Add(Funcionario);
                dc.SaveChanges();
                var auto = Convert.ToInt32(Funcionario.autonumero);



            }

        }


    }
}
