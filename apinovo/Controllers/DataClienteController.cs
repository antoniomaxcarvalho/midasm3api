using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataClienteController : ApiController
    {


        [HttpGet]
        public IEnumerable<tb_cliente> GetCliente(Int32 autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_cliente.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }
        }


        public IEnumerable<tb_cliente> GetAllCliente()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_cliente.Where((a => a.cancelado != "S")) orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        //public IEnumerable<tb_cliente> GetAllClienteUsuario(string cliente)
        //{
        //    using (var dc = new manutEntities())
        //    {
        //        var user = from p in dc.tb_cliente.Where((a => a.cancelado != "S" && cliente.Contains(a.sigla))) orderby p.nome select p;
        //        return user.ToList(); ;
        //    }
        //}

        public IEnumerable GetAllClienteUsuario(string cliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_cliente.Where((a => a.cancelado != "S" && a.desabilitado == 0 && cliente.Contains(a.sigla) ))
                           select new
                           {
                               label = p.nome,
                               value = p.autonumero

                           };
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarCliente()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_cliente.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_cliente.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]
        public string IncluirCliente()
        {

            using (var dc = new manutEntities())
            {
                var k = new tb_cliente
                {
                    sigla = string.Empty,
                    nome = string.Empty,
                    contrato = string.Empty,
                    cep = string.Empty,
                    endereco = string.Empty,
                    numero = string.Empty,
                    complemento = string.Empty,
                    cidade = string.Empty,
                    estado = string.Empty,
                    email = string.Empty,
                    telefone = string.Empty,
                    contato = string.Empty,
                    dataInicio = null,
                    dataBase = string.Empty,
                    cancelado = "N",
                    bdiServico = 0,
                    bdiMaterial = 0,
                    contadorOs = 0,
                    dataLimite = null,
                    planilhaFechada = "N",
                    prazoInicialMeses = 0,
                    prorrogacoesMeses = 0,
                    reducao = 0,
                    margem = 0,
                    qtdeCustoFixoEtapa = 0,
                    obra = "",
                    cap = "",
                    processo = "",
                    acrescimoEstoque = 0,
                    contadorOrcamento = 0,
                    contadorOrdemServico = 0,
                    contadorPmocCivil = 0,
                    contadorPmocEquipamento = 0,
                    prazoInicialDias = "",
                    informacoesPMOC = "",
                    desabilitado = 0,
                    
                    

                };

                dc.tb_cliente.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }

        [HttpPost]
        public void AtualizarCliente()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"].ToString());


                var linha = dc.tb_cliente.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.sigla = HttpContext.Current.Request.Form["sigla"].ToString().Trim();
                    linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    linha.contrato = HttpContext.Current.Request.Form["contrato"].ToString().Trim();
                    linha.cep = HttpContext.Current.Request.Form["cep"].ToString().Trim();
                    linha.endereco = HttpContext.Current.Request.Form["endereco"].ToString().Trim();
                    linha.numero = HttpContext.Current.Request.Form["numero"].ToString().Trim();
                    linha.complemento = HttpContext.Current.Request.Form["complemento"].ToString().Trim();
                    linha.cidade = HttpContext.Current.Request.Form["cidade"].ToString().Trim();
                    linha.estado = HttpContext.Current.Request.Form["estado"].ToString().Trim();
                    linha.email = HttpContext.Current.Request.Form["email"].ToString().Trim();
                    linha.telefone = HttpContext.Current.Request.Form["telefone"].ToString().Trim();
                    linha.contato = HttpContext.Current.Request.Form["contato"].ToString().Trim();
                    linha.dataBase = HttpContext.Current.Request.Form["dataBase"].ToString().Trim();
                    linha.prazoInicialMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prazoInicialMeses"]);
                    linha.prorrogacoesMeses = Convert.ToInt32(HttpContext.Current.Request.Form["prorrogacoesMeses"]);
                   

                    if (IsDate(HttpContext.Current.Request.Form["dataLimite"].ToString()))
                    {
                        linha.dataLimite = Convert.ToDateTime(HttpContext.Current.Request.Form["dataLimite"].ToString());
                    }

                    if (IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
                    {
                        linha.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                    }

                    linha.bdiMaterial = Convert.ToDouble(HttpContext.Current.Request.Form["bdiMaterial"].ToString().Trim());
                    linha.bdiServico = Convert.ToDouble(HttpContext.Current.Request.Form["bdiServico"].ToString().Trim());
                    linha.planilhaFechada = HttpContext.Current.Request.Form["planilhaFechada"].ToString().Trim();
                    linha.reducao = Convert.ToDecimal(HttpContext.Current.Request.Form["reducao"].ToString().Trim());
                    linha.margem = Convert.ToDecimal(HttpContext.Current.Request.Form["margem"].ToString().Trim());
                    linha.acrescimoEstoque = Convert.ToDecimal(HttpContext.Current.Request.Form["acrescimoEstoque"].ToString().Trim());
                    linha.obra = HttpContext.Current.Request.Form["obra"].ToString().Trim();
                    linha.cap = HttpContext.Current.Request.Form["cap"].ToString().Trim();
                    linha.processo = HttpContext.Current.Request.Form["processo"].ToString().Trim();
                    linha.prazoInicialDias = HttpContext.Current.Request.Form["prazoInicialDias"].ToString().Trim();
                    linha.desabilitado = Convert.ToSByte(HttpContext.Current.Request.Form["prazoInicialMeses"].ToString().Trim());
                    //linha.qtdeCustoFixoEtapa = Convert.ToDecimal(HttpContext.Current.Request.Form["margem"].ToString().Trim());
                    dc.tb_cliente.AddOrUpdate(linha);
                    dc.SaveChanges();
                }
            }

        }

        [HttpPost]
        public string FinalizarCliente()
        {
            using (var dc = new manutEntities())
            {
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var siglaCliente = HttpContext.Current.Request.Form["siglaCliente"].ToString();

                using (var transaction = dc.Database.BeginTransaction())
                {
                    try
                    {
                        dc.tb_usuario.Where(x => (x.cliente.Contains(siglaCliente)) && x.cancelado != "S").ToList().ForEach(x =>
                       {
                           x.cliente = x.cliente.Replace(siglaCliente, "");

                       });
                        dc.SaveChanges();


                        // Criar o Registro de Fechada no Acompanhamento -------------------------------------------------

                        dc.tb_os.Where(x => (!x.nomeStatus.Contains("Fechada") && x.cancelado != "S") && x.autonumeroCliente == autonumeroCliente).ToList().ForEach(x =>
                        {

                            var Os = new tb_os_acompanhamento
                            {
                                nome = "Fechamento Pelo Sistema *",

                                autonumeroOs = x.autonumero,
                                codigoOs = x.codigoOs,
                                autonumeroUsuario = x.autonumeroUsuario,
                                nomeUsuario = x.nomeUsuario,
                                cancelado = "N",
                                data = x.dataSolicitacao,
                                autonumeroCliente = x.autonumeroCliente,
                                nomeCliente = x.nomeCliente,
                                nomeEquipe = x.nomeEquipe,
                                nomeStatus = "Fechada"
                            };

                            dc.tb_os_acompanhamento.Add(Os);
                            dc.SaveChanges();

                        });

                        // FIM - Criar o Registro de Fechada no Acompanhament ---------------------------------------------


                        dc.tb_os.Where(x => (!x.nomeStatus.Contains("Fechada")) && x.cancelado != "S" && x.autonumeroCliente == autonumeroCliente).ToList().ForEach(x =>
                        {
                            x.nomeStatus = "Fechada";
                            x.dataTermino = x.dataSolicitacao;

                        });
                        dc.SaveChanges();

                        transaction.Commit();
                        return string.Empty;
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return e.Message;
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
