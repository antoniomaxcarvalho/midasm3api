using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataFuncionarioCursoController : ApiController
    {
        [HttpGet]
        public IEnumerable<funcionariocurso> GetFuncionarioCursoAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.funcionariocurso.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<funcionariocurso> GetAllFuncionarioCurso(int autonumeroFuncionarioCurso)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.funcionariocurso.Where(a => a.cancelado != "S" && a.autonumeroFuncionario == autonumeroFuncionarioCurso) orderby p.nomeFuncionario select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable GetAllFuncionarioCursoForaValidade(int autonumeroContrato)
        {
            var hoje30Dias = DateTime.Now.AddMonths(1);
            var hoje = DateTime.Now;

            using (var dc = new manutEntities())
            {
                var lista3 = (from p in dc.funcionariocurso.Where(a => a.cancelado != "S" && a.validade < hoje30Dias && a.validade >= hoje) select p).ToList();
                var lista4 = (from i in dc.funcionario.Where(i => i.autonumeroContrato == autonumeroContrato && i.cancelado != "S") select i).ToList();

                var lista5 = (from k in lista3
                              join i in lista4 on k.autonumeroFuncionario equals i.autonumero

                              select new
                              {

                                  i.autonumero

                              }).ToList();



                return lista5.ToList(); ;
            }

        }

        [HttpDelete]
        public string CancelarFuncionarioCurso()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.funcionariocurso.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.funcionariocurso.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        public void IncluirAlterarFuncionarioCurso()
        {

            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);

            using (var dc = new manutEntities())
            {


                var nomeFuncionario = HttpContext.Current.Request.Form["nomeFuncionario"].ToString().Trim();
                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var autonumeroFuncionario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString().Trim());

                DateTime? inicioCurso = DateTime.Now;
                DateTime? fimCurso = DateTime.Now;
                DateTime? validade = DateTime.Now;

                if (IsDate(HttpContext.Current.Request.Form["inicioCurso"].ToString()))
                {
                    inicioCurso = Convert.ToDateTime(HttpContext.Current.Request.Form["inicioCurso"].ToString());
                }
                else
                {
                    inicioCurso = null;
                }
                if (IsDate(HttpContext.Current.Request.Form["fimCurso"].ToString()))
                {
                    fimCurso = Convert.ToDateTime(HttpContext.Current.Request.Form["fimCurso"].ToString());
                }
                else
                {
                    fimCurso = null;
                }
                if (IsDate(HttpContext.Current.Request.Form["validade"].ToString()))
                {
                    validade = Convert.ToDateTime(HttpContext.Current.Request.Form["validade"].ToString());
                }
                else
                {
                    validade = null;
                }


                if (autonumero == 0)
                {
                    var Funcionario = new funcionariocurso
                    {
                        autonumeroFuncionario = autonumeroFuncionario,
                        nomeFuncionario = nomeFuncionario,
                        nome = nome,
                        inicioCurso = inicioCurso,
                        fimCurso = fimCurso,
                        validade = validade,
                        cancelado = "N"
                    };

                    dc.funcionariocurso.Add(Funcionario);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(Funcionario.autonumero);

                }
                else
                {

                    var linha = dc.funcionariocurso.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.inicioCurso = inicioCurso;
                        linha.fimCurso = fimCurso;
                        linha.validade = validade;
                        linha.nome = nome;
                        dc.funcionariocurso.AddOrUpdate(linha);
                        dc.SaveChanges();

                    }
                }

            }

        }

        public static bool IsDate(string MyString)
        {
            try
            {
                DateTime.Parse(MyString); // or Double.Parse if you want to use double
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
