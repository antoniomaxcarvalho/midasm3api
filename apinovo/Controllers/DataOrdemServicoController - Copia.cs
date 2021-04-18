using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataOrdemServicoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_ordemservico> GetAllOrdemServicoCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").OrderByDescending(p => p.codigoOs) select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_ordemservico> GetAllOrdemServico()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_ordemservico.Where(a => a.cancelado != "S") orderby p.codigoOs select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirOrdemServico()
        {
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
            var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
            var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
            var valor = Convert.ToDecimal(HttpContext.Current.Request.Form["valor"].ToString().Trim());
            var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
            var autonumeroSistema = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroSistema"].ToString().Trim());

            var processo = HttpContext.Current.Request.Form["processo"].ToString().Trim();
            var cap = HttpContext.Current.Request.Form["cap"].ToString().Trim();
            var enderecoCliente = HttpContext.Current.Request.Form["enderecoCliente"].ToString().Trim();
            var solicitante = HttpContext.Current.Request.Form["solicitante"].ToString().Trim();
            var nomeItem = HttpContext.Current.Request.Form["nomeItem"].ToString().Trim();
            var tipoAtendimento = HttpContext.Current.Request.Form["tipoAtendimento"].ToString().Trim();
            var autonumeroServico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroServico"].ToString());

            var siglaItem = HttpContext.Current.Request.Form["siglaItem"].ToString().Trim();
            var resumoServico = HttpContext.Current.Request.Form["resumoServico"].ToString().Trim();

            var quantidadeSS = Convert.ToInt32(HttpContext.Current.Request.Form["quantidadeSS"].ToString());
            var situacao = HttpContext.Current.Request.Form["situacao"].ToString().Trim();
            var local = HttpContext.Current.Request.Form["local"].ToString().Trim();
            //var etapa = HttpContext.Current.Request.Form["etapa"].ToString().Trim();
            //var medicao = HttpContext.Current.Request.Form["medicao"].ToString().Trim();


            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()) &&
                DataClienteController.IsDate(HttpContext.Current.Request.Form["dataFim"].ToString()) &&
                DataClienteController.IsDate(HttpContext.Current.Request.Form["dataEmissao"].ToString()))
            {

                var data11 = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                var data22 = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());

                using (var dc = new manutEntities())
                {
                    using (var transaction = dc.Database.BeginTransaction())
                    {
                        try
                        {
                            var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                            if (linha != null)
                            {
                                linha.contadorOrdemServico = linha.contadorOrdemServico + 1;

                                dc.tb_cliente.AddOrUpdate(linha);
                                dc.SaveChanges();

                                var contadorOsCliente = Convert.ToInt32(linha.contadorOrdemServico);
                                var codigo = cap + "-" + contadorOsCliente.ToString("000") + "/" + DateTime.Now.ToString("yy") + "-" + tipoAtendimento;

                                var k = new tb_ordemservico
                                {
                                    autonumeroCliente = autonumeroCliente,
                                    nomeCliente = nomeCliente,
                                    sigla = sigla,
                                    nomeUsuario = nomeUsuario,
                                    dataEmissao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataEmissao"].ToString()),
                                    dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString()),
                                    dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString()),
                                    codigoOs = codigo,
                                    cancelado = "N",
                                    valor = valor,
                                    nomeSistema = nomeSistema,
                                    autonumeroSistema = autonumeroSistema,
                                    processo = processo,
                                    cap = cap,
                                    enderecoCliente = enderecoCliente,
                                    solicitante = solicitante,
                                    nomeItem = nomeItem,
                                    siglaItem = siglaItem,
                                    resumoServico = resumoServico,
                                    quantidadeSS = quantidadeSS,
                                    situacao = situacao,
                                    etapa = "",
                                    medicao = "",
                                    local = local

                                };

                                dc.tb_ordemservico.Add(k);
                                dc.SaveChanges();
                                var auto = Convert.ToInt64(k.autonumero);

                                (from i in dc.tb_os_itens
                                 join p in dc.tb_os
                                 on i.codigoOs equals p.codigoOs
                                 where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && p.cancelado != "S" &&
                                       i.autonumeroCliente == p.autonumeroCliente &&
                                        p.nomeStatus == "Fechada" && p.autonumeroSistema == autonumeroSistema && p.autonumeroServico == autonumeroServico
                                          && p.dataTermino >= data11 && p.dataTermino <= data22
                                 select new
                                 {
                                     i,
                                     p
                                 }).ToList().ForEach(x =>
                                 {
                                     x.i.codigoOrdemServico = codigo;
                                     x.p.codigoOrdemServico = codigo;
                                 });

                                dc.SaveChanges();

                                transaction.Commit();
                                return auto.ToString();
                            }

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                        return "";
                    }
                }


            }
            else
            {
                throw new ArgumentException("Execption");
            }

        }
        [HttpDelete]
        public string CancelarOrdemServico()
        {

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var codigoOs = string.Empty;
                        var encontrouTb_ordemservico = false;
                        var encontrouTb_os_itens = false;
                        var encontrouTb_os = false;

                        var linha = dc.tb_ordemservico.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null)
                        {

                            if (!string.IsNullOrEmpty(linha.etapa))
                            {
                                throw new ArgumentException("Erro: EXISTE Medição para esta ordem de serviço");
                            }

                            if (linha.cancelado != "S")
                            {
                                encontrouTb_ordemservico = true;
                                codigoOs = linha.codigoOs;

                                linha.cancelado = "S";
                                dc.tb_ordemservico.AddOrUpdate(linha);
                                dc.SaveChanges();
                            }
                        }
                        if (!string.IsNullOrEmpty(codigoOs))
                        {
                            dc.tb_os_itens.Where(x => x.codigoOrdemServico == codigoOs).ToList().ForEach(x =>
                            {
                                encontrouTb_os_itens = true;
                                x.codigoOrdemServico = "";
                                x.etapa = "";
                                x.medicao = "";
                            });
                            dc.SaveChanges();
                            dc.tb_os.Where(x => x.codigoOrdemServico == codigoOs).ToList().ForEach(x =>
                            {
                                encontrouTb_os = true;
                                x.codigoOrdemServico = "";
                                x.etapa = "";
                                x.medicao = "";
                            });
                            dc.SaveChanges();


                        }
                        if (encontrouTb_ordemservico && encontrouTb_os_itens && encontrouTb_os)
                        {
                            transaction.Commit();
                            return string.Empty;
                        }
                        else
                        {
                            throw new ArgumentException("Erro: Não foi possível cancelar a ordem de serviço");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ex.Message.ToString();
                    }

                }
            }
        }

        [HttpPost]
        public IEnumerable<tb_ordemservico> GetAllOrdemServicoEmissao()
        {
            var dataInicioMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicioMedicao"]);
            var dataFimMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFimMedicao"]);
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_ordemservico.Where(a => a.cancelado != "S" && a.dataEmissao >= dataInicioMedicao &&
                           a.dataEmissao <= dataFimMedicao && a.autonumeroCliente == autonumeroCliente)
                           orderby p.codigoOs
                           select p;
                return user.ToList(); ;
            }
        }

        public IEnumerable<tb_ordemservico> GetOsNoIntervalo(Int64 autonumeroCliente, string dataInicio, string dataFim, Int32 autonumeroSistema)
        {
            IQueryable<tb_ordemservico> itens;
            var di = Convert.ToDateTime(dataInicio);
            var df = Convert.ToDateTime(dataFim);

            using (var dc = new manutEntities())
            {
                // Se o intervalo está dentro de outro - Interseção ( isOverlapDates) ------------------------------
                itens = (from i in dc.tb_ordemservico

                         where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.cancelado != "S" &&
                                i.autonumeroSistema == autonumeroSistema
                                 && i.dataInicio <= df && di <= i.dataFim && i.dataInicio != di && df != i.dataFim
                         select i);

                return itens.ToList(); ;
            }
        }

        public IEnumerable<ssCodigo> GetAllSolicitacaoServicoDaOC(string codigoOrdemServico, Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {

                var resposta = (from i in dc.tb_os
                                where i.codigoOrdemServico == codigoOrdemServico && i.cancelado != "S" && i.autonumeroCliente == autonumeroCliente
                                select new
                                {
                                    i.codigoOs,
                                    i.siglaCliente
                                }).Distinct();

                var j = 1;
                var ss1 = string.Empty;
                var ss2 = string.Empty;
                var ss3 = string.Empty;
                var ss4 = string.Empty;

                var ss11 = string.Empty;
                var ss22 = string.Empty;
                var ss33 = string.Empty;
                var ss44 = string.Empty;

                List<ssCodigo> ssC = new List<ssCodigo>();

                resposta.ToList().ForEach(x =>
                {
                    var codigoOs = x.codigoOs;
                    codigoOs = x.codigoOs.Replace(x.siglaCliente + "-", "");
                    codigoOs = codigoOs.Substring(0, codigoOs.Length - 5);



                    if (j == 4)
                    {
                        ss4 = codigoOs;
                        ss44 = x.codigoOs;
                        j = 1;
                        ssC.Add(new ssCodigo(ss1, ss2, ss3, ss4, ss11, ss22, ss33, ss44));
                        goto _continue;


                    }
                    if (j == 3)
                    {
                        ss3 = codigoOs;
                        ss33 = x.codigoOs;
                        j++;
                        goto _continue;
                    }
                    if (j == 2)
                    {
                        ss2 = codigoOs;
                        ss22 = x.codigoOs;
                        j++;
                        goto _continue;
                    }
                    if (j == 1)
                    {
                        ss1 = string.Empty;
                        ss2 = string.Empty;
                        ss3 = string.Empty;
                        ss4 = string.Empty;
                        ss11 = string.Empty;
                        ss22 = string.Empty;
                        ss33 = string.Empty;
                        ss44 = string.Empty;

                        ss1 = codigoOs;
                        ss11 = x.codigoOs;

                        j++;
                    }

                _continue:;

                });

                ssC.Add(new ssCodigo(ss1, ss2, ss3, ss4, ss11, ss22, ss33, ss44));

                return ssC.ToList();
            }
        }
        public class ssCodigo
        {
            public ssCodigo(string ss1, string ss2, string ss3, string ss4, string ss11, string ss22, string ss33, string ss44)
            {
                this.ss1 = ss1;
                this.ss2 = ss2;
                this.ss3 = ss3;
                this.ss4 = ss4;

                this.ss11 = ss11;
                this.ss22 = ss22;
                this.ss33 = ss33;
                this.ss44 = ss44;
            }

            public string ss1 { get; set; }
            public string ss2 { get; set; }
            public string ss3 { get; set; }
            public string ss4 { get; set; }
            public string ss11 { get; set; }
            public string ss22 { get; set; }
            public string ss33 { get; set; }
            public string ss44 { get; set; }

        }

    }
}
