using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataFuncionarioController : ApiController
    {

        [HttpGet]
        public IEnumerable<funcionario> GetFuncionarioAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.funcionario.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }


        public IEnumerable<funcionario> GetAllFuncionario(string filtrado)
        {

            using (var dc = new manutEntities())
            {

                if (filtrado == "S")
                {
                    var user = from p in dc.funcionario.Where(a => a.cancelado != "S" && a.dataSaida == null) orderby p.nome select p;
                    return user.ToList();
                }
                else
                {
                    var user = from p in dc.funcionario.Where(a => (a.cancelado != "S")) orderby p.nome select p;
                    return user.ToList();
                }


            }

        }


        public IEnumerable<funcionario> GetAllFuncionarioProfissao(int autonumeroProfissao)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.funcionario.Where(a => a.cancelado != "S" && a.autonumeroProfissao == autonumeroProfissao) orderby p.nome select p;
                return user.ToList(); ;
            }

        }

        [HttpDelete]
        public string CancelarFuncionario()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.funcionario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.funcionario.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        //public string AddPedido(Int64 autonumeroProduto, string codBarra, string nomeProduto,

        public string IncluirFuncionario()
        {
            using (var dc = new manutEntities())
            {
                var Funcionario = new funcionario
                {
                    nomeProfissao = "",
                    nome = "",
                    email = "",
                    telefone = "",
                    matricula = "",
                    cancelado = "N",
                    autonumeroProfissao = 0,
                    salario = 0,
                    obs = "",
                    autonumeroContrato = 0,
                    nomeContrato = "",
                    


                };

                dc.funcionario.Add(Funcionario);
                dc.SaveChanges();
                var auto = Convert.ToInt32(Funcionario.autonumero);

                return auto.ToString("#######0");
            }
        }

        [HttpPost]
        public void IncluirAlterarFuncionario()
        {

            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);

            using (var dc = new manutEntities())
            {
                var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString().Trim());
                var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var nomeProfissao = HttpContext.Current.Request.Form["nomeProfissao"].ToString().Trim();
                var email = HttpContext.Current.Request.Form["email"].ToString().Trim();
                var matricula = HttpContext.Current.Request.Form["matricula"].ToString().Trim();
                var telefone = HttpContext.Current.Request.Form["telefone"].ToString().Trim();
                var autonumeroProfissao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroProfissao"].ToString().Trim());
                var cc = HttpContext.Current.Request.Form["salario"].ToString().Trim();
                var salario = Convert.ToDecimal(HttpContext.Current.Request.Form["salario"].ToString().Trim());

                var obs = HttpContext.Current.Request.Form["obs"].ToString().Trim();
                var cpf = HttpContext.Current.Request.Form["cpf"].ToString().Trim();

                DateTime? dataSaida = DateTime.Now;
                if (IsDate(HttpContext.Current.Request.Form["dataSaida"].ToString()))
                {
                    dataSaida = Convert.ToDateTime(HttpContext.Current.Request.Form["dataSaida"].ToString());
                }
                else
                {
                    dataSaida = null;
                }

                DateTime? dataEntrada = DateTime.Now;
                if (IsDate(HttpContext.Current.Request.Form["dataEntrada"].ToString()))
                {
                    dataEntrada = Convert.ToDateTime(HttpContext.Current.Request.Form["dataEntrada"].ToString());
                }
                else
                {
                    dataEntrada = null;
                }
                if (autonumero == 0)
                {
                    var Funcionario = new funcionario
                    {
                        autonumeroContrato = autonumeroContrato,
                        nomeContrato = nomeContrato,
                        nomeProfissao = nomeProfissao,
                        nome = nome,
                        email = email,
                        telefone = telefone,
                        matricula = matricula,
                        cancelado = "N",
                        autonumeroProfissao = autonumeroProfissao,
                        salario = salario,
                        dataSaida = dataSaida,
                        dataEntrada = dataEntrada,
                        obs = obs,
                        cpf = cpf


                    };

                    dc.funcionario.Add(Funcionario);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(Funcionario.autonumero);

                }
                else
                {

                    var linha = dc.funcionario.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.nome = nome.Trim();
                        linha.nomeProfissao = nomeProfissao.Trim();
                        linha.nome = nome.Trim();
                        linha.email = HttpContext.Current.Request.Form["email"].ToString().Trim();
                        linha.matricula = matricula.Trim();
                        linha.telefone = telefone.Trim();
                        linha.autonumeroProfissao = autonumeroProfissao;
                        linha.salario = salario;
                        linha.dataSaida = dataSaida;
                        linha.dataEntrada = dataEntrada;
                        linha.obs = obs;
                        linha.cpf = cpf;

                        dc.funcionario.AddOrUpdate(linha);
                        dc.SaveChanges();

                    }
                }

            }

        }


        [HttpPost]
        public void CopiarFuncionarioOutroContrato()
        {




            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumeroFuncionario = Convert.ToInt32(auto2);


            var autonumeroNovo = HttpContext.Current.Request.Form["autonumeroNovoContrato"].ToString();
            if (string.IsNullOrEmpty(autonumeroNovo))
            {
                autonumeroNovo = "0";
            }
            var autonumeroNovoContrato = Convert.ToInt32(autonumeroNovo);

            var nomeNovoContrato = HttpContext.Current.Request.Form["nomeNovoContrato"].ToString();

            using (var dc = new manutEntities())
            {

                try
                {

                    var autonumeroFuncionarioNovo = 0;

                    var linha = dc.funcionario.Find(autonumeroFuncionario); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {

                        linha.autonumeroContrato = autonumeroNovoContrato;
                        linha.nomeContrato = nomeNovoContrato;
                        dc.funcionario.Add(linha);
                        dc.SaveChanges();

                        autonumeroFuncionarioNovo = linha.autonumero;

                    }

                    dc.funcionariocurso.Where(a => a.cancelado != "S" && a.autonumeroFuncionario == autonumeroFuncionario).ToList().ForEach(x =>
                    {
                        x.autonumeroFuncionario = autonumeroFuncionarioNovo;
                        dc.funcionariocurso.Add(x);
                        dc.SaveChanges();
                    });

                    dc.funcionariodocumento.Where(a => a.autonumeroFuncionario == autonumeroFuncionario).ToList().ForEach(x =>
                    {
                        x.autonumeroFuncionario = autonumeroFuncionarioNovo;
                        dc.funcionariodocumento.Add(x);
                        dc.SaveChanges();
                    });
                }
                catch (Exception ex)
                {
                    var c1 = 1;
                    var c2 = ex;
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

        public IEnumerable<funcionario> GetAllFuncionarioDoContrato(int autonumeroContrato, string filtrado, string inativo)
        {

            using (var dc = new manutEntities())
            {


                if (filtrado == "S")
                {
                    var user = from p in dc.funcionario.Where(a => a.cancelado != "S" && a.autonumeroContrato == autonumeroContrato
                       && a.dataSaida == null)
                               orderby p.nome
                               select p;
                    return user.ToList();
                }
                else
                {
                    var l = (from p in dc.funcionario.Where(a => a.cancelado != "S" && a.autonumeroContrato == autonumeroContrato && a.dataSaida == null) select p).OrderBy(p => p.nome).ToList();
                    var l2 = (from p in dc.funcionario.Where(a => a.cancelado == "X") select p).ToList();  // Para pegar a estrutura;


                    if (inativo == "S")
                    {
                        l2 = (from p in dc.funcionario.Where(a => a.cancelado != "S" && a.autonumeroContrato == autonumeroContrato && a.dataSaida != null) select p).OrderBy(p => p.nome).ToList();
                    }
                    return (l.OrderBy(p => p.nome).Union(l2.OrderBy(p => p.nome))).ToList();
                }
            }

        }



    }
}

