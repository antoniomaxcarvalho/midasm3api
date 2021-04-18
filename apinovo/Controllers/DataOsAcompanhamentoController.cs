using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataOsAcompanhamentoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_os_acompanhamento> GetAcompanhamentoCodigoOs(string codigoOs)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os_acompanhamento.Where(a => (a.codigoOs == codigoOs && a.cancelado != "S")).OrderByDescending(p => p.data) select p;
                return user.ToList();
            }

        }
        [HttpGet]
        public IEnumerable<tb_os_acompanhamento> GetAcompanhamentoAutonumero(Int64 autonumeroOs)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os_acompanhamento.Where((a => (a.autonumeroOs == autonumeroOs) && a.cancelado != "S")).OrderByDescending(p => p.data) select p;
                return user.ToList(); ;
            }

        }



        [HttpDelete]
        public string CancelarAcompanhamento()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_os_acompanhamento.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_os_acompanhamento.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        public string IncluirItensAcompanhamento()
        {

            var c = 1;

            //var y = HttpContext.Current.Request.Form["dataAcompanhamento"].ToString();

            //var x = DateTime.Parse(HttpContext.Current.Request.Form["dataAcompanhamento"].ToString());

            using (var dc = new manutEntities())
            {
                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();

                var autonumeroOs = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroOs"].ToString());
                var codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();

                var nomeEquipe = HttpContext.Current.Request.Form["nomeEquipe"].ToString().Trim();
                //var autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString());

                var nomeStatus = HttpContext.Current.Request.Form["nomeStatus"].ToString().Trim();


                DateTime dataAcompanhamento = DateTime.Now;
                if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataAcompanhamento"].ToString()))
                {
                    dataAcompanhamento = Convert.ToDateTime(HttpContext.Current.Request.Form["dataAcompanhamento"].ToString());
                }

                if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                {
                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                    {
                        var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                        if (dataAcompanhamento > dataLimite)
                        {
                            dataAcompanhamento = dataLimite;
                        }
                    }
                }

                var dataAcomp = dataAcompanhamento;
                if (nomeStatus == "Fechada")
                {
                    var dataTermino = DateTime.Now;
                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataTermino"].ToString()))
                    {
                        dataTermino = Convert.ToDateTime(HttpContext.Current.Request.Form["dataTermino"].ToString());
                    }

                    dataAcomp = dataTermino;
                }

                    var Os = new tb_os_acompanhamento
                {
                    nome = nome,

                    autonumeroOs = autonumeroOs,
                    codigoOs = codigoOs,
                    autonumeroUsuario = autonumeroUsuario,
                    nomeUsuario = nomeUsuario,
                    cancelado = "N",
                    data = dataAcomp,
                    autonumeroCliente = autonumeroCliente,
                    nomeCliente = nomeCliente,
                    nomeEquipe = nomeEquipe,
                    //autonumeroEquipe = 0,
                    nomeStatus = nomeStatus
                };

                dc.tb_os_acompanhamento.Add(Os);
                dc.SaveChanges();

                var auto = Convert.ToInt32(Os.autonumero);

                var linha = dc.tb_os.Find(autonumeroOs); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {

                    if (nomeStatus == "Fechada")
                    {
                        //var dataTermino = DateTime.Now;

                        //if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataTermino"].ToString()))
                        //{
                        //    dataTermino = Convert.ToDateTime(HttpContext.Current.Request.Form["dataTermino"].ToString());
                        //}

                        //if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        //{
                        //    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                        //    {
                        //        var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());

                        //        var dl = dataLimite.Add(new TimeSpan());

                        //        if (dataTermino > dataLimite)
                        //        {
                        //            dataTermino = dataLimite;
                        //        }
                        //    }
                        //}

                        var totalHoras = TimeSpan.Parse(HttpContext.Current.Request.Form["totalHoras"].ToString());
                        linha.dataTermino = dataAcomp;
                        linha.totalHoras = totalHoras;
                 
                    }

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
                    {
                        linha.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                    }

                    if (nomeStatus == "Autorizado")
                    {
                        linha.autonumeroAutorizado = autonumeroUsuario;
                        linha.nomeAutorizado = nomeUsuario;
                    }


                    linha.nomeStatus = nomeStatus;
                    linha.nomeEquipe = nomeEquipe;
                    dc.tb_os.AddOrUpdate(linha);
                    dc.SaveChanges();
                }

                return auto.ToString("#######0");

            }
        }

        [HttpPost]
        public string ExisteOrdemServicoNaData()
        {
            //var y = HttpContext.Current.Request.Form["dataAcompanhamento"].ToString();

            //var x = DateTime.Parse(HttpContext.Current.Request.Form["dataAcompanhamento"].ToString());

            using (var dc = new manutEntities())
            {

                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                DateTime? dataTermino = Convert.ToDateTime(HttpContext.Current.Request.Form["dataTermino"].ToString());

                var qtde = (from p in dc.tb_ordemservico.Where(a => (a.dataEmissao >= dataTermino && a.dataEmissao <= dataTermino &&
                            a.autonumeroCliente == autonumeroCliente && a.cancelado != "S"))
                            select p).Count();

                return qtde.ToString("#######0");

            }
        }
    }
}
