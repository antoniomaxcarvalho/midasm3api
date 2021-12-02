using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataOsItemController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_os_itens> GetItensOsCodigoOs(string codigoOs)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os_itens.Where((a => (a.codigoOs == codigoOs && a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
                return user.ToList();
            }

        }

        [HttpGet]
        public IEnumerable<tb_os_itens> GetItensOsAutonumero(Int64 autonumeroOs)
        {

            DataOsController.AcertarValoresNaOs(string.Empty, autonumeroOs);

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os_itens.Where((a => (a.autonumeroOs == autonumeroOs) && a.cancelado != "S")).OrderByDescending(p => p.autonumero) select p;
                return user.ToList(); ;
            }

        }

        [HttpGet]
        public IEnumerable<tb_usuario> GetUsuarioSenha(string login, string senha)
        {
            var c = 2;
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_usuario.Where((a => (a.login == login) && a.senha == senha)) select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<tb_os_itens> GetAllItensOs()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os_itens.Where((a => (a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
                return user.ToList(); ;
            }

        }

        [HttpDelete]
        public string CancelarItensOs()
        {
            var message = "Erro";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var linha = dc.tb_os_itens.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {


                            // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                            var cliente = dc.tb_cliente.FirstOrDefault(a => a.autonumero == linha.autonumeroCliente);
                            if (cliente != null)
                            {
                                if (linha.nomeFonte.Contains("MANUT") || linha.nomeFonte.Contains("EQUIPE"))
                                {
                                    var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == linha.codigoInsumoServico && a.autonumeroCliente == cliente.autonumero);
                                    if (planilhaFechada != null)
                                    {
                                        planilhaFechada.estoque = planilhaFechada.estoque + (decimal)linha.quantidade;
                                        planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada - (decimal)linha.quantidade;
                                        dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                        dc.SaveChanges();
                                    }
                                }
                            }
                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------

                            linha.cancelado = "S";
                            dc.tb_os_itens.AddOrUpdate(linha);
                            dc.SaveChanges();


                            decimal totalItens = 0m;
                            decimal totalItensServico = 0m;
                            decimal totalItensMaterial = 0m;

                            var p = dc.tb_os_itens.Where(a => a.autonumeroOs == linha.autonumeroOs && a.cancelado != "S").ToList();

                            if (p != null)
                            {
                                foreach (var value in p)
                                {
                                    if (value.servico.Equals("S"))
                                    {
                                        totalItensServico = Convert.ToDecimal(value.total) + totalItensServico;
                                    }
                                    else
                                    {
                                        totalItensMaterial = Convert.ToDecimal(value.total) + totalItensMaterial;
                                    }
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                            }

                            var k = dc.tb_os.FirstOrDefault(a => a.autonumero == linha.autonumeroOs && a.cancelado != "S");
                            if (k != null)
                            {
                                k.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                                k.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                                k.valor = Convert.ToDecimal(k.valorServico + k.valorMaterial);
                                dc.tb_os.AddOrUpdate(k);
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
        public string IncluirItensOs()
        {
            var c = 1;
            var form = System.Web.HttpContext.Current.Request.Form;
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {

                        //Debug.WriteLine("inicio");
                        //var form = System.Web.HttpContext.Current.Request.Form;
                        //Debug.WriteLine(form);
                        var unidade = HttpContext.Current.Request.Form["unidade"].ToString().Trim();
                        //Debug.WriteLine(unidade);
                        var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                        //Debug.WriteLine(nome);
                        var nomeFonte = HttpContext.Current.Request.Form["nomeFonte"].ToString();
                        //Debug.WriteLine(nomeFonte);
                        var quantidade = Convert.ToDecimal(HttpContext.Current.Request.Form["quantidade"].ToString());
                        //Debug.WriteLine(quantidade);

                        var precoUnitario = Convert.ToDecimal(HttpContext.Current.Request.Form["precoUnitario"].ToString());
                        var total = Convert.ToDecimal(HttpContext.Current.Request.Form["total"].ToString());
                        var bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString().Trim());
                        var bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString().Trim());
                        var autonumeroOs = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroOs"].ToString());
                        var codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                        var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                        var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                        var servico = HttpContext.Current.Request.Form["servico"].ToString().Trim();
                        var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                        var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                        var codigoInsumoServico = HttpContext.Current.Request.Form["codigoInsumoServico"].ToString().Trim();
                        var memoriaCalculo = HttpContext.Current.Request.Form["memoriaCalculo"].ToString();
                        var itemPF = Convert.ToInt32(HttpContext.Current.Request.Form["itemPF"].ToString());

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


                        var tot2 = (Math.Truncate((decimal)quantidade * (decimal)precoUnitario * 100)) / 100;

                        var Os = new tb_os_itens
                        {

                            nome = nome,
                            nomeFonte = nomeFonte,
                            unidade = unidade,
                            quantidade = quantidade,
                            precoUnitario = precoUnitario,
                            total = tot2,
                            bdiServico = bdiServico,
                            bdiMaterial = bdiMaterial,
                            autonumeroOs = autonumeroOs,
                            codigoOs = codigoOs,
                            autonumeroUsuario = autonumeroUsuario,
                            nomeUsuario = nomeUsuario,
                            cancelado = "N",
                            data = dataIncluir,
                            servico = servico,
                            autonumeroCliente = autonumeroCliente,
                            nomeCliente = nomeCliente,
                            codigoInsumoServico = codigoInsumoServico,
                            memoriaCalculo = memoriaCalculo,
                            nomePF = "",
                            quantidadePF = 0,
                            precoUnitarioPF = 0,
                            totalPF = 0,
                            nomeFontePF = "",
                            itemPF = 0,
                            unidadePF = "",
                            codigoPF = "",
                            codigoOrdemServico = "",
                            etapa = "",
                            medicao = "",
                            qtdeMaoObra = 0

                        };

                        if (nomeFonte.Contains("MANUT") || nomeFonte.Contains("EQUIPE"))
                        {
                            var tot = (Math.Truncate((decimal)Os.quantidade * (decimal)Os.precoUnitarioPF * 100)) / 100;
                            Os.nomePF = Os.nome;
                            Os.quantidadePF = Os.quantidade;
                            Os.precoUnitarioPF = Os.precoUnitario;
                            Os.totalPF = Os.total;
                            Os.nomeFontePF = Os.nomeFonte;
                            Os.itemPF = itemPF;
                            Os.unidadePF = Os.unidade;
                            Os.codigoPF = Os.codigoInsumoServico;
                        }

                        dc.tb_os_itens.Add(Os);
                        dc.SaveChanges();


                        // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                        var cliente = dc.tb_cliente.FirstOrDefault(a => a.autonumero == autonumeroCliente);
                        if (cliente != null)
                        {
                            if (nomeFonte.Contains("MANUT") || nomeFonte.Contains("EQUIPE"))
                            {
                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == codigoInsumoServico && a.autonumeroCliente == autonumeroCliente);
                                if (planilhaFechada != null)
                                {
                                    // As quantidades em estoque poderá ser aumentada com o acrescimo no cadastro de cliente e recalcular a planilha
                                    var estoque = planilhaFechada.estoque - quantidade;
                                    if (estoque < 0) throw new ArgumentException("planilhaFechada.estoque: " + codigoInsumoServico + " Estoque: " + planilhaFechada.estoque.ToString() + " - quantidade: " + quantidade.ToString() + "  < 0");

                                    planilhaFechada.estoque = estoque;
                                    planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada + quantidade;
                                    dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                    dc.SaveChanges();
                                }
                            }
                        }
                        // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------


                        decimal totalItens = 0m;
                        decimal totalItensServico = 0m;
                        decimal totalItensMaterial = 0m;
                        var auto = Convert.ToInt32(Os.autonumero);

                        var p = dc.tb_os_itens.Where(a => a.autonumeroOs == autonumeroOs && a.cancelado != "S").ToList();

                        if (p != null)
                        {
                            foreach (var value in p)
                            {
                                if (value.servico.Equals("S"))
                                {
                                    totalItensServico = Convert.ToDecimal(value.total) + totalItensServico;
                                }
                                else
                                {
                                    totalItensMaterial = Convert.ToDecimal(value.total) + totalItensMaterial;
                                }
                                totalItens = Convert.ToDecimal(value.total) + totalItens;
                            }
                            var k = dc.tb_os.FirstOrDefault(a => a.autonumero == autonumeroOs && a.cancelado != "S");
                            if (k != null)
                            {
                                k.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                                k.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                                k.valor = Convert.ToDecimal(k.valorServico + k.valorMaterial);

                                dc.tb_os.AddOrUpdate(k);
                                dc.SaveChanges();
                            }
                            else
                            {
                                throw new ArgumentException("Execption - Baixa no Estoque");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Execption - tb_os_itens = Null");
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
        public string IncluirItemPrecoZeroPlanilha()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
            try
            {
                using (var dc = new manutEntities())
                {
                    var linha = dc.tb_os_itens.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {

                        //linha.nome = "";
                        //linha.unidade = "";
                        linha.unidadePF = "";
                        linha.quantidade = 0;
                        //linha.precoUnitario = 0;
                        linha.total = 0;
                        linha.quantidadePF = 0;
                        linha.precoUnitarioPF = 0;
                        linha.totalPF = 0;
                        linha.nomeFontePF = "";
                        linha.nomePF = "";
                        linha.itemPF = 0;
                        linha.codigoPF = "";

                        linha.codigoOrdemServico = "";

                        dc.tb_os_itens.Add(linha);
                        dc.SaveChanges();



                    }


                }
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
            return message;

        }



        //[HttpPost]
        //public void AlterarItemQtde()
        //{

        //    using (var dc = new manutEntities())
        //    {
        //        var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

        //        var linha = dc.tb_os.Find(autonumero); // sempre irá procurar pela chave primaria
        //        if (linha != null && linha.cancelado != "S")
        //        {
        //            linha.codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
        //            linha.autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //            linha.nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //            linha.autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
        //            linha.nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();



        //            dc.tb_os.AddOrUpdate(linha);
        //            dc.SaveChanges();

        //        }
        //    }

        //}

        [HttpPost]
        public string AlterarItemQtde()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
            var qtde = Convert.ToDecimal(HttpContext.Current.Request.Form["qtde"]);
            var memoriaCalculo = HttpContext.Current.Request.Form["memoriaCalculo"].ToString().Trim();

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var linha = dc.tb_os_itens.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {


                            // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                            var cliente = dc.tb_cliente.FirstOrDefault(a => a.autonumero == linha.autonumeroCliente);
                            if (cliente != null)
                            {
                                if (linha.nomeFonte.Contains("MANUT") || linha.nomeFonte.Contains("EQUIPE"))
                                {
                                    var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == linha.codigoInsumoServico && a.autonumeroCliente == cliente.autonumero);
                                    if (planilhaFechada != null)
                                    {

                                        var estoque = planilhaFechada.estoque + (decimal)linha.quantidade - qtde;
                                        if (estoque < 0) throw new ArgumentException("Execption");

                                        planilhaFechada.estoque = estoque;
                                        planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada - (decimal)linha.quantidade + qtde;
                                        dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                        dc.SaveChanges();
                                    }
                                }
                            }
                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------


                            linha.quantidade = qtde;
                            linha.total = qtde * Convert.ToDecimal(linha.precoUnitario);

                            if (linha.nomeFonte.Contains("MANUT") || linha.nomeFonte.Contains("EQUIPE"))
                            {
                                linha.quantidadePF = linha.quantidade;
                                linha.totalPF = linha.total;
                            }
                            linha.memoriaCalculo = memoriaCalculo;
                            dc.tb_os_itens.AddOrUpdate(linha);
                            dc.SaveChanges();

                            decimal totalItens = 0m;
                            decimal totalItensServico = 0m;
                            decimal totalItensMaterial = 0m;

                            var p = dc.tb_os_itens.Where(a => a.autonumeroOs == linha.autonumeroOs && a.cancelado != "S").ToList();

                            if (p != null)
                            {
                                foreach (var value in p)
                                {
                                    if (value.servico.Equals("S"))
                                    {
                                        totalItensServico = Convert.ToDecimal(value.total) + totalItensServico;
                                    }
                                    else
                                    {
                                        totalItensMaterial = Convert.ToDecimal(value.total) + totalItensMaterial;
                                    }
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                                var k = dc.tb_os.FirstOrDefault(a => a.autonumero == linha.autonumeroOs && a.cancelado != "S");
                                if (k != null)
                                {

                                    k.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                                    k.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                                    k.valor = Convert.ToDecimal(k.valorServico + k.valorMaterial);
                                    dc.tb_os.AddOrUpdate(k);
                                    dc.SaveChanges();
                                }
                                else
                                {
                                    throw new ArgumentException("Execption");
                                }
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
        public string AlterarItemQtdeMaoObra()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var codigoPF = HttpContext.Current.Request.Form["codigoPF"].ToString();
            var codigoOrdemServico = HttpContext.Current.Request.Form["codigoOrdemServico"].ToString();

            var qtde = Convert.ToInt16(HttpContext.Current.Request.Form["qtde"]);

            using (var dc = new manutEntities())
            {
                try
                {
                    dc.tb_os_itens.Where(p => p.codigoPF == codigoPF && p.codigoOrdemServico == codigoOrdemServico && p.cancelado != "S").ToList().ForEach(x =>
                    {
                        x.qtdeMaoObra = qtde;
                    });
                    dc.SaveChanges();
                }
                catch (Exception)
                {
                    return "Erro";
                }

            }

            return message;

        }



        [HttpPost]
        public string IncluirItensOsComOrcamento()
        {
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var autonumeroOrcamento = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOrcamento"].ToString());
                        var autonumeroOs = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOs"].ToString());
                        var codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                        var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                        var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                        var bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString().Trim());
                        var bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString().Trim());
                        var dataLimite = HttpContext.Current.Request.Form["dataLimite"].ToString();

                        var dataIncluir = DateTime.Now;
                        if (DataClienteController.IsDate(dataLimite))
                        {
                            var dataLimite2 = Convert.ToDateTime(dataLimite);
                            if (dataIncluir > dataLimite2)
                            {
                                dataIncluir = dataLimite2;
                            }
                        }

                        var p = dc.tb_orcamento_itens.Where(a => a.autonumeroOrcamento == autonumeroOrcamento && a.cancelado != "S").ToList();

                        if (p != null)
                        {
                            foreach (var valueItem in p)
                            {
                                if (string.IsNullOrEmpty(valueItem.nomeFontePF))
                                {
                                    continue;
                                }
                                var unidadePF = valueItem.unidadePF;
                                var nome = valueItem.nomePF;
                                var nomeFonte = valueItem.nomeFontePF;
                                var quantidade = Convert.ToDecimal(valueItem.quantidadePF);
                                var precoUnitario = Convert.ToDecimal(valueItem.precoUnitarioPF);
                                var total = Convert.ToDecimal(valueItem.totalPF);

                                var servico = valueItem.servico;
                                var autonumeroCliente = Convert.ToInt32(valueItem.autonumeroCliente);
                                var nomeCliente = valueItem.nomeCliente;
                                var codigoInsumoServico = valueItem.codigoPlanilhaFechada;
                                var memoriaCalculo = valueItem.memoriaCalculo;



                                var autonumeroPlanilhaFechada = Convert.ToInt64(valueItem.autonumeroPlanilhaFechada);

                                var Ositem = new tb_os_itens
                                {
                                    nome = nome,
                                    nomeFonte = nomeFonte,
                                    unidade = unidadePF,
                                    quantidade = quantidade,
                                    precoUnitario = precoUnitario,
                                    total = total,
                                    bdiServico = bdiServico,
                                    bdiMaterial = bdiMaterial,
                                    autonumeroOs = autonumeroOs,
                                    codigoOs = codigoOs,
                                    autonumeroUsuario = autonumeroUsuario,
                                    nomeUsuario = nomeUsuario,
                                    cancelado = "N",
                                    data = dataIncluir,
                                    servico = servico,
                                    autonumeroCliente = autonumeroCliente,
                                    nomeCliente = nomeCliente,
                                    codigoInsumoServico = codigoInsumoServico,
                                    memoriaCalculo = memoriaCalculo,
                                    nomePF = "",
                                    quantidadePF = 0,
                                    precoUnitarioPF = 0,
                                    totalPF = 0,
                                    nomeFontePF = "",
                                    itemPF = 0,
                                    unidadePF = "",
                                    codigoPF = "",
                                    codigoOrdemServico = "",
                                    etapa = "",
                                    medicao = "",
                                    qtdeMaoObra = 0


                                };

                                if (nomeFonte.Contains("MANUT") || nomeFonte.Contains("EQUIPE"))
                                {
                                    Ositem.nomePF = Ositem.nome;
                                    Ositem.quantidadePF = Ositem.quantidade;
                                    Ositem.precoUnitarioPF = Ositem.precoUnitario;
                                    Ositem.totalPF = Ositem.total;
                                    Ositem.nomeFontePF = Ositem.nomeFonte;
                                    Ositem.itemPF = Ositem.itemPF;
                                    Ositem.unidadePF = Ositem.unidade;
                                    Ositem.codigoPF = Ositem.codigoInsumoServico;
                                }

                                dc.tb_os_itens.Add(Ositem);
                                dc.SaveChanges();

                                // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------

                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == codigoInsumoServico && a.autonumeroCliente == autonumeroCliente);
                                if (planilhaFechada != null)
                                {
                                    var estoque = planilhaFechada.estoque - quantidade;
                                    if (estoque < 0) throw new ArgumentException("Execption# planilhaFechada.estoque: " + codigoInsumoServico + " Estoque: " + planilhaFechada.estoque.ToString() + " - quantidade: " + quantidade.ToString() + "  < 0");

                                    planilhaFechada.estoque = estoque;
                                    planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada + quantidade;

                                    dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                    dc.SaveChanges();
                                }

                                // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                            }

                            decimal totalItens = 0m;
                            decimal totalItensServico = 0m;
                            decimal totalItensMaterial = 0m;

                            var g = dc.tb_os_itens.Where(a => a.autonumeroOs == autonumeroOs && a.cancelado != "S").ToList();

                            if (g != null)
                            {
                                foreach (var value in g)
                                {
                                    if (value.servico.Equals("S"))
                                    {
                                        totalItensServico = Convert.ToDecimal(value.total) + totalItensServico;
                                    }
                                    else
                                    {
                                        totalItensMaterial = Convert.ToDecimal(value.total) + totalItensMaterial;
                                    }
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                                var k = dc.tb_os.FirstOrDefault(a => a.autonumero == autonumeroOs && a.cancelado != "S");
                                if (k != null)
                                {
                                    k.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                                    k.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                                    k.valor = Convert.ToDecimal(k.valorServico + k.valorMaterial);

                                    dc.tb_os.AddOrUpdate(k);
                                    dc.SaveChanges();
                                }
                                else
                                {
                                    throw new ArgumentException("Execption - Baixa no Estoque");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Execption - tb_os_itens = Null");
                            }

                            var s = dc.tb_orcamento.FirstOrDefault(a => a.autonumero == autonumeroOrcamento && a.cancelado != "S");
                            if (s != null)
                            {
                                s.codigoOs = codigoOs;
                                dc.tb_orcamento.AddOrUpdate(s);
                                dc.SaveChanges();
                            }

                            transaction.Commit();
                            return totalItens.ToString("########0.00");

                        }
                        else
                        {
                            throw new ArgumentException("Execption - tb_orcamento_itens = Null");
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
        public string IncluirItensOsComCustoFixo()
        {
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                        var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
                        var autonumeroOs = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroOs"].ToString());
                        var codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                        var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                        var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                        var bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString().Trim());
                        var bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString().Trim());
                        var dataLimite = HttpContext.Current.Request.Form["dataLimite"].ToString();
                        var ultimaEtapa = HttpContext.Current.Request.Form["ultimaEtapa"].ToString();
                        var prazoInicialMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prazoInicialMeses"].ToString());

                        var dataIncluir = DateTime.Now;
                        if (DataClienteController.IsDate(dataLimite))
                        {
                            var dataLimite2 = Convert.ToDateTime(dataLimite);
                            if (dataIncluir > dataLimite2)
                            {
                                dataIncluir = dataLimite2;
                            }
                        }

                        //var nroDeEtapas = (from l in dc.tb_etapa.Where(a => a.autonumeroCliente == autonumeroCliente) select l).Count();
                        //if (ultimaEtapa == "S")
                        //{
                        //    nroDeEtapas = 1;
                        //}

                        var p = dc.tb_planilhafechada.Where(a => a.autonumeroCliente == autonumeroCliente && a.qtdeCustoFixo > 0).ToList();

                        if (p != null)
                        {
                            foreach (var valueItem in p)
                            {
                                var unidade = valueItem.unidade;
                                var nome = valueItem.nome;
                                var nomeFonte = valueItem.fonte;
                                var quantidade = Math.Round((Convert.ToDecimal(valueItem.qtdeCustoFixo) / prazoInicialMeses), 4);
                                if (ultimaEtapa == "S")
                                {
                                    quantidade = Convert.ToDecimal(valueItem.estoque);
                                }
                                var precoUnitario = Convert.ToDecimal(valueItem.preco);
                                var total = quantidade * Convert.ToDecimal(valueItem.preco);

                                var servico = "S";
                                var codigoInsumoServico = valueItem.codigo;
                                var memoriaCalculo = string.Empty;
                                var itemPF = valueItem.item;

                                var autonumeroPlanilhaFechada = Convert.ToInt64(valueItem.autonumero);

                                var Ositem = new tb_os_itens
                                {
                                    nome = nome,
                                    nomeFonte = nomeFonte,
                                    unidade = unidade,
                                    quantidade = quantidade,
                                    precoUnitario = precoUnitario,
                                    total = total,
                                    bdiServico = bdiServico,
                                    bdiMaterial = bdiMaterial,
                                    autonumeroOs = autonumeroOs,
                                    codigoOs = codigoOs,
                                    autonumeroUsuario = autonumeroUsuario,
                                    nomeUsuario = nomeUsuario,
                                    cancelado = "N",
                                    data = dataIncluir,
                                    servico = servico,
                                    autonumeroCliente = autonumeroCliente,
                                    nomeCliente = nomeCliente,
                                    codigoInsumoServico = codigoInsumoServico,
                                    memoriaCalculo = memoriaCalculo,
                                    nomePF = nome,
                                    quantidadePF = quantidade,
                                    precoUnitarioPF = precoUnitario,
                                    totalPF = total,
                                    nomeFontePF = nomeFonte,
                                    itemPF = itemPF,
                                    unidadePF = unidade,
                                    codigoPF = codigoInsumoServico,
                                    codigoOrdemServico = "",
                                    etapa = "",
                                    medicao = "",
                                    qtdeMaoObra = 0


                                };

                                if (nomeFonte.Contains("MANUT") || nomeFonte.Contains("EQUIPE"))
                                {
                                    Ositem.nomePF = Ositem.nome;
                                    Ositem.quantidadePF = Ositem.quantidade;
                                    Ositem.precoUnitarioPF = Ositem.precoUnitario;
                                    Ositem.totalPF = Ositem.total;
                                    Ositem.nomeFontePF = Ositem.nomeFonte;
                                    Ositem.itemPF = Ositem.itemPF;
                                    Ositem.unidadePF = Ositem.unidade;
                                    Ositem.codigoPF = Ositem.codigoInsumoServico;
                                }

                                dc.tb_os_itens.Add(Ositem);
                                dc.SaveChanges();

                                // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------

                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == codigoInsumoServico && a.autonumeroCliente == autonumeroCliente);
                                if (planilhaFechada != null)
                                {
                                    var estoque = planilhaFechada.estoque - quantidade;
                                    if (estoque < 0) throw new ArgumentException("Execption -> planilhaFechada.estoque: " + codigoInsumoServico + " Estoque: " + planilhaFechada.estoque.ToString() + " - quantidade: " + quantidade.ToString() + "  < 0");

                                    planilhaFechada.estoque = estoque;
                                    planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada + quantidade;

                                    dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                    dc.SaveChanges();
                                }

                                // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                            }

                            decimal totalItens = 0m;
                            decimal totalItensServico = 0m;
                            decimal totalItensMaterial = 0m;

                            var g = dc.tb_os_itens.Where(a => a.autonumeroOs == autonumeroOs && a.cancelado != "S").ToList();

                            if (g != null)
                            {
                                foreach (var value in g)
                                {
                                    if (value.servico.Equals("S"))
                                    {
                                        totalItensServico = Convert.ToDecimal(value.total) + totalItensServico;
                                    }
                                    else
                                    {
                                        totalItensMaterial = Convert.ToDecimal(value.total) + totalItensMaterial;
                                    }
                                    totalItens = Convert.ToDecimal(value.total) + totalItens;
                                }
                                var k = dc.tb_os.FirstOrDefault(a => a.autonumero == autonumeroOs && a.cancelado != "S");
                                if (k != null)
                                {
                                    k.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                                    k.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                                    k.valor = Convert.ToDecimal(k.valorServico + k.valorMaterial);
                                    k.importadaCustoFixo = "S";

                                    dc.tb_os.AddOrUpdate(k);
                                    dc.SaveChanges();
                                }
                                else
                                {
                                    throw new ArgumentException("Execption - Baixa no Estoque");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Execption - tb_os_itens = Null");
                            }

                            //var s = dc.tb_orcamento.FirstOrDefault(a => a.autonumero == autonumeroOrcamento && a.cancelado != "S");
                            //if (s != null)
                            //{
                            //    s.codigoOs = codigoOs;
                            //    dc.tb_orcamento.AddOrUpdate(s);
                            //    dc.SaveChanges();
                            //}

                            transaction.Commit();
                            return totalItens.ToString("########0.00");

                        }
                        else
                        {
                            throw new ArgumentException("Execption - tb_planilhafechada = Null");
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
        public string AssociarItensSolicitacaoServico()
        {
            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {

                        var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);
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

                        //totalPF = Math.Truncate(totalPF * 100) / 100;
                        totalPF = (Math.Truncate((decimal)quantidadePF * (decimal)precoUnitarioPF * 100)) / 100;

                        var k = dc.tb_os_itens.FirstOrDefault(a => a.autonumero == autonumero && a.cancelado != "S");
                        if (k != null)
                        {

                            k.codigoPF = codigoPlanilhaFechada;
                            k.nomePF = nomePF;
                            k.quantidadePF = quantidadePF;
                            k.precoUnitarioPF = precoUnitarioPF;
                            k.totalPF = totalPF;
                            k.nomeFontePF = nomeFontePF;
                            k.itemPF = itemPF;
                            k.unidadePF = unidadePF;

                            dc.tb_os_itens.AddOrUpdate(k);
                            dc.SaveChanges();

                            // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------

                            var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == codigoPlanilhaFechada && a.autonumeroCliente == autonumeroCliente);
                            if (planilhaFechada != null)
                            {
                                var estoque = planilhaFechada.estoque - quantidadePF;
                                if (estoque < 0) throw new ArgumentException("Execption# planilhaFechada.estoque: " + codigoPlanilhaFechada + " Estoque: " + planilhaFechada.estoque.ToString() + " - quantidade: " + quantidadePF.ToString() + "  < 0");

                                planilhaFechada.estoque = estoque;
                                planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada + quantidadePF;

                                dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                dc.SaveChanges();
                            }

                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------


                            transaction.Commit();
                            return "";
                        }
                        else
                        {
                            throw new ArgumentException("Execption - Calcular valor itens");
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
        public string DesassociarItensSolicitacaoServico()
        {
            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {

                        var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);
                        var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

                        var k = dc.tb_os_itens.FirstOrDefault(a => a.autonumero == autonumero && a.cancelado != "S");
                        if (k != null)
                        {

                            var quantidadePF = k.quantidadePF;

                            var nomeFontePF = k.nomeFontePF;
                            var codigoPF = k.codigoPF;

                            k.codigoPF = "";
                            k.nomePF = "";
                            k.quantidadePF = 0;
                            k.precoUnitarioPF = 0;
                            k.totalPF = 0;
                            k.nomeFontePF = "";
                            k.itemPF = 0;
                            k.unidadePF = "";

                            dc.tb_os_itens.AddOrUpdate(k);
                            dc.SaveChanges();

                            if (nomeFontePF.Contains("MANUT") || nomeFontePF.Contains("EQUIPE"))
                            {
                                var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == codigoPF && a.autonumeroCliente == autonumeroCliente);
                                if (planilhaFechada != null)
                                {
                                    planilhaFechada.estoque = planilhaFechada.estoque + (decimal)quantidadePF;
                                    planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada - (decimal)quantidadePF;
                                    dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);
                                    dc.SaveChanges();
                                }
                                else
                                {
                                    throw new ArgumentException("Execption# planilhaFechada Não encontrada: " + k.codigoPF);

                                }
                            }

                            transaction.Commit();
                            return "";
                        }
                        else
                        {
                            throw new ArgumentException("Execption - Calcular valor itens");
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


    }

}
