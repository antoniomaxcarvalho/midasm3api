using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataCheckListHistoricoController : ApiController
    {

        [HttpGet]
        public IEnumerable<checklisthistorico> GetAllCheckListSubSistema(int autonumeroCliente, int autonumeroSubSistema)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistorico.Where(a => a.autonumeroCliente == autonumeroCliente && a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").OrderBy(c => c.grupo).ThenBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }

        [HttpGet]
        public IEnumerable GetCheckListSubSistema(int autonumeroCliente, int autonumeroSubSistema, string ano, string mes)
        {
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.checklisthistorico.Where(a => a.autonumeroCliente == autonumeroCliente && a.anoMes == anoMes &&
                           a.autonumeroSubSistema == autonumeroSubSistema && a.cancelado != "S").OrderBy(c => c.nomePredio).ThenBy(c => c.contadorPmocEquipamento).ToList()
                            select new
                            {
                                p.nomeSubSistema,
                                p.nomePredio,
                                nroFolha = p.contadorPmocEquipamento,
                                p.grupo,
                                p.dataPrevista,
                                p.fechado,
                                check = 0
                            }).Distinct().ToList();
                return user.ToList();
            }
        }


        [HttpGet]
        public IEnumerable<checklisthistorico> GetAllListHistoricoGrupo(int autonumeroCliente, long grupo)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistorico.Where(a => a.autonumeroCliente == autonumeroCliente && a.grupo == grupo && a.cancelado != "S").OrderBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }

        [HttpGet]
        public IEnumerable<checklisthistorico> GetAllListHistoricoNroFolha(int autonumeroCliente, long nroFolha)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistorico.Where(a => a.autonumeroCliente == autonumeroCliente && a.contadorPmocEquipamento == nroFolha && a.cancelado != "S").OrderBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }

        [HttpGet]
        //public IEnumerable GetSomarHoras(int autonumeroCliente, long nroFolha)
        //{

        //var csql = new StringBuilder();


        //csql.Append("SELECT nomeSetor, sec_to_time(sum(time_to_sec(totalHoras))) as totalHoras, count(autonumero) as totalOS ");
        //csql.Append("FROM checklisthistorico WHERE  cancelado != 'S' and  nomeSistema != '' and cast(dataTermino as date) >= {0} and cast(dataTermino as date) <= {1} and totalHoras > 0 group by nomeSetor)");
        //using (var dc = new manutEntities())
        //{
        //    var user = dc.Database.ExecuteSqlCommand(@csql.ToString(), new object[] { data1, data2 });
        //}


        //}



        [HttpPost]
        public string FecharListHistoricoPorFolha()
        {
            var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
            var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nroFolha = Convert.ToInt64(HttpContext.Current.Request.Form["nroFolha"].ToString());
            var fechado = HttpContext.Current.Request.Form["fechado"].ToString();

            if (string.IsNullOrEmpty(fechado))
            {
                fechado = "N";
            }

            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {
                dc.checklisthistorico.Where(x => x.autonumeroCliente == autonumeroCliente && x.anoMes == anoMes && x.cancelado != "S" && x.contadorPmocEquipamento == nroFolha).ToList().ForEach(x =>
                {
                    x.fechado = fechado;
                });
                dc.SaveChanges();
            }
            return "";
        }


        [HttpPost]
        public string FecharListHistoricoPorAparelho()
        {
            var c = 1;
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt64(auto2);

            var fechado = HttpContext.Current.Request.Form["fechado"].ToString();
            if (string.IsNullOrEmpty(fechado))
            {
                fechado = "N";
            }

            using (var dc = new manutEntities())
            {

                var linha = dc.checklisthistorico.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.fechado = fechado;
                    dc.SaveChanges();
                }


            }
            return "";
        }


        public IEnumerable GetMesManutencao(int autonumeroCliente, long nroFolha)
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var lista3 = (from i in dc.checklisthistorico.Where(i => i.contadorPmocEquipamento == nroFolha && i.cancelado != "S") select i).ToList();
                var lista4 = (from i in dc.tb_subsistemacliente.Where(i => i.autonumeroCliente == autonumeroCliente) select i).ToList();

                var lista5 = (from k in lista3
                              join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                              select new
                              {
                                  k.contadorPmocEquipamento,
                                  k.autonumeroSubSistema,
                                  k.mesManuSemestre,
                                  k.mesManuAno,
                                  i.bimestre,
                                  i.trimestre,
                                  i.semestre,
                                  i.anual
                              }).ToList();

                return lista5;
            }



        }



        [HttpPost]
        public string AtualizarFuncionarioAparelho()
        {
            var c = 1;
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt64(auto2);

            var autonumeroFuncionario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroFuncionario"].ToString());
            var nomeFuncionario = HttpContext.Current.Request.Form["nomeFuncionario"].ToString().Trim();
            var autonumeroProfissao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroProfissao"].ToString());
            var nomeProfissao = HttpContext.Current.Request.Form["nomeProfissao"].ToString().Trim();

            DateTime? dataInicio = DateTime.Now;
            if (IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
            {
                dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
            }
            else
            {
                dataInicio = null;
            }

            DateTime? dataFim = DateTime.Now;

            if (IsDate(HttpContext.Current.Request.Form["dataFim"].ToString()))
            {
                dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());
            }
            else
            {
                dataFim = null;
            }

            var totalHoras = TimeSpan.Parse("00:00");
            if (IsTime(HttpContext.Current.Request.Form["totalHoras"].ToString()))
            {
                totalHoras = TimeSpan.Parse(HttpContext.Current.Request.Form["totalHoras"].ToString());
                // horas maior que 24 
                //totalHoras = new TimeSpan(int.Parse(totHoras.Split(':')[0]), int.Parse(totHoras.Split(':')[1]), 0);
            }

            using (var dc = new manutEntities())
            {

                var linha = dc.checklisthistorico.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.autonumeroFuncionario = autonumeroFuncionario;
                    linha.nomeFuncionario = nomeFuncionario;
                    linha.autonumeroProfissao = autonumeroProfissao;
                    linha.nomeProfissao = nomeProfissao;
                    linha.dataInicio = dataInicio;
                    linha.dataFim = dataFim;
                    linha.totalHoras = totalHoras;
                    dc.SaveChanges();
                }


            }
            return "";
        }


        public static bool IsTime(string MyString)
        {
            try
            {
                TimeSpan.Parse(MyString); // or Double.Parse if you want to use double
                // horas maior que 24 
                //TimeSpan ts = new TimeSpan(int.Parse(MyString.Split(':')[0]), int.Parse(MyString.Split(':')[1]), 0);
                return true;
            }
            catch (Exception)
            {
                return false;
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
