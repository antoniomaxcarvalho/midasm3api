using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace apinovo.Controllers
{
    public class DataOrcamentoItemController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_orcamento_itens> GetItensOrcamentoCodigo(string codigoOrcamento)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_orcamento_itens.Where((a => (a.codigoOrcamento == codigoOrcamento && a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
                return user.ToList();
            }

        }

        [HttpGet]
        public IEnumerable<tb_orcamento_itens> GetItensOrcamentoAutonumero(Int64 autonumeroOrcamento)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_orcamento_itens.Where((a => (a.autonumeroOrcamento == autonumeroOrcamento) && a.cancelado != "S")).OrderByDescending(p => p.autonumero) select p;
                return user.ToList(); ;
            }

        }


        public IEnumerable<tb_orcamento_itens> GetAllItensOrcamento()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_orcamento_itens.Where((a => (a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
                return user.ToList(); ;
            }

        }

        [HttpDelete]
        public string CancelarItensOrcamento()
        {
            var message = "Erro";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var linha = dc.tb_orcamento_itens.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {

                            linha.cancelado = "S";
                            dc.tb_orcamento_itens.AddOrUpdate(linha);
                            dc.SaveChanges();

                            decimal totalItens = 0m;

                            var p = dc.tb_orcamento_itens.Where(a => a.autonumeroOrcamento == linha.autonumeroOrcamento && a.cancelado != "S").ToList();

                            if (p != null)
                            {
                                foreach (var value in p)
                                {
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                            }

                            var k = dc.tb_orcamento.FirstOrDefault(a => a.autonumero == linha.autonumeroOrcamento && a.cancelado != "S");
                            if (k != null)
                            {

                                k.valor = totalItens;
                                dc.tb_orcamento.AddOrUpdate(k);
                                dc.SaveChanges();
                            }
                            else
                            {
                                throw new ArgumentException("Execption");
                            }

                            transaction.Commit();
                            return totalItens.ToString("########0.00");

                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return "Erro";
                    }
                }
            }

            return message;

        }

        [HttpPost]
        public string IncluirItensOrcamento()
        {

            var c = 1;

            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var autonumeroOrcamento = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOrcamento"].ToString());
                        var codigoOrcamento = HttpContext.Current.Request.Form["codigoOrcamento"].ToString();
                        var autonumeroInsumoServico = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroInsumoServico"].ToString());
                        var codigoInsumoServico = HttpContext.Current.Request.Form["codigoInsumoServico"].ToString();
                        var autonumeroPlanilhaFechada = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPlanilhaFechada"].ToString());
                        var codigoPlanilhaFechada = HttpContext.Current.Request.Form["codigoPlanilhaFechada"].ToString();
                        var nomePF = HttpContext.Current.Request.Form["nomePF"].ToString();

                        var unidade = HttpContext.Current.Request.Form["unidade"].ToString();
                        var nome = HttpContext.Current.Request.Form["nome"].ToString();
                        var nomeFonte = HttpContext.Current.Request.Form["nomeFonte"].ToString();
                        var nomeFontePF = HttpContext.Current.Request.Form["nomeFontePF"].ToString();
                        var quantidade = Convert.ToDecimal(HttpContext.Current.Request.Form["quantidade"].ToString());
                        var precoUnitario = Convert.ToDecimal(HttpContext.Current.Request.Form["precoUnitario"].ToString());
                        var total = Convert.ToDecimal(HttpContext.Current.Request.Form["total"].ToString());
                        var servico = HttpContext.Current.Request.Form["servico"].ToString();
                        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                        var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                        var memoriaCalculo = HttpContext.Current.Request.Form["memoriaCalculo"].ToString();

                        var quantidadePF = Convert.ToDecimal(HttpContext.Current.Request.Form["quantidadePF"].ToString());
                        var precoUnitarioPF = Convert.ToDecimal(HttpContext.Current.Request.Form["precoUnitarioPF"].ToString());
                        var totalPF = Convert.ToDecimal(HttpContext.Current.Request.Form["totalPF"].ToString());
                        var itemPF = Convert.ToInt32(HttpContext.Current.Request.Form["itemPF"].ToString());
                        var unidadePF = HttpContext.Current.Request.Form["unidadePF"].ToString();

                        var dataIncluir = DateTime.Now;
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (dataIncluir > dataLimite)
                                {
                                    dataIncluir = dataLimite;
                                }
                            }
                        }

                        var Os = new tb_orcamento_itens
                        {
                            autonumeroOrcamento = autonumeroOrcamento,
                            codigoOrcamento = codigoOrcamento,
                            autonumeroInsumoServico = autonumeroInsumoServico,
                            codigoInsumoServico = codigoInsumoServico,
                            autonumeroPlanilhaFechada = autonumeroPlanilhaFechada,
                            unidade = unidade,
                            nome = nome,
                            nomeFonte = nomeFonte,
                            nomeFontePF = nomeFontePF,
                            quantidade = quantidade,
                            precoUnitario = precoUnitario,
                            total = total,
                            servico = servico,
                            codigoPlanilhaFechada = codigoPlanilhaFechada,
                            nomePF = nomePF,
                            quantidadePF = quantidadePF,
                            precoUnitarioPF = precoUnitarioPF,
                            totalPF = totalPF,

                            cancelado = "N",
                            data = dataIncluir,
                            autonumeroCliente = autonumeroCliente,
                            nomeCliente = nomeCliente,

                            memoriaCalculo = memoriaCalculo,
                            itemPF = itemPF,
                            unidadePF = unidadePF


                        };

                        dc.tb_orcamento_itens.Add(Os);
                        dc.SaveChanges();

                        decimal totalItens = 0m;
                        var auto = Convert.ToInt32(Os.autonumero);

                        var p = dc.tb_orcamento_itens.Where(a => a.autonumeroOrcamento == autonumeroOrcamento && a.cancelado != "S").ToList();

                        if (p != null)
                        {
                            foreach (var value in p)
                            {
                                totalItens = Convert.ToDecimal(value.total) + totalItens;
                            }
                            var k = dc.tb_orcamento.FirstOrDefault(a => a.autonumero == autonumeroOrcamento && a.cancelado != "S");
                            if (k != null)
                            {
                                k.valor = totalItens;

                                dc.tb_orcamento.AddOrUpdate(k);
                                dc.SaveChanges();
                            }
                            else
                            {
                                throw new ArgumentException("Execption - Calcular valor itens");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Execption - tb_orcamento_itens = Null");
                        }

                        transaction.Commit();
                        return totalItens.ToString("########0.00");
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
        public string AssociarItensOrcamento()
        {
            using (var dc = new manutEntities())
            {
                try
                {

                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                    var nomePF = HttpContext.Current.Request.Form["nomePF"].ToString();
                    var quantidadePF = Convert.ToDecimal(HttpContext.Current.Request.Form["quantidadePF"].ToString());
                    var precoUnitarioPF = Convert.ToDecimal(HttpContext.Current.Request.Form["precoUnitarioPF"].ToString());
                    var totalPF = Convert.ToDecimal(HttpContext.Current.Request.Form["totalPF"].ToString());
                    var autonumeroPlanilhaFechada = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroPlanilhaFechada"].ToString());
                    var codigoPlanilhaFechada = HttpContext.Current.Request.Form["codigoPlanilhaFechada"].ToString();
                    var nomeFontePF = HttpContext.Current.Request.Form["nomeFontePF"].ToString();
                    var itemPF = Convert.ToInt32(HttpContext.Current.Request.Form["itemPF"].ToString());
                    var unidadePF = HttpContext.Current.Request.Form["unidadePF"].ToString();

                    var k = dc.tb_orcamento_itens.FirstOrDefault(a => a.autonumero == autonumero && a.cancelado != "S");
                    if (k != null)
                    {
                        k.autonumeroPlanilhaFechada = autonumeroPlanilhaFechada;
                        k.codigoPlanilhaFechada = codigoPlanilhaFechada;
                        k.nomePF = nomePF;
                        k.quantidadePF = quantidadePF;
                        k.precoUnitarioPF = precoUnitarioPF;
                        k.totalPF = totalPF;
                        k.nomeFontePF = nomeFontePF;
                        k.itemPF = itemPF;
                        k.unidadePF = unidadePF;

                        dc.tb_orcamento_itens.AddOrUpdate(k);
                        dc.SaveChanges();
                        return "";
                    }
                    else
                    {
                        throw new ArgumentException("Execption - Calcular valor itens");
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }

            }
        }

        [HttpPost]
        public string DesassociarItensOrcamento()
        {
            using (var dc = new manutEntities())
            {
                try
                {

                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

                    var k = dc.tb_orcamento_itens.FirstOrDefault(a => a.autonumero == autonumero && a.cancelado != "S");
                    if (k != null)
                    {
                        k.autonumeroPlanilhaFechada = 0;
                        k.codigoPlanilhaFechada = "";
                        k.nomePF = "";
                        k.quantidadePF = 0;
                        k.precoUnitarioPF = 0;
                        k.totalPF = 0;
                        k.nomeFontePF = "";
                        k.itemPF = 0;
                        k.unidadePF = "";

                        dc.tb_orcamento_itens.AddOrUpdate(k);
                        dc.SaveChanges();
                        return "";
                    }
                    else
                    {
                        throw new ArgumentException("Execption - Calcular valor itens");
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }

            }
        }


        [HttpPost]
        public string AlterarItemQtde()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
            var qtde = Convert.ToDecimal(HttpContext.Current.Request.Form["qtde"]);
            var memoriaCalculo = HttpContext.Current.Request.Form["memoriaCalculo"].ToString();

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var linha = dc.tb_orcamento_itens.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {

                            linha.quantidade = qtde;
                            linha.total = qtde * Convert.ToDecimal(linha.precoUnitario);
                            linha.memoriaCalculo = memoriaCalculo;
                            dc.tb_orcamento_itens.AddOrUpdate(linha);
                            dc.SaveChanges();

                            decimal totalItens = 0m;

                            var p = dc.tb_orcamento_itens.Where(a => a.autonumeroOrcamento == linha.autonumeroOrcamento && a.cancelado != "S").ToList();

                            if (p != null)
                            {
                                foreach (var value in p)
                                {
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                                var k = dc.tb_orcamento.FirstOrDefault(a => a.autonumero == linha.autonumeroOrcamento && a.cancelado != "S");
                                if (k != null)
                                {
                                    k.valor = totalItens;
                                    dc.tb_orcamento.AddOrUpdate(k);
                                    dc.SaveChanges();
                                }
                                else
                                {
                                    throw new ArgumentException("Execption - Calcular valor itens");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Execption - tb_orcamento_itens = Null");
                            }

                            transaction.Commit();
                            return totalItens.ToString("########0.00");

                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return "Erro";
                    }
                }
            }

            return message;

        }

        [HttpPost]
        public List<tb_orcamento_itens> ValidarOrcamentoEstoque()
        {
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {

                    var item = new List<tb_orcamento_itens>();

                    var autonumeroOrcamento = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOrcamento"].ToString());
                    var margem = Convert.ToDecimal(HttpContext.Current.Request.Form["margem"].ToString());

                    var p = dc.tb_orcamento_itens.Where(a => a.autonumeroOrcamento == autonumeroOrcamento && a.cancelado != "S").ToList();

                    if (p != null)
                    {
                        foreach (var valueItem in p)
                        {
                            var quantidade = Convert.ToDecimal(valueItem.quantidadePF);
                            var autonumeroCliente = Convert.ToInt64(valueItem.autonumeroCliente);
                            var autonumeroPlanilhaFechada = Convert.ToInt64(valueItem.autonumeroPlanilhaFechada);


                            if (valueItem.quantidadePF > 0)
                            {
                                // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.autonumero == autonumeroPlanilhaFechada);
                                if (planilhaFechada != null)
                                {
                                    var estoque = planilhaFechada.estoque - quantidade;
                                    if (estoque < 0)
                                    {
                                        valueItem.quantidade = planilhaFechada.estoque; // Passar o Estoque da Planilha para o SITE
                                        item.Add(valueItem);
                                    }
                                }
                                else
                                {
                                    item.Add(valueItem);
                                }
                            }

                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                        }

                        var valorPF = 0m;
                        var valorOrcamento = 0m;
                        foreach (var valueItem in p)
                        {
                            valorPF = Convert.ToDecimal(valueItem.totalPF) + valorPF;
                            valorOrcamento = Convert.ToDecimal(valueItem.total) + valorOrcamento;
                        }

                        var dif2 = valorPF - valorOrcamento;
                        var valorMargem = valorOrcamento * (margem / 100);

                        if (dif2 < 0)
                        {
                            dif2 = dif2 * -1;
                        }

                        if (valorMargem < dif2)
                        {
                            var i = new tb_orcamento_itens();

                            i.autonumero = 0;
                            i.autonumeroCliente = 0;
                            i.autonumeroInsumoServico = 0;
                            i.autonumeroOrcamento = 0;
                            i.autonumeroPlanilhaFechada = 0;
                            i.cancelado = "N";
                            i.codigoInsumoServico = "";
                            i.codigoOrcamento = null;
                            i.codigoPlanilhaFechada = "";
                            i.data = null;
                            i.memoriaCalculo = "";
                            i.nome = "Diferença Entre o Valor do Orçamento e a Planilha Maior Que a Margem Permitida ";
                            i.nomeCliente = "";
                            i.nomeFonte = "";
                            i.nomeFontePF = "";
                            i.nomePF = "Diferença Entre o Valor do Orçamento e a Planilha Maior Que a Margem Permitida ";
                            i.precoUnitario = 0;
                            i.precoUnitarioPF = 0;
                            i.quantidade = 0;
                            i.quantidadePF = 0;
                            i.servico = null;
                            i.total = 0;
                            i.totalPF = 0;
                            i.unidade = "";
                            i.itemPF = 0;
                            i.unidadePF = "";

                            item.Add(i);

                        }

                        var p1 = (from a in dc.tb_orcamento_itens
                                  where a.autonumeroOrcamento == autonumeroOrcamento && a.cancelado != "S"
                                  group a by a.autonumeroPlanilhaFechada into g
                                  select new
                                  {

                                      autonumeroPlanilhaFechada = g.Key,
                                      quantidadePF = g.Sum(a => a.quantidadePF)

                                  }
                                   ).ToList();

                        foreach (var valueItem in p1)
                        {
                            var quantidade = Convert.ToDecimal(valueItem.quantidadePF);
                            var autonumeroPlanilhaFechada = Convert.ToInt64(valueItem.autonumeroPlanilhaFechada);

                            if (valueItem.quantidadePF > 0)
                            {
                                // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.autonumero == autonumeroPlanilhaFechada);
                                if (planilhaFechada != null)
                                {
                                    var estoque = planilhaFechada.estoque - quantidade;
                                    if (estoque < 0)
                                    {
                                        var i = new tb_orcamento_itens();

                                        i.autonumero = 0;
                                        i.autonumeroCliente = 0;
                                        i.autonumeroInsumoServico = 0;
                                        i.autonumeroOrcamento = 0;
                                        i.autonumeroPlanilhaFechada = 0;
                                        i.cancelado = "N";
                                        i.codigoInsumoServico = "";
                                        i.codigoOrcamento = null;
                                        i.codigoPlanilhaFechada = planilhaFechada.codigo;
                                        i.data = null;
                                        i.memoriaCalculo = "";
                                        i.nome = "Item com mais de uma referência CodigoPlanilha Fechada : " + planilhaFechada.codigo;
                                        i.nomeCliente = "";
                                        i.nomeFonte = "";
                                        i.nomeFontePF = "";
                                        i.nomePF = "Item com mais de uma referência CodigoPlanilha Fechada : " + planilhaFechada.codigo;
                                        i.precoUnitario = 0;
                                        i.precoUnitarioPF = 0;
                                        i.quantidade = planilhaFechada.estoque;
                                        i.quantidadePF = quantidade;
                                        i.servico = null;
                                        i.total = 0;
                                        i.totalPF = 0;
                                        i.unidade = "";
                                        i.itemPF = (int)planilhaFechada.item;
                                        i.unidadePF = "";

                                        item.Add(i);

                                    }
                                }

                            }

                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                        }


                        return item;
                    }
                    else
                    {
                        throw new ArgumentException("Execption - tb_orcamento_itens = Null");
                    }


                }
            }
        }





    }
}
