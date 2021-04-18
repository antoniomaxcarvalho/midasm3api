using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataOrcamentoController : ApiController
    {

        [HttpGet]
        public IEnumerable<tb_orcamento> GetOrcamento(string codigoOrcamento)
        {
            using (var dc = new manutEntities())
            {
                var cod = codigoOrcamento.Trim();
                var user = from p in dc.tb_orcamento.Where((a => a.codigoOrcamento.Contains(cod))) select p;
                return user.ToList(); ;
            }

        }

        [HttpGet]
        public IEnumerable<tb_orcamento> GetOrcamentoAutonumero(Int64 autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_orcamento.Where((a => (a.autonumero == autonumero))) select p;
                return user.ToList(); ;
            }
        }

        public IEnumerable<tb_orcamento> GetAllOrcamentoCliente(Int64 autonumeroCliente)
        {
            IQueryable<tb_orcamento> user;

            using (var dc = new manutEntities())
            {
                user = from p in dc.tb_orcamento.Where(a => (a.cancelado != "S" && a.autonumeroCliente == autonumeroCliente)).OrderByDescending(p => p.autonumero) select p;

                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarOrcamento()
        {
            //var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var linha = dc.tb_orcamento.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {
                            linha.cancelado = "S";
                            dc.tb_orcamento.AddOrUpdate(linha);

                            dc.SaveChanges();
                            dc.tb_orcamento_itens.Where(x => x.autonumeroOrcamento == autonumero).ToList().ForEach(x =>
                            {
                                x.cancelado = "S";
                            });
                            dc.SaveChanges();


                        }

                        //Debug.WriteLine("Commit");
                        transaction.Commit();
                        return string.Empty;
                    }

                    catch (Exception)
                    {
                        transaction.Rollback();
                        return "Erro";
                    }

                }

            }
        }


        [HttpPost]
        public tb_orcamento IncluirOrcamento()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {

                    var sigla = HttpContext.Current.Request.Form["sigla"].ToString();
                    var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                    var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString();
                    var descricao = HttpContext.Current.Request.Form["descricao"].ToString();
                    var localObra = HttpContext.Current.Request.Form["localObra"].ToString();

                    var Orcamento = new tb_orcamento
                    {

                        codigoOrcamento = "",
                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString().Trim()),
                        nomeUsuario = nomeUsuario,
                        nomeStatus = "Aberta",
                        cancelado = "N",
                        valor = 0,
                        autonumeroOs = 0,
                        codigoOs = "",
                        siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString(),
                        descricao = descricao,
                        localObra = localObra

                    };

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
                    {
                        Orcamento.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (Orcamento.dataInicio > dataLimite)
                                {
                                    Orcamento.dataInicio = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }
                    }




                    var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linha != null)
                    {
                        linha.contadorOrcamento = linha.contadorOrcamento + 1;

                        dc.tb_cliente.AddOrUpdate(linha);
                        dc.SaveChanges();

                        var contadorOrcamentoCliente = Convert.ToInt32(linha.contadorOrcamento);
                        var codigo = sigla + "-ORC" + contadorOrcamentoCliente.ToString("00000") + "-" + DateTime.Now.ToString("yyyy") + "-ORC";
                        Orcamento.codigoOrcamento = codigo;

                        dc.tb_orcamento.AddOrUpdate(Orcamento);
                        dc.SaveChanges();

                        //var autoOsNova = Convert.ToInt32(Orcamento.autonumero);

                        transaction.Commit();

                        return Orcamento;

                    }
                    transaction.Rollback();

                }
                return null;
            }

        }


        public IEnumerable<tb_orcamento> GetAllOrcamentoData(string clientesDoUsuario, string data1, string data2)
        {
            IQueryable<tb_orcamento> user;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                if (!string.IsNullOrEmpty(clientesDoUsuario))
                {
                    user = from p in dc.tb_orcamento.Where((a => a.cancelado != "S" && clientesDoUsuario.Contains(a.siglaCliente)
                           && ((a.dataInicio >= data11 && a.dataInicio <= data22) ||
                           (a.dataInicio <= data11 && a.nomeStatus != "Fechada"))
                           )).OrderByDescending(p => p.autonumero)
                           select p;
                }
                else
                {
                    user = from p in dc.tb_orcamento.Where((a => a.cancelado != "S"
                        && ((a.dataInicio >= data11 && a.dataInicio <= data22) ||
                        (a.dataInicio <= data11 && a.nomeStatus != "Fechada"))
                        )).OrderByDescending(p => p.autonumero)
                           select p;
                }
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string AlterarOrcamento()
        {

            using (var dc = new manutEntities())
            {

                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"].ToString());
                var descricao = HttpContext.Current.Request.Form["descricao"].ToString();
                var localObra = HttpContext.Current.Request.Form["localObra"].ToString();

                var linha = dc.tb_orcamento.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.descricao = HttpContext.Current.Request.Form["descricao"].ToString().Trim();
                    linha.localObra = HttpContext.Current.Request.Form["localObra"].ToString().Trim();
                    dc.tb_orcamento.AddOrUpdate(linha);
                    dc.SaveChanges();
                }
                return "";
            }

        }



    }
}
