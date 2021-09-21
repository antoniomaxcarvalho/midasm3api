using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataEquipamentoController : ApiController
    {

        [HttpGet]
        public IEnumerable<tb_cadastro> GetEquipamento(Int64 autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_cadastro.Where(a => a.autonumero == autonumero && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_cadastro> GetEquipamentoCliente(Int64 autonumeroCliente, int nroDeLinhas, int autonumeroSubSistema, int autonumeroEquipe,
            int autonumeroLocalFisico, int autonumeroSetor, int autonumeroLocalAtendido)
        {
            using (var dc = new manutEntities())
            {


                if (autonumeroSubSistema > 0)
                {

                    var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema).OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                    if (nroDeLinhas > 0)
                    {
                        return user1.ToList().Take(nroDeLinhas);
                    }
                    else
                    {
                        return user1.ToList();
                    }
                }
                if (autonumeroEquipe > 0)
                {

                    var user2 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroEquipe == autonumeroEquipe).OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                    if (nroDeLinhas > 0)
                    {
                        return user2.ToList().Take(nroDeLinhas);
                    }
                    else
                    {
                        return user2.ToList();
                    }
                }
                if (autonumeroLocalFisico > 0)
                {

                    var user3 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico).OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                    if (nroDeLinhas > 0)
                    {
                        return user3.ToList().Take(nroDeLinhas);
                    }
                    else
                    {
                        return user3.ToList();
                    }
                }

                if (autonumeroSetor > 0)
                {

                    var user3 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                    if (nroDeLinhas > 0)
                    {
                        return user3.ToList().Take(nroDeLinhas);
                    }
                    else
                    {
                        return user3.ToList();
                    }
                }

                if (autonumeroLocalAtendido > 0)
                {

                    var user3 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalAtendido == autonumeroLocalAtendido).OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                    if (nroDeLinhas > 0)
                    {
                        return user3.ToList().Take(nroDeLinhas);
                    }
                    else
                    {
                        return user3.ToList();
                    }
                }


                var user = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").OrderBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema).ThenBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico) select p;

                if (nroDeLinhas > 0)
                {
                    return user.ToList().Take(nroDeLinhas);
                }
                else
                {
                    return user.ToList();
                }

            }
        }
        public IEnumerable<tb_cadastro> GetAllEquipamento()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_cadastro.Where((a => a.cancelado != "S")) orderby p.modelo select p;
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarEquipamento()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_cadastro.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_cadastro.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]
        public string IncluirEquipamento()
        {

            using (var dc = new manutEntities())
            {
                var k = new tb_cadastro
                {

                    autonumeroCliente = 0,
                    nomeCliente = string.Empty,
                    autonumeroSistema = 0,
                    nomeSistema = string.Empty,
                    autonumeroSubSistema = 0,
                    nomeSubSistema = string.Empty,
                    autonumeroLocalAtendido = 0,
                    nomeLocalAtendido = string.Empty,
                    autonumeroLocalFisico = 0,
                    nomeLocalFisico = string.Empty,
                    autonumeroEquipe = 0,
                    nomeEquipe = string.Empty,
                    autonumeroSetor = 0,
                    nomeSetor = string.Empty,
                    autonumeroPredio = 0,
                    nomePredio = string.Empty,
                    serie = string.Empty,
                    modelo = string.Empty,
                    capacidade = string.Empty,
                    patrimonio = string.Empty,
                    diaUtil = 0,
                    grupo = 0,
                    fabricante = string.Empty,
                    dataPrevista = null,
                    dataCompra = null,
                    dataGarantia = null,
                    cancelado = "N",
                    periodicidade = 0,
                    endereco = string.Empty,
                    mesManuAno = string.Empty,
                    mesManuSem01 = string.Empty,
                    mesManuSem02 = string.Empty,
                    mesManuTri01 = string.Empty,
                    mesManuTri02 = string.Empty,
                    mesManuTri03 = string.Empty,
                    mesManuTri04 = string.Empty

                };

                dc.tb_cadastro.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }

        [HttpPost]
        public string AtualizarEquipamento()
        {

            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"].ToString());

                var linha = new tb_cadastro
                {

                    autonumeroCliente = 0,
                    nomeCliente = string.Empty,
                    autonumeroSistema = 0,
                    nomeSistema = string.Empty,

                    autonumeroSubSistema = 0,
                    nomeSubSistema = string.Empty,
                    autonumeroLocalAtendido = 0,
                    nomeLocalAtendido = string.Empty,
                    autonumeroLocalFisico = 0,
                    nomeLocalFisico = string.Empty,
                    autonumeroEquipe = 0,
                    nomeEquipe = string.Empty,
                    autonumeroSetor = 0,
                    nomeSetor = string.Empty,
                    autonumeroPredio = 0,
                    nomePredio = string.Empty,
                    serie = string.Empty,
                    modelo = string.Empty,
                    capacidade = string.Empty,
                    patrimonio = string.Empty,
                    diaUtil = 0,
                    grupo = 0,
                    fabricante = string.Empty,
                    dataPrevista = null,
                    dataCompra = null,
                    dataGarantia = null,
                    cancelado = "N",
                    periodicidade = 0,
                    endereco = string.Empty,
                    mesManuAno = string.Empty,
                    mesManuSem01 = string.Empty,
                    mesManuSem02 = string.Empty,
                    mesManuTri01 = string.Empty,
                    mesManuTri02 = string.Empty,
                    mesManuTri03 = string.Empty,
                    mesManuTri04 = string.Empty
                };

                if (autonumero > 0)
                {
                    linha = dc.tb_cadastro.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha == null || linha.cancelado == "S")
                    {
                        throw new ArgumentException("Execption");
                    }
                }

                //var x = HttpContext.Current.Request.Form["autonumeroSetor"].ToString().Trim();

                linha.autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                linha.nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                linha.autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                linha.nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
                linha.autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());
                linha.nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString().Trim();
                linha.autonumeroLocalAtendido = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocalAtendido"].ToString().Trim());
                linha.nomeLocalAtendido = HttpContext.Current.Request.Form["nomeLocalAtendido"].ToString().Trim();
                linha.autonumeroLocalFisico = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocalFisico"].ToString());
                linha.nomeLocalFisico = HttpContext.Current.Request.Form["nomeLocalFisico"].ToString().Trim();
                linha.autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString());
                linha.nomeEquipe = HttpContext.Current.Request.Form["nomeEquipe"].ToString().Trim();
                linha.autonumeroSetor = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSetor"].ToString());
                linha.nomeSetor = HttpContext.Current.Request.Form["nomeSetor"].ToString().Trim();
                linha.autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                linha.nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();

                linha.serie = HttpContext.Current.Request.Form["serie"].ToString().Trim();
                linha.modelo = HttpContext.Current.Request.Form["modelo"].ToString().Trim();
                linha.capacidade = HttpContext.Current.Request.Form["capacidade"].ToString().Trim();
                linha.patrimonio = HttpContext.Current.Request.Form["patrimonio"].ToString().Trim();
                linha.fabricante = HttpContext.Current.Request.Form["fabricante"].ToString().Trim();
                linha.cancelado = "N";

                linha.diaUtil = Convert.ToInt32(HttpContext.Current.Request.Form["diaUtil"].ToString().Trim());
                linha.grupo = Convert.ToInt32(HttpContext.Current.Request.Form["grupo"].ToString().Trim());

                if (IsDate(HttpContext.Current.Request.Form["dataPrevista"].ToString()))
                {
                    linha.dataPrevista = Convert.ToDateTime(HttpContext.Current.Request.Form["dataPrevista"].ToString());
                }
                if (IsDate(HttpContext.Current.Request.Form["dataCompra"].ToString()))
                {
                    linha.dataCompra = Convert.ToDateTime(HttpContext.Current.Request.Form["dataCompra"].ToString());
                }
                if (IsDate(HttpContext.Current.Request.Form["dataGarantia"].ToString()))
                {
                    linha.dataGarantia = Convert.ToDateTime(HttpContext.Current.Request.Form["dataGarantia"].ToString());
                }

                linha.periodicidade = Convert.ToInt32(HttpContext.Current.Request.Form["periodicidade"].ToString());
                linha.endereco = HttpContext.Current.Request.Form["endereco"].ToString().Trim();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuAno"].ToString().Trim()))
                {
                    linha.mesManuAno = HttpContext.Current.Request.Form["mesManuAno"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuAno = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuSem01"].ToString().Trim()))
                {
                    linha.mesManuSem01 = HttpContext.Current.Request.Form["mesManuSem01"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuSem01 = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuSem02"].ToString().Trim()))
                {
                    linha.mesManuSem02 = HttpContext.Current.Request.Form["mesManuSem02"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuSem02 = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuTri01"].ToString().Trim()))
                {
                    linha.mesManuTri01 = HttpContext.Current.Request.Form["mesManuTri01"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuTri01 = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuTri02"].ToString().Trim()))
                {
                    linha.mesManuTri02 = HttpContext.Current.Request.Form["mesManuTri02"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuTri02 = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuTri03"].ToString().Trim()))
                {
                    linha.mesManuTri03 = HttpContext.Current.Request.Form["mesManuTri03"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuTri03 = "";
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["mesManuTri04"].ToString().Trim()))
                {
                    linha.mesManuTri04 = HttpContext.Current.Request.Form["mesManuTri04"].ToString().Trim().PadLeft(2, '0');
                }
                else
                {
                    linha.mesManuTri04 = "";
                }

                dc.tb_cadastro.AddOrUpdate(linha);
                dc.SaveChanges();

                return linha.autonumero.ToString("#######0");

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


        [HttpGet]
        public IEnumerable<EquipSubSistema> GetEquipamentoSubSistema(Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                List<EquipSubSistema> l1 = null;
                l1 = dc.tb_cadastro.Where(h => h.autonumeroCliente == autonumeroCliente && h.cancelado != "S").GroupBy(c => new { c.autonumeroSubSistema }).Select(g => new EquipSubSistema { total = g.Count(), autonumeroSubSistema = g.Key.autonumeroSubSistema }).ToList();

                return l1;
            }

        }
        public class EquipSubSistema
        {
            public int total { get; set; }
            public int? autonumeroSubSistema { get; set; }

        };



        public IEnumerable<GrupoData> GetEquipamentoGrupoCliente(Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = dc.tb_cadastro.Where(h => h.autonumeroCliente == autonumeroCliente && h.cancelado != "S").
                    GroupBy(c => new { c.grupo, c.diaUtil }).Select(g => new GrupoData { diaUtil = g.Key.diaUtil, grupo = g.Key.grupo }).ToList();

                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<ResultadoPMOC> GetResultadoPMOC(Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = dc.Database.SqlQuery<ResultadoPMOC>("SELECT distinct dataPrevista,diaUtil,nomeEquipe,nomeSubSistema,count(grupo) as totalGrupo FROM manut.tb_cadastro where cancelado != 'S' and autonumeroCliente = " + autonumeroCliente + " group by diaUtil, nomeEquipe,nomeSubSistema,dataPrevista ").ToList();
                //var comando = "SELECT distinct dataPrevista,diaUtil,grupo,nomeEquipe,nomeSubSistema,count(grupo) as totalGrupo FROM manut.tb_cadastro group by nomeEquipe,diaUtil,grupo ";
                //var user = dc.Database.ExecuteSqlCommand(comando);
                return user.ToList(); ;
            }
        }

        public class GrupoData
        {
            public int? grupo { get; set; }
            public int? diaUtil { get; set; }

        };


        public class ResultadoPMOC
        {
            public DateTime? dataPrevista { get; set; }
            public int? diaUtil { get; set; }
            public int? grupo { get; set; }
            public string nomeEquipe { get; set; }
            public string nomeSubSistema { get; set; }
            public int? totalGrupo { get; set; }
        };

        [HttpGet]
        public IEnumerable<Condicao> GetCondicoes(Int32 autonumeroCliente, int nroMaxDiasUteis, string dInicio)
        {

            var c = 1;

            using (var dc = new manutEntities())
            {
                // INCLUIR No cliente se não existir SUBSISTEMA -----------------------------------------------------
                var p = (from x in (from i in dc.tb_cadastro
                                    where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S"
                                    group i by new { i.autonumeroSistema, i.autonumeroSubSistema } into g
                                    select new
                                    {
                                        g.Key
                                    })
                         join o in dc.tb_subsistema
                         on x.Key.autonumeroSubSistema equals o.autonumero
                         select new
                         {
                             o
                         });

                foreach (var valueItem in p)
                {
                    var autonumeroSubsistema = Convert.ToInt32(valueItem.o.autonumero);
                    var linha = dc.tb_subsistemacliente.Find(autonumeroCliente, autonumeroSubsistema); // sempre irá procurar pela chave primaria
                    if (linha == null)
                    {

                        var autonumeroEquipe = 1;
                        var nomeEquipe = "1pr";

                        var semestre = string.Empty;
                        var trimestre = string.Empty;
                        var bimestre = string.Empty;
                        var anual = string.Empty;
                        var mesesParaCalcular = string.Empty;



                        if (IsDate(dInicio))
                        {
                            var dataInicio = Convert.ToDateTime(dInicio);
                            if (valueItem.o.chkAno == 1)
                            {
                                anual = dataInicio.ToString("MMMM");
                                mesesParaCalcular = dataInicio.ToString("MM");
                            }
                            if (valueItem.o.chkSemestre == 1)
                            {
                                semestre = dataInicio.AddMonths(6).ToString("MMMM") + " - " + dataInicio.AddMonths(12).ToString("MMMM");

                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(6).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular.Trim() + (dataInicio.AddMonths(6).ToString("MM")).Trim();
                                }
                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(12).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular.Trim() + (dataInicio.AddMonths(12).ToString("MM")).Trim();
                                }
                            }
                            if (valueItem.o.chkTrimestre == 1)
                            {
                                trimestre = dataInicio.AddMonths(3).ToString("MMMM") + " - " + dataInicio.AddMonths(6).ToString("MMMM") + " - " + dataInicio.AddMonths(9).ToString("MMMM") + " - " + dataInicio.AddMonths(12).ToString("MMMM");

                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(3).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular + (dataInicio.AddMonths(3).ToString("MM"));
                                }
                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(6).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular + (dataInicio.AddMonths(6).ToString("MM"));
                                }
                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(9).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular + (dataInicio.AddMonths(9).ToString("MM"));
                                }
                                if (!mesesParaCalcular.Contains(dataInicio.AddMonths(12).ToString("MM")))
                                {
                                    mesesParaCalcular = mesesParaCalcular + (dataInicio.AddMonths(12).ToString("MM"));
                                }

                            }

                        }


                        var l = new tb_subsistemacliente();
                        l.autonumeroCliente = autonumeroCliente;
                        l.autonumeroSubsistema = autonumeroSubsistema;
                        l.anual = anual;
                        l.autonumeroEquipe = autonumeroEquipe;
                        l.autonumeroEquipe2 = valueItem.o.autonumeroEquipe2;
                        l.autonumeroSistema = valueItem.o.autonumeroSistema;
                        l.chkTodoMes = valueItem.o.chkTodoMes;
                        l.mesesParaCalcular = mesesParaCalcular;
                        l.nome = valueItem.o.nome;
                        l.nomeEquipe = nomeEquipe;
                        l.nomeEquipe2 = valueItem.o.nomeEquipe2;
                        l.nomeSistema = valueItem.o.nomeSistema;
                        l.qtdeAtendidaEquipePorDia = valueItem.o.qtdeAtendidaEquipePorDia;
                        l.qtdePorGrupoRelatorio = valueItem.o.qtdePorGrupoRelatorio;
                        l.semestre = semestre;
                        l.trimestre = trimestre;
                        l.bimestre = bimestre;
                        l.chkTodoMes = 1;

                        dc.tb_subsistemacliente.AddOrUpdate(l);
                        dc.SaveChanges();
                    }
                }

                // FIM - INCLUIR No cliente se não existir SUBSISTEMA -----------------------------------------------------


                var csql = new StringBuilder();

                csql.Append("SELECT c.autonumeroSubSistema,c.nomeSubSistema,count(c.autonumeroSubSistema) as qtdeAparelho, ");
                csql.Append("s.qtdePorGrupoRelatorio, s.qtdeAtendidaEquipePorDia, s.nomeEquipe,s.autonumeroEquipe,s.nomeEquipe2,s.autonumeroEquipe2 , ");
                csql.Append("FLOOR((count(c.autonumeroSubSistema) / (s.qtdeAtendidaEquipePorDia * " + nroMaxDiasUteis + ")) + 1) as qtdeEquipes , ");
                csql.Append("anual,semestre,trimestre,s.chkTodoMes,s.mesesParaCalcular  ");
                csql.Append("FROM manut.tb_cadastro as c, manut.tb_subsistemacliente as s ");
                csql.Append("WHERE c.cancelado != 'S'  and  c.autonumeroSubSistema = s.autonumeroSubSistema and  c.autonumeroCliente = " + autonumeroCliente + "  " + " and s.autonumeroCliente = " + autonumeroCliente + " ");
                csql.Append("GROUP by s.nomeEquipe,c.nomeSubSistema,c.autonumeroSubSistema ORDER BY nomeSubSistema; ");
                //     csql.Append("WHERE c.autonumeroSubSistema = s.autonumero and c.autonumeroCliente = " + autonumeroCliente + " and ( c.autonumeroSubSistema = 17 || c.autonumeroSubSistema = 75 ) group by s.nomeEquipe,c.nomeSubSistema,c.autonumeroSubSistema; ");

                var user = dc.Database.SqlQuery<Condicao>(csql.ToString()).ToList();
                return user.ToList(); ;

            }
        }

        [HttpPost]
        public void AtualizarCondicoes()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"]);
                var autonumeroSubsistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

                var linha = dc.tb_subsistemacliente.Find(autonumeroCliente, autonumeroSubsistema); // sempre irá procurar pela chave primaria
                if (linha != null)
                {

                    linha.autonumeroEquipe = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe"].ToString().Trim());
                    linha.nomeEquipe = HttpContext.Current.Request.Form["nomeEquipe"].ToString().Trim();
                    linha.autonumeroEquipe2 = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipe2"].ToString().Trim());
                    linha.nomeEquipe2 = HttpContext.Current.Request.Form["nomeEquipe2"].ToString().Trim();
                    linha.qtdeAtendidaEquipePorDia = Convert.ToInt32(HttpContext.Current.Request.Form["qtdeAtendidaEquipePorDia"].ToString().Trim());
                    linha.qtdePorGrupoRelatorio = 8;
                    //linha.qtdePorGrupoRelatorio = Convert.ToInt32(HttpContext.Current.Request.Form["qtdePorGrupoRelatorio"].ToString().Trim());
                    linha.anual = HttpContext.Current.Request.Form["anual"].ToString().Trim();
                    linha.semestre = HttpContext.Current.Request.Form["semestre"].ToString().Trim();
                    linha.trimestre = HttpContext.Current.Request.Form["trimestre"].ToString().Trim();
                    linha.chkTodoMes = Convert.ToSByte(HttpContext.Current.Request.Form["chkTodoMes"].ToString().Trim());

                    var mesesParaCalcular = string.Empty;
                    var todosMeses = linha.anual + linha.semestre + linha.trimestre;

                    if (todosMeses.ToLower().Contains("jan"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "01";
                    }
                    if (todosMeses.ToLower().Contains("fev"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "02";
                    }
                    if (todosMeses.ToLower().Contains("mar"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "03";
                    }
                    if (todosMeses.ToLower().Contains("abr"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "04";
                    }
                    if (todosMeses.ToLower().Contains("mai"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "05";
                    }
                    if (todosMeses.ToLower().Contains("jun"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "06";
                    }
                    if (todosMeses.ToLower().Contains("jul"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "07";
                    }
                    if (todosMeses.ToLower().Contains("ago"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "08";
                    }
                    if (todosMeses.ToLower().Contains("set"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "09";
                    }
                    if (todosMeses.ToLower().Contains("out"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "10";
                    }
                    if (todosMeses.ToLower().Contains("nov"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "11";
                    }
                    if (todosMeses.ToLower().Contains("dez"))
                    {
                        mesesParaCalcular = mesesParaCalcular + "12";
                    }

                    linha.mesesParaCalcular = mesesParaCalcular;

                    dc.tb_subsistemacliente.AddOrUpdate(linha);
                    dc.SaveChanges();

                }
            }

        }

        public string AutorizarOCLinha()
        {

            using (var dc = new manutEntities())
            {

                var c = 1;
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var autonumeroSubsistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubsistema"].ToString());

                var linha = dc.tb_subsistemacliente.Find(autonumeroCliente, autonumeroSubsistema); // sempre irá procurar pela chave primaria
                if (linha != null)
                {


                    if (linha.chkTodoMes > 0)
                    {
                        linha.chkTodoMes = 0;
                    }
                    else
                    {
                        linha.chkTodoMes = 1;
                    }
                    dc.tb_subsistemacliente.AddOrUpdate(linha);
                    dc.SaveChanges();
                }
                return "";
            }

        }


        public class Condicao
        {
            public int? autonumeroSubSistema { get; set; }
            public int? qtdeAparelho { get; set; }
            public int? qtdePorGrupoRelatorio { get; set; }
            public int? qtdeAtendidaEquipePorDia { get; set; }
            public int? autonumeroEquipe { get; set; }
            public int? qtdeEquipes { get; set; }
            public int? autonumeroEquipe2 { get; set; }
            public string nomeSubSistema { get; set; }
            public string nomeEquipe { get; set; }
            public string nomeEquipe2 { get; set; }
            public string anual { get; set; }
            public string semestre { get; set; }
            public string trimestre { get; set; }
            public string mesesParaCalcular { get; set; }
            public sbyte chkTodoMes { get; set; }


        };


        [HttpPost]
        public string CalcularPMOC()

        {
            var message = String.Empty;

            try
            {
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
                var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());
                var nroMaxDiasUteis = Convert.ToInt32(HttpContext.Current.Request.Form["nroMaxDiasUteis"].ToString());
                var pmocCheckList = HttpContext.Current.Request.Form["pmocCheckList"].ToString();

                List<DiaUtil> listaDiaUtil = null;

                if (IsDate(HttpContext.Current.Request.Form["dataInicio"]) &&
                    IsDate(HttpContext.Current.Request.Form["dataFim"]))
                {
                    var dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                    var dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());
                    listaDiaUtil = DataFeriadoController.GetDiasUteisIntervalo(dataInicio, dataFim, nroMaxDiasUteis);
                }
                else
                {
                    listaDiaUtil = DataFeriadoController.GetDiasUteisMes(ano, mes, nroMaxDiasUteis);
                }

                var e = new DataTabelasFixasController();
                var equipesBd = e.GetAllEquipe();


                int nroDiasUteisMes = listaDiaUtil.Count;

                LimparPMOC(); // Zerar datas e grupos e equipes na tabela equipamento

                Debug.WriteLine("LimparPMOC");

                Debug.WriteLine("LimparPMOC");
                long contadorPmocEquipamento = 0;
                using (var dc = new manutEntities())
                {
                    var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (cli == null)
                    {
                        throw new ArgumentException("Erro: Tabela Contrato Não Encontrada");
                    }
                    contadorPmocEquipamento = (long)cli.contadorPmocEquipamento;
                }

                using (var dc = new manutEntities())
                {

                    var condi = GetCondicoes(autonumeroCliente, nroMaxDiasUteis, null);
                    int contGrupo = 0;

                    if (condi != null)
                    {

                        int contRegistroDia = 0;
                        int? autonumeroSubSistema = 0;
                        int contDiaUtil = 0;


                        foreach (var condicoes in condi)
                        {
                            // TESTAR se o subSistema deverá ser calculado ------------------------------
                            if (condicoes.chkTodoMes == 0)
                            {
                                if (!mes.ToString().Trim().PadLeft(2, '0').Contains(condicoes.mesesParaCalcular))
                                {
                                    continue;
                                }
                            }
                            // FIM TESTAR se o subSistema deverá ser calculado --------------------------

                            Debug.WriteLine(" condicoes.nomeSubSistema: " + condicoes.nomeSubSistema);


                            autonumeroSubSistema = condicoes.autonumeroSubSistema;
                            var autonumeroEquipe = condicoes.autonumeroEquipe;
                            var nomeEquipe = condicoes.nomeEquipe;
                            var autonumeroEquipe2 = condicoes.autonumeroEquipe2;
                            var nomeEquipe2 = condicoes.nomeEquipe2;
                            var qtdeAtendidaEquipePorDia = condicoes.qtdeAtendidaEquipePorDia;
                            var qtdePorGrupoRelatorio = condicoes.qtdePorGrupoRelatorio;
                            var qtdPorGrupoRelatorioDia = 0;

                            if (autonumeroSubSistema == 53)
                            {
                                Debug.WriteLine(" qtdePorGrupoRelatorio " + qtdePorGrupoRelatorio);
                                Debug.WriteLine(" qtdeAtendidaEquipePorDia " + qtdeAtendidaEquipePorDia);

                            }

                            // ENCONTRAR EQUIPE DISPONÍNEL ------------------------------------------------------------------------

                            var ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe, item.autonumero));
                            if (ultimoDiaUtilEquipe == null)
                            {
                                continue;
                            }

                            contGrupo++;
                            contadorPmocEquipamento++;

                            // TESTAR Se a equipe PRINCIPAL encontrada pode ser usada Senão encontrar a equipe SECUNDARIA
                            if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                            {
                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                            }
                            else
                            {
                                // PROCURAR PELAS EQUIPES, UMA DISPONÍVEL
                                autonumeroEquipe = autonumeroEquipe2;
                                nomeEquipe = nomeEquipe2;

                                ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe2, item.autonumero));

                                //TESTAR Se a equipe SECUNDARIA encontrada pode ser usada Senão encontrar QUALQUER Equipe que atenda
                                if (ultimoDiaUtilEquipe == null)
                                {
                                    //QUALQUER Equipe que atenda
                                    ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                    if (ultimoDiaUtilEquipe != null)
                                    {
                                        Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                        autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                        nomeEquipe = ultimoDiaUtilEquipe.nome;

                                        if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                        contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                                    }
                                }
                                else
                                {

                                    if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                                    {
                                        // ENCONTROU A EQUIPE
                                        if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;

                                        autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                        nomeEquipe = ultimoDiaUtilEquipe.nome;
                                        contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                    }
                                    else
                                    {
                                        //QUALQUER Equipe que atenda
                                        ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                        if (ultimoDiaUtilEquipe != null)
                                        {
                                            // ENCONTROU A EQUIPE
                                            Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                            autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                            nomeEquipe = ultimoDiaUtilEquipe.nome;

                                            if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                            contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                        }
                                    }


                                }


                            }

                            // FIM -  ENCONTRAR EQUIPE DISPONÍNEL ------------------------------------------------------------------------


                            Debug.WriteLine(" autonumeroSubSistema: " + autonumeroSubSistema);
                            var k = GetEquipamentoEmOrdemVisita(autonumeroCliente, autonumeroSubSistema);

                            //DateTime? ultimaData = DateTime.Now.AddYears(10);
                            qtdPorGrupoRelatorioDia = 0;
                            contRegistroDia = 0;
                            foreach (var equipamento in k)
                            {

                                equipamento.autonumeroEquipe = autonumeroEquipe;
                                equipamento.nomeEquipe = nomeEquipe;

                                //Debug.WriteLine("listaDiaUtil " + contDiaUtil + " nomeEquipe" + nomeEquipe.Trim() + " autonumeroSubSistema: " + autonumeroSubSistema + " " + condicoes.nomeSubSistema);
                                equipamento.diaUtil = listaDiaUtil[contDiaUtil].diaUtil;
                                equipamento.dataPrevista = listaDiaUtil[contDiaUtil].data;
                                equipamento.grupo = contGrupo;
                                equipamento.contadorPmocEquipamento = contadorPmocEquipamento;

                                //if (ultimaData != equipamento.dataPrevista && contGrupo > 1)
                                //{
                                //    ultimaData = equipamento.dataPrevista;
                                //    contGrupo++;
                                //}





                                ultimoDiaUtilEquipe.diaUtil = equipamento.diaUtil;

                                qtdPorGrupoRelatorioDia++;
                                if (qtdPorGrupoRelatorioDia >= qtdePorGrupoRelatorio)
                                {
                                    contGrupo++;
                                    contadorPmocEquipamento++;
                                    qtdPorGrupoRelatorioDia = 0;
                                }

                                contRegistroDia++;

                                Debug.WriteLine(" contRegistroDia >= qtdeAtendidaEquipePorDia: " + qtdeAtendidaEquipePorDia);
                                if (contRegistroDia >= qtdeAtendidaEquipePorDia)
                                {
                                    Debug.WriteLine(" contRegistroDia >= qtdeAtendidaEquipePorDia: " + contRegistroDia);
                                    contRegistroDia = 0;

                                    contDiaUtil++;
                                    if (qtdPorGrupoRelatorioDia > 0)
                                    {
                                        contGrupo++;
                                        contadorPmocEquipamento++;
                                        qtdPorGrupoRelatorioDia = 0;
                                    }

                                    int f = contDiaUtil + 1;

                                    if (f > nroDiasUteisMes)
                                    {

                                        // PROCURAR PELAS EQUIPES, UMA DISPONÍVEL
                                        autonumeroEquipe = autonumeroEquipe2;
                                        nomeEquipe = nomeEquipe2;

                                        ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe2, item.autonumero));

                                        //TESTAR Se a equipe SECUNDARIA encontrada pode ser usada Senão encontrar QUALQUER Equipe que atenda
                                        if (ultimoDiaUtilEquipe == null)
                                        {
                                            //QUALQUER Equipe que atenda
                                            ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                            if (ultimoDiaUtilEquipe != null)
                                            {
                                                // ENCONTROU A EQUIPE
                                                Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                                autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                nomeEquipe = ultimoDiaUtilEquipe.nome;

                                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(" ccccccccccccccccccccccccccccultimoDiaUtilEquipe.diaUtil : " + ultimoDiaUtilEquipe.diaUtil);

                                            if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                                            {
                                                // ENCONTROU A EQUIPE
                                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;

                                                autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                nomeEquipe = ultimoDiaUtilEquipe.nome;
                                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;


                                            }
                                            else
                                            {
                                                //QUALQUER Equipe que atenda
                                                ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);
                                                Debug.WriteLine(" aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaultimoDiaUtilEquipe : " + ultimoDiaUtilEquipe);


                                                if (ultimoDiaUtilEquipe != null)
                                                {
                                                    // ENCONTROU A EQUIPE
                                                    Debug.WriteLine("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                                    autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                    nomeEquipe = ultimoDiaUtilEquipe.nome;

                                                    if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                                    contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                                }
                                            }


                                        }

                                    }
                                }

                                dc.tb_cadastro.AddOrUpdate(equipamento);

                                if (autonumeroSubSistema != condicoes.autonumeroSubSistema)
                                {
                                    contRegistroDia = 0;
                                    contDiaUtil = 0;
                                }



                            }

                            //contDiaUtil++;
                            dc.SaveChanges();

                        }

                        var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                        if (linha != null)
                        {
                            linha.contadorPmocEquipamento = contadorPmocEquipamento;
                            dc.tb_cliente.AddOrUpdate(linha);
                            dc.SaveChanges();
                        }

                    }

                }
                if (pmocCheckList == "S")
                {
                    SalvarPmocEquipamento(autonumeroCliente, ano, mes);
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.ToString();
                }
                Debug.WriteLine(" Exception ex: " + message);
            }

            return message;


        }

        [HttpPost]
        public string CalcularPMOCEquipamento()

        {
            var message = String.Empty;

            try
            {
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
                var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());
                var nroMaxDiasUteis = Convert.ToInt32(HttpContext.Current.Request.Form["nroMaxDiasUteis"].ToString());
                var sigla = HttpContext.Current.Request.Form["sigla"].ToString();

                var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

                List<DiaUtil> listaDiaUtil = null;


                //CalcularQtdeSubSistema(autonumeroCliente, anoMes);
                //return "";
                //if (IsDate(HttpContext.Current.Request.Form["dataInicio"]) &&
                //    IsDate(HttpContext.Current.Request.Form["dataFim"]))
                //{
                //    var dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                //    var dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());
                //    listaDiaUtil = DataFeriadoController.GetDiasUteisIntervalo(dataInicio, dataFim, nroMaxDiasUteis);
                //}
                //else
                //{
                listaDiaUtil = DataFeriadoController.GetDiasUteisMes(ano, mes, nroMaxDiasUteis);
                //}

                var e = new DataTabelasFixasController();
                var equipesBd = e.GetAllEquipe();


                int nroDiasUteisMes = listaDiaUtil.Count;

                LimparPMOC(); // Zerar datas e grupos e equipes na tabela equipamento

                Debug.WriteLine("LimparPMOC");
                long contadorPmocEquipamento = 0;
                string nomeCliente = string.Empty;
                using (var dc = new manutEntities())
                {
                    var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (cli == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Erro: Tabela Contrato Não Encontrada"));
                    }
                    contadorPmocEquipamento = (long)cli.contadorPmocEquipamento;
                    nomeCliente = cli.nome;
                }



                using (var dc = new manutEntities())
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        x.contadorPmocEquipamento = 0;
                        x.grupo = 0;
                        x.dataPrevista = null;

                    });
                    dc.SaveChanges();

                    var condi = GetCondicoes(autonumeroCliente, nroMaxDiasUteis, null);


                    int contGrupo = 0;

                    if (condi != null)
                    {

                        // Apenas os marcados ----------------
                        condi = condi.Where(p => p.chkTodoMes == 1).ToList();

                        int contRegistroDia = 0;
                        int? autonumeroSubSistema = 0;
                        int contDiaUtil = 0;


                        foreach (var condicoes in condi)
                        {
                            // TESTAR se o subSistema deverá ser calculado ------------------------------
                            if (condicoes.chkTodoMes == 0)
                            {
                                if (!mes.ToString().Trim().PadLeft(2, '0').Contains(condicoes.mesesParaCalcular))
                                {
                                    continue;
                                }
                            }
                            // FIM TESTAR se o subSistema deverá ser calculado --------------------------

                            Debug.WriteLine(" condicoes.nomeSubSistema: " + condicoes.nomeSubSistema);


                            autonumeroSubSistema = condicoes.autonumeroSubSistema;
                            var autonumeroEquipe = condicoes.autonumeroEquipe;
                            var nomeEquipe = condicoes.nomeEquipe;
                            var autonumeroEquipe2 = condicoes.autonumeroEquipe2;
                            var nomeEquipe2 = condicoes.nomeEquipe2;
                            var qtdeAtendidaEquipePorDia = condicoes.qtdeAtendidaEquipePorDia;
                            var qtdePorGrupoRelatorio = condicoes.qtdePorGrupoRelatorio;
                            var qtdPorGrupoRelatorioDia = 0;

                            //if (autonumeroSubSistema == 53)
                            //{
                            //    Debug.WriteLine(" qtdePorGrupoRelatorio " + qtdePorGrupoRelatorio);
                            //    Debug.WriteLine(" qtdeAtendidaEquipePorDia " + qtdeAtendidaEquipePorDia);

                            //}

                            // ENCONTRAR EQUIPE DISPONÍNEL ------------------------------------------------------------------------

                            var ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe, item.autonumero));
                            if (ultimoDiaUtilEquipe == null)
                            {
                                continue;
                            }

                            contGrupo++;
                            contadorPmocEquipamento++;

                            // TESTAR Se a equipe PRINCIPAL encontrada pode ser usada Senão encontrar a equipe SECUNDARIA
                            if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                            {
                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                            }
                            else
                            {
                                // PROCURAR PELAS EQUIPES, UMA DISPONÍVEL
                                autonumeroEquipe = autonumeroEquipe2;
                                nomeEquipe = nomeEquipe2;

                                ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe2, item.autonumero));

                                //TESTAR Se a equipe SECUNDARIA encontrada pode ser usada Senão encontrar QUALQUER Equipe que atenda
                                if (ultimoDiaUtilEquipe == null)
                                {
                                    //QUALQUER Equipe que atenda
                                    ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                    if (ultimoDiaUtilEquipe != null)
                                    {
                                        Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                        autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                        nomeEquipe = ultimoDiaUtilEquipe.nome;

                                        if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                        contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                                    }
                                }
                                else
                                {

                                    if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                                    {
                                        // ENCONTROU A EQUIPE
                                        if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;

                                        autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                        nomeEquipe = ultimoDiaUtilEquipe.nome;
                                        contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                    }
                                    else
                                    {
                                        //QUALQUER Equipe que atenda
                                        ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                        if (ultimoDiaUtilEquipe != null)
                                        {
                                            // ENCONTROU A EQUIPE
                                            Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                            autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                            nomeEquipe = ultimoDiaUtilEquipe.nome;

                                            if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                            contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                        }
                                    }


                                }


                            }

                            // FIM -  ENCONTRAR EQUIPE DISPONÍNEL ------------------------------------------------------------------------


                            Debug.WriteLine(" autonumeroSubSistema: " + autonumeroSubSistema);
                            var k = GetEquipamentoEmOrdemVisitaMes(autonumeroCliente, autonumeroSubSistema, mes);

                            //DateTime? ultimaData = DateTime.Now.AddYears(10);
                            qtdPorGrupoRelatorioDia = 0;
                            contRegistroDia = 0;
                            foreach (var equipamento in k)
                            {

                                equipamento.autonumeroEquipe = autonumeroEquipe;
                                equipamento.nomeEquipe = nomeEquipe;

                                Debug.WriteLine("listaDiaUtil " + contDiaUtil + " nomeEquipe" + nomeEquipe.Trim() + " autonumeroSubSistema: " + autonumeroSubSistema + " " + condicoes.nomeSubSistema);
                                equipamento.diaUtil = listaDiaUtil[contDiaUtil].diaUtil;
                                equipamento.dataPrevista = listaDiaUtil[contDiaUtil].data;
                                equipamento.grupo = contGrupo;
                                equipamento.contadorPmocEquipamento = contadorPmocEquipamento;

                                ultimoDiaUtilEquipe.diaUtil = equipamento.diaUtil;

                                qtdPorGrupoRelatorioDia++;
                                Debug.WriteLine("qtdPorGrupoRelatorioDia " + qtdPorGrupoRelatorioDia.ToString() + " qtdePorGrupoRelatorio" + qtdePorGrupoRelatorio.ToString());
                                    if (qtdPorGrupoRelatorioDia >= qtdePorGrupoRelatorio)
                                {
                                    contGrupo++;
                                    contadorPmocEquipamento++;
                                    qtdPorGrupoRelatorioDia = 0;
                                }

                                contRegistroDia++;

                                Debug.WriteLine(" contRegistroDia >= qtdeAtendidaEquipePorDia: " + qtdeAtendidaEquipePorDia);
                                if (contRegistroDia >= qtdeAtendidaEquipePorDia)
                                {
                                    Debug.WriteLine(" contRegistroDia >= qtdeAtendidaEquipePorDia: " + contRegistroDia);
                                    contRegistroDia = 0;

                                    contDiaUtil++;
                                    if (qtdPorGrupoRelatorioDia > 0)
                                    {
                                        contGrupo++;
                                        contadorPmocEquipamento++;
                                        qtdPorGrupoRelatorioDia = 0;
                                    }

                                    int f = contDiaUtil + 1;

                                    if (f > nroDiasUteisMes)
                                    {

                                        // PROCURAR PELAS EQUIPES, UMA DISPONÍVEL
                                        autonumeroEquipe = autonumeroEquipe2;
                                        nomeEquipe = nomeEquipe2;

                                        ultimoDiaUtilEquipe = equipesBd.FirstOrDefault(item => Equals(autonumeroEquipe2, item.autonumero));

                                        //TESTAR Se a equipe SECUNDARIA encontrada pode ser usada Senão encontrar QUALQUER Equipe que atenda
                                        if (ultimoDiaUtilEquipe == null)
                                        {
                                            //QUALQUER Equipe que atenda
                                            ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);

                                            if (ultimoDiaUtilEquipe != null)
                                            {
                                                // ENCONTROU A EQUIPE
                                                Debug.WriteLine("ultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                                autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                nomeEquipe = ultimoDiaUtilEquipe.nome;

                                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(" ccccccccccccccccccccccccccccultimoDiaUtilEquipe.diaUtil : " + ultimoDiaUtilEquipe.diaUtil);

                                            if (ultimoDiaUtilEquipe.diaUtil < nroDiasUteisMes)
                                            {
                                                // ENCONTROU A EQUIPE
                                                if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;

                                                autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                nomeEquipe = ultimoDiaUtilEquipe.nome;
                                                contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;


                                            }
                                            else
                                            {
                                                //QUALQUER Equipe que atenda
                                                ultimoDiaUtilEquipe = equipesBd.OrderBy(a => a.autonumero).FirstOrDefault(item => item.diaUtil < nroDiasUteisMes);
                                                Debug.WriteLine(" aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaultimoDiaUtilEquipe : " + ultimoDiaUtilEquipe);


                                                if (ultimoDiaUtilEquipe != null)
                                                {
                                                    // ENCONTROU A EQUIPE
                                                    Debug.WriteLine("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbultimoDiaUtilEquipe.nome: " + ultimoDiaUtilEquipe.nome);
                                                    autonumeroEquipe = ultimoDiaUtilEquipe.autonumero;
                                                    nomeEquipe = ultimoDiaUtilEquipe.nome;

                                                    if (ultimoDiaUtilEquipe.diaUtil > 0) ultimoDiaUtilEquipe.diaUtil--;
                                                    contDiaUtil = (int)ultimoDiaUtilEquipe.diaUtil;

                                                }
                                            }


                                        }

                                    }
                                }
                                var c = 1;

                                dc.tb_cadastro.AddOrUpdate(equipamento);

                                if (autonumeroSubSistema != condicoes.autonumeroSubSistema)
                                {
                                    contRegistroDia = 0;
                                    contDiaUtil = 0;
                                }



                            }

                            //contDiaUtil++;
                            dc.SaveChanges();

                        }


                        var linha = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                        if (linha != null)
                        {
                            contadorPmocEquipamento--;
                            linha.contadorPmocEquipamento = contadorPmocEquipamento;
                            dc.tb_cliente.AddOrUpdate(linha);
                            dc.SaveChanges();
                        }



                    }

                }

                //var imp = new DataImprimirController();
                //imp.ImprimirPmocEquipamento(autonumeroCliente, sigla, anoMes);

                SalvarPmocEquipamentoNovo(autonumeroCliente, ano, mes);





            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.ToString();
                }
                Debug.WriteLine(" Exception ex: " + message);
            }

            return message;


        }


        [HttpGet]
        public IEnumerable<tb_cadastro> GetEquipamentoEmOrdemVisita(Int64 autonumeroCliente, Int32? autonumeroSubSistema)
        {

            var csql = new StringBuilder();

            csql.Append("SELECT * ");
            csql.Append("FROM manut.tb_cadastro  ");
            csql.Append("WHERE cancelado != 'S' and autonumeroCliente =  " + autonumeroCliente + "  ");
            csql.Append("AND autonumeroSubSistema =  " + autonumeroSubSistema + "  ");
            csql.Append("ORDER BY nomeSubSistema,nomePredio,nomeLocalFisico,nomeLocalAtendido,grupo; ");

            using (var dc = new manutEntities())
            {
                var user = dc.Database.SqlQuery<tb_cadastro>(csql.ToString()).ToList();
                return user.ToList(); ;
            }




        }

        [HttpGet]
        public IEnumerable<tb_cadastro> GetEquipamentoEmOrdemSetor(Int64 autonumeroCliente, Int32? autonumeroSubSistema, Int32? autonumeroSetor)
        {

            var csql = new StringBuilder();

            csql.Append("SELECT * ");
            csql.Append("FROM manut.tb_cadastro  ");
            csql.Append("WHERE autonumeroCliente =  " + autonumeroCliente + "  ");
            csql.Append("AND autonumeroSubSistema =  " + autonumeroSubSistema + "  ");
            csql.Append("AND autonumeroSetor =  " + autonumeroSetor + "  ");
            csql.Append("ORDER BY nomeSubSistema,nomePredio,nomeLocalFisico,nomeLocalAtendido; ");

            using (var dc = new manutEntities())
            {
                var user = dc.Database.SqlQuery<tb_cadastro>(csql.ToString()).ToList();
                return user.ToList(); ;
            }
        }


        [HttpGet]
        public void LimparPMOC()
        {

            var csql = new StringBuilder();


            csql.Append("UPDATE  manut.tb_cadastro set diaUtil = 0,grupo = 0,nomeEquipe = '', autonumeroEquipe = 0,dataPrevista = null; ");



            using (var dc = new manutEntities())
            {
                var user = dc.Database.ExecuteSqlCommand(csql.ToString());
            }
        }


        [HttpGet]
        public IEnumerable GetEquipamentoEmOrdemGrupo(Int64 autonumeroCliente, Int32? autonumeroSubSistema)
        {
            using (var dc = new manutEntities())
            {

                //var user = dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.autonumeroSubSistema == autonumeroSubSistema).
                //  GroupBy(c => new { c.nomeSubSistema, c.grupo }).Select(g => new { g.Key.grupo, qtde = g.Count() }).ToList().OrderBy(p => p.grupo);



                // INCLUIR No cliente se não existir SUBSISTEMA -----------------------------------------------------
                var p = (from x in (from i in dc.tb_cadastro
                                    where i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.autonumeroSubSistema == autonumeroSubSistema
                                    group i by new { i.nomeSubSistema, i.grupo } into g
                                    select new
                                    {
                                        g.Key,
                                        qtde = g.Count()
                                    })
                         join o in dc.tb_cadastro.Where(k => k.autonumeroCliente == autonumeroCliente && k.cancelado != "S" && k.autonumeroSubSistema == autonumeroSubSistema)
                         on x.Key.nomeSubSistema equals o.nomeSubSistema
                         where x.Key.grupo == o.grupo
                         select new
                         {

                             o.nomeSubSistema,
                             o.grupo,
                             o.mesManuAno,
                             o.mesManuSemestre,
                             x.qtde
                         }).ToList().Distinct().OrderBy(o => o.grupo);

                //.GroupBy(k => k.grupo)
                return p;
            }



        }

        [HttpPost]
        public void AlterarAnoSemestreGrupo()
        {
            var c = 1;

            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var autonumeroSubSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistema"].ToString());

            var mesManuAno = HttpContext.Current.Request.Form["mesManuAno"].ToString().Trim();
            var mesManuSemestre = HttpContext.Current.Request.Form["mesManuSemestre"].ToString().Trim();
            var grupoInicial = Convert.ToInt32(HttpContext.Current.Request.Form["grupoInicial"].ToString().Trim());
            var grupoFinal = Convert.ToInt32(HttpContext.Current.Request.Form["grupoFinal"].ToString().Trim());

            var anoSemestreOuAmbos = HttpContext.Current.Request.Form["anoSemestreOuAmbos"].ToString().Trim();


            if (anoSemestreOuAmbos == "Ano")
            {

                using (var dc = new manutEntities())
                {
                    dc.tb_cadastro.Where(x => x.autonumeroCliente == autonumeroCliente && x.autonumeroSubSistema == autonumeroSubSistema && x.cancelado != "S" && x.grupo >= grupoInicial && x.grupo <= grupoFinal).ToList().ForEach(x =>
                    {
                        x.mesManuAno = mesManuAno;
                    });
                    dc.SaveChanges();
                }
            }

            if (anoSemestreOuAmbos == "Sem")
            {

                using (var dc = new manutEntities())
                {
                    dc.tb_cadastro.Where(x => x.autonumeroCliente == autonumeroCliente && x.autonumeroSubSistema == autonumeroSubSistema && x.cancelado != "S" && x.grupo >= grupoInicial && x.grupo <= grupoFinal).ToList().ForEach(x =>
                    {
                        x.mesManuSemestre = mesManuSemestre;
                    });
                    dc.SaveChanges();
                }
            }

            if (anoSemestreOuAmbos == "Ambos")
            {

                using (var dc = new manutEntities())
                {
                    dc.tb_cadastro.Where(x => x.autonumeroCliente == autonumeroCliente && x.autonumeroSubSistema == autonumeroSubSistema && x.cancelado != "S" && x.grupo >= grupoInicial && x.grupo <= grupoFinal).ToList().ForEach(x =>
                    {
                        x.mesManuAno = mesManuAno;
                        x.mesManuSemestre = mesManuSemestre;
                    });
                    dc.SaveChanges();
                }
            }

        }


        [HttpPost]
        public string SalvarPmocEquipamento(int autonumeroCliente, int ano, int mes)
        {
            var c = 1;
            //var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            //var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
            //var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {
                try
                {

                    //Apagar PMOC anterior -----------------------------------------------------------------------------
                    var lista = dc.checklisthistorico.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    if (lista.Count > 0)
                    {
                        if (lista[0].fechado == "S")
                        {
                            throw new ArgumentException("Erro - PMOC Fechado");
                        }
                    }
                    dc.checklisthistorico.RemoveRange(lista);
                    dc.SaveChanges();

                    lista.Clear();

                    var lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.checklisthistitem.RemoveRange(lista2);
                    dc.SaveChanges();

                    lista2.Clear();

                    //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------

                    //lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S").ToList()
                    lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S").ToList()
                             join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                             where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                             select new checklisthistorico
                             {
                                 anoMes = anoMes,
                                 autonumeroCliente = autonumeroCliente,
                                 autonumeroEquipamento = i.autonumero,
                                 autonumeroLocalFisico = i.autonumeroLocalFisico,
                                 autonumeroPredio = i.autonumeroPredio,
                                 autonumeroSetor = i.autonumeroSetor,
                                 autonumeroSistema = i.autonumeroSistema,
                                 autonumeroSubSistema = i.autonumeroSubSistema,
                                 cancelado = "N",
                                 capacidade = i.capacidade,
                                 contadorPmocEquipamento = i.contadorPmocEquipamento,
                                 dataPrevista = i.dataPrevista,
                                 endereco = i.endereco,
                                 fabricante = i.fabricante,
                                 fechado = "N",
                                 grupo = i.grupo,
                                 mesManuAno = i.mesManuAno,
                                 mesManuSemestre = i.mesManuSemestre,
                                 modelo = i.modelo,
                                 nomeCliente = i.nomeCliente,
                                 nomeLocalFisico = i.nomeLocalFisico,
                                 nomePredio = i.nomePredio,
                                 nomeSetor = i.nomeSetor,
                                 nomeSistema = i.nomeSistema,
                                 nomeSubSistema = i.nomeSubSistema,
                                 obsLocal = i.obsLocal,
                                 patrimonio = i.patrimonio,
                                 serie = i.serie,
                                 autonumeroFuncionario = 0,
                                 nomeFuncionario = "",
                                 autonumeroProfissao = 0,
                                 nomeProfissao = "",
                                 dataInicio = null,
                                 dataFim = null,
                                 totalHoras = TimeSpan.Parse("00:00")



                             }).ToList();


                    //manutEntities context = new manutEntities();
                    //foreach (var e in lista)
                    //{
                    //    context.checklisthistorico.Add(e);
                    //}

                    //context.SaveChanges();

                    dc.checklisthistorico.AddRange(lista);
                    dc.SaveChanges();

                    //var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S") select i).ToList();
                    var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S").ToList()
                                  join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                  where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                  select i).ToList();

                    var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente && i.cancelado != "S") select i).ToList();

                    var lista5 = (from k in lista3
                                  join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                                  select new checklisthistitem
                                  {
                                      anoMes = anoMes,
                                      autonumeroContrato = autonumeroCliente,
                                      autonumeroEquipamento = k.autonumero,
                                      autonumeroSubSistema = k.autonumeroSubSistema,
                                      nomeSubSistema = k.nomeSubSistema,
                                      autonumeroCheckList = i.autonumero,
                                      executou = "N",
                                      a = i.a,
                                      e = i.e,
                                      b = i.b,
                                      d = i.d,
                                      m = i.m,
                                      q = i.q,
                                      s = i.s,
                                      t = i.t,
                                      nome = i.nome,
                                      item = i.item

                                  }).ToList();


                    lista3.Clear();
                    lista4.Clear();

                    //manutEntities context = new manutEntities();
                    //foreach (var e in lista5)
                    //{
                    //    context.checklisthistitem.Add(e);
                    //}

                    //context.SaveChanges();

                    dc.checklisthistitem.AddRange(lista5);
                    dc.SaveChanges();

                    this.SalvarPmocEquipamento2(autonumeroCliente, ano, mes);

                    return "";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.ToString();
                    }
                    return message;
                }

            }

        }
        [HttpGet]
        public string SalvarPmocEquipamento2(int autonumeroCliente, int ano, int mes)
        {
            var c = 1;
            //var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            //var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
            //var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {
                try
                {

                    //Apagar PMOC anterior -----------------------------------------------------------------------------
                    var lista = dc.checklisthistoriconrofolha.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();

                    dc.checklisthistoriconrofolha.RemoveRange(lista);
                    dc.SaveChanges();

                    lista.Clear();

                    var lista2 = dc.checklisthistitemnrofolha.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.checklisthistitemnrofolha.RemoveRange(lista2);
                    dc.SaveChanges();

                    lista2.Clear();

                    //var lista33 = dc.checklisthistorico.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    //dc.checklisthistorico.RemoveRange(lista33);
                    //dc.SaveChanges();

                    //lista33.Clear();

                    //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------

                    long nroFolha = 0;
                    var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S") select i).OrderBy(p => p.contadorPmocEquipamento).ToList();
                    foreach (var item in lista3)
                    {

                        if (nroFolha == item.contadorPmocEquipamento)
                        {
                            continue;
                        }

                        nroFolha = (long)item.contadorPmocEquipamento;

                        long equip1 = 0;
                        long equip2 = 0;
                        long equip3 = 0;
                        long equip4 = 0;
                        long equip5 = 0;
                        long equip6 = 0;
                        long equip7 = 0;
                        long equip8 = 0;


                        var x = lista3.Where(p => p.contadorPmocEquipamento == nroFolha).OrderBy(p => p.autonumero).ToList();
                        var i = 1;
                        foreach (var item2 in x)
                        {
                            switch (i)
                            {
                                case 1:
                                    equip1 = item2.autonumero;
                                    break;
                                case 2:
                                    equip2 = item2.autonumero;
                                    break;
                                case 3:
                                    equip3 = item2.autonumero;
                                    break;
                                case 4:
                                    equip4 = item2.autonumero;
                                    break;
                                case 5:
                                    equip5 = item2.autonumero;
                                    break;
                                case 6:
                                    equip6 = item2.autonumero;
                                    break;
                                case 7:
                                    equip7 = item2.autonumero;
                                    break;
                                case 8:
                                    equip8 = item2.autonumero;
                                    break;
                                default:
                                    break;
                            }
                            i++;
                        }

                        var z = new checklisthistoriconrofolha
                        {
                            anoMes = anoMes,
                            autonumeroCliente = autonumeroCliente,
                            contadorPmocEquipamento = nroFolha,
                            equip1 = (int)equip1,
                            equip2 = (int)equip2,
                            equip3 = (int)equip3,
                            equip4 = (int)equip4,
                            equip5 = (int)equip5,
                            equip6 = (int)equip6,
                            equip7 = (int)equip7,
                            equip8 = (int)equip8,
                        };

                        dc.checklisthistoriconrofolha.AddOrUpdate(z);
                        dc.SaveChanges();


                    }



                    //dc.checklisthistorico.AddRange(lista);
                    //dc.SaveChanges();

                    var lista6 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S")
                                  select new
                                  {
                                      i.autonumeroSubSistema,
                                      i.contadorPmocEquipamento
                                  }).ToList().Distinct();

                    var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente && i.cancelado != "S") select i).ToList();

                    var lista5 = (from k in lista6
                                  join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                                  select new checklisthistitemnrofolha
                                  {
                                      anoMes = anoMes,
                                      autonumeroCliente = autonumeroCliente,
                                      autonumeroCheckList = i.autonumero,
                                      autonumeroSubSistema = i.autonumeroSubsistema,
                                      a = i.a,
                                      e = i.e,
                                      b = i.b,
                                      d = i.d,
                                      m = i.m,
                                      q = i.q,
                                      s = i.s,
                                      t = i.t,
                                      nome = i.nome,
                                      item = i.item,
                                      contadorPmocEquipamento = k.contadorPmocEquipamento,
                                      equip1 = "",
                                      equip2 = "",
                                      equip3 = "",
                                      equip4 = "",
                                      equip5 = "",
                                      equip6 = "",
                                      equip7 = "",
                                      equip8 = "",



                                  }).ToList();



                    lista4.Clear();


                    dc.checklisthistitemnrofolha.AddRange(lista5);
                    dc.SaveChanges();


                    return "";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.ToString();
                    }
                    return message;
                }

            }

        }



        [HttpPost]
        public string SalvarPmocEquipamentoNovo(int autonumeroCliente, int ano, int mes)
        {
            var c = 1;

            //var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            //var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
            //var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {
                try
                {

                    //Apagar PMOC anterior -----------------------------------------------------------------------------
                    var lista = dc.checklisthistorico.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    if (lista.Count > 0)
                    {
                        if (lista[0].fechado == "S")
                        {
                            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Erro - PMOC Fechado"));
                        }
                    }
                    dc.checklisthistorico.RemoveRange(lista);
                    dc.SaveChanges();

                    lista.Clear();

                    var lista2 = dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.checklisthistitem.RemoveRange(lista2);
                    dc.SaveChanges();

                    lista2.Clear();

                    //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------


                    // i.dataPrevista != null Apenas os Equipamentos filtrados (ano,mes,sem .... ) no cadastro -------------------------------------


                    switch (mes)
                    {
                        case 1:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes01,
                                         bimestral = i.bim01,
                                         trimestral = i.tri01,
                                         semestral = i.sem01,
                                         anual = i.ano01,
                                     }).ToList();
                            break;
                        case 2:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes02,
                                         bimestral = i.bim02,
                                         trimestral = i.tri02,
                                         semestral = i.sem02,
                                         anual = i.ano02,
                                     }).ToList();
                            break;
                        case 3:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes03,
                                         bimestral = i.bim03,
                                         trimestral = i.tri03,
                                         semestral = i.sem03,
                                         anual = i.ano01,
                                     }).ToList();
                            break;
                        case 4:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes04,
                                         bimestral = i.bim04,
                                         trimestral = i.tri04,
                                         semestral = i.sem04,
                                         anual = i.ano04,
                                     }).ToList();
                            break;
                        case 5:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes05,
                                         bimestral = i.bim05,
                                         trimestral = i.tri05,
                                         semestral = i.sem05,
                                         anual = i.ano05,
                                     }).ToList();
                            break;
                        case 6:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes06,
                                         bimestral = i.bim06,
                                         trimestral = i.tri06,
                                         semestral = i.sem06,
                                         anual = i.ano06,
                                     }).ToList();
                            break;
                        case 7:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes07,
                                         bimestral = i.bim07,
                                         trimestral = i.tri07,
                                         semestral = i.sem07,
                                         anual = i.ano07,
                                     }).ToList();
                            break;
                        case 8:

                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes08,
                                         bimestral = i.bim08,
                                         trimestral = i.tri08,
                                         semestral = i.sem08,
                                         anual = i.ano08,
                                     }).ToList();
                            break;
                        case 9:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes09,
                                         bimestral = i.bim09,
                                         trimestral = i.tri09,
                                         semestral = i.sem09,
                                         anual = i.ano09,
                                     }).ToList();
                            break;

                        case 10:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes10,
                                         bimestral = i.bim10,
                                         trimestral = i.tri10,
                                         semestral = i.sem10,
                                         anual = i.ano10,
                                     }).ToList();
                            break;

                        case 11:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes11,
                                         bimestral = i.bim11,
                                         trimestral = i.tri11,
                                         semestral = i.sem11,
                                         anual = i.ano11,
                                     }).ToList();
                            break;

                        case 12:
                            lista = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                     join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                     where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                     select new checklisthistorico
                                     {
                                         anoMes = anoMes,
                                         autonumeroCliente = autonumeroCliente,
                                         autonumeroEquipamento = i.autonumero,
                                         autonumeroLocalFisico = i.autonumeroLocalFisico,
                                         autonumeroPredio = i.autonumeroPredio,
                                         autonumeroSetor = i.autonumeroSetor,
                                         autonumeroSistema = i.autonumeroSistema,
                                         autonumeroSubSistema = i.autonumeroSubSistema,
                                         cancelado = "N",
                                         capacidade = i.capacidade,
                                         contadorPmocEquipamento = i.contadorPmocEquipamento,
                                         dataPrevista = i.dataPrevista,
                                         endereco = i.endereco,
                                         fabricante = i.fabricante,
                                         fechado = "N",
                                         grupo = i.grupo,
                                         mesManuAno = i.mesManuAno,
                                         mesManuSemestre = i.mesManuSemestre,
                                         modelo = i.modelo,
                                         nomeCliente = i.nomeCliente,
                                         nomeLocalFisico = i.nomeLocalFisico,
                                         nomePredio = i.nomePredio,
                                         nomeSetor = i.nomeSetor,
                                         nomeSistema = i.nomeSistema,
                                         nomeSubSistema = i.nomeSubSistema,
                                         obsLocal = i.obsLocal,
                                         patrimonio = i.patrimonio,
                                         serie = i.serie,
                                         autonumeroFuncionario = 0,
                                         nomeFuncionario = "",
                                         autonumeroProfissao = 0,
                                         nomeProfissao = "",
                                         dataInicio = null,
                                         dataFim = null,
                                         totalHoras = TimeSpan.Parse("00:00"),
                                         mensal = i.mes12,
                                         bimestral = i.bim12,
                                         trimestral = i.tri12,
                                         semestral = i.sem12,
                                         anual = i.ano12,
                                     }).ToList();
                            break;
                        default:
                            break;
                    }


                    dc.checklisthistorico.AddRange(lista);
                    dc.SaveChanges();

                    c = 1;

                    var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.dataPrevista != null && i.cancelado != "S").ToList()
                                  join s in dc.tb_subsistemacliente on i.autonumeroSubSistema equals s.autonumeroSubsistema
                                  where s.chkTodoMes == 1 && s.autonumeroCliente == i.autonumeroCliente
                                  select i).ToList();

                    //var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S") select i).ToList();
                    var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente && i.cancelado != "S") select i).ToList();

                    var lista5 = (from k in lista3
                                  join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                                  select new checklisthistitem
                                  {
                                      anoMes = anoMes,
                                      autonumeroContrato = autonumeroCliente,
                                      autonumeroEquipamento = k.autonumero,
                                      autonumeroSubSistema = k.autonumeroSubSistema,
                                      nomeSubSistema = k.nomeSubSistema,
                                      autonumeroCheckList = i.autonumero,
                                      executou = "N",
                                      a = i.a,
                                      e = i.e,
                                      b = i.b,
                                      d = i.d,
                                      m = i.m,
                                      q = i.q,
                                      s = i.s,
                                      t = i.t,
                                      nome = i.nome,
                                      item = i.item

                                  }).ToList();


                    lista3.Clear();
                    lista4.Clear();

                    //manutEntities context = new manutEntities();
                    //foreach (var e in lista5)
                    //{
                    //    context.checklisthistitem.Add(e);
                    //}

                    //context.SaveChanges();

                    dc.checklisthistitem.AddRange(lista5);
                    dc.SaveChanges();

                    SalvarPmocEquipamento2Novo(autonumeroCliente, ano, mes);
                    return "";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.ToString();
                    }
                    return message;
                }

            }

        }


        [HttpGet]
        public string SalvarPmocEquipamento2Novo(int autonumeroCliente, int ano, int mes)
        {
            var c = 1;
            //var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            //var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
            //var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {
                try
                {

                    //Apagar PMOC anterior -----------------------------------------------------------------------------
                    var lista = dc.checklisthistoriconrofolha.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();

                    dc.checklisthistoriconrofolha.RemoveRange(lista);
                    dc.SaveChanges();

                    lista.Clear();

                    var lista2 = dc.checklisthistitemnrofolha.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.checklisthistitemnrofolha.RemoveRange(lista2);
                    dc.SaveChanges();

                    lista2.Clear();

                    //var lista33 = dc.checklisthistorico.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    //dc.checklisthistorico.RemoveRange(lista33);
                    //dc.SaveChanges();

                    //lista33.Clear();

                    //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------

                    long nroFolha = 0;
                    var lista3 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S") select i).OrderBy(p => p.contadorPmocEquipamento).ToList();
                    foreach (var item in lista3)
                    {

                        if (nroFolha == item.contadorPmocEquipamento)
                        {
                            continue;
                        }

                        nroFolha = (long)item.contadorPmocEquipamento;

                        long equip1 = 0;
                        long equip2 = 0;
                        long equip3 = 0;
                        long equip4 = 0;
                        long equip5 = 0;
                        long equip6 = 0;
                        long equip7 = 0;
                        long equip8 = 0;


                        var x = lista3.Where(p => p.contadorPmocEquipamento == nroFolha).OrderBy(p => p.autonumero).ToList();
                        var i = 1;
                        sbyte m = 0;
                        sbyte b = 0;
                        sbyte t = 0;
                        sbyte s = 0;
                        sbyte a = 0;
                        foreach (var item2 in x)
                        {


                            switch (mes)
                            {
                                case 1:
                                    if (item2.mes01 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim01 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri01 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem01 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano01 == 1)
                                    {
                                        a = 1;
                                    }
                                    break;
                                case 2:
                                    if (item2.mes02 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim02 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri02 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem02 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano02 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 3:
                                    if (item2.mes03 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim03 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri03 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem03 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano03 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 4:

                                    if (item2.mes04 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim04 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri04 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem04 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano04 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 5:
                                    if (item2.mes05 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim05 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri05 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem05 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano05 == 1)
                                    {
                                        a = 1;
                                    }


                                    break;

                                case 6:

                                    if (item2.mes06 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim06 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri06 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem06 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano06 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 7:

                                    if (item2.mes07 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim07 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri07 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem07 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano07 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 8:
                                    if (item2.mes08 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim08 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri08 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem08 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano08 == 1)
                                    {
                                        a = 1;
                                    }


                                    break;

                                case 9:

                                    if (item2.mes09 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim09 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri09 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem09 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano09 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;

                                case 10:

                                    if (item2.mes10 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim10 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri10 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem10 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano10 == 1)
                                    {
                                        a = 1;
                                    }



                                    break;

                                case 11:

                                    if (item2.mes11 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim11 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri11 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem11 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano11 == 1)
                                    {
                                        a = 1;
                                    }


                                    break;

                                case 12:

                                    if (item2.mes12 == 1)
                                    {
                                        m = 1;
                                    }
                                    if (item2.bim12 == 1)
                                    {
                                        b = 1;
                                    }
                                    if (item2.tri12 == 1)
                                    {
                                        t = 1;
                                    }
                                    if (item2.sem12 == 1)
                                    {
                                        s = 1;
                                    }
                                    if (item2.ano12 == 1)
                                    {
                                        a = 1;
                                    }

                                    break;
                                default:
                                    break;
                            }

                            switch (i)
                            {
                                case 1:
                                    equip1 = item2.autonumero;
                                    break;
                                case 2:
                                    equip2 = item2.autonumero;
                                    break;
                                case 3:
                                    equip3 = item2.autonumero;
                                    break;
                                case 4:
                                    equip4 = item2.autonumero;
                                    break;
                                case 5:
                                    equip5 = item2.autonumero;
                                    break;
                                case 6:
                                    equip6 = item2.autonumero;
                                    break;
                                case 7:
                                    equip7 = item2.autonumero;
                                    break;
                                case 8:
                                    equip8 = item2.autonumero;
                                    break;
                                default:
                                    break;
                            }
                            i++;
                        }

                        var z = new checklisthistoriconrofolha
                        {
                            anoMes = anoMes,
                            autonumeroCliente = autonumeroCliente,
                            contadorPmocEquipamento = nroFolha,
                            equip1 = (int)equip1,
                            equip2 = (int)equip2,
                            equip3 = (int)equip3,
                            equip4 = (int)equip4,
                            equip5 = (int)equip5,
                            equip6 = (int)equip6,
                            equip7 = (int)equip7,
                            equip8 = (int)equip8,
                            mensal = m,
                            bimestral = b,
                            trimestral = t,
                            semestral = s,
                            anual = a

                        };

                        dc.checklisthistoriconrofolha.AddOrUpdate(z);
                        dc.SaveChanges();


                    }



                    //dc.checklisthistorico.AddRange(lista);
                    //dc.SaveChanges();

                    var lista6 = (from i in dc.tb_cadastro.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S")
                                  select new
                                  {
                                      i.autonumeroSubSistema,
                                      i.contadorPmocEquipamento
                                  }).ToList().Distinct();

                    var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente && i.cancelado != "S") select i).ToList();

                    var lista5 = (from k in lista6
                                  join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                                  select new checklisthistitemnrofolha
                                  {
                                      anoMes = anoMes,
                                      autonumeroCliente = autonumeroCliente,
                                      autonumeroCheckList = i.autonumero,
                                      autonumeroSubSistema = i.autonumeroSubsistema,
                                      a = i.a,
                                      e = i.e,
                                      b = i.b,
                                      d = i.d,
                                      m = i.m,
                                      q = i.q,
                                      s = i.s,
                                      t = i.t,
                                      nome = i.nome,
                                      item = i.item,
                                      contadorPmocEquipamento = k.contadorPmocEquipamento,
                                      equip1 = "",
                                      equip2 = "",
                                      equip3 = "",
                                      equip4 = "",
                                      equip5 = "",
                                      equip6 = "",
                                      equip7 = "",
                                      equip8 = "",



                                  }).ToList();



                    lista4.Clear();


                    dc.checklisthistitemnrofolha.AddRange(lista5);
                    dc.SaveChanges();


                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.ToString();
                    }
                    return message;
                }

            }

            CalcularQtdeSubSistema(autonumeroCliente, anoMes);
            return "";
        }



        [HttpGet]
        public void CalcularQtdeSubSistema(int autonumeroCliente, string anoMes)
        {
            var message = "";
            try
            {
                var c = 1;
                using (var dc = new manutEntities())
                {

                    var cli = dc.tb_cliente.Find(autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (cli == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Erro: Tabela Contrato Não Encontrada"));
                    }

                    var nomeCliente = cli.nome;


                    var lista2 = dc.subsistemaqtdepmoc.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.subsistemaqtdepmoc.RemoveRange(lista2);
                    dc.SaveChanges();

                    var csql = new StringBuilder();


                    csql.Append("SELECT autonumeroCliente, 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa' as nomeCliente, autonumeroSistema, nomeSistema,  autonumeroSubSistema, nome as  nomeSubSistema,  ");
                    csql.Append("0 as mensal,0 as bimestral, 0 as trimestral,0 as semestral, 0 as anual FROM manut.tb_subsistemacliente where  autonumeroCliente = " + autonumeroCliente);
                    var todos = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();

                    csql.Clear();

                    //var mensal = dc.checklisthistorico.Where(p => p.autonumeroCliente == autonumeroCliente && p.cancelado != "S" && p.mensal == 1 && p.anoMes == anoMes).ToList().Count();

                    csql.Append("SELECT  autonumeroSubSistema, ");
                    csql.Append("count(mensal) as mensal  FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  mensal = 1 and anomes = '" + anoMes + "' group by  autonumeroSubSistema ");


                    var mensal = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();

                    var m = (from k in todos
                             join x in mensal on k.autonumeroSubSistema equals x.autonumeroSubSistema
                             select new { x.autonumeroSubSistema, x.mensal }).ToList();

                    foreach (var j in m)
                    {
                        var k = todos.Find(l => l.autonumeroSubSistema == j.autonumeroSubSistema);
                        if (k != null)
                        {
                            k.mensal = j.mensal;
                        }
                    }

                    csql.Clear();
                    //csql.Append("SELECT autonumeroCliente,  nomeCliente, autonumeroSistema, nomeSistema,  autonumeroSubSistema, nomeSubSistema, ");
                    //csql.Append("count(bimestral) as bimestral FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and bimestral = 1 and anomes = '" + anoMes + "'  ");

                    csql.Append("SELECT  autonumeroSubSistema, ");
                    csql.Append("count(bimestral) as bimestral  FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  bimestral = 1 and anomes = '" + anoMes + "' group by  autonumeroSubSistema ");


                    var bimestral = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();

                    var b = (from k in todos
                             join x in bimestral on k.autonumeroSubSistema equals x.autonumeroSubSistema
                             select new { x.autonumeroSubSistema, x.bimestral }).ToList();

                    foreach (var j in b)
                    {
                        var k = todos.Find(l => l.autonumeroSubSistema == j.autonumeroSubSistema);
                        if (k != null)
                        {

                            k.bimestral = j.bimestral;
                        }
                    }

                    csql.Clear();
                    //csql.Append("SELECT autonumeroCliente, nomeCliente, autonumeroSistema, nomeSistema,  autonumeroSubSistema, nomeSubSistema, ");
                    //csql.Append("count(trimestral) as trimestral FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  trimestral = 1 and anomes = '" + anoMes + "'  ");

                    csql.Append("SELECT  autonumeroSubSistema, ");
                    csql.Append("count(trimestral) as trimestral  FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  trimestral = 1 and anomes = '" + anoMes + "' group by  autonumeroSubSistema ");


                    var trimestral = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();


                    var t = (from k in todos
                             join x in trimestral on k.autonumeroSubSistema equals x.autonumeroSubSistema
                             select new { x.autonumeroSubSistema, x.trimestral }).ToList();

                    foreach (var j in t)
                    {
                        var k = todos.Find(l => l.autonumeroSubSistema == j.autonumeroSubSistema);
                        if (k != null)
                        {

                            k.trimestral = j.trimestral;
                        }
                    }

                    csql.Clear();
                    //csql.Append("SELECT autonumeroCliente, nomeCliente, autonumeroSistema, nomeSistema,  autonumeroSubSistema, nomeSubSistema, ");
                    //csql.Append("count(semestral) as semestral FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and semestral = 1 and anomes = '" + anoMes + "'  ");
                    csql.Append("SELECT  autonumeroSubSistema, ");
                    csql.Append("count(semestral) as semestral  FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  semestral = 1 and anomes = '" + anoMes + "' group by  autonumeroSubSistema ");


                    var semestral = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();


                    var s = (from k in todos
                             join x in semestral on k.autonumeroSubSistema equals x.autonumeroSubSistema
                             select new { x.autonumeroSubSistema, x.semestral }).ToList();

                    foreach (var j in s)
                    {
                        var k = todos.Find(l => l.autonumeroSubSistema == j.autonumeroSubSistema);

                        if (k != null)
                        {

                            k.semestral = j.semestral;
                        }
                    }

                    csql.Clear();
                    //csql.Append("SELECT autonumeroCliente, nomeCliente, autonumeroSistema, nomeSistema,  autonumeroSubSistema, nomeSubSistema, ");
                    //csql.Append("count(anual) as anual FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  anual = 1 and anomes = '" + anoMes + "'  ");

                    csql.Append("SELECT  autonumeroSubSistema, ");
                    csql.Append("count(anual) as anual  FROM manut.checklisthistorico where  autonumeroCliente = " + autonumeroCliente + " and  anual = 1 and anomes = '" + anoMes + "' group by  autonumeroSubSistema ");


                    var anual = dc.Database.SqlQuery<avaliacao>(csql.ToString()).ToList();


                    var a = (from k in todos
                             join x in anual on k.autonumeroSubSistema equals x.autonumeroSubSistema
                             select new { x.autonumeroSubSistema, x.anual }).ToList();

                    foreach (var j in a)
                    {
                        var k = todos.Find(l => l.autonumeroSubSistema == j.autonumeroSubSistema);
                        if (k != null)
                        {

                            k.anual = j.anual;
                        }
                    }

                    List<subsistemaqtdepmoc> lpmoc = new List<subsistemaqtdepmoc>();

                    foreach (var j in todos)
                    {
                        var k = new subsistemaqtdepmoc();
                        k.anoMes = anoMes;
                        k.autonumeroCliente = autonumeroCliente;
                        k.autonumeroSistema = j.autonumeroSistema;
                        k.autonumeroSubSistema = j.autonumeroSubSistema;
                        k.mensal = j.mensal;
                        k.bimestral = j.bimestral;
                        k.trimestral = j.trimestral;
                        k.semestral = j.semestral;
                        k.anual = j.anual;
                        k.nomeCliente = nomeCliente;
                        k.nomeSistema = j.nomeSistema;
                        k.nomeSubSistema = j.nomeSubSistema;


                        if (k.mensal == null) k.mensal = 0;
                        if (k.bimestral == null) k.bimestral = 0;
                        if (k.trimestral == null) k.trimestral = 0;
                        if (k.semestral == null) k.semestral = 0;
                        if (k.anual == null) k.anual = 0;


                        lpmoc.Add(k);

                    }


                    dc.subsistemaqtdepmoc.AddRange(lpmoc);
                    dc.SaveChanges();




                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.ToString();
                }
                Debug.WriteLine(" Exception ex: " + message);
            }
        }

        public class avaliacao
        {

            public int? autonumeroCliente { get; set; }
            public int? autonumeroSistema { get; set; }
            public int? autonumeroSubSistema { get; set; }
            //public int? qtde { get; set; }
            public int? mensal { get; set; }
            public int? bimestral { get; set; }
            public int? trimestral { get; set; }
            public int? semestral { get; set; }
            public int? anual { get; set; }
            public string nomeCliente { get; set; }
            public string nomeSistema { get; set; }
            public string nomeSubSistema { get; set; }

        };


        [HttpGet]
        public IEnumerable<tb_cadastro> EquipamentoCalendario(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
        int autonumeroLocalFisico, int autonumeroSetor)

        {
            var c = 1;
            using (var dc = new manutEntities())

            {
                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {

                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();

                    }
                    else
                    {
                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();

                    }
                    else
                    {
                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();

                    }
                    else
                    {
                        var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                        return user1.ToList();
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema).OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                    return user1.ToList();

                }
                else
                {
                    var user1 = from p in dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").OrderBy(p => p.nomePredio).ThenBy(p => p.nomeSetor).ThenBy(p => p.nomeLocalFisico).ThenBy(p => p.nomeSistema).ThenBy(p => p.nomeSubSistema) select p;

                    return user1.ToList();
                }

            }

        }


        [HttpGet]
        public IEnumerable<tb_cadastro> GetEquipamentoEmOrdemVisitaMes(long autonumeroCliente, int? autonumeroSubSistema, int mes)
        {

            var csql = new StringBuilder();

            var mesManut = " AND ( mes" + mes.ToString().PadLeft(2, '0') + " = 1 ";
            var bimManut = " OR bim" + mes.ToString().PadLeft(2, '0') + " = 1 ";
            var triManut = " OR tri" + mes.ToString().PadLeft(2, '0') + " = 1 ";
            var semManut = " OR sem" + mes.ToString().PadLeft(2, '0') + " = 1 ";
            var anoManut = " OR ano" + mes.ToString().PadLeft(2, '0') + " = 1 )";

            var filtro = mesManut + bimManut + triManut + semManut + anoManut;

            csql.Append("SELECT * ");
            csql.Append("FROM manut.tb_cadastro  ");
            csql.Append("WHERE cancelado != 'S' and autonumeroCliente =  " + autonumeroCliente + "  " + filtro + " ");
            csql.Append("AND autonumeroSubSistema =  " + autonumeroSubSistema + " ");
            csql.Append("ORDER BY nomeSubSistema,nomePredio,nomeSetor,nomeLocalFisico,grupo; ");

            using (var dc = new manutEntities())
            {
                var c1 = csql.ToString();
                Debug.WriteLine(c1);
                var user = dc.Database.SqlQuery<tb_cadastro>(csql.ToString()).ToList();
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string AlterarMesPmoc()
        {

            var c = 1;
            using (var dc = new manutEntities())
            {


                var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
                if (string.IsNullOrEmpty(auto2))
                {
                    auto2 = "0";
                }
                var autonumero = Convert.ToInt32(auto2);



                var linha = dc.tb_cadastro.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha == null || linha.cancelado == "S")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Alterar Mês Equipamento"));
                }

                linha.mes01 = Convert.ToSByte(HttpContext.Current.Request.Form["mes01"].ToString());
                linha.mes02 = Convert.ToSByte(HttpContext.Current.Request.Form["mes02"].ToString());
                linha.mes03 = Convert.ToSByte(HttpContext.Current.Request.Form["mes03"].ToString());
                linha.mes04 = Convert.ToSByte(HttpContext.Current.Request.Form["mes04"].ToString());
                linha.mes05 = Convert.ToSByte(HttpContext.Current.Request.Form["mes05"].ToString());
                linha.mes06 = Convert.ToSByte(HttpContext.Current.Request.Form["mes06"].ToString());
                linha.mes07 = Convert.ToSByte(HttpContext.Current.Request.Form["mes07"].ToString());
                linha.mes08 = Convert.ToSByte(HttpContext.Current.Request.Form["mes08"].ToString());
                linha.mes09 = Convert.ToSByte(HttpContext.Current.Request.Form["mes09"].ToString());
                linha.mes10 = Convert.ToSByte(HttpContext.Current.Request.Form["mes10"].ToString());
                linha.mes11 = Convert.ToSByte(HttpContext.Current.Request.Form["mes11"].ToString());
                linha.mes12 = Convert.ToSByte(HttpContext.Current.Request.Form["mes12"].ToString());


                linha.bim01 = Convert.ToSByte(HttpContext.Current.Request.Form["bim01"].ToString());
                linha.bim02 = Convert.ToSByte(HttpContext.Current.Request.Form["bim02"].ToString());
                linha.bim03 = Convert.ToSByte(HttpContext.Current.Request.Form["bim03"].ToString());
                linha.bim04 = Convert.ToSByte(HttpContext.Current.Request.Form["bim04"].ToString());
                linha.bim05 = Convert.ToSByte(HttpContext.Current.Request.Form["bim05"].ToString());
                linha.bim06 = Convert.ToSByte(HttpContext.Current.Request.Form["bim06"].ToString());
                linha.bim07 = Convert.ToSByte(HttpContext.Current.Request.Form["bim07"].ToString());
                linha.bim08 = Convert.ToSByte(HttpContext.Current.Request.Form["bim08"].ToString());
                linha.bim09 = Convert.ToSByte(HttpContext.Current.Request.Form["bim09"].ToString());
                linha.bim10 = Convert.ToSByte(HttpContext.Current.Request.Form["bim10"].ToString());
                linha.bim11 = Convert.ToSByte(HttpContext.Current.Request.Form["bim11"].ToString());
                linha.bim12 = Convert.ToSByte(HttpContext.Current.Request.Form["bim12"].ToString());


                linha.tri01 = Convert.ToSByte(HttpContext.Current.Request.Form["tri01"].ToString());
                linha.tri02 = Convert.ToSByte(HttpContext.Current.Request.Form["tri02"].ToString());
                linha.tri03 = Convert.ToSByte(HttpContext.Current.Request.Form["tri03"].ToString());
                linha.tri04 = Convert.ToSByte(HttpContext.Current.Request.Form["tri04"].ToString());
                linha.tri05 = Convert.ToSByte(HttpContext.Current.Request.Form["tri05"].ToString());
                linha.tri06 = Convert.ToSByte(HttpContext.Current.Request.Form["tri06"].ToString());
                linha.tri07 = Convert.ToSByte(HttpContext.Current.Request.Form["tri07"].ToString());
                linha.tri08 = Convert.ToSByte(HttpContext.Current.Request.Form["tri08"].ToString());
                linha.tri09 = Convert.ToSByte(HttpContext.Current.Request.Form["tri09"].ToString());
                linha.tri10 = Convert.ToSByte(HttpContext.Current.Request.Form["tri10"].ToString());
                linha.tri11 = Convert.ToSByte(HttpContext.Current.Request.Form["tri11"].ToString());
                linha.tri12 = Convert.ToSByte(HttpContext.Current.Request.Form["tri12"].ToString());


                linha.sem01 = Convert.ToSByte(HttpContext.Current.Request.Form["sem01"].ToString());
                linha.sem02 = Convert.ToSByte(HttpContext.Current.Request.Form["sem02"].ToString());
                linha.sem03 = Convert.ToSByte(HttpContext.Current.Request.Form["sem03"].ToString());
                linha.sem04 = Convert.ToSByte(HttpContext.Current.Request.Form["sem04"].ToString());
                linha.sem05 = Convert.ToSByte(HttpContext.Current.Request.Form["sem05"].ToString());
                linha.sem06 = Convert.ToSByte(HttpContext.Current.Request.Form["sem06"].ToString());
                linha.sem07 = Convert.ToSByte(HttpContext.Current.Request.Form["sem07"].ToString());
                linha.sem08 = Convert.ToSByte(HttpContext.Current.Request.Form["sem08"].ToString());
                linha.sem09 = Convert.ToSByte(HttpContext.Current.Request.Form["sem09"].ToString());
                linha.sem10 = Convert.ToSByte(HttpContext.Current.Request.Form["sem10"].ToString());
                linha.sem11 = Convert.ToSByte(HttpContext.Current.Request.Form["sem11"].ToString());
                linha.sem12 = Convert.ToSByte(HttpContext.Current.Request.Form["sem12"].ToString());


                linha.ano01 = Convert.ToSByte(HttpContext.Current.Request.Form["ano01"].ToString());
                linha.ano02 = Convert.ToSByte(HttpContext.Current.Request.Form["ano02"].ToString());
                linha.ano03 = Convert.ToSByte(HttpContext.Current.Request.Form["ano03"].ToString());
                linha.ano04 = Convert.ToSByte(HttpContext.Current.Request.Form["ano04"].ToString());
                linha.ano05 = Convert.ToSByte(HttpContext.Current.Request.Form["ano05"].ToString());
                linha.ano06 = Convert.ToSByte(HttpContext.Current.Request.Form["ano06"].ToString());
                linha.ano07 = Convert.ToSByte(HttpContext.Current.Request.Form["ano07"].ToString());
                linha.ano08 = Convert.ToSByte(HttpContext.Current.Request.Form["ano08"].ToString());
                linha.ano09 = Convert.ToSByte(HttpContext.Current.Request.Form["ano09"].ToString());
                linha.ano10 = Convert.ToSByte(HttpContext.Current.Request.Form["ano10"].ToString());
                linha.ano11 = Convert.ToSByte(HttpContext.Current.Request.Form["ano11"].ToString());
                linha.ano12 = Convert.ToSByte(HttpContext.Current.Request.Form["ano12"].ToString());


                dc.tb_cadastro.AddOrUpdate(linha);
                dc.SaveChanges();

                return "";

            }


        }


        [HttpGet]
        public string AlterarMesPmocTodosEquipamentos(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
                      int autonumeroLocalFisico, int autonumeroSetor, int mesParaAlterar, int condicaoAlterar)

        {
            var c = 1;

            var cond = Convert.ToSByte(condicaoAlterar);
            using (var dc = new manutEntities())
            {

                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }



                        });
                        dc.SaveChanges();

                        return "";

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.mes01 = cond;
                                    break;
                                case 2:
                                    x.mes02 = cond;
                                    break;
                                case 3:
                                    x.mes03 = cond;
                                    break;
                                case 4:
                                    x.mes04 = cond;
                                    break;
                                case 5:
                                    x.mes05 = cond;
                                    break;
                                case 6:
                                    x.mes06 = cond;
                                    break;
                                case 7:
                                    x.mes07 = cond;
                                    break;
                                case 8:
                                    x.mes08 = cond;
                                    break;

                                case 9:
                                    x.mes09 = cond;
                                    break;
                                case 10:
                                    x.mes10 = cond;
                                    break;
                                case 11:
                                    x.mes11 = cond;
                                    break;
                                case 12:
                                    x.mes12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.mes01 = cond;
                                break;
                            case 2:
                                x.mes02 = cond;
                                break;
                            case 3:
                                x.mes03 = cond;
                                break;
                            case 4:
                                x.mes04 = cond;
                                break;
                            case 5:
                                x.mes05 = cond;
                                break;
                            case 6:
                                x.mes06 = cond;
                                break;
                            case 7:
                                x.mes07 = cond;
                                break;
                            case 8:
                                x.mes08 = cond;
                                break;

                            case 9:
                                x.mes09 = cond;
                                break;
                            case 10:
                                x.mes10 = cond;
                                break;
                            case 11:
                                x.mes11 = cond;
                                break;
                            case 12:
                                x.mes12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";


                }
                else
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.mes01 = cond;
                                break;
                            case 2:
                                x.mes02 = cond;
                                break;
                            case 3:
                                x.mes03 = cond;
                                break;
                            case 4:
                                x.mes04 = cond;
                                break;
                            case 5:
                                x.mes05 = cond;
                                break;
                            case 6:
                                x.mes06 = cond;
                                break;
                            case 7:
                                x.mes07 = cond;
                                break;
                            case 8:
                                x.mes08 = cond;
                                break;

                            case 9:
                                x.mes09 = cond;
                                break;
                            case 10:
                                x.mes10 = cond;
                                break;
                            case 11:
                                x.mes11 = cond;
                                break;
                            case 12:
                                x.mes12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";
                }



            }


        }


        [HttpGet]
        public string AlterarBimPmocTodosEquipamentos(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
              int autonumeroLocalFisico, int autonumeroSetor, int mesParaAlterar, int condicaoAlterar)

        {
            var c = 1;

            var cond = Convert.ToSByte(condicaoAlterar);
            using (var dc = new manutEntities())
            {

                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }



                        });
                        dc.SaveChanges();

                        return "";

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.bim01 = cond;
                                    break;
                                case 2:
                                    x.bim02 = cond;
                                    break;
                                case 3:
                                    x.bim03 = cond;
                                    break;
                                case 4:
                                    x.bim04 = cond;
                                    break;
                                case 5:
                                    x.bim05 = cond;
                                    break;
                                case 6:
                                    x.bim06 = cond;
                                    break;
                                case 7:
                                    x.bim07 = cond;
                                    break;
                                case 8:
                                    x.bim08 = cond;
                                    break;

                                case 9:
                                    x.bim09 = cond;
                                    break;
                                case 10:
                                    x.bim10 = cond;
                                    break;
                                case 11:
                                    x.bim11 = cond;
                                    break;
                                case 12:
                                    x.bim12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.bim01 = cond;
                                break;
                            case 2:
                                x.bim02 = cond;
                                break;
                            case 3:
                                x.bim03 = cond;
                                break;
                            case 4:
                                x.bim04 = cond;
                                break;
                            case 5:
                                x.bim05 = cond;
                                break;
                            case 6:
                                x.bim06 = cond;
                                break;
                            case 7:
                                x.bim07 = cond;
                                break;
                            case 8:
                                x.bim08 = cond;
                                break;

                            case 9:
                                x.bim09 = cond;
                                break;
                            case 10:
                                x.bim10 = cond;
                                break;
                            case 11:
                                x.bim11 = cond;
                                break;
                            case 12:
                                x.bim12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";


                }
                else
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.bim01 = cond;
                                break;
                            case 2:
                                x.bim02 = cond;
                                break;
                            case 3:
                                x.bim03 = cond;
                                break;
                            case 4:
                                x.bim04 = cond;
                                break;
                            case 5:
                                x.bim05 = cond;
                                break;
                            case 6:
                                x.bim06 = cond;
                                break;
                            case 7:
                                x.bim07 = cond;
                                break;
                            case 8:
                                x.bim08 = cond;
                                break;

                            case 9:
                                x.bim09 = cond;
                                break;
                            case 10:
                                x.bim10 = cond;
                                break;
                            case 11:
                                x.bim11 = cond;
                                break;
                            case 12:
                                x.bim12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";
                }



            }


        }

        [HttpGet]
        public string AlterarSemPmocTodosEquipamentos(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
      int autonumeroLocalFisico, int autonumeroSetor, int mesParaAlterar, int condicaoAlterar)

        {
            var c = 1;

            var cond = Convert.ToSByte(condicaoAlterar);
            using (var dc = new manutEntities())
            {

                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }



                        });
                        dc.SaveChanges();

                        return "";

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.sem01 = cond;
                                    break;
                                case 2:
                                    x.sem02 = cond;
                                    break;
                                case 3:
                                    x.sem03 = cond;
                                    break;
                                case 4:
                                    x.sem04 = cond;
                                    break;
                                case 5:
                                    x.sem05 = cond;
                                    break;
                                case 6:
                                    x.sem06 = cond;
                                    break;
                                case 7:
                                    x.sem07 = cond;
                                    break;
                                case 8:
                                    x.sem08 = cond;
                                    break;

                                case 9:
                                    x.sem09 = cond;
                                    break;
                                case 10:
                                    x.sem10 = cond;
                                    break;
                                case 11:
                                    x.sem11 = cond;
                                    break;
                                case 12:
                                    x.sem12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.sem01 = cond;
                                break;
                            case 2:
                                x.sem02 = cond;
                                break;
                            case 3:
                                x.sem03 = cond;
                                break;
                            case 4:
                                x.sem04 = cond;
                                break;
                            case 5:
                                x.sem05 = cond;
                                break;
                            case 6:
                                x.sem06 = cond;
                                break;
                            case 7:
                                x.sem07 = cond;
                                break;
                            case 8:
                                x.sem08 = cond;
                                break;

                            case 9:
                                x.sem09 = cond;
                                break;
                            case 10:
                                x.sem10 = cond;
                                break;
                            case 11:
                                x.sem11 = cond;
                                break;
                            case 12:
                                x.sem12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";


                }
                else
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.sem01 = cond;
                                break;
                            case 2:
                                x.sem02 = cond;
                                break;
                            case 3:
                                x.sem03 = cond;
                                break;
                            case 4:
                                x.sem04 = cond;
                                break;
                            case 5:
                                x.sem05 = cond;
                                break;
                            case 6:
                                x.sem06 = cond;
                                break;
                            case 7:
                                x.sem07 = cond;
                                break;
                            case 8:
                                x.sem08 = cond;
                                break;

                            case 9:
                                x.sem09 = cond;
                                break;
                            case 10:
                                x.sem10 = cond;
                                break;
                            case 11:
                                x.sem11 = cond;
                                break;
                            case 12:
                                x.sem12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";
                }



            }


        }


        [HttpGet]
        public string AlterarTriPmocTodosEquipamentos(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
              int autonumeroLocalFisico, int autonumeroSetor, int mesParaAlterar, int condicaoAlterar)

        {
            var c = 1;

            var cond = Convert.ToSByte(condicaoAlterar);
            using (var dc = new manutEntities())
            {

                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }



                        });
                        dc.SaveChanges();

                        return "";

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.tri01 = cond;
                                    break;
                                case 2:
                                    x.tri02 = cond;
                                    break;
                                case 3:
                                    x.tri03 = cond;
                                    break;
                                case 4:
                                    x.tri04 = cond;
                                    break;
                                case 5:
                                    x.tri05 = cond;
                                    break;
                                case 6:
                                    x.tri06 = cond;
                                    break;
                                case 7:
                                    x.tri07 = cond;
                                    break;
                                case 8:
                                    x.tri08 = cond;
                                    break;

                                case 9:
                                    x.tri09 = cond;
                                    break;
                                case 10:
                                    x.tri10 = cond;
                                    break;
                                case 11:
                                    x.tri11 = cond;
                                    break;
                                case 12:
                                    x.tri12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.tri01 = cond;
                                break;
                            case 2:
                                x.tri02 = cond;
                                break;
                            case 3:
                                x.tri03 = cond;
                                break;
                            case 4:
                                x.tri04 = cond;
                                break;
                            case 5:
                                x.tri05 = cond;
                                break;
                            case 6:
                                x.tri06 = cond;
                                break;
                            case 7:
                                x.tri07 = cond;
                                break;
                            case 8:
                                x.tri08 = cond;
                                break;

                            case 9:
                                x.tri09 = cond;
                                break;
                            case 10:
                                x.tri10 = cond;
                                break;
                            case 11:
                                x.tri11 = cond;
                                break;
                            case 12:
                                x.tri12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";


                }
                else
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.tri01 = cond;
                                break;
                            case 2:
                                x.tri02 = cond;
                                break;
                            case 3:
                                x.tri03 = cond;
                                break;
                            case 4:
                                x.tri04 = cond;
                                break;
                            case 5:
                                x.tri05 = cond;
                                break;
                            case 6:
                                x.tri06 = cond;
                                break;
                            case 7:
                                x.tri07 = cond;
                                break;
                            case 8:
                                x.tri08 = cond;
                                break;

                            case 9:
                                x.tri09 = cond;
                                break;
                            case 10:
                                x.tri10 = cond;
                                break;
                            case 11:
                                x.tri11 = cond;
                                break;
                            case 12:
                                x.tri12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";
                }



            }


        }


        [HttpGet]
        public string AlterarAnoPmocTodosEquipamentos(long autonumeroCliente, int autonumeroPredio, int autonumeroSubSistema,
              int autonumeroLocalFisico, int autonumeroSetor, int mesParaAlterar, int condicaoAlterar)

        {
            var c = 1;

            var cond = Convert.ToSByte(condicaoAlterar);
            using (var dc = new manutEntities())
            {

                if (autonumeroPredio == 0 && autonumeroSetor == 0 && autonumeroLocalFisico == 0)
                {
                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }



                        });
                        dc.SaveChanges();

                        return "";

                    }
                }

                if (autonumeroLocalFisico > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {


                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroLocalFisico == autonumeroLocalFisico && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSetor > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();
                    }
                }

                if (autonumeroPredio > 0)
                {

                    if (autonumeroSubSistema > 0)
                    {

                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";

                    }
                    else
                    {
                        dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroPredio == autonumeroPredio && a.cancelado != "S").ToList().ForEach(x =>
                        {
                            switch (mesParaAlterar)
                            {
                                case 1:
                                    x.ano01 = cond;
                                    break;
                                case 2:
                                    x.ano02 = cond;
                                    break;
                                case 3:
                                    x.ano03 = cond;
                                    break;
                                case 4:
                                    x.ano04 = cond;
                                    break;
                                case 5:
                                    x.ano05 = cond;
                                    break;
                                case 6:
                                    x.ano06 = cond;
                                    break;
                                case 7:
                                    x.ano07 = cond;
                                    break;
                                case 8:
                                    x.ano08 = cond;
                                    break;

                                case 9:
                                    x.ano09 = cond;
                                    break;
                                case 10:
                                    x.ano10 = cond;
                                    break;
                                case 11:
                                    x.ano11 = cond;
                                    break;
                                case 12:
                                    x.ano12 = cond;
                                    break;

                                default:
                                    break;
                            }

                        });
                        dc.SaveChanges();


                        return "";
                    }

                }

                if (autonumeroSubSistema > 0)
                {

                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S" && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.ano01 = cond;
                                break;
                            case 2:
                                x.ano02 = cond;
                                break;
                            case 3:
                                x.ano03 = cond;
                                break;
                            case 4:
                                x.ano04 = cond;
                                break;
                            case 5:
                                x.ano05 = cond;
                                break;
                            case 6:
                                x.ano06 = cond;
                                break;
                            case 7:
                                x.ano07 = cond;
                                break;
                            case 8:
                                x.ano08 = cond;
                                break;

                            case 9:
                                x.ano09 = cond;
                                break;
                            case 10:
                                x.ano10 = cond;
                                break;
                            case 11:
                                x.ano11 = cond;
                                break;
                            case 12:
                                x.ano12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";


                }
                else
                {
                    dc.tb_cadastro.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").ToList().ForEach(x =>
                    {
                        switch (mesParaAlterar)
                        {
                            case 1:
                                x.ano01 = cond;
                                break;
                            case 2:
                                x.ano02 = cond;
                                break;
                            case 3:
                                x.ano03 = cond;
                                break;
                            case 4:
                                x.ano04 = cond;
                                break;
                            case 5:
                                x.ano05 = cond;
                                break;
                            case 6:
                                x.ano06 = cond;
                                break;
                            case 7:
                                x.ano07 = cond;
                                break;
                            case 8:
                                x.ano08 = cond;
                                break;

                            case 9:
                                x.ano09 = cond;
                                break;
                            case 10:
                                x.ano10 = cond;
                                break;
                            case 11:
                                x.ano11 = cond;
                                break;
                            case 12:
                                x.ano12 = cond;
                                break;

                            default:
                                break;
                        }

                    });
                    dc.SaveChanges();


                    return "";
                }



            }


        }



    }

}
