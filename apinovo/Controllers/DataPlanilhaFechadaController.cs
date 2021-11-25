using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
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
    public class DataPlanilhaFechadaController : ApiController
    {
        public class planilhaFechada
        {
            public Int64 autonumero { get; set; }
            public string nome { get; set; }
            public string unidade { get; set; }
            public decimal preco { get; set; }
            public string codigo { get; set; }
            public string fonte { get; set; }
            public Int64 item { get; set; }
            public decimal qtdeContratada { get; set; }
            public decimal estoque { get; set; }
            public decimal qtdeCustoFixo { get; set; }
            public decimal qtdeUsada { get; set; }
        }

        public class itensNasOs
        {
            public string nomeFonte { get; set; }
            public string codigoInsumoServico { get; set; }
            public string codigoOs { get; set; }
            public decimal quantidade { get; set; }
            public string medicao { get; set; }
            public string etapa { get; set; }
        }

        [HttpGet]
        public IEnumerable<tb_planilhafechada> GetAllPlanilhaFechada(Int64 autonumeroCliente)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_planilhafechada where p.autonumeroCliente == autonumeroCliente select p;
                return user.ToList().OrderBy(p => p.fonte).ThenBy(p => p.item);
            }
        }

        [HttpGet]
        public List<planilhaFechada> GetPlanilhaNome(string pesq1, string pesq2, string fonte, Int64 autonumeroCliente, bool estqMaiorZero)
        {

            List<planilhaFechada> l1 = null;


            var p1 = string.Empty;
            var p2 = string.Empty;
            var p3 = string.Empty;



            if (!string.IsNullOrEmpty(pesq1))
            { p1 = pesq1; }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = pesq2; }

            using (var dc = new manutEntities())
            {
                if (!estqMaiorZero)
                {
                    Debug.WriteLine("! estqMaiorZero");
                    if (string.IsNullOrEmpty(p2))
                    {
                        Debug.WriteLine("1");

                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p1) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque == 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada


                              }).ToList();
                    }

                    if (string.IsNullOrEmpty(p1))
                    {
                        Debug.WriteLine("2");
                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p2) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque == 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada
                              }).ToList();
                    }

                    if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                    {
                        Debug.WriteLine("3");
                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque == 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada

                              }).ToList(); ;
                    }

                }
                else
                {

                    Debug.WriteLine("xxxxxx" + p1);
                    Debug.WriteLine("autonumeroCliente" + autonumeroCliente.ToString());
                    if (string.IsNullOrEmpty(p2))
                    {
                        Debug.WriteLine("4");
                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p1) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque > 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada

                              }).ToList();
                    }

                    if (string.IsNullOrEmpty(p1))
                    {
                        Debug.WriteLine("5");
                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p2) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque > 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada
                              }).ToList();
                    }

                    if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                    {
                        Debug.WriteLine("6");
                        l1 = (from p in dc.tb_planilhafechada
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque > 0
                              select new planilhaFechada
                              {
                                  item = p.item,
                                  autonumero = p.autonumero,
                                  codigo = p.codigo,
                                  fonte = p.fonte,
                                  nome = p.nome,
                                  preco = p.preco,
                                  unidade = p.unidade,
                                  qtdeContratada = p.qtdeContratada,
                                  qtdeCustoFixo = p.qtdeCustoFixo,
                                  estoque = p.estoque,
                                  qtdeUsada = p.qtdeUsada

                              }).ToList(); ;
                    }
                }

            }

            return l1.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();


        }

        [HttpGet]
        public List<planilhaFechada> GetPlanilhaFechadaCodigo(string pesq1, string pesq2, string fonte, Int64 autonumeroCliente)
        {

            List<planilhaFechada> l1 = null;


            var p1 = string.Empty;
            var p2 = string.Empty;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = pesq1.Trim(); }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = pesq2.Trim(); }

            using (var dc = new manutEntities())
            {

                if (string.IsNullOrEmpty(p2))
                {

                    l1 = (from p in dc.tb_planilhafechada
                          where p.codigo.Contains(p1) && p.autonumeroCliente == autonumeroCliente
                          select new planilhaFechada
                          {
                              item = p.item,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              qtdeContratada = p.qtdeContratada,
                              qtdeCustoFixo = p.qtdeCustoFixo,
                              estoque = p.estoque,
                              qtdeUsada = p.qtdeUsada

                          }).ToList(); ;
                }

                if (string.IsNullOrEmpty(p1))
                {
                    l1 = (from p in dc.tb_planilhafechada
                          where p.codigo.Contains(p2) && p.autonumeroCliente == autonumeroCliente
                          select new planilhaFechada
                          {
                              item = p.item,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              qtdeContratada = p.qtdeContratada,
                              qtdeCustoFixo = p.qtdeCustoFixo,
                              estoque = p.estoque,
                              qtdeUsada = p.qtdeUsada
                          }).ToList(); ;
                }

                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                {
                    l1 = (from p in dc.tb_planilhafechada
                          where p.codigo.Contains(p1) && p.codigo.Contains(p2) && p.autonumeroCliente == autonumeroCliente
                          select new planilhaFechada
                          {
                              item = p.item,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              qtdeContratada = p.qtdeContratada,
                              qtdeCustoFixo = p.qtdeCustoFixo,
                              estoque = p.estoque,
                              qtdeUsada = p.qtdeUsada
                          }).ToList(); ;
                }

            }


            return l1.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();


        }


        [HttpGet]
        public List<planilhaFechada> GetPlanilhaFechadaPreco(string pesq1, string pesq2, string fonte, Int64 autonumeroCliente, bool estqMaiorZero)
        {

            List<planilhaFechada> l1 = null;


            var p1 = 0m;
            var p2 = 0m;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = Convert.ToDecimal(pesq1.Trim()); }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = Convert.ToDecimal(pesq2.Trim()); }

            using (var dc = new manutEntities())
            {
                if (!estqMaiorZero)
                {
                    l1 = (from p in dc.tb_planilhafechada
                          where p.preco >= p1 && p.preco <= p2 && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque == 0
                          select new planilhaFechada
                          {
                              item = p.item,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              qtdeContratada = p.qtdeContratada,
                              qtdeCustoFixo = p.qtdeCustoFixo,
                              estoque = p.estoque,
                              qtdeUsada = p.qtdeUsada
                          }).Take(500).ToList();
                }
                else
                {
                    l1 = (from p in dc.tb_planilhafechada
                          where p.preco >= p1 && p.preco <= p2 && fonte.Contains(p.fonte) && p.autonumeroCliente == autonumeroCliente && p.estoque > 0
                          select new planilhaFechada
                          {
                              item = p.item,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              qtdeContratada = p.qtdeContratada,
                              qtdeCustoFixo = p.qtdeCustoFixo,
                              estoque = p.estoque,
                              qtdeUsada = p.qtdeUsada
                          }).Take(500).ToList();
                }
            }

            return l1.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();

        }


        [HttpGet]
        public List<string> GetAllTabelas(Int64 autonumeroCliente)
        {

            using (var dc = new manutEntities())
            {

                return (from ta in dc.tb_planilhafechada select ta.fonte).Distinct().Union(from tb in dc.tb_planilhafechada select tb.fonte).ToList();
            }

        }
        [HttpPost]
        public string somarQtdeContratadaPlanilha()
        {

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
            var qtdeSoma = Convert.ToDecimal(HttpContext.Current.Request.Form["qtdeSoma"]);


            using (var dc = new manutEntities())
            {
                var linha = dc.tb_planilhafechada.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    //var estoque = linha.qtdeContratada - qtdeCustoFixo;
                    //if (estoque < 0)
                    //{
                    //    throw new ArgumentException("Erro - Estoque Negativo");
                    //}
                    linha.qtdeContratada = linha.qtdeContratada + qtdeSoma;
                    //linha.estoque = estoque;
                    dc.tb_planilhafechada.AddOrUpdate(linha);
                    dc.SaveChanges();
                    //return estoque.ToString("########0");
                    return "";
                }
                else
                {
                    throw new ArgumentException("Erro - Produto não encontrado");
                }

            }
        }


        [HttpPost]
        public string AlterarQtdeCustoFixo()
        {
            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
            var qtdeCustoFixo = Convert.ToDecimal(HttpContext.Current.Request.Form["qtdeCustoFixo"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_planilhafechada.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    //var estoque = linha.qtdeContratada - qtdeCustoFixo;
                    //if (estoque < 0)
                    //{
                    //    throw new ArgumentException("Erro - Estoque Negativo");
                    //}
                    linha.qtdeCustoFixo = qtdeCustoFixo;
                    //linha.estoque = estoque;
                    dc.tb_planilhafechada.AddOrUpdate(linha);
                    dc.SaveChanges();
                    //return estoque.ToString("########0");
                    return "";
                }
                else
                {
                    throw new ArgumentException("Erro - Produto não encontrado");
                }

            }
        }

        [HttpPost]
        public string AcertarEstoquePlanilha()
        {
            var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

            var auto = new SqlParameter("@auto", System.Data.SqlDbType.BigInt)
            {
                Value = autonumeroCliente
            };

            var csql = new StringBuilder();

            using (var dc = new manutEntities())
            {

                //using (var transaction = dc.Database.BeginTransaction())
                //{
                //    try
                //    {

                csql.Append("DROP TABLE IF EXISTS  xTemp; ");
                csql.Append("CREATE TEMPORARY TABLE xTemp ( ");
                //csql.Append("SELECT p.autonumero as autonumeroPlanilhaFechada,sum(i.quantidade) as qtdeTotalAcumulada, (p.qtdeContratada  - p.qtdeCustoFixo  - sum(i.quantidade)) as estoque ");
                csql.Append("SELECT p.autonumero as autonumeroPlanilhaFechada,sum(i.quantidadePF) as qtdeTotalAcumulada, (p.qtdeContratada  - sum(i.quantidadePF)) as estoque ");
                csql.Append("FROM tb_os_itens as i,tb_planilhafechada as p ");
                csql.Append("WHERE i.codigoPf = p.codigo and i.autonumeroCliente = {0} and  p.autonumeroCliente = {0}  and i.cancelado != 'S' and i.codigoPf != '' ");
                csql.Append("GROUP by i.codigoPF );   ");

                csql.Append("UPDATE tb_planilhafechada ");
                csql.Append("SET estoque = qtdeContratada,qtdeUsada = 0 where autonumeroCliente = {0};  ");


                csql.Append("UPDATE tb_planilhafechada ");
                csql.Append("INNER JOIN xTemp ON tb_planilhafechada.autonumero = xTemp.autonumeroPlanilhaFechada ");
                csql.Append("SET  tb_planilhafechada.estoque = xTemp.estoque, tb_planilhafechada.qtdeUsada = xTemp.qtdeTotalAcumulada;");


                var user2 = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { autonumeroCliente });


                var acrescimoEstoque = 0m;
                var linha = dc.tb_cliente.FirstOrDefault(a => a.autonumero == autonumeroCliente);
                if (linha != null)
                {
                    acrescimoEstoque = (decimal)linha.acrescimoEstoque;

                }
                else
                {
                    throw new ArgumentException("Execption - tb_planilhafechada - Acréscimo não encontrado");
                }

                if (acrescimoEstoque > 0m)
                {
                    //var multiplicador = ( 1 + (acrescimoEstoque / 100));

                    var multiplicador = (acrescimoEstoque / 100);

                    dc.tb_planilhafechada.Where(p => p.autonumeroCliente == autonumeroCliente).ToList().ForEach(x =>
                   {
                       //x.estoque = x.estoque * multiplicador;
                       x.estoque = x.estoque + (x.qtdeContratada * multiplicador);
                       dc.tb_planilhafechada.AddOrUpdate(x);

                   });

                    dc.SaveChanges();
                }

                return string.Empty;


            }
        }

        [HttpPost]
        public List<tb_planilhafechada> ValidarCustoFixoEstoque()
        {
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {

                    var item = new List<tb_planilhafechada>();

                    var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    var prazoInicialMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prazoInicialMeses"].ToString());



                    //var nroDeEtapas = (from l in dc.tb_etapa.Where(a => a.autonumeroCliente == autonumeroCliente) select l).Count();

                    var p = dc.tb_planilhafechada.Where(a => a.autonumeroCliente == autonumeroCliente && a.qtdeCustoFixo > 0).ToList();

                    if (p != null)
                    {
                        foreach (var valueItem in p)
                        {
                            var quantidade = Convert.ToDecimal(valueItem.qtdeCustoFixo); ;
                            var estoque = valueItem.estoque - quantidade / prazoInicialMeses;
                            if (estoque < 0)
                            {
                                valueItem.qtdeCustoFixo = valueItem.estoque; // Passar o Estoque da Planilha para o SITE
                                item.Add(valueItem);
                            }

                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                        }
                        return item;
                    }
                    else
                    {
                        throw new ArgumentException("Execption - tb_planilhafechada = Null");
                    }


                }
            }
        }


        [HttpGet]
        public IEnumerable<itensNasOs> GetItemNasOS(Int64 autonumeroCliente, string codigo, string nomeFonte)
        {
            using (var dc = new manutEntities())
            {
                var l1 = (from p in dc.tb_os_itens
                          where p.nomeFontePF == nomeFonte && p.autonumeroCliente == autonumeroCliente && p.codigoPF == codigo && p.quantidadePF > 0 && p.cancelado != "S"
                          select new itensNasOs
                          {
                              nomeFonte = p.nomeFonte,
                              codigoInsumoServico = p.codigoInsumoServico,
                              codigoOs = p.codigoOs,
                              quantidade = (decimal)p.quantidadePF,
                              medicao = p.medicao,
                              etapa = p.etapa

                          }).OrderBy(a => a.codigoOs).ToList();
                return l1;
            }
        }


        [HttpPost]
        public HttpResponseMessage totalUtilizado()
        {
            var cc = 1;
            try
            {
                using (var dc = new manutEntities())
                {
                    var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();

                    var totalPlanilhaFechada = (from i in dc.tb_planilhafechada where i.autonumeroCliente == autonumeroCliente select i).Sum(i => i.qtdeContratada * i.preco);

                    var totalCustoFixo = (from i in dc.tb_planilhafechada where i.autonumeroCliente == autonumeroCliente select i).Sum(i => i.qtdeCustoFixo * i.preco);

                    var totalCustoFixoUsadaoOs = (from i in dc.tb_os where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.importadaCustoFixo == "S" select i).Sum(i => i.valor);

                    var totalOsFechadaMedida = (from i in dc.tb_os where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && (!i.nomeStatus.Contains("Aberta")) select i).Sum(i => i.valor);

                    var totalOsAberta = (from i in dc.tb_os where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.nomeStatus.Contains("Aberta") select i).Sum(i => i.valor);

                    var totalOrcamentoAberto = (from i in dc.tb_orcamento where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.codigoOs == "" select i).Sum(i => i.valor);

                    var totalPrecoXEstoque = (from i in dc.tb_planilhafechada where i.autonumeroCliente == autonumeroCliente select i).Sum(i => i.estoque * i.preco);


                    //if (totalPlanilhaFechada == null)
                    //{
                    //    totalPlanilhaFechada = 0;
                    //}
                    //if (totalCustoFixo == null)
                    //{
                    //    totalCustoFixo = 0;
                    //}
                    if (totalCustoFixoUsadaoOs == null)
                    {
                        totalCustoFixoUsadaoOs = 0;
                    }
                    if (totalOsAberta == null)
                    {
                        totalOsAberta = 0;
                    }
                    if (totalOrcamentoAberto == null)
                    {
                        totalOrcamentoAberto = 0;
                    }
                    if (totalOsFechadaMedida == null)
                    {
                        totalOsFechadaMedida = 0;
                    }


                    decimal saldoCustoFixo = (decimal)(totalCustoFixo - totalCustoFixoUsadaoOs);
                    if (saldoCustoFixo < 0)
                    {

                        saldoCustoFixo = 0m;
                    }

                    decimal totalUtilizado = (decimal)(totalPlanilhaFechada - saldoCustoFixo - totalOsFechadaMedida);

                    using (var rd = new ReportDocument())
                    {
                        var Response = HttpContext.Current.ApplicationInstance.Response;

                        var local = HttpContext.Current.Server.MapPath("~/rpt/totalUtilizado.rpt");

                        rd.Load(local);

                        rd.SetParameterValue("totalPlanilhaFechada", totalPlanilhaFechada);
                        //rd.SetParameterValue("totalCustoFixo", totalCustoFixo);
                        //rd.SetParameterValue("totalCustoFixoUsadaoOs", totalCustoFixoUsadaoOs);
                        rd.SetParameterValue("totalOsFechadaMedida", totalOsFechadaMedida);
                        rd.SetParameterValue("totalOsAberta", totalOsAberta);
                        rd.SetParameterValue("totalOrcamentoAberto", totalOrcamentoAberto);
                        rd.SetParameterValue("saldoCustoFixo", saldoCustoFixo);
                        rd.SetParameterValue("totalUtilizado", totalUtilizado);
                        rd.SetParameterValue("nomeCliente", nomeCliente);
                        rd.SetParameterValue("totalPrecoXEstoque", totalPrecoXEstoque);

                        rd.RecordSelectionFormula = string.Empty;

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
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }

        }
    }
}
