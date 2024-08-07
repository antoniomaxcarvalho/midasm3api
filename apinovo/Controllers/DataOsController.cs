using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class resumoPlanilha
    {
        public Int64? autonumeroSistema { get; set; }
        public string nomeSistema { get; set; }
        public decimal quantidadeSS { get; set; }
        public decimal totalPF { get; set; }
        public string nomeServico { get; set; }
        public Int32? autonumeroServico { get; set; }
        public string codigoOs { get; set; }
        //public string siglaCliente { get; set; }
    }
    public class itensPlanilha
    {
        public Int64 autonumero { get; set; }
        public string codigoPF { get; set; }
        public string nome { get; set; }
        public string unidade { get; set; }
        public decimal preco { get; set; }
        public decimal quantidadePF { get; set; }
        public decimal totalPF { get; set; }
        public decimal total { get; set; }
        public Int16 qtdeMaoObra { get; set; }


        //public decimal precoUnitarioPF { get; set; }
    }

    public class DataOsController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_os> GetOs(string codigoOs, string clientesDoUsuario)
        {
            AcertarValoresNaOs(codigoOs, 0);

            using (var dc = new manutEntities())
            {
                var cod = codigoOs.Trim();
                var user = from p in dc.tb_os.Where((a => a.codigoOs.Contains(cod) && clientesDoUsuario.Contains(a.siglaCliente))) select p;
                return user.ToList(); ;
            }

        }
        [HttpGet]
        public IEnumerable<tb_os> GetOsAutonumero(Int64 autonumero)
        {
            AcertarValoresNaOs(string.Empty, autonumero);
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_os.Where((a => (a.autonumero == autonumero))) select p;
                return user.ToList(); ;
            }
        }

        //public IEnumerable<tb_os> GetAllOs()
        //{

        //    using (var dc = new manutEntities())
        //    {
        //        var user = from p in dc.tb_os.Where((a => (a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
        //        return user.ToList(); ;
        //    }

        //}


        public IEnumerable<tb_os> GetAllOs(string clientesDoUsuario)
        {
            IQueryable<tb_os> user;

            using (var dc = new manutEntities())
            {
                if (!string.IsNullOrEmpty(clientesDoUsuario))
                {
                    user = from p in dc.tb_os.Where((a => (a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null) && clientesDoUsuario.Contains(a.siglaCliente)))).OrderByDescending(p => p.autonumero) select p;
                }
                else
                {
                    user = from p in dc.tb_os.Where(a => (a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null))).OrderByDescending(p => p.autonumero) select p;
                }
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarOs()
        {
            //var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        //Debug.WriteLine("autonumero" + autonumero.ToString());
                        var linha = dc.tb_os.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {
                            linha.cancelado = "S";
                            dc.tb_os.AddOrUpdate(linha);

                            dc.SaveChanges();

                            // Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------
                            var cliente = dc.tb_cliente.FirstOrDefault(a => a.autonumero == linha.autonumeroCliente);
                            if (cliente != null)
                            {

                                dc.tb_os_itens.Where(x => x.autonumeroOs == autonumero && x.cancelado == "N" && (x.nomeFonte.Contains("MANUT") || x.nomeFonte.Contains("EQUIPE"))).ToList().ForEach(x =>
                                {
                                    var planilhaFechada = dc.tb_planilhafechada.FirstOrDefault(a => a.codigo == x.codigoInsumoServico && a.autonumeroCliente == cliente.autonumero);
                                    if (planilhaFechada != null)
                                    {
                                        planilhaFechada.estoque = planilhaFechada.estoque + (decimal)x.quantidade;
                                        planilhaFechada.qtdeUsada = planilhaFechada.qtdeUsada - (decimal)x.quantidade;

                                        //Debug.WriteLine("planilhaFechada.estoque" + planilhaFechada.estoque.ToString());
                                        dc.tb_planilhafechada.AddOrUpdate(planilhaFechada);

                                        dc.SaveChanges();
                                    }
                                });

                            }
                            // FIM - Testar se Cliente tem Planilha Fechada - Dar Entrada Estoque -----------------------------------------------



                            dc.tb_os_itens.Where(x => x.autonumeroOs == autonumero).ToList().ForEach(x =>
                            {

                                //Debug.WriteLine("dc.tb_os_itens");
                                x.cancelado = "S";
                            });
                            dc.SaveChanges();

                            //var comando = "UPDATE manut.tb_os_itens SET cancelado = 'S' WHERE autonumeroOs = " + autonumero.ToString("########0");
                            //dc.Database.ExecuteSqlCommand(comando);

                            dc.tb_os_acompanhamento.Where(x => x.autonumeroOs == autonumero).ToList().ForEach(x =>
                            {
                                //    Debug.WriteLine("dc.tb_os_acompanhamento");
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

        //[HttpPost]
        //public string[] IncluirOs()
        //{
        //    var sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
        //    var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

        //    var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
        //    var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
        //    var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();

        //    var dataSolicitacao = DateTime.Now;
        //    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataSolicitacao"].ToString()))
        //    {
        //        dataSolicitacao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataSolicitacao"].ToString());
        //    }
        //    if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
        //    {
        //        if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
        //        {
        //            var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
        //            if (dataSolicitacao > dataLimite)
        //            {
        //                dataSolicitacao = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
        //                                                     DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
        //            }
        //        }
        //    }


        //    using (var dc = new manutEntities())
        //    {
        //        using (var transaction = dc.Database.BeginTransaction())
        //        {

        //            try
        //            {


        //                var Os = new tb_os
        //                {

        //                    codigoOs = "",
        //                    autonumeroCliente = autonumeroCliente,
        //                    nomeCliente = nomeCliente,
        //                    autonumeroSistema = 0,
        //                    nomeSistema = "",
        //                    autonumeroSubSistema = 0,
        //                    nomeSubSistema = "",
        //                    autonumeroPredio = 0,
        //                    nomePredio = "",
        //                    autonumeroSetor = 0,
        //                    nomeSetor = "",
        //                    autonumeroLocalFisico = 0,
        //                    nomeLocalFisico = "",
        //                    autonumeroPrioridade = 0,
        //                    nomePrioridade = "",
        //                    autonumeroUsuario = autonumeroUsuario,
        //                    nomeUsuario = nomeUsuario,
        //                    autonumeroServico = 0,
        //                    nomeServico = "",
        //                    complemento = "",
        //                    ramal = "",
        //                    dataSolicitacao = dataSolicitacao,
        //                    //dataInicio date DEFAULT NULL,
        //                    //dataTermino date DEFAULT NULL,
        //                    //nomeTarefa = "",
        //                    descricao = "",
        //                    prazoPrioridade = "",
        //                    nomeStatus = "Aberta",
        //                    cancelado = "N",
        //                    valor = 0,
        //                    siglaCliente = sigla,
        //                    //autonumeroEquipe = 0,
        //                    nomeEquipe = "",
        //                    url = "",
        //                    url1 = "",
        //                    codigoEquipamento = "",
        //                    totalHoras = TimeSpan.Parse("00:00")
        //                };

        //                dc.tb_os.Add(Os);
        //                dc.SaveChanges();
        //                var autoOsNova = Convert.ToInt32(Os.autonumero);


        //                var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
        //                if (linha != null)
        //                {
        //                    linha.contadorOs = linha.contadorOs + 1;

        //                    dc.tb_cliente.AddOrUpdate(linha);
        //                    dc.SaveChanges();

        //                    var contadorOsCliente = Convert.ToInt32(linha.contadorOs);
        //                    var codigo = sigla + "-" + contadorOsCliente.ToString("00000") + "-" + DateTime.Now.ToString("yyyy");
        //                    Os.codigoOs = codigo;

        //                    dc.tb_os.AddOrUpdate(Os);
        //                    dc.SaveChanges();

        //                    transaction.Commit();

        //                    return new string[] { autoOsNova.ToString("#######0"), codigo, Os.dataSolicitacao.ToString() };

        //                }
        //                else
        //                {
        //                    throw new ArgumentException("Execption");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //        }
        //        return null;
        //    }

        //}

        [HttpPost]
        public tb_os IncluirOs()
        {
            var sigla = HttpContext.Current.Request.Form["sigla"].ToString();
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());

            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
            //var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
            var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString();

            var dataSolicitacao = DateTime.Now;
            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataSolicitacao"].ToString()))
            {
                dataSolicitacao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataSolicitacao"].ToString());
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
            {
                if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                {
                    var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                    if (dataSolicitacao > dataLimite)
                    {
                        dataSolicitacao = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                    }
                }
            }

            using (var dc = new manutEntities())
            {
                using (var transaction = dc.Database.BeginTransaction())
                {

                        var Os = new tb_os
                    {

                        codigoOs = "",
                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        autonumeroSistema = 0,
                        nomeSistema = "",
                        autonumeroSubSistema = 0,
                        nomeSubSistema = "",
                        autonumeroPredio = 0,
                        nomePredio = "",
                        autonumeroSetor = 0,
                        nomeSetor = "",
                        autonumeroLocalFisico = 0,
                        nomeLocalFisico = "",
                        autonumeroPrioridade = 0,
                        nomePrioridade = "",
                        //autonumeroUsuario = autonumeroUsuario,
                        nomeUsuario = nomeUsuario,
                        autonumeroServico = 0,
                        nomeServico = "",
                        complemento = "",
                        ramal = "",
                        dataSolicitacao = dataSolicitacao,
                        //dataInicio date DEFAULT NULL,
                        //dataTermino date DEFAULT NULL,
                        //nomeTarefa = "",
                        descricao = "",
                        prazoPrioridade = "",
                        nomeStatus = "Aberta",
                        cancelado = "N",
                        valor = 0,
                        //siglaCliente = sigla,
                        //autonumeroEquipe = 0,
                        nomeEquipe = "",
                        url = "",
                        url1 = "",
                        codigoEquipamento = 0,
                        totalHoras = TimeSpan.Parse("00:00"),
                        valorMaterial = 0,
                        valorServico = 0,
                        matricula = "",
                        codigoTabela = "",
                        importadaCustoFixo = "N",
                        codigoOrdemServico = "",
                        etapa = "",
                        medicao = "",
                        localAtendido = "",
                        bdiServico = 0,
                        autonumeroAutorizado = 0,
                        autonumeroUsuario = 0,
                        desabilitado = 0,

                    };

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
                    {
                        Os.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (Os.dataInicio > dataLimite)
                                {
                                    Os.dataInicio = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }
                    }



                    Os.autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString().Trim());
                    Os.autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    Os.autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                    Os.autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                    Os.autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                    Os.autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                    Os.autonumeroLocalFisico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                    Os.autonumeroPrioridade = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPrioridade"].ToString());
                    Os.autonumeroServico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroServico"].ToString());



                    Os.nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString();
                    Os.nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString();
                    Os.nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString();
                    Os.nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString();
                    Os.nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString();
                    Os.nomePrioridade = HttpContext.Current.Request.Form["nomePrioridade"].ToString();
                    Os.nomeServico = HttpContext.Current.Request.Form["nomeServico"].ToString();
                    Os.complemento = HttpContext.Current.Request.Form["complemento"].ToString();
                    Os.ramal = HttpContext.Current.Request.Form["ramal"].ToString();
                    Os.valor = Convert.ToDecimal(HttpContext.Current.Request.Form["valor"].ToString());
                    Os.descricao = HttpContext.Current.Request.Form["descricao"].ToString();
                    Os.prazoPrioridade = HttpContext.Current.Request.Form["prazoPrioridade"].ToString();
                    Os.nomeStatus = HttpContext.Current.Request.Form["nomeStatus"].ToString();
                    Os.bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString());
                    Os.bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString());
                    Os.siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString();
                    Os.localAtendido = HttpContext.Current.Request.Form["localAtendido"].ToString();

                    var nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString();

                    var linhaCliente = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linhaCliente == null && linhaCliente.cancelado != "S")
                    {
                        throw new ArgumentException("Erro: Cliente Não Encontrado");
                    }

                    Os.bdiMaterial = linhaCliente.bdiMaterial;
                    Os.bdiServico = linhaCliente.bdiServico;

                    if (nomeSubSistema.ToUpper().Contains("MÃO DE") || nomeSubSistema.ToUpper().Contains("MAO DE"))
                    {
                        Os.bdiMaterial = 0;
                        Os.bdiServico = 0;
                    }

                    if (string.IsNullOrEmpty(Os.nomeStatus))
                    {
                        Os.nomeStatus = "Aberta";
                    }

                    Os.codigoEquipamento = Convert.ToInt64(HttpContext.Current.Request.Form["codigoEquipamento"].ToString());


                    dc.tb_os.Add(Os);
                    dc.SaveChanges();
                    var autoOsNova = Convert.ToInt32(Os.autonumero);


                    var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linha != null)
                    {
                        linha.contadorOs = linha.contadorOs + 1;

                        dc.tb_cliente.AddOrUpdate(linha);
                        dc.SaveChanges();

                        var contadorOsCliente = Convert.ToInt32(linha.contadorOs);
                        var codigo = sigla + "-" + contadorOsCliente.ToString("00000") + "-" + DateTime.Now.ToString("yyyy");
                        Os.codigoOs = codigo;

                        dc.tb_os.AddOrUpdate(Os);
                        dc.SaveChanges();

                        transaction.Commit();

                        return Os;

                    }
                    transaction.Rollback();

                }
                return null;
            }

        }

        [HttpPost]
        public void AtualizarOs()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);



                var linha = dc.tb_os.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.codigoOs = HttpContext.Current.Request.Form["codigoOs"].ToString().Trim();
                    linha.autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                    linha.nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                    linha.autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                    linha.nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
                    linha.autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                    linha.nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString().Trim();
                    linha.autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                    linha.nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
                    linha.autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                    linha.nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString().Trim();
                    linha.autonumeroLocalFisico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                    linha.nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString().Trim();
                    linha.autonumeroPrioridade = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPrioridade"].ToString());
                    linha.nomePrioridade = HttpContext.Current.Request.Form["nomePrioridade"].ToString().Trim();
                    linha.autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                    linha.nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                    linha.autonumeroServico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroServico"].ToString());
                    linha.nomeServico = HttpContext.Current.Request.Form["nomeServico"].ToString().Trim();
                    linha.complemento = HttpContext.Current.Request.Form["complemento"].ToString().Trim();
                    linha.ramal = HttpContext.Current.Request.Form["ramal"].ToString().Trim();
                    linha.valor = Convert.ToDecimal(HttpContext.Current.Request.Form["valor"].ToString());
                    linha.localAtendido = HttpContext.Current.Request.Form["localAtendido"].ToString().Trim();

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataSolicitacao"].ToString()))
                    {
                        linha.dataSolicitacao = Convert.ToDateTime(HttpContext.Current.Request.Form["dataSolicitacao"].ToString());
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (linha.dataSolicitacao > dataLimite)
                                {
                                    linha.dataSolicitacao = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }

                    }

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
                    {
                        linha.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (linha.dataInicio > dataLimite)
                                {
                                    linha.dataInicio = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }
                    }

                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataTermino"].ToString()))
                    {
                        linha.dataTermino = Convert.ToDateTime(HttpContext.Current.Request.Form["dataTermino"].ToString());
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (linha.dataTermino > dataLimite)
                                {
                                    linha.dataTermino = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (linha.nomeStatus == "Fechada")
                        {
                            //linha.dataTermino = DateTime.Now;
                        }
                    }

                    linha.descricao = HttpContext.Current.Request.Form["descricao"].ToString().Trim();
                    linha.prazoPrioridade = HttpContext.Current.Request.Form["prazoPrioridade"].ToString().Trim();
                    linha.nomeStatus = HttpContext.Current.Request.Form["nomeStatus"].ToString().Trim();



                    var linhaCliente = dc.tb_cliente.Find(linha.autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linhaCliente == null && linhaCliente.cancelado != "S")
                    {
                        throw new ArgumentException("Erro: Cliente Não Encontrado");
                    }

                    linha.bdiMaterial = linhaCliente.bdiMaterial;
                    linha.bdiServico = linhaCliente.bdiServico;

                    if (linha.nomeSubSistema.ToUpper().Contains("MÃO DE") || linha.nomeSubSistema.ToUpper().Contains("MAO DE"))
                    {
                        linha.bdiMaterial = 0;
                        linha.bdiServico = 0;
                    }


                    //linha.bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString());
                    //linha.bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString());
                    linha.siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString().Trim();
                    //linha.autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString());
                    linha.nomeEquipe = HttpContext.Current.Request.Form["nomeEquipe"].ToString().Trim();
                    linha.localAtendido = HttpContext.Current.Request.Form["localAtendido"].ToString().Trim();

                    if (string.IsNullOrEmpty(linha.nomeStatus))
                    {
                        linha.nomeStatus = "Aberta";
                    }

                    if (linha.nomeStatus == "Aberta")
                    {
                        linha.dataTermino = null;
                        linha.dataInicio = null;
                    }

                    linha.codigoEquipamento = Convert.ToInt64(HttpContext.Current.Request.Form["codigoEquipamento"].ToString());

                    if (linha.desabilitado == null)
                    {
                        linha.desabilitado = 0;
                    }

                    dc.tb_os.AddOrUpdate(linha);
                    dc.SaveChanges();

                }
            }

        }
        [HttpPost]
        public void AtualizarStatusOs()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                //var varcccv = HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite");
                //Debug.WriteLine(varcccv);
                //foreach (string key in (HttpContext.Current.Request.Form.AllKeys))
                //{
                //    Debug.WriteLine("{0} - {1}", key, HttpContext.Current.Request.Form[key]);


                var linha = dc.tb_os.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.nomeStatus = HttpContext.Current.Request.Form["nomeStatus"].ToString().Trim();
                    if (string.IsNullOrEmpty(linha.nomeStatus))
                    {
                        linha.nomeStatus = "Aberta";
                    }

                    if (linha.nomeStatus == "Fechada" && !DataClienteController.IsDate(linha.dataTermino.ToString()))
                    {
                        linha.dataTermino = DateTime.Now;
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                        {
                            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                            {
                                var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                if (linha.dataTermino > dataLimite)
                                {
                                    linha.dataTermino = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                }
                            }
                        }

                    }

                    if (linha.desabilitado == null)
                    {
                        linha.desabilitado = 0;
                    }

                    dc.tb_os.AddOrUpdate(linha);
                    dc.SaveChanges();

                }
            }

        }

        [HttpGet]
        public Array SomatorioOsFechada(string data1, string data2, string clientesDoUsuario, string tipoVencto)
        {
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {

                var valorTotal = 0m;
                var qtdeRegistros = 0;

                if (!tipoVencto.Equals("E")) // Data Emissao ou Fechamento
                {
                    // fechamento
                    dc.tb_os.Where(x => (clientesDoUsuario.Contains(x.siglaCliente) && x.dataTermino >= data11 &&
                       x.dataTermino <= data22 && x.nomeStatus == "Fechada" && x.cancelado != "S")).ToList().ForEach(x =>
                       {
                           qtdeRegistros++;
                           valorTotal = valorTotal + Convert.ToDecimal(x.valor);
                       });
                }
                else
                {
                    // emissao
                    dc.tb_os.Where(x => (clientesDoUsuario.Contains(x.siglaCliente) && x.dataInicio >= data11 &&
                       x.dataInicio <= data22 && x.nomeStatus == "Fechada" && x.cancelado != "S")).ToList().ForEach(x =>
                       {
                           qtdeRegistros++;
                           valorTotal = valorTotal + Convert.ToDecimal(x.valor);
                       });
                }


                return new[] { valorTotal, qtdeRegistros };
            }

        }

        [HttpPost]
        public void OsFechadaParaMedida()
        {
            var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString());
            var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());
            var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var tipoVencto = HttpContext.Current.Request.Form["tipoVencto"].ToString();

            using (var dc = new manutEntities())
            {
                if (!tipoVencto.Equals("E")) // Data Emissao ou Fechamento
                {
                    // fechamento
                    dc.tb_os.Where(x => (x.autonumeroCliente == autonumeroCliente && x.dataTermino >= data1 &&
                        x.dataTermino <= data2 && x.nomeStatus == "Fechada" && x.cancelado != "S")).ToList().ForEach(x =>
                        {
                            x.nomeStatus = "O.S. Medida";
                        });
                }
                else
                {
                    // Emissao
                    dc.tb_os.Where(x => (x.autonumeroCliente == autonumeroCliente && x.dataInicio >= data1 &&
                        x.dataInicio <= data2 && x.nomeStatus == "Fechada" && x.cancelado != "S")).ToList().ForEach(x =>
                        {
                            x.nomeStatus = "O.S. Medida";
                        });

                }

                dc.SaveChanges();

            }

        }

        [HttpPost]
        public string[] CopiarOs()
        {
            using (var dc = new manutEntities())
            {

                using (var transaction = dc.Database.BeginTransaction())
                {
                    Int64 autoOsNova = 0;
                    string codigo = string.Empty;

                    try
                    {
                        var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                        //var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
                        //var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();

                        var linha = dc.tb_os.Find(autonumero); // sempre irá procurar pela chave primaria
                        if (linha != null && linha.cancelado != "S")
                        {

                            var Os = new tb_os();
                            dc.tb_os.Add(Os);
                            dc.SaveChanges();
                            autoOsNova = Convert.ToInt32(Os.autonumero);


                            var linha2 = dc.tb_cliente.Find(linha.autonumeroCliente); // sempre irá procurar pela chave primaria
                            if (linha2 != null)
                            {
                                linha2.contadorOs = linha2.contadorOs + 1;

                                dc.tb_cliente.AddOrUpdate(linha2);
                                dc.SaveChanges();


                                var dataSolicitacao = DateTime.Now;
                                if (HttpContext.Current.Request.Form.AllKeys.Contains("dataLimite"))
                                {
                                    if (DataClienteController.IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                                    {
                                        var dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                                        if (dataSolicitacao > dataLimite)
                                        {
                                            dataSolicitacao = new DateTime(dataLimite.Year, dataLimite.Month, dataLimite.Day, DateTime.Now.TimeOfDay.Hours,
                                                             DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
                                        }
                                    }
                                }

                                var contadorOsCliente = Convert.ToInt32(linha2.contadorOs);
                                codigo = linha.siglaCliente + "-" + contadorOsCliente.ToString("00000") + "-" + DateTime.Now.ToString("yyyy");

                                Os.codigoOs = codigo;
                                Os.autonumeroCliente = linha.autonumeroCliente;
                                Os.nomeCliente = linha.nomeCliente;
                                Os.autonumeroSistema = linha.autonumeroSistema;
                                Os.nomeSistema = linha.nomeSistema;
                                Os.autonumeroSubSistema = linha.autonumeroSubSistema;
                                Os.nomeSubSistema = linha.nomeSubSistema;
                                Os.autonumeroPredio = linha.autonumeroPredio;
                                Os.nomePredio = linha.nomePredio;
                                Os.autonumeroSetor = linha.autonumeroSetor;
                                Os.nomeSetor = linha.nomeSetor;
                                Os.autonumeroLocalFisico = linha.autonumeroLocalFisico;
                                Os.nomeLocalFisico = linha.nomeLocalFisico;
                                Os.autonumeroPrioridade = linha.autonumeroPrioridade;
                                Os.nomePrioridade = linha.nomePrioridade;
                                Os.autonumeroUsuario = linha.autonumeroUsuario;
                                Os.nomeUsuario = linha.nomeUsuario;
                                Os.autonumeroServico = linha.autonumeroServico;
                                Os.nomeServico = linha.nomeServico;
                                Os.complemento = linha.complemento;
                                Os.ramal = linha.ramal;
                                Os.dataSolicitacao = dataSolicitacao;
                                Os.descricao = linha.descricao;
                                Os.prazoPrioridade = linha.prazoPrioridade;
                                Os.nomeStatus = "Aberta";
                                Os.cancelado = linha.cancelado;
                                Os.valor = linha.valor;
                                Os.siglaCliente = linha.siglaCliente;
                                Os.dataInicio = dataSolicitacao;
                                Os.nomeEquipe = linha.nomeEquipe;
                                Os.bdiMaterial = linha.bdiMaterial;
                                Os.bdiServico = linha.bdiServico;
                                Os.valorServico = 0;
                                Os.valorMaterial = linha.valorMaterial;
                                Os.codigoOrdemServico = ""; ;
                                Os.etapa = "";
                                Os.medicao = "";
                                Os.localAtendido = linha.localAtendido;
                                Os.desabilitado = 0;

                                dc.tb_os.AddOrUpdate(Os);
                                dc.SaveChanges();

                                var itens = new DataOsItemController();
                                var p = itens.GetItensOsAutonumero(autonumero);
                                if (p != null)
                                {
                                    foreach (var value in p)
                                    {
                                        value.autonumeroOs = autoOsNova;
                                        value.codigoOs = codigo;
                                        value.autonumeroUsuario = linha.autonumeroUsuario;
                                        value.nomeUsuario = linha.nomeUsuario;
                                        value.cancelado = "N";
                                        value.data = dataSolicitacao;
                                        value.qtdeMaoObra = 0;

                                        dc.tb_os_itens.Add(value);
                                        dc.SaveChanges();

                                    }

                                }
                            }

                        }

                        transaction.Commit();
                        return new string[] { autoOsNova.ToString("#######0"), codigo, DateTime.Now.ToString() };

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }


                return null;

            }

        }

        [HttpPost]
        public string UploadFile()
        {

            var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();

            try
            {

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                    var antesDepois = HttpContext.Current.Request.Form["antesDepois"].ToString().Trim();

                    var caminho = "~/UploadedFiles/";

                    if (httpPostedFile == null)
                    {
                        message = "Erro Upload 1";
                        return message;
                    }

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);



                    // Atualizar no BD ( tabela produtos )
                    using (var dc = new manutEntities())
                    {
                        var os = dc.tb_os.FirstOrDefault(a => a.autonumero == autonumero);


                        if (os != null)
                        {
                            if (os.url == null) os.url = "";
                            if (os.url1 == null) os.url1 = "";

                            if (antesDepois == "A")
                            {
                                os.url = fileName.Trim();
                            }
                            else
                            {
                                os.url1 = fileName.Trim();
                            }

                            dc.tb_os.AddOrUpdate(os);

                            dc.SaveChanges();
                            message = fileName;

                            return message;
                        }

                        message = "Erro Upload 2";
                        return message;
                    }

                }

                return message;


            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName,
                            ve.ErrorMessage));
                    }
                }
                message = sb.ToString();
                //throw new DbEntityValidationException(sb.ToString(), e);
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130) + " - DataprodutoController SaveFilesFoto"
                    : ex.Message + " - DataprodutoController SaveFilesFoto";

            }

            return message;
        }

        public static string AcertarValoresNaOs(string codigoOs, Int64 autonumero)
        {

            using (var dc = new manutEntities())
            {
                var c = 1;

                List<tb_os> j = null;


                if (autonumero > 0)
                {

                    var alterou = false;
                    var p = dc.tb_os_itens.Where(a => a.autonumeroOs == autonumero && a.cancelado != "S").ToList();

                    if (p != null)
                    {
                        foreach (var valuex in p)
                        {

                            var tot = (Math.Truncate((decimal)valuex.quantidade * (decimal)valuex.precoUnitario * 100)) / 100;
                            var totPlanilha = (Math.Truncate((decimal)valuex.quantidadePF * (decimal)valuex.precoUnitarioPF * 100)) / 100;

                            if (valuex.total != tot || valuex.totalPF != totPlanilha)
                            {
                                alterou = true;
                                valuex.total = tot;
                                valuex.totalPF = totPlanilha;
                                dc.tb_os_itens.AddOrUpdate(valuex);
                                dc.SaveChanges();
                            }

                        }

                        if (alterou)
                        {
                            var totalItens = dc.tb_os_itens.Where(a => a.autonumeroOs == autonumero && a.cancelado != "S").Sum(k => k.total);

                            dc.tb_os.Where(a => a.cancelado != "S" && a.autonumero == autonumero).ToList().ForEach(x =>
                            {
                                x.valor = totalItens;
                            });
                            dc.SaveChanges();

                        }
                    }
                }

                if (string.IsNullOrEmpty(codigoOs) && autonumero == 0)
                {
                    j = dc.tb_os.Where(a => a.valor != (a.valorMaterial + a.valorServico) && a.cancelado != "S").ToList();
                }
                else
                {

                    if (!string.IsNullOrEmpty(codigoOs))
                    {
                        j = dc.tb_os.Where(a => a.valor != (a.valorMaterial + a.valorServico) && a.cancelado != "S" && a.codigoOs == codigoOs).ToList();
                    }

                    if (autonumero > 0)
                    {
                        j = dc.tb_os.Where(a => a.valor != (a.valorMaterial + a.valorServico) && a.cancelado != "S" && a.autonumero == autonumero).ToList();
                    }
                }



                if (j != null)
                {

                    foreach (var valueJ in j)
                    {
                        decimal totalItensServico = 0m;
                        decimal totalItensMaterial = 0m;


                        var auto = Convert.ToInt32(valueJ.autonumero);
                        var p = dc.tb_os_itens.Where(a => a.autonumeroOs == auto && a.cancelado != "S").ToList();

                        if (p != null)
                        {
                            foreach (var valuex in p)
                            {
                                if (valuex.servico.Equals("S"))
                                {
                                    totalItensServico = Convert.ToDecimal(valuex.total) + totalItensServico;
                                }
                                else
                                {
                                    totalItensMaterial = Convert.ToDecimal(valuex.total) + totalItensMaterial;
                                }

                            }

                            valueJ.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                            valueJ.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                            valueJ.valor = Convert.ToDecimal((valueJ.valorServico + valueJ.valorMaterial));

                            if (valueJ.desabilitado == null)
                            {
                                valueJ.desabilitado = 0;
                            }
                            //Debug.WriteLine("OS" + valueJ.codigoOs + " - valorServico " + valueJ.valorServico.ToString() + " - valorMaterial " + valueJ.valorMaterial.ToString() + " - Valor " + valueJ.valor.ToString());
                            //var ccc = valueJ.valorMaterial;
                            dc.tb_os.AddOrUpdate(valueJ);
                            dc.SaveChanges();
                        }
                    }

                }
                return string.Empty;

            }

        }

        [HttpGet]
        public IEnumerable<tb_os> GetAllOsData(string clientesDoUsuario, string data1, string data2)
        {
            var c = 1;
            IQueryable<tb_os> user;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                if (!string.IsNullOrEmpty(clientesDoUsuario))
                {
                    if (clientesDoUsuario.IndexOf("0") == -1)  // Não exite o Nro Zero na String
                    {

                        user = from p in dc.tb_os.Where((a => a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null)
                        && ((a.dataSolicitacao >= data11 && a.dataSolicitacao <= data22) ||
                        (a.dataSolicitacao <= data11 && a.nomeStatus != "Fechada" && a.nomeStatus != "O.S. Medida"))
                        )).OrderByDescending(p => p.autonumero)
                               select p;
                    }
                    else
                    {

                        user = from p in dc.tb_os.Where((a => a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null)
                               && clientesDoUsuario.Contains(a.siglaCliente)
                          && ((a.dataSolicitacao >= data11 && a.dataSolicitacao <= data22) ||
                          (a.dataSolicitacao <= data11 && a.nomeStatus != "Fechada" && a.nomeStatus != "O.S. Medida"))
                          )).OrderByDescending(p => p.autonumero)
                               select p;

                        var k = user.ToList().Where(p => clientesDoUsuario.Contains(p.autonumeroCliente.ToString().PadLeft(4, '0')));
                        return k;
                    }
                }
                else
                {
                    user = from p in dc.tb_os.Where((a => a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null)
                        && ((a.dataSolicitacao >= data11 && a.dataSolicitacao <= data22) ||
                        (a.dataSolicitacao <= data11 && a.nomeStatus != "Fechada" && a.nomeStatus != "O.S. Medida"))
                        )).OrderByDescending(p => p.autonumero)
                           select p;
                }
                return user.ToList();
            }
        }

        [HttpGet]
        public IEnumerable<tb_os> GetAllOsDataCliente(long autonumeroCliente, string data1, string data2)
        {
            var c = 1;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                return (from p in dc.tb_os.Where(a => a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null)
                       && a.autonumeroCliente == autonumeroCliente && a.dataSolicitacao >= data11 && a.dataSolicitacao <= data22).OrderByDescending(p => p.autonumero)
                        select p).ToList();
            }
        }


        [HttpGet]
        public IEnumerable<tb_os_itens> GetAllOsSistemaCompatibildade(Int64 autonumeroCliente, string data1, string data2, Int32 autonumeroSistema, Int32 autonumeroServico)
        {
            var c = 1;
            IQueryable<tb_os_itens> itensForaDaPlanilha;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                if (autonumeroServico == 0)
                {
                    itensForaDaPlanilha = (from a in dc.tb_os
                                           join b in dc.tb_os_itens on a.autonumero equals b.autonumeroOs
                                           where a.cancelado != "S" && b.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null) && b.autonumeroCliente == autonumeroCliente
                                             && a.dataTermino >= data11 && a.dataTermino <= data22 &&
                                             a.nomeStatus == "Fechada" && a.autonumeroSistema == autonumeroSistema && !b.nomeFonte.Contains("MANUT") && !b.nomeFonte.Contains("EQUIPE")
                                           select b).OrderBy(p => p.autonumeroOs);
                }
                else
                {
                    itensForaDaPlanilha = (from a in dc.tb_os
                                           join b in dc.tb_os_itens on a.autonumero equals b.autonumeroOs
                                           where a.cancelado != "S" && b.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null) && b.autonumeroCliente == autonumeroCliente
                                             && a.dataTermino >= data11 && a.dataTermino <= data22 &&
                                             a.nomeStatus == "Fechada" && a.autonumeroSistema == autonumeroSistema && !b.nomeFonte.Contains("MANUT") && !b.nomeFonte.Contains("EQUIPE") && a.autonumeroServico == autonumeroServico
                                           select b).OrderBy(p => p.autonumeroOs);


                }
                var k = itensForaDaPlanilha.ToList();
                return k;
            }
        }


        [HttpGet]
        public IEnumerable<tb_os_itens> GetAllOsSistema(Int64 autonumeroCliente, string data1, string data2, Int32 autonumeroSistema, Int32 autonumeroServico)
        {

            var c = 1;
            //IQueryable<tb_os_itens> itensForaDaPlanilha;
            //IQueryable<tb_os_itens> itensDaPlanilha;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {


                if (autonumeroServico == 0)
                {

                    var resposta = (from a in dc.tb_os
                                    join b in dc.tb_os_itens on a.autonumero equals b.autonumeroOs
                                    where a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null) && b.cancelado != "S" && b.autonumeroCliente == autonumeroCliente
                                      && a.dataTermino >= data11 && a.dataTermino <= data22 &&
                                      a.nomeStatus == "Fechada" && a.autonumeroSistema == autonumeroSistema
                                    select b).OrderBy(p => p.autonumeroOs);
                    var k = resposta.ToList();
                    return k;
                }
                else
                {
                    var resposta = (from a in dc.tb_os
                                    join b in dc.tb_os_itens on a.autonumero equals b.autonumeroOs
                                    where a.cancelado != "S" && (a.desabilitado == 0 || a.desabilitado == null) && b.cancelado != "S" && b.autonumeroCliente == autonumeroCliente
                                      && a.dataTermino >= data11 && a.dataTermino <= data22 &&
                                      a.nomeStatus == "Fechada" && a.autonumeroSistema == autonumeroSistema && a.autonumeroServico == autonumeroServico
                                    select b).OrderBy(p => p.autonumeroOs);
                    var k = resposta.ToList();
                    return k;
                }


            }
        }


        [HttpPost]
        public string AcertarValoresOs()
        {

            using (var dc = new manutEntities())
            {
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var data1 = Convert.ToDateTime(HttpContext.Current.Request.Form["data1"].ToString());
                var data2 = Convert.ToDateTime(HttpContext.Current.Request.Form["data2"].ToString());

                var j = dc.tb_os.Where(l => l.autonumeroCliente == autonumeroCliente && l.cancelado != "S" && l.dataTermino >= data1 &&
                       l.dataTermino <= data2).ToList();
                if (j != null)
                {
                    foreach (var valueOs in j)
                    {
                        decimal totalItens = 0m;
                        decimal totalItensServico = 0m;
                        decimal totalItensMaterial = 0m;
                        var auto = Convert.ToInt32(valueOs.autonumero);

                        var k = dc.tb_os_itens.Where(a => a.autonumeroOs == auto && a.cancelado != "S").ToList();

                        foreach (var valueItem in k)
                        {
                            if (valueItem.servico.Equals("S"))
                            {
                                totalItensServico = Convert.ToDecimal(valueItem.total) + totalItensServico;
                            }
                            else
                            {
                                totalItensMaterial = Convert.ToDecimal(valueItem.total) + totalItensMaterial;
                            }
                            totalItens = Convert.ToDecimal(valueItem.total) + totalItens;

                        }
                        valueOs.valorServico = Convert.ToDecimal(totalItensServico.ToString("#######0.00"));
                        valueOs.valorMaterial = Convert.ToDecimal(totalItensMaterial.ToString("#######0.00"));
                        valueOs.valor = Convert.ToDecimal(valueOs.valorServico + valueOs.valorMaterial);

                        if (valueOs.desabilitado == null)
                        {
                            valueOs.desabilitado = 0;
                        }

                        dc.tb_os.AddOrUpdate(valueOs);
                        dc.SaveChanges();

                    }
                }

            }

            return "";
        }

        [HttpGet]
        public IEnumerable<itensPlanilha> GetAllItensParaOS(Int64 autonumeroCliente, string data1, string data2, Int32 autonumeroSistema, Int32 autonumeroServico, string codigoOs)
        {
            var c = 1;
            IQueryable<itensPlanilha> itensForaDaPlanilha;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                if (string.IsNullOrEmpty(codigoOs))
                {
                    // Sistema !=  ORÇAMENTO pode ter mais de 1 S.S por O.S. -------------------------------------------------

                    itensForaDaPlanilha = (from x in (from i in dc.tb_os_itens
                                                      join p in dc.tb_os
                                                      on i.codigoOs equals p.codigoOs
                                                      where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && p.cancelado != "S" &&
                                                            i.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0 &&
                                                             p.nomeStatus == "Fechada" && p.autonumeroSistema == autonumeroSistema && p.autonumeroServico == autonumeroServico
                                                              && p.dataTermino >= data11 && p.dataTermino <= data22 && p.codigoOrdemServico == ""
                                                              && i.codigoPF != ""
                                                      group i by i.codigoPF into g
                                                      select new
                                                      {
                                                          codigoPF = g.Key,
                                                          quantidadePF = g.Sum(a => a.quantidadePF),
                                                          //totalPF = g.Sum(a => Math.Round((decimal)a.quantidadePF * (decimal)a.precoUnitarioPF, 2)),
                                                          totalPF = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                                          total = g.Sum(a => Math.Round((decimal)a.quantidade * (decimal)a.precoUnitario, 2)),

                                                      })
                                           join o in dc.tb_planilhafechada
                                           on x.codigoPF equals o.codigo
                                           where o.autonumeroCliente == autonumeroCliente
                                           select new itensPlanilha
                                           {

                                               autonumero = o.autonumero,
                                               codigoPF = x.codigoPF,
                                               nome = o.nome,
                                               unidade = o.unidade,
                                               preco = (decimal)o.preco,
                                               quantidadePF = (decimal)x.quantidadePF,
                                               totalPF = x.totalPF,
                                               total = x.total
                                           }).Distinct();
                }
                else
                {
                    // INICIO - Utilizado para que se tenha 1 S.S == 1 O.S ( quando o Sistema for ORÇAMENTO ) -> i.codigoOs == codigoOs -------------------------------------------------

                    itensForaDaPlanilha = (from x in (from i in dc.tb_os_itens
                                                      join p in dc.tb_os
                                                      on i.codigoOs equals p.codigoOs
                                                      where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && p.cancelado != "S" &&
                                                            i.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0 &&
                                                             p.nomeStatus == "Fechada" && p.autonumeroSistema == autonumeroSistema && p.autonumeroServico == autonumeroServico
                                                              && p.dataTermino >= data11 && p.dataTermino <= data22 && p.codigoOrdemServico == ""
                                                              && i.codigoPF != "" && i.codigoOs == codigoOs
                                                      group i by i.codigoPF into g
                                                      select new
                                                      {
                                                          codigoPF = g.Key,
                                                          quantidadePF = g.Sum(a => a.quantidadePF),
                                                          //totalPF = g.Sum(a => Math.Round((decimal)a.quantidadePF * (decimal)a.precoUnitarioPF, 2)),
                                                          totalPF = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                                          total = g.Sum(a => Math.Round((decimal)a.quantidade * (decimal)a.precoUnitario, 2)),
                                                      })
                                           join o in dc.tb_planilhafechada
                                           on x.codigoPF equals o.codigo
                                           where o.autonumeroCliente == autonumeroCliente
                                           select new itensPlanilha
                                           {

                                               autonumero = o.autonumero,
                                               codigoPF = x.codigoPF,
                                               nome = o.nome,
                                               unidade = o.unidade,
                                               preco = (decimal)o.preco,
                                               quantidadePF = (decimal)x.quantidadePF,
                                               totalPF = x.totalPF,
                                               total = x.total
                                           }).Distinct();
                    // FIM - Utilizado para que se tenha 1 S.S == 1 O.S ( quando o sistema for ORÇAMENTO ) -> i.codigoOs == codigoOs ----------------------------------------------

                }



                return itensForaDaPlanilha.ToList();
            }
        }

        public IEnumerable<itensPlanilha> GetAllItensCodigoOrdemServico(string codigoOrdemServico, Int64 autonumeroCliente)
        {
            var c = 1;
            using (var dc = new manutEntities())
            {

                var user = (from x in (from i in dc.tb_os_itens
                                       where i.codigoOrdemServico == codigoOrdemServico && i.codigoOrdemServico != "" && i.cancelado != "S" && i.autonumeroCliente == autonumeroCliente
                                       group i by new { i.codigoPF } into g
                                       select new
                                       {
                                           g.Key,
                                           quantidadePF = g.Sum(a => a.quantidadePF),
                                           //totalPF = g.Sum(a => Math.Round((decimal)a.quantidadePF * (decimal)a.precoUnitarioPF, 2)),
                                           //totalPF = Math.Round((decimal)g.Sum(a => a.totalPF), 2)
                                           totalPF = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                           //totalPF = (decimal)g.Sum(a => a.totalPF)
                                       })
                            join o in dc.tb_os_itens
                            on x.Key.codigoPF equals o.codigoPF
                            where o.codigoOrdemServico == codigoOrdemServico && o.codigoOrdemServico != "" && o.cancelado != "S" && o.autonumeroCliente == autonumeroCliente
                            select new itensPlanilha
                            {
                                codigoPF = x.Key.codigoPF,
                                unidade = o.unidadePF,
                                nome = o.nomePF,
                                autonumero = 0,
                                quantidadePF = (decimal)x.quantidadePF,
                                totalPF = x.totalPF,
                                preco = (decimal)o.precoUnitarioPF,
                                qtdeMaoObra = (short)o.qtdeMaoObra
                                //precoUnitarioPF = (decimal)o.precoUnitarioPF
                            }).OrderByDescending(p => p.codigoPF).Distinct();


                //var user = from p in dc.tb_os_itens.Where((a => (a.codigoOrdemServico == codigoOrdemServico && a.cancelado != "S"))).OrderByDescending(p => p.autonumero) select p;
                return user.ToList();
            }
        }

        //public class itensPlanilha
        //{
        //    public Int64 autonumero { get; set; }
        //    public string codigoPF { get; set; }
        //    public string nome { get; set; }
        //    public string unidade { get; set; }
        //    public decimal preco { get; set; }
        //    public decimal quantidadePF { get; set; }
        //    public decimal totalPF { get; set; }


        //}

        [HttpGet]
        public IEnumerable<resumoPlanilha> GetAllResumoOs(Int64 autonumeroCliente, string data1, string data2)
        {
            var c = 1;
            List<resumoPlanilha> itensPlanilha;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {

                itensPlanilha = (from i in dc.tb_os_itens
                                 join p in dc.tb_os
                                 on i.codigoOs equals p.codigoOs
                                 where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && p.cancelado != "S" &&
                                 i.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0 &&
                                 p.nomeStatus == "Fechada" && p.dataTermino >= data11 && p.dataTermino <= data22 && p.autonumeroSistema != 10

                                 group i by new { p.autonumeroSistema, p.nomeSistema, p.nomeServico, p.autonumeroServico } into g
                                 select new resumoPlanilha
                                 {
                                     autonumeroServico = g.Key.autonumeroServico,
                                     nomeServico = g.Key.nomeServico,
                                     autonumeroSistema = g.Key.autonumeroSistema,
                                     nomeSistema = g.Key.nomeSistema,
                                     quantidadeSS = 1,
                                     //totalPF = g.Sum(a => Math.Round((decimal)a.quantidadePF * (decimal)a.precoUnitarioPF, 2)),
                                     totalPF = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                     codigoOs = ""
                                     //totalPF = Math.Round((decimal)g.Sum(a => a.totalPF), 2)
                                 }).Union(
                                    from i in dc.tb_os_itens
                                    join p in dc.tb_os
                                    on i.codigoOs equals p.codigoOs
                                    where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && p.cancelado != "S" &&
                                    i.autonumeroCliente == p.autonumeroCliente && i.quantidadePF > 0 &&
                                    p.nomeStatus == "Fechada" && p.dataTermino >= data11 && p.dataTermino <= data22 && p.autonumeroSistema == 10

                                    group i by new { p.autonumeroSistema, p.nomeSistema, p.nomeServico, p.autonumeroServico, p.codigoOs } into g
                                    select new resumoPlanilha
                                    {
                                        autonumeroServico = g.Key.autonumeroServico,
                                        nomeServico = g.Key.nomeServico,
                                        autonumeroSistema = g.Key.autonumeroSistema,
                                        nomeSistema = g.Key.nomeSistema,
                                        quantidadeSS = 1,
                                        //totalPF = g.Sum(a => Math.Round((decimal)a.quantidadePF * (decimal)a.precoUnitarioPF, 2)),
                                        totalPF = Math.Truncate(g.Sum(j => (decimal)j.quantidadePF * (decimal)j.precoUnitarioPF) * 100) / 100,
                                        codigoOs = g.Key.codigoOs
                                        //totalPF = Math.Round((decimal)g.Sum(a => a.totalPF), 2)
                                    }
                                 ).ToList();

                // QTDE DE OS por sistema ----------------------------------------------------------------------------------
                var qtdeOS = (from i in dc.tb_os

                              where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&
                                    i.nomeStatus == "Fechada" && i.valor > 0 && i.dataTermino >= data11 && i.dataTermino <= data22 && i.autonumeroSistema != 10

                              group i by new { i.autonumeroSistema, i.nomeSistema, i.nomeServico, i.autonumeroServico } into g
                              select new resumoPlanilha
                              {
                                  autonumeroSistema = g.Key.autonumeroSistema,
                                  nomeSistema = g.Key.nomeSistema,
                                  nomeServico = g.Key.nomeServico,
                                  autonumeroServico = g.Key.autonumeroServico,
                                  quantidadeSS = g.Count(),
                                  totalPF = Math.Truncate(g.Sum(j => (decimal)j.valor) * 100) / 100,
                              });

                var lista = new List<resumoPlanilha>();

                foreach (var itemP in itensPlanilha)
                {
                    foreach (var q in qtdeOS)
                    {
                        if (itemP.autonumeroSistema == q.autonumeroSistema && itemP.autonumeroServico == q.autonumeroServico && q.autonumeroSistema != 10)
                        {
                            itemP.quantidadeSS = q.quantidadeSS;
                            break;
                        }
                    }
                    lista.Add(itemP);
                }

                dc.SaveChanges();
                var xx = lista.ToList();
                return xx;

            }
            // FIM - QTDE DE OS por sistema ----------------------------------------------------------------------------------

        }


        //public static bool isOverlapDates(DateTime dtStartA, DateTime dtEndA, DateTime dtStartB, DateTime dtEndB)
        //{
        //    // Se o intervalo está dentro de outro - Interseção  -----------
        //    return dtStartA < dtEndB && dtStartB < dtEndA;
        //}

        [HttpGet]
        public IEnumerable<resumoPlanilha> GetAllResumoSsValorZerado(Int64 autonumeroCliente, string data1, string data2)
        {
            IQueryable<resumoPlanilha> itensPlanilha;
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {

                itensPlanilha = (from p in dc.tb_os
                                 where p.autonumeroCliente == autonumeroCliente && p.cancelado != "S" &&
                                 p.nomeStatus == "Fechada" && p.dataTermino >= data11 && p.dataTermino <= data22 && p.autonumeroSistema != 10 && p.valor == 0 && p.codigoOrdemServico == ""

                                 group p by new { p.autonumeroSistema, p.nomeSistema, p.nomeServico, p.autonumeroServico } into g
                                 select new resumoPlanilha
                                 {
                                     autonumeroServico = g.Key.autonumeroServico,
                                     nomeServico = g.Key.nomeServico,
                                     autonumeroSistema = g.Key.autonumeroSistema,
                                     nomeSistema = g.Key.nomeSistema,
                                     quantidadeSS = 1,
                                     totalPF = 0,
                                     codigoOs = ""
                                     //totalPF = Math.Round((decimal)g.Sum(a => a.totalPF), 2)
                                 });

                // QTDE DE OS por sistema ----------------------------------------------------------------------------------
                var qtdeOS = (from i in dc.tb_os

                              where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" &&
                                    i.nomeStatus == "Fechada" && i.valor == 0 && i.dataTermino >= data11 && i.dataTermino <= data22 && i.codigoOrdemServico == ""

                              group i by new { i.autonumeroSistema, i.nomeSistema, i.nomeServico, i.autonumeroServico } into g
                              select new resumoPlanilha
                              {
                                  autonumeroSistema = g.Key.autonumeroSistema,
                                  nomeSistema = g.Key.nomeSistema,
                                  nomeServico = g.Key.nomeServico,
                                  autonumeroServico = g.Key.autonumeroServico,
                                  quantidadeSS = g.Count(),
                                  totalPF = 0
                              });

                var lista = new List<resumoPlanilha>();

                foreach (var itemP in itensPlanilha)
                {
                    foreach (var q in qtdeOS)
                    {
                        if (itemP.autonumeroSistema == q.autonumeroSistema && itemP.autonumeroServico == q.autonumeroServico)
                        {
                            itemP.quantidadeSS = q.quantidadeSS;
                            break;
                        }
                    }
                    lista.Add(itemP);
                }

                dc.SaveChanges();
                return lista.ToList();

            }
            // FIM - QTDE DE OS por sistema ----------------------------------------------------------------------------------





        }

        [HttpGet]
        public System.Collections.IEnumerable GetSsValorZeradoPorSistema(Int64 autonumeroCliente, string data1, string data2, Int64 autonumeroSistema)
        {
            var data11 = Convert.ToDateTime(data1);
            var data22 = Convert.ToDateTime(data2);

            using (var dc = new manutEntities())
            {
                var itensPlanilha = (from p in dc.tb_os
                                     where p.autonumeroCliente == autonumeroCliente && p.cancelado != "S" &&
                                     p.nomeStatus == "Fechada" && p.dataTermino >= data11 && p.dataTermino <= data22 && p.autonumeroSistema != 10 && p.valor == 0 && p.autonumeroSistema == autonumeroSistema && p.codigoOrdemServico == ""
                                     select new
                                     {
                                         p.codigoOs,
                                         p.dataTermino
                                     });
                return itensPlanilha.ToList();

            }

        }

        [HttpPost]
        public string UploadFileOsFoto()
        {

            var message = HttpContext.Current.Request.Files.AllKeys.Any().ToString();

            try
            {

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);
                    var antesDepois = HttpContext.Current.Request.Form["antesDepois"].ToString().Trim();

                    var caminho = "~/UploadedFiles/";

                    if (httpPostedFile == null)
                    {
                        message = "Erro Upload 1";
                        return message;
                    }

                    //// Criar a pasta se não existir ou devolver informação sobre a pasta
                    //var inf = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(caminho));


                    var extension = Path.GetExtension(httpPostedFile.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(caminho), fileName);
                    if (File.Exists(fileSavePath))
                    {
                        File.Delete(fileSavePath);
                    }

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);



                    // Atualizar no BD ( tabela produtos )
                    using (var dc = new manutEntities())
                    {
                        var os = dc.tb_os.FirstOrDefault(a => a.autonumero == autonumero);


                        if (os != null)
                        {
                            if (os.url == null) os.url = "";
                            if (os.url1 == null) os.url1 = "";

                            if (antesDepois == "A")
                            {
                                os.url = fileName.Trim();
                            }
                            else
                            {
                                os.url1 = fileName.Trim();
                            }

                            dc.tb_os.AddOrUpdate(os);

                            dc.SaveChanges();
                            message = fileName;

                            return message;
                        }

                        message = "Erro Upload 2";
                        return message;
                    }

                }

                return message;


            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName,
                            ve.ErrorMessage));
                    }
                }
                message = sb.ToString();
                //throw new DbEntityValidationException(sb.ToString(), e);
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null
                    ? ex.InnerException.ToString().Substring(0, 130) + " - DataprodutoController SaveFilesFoto"
                    : ex.Message + " - DataprodutoController SaveFilesFoto";

            }

            return message;
        }

        [HttpPost]
        public string AcertarOSDesabilitado()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var osX = (from a in dc.tb_os

                           where a.cancelado != "S" && a.desabilitado == null
                           select new
                           {
                               a
                           }).ToList();

                osX.ForEach(x =>
                {
                    x.a.desabilitado = 0;
                });
                dc.SaveChanges();

                return string.Empty;

            }

        }

    }

}