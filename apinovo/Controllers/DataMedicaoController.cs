using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataMedicaoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_medicao> GetMedicao(long autonumeroCliente)
        {
            var c = 1;
            //AcertarSSComOrdemDeServico(autonumeroCliente);

            //AcertarMedicao(autonumeroCliente);

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_medicao.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").OrderBy(p => p.dataInicioMedicao).ThenBy(p => p.dataInicioMedicao) select p;
                return user.ToList(); ;
            }


        }

        [HttpPost]
        public string AtualizarMedicao()
        {
            using (var dc = new manutEntities())
            {
                //try
                //{
                var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);

                var prazoInicialMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prazoInicialMeses"]);
                var prorrogacoesMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prorrogacoesMeses"]);

                var obra = HttpContext.Current.Request.Form["obra"].ToString().Trim();

                var endereco = HttpContext.Current.Request.Form["endereco"].ToString().Trim();
                var contrato = HttpContext.Current.Request.Form["contrato"].ToString().Trim();
                var processo = HttpContext.Current.Request.Form["processo"].ToString().Trim();
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var unidadeDaSMS = HttpContext.Current.Request.Form["unidadeDaSMS"].ToString();

                var valorGlobalPrevisto = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalPrevisto"].ToString());
                var valorGlobalMedido = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalMedido"].ToString());
                var reducao = Convert.ToDecimal(HttpContext.Current.Request.Form["reducao"].ToString());
                var valorMedicao = Convert.ToDecimal(HttpContext.Current.Request.Form["valorMedicao"].ToString());
                var variacaoContratual = Convert.ToDecimal(HttpContext.Current.Request.Form["variacaoContratual"].ToString());
                var aFaturar = Convert.ToDecimal(HttpContext.Current.Request.Form["aFaturar"].ToString());
                var prazoInicialDias = HttpContext.Current.Request.Form["prazoInicialDias"].ToString().Trim();
                var sequencia = Convert.ToInt32(HttpContext.Current.Request.Form["sequencia"]);

                DateTime? dataInicio = null;
                DateTime? dataFim = null;
                DateTime? dataInicioMedicao = null;
                DateTime? dataFimMedicao = null;

                if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataInicio"]))
                {
                    dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"]);
                }
                if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataFim"]))
                {
                    dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"]);
                }
                if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataInicioMedicao"]))
                {
                    dataInicioMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicioMedicao"]);
                }
                if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataFimMedicao"]))
                {
                    dataFimMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFimMedicao"]);
                }

                var tb_medicao = dc.tb_medicao.Where(x => x.medicao == medicao && x.etapa == etapa && x.cancelado != "S" && x.autonumeroCliente == autonumeroCliente).ToList();

                foreach (var item in tb_medicao)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    item.cancelado = "S";
                    //dc.tb_medicao.Remove(item);
                }
                dc.SaveChanges();

                var linha = new tb_medicao();
                linha.autonumeroCliente = autonumeroCliente;
                linha.prazoInicialMeses = prazoInicialMeses;
                linha.prorrogacoesMeses = prorrogacoesMeses;
                linha.obra = obra;
                linha.endereco = endereco;
                linha.contrato = contrato;
                linha.processo = processo;
                linha.medicao = medicao;
                linha.etapa = etapa;
                linha.reducao = reducao;
                //linha.valorGlobalPrevisto = valorGlobalPrevisto;
                //linha.valorGlobalMedido = valorGlobalMedido;
                //linha.valorMedicao = valorMedicao;
                //linha.variacaoContratualPercentual = variacaoContratualPercentual;
                //linha.aFaturar = aFaturar;
                linha.dataInicio = dataInicio;
                linha.dataFim = dataFim;
                linha.dataInicioMedicao = dataInicioMedicao;
                linha.dataFimMedicao = dataFimMedicao;
                linha.cancelado = "N";
                linha.unidadeDaSMS = unidadeDaSMS;
                linha.prazoInicialDias = prazoInicialDias;
                linha.sequencia = sequencia;

                dc.tb_medicao.Add(linha);
                dc.SaveChanges();
                return linha.autonumero.ToString("#########0");
                //}
                //catch (Exception)
                //{
                //    return "Erro";
                //}
            }
        }

        [HttpPost]
        public string AtualizarValoresMedicao()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                try
                {
                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                    //var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);
                    //var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                    //var etapa = HttpContext.Current.Request.Form["etapa"].ToString();

                    var valorGlobalPrevisto = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalPrevisto"].ToString());
                    var valorGlobalMedido = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalMedido"].ToString());
                    var reducao = Convert.ToDecimal(HttpContext.Current.Request.Form["reducao"].ToString());
                    var valorMedicao = Convert.ToDecimal(HttpContext.Current.Request.Form["valorMedicao"].ToString());
                    var variacaoContratual = Convert.ToDecimal(HttpContext.Current.Request.Form["variacaoContratual"].ToString());
                    var aFaturar = Convert.ToDecimal(HttpContext.Current.Request.Form["aFaturar"].ToString());


                    //var linha = dc.tb_medicao.Where(x => x.medicao == medicao && x.etapa == etapa && x.cancelado != "S");
                    var linha = dc.tb_medicao.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {

                        linha.valorGlobalPrevisto = valorGlobalPrevisto;
                        linha.valorGlobalMedido = valorGlobalMedido;
                        linha.valorMedicao = valorMedicao;
                        linha.variacaoContratual = variacaoContratual;
                        linha.aFaturar = aFaturar;

                        dc.tb_medicao.AddOrUpdate(linha);
                        dc.SaveChanges();
                        return "";
                    }
                    return "Erro";

                }

                catch (Exception)
                {
                    return "Erro";
                }


            }
        }

        //[HttpPost]
        //public string CalcularMedicaoX()
        //{
        //    var c = 1;

        //    var autonumeroMedicao = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroMedicao"].ToString());
        //    var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //    var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
        //    var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
        //    var bdiServico = Convert.ToDecimal(HttpContext.Current.Request.Form["bdiServico"].ToString());

        //    var sequenciaInformada = Convert.ToInt32(HttpContext.Current.Request.Form["sequencia"].ToString());


        //    DateTime? dataInicioMedicao = null;
        //    DateTime? dataFimMedicao = null;

        //    if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataInicioMedicao"]))
        //    {
        //        dataInicioMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicioMedicao"]);
        //    }
        //    if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataFimMedicao"]))
        //    {
        //        dataFimMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFimMedicao"]);
        //    }
        //    using (var dc = new manutEntities())
        //    {


        //        if (sequenciaInformada == 0)
        //        {
        //            var p = dc.tb_medicao.Where(x => x.autonumero == autonumeroMedicao && x.cancelado != "S").ToList();
        //            foreach (var item in p)
        //            {
        //                if (item == null)
        //                {
        //                    continue;
        //                }
        //                sequenciaInformada = (int)item.sequencia;
        //            }
        //        }

        //        using (var transaction = dc.Database.BeginTransaction())
        //        {
        //            //try
        //            //{

        //            dc.tb_medicaoitens.Where(x => x.autonumeroMedicao == autonumeroMedicao).ToList().ForEach(x =>
        //            {
        //                x.cancelado = "S";
        //            });


        //            //Calcular Medicao (Qtde e Valor) entre as Datas do intervalo -------------------------------------

        //            var itens = ((from y in (from x in (from i in dc.tb_os_itens
        //                                                join p in dc.tb_planilhafechada
        //                                                on i.codigoPF equals p.codigo
        //                                                join k in dc.tb_os
        //                                                on i.autonumeroOs equals k.autonumero
        //                                                where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&
        //                                                i.autonumeroCliente == p.autonumeroCliente &&
        //                                                i.quantidadePF > 0 && k.nomeStatus == "Fechada" && i.codigoOrdemServico != "" &&
        //                                                k.dataTermino >= dataInicioMedicao && k.dataTermino <= dataFimMedicao
        //                                                group i by p.codigo into g
        //                                                select new
        //                                                {
        //                                                    g.Key,
        //                                                    medicaoAtual = g.Sum(p => p.quantidadePF),
        //                                                    valorAposMedicao = g.Sum(j => Math.Round((decimal)j.quantidadePF * (decimal)j.precoUnitarioPF, 2)),
        //                                                    //valorAposMedicao = Math.Round((decimal)g.Sum(j => j.totalPF), 2),

        //                                                })
        //                                     join o in dc.tb_planilhafechada
        //                                     on x.Key equals o.codigo
        //                                     where o.autonumeroCliente == autonumeroCliente
        //                                     select new
        //                                     {
        //                                         o.codigo,
        //                                         autonumeroPlanilhaFechada = o.autonumero,
        //                                         o.item,
        //                                         o.nome,
        //                                         o.qtdeContratada,
        //                                         x.medicaoAtual,
        //                                         x.valorAposMedicao,
        //                                     })
        //                          join f in dc.tb_planilhafechada
        //                          on y.codigo equals f.codigo
        //                          where f.autonumeroCliente == autonumeroCliente
        //                          select new medicaoitens
        //                          {
        //                              autonumeroMedicao = autonumeroMedicao,
        //                              medicao = medicao,
        //                              etapa = etapa,
        //                              fonte = f.fonte,
        //                              autonumeroPlanilhaFechada = y.autonumeroPlanilhaFechada,
        //                              item = y.item,
        //                              codigo = y.codigo,
        //                              nome = y.nome,
        //                              unidade = f.unidade,
        //                              preco = f.preco,
        //                              qtdeContratada = y.qtdeContratada,
        //                              medicaoAtual = y.medicaoAtual,
        //                              qtdeTotalAcumulada = 0m,
        //                              estoque = 0m,
        //                              valorAposMedicao = y.valorAposMedicao
        //                          })).Distinct().ToList();

        //            //FIM - Calcular Medicao (Qtde e Valor) entre as Datas do intervalo -------------------------------------

        //            // Debug.WriteLine("itens");


        //            //Calcular Estoque até data final -------------------------------------
        //            var itens2 = (from x in (from i in dc.tb_os_itens
        //                                     join p in dc.tb_planilhafechada
        //                                     on i.codigoPF equals p.codigo
        //                                     join k in dc.tb_os
        //                                     on i.autonumeroOs equals k.autonumero
        //                                     where i.autonumeroCliente == autonumeroCliente && k.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && k.dataTermino <= dataFimMedicao &&
        //                                           i.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0
        //                                     group i by p.codigo into g
        //                                     select new
        //                                     {
        //                                         g.Key,
        //                                         qtdeTotalAcumulada = g.Sum(p => p.quantidadePF),
        //                                     })
        //                          join o in dc.tb_planilhafechada
        //                          on x.Key equals o.codigo
        //                          where o.autonumeroCliente == autonumeroCliente
        //                          select new
        //                          {
        //                              codigo = x.Key,
        //                              x.qtdeTotalAcumulada,
        //                              estoque = o.qtdeContratada - x.qtdeTotalAcumulada
        //                          }).ToList();

        //            //FIM - Calcular Estoque até data final -------------------------------------

        //            // Debug.WriteLine("itens2");

        //            itens = (from i in itens
        //                     join i2 in itens2 on i.codigo equals i2.codigo
        //                     select new medicaoitens
        //                     {
        //                         autonumeroMedicao = autonumeroMedicao,
        //                         medicao = medicao,
        //                         etapa = etapa,
        //                         fonte = i.fonte,
        //                         autonumeroPlanilhaFechada = i.autonumeroPlanilhaFechada,
        //                         item = i.item,
        //                         codigo = i.codigo,
        //                         nome = i.nome,
        //                         unidade = i.unidade,
        //                         preco = i.preco,
        //                         qtdeContratada = i.qtdeContratada,
        //                         medicaoAtual = i.medicaoAtual,
        //                         qtdeTotalAcumulada = i2.qtdeTotalAcumulada,
        //                         estoque = i2.estoque,
        //                         valorAposMedicao = i.valorAposMedicao,

        //                     }).ToList();


        //            var valorMedicaoAtual = 0m;
        //            foreach (var i in itens)
        //            {

        //                if (i == null)
        //                {
        //                    continue;
        //                }

        //                valorMedicaoAtual = valorMedicaoAtual + (decimal)i.valorAposMedicao;
        //                var k = new tb_medicaoitens
        //                {
        //                    autonumeroMedicao = autonumeroMedicao,
        //                    medicao = medicao,
        //                    etapa = etapa,
        //                    fonte = i.fonte,
        //                    autonumeroPlanilhaFechada = i.autonumeroPlanilhaFechada,
        //                    item = i.item,
        //                    codigo = i.codigo,
        //                    nome = i.nome,
        //                    unidade = i.unidade,
        //                    preco = i.preco,
        //                    qtdeContratada = i.qtdeContratada,
        //                    medicaoAtual = i.medicaoAtual,
        //                    qtdeTotalAcumulada = i.qtdeTotalAcumulada,
        //                    estoque = i.estoque,
        //                    valorAposMedicao = i.valorAposMedicao,
        //                    cancelado = "N"

        //                };

        //                dc.tb_medicaoitens.Add(k);

        //            }

        //            var pf = dc.tb_planilhafechada.Where(a => a.autonumeroCliente == autonumeroCliente).ToList();
        //            foreach (var i in pf)
        //            {

        //                var found = itens.Find(item => item.codigo == i.codigo);

        //                if (found == null)
        //                {

        //                    var k = new tb_medicaoitens
        //                    {
        //                        autonumeroMedicao = autonumeroMedicao,
        //                        medicao = medicao,
        //                        etapa = etapa,
        //                        fonte = i.fonte,
        //                        autonumeroPlanilhaFechada = i.autonumero,
        //                        item = i.item,
        //                        codigo = i.codigo,
        //                        nome = i.nome,
        //                        unidade = i.unidade,
        //                        preco = i.preco,
        //                        qtdeContratada = i.qtdeContratada,
        //                        medicaoAtual = 0,
        //                        qtdeTotalAcumulada = 0,
        //                        estoque = i.estoque,
        //                        valorAposMedicao = 0,
        //                        cancelado = "N"
        //                    };

        //                    dc.tb_medicaoitens.Add(k);
        //                }

        //            }

        //            // Debug.WriteLine("SaveChanges");

        //            dc.SaveChanges();

        //            var valorGlobalPrevisto = 0m;
        //            var t = dc.tb_etapa.Where(x => x.autonumeroCliente == autonumeroCliente).OrderBy(a => a.sequencia);


        //            foreach (var item in t)
        //            {
        //                var sequenciaBD = item.sequencia;

        //                if (sequenciaBD <= sequenciaInformada)
        //                {
        //                    valorGlobalPrevisto = valorGlobalPrevisto + (decimal)item.valorGlobal;
        //                    //if (item.etapa == etapa)
        //                    //{
        //                    //    break;
        //                    //}
        //                }
        //            }

        //            var valorGlobalMedido = 0m;
        //            var d = dc.tb_medicao.Where(x => x.autonumeroCliente == autonumeroCliente && x.cancelado != "S").OrderBy(a => a.autonumero);

        //            foreach (var item in d)
        //            {
        //                //if (item.medicao == medicao)
        //                //{
        //                //    break;
        //                //}
        //                var sequenciaBD = item.sequencia;

        //                if (sequenciaBD < sequenciaInformada)
        //                {
        //                    valorGlobalMedido = valorGlobalMedido + (decimal)item.aFaturar;
        //                }

        //            }



        //            var linha = dc.tb_medicao.Find(autonumeroMedicao); // sempre irá procurar pela chave primaria
        //            if (linha != null && linha.cancelado != "S")
        //            {

        //                var valorTotalBdiServico = valorMedicaoAtual * (bdiServico / 100);
        //                valorMedicaoAtual = valorMedicaoAtual + valorTotalBdiServico;
        //                var vContratual = ((decimal)linha.reducao / 100) * valorMedicaoAtual;

        //                var aFaturar = (valorMedicaoAtual - vContratual);

        //                linha.valorMedicao = valorMedicaoAtual;
        //                linha.valorGlobalPrevisto = valorGlobalPrevisto;
        //                linha.valorGlobalMedido = valorGlobalMedido + aFaturar;
        //                linha.variacaoContratual = vContratual;
        //                linha.aFaturar = aFaturar;
        //                linha.valorTotalBdiServico = valorTotalBdiServico;


        //                dc.tb_medicao.AddOrUpdate(linha);
        //                dc.SaveChanges();

        //            }

        //            // Atualizar Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

        //            var itens3 = (from i in dc.tb_os_itens
        //                          join k in dc.tb_os
        //                          on i.autonumeroOs equals k.autonumero
        //                          where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&

        //                          i.quantidadePF > 0 && k.nomeStatus == "Fechada" && i.codigoOrdemServico != "" &&
        //                          k.dataTermino >= dataInicioMedicao && k.dataTermino <= dataFimMedicao

        //                          select new
        //                          {
        //                              i,
        //                              k

        //                          });

        //            itens3.ToList().ForEach(x =>
        //            {
        //                x.i.etapa = etapa;
        //                x.i.medicao = medicao;
        //                x.k.etapa = etapa;
        //                x.k.medicao = medicao;
        //            });

        //            dc.SaveChanges();

        //            // FIM - Atualizar Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

        //            // Atualizar Tb_ordemservico  -----------------------------------------------------------------------------------------


        //            var ordem = from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" &&
        //                       a.dataEmissao >= dataInicioMedicao && a.dataEmissao <= dataFimMedicao)
        //                        select p;

        //            //var ordem = from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" &&
        //            //          a.dataEmissao >= dataInicioMedicao)
        //            //            select p;

        //            //var ordem2 = from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S")
        //            //             select p;

        //            //// Debug.WriteLine("aaaaaaaaaaaaaa");
        //            //// Debug.WriteLine(dataInicioMedicao);
        //            // Debug.WriteLine(ordem.Count());
        //            ordem.ToList().ForEach(x =>
        //            {
        //                x.etapa = etapa;
        //                x.medicao = medicao;
        //            });
        //            //ordem2.ToList().ForEach(f =>
        //            //{
        //            //    // Debug.WriteLine(f.dataEmissao); ;
        //            //});
        //            dc.SaveChanges();
        //            // FIM Atualizar Tb_ordemservico  ------------------------------------------------------------------------------------

        //            transaction.Commit();
        //            return String.Empty;

        //            //}
        //            //catch (DbEntityValidationException e)
        //            //{
        //            //    transaction.Rollback();
        //            //    foreach (var eve in e.EntityValidationErrors)
        //            //    {
        //            //        // Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            //        foreach (var ve in eve.ValidationErrors)
        //            //        {
        //            //            // Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //            //                ve.PropertyName, ve.ErrorMessage);
        //            //        }
        //            //    }
        //            //    throw;
        //            //}

        //            //catch (Exception ex)
        //            //{
        //            //    transaction.Rollback();


        //            //    var c = string.Empty;
        //            //    if (ex.InnerException != null)
        //            //    {
        //            //        c = ex.InnerException.ToString().Substring(0, 130);
        //            //    }
        //            //    // Debug.WriteLine(ex.Message + " ---- " + c);
        //            //    return "Erro";
        //            //}

        //        }
        //    }
        //}

        [HttpPost]
        public string CalcularMedicao()
        {
            var c = 1;

            var autonumeroMedicao = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroMedicao"].ToString());
            var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
            var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
            var bdiServico = Convert.ToDecimal(HttpContext.Current.Request.Form["bdiServico"].ToString());

            var sequenciaInformada = Convert.ToInt32(HttpContext.Current.Request.Form["sequencia"].ToString());


            DateTime? dataInicioMedicao = null;
            DateTime? dataFimMedicao = null;

            if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataInicioMedicao"]))
            {
                dataInicioMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicioMedicao"]);
            }
            if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataFimMedicao"]))
            {
                dataFimMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFimMedicao"]);
            }

            using (var dc = new manutEntities())
            {
                dc.tb_medicaoitens.Where(x => x.medicao == medicao && x.etapa == etapa).ToList().ForEach(x =>
                {
                    x.cancelado = "S";
                });
                dc.SaveChanges();
            }

            using (var dc = new manutEntities())
            {

                if (sequenciaInformada == 0)
                {
                    var p = dc.tb_medicao.Where(x => x.autonumero == autonumeroMedicao && x.cancelado != "S").ToList();
                    foreach (var item in p)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        sequenciaInformada = (int)item.sequencia;
                    }
                }

                // Atualizar Tb_ordemservico  -----------------------------------------------------------------------------------------


                var ordem = (from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" &&
                           a.dataEmissao >= dataInicioMedicao && a.dataEmissao <= dataFimMedicao)
                            select p).ToList();

                // Debug.WriteLine(ordem.Count());
                ordem.ForEach(x =>
                {
                    x.etapa = etapa;
                    x.medicao = medicao;
                });

                dc.SaveChanges();
                // FIM Atualizar Tb_ordemservico  ------------------------------------------------------------------------------------


                // Debug.WriteLine(1111111);
                //dc.tb_ordemservico.Where(x => x.medicao == medicao && x.etapa == etapa && x.cancelado != "S" && x.autonumeroCliente == autonumeroCliente).ToList().ForEach(x =>

                //try
                //{

                    ordem.ForEach(x =>
                    {

                        Debug.WriteLine(x.codigoOs);
                    //var totalPF = dc.tb_os_itens.Where(k => k.codigoOrdemServico == x.codigoOs && k.autonumeroCliente == autonumeroCliente && k.cancelado != "S").Sum(k => (k.totalPF));


                    var totalPF = dc.tb_os_itens.Where(k => k.codigoOrdemServico == x.codigoOs && k.autonumeroCliente == autonumeroCliente && k.cancelado != "S")
                        .Sum(j => (Math.Truncate((decimal)j.quantidadePF * (decimal)j.precoUnitarioPF * 100)) / 100);

                        Debug.WriteLine(totalPF);
                        x.valor = totalPF;
                        dc.tb_ordemservico.AddOrUpdate(x);
                        dc.SaveChanges();
                    });
                //}

                //catch (Exception ex)
                //{



                //    var c11 = string.Empty;
                //    if (ex.InnerException != null)
                //    {
                //        c11 = ex.InnerException.ToString().Substring(0, 130);
                //    }
                //    // Debug.WriteLine(ex.Message + " ---- " + c);
                //    return "Erro";
                //}


                //Debug.WriteLine("00000000000000000000000000000000000000000");

                var c1 = 2;
                using (var transaction = dc.Database.BeginTransaction())
                {

                    //Calcular Medicao (Qtde e Valor) entre as Datas do intervalo -------------------------------------

                    var itens = ((from y in (from x in (from i in dc.tb_os_itens
                                                        join p in dc.tb_planilhafechada
                                                        on i.codigoPF equals p.codigo
                                                        join k in dc.tb_ordemservico
                                                        on i.codigoOrdemServico equals k.codigoOs
                                                        where i.autonumeroCliente == autonumeroCliente && k.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&
                                                        i.autonumeroCliente == p.autonumeroCliente &&
                                                        i.quantidadePF > 0 && i.codigoOrdemServico != "" &&
                                                        k.dataEmissao >= dataInicioMedicao && k.dataEmissao <= dataFimMedicao && k.cancelado != "S"
                                                        group i by p.codigo into g
                                                        select new
                                                        {
                                                            g.Key,
                                                            medicaoAtual = g.Sum(p => p.quantidadePF),
                                                            //valorAposMedicao = g.Sum(j => Math.Truncate((decimal)j.quantidadePF * (decimal)j.precoUnitarioPF)),
                                                            valorAposMedicao = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                                            //valorAposMedicao = Math.Round((decimal)g.Sum(j => j.totalPF), 2),
                                                            //valorAposMedicao = (decimal)g.Sum(j => j.totalPF),


                                                        })
                                             join o in dc.tb_planilhafechada
                                             on x.Key equals o.codigo
                                             where o.autonumeroCliente == autonumeroCliente
                                             select new
                                             {
                                                 o.codigo,
                                                 autonumeroPlanilhaFechada = o.autonumero,
                                                 o.item,
                                                 o.nome,
                                                 o.qtdeContratada,
                                                 x.medicaoAtual,
                                                 x.valorAposMedicao,
                                               
                                             })
                                  join f in dc.tb_planilhafechada
                                  on y.codigo equals f.codigo
                                  where f.autonumeroCliente == autonumeroCliente
                                  select new medicaoitens
                                  {
                                      autonumeroMedicao = autonumeroMedicao,
                                      medicao = medicao,
                                      etapa = etapa,
                                      fonte = f.fonte,
                                      autonumeroPlanilhaFechada = y.autonumeroPlanilhaFechada,
                                      item = y.item,
                                      codigo = y.codigo,
                                      nome = y.nome,
                                      unidade = f.unidade,
                                      preco = f.preco,
                                      qtdeContratada = y.qtdeContratada,
                                      medicaoAtual = y.medicaoAtual,
                                      qtdeTotalAcumulada = 0m,
                                      estoque = 0m,
                                      valorAposMedicao = y.valorAposMedicao
                                  })).Distinct().ToList();

                    //FIM - Calcular Medicao (Qtde e Valor) entre as Datas do intervalo -------------------------------------

                    // Debug.WriteLine("itens");


                    //Calcular Estoque até data final -------------------------------------
                    var itens2 = (from x in (from i in dc.tb_os_itens
                                             join p in dc.tb_planilhafechada
                                             on i.codigoPF equals p.codigo
                                             join k in dc.tb_ordemservico
                                             on i.codigoOrdemServico equals k.codigoOs
                                             where i.autonumeroCliente == autonumeroCliente && k.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&
                                                   k.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0 && i.codigoOrdemServico != "" &&
                                                    k.cancelado != "S" && k.dataEmissao <= dataFimMedicao
                                             group i by p.codigo into g
                                             select new
                                             {
                                                 g.Key,
                                                 qtdeTotalAcumulada = g.Sum(p => p.quantidadePF),
                                             })
                                  join o in dc.tb_planilhafechada
                                  on x.Key equals o.codigo
                                  where o.autonumeroCliente == autonumeroCliente
                                  select new
                                  {
                                      codigo = x.Key,
                                      x.qtdeTotalAcumulada,
                                      estoque = o.qtdeContratada - x.qtdeTotalAcumulada
                                  }).ToList();

                    //FIM - Calcular Estoque até data final -------------------------------------

                    // Debug.WriteLine("itens2");

                    itens = (from i in itens
                             join i2 in itens2 on i.codigo equals i2.codigo
                             select new medicaoitens
                             {
                                 autonumeroMedicao = autonumeroMedicao,
                                 medicao = medicao,
                                 etapa = etapa,
                                 fonte = i.fonte,
                                 autonumeroPlanilhaFechada = i.autonumeroPlanilhaFechada,
                                 item = i.item,
                                 codigo = i.codigo,
                                 nome = i.nome,
                                 unidade = i.unidade,
                                 preco = i.preco,
                                 qtdeContratada = i.qtdeContratada,
                                 medicaoAtual = i.medicaoAtual,
                                 qtdeTotalAcumulada = i2.qtdeTotalAcumulada,
                                 estoque = i2.estoque,
                                 valorAposMedicao = i.valorAposMedicao,

                             }).ToList();


                    //var valorMedicaoAtual = 0m;
                    foreach (var i in itens)
                    {

                        if (i == null)
                        {
                            continue;
                        }

                        //valorMedicaoAtual = valorMedicaoAtual + (decimal)i.valorAposMedicao;
                        var k = new tb_medicaoitens
                        {
                            autonumeroMedicao = autonumeroMedicao,
                            medicao = medicao,
                            etapa = etapa,
                            fonte = i.fonte,
                            autonumeroPlanilhaFechada = i.autonumeroPlanilhaFechada,
                            item = i.item,
                            codigo = i.codigo,
                            nome = i.nome,
                            unidade = i.unidade,
                            preco = i.preco,
                            qtdeContratada = i.qtdeContratada,
                            medicaoAtual = i.medicaoAtual,
                            qtdeTotalAcumulada = i.qtdeTotalAcumulada,
                            estoque = i.estoque,
                            valorAposMedicao = i.valorAposMedicao,
                            cancelado = "N"

                        };

                        dc.tb_medicaoitens.Add(k);

                    }


                    var valorMedicaoAtual =  (decimal) dc.tb_ordemservico.Where(k => k.medicao == medicao && k.etapa == etapa && k.autonumeroCliente == autonumeroCliente && k.cancelado != "S").Sum(k => (k.valor));



                    var pf = dc.tb_planilhafechada.Where(a => a.autonumeroCliente == autonumeroCliente).ToList();
                    foreach (var i in pf)
                    {

                        var found = itens.Find(item => item.codigo == i.codigo);

                        if (found == null)
                        {

                            var k = new tb_medicaoitens
                            {
                                autonumeroMedicao = autonumeroMedicao,
                                medicao = medicao,
                                etapa = etapa,
                                fonte = i.fonte,
                                autonumeroPlanilhaFechada = i.autonumero,
                                item = i.item,
                                codigo = i.codigo,
                                nome = i.nome,
                                unidade = i.unidade,
                                preco = i.preco,
                                qtdeContratada = i.qtdeContratada,
                                medicaoAtual = 0,
                                qtdeTotalAcumulada = 0,
                                estoque = i.estoque,
                                valorAposMedicao = 0,
                                cancelado = "N"
                            };

                            dc.tb_medicaoitens.Add(k);
                        }

                    }

                    // Debug.WriteLine("SaveChanges");

                    dc.SaveChanges();

                    var valorGlobalPrevisto = 0m;
                    var t = dc.tb_etapa.Where(x => x.autonumeroCliente == autonumeroCliente).OrderBy(a => a.sequencia);


                    foreach (var item in t)
                    {
                        var sequenciaBD = item.sequencia;

                        if (sequenciaBD <= sequenciaInformada)
                        {
                            valorGlobalPrevisto = valorGlobalPrevisto + (decimal)item.valorGlobal;
                            //if (item.etapa == etapa)
                            //{
                            //    break;
                            //}
                        }
                    }

                    var valorGlobalMedido = 0m;
                    var d = dc.tb_medicao.Where(x => x.autonumeroCliente == autonumeroCliente && x.cancelado != "S").OrderBy(a => a.autonumero);

                    foreach (var item in d)
                    {
                        //if (item.medicao == medicao)
                        //{
                        //    break;
                        //}
                        var sequenciaBD = item.sequencia;

                        if (sequenciaBD < sequenciaInformada)
                        {
                            valorGlobalMedido = valorGlobalMedido + (decimal)item.aFaturar;
                        }

                    }


                    var linha = dc.tb_medicao.Find(autonumeroMedicao); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        //var c2 = 1;

                        var valorTotalBdiServico = Math.Truncate(valorMedicaoAtual * (bdiServico / 100) * 100) / 100;
               

                        //Debug.WriteLine("valorMedicaoAtual = " + valorMedicaoAtual.ToString());
                        //Debug.WriteLine("linha.reducao = " + linha.reducao.ToString());

           
                        valorMedicaoAtual = valorMedicaoAtual + valorTotalBdiServico;
                        var vContratual = Math.Truncate((((decimal)linha.reducao / 100) * valorMedicaoAtual) * 100) / 100;
                        //Debug.WriteLine("linha.vContratual = " + vContratual.ToString());


                        //var c3 = 1;

                        //var valorTotalBdiServico = valorMedicaoAtual * (bdiServico / 100);
                        //valorMedicaoAtual = valorMedicaoAtual + valorTotalBdiServico;
                        //var vContratual = ((decimal)linha.reducao / 100) * valorMedicaoAtual;

                        var aFaturar = (valorMedicaoAtual - vContratual);


                        //Debug.WriteLine("linha.valorMedicao = " + valorMedicaoAtual.ToString());
                        //Debug.WriteLine("linha.valorGlobalPrevisto = " + valorGlobalPrevisto.ToString());
                        //Debug.WriteLine("linha.valorGlobalMedido = " + (valorGlobalMedido + aFaturar).ToString());
                        //Debug.WriteLine("linha.vContratual = " + vContratual.ToString()); Debug.WriteLine("linha.valorMedicao = " + valorMedicaoAtual.ToString());
                        //Debug.WriteLine("linha.aFaturar = " + aFaturar.ToString());
                        //Debug.WriteLine("linha.valorTotalBdiServico = " + valorTotalBdiServico.ToString());

                        linha.valorMedicao = valorMedicaoAtual;
                        linha.valorGlobalPrevisto = valorGlobalPrevisto;
                        linha.valorGlobalMedido = valorGlobalMedido + aFaturar;
                        linha.variacaoContratual = vContratual;
                        linha.aFaturar = aFaturar;
                        linha.valorTotalBdiServico = valorTotalBdiServico;


                        dc.tb_medicao.AddOrUpdate(linha);
                        dc.SaveChanges();

                    }

                    // Atualizar Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

                    var itens3 = (from i in dc.tb_os_itens
                                  join k in dc.tb_os
                                  on i.autonumeroOs equals k.autonumero
                                  where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&

                                  i.quantidadePF > 0 && k.nomeStatus == "Fechada" && i.codigoOrdemServico != "" &&
                                  k.dataTermino >= dataInicioMedicao && k.dataTermino <= dataFimMedicao

                                  select new
                                  {
                                      i,
                                      k

                                  }).ToList();

                    itens3.ForEach(x =>
                    {
                        x.i.etapa = etapa;
                        x.i.medicao = medicao;
                        x.k.etapa = etapa;
                        x.k.medicao = medicao;
                    });

                    dc.SaveChanges();

                    // FIM - Atualizar Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

                    // Atualizar Tb_ordemservico  -----------------------------------------------------------------------------------------


                    //var ordemX = from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" &&
                    //           a.dataEmissao >= dataInicioMedicao && a.dataEmissao <= dataFimMedicao)
                    //            select p;

                    //// Debug.WriteLine(ordemX.Count());
                    //ordem.ToList().ForEach(x =>
                    //{
                    //    x.etapa = etapa;
                    //    x.medicao = medicao;
                    //});

                    //dc.SaveChanges();
                    //// FIM Atualizar Tb_ordemservico  ------------------------------------------------------------------------------------

                    transaction.Commit();
                    return String.Empty;

                    //}
                    //catch (DbEntityValidationException e)
                    //{
                    //    transaction.Rollback();
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        // Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //        foreach (var ve in eve.ValidationErrors)
                    //        {
                    //            // Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //                ve.PropertyName, ve.ErrorMessage);
                    //        }
                    //    }
                    //    throw;
                    //}

                    //catch (Exception ex)
                    //{
                    //    transaction.Rollback();


                    //    var c = string.Empty;
                    //    if (ex.InnerException != null)
                    //    {
                    //        c = ex.InnerException.ToString().Substring(0, 130);
                    //    }
                    //    // Debug.WriteLine(ex.Message + " ---- " + c);
                    //    return "Erro";
                    //}

                }
            }
        }


        [HttpDelete]
        public string CancelarMedicao()
        {
            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            // Debug.WriteLine("aaaaaaaaaa");
            // Debug.WriteLine(autonumero);

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var medicao = string.Empty;
                        var etapa = string.Empty;

                        var linha = dc.tb_medicao.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")

                        {
                            medicao = linha.medicao;
                            etapa = linha.etapa;
                            var autonumeroCliente = Convert.ToInt64(linha.autonumeroCliente);

                            linha.cancelado = "S";
                            dc.tb_medicao.AddOrUpdate(linha);
                            dc.SaveChanges();

                            dc.tb_medicaoitens.Where(x => x.autonumeroMedicao == autonumero).ToList().ForEach(x =>
                            {
                                x.cancelado = "S";
                            });
                            dc.SaveChanges();

                            // Debug.WriteLine("medicao = linha.medicao;", medicao);
                            // Debug.WriteLine("etapa = linha.medicao;", etapa);

                            // LIMPAR Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

                            var itens3 = (from i in dc.tb_os_itens
                                          join k in dc.tb_os
                                          on i.autonumeroOs equals k.autonumero
                                          where i.medicao.ToLower() == medicao.ToLower() && i.etapa.ToLower() == etapa.ToLower() && i.cancelado != "S" && i.autonumeroCliente == autonumeroCliente
                                          select new
                                          {
                                              i,
                                              k
                                          }).ToList();
                            itens3.ForEach(x =>
                            {
                                x.i.etapa = "";
                                x.i.medicao = "";
                                x.k.etapa = "";
                                x.k.medicao = "";
                            });
                            dc.SaveChanges();

                            // FIM - LIMPAR Tb_os e Tb_os_itens com p nro da etapa e medicao -----------------------------------------------

                            // LIMPAR Tb_ordemservico  -----------------------------------------------------------------------------------------
                            var ordem = (from p in dc.tb_ordemservico.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" &&
                                        a.medicao == medicao && a.etapa == etapa)
                                        select p).ToList();
                            ordem.ForEach(x =>
                            {
                                x.etapa = "";
                                x.medicao = "";
                            });
                            dc.SaveChanges();
                            // FIM LIMPAR Tb_ordemservico  ------------------------------------------------------------------------------------
                        }

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
        public HttpResponseMessage ImprimirMedicao()

        {
            var message = String.Empty;

            try
            {
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var modeloImprimirComCodigo = HttpContext.Current.Request.Form["modeloImprimirComCodigo"].ToString();

                var filtro = "{tb_medicao1.cancelado} <> 'S'  and  {tb_medicaoitens1.cancelado} <> 'S'  and { tb_medicao1.autonumeroCliente} = " + autonumeroCliente + " and { tb_medicao1.medicao} = '" + medicao.Trim() + "'  and { tb_medicao1.etapa} =  '" + etapa.Trim() + "'";

                if (modeloImprimirComCodigo == "S")
                {
                    filtro = filtro + "  and {tb_medicaoitens1.medicaoAtual} > 0 ";
                }

                decimal? totalMedicao = 0m;
                using (var dc = new manutEntities())
                {
                    totalMedicao = dc.tb_ordemservico.Where(k => k.medicao == medicao && k.etapa == etapa && k.autonumeroCliente == autonumeroCliente && k.cancelado != "S").Sum(k => (k.valor));
                }

                if (totalMedicao == null)
                {
                    totalMedicao = 0;
                }

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    var caminho = HttpContext.Current.Server.MapPath("~/rpt/");

                    // saude -----------------
                    if (autonumeroCliente > 10 && autonumeroCliente < 14) // Saude ----------------------------
                    {
                        caminho = HttpContext.Current.Server.MapPath("~/rpt/saude/");
                    }

                    var local = caminho + "medicao.rpt";

                    if (modeloImprimirComCodigo == "S" || modeloImprimirComCodigo == "T")
                    {
                        if (autonumeroCliente > 10 && autonumeroCliente < 14) // Saude ----------------------------
                        {
                            local = caminho + "medicao.rpt";
                        }
                    }
                    else
                    {
                        local = caminho + "medicaoComNome.rpt";
                    }


                    rd.Load(local);
                    rd.SetParameterValue("@totalMedicao", totalMedicao);
                    //rd.SetParameterValue("p2", modelo);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;


                }

            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;





        }

        [HttpPost]
        public HttpResponseMessage ImprimirPlanilha3()

        {
            var message = String.Empty;

            try
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var unidadeDaSMS = HttpContext.Current.Request.Form["unidadeDaSMS"].ToString();

                var filtro = "{tb_ordemservico1.siglaItem} <> '' AND {tb_medicao1.cancelado} <> 'S' and {tb_ordemservico1.cancelado} <> 'S'  and { tb_ordemservico1.autonumeroCliente} = " + autonumeroCliente + " and { tb_ordemservico1.medicao} = '" + medicao.Trim() + "'  and { tb_ordemservico1.etapa} =  '" + etapa.Trim() + "'";



                //var totalPF = dc.tb_os_itens.Where(k => k.codigoOrdemServico == x.codigoOs && k.autonumeroCliente == autonumeroCliente && k.cancelado != "S")
                //.Sum(j => (Math.Truncate((decimal)j.quantidadePF * (decimal)j.precoUnitarioPF * 100)) / 100);

                //var ordem = (from p in dc.tb_ordemservico
                //             join k in dc.tb_medicao
                //             on p.medicao equals k.medicao
                //             where p.autonumeroCliente == autonumeroCliente && p.cancelado != "S" && p.siglaItem != "" && p.medicao == medicao && p.etapa == etapa && k.cancelado != ""

                //             select new
                //             {
                //                 p

                //             }).ToList();


                //var bdi = ordem.Sum(j => (Math.Truncate((decimal)j.p.valor * (decimal)j.p.bd * 100)) / 100);


                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    var local = HttpContext.Current.Server.MapPath("~/rpt/planilha3.rpt");

                    rd.Load(local);
                    rd.SetParameterValue("unidadeSMS", unidadeDaSMS);
                    //rd.SetParameterValue("p2", modelo);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;


                }

            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;





        }

        public IEnumerable<sistema> GetAllSistemaMedicao(Int64 autonumeroCliente, string etapa, string medicao)
        {
            using (var dc = new manutEntities())
            {

                var resposta = (from i in dc.tb_os
                                where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.etapa == etapa && i.medicao == medicao
                                select new sistema
                                {
                                    autonumero = (int)i.autonumeroSistema,
                                    nome = i.nomeSistema
                                }).Distinct().ToList();

                return resposta;
            }
        }

        [HttpPost]
        public HttpResponseMessage RelatorioFotografico()

        {
            var message = String.Empty;

            try
            {
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var unidadeDaSMS = HttpContext.Current.Request.Form["unidadeDaSMS"].ToString();

                var filtro = " ({ tb_os1.url} <> '' or { tb_os1.url1} <> '')  and {tb_os1.cancelado} <> 'S'  and { tb_os1.autonumeroCliente} = " + autonumeroCliente + " and { tb_os1.medicao} = '" + medicao.Trim() + "'  and { tb_os1.etapa} =  '" + etapa.Trim() + "'";

                using (var rd = new ReportDocument())
                {
                    var Response = HttpContext.Current.ApplicationInstance.Response;


                    var local = HttpContext.Current.Server.MapPath("~/rpt/relFoto.rpt");

                    rd.Load(local);
                    rd.SetParameterValue("unidadeSMS", unidadeDaSMS);
                    //rd.SetParameterValue("p2", modelo);

                    rd.RecordSelectionFormula = filtro;

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////75 is my print job limit.
                    //if (rd.Count > 75) ((ReportClass)reportQueue.Dequeue()).Dispose();
                    //return CreateReport(reportClass);

                    rd.Close();
                    rd.Dispose();

                    var resp = Request.CreateResponse(HttpStatusCode.OK);
                    resp.Content = new StreamContent(stream);
                    return resp;


                }

            }

            catch (LogOnException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "Incorrect Logon Parameters. Check your user name and password";
            }
            catch (DataSourceException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = "An error has occurred while connecting to the database.";
            }
            catch (EngineException ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            catch (Exception ex)
            {
                var c = string.Empty;
                if (ex.InnerException != null)
                {
                    c = ex.InnerException.ToString().Substring(0, 130);
                }
                message = message + ex.Message + " ---- " + c;
                //message = ex.InnerException  != null ? ex.InnerException.ToString().Substring(0, 130) : ex.Message;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            return null;





        }


        [HttpGet]
        public void AcertarSSComOrdemDeServico(long autonumeroCliente)
        {

            var csql = new StringBuilder();

            csql.Append("UPDATE manut.tb_ordemservico as os,tb_os as ss ");
            csql.Append("SET ss.etapa = os.etapa,ss.medicao = os.medicao ");
            csql.Append("WHERE os.cancelado != 'S' and ss.CANCELADO != 'S' and ss.nomeStatus = 'fechada' and ss.codigoOrdemServico != '' ");
            csql.Append("and ss.valor > 0 and(ss.medicao != os.medicao OR ss.etapa != os.etapa)  and os.codigoOs = ss.codigoOrdemServico and os.autonumeroCliente = " + autonumeroCliente.ToString());

            using (var dc = new manutEntities())
            {
                var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            }

            csql.Clear();

            csql.Append("UPDATE manut.tb_os_itens as i,tb_os as ss ");
            csql.Append("SET i.etapa = ss.etapa,i.medicao = ss.medicao ");
            csql.Append("WHERE i.cancelado != 'S' and ss.CANCELADO != 'S' and ss.nomeStatus = 'fechada' and ss.codigoOrdemServico != ''  ");
            csql.Append("and ss.valor > 0 and (ss.medicao != i.medicao OR ss.etapa != i.etapa) and i.codigoOs = ss.codigoOs and ss.autonumeroCliente = " + autonumeroCliente.ToString());

            using (var dc = new manutEntities())
            {
                var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            }

        }


        [HttpGet]
        public void AcertarMedicao(long autonumeroCliente)
        {
            var csql = new StringBuilder();

            csql.Append("DELETE FROM TB_MEDICAO where cancelado = 'S';  ");
            using (var dc = new manutEntities())
            {
                var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            }
            csql.Clear();

            using (var dc = new manutEntities())
            {
                var resposta = (from i in dc.tb_medicaoitens
                                join p in dc.tb_medicao
                                on i.autonumeroMedicao equals p.autonumero
                                where p.autonumeroCliente == autonumeroCliente
                                select new
                                {
                                    i.autonumeroMedicao
                                }).Distinct();

                foreach (var item in resposta)
                {
                    var linha = dc.tb_medicao.Find(item.autonumeroMedicao); // sempre irá procurar pela chave primaria
                    if (linha == null)
                    {
                        var gravou = false;
                        dc.tb_medicaoitens.Where(p => p.autonumeroMedicao == item.autonumeroMedicao && p.cancelado != "S").ToList().ForEach(x =>
                         {
                             gravou = true;
                             x.cancelado = "S";
                         });
                        if (gravou)
                        {
                            dc.SaveChanges();
                        }

                    }
                }
            }


            //csql.Append("update tb_medicaoItens  set cancelado = 'S'; ");
            //using (var dc = new manutEntities())
            //{
            //    var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            //}
            //csql.Clear();

            ////csql.Append("UPDATE tb_medicaoitens i INNER JOIN tb_medicao as m ON i.autonumeroMedicao = m.autonumero SET i.cancelado = 'N'  WHERE  m.cancelado = 'N'; ");
            ////using (var dc = new manutEntities())
            ////{
            ////    var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            ////}
            ////csql.Clear();


            //csql.Append("UPDATE tb_medicaoitens i INNER JOIN tb_medicao as m ON i.autonumeroMedicao = m.autonumero SET i.cancelado = 'S'  WHERE i.cancelado != m.cancelado and m.cancelado = 'S' ");
            //using (var dc = new manutEntities())
            //{
            //    var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            //}
            //csql.Clear();

        }



        [HttpPost]
        public string AlterarValorGlobal()
        {
            using (var dc = new manutEntities())
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var valorGlobalInclusiveEstaEtapa = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalInclusiveEstaEtapa"].ToString());

                var tb_medicao = dc.tb_medicao.Where(x => x.medicao == medicao && x.etapa == etapa && x.cancelado != "S" && x.autonumeroCliente == autonumeroCliente).ToList();

                foreach (var item in tb_medicao)
                {
                    item.valorGlobalPrevisto = valorGlobalInclusiveEstaEtapa;
                    dc.tb_medicao.AddOrUpdate(item);
                    dc.SaveChanges();
                }

                return "";
            }
        }

        [HttpPost]
        public string AlterarValorGlobalMedido()
        {
            using (var dc = new manutEntities())
            {

                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);
                var medicao = HttpContext.Current.Request.Form["medicao"].ToString();
                var etapa = HttpContext.Current.Request.Form["etapa"].ToString();
                var valorGlobalMedido = Convert.ToDecimal(HttpContext.Current.Request.Form["valorGlobalMedido"].ToString());

                var tb_medicao = dc.tb_medicao.Where(x => x.medicao == medicao && x.etapa == etapa && x.cancelado != "S" && x.autonumeroCliente == autonumeroCliente).ToList();

                foreach (var item in tb_medicao)
                {
                    item.valorGlobalMedido = valorGlobalMedido;
                    dc.tb_medicao.AddOrUpdate(item);
                    dc.SaveChanges();
                }

                return "";
            }
        }


        public class medicaoitens
        {
            public long autonumero { get; set; }
            public Nullable<long> autonumeroMedicao { get; set; }
            public string medicao { get; set; }
            public string etapa { get; set; }
            public string fonte { get; set; }
            public Nullable<long> autonumeroPlanilhaFechada { get; set; }
            public Nullable<long> item { get; set; }
            public string codigo { get; set; }
            public string nome { get; set; }
            public string unidade { get; set; }
            public Nullable<decimal> preco { get; set; }
            public Nullable<decimal> qtdeContratada { get; set; }
            public Nullable<decimal> medicaoAtual { get; set; }
            public Nullable<decimal> qtdeTotalAcumulada { get; set; }
            public Nullable<decimal> estoque { get; set; }
            public Nullable<decimal> valorAposMedicao { get; set; }
        }

        public class sistema
        {
            public long autonumero { get; set; }
            public string nome { get; set; }

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
