using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
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
                    linha.qtdePorGrupoRelatorio = Convert.ToInt32(HttpContext.Current.Request.Form["qtdePorGrupoRelatorio"].ToString().Trim());
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
                if (linha != null )
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
                var pmocCheckList =HttpContext.Current.Request.Form["pmocCheckList"].ToString();

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


                var c = 1;

                List<DiaUtil> listaDiaUtil = null;

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

                //var imp = new DataImprimirController();
                //imp.ImprimirPmocEquipamento(autonumeroCliente, sigla, anoMes);

                SalvarPmocEquipamento(autonumeroCliente, ano, mes);
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



    }

}
