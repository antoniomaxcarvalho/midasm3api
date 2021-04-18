using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataCheckListHistoricoCivilController : ApiController
    {

        [HttpGet]
        public IEnumerable<checklisthistoricocivil> GetAllCheckListSetor(int autonumeroCliente, int autonumeroSetor)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistoricocivil.Where(a => a.autonumeroCliente == autonumeroCliente && a.autonumeroSetor == autonumeroSetor && a.cancelado != "S")
                           .OrderBy(c => c.nome).ThenBy(c => c.autonumero)
                           select p;
                return user.ToList();
            }
        }

        [HttpGet]
        public IEnumerable GetCheckListSetor(int autonumeroCliente, int autonumeroSetor, string ano, string mes)
        {
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.checklisthistoricocivil.Where(a => a.autonumeroCliente == autonumeroCliente && a.anoMes == anoMes &&
                           a.autonumeroSetor == autonumeroSetor && a.cancelado != "S").OrderBy(c => c.nomePredio).ThenBy(c => c.contadorPmocCivil).ToList()
                            select p).ToList().OrderBy(p => p.contadorPmocCivil);
                //select new
                //{
                //    p.nomeSetor,
                //    p.nomePredio,
                //    p.nome,
                //    nroFolha = p.contadorPmocCivil,
                //    p.fechado,
                //    check = 0
                //}).Distinct().ToList();
                return user.ToList();
            }
        }


        [HttpGet]
        public IEnumerable<checklisthistoricocivil> GetAllListHistoricoNroFolha(int autonumeroCliente, long nroFolha)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistoricocivil.Where(a => a.autonumeroCliente == autonumeroCliente && a.contadorPmocCivil == nroFolha && a.cancelado != "S").OrderBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }



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
                dc.checklisthistoricocivil.Where(x => x.autonumeroCliente == autonumeroCliente && x.anoMes == anoMes && x.cancelado != "S" && x.contadorPmocCivil == nroFolha).ToList().ForEach(x =>
                {
                    x.fechado = fechado;
                });
                dc.SaveChanges();
            }
            return "";
        }


        [HttpPost]
        public string FecharListHistoricoPorLocal()
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

                var linha = dc.checklisthistoricocivil.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.fechado = fechado;
                    dc.SaveChanges();
                }


            }
            return "";
        }


        //public IEnumerable GetMesManutencao(int autonumeroCliente, long nroFolha)
        //{
        //    var c = 1;
        //    using (var dc = new manutEntities())
        //    {
        //        var lista3 = (from i in dc.checklisthistoricocivil.Where(i => i.contadorPmocCivil == nroFolha && i.cancelado != "S") select i).ToList();
        //        var lista4 = (from i in dc.tb_subsistema.Where(i => i.autonumeroCliente == autonumeroCliente) select i).ToList();

        //        var lista5 = (from k in lista3
        //                      join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

        //                      select new
        //                      {
        //                          k.contadorPmocCivil,
        //                          k.autonumeroSubSistema,
        //                          i.bimestre,
        //                          i.trimestre,
        //                          i.semestre,
        //                          i.anual
        //                      }).ToList();

        //        return lista5;
        //    }



        //}



        [HttpPost]
        public string AtualizarFuncionarioLocal()
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

                var linha = dc.checklisthistoricocivil.Find(autonumero); // sempre irá procurar pela chave primaria
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


        //public string SalvarPmocCivil(int autonumeroCliente, int ano, int mes)
        //{
        //    var c = 1;
        //    //var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
        //    //var ano = Convert.ToInt32(HttpContext.Current.Request.Form["ano"].ToString());
        //    //var mes = Convert.ToInt32(HttpContext.Current.Request.Form["mes"].ToString());

        //    var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

        //    using (var dc = new manutEntities())
        //    {
        //        try
        //        {

        //            //Apagar PMOC anterior -----------------------------------------------------------------------------

        //            var lista = dc.checklisthistoricocivil.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
        //            if (lista.Count > 0)
        //            {
        //                if (lista[0].fechado == "S")
        //                {
        //                    throw new ArgumentException("Erro - PMOC Fechado");
        //                }
        //            }
        //            dc.checklisthistoricocivil.RemoveRange(lista);
        //            dc.SaveChanges();

        //            lista.Clear();

        //            var lista2 = dc.checklisthistoricocivilitem.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
        //            dc.checklisthistoricocivilitem.RemoveRange(lista2);
        //            dc.SaveChanges();

        //            lista2.Clear();

        //            //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------

        //            lista = (from i in dc.local.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S").ToList()
        //                     select new checklisthistoricocivil
        //                     {
        //                         anoMes = anoMes,
        //                         autonumeroCliente = autonumeroCliente,
        //                         autonumeroPredio = i.autonumeroPredio,
        //                         autonumeroSetor = i.autonumeroSetor,
        //                         autonumeroSubSistema = i.autonumeroSubSistema,
        //                         autonumeroSistema = i.autonumeroSistema,
        //                         cancelado = "N",
        //                         fechado = "N",
        //                         nomeCliente = i.nomeCliente,
        //                         nome = i.nome,
        //                         nomePredio = i.nomePredio,
        //                         nomeSetor = i.nomeSetor,
        //                         nomeSubSistema = i.nomeSubSistema,
        //                         nomeSistema = i.nomeSistema,
        //                         autonumeroFuncionario = 0,
        //                         nomeFuncionario = "",
        //                         autonumeroProfissao = 0,
        //                         nomeProfissao = "",
        //                         dataInicio = null,
        //                         dataFim = null,
        //                         totalHoras = TimeSpan.Parse("00:00"),
        //                         contadorPmocCivil = i.contadorPmocCivil,
        //                         obs = ""


        //                     }).ToList();


        //            //manutEntities context = new manutEntities();
        //            //foreach (var e in lista)
        //            //{
        //            //    context.checklisthistorico.Add(e);
        //            //}

        //            //context.SaveChanges();

        //            dc.checklisthistoricocivil.AddRange(lista);
        //            dc.SaveChanges();

        //            var lista3 = (from i in dc.checklisthistoricocivil.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.anoMes == anoMes && i.autonumeroSubSistema > 0) select i).ToList();
        //            var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente) select i).ToList();

        //            var lista5 = (from k in lista3
        //                          join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

        //                          select new checklisthistoricocivilitem
        //                          {
        //                              anoMes = anoMes,
        //                              autonumeroCliente = autonumeroCliente,
        //                              autonumeroHistoricoCivil = k.autonumero,
        //                              autonumeroSubSistema = k.autonumeroSubSistema,
        //                              nomeSubSistema = k.nomeSubSistema,
        //                              executou = "N",
        //                              a = i.a,
        //                              b = i.b,
        //                              d = i.d,
        //                              m = i.m,
        //                              q = i.q,
        //                              s = i.s,
        //                              t = i.t,
        //                              nome = i.nome,
        //                              item = i.item,
        //                              autonumeroPredio = k.autonumeroPredio,
        //                              autonumeroSetor = k.autonumeroSetor,
        //                              nomeCliente = k.nomeCliente,
        //                              nomePredio = k.nomePredio


        //                          }).ToList();


        //            lista3.Clear();
        //            lista4.Clear();

        //            //manutEntities context = new manutEntities();
        //            //foreach (var e in lista5)
        //            //{
        //            //    context.checklisthistitem.Add(e);
        //            //}

        //            //context.SaveChanges();

        //            dc.checklisthistoricocivilitem.AddRange(lista5);
        //            dc.SaveChanges();


        //            return "";
        //        }
        //        catch (Exception ex)
        //        {
        //            var message = ex.Message;
        //            if (ex.InnerException != null)
        //            {
        //                message = ex.InnerException.ToString();
        //            }
        //            return message;
        //        }

        //    }

        //}

        public string SalvarPmocCivil(int autonumeroCliente, int ano, int mes)
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

                    var lista = dc.checklisthistoricocivil.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    if (lista.Count > 0)
                    {
                        if (lista[0].fechado == "S")
                        {
                            throw new ArgumentException("Erro - PMOC Fechado");
                        }
                    }
                    dc.checklisthistoricocivil.RemoveRange(lista);
                    dc.SaveChanges();

                    lista.Clear();

                    var lista2 = dc.checklisthistoricocivilitem.Where(p => p.autonumeroCliente == autonumeroCliente && p.anoMes == anoMes).ToList();
                    dc.checklisthistoricocivilitem.RemoveRange(lista2);
                    dc.SaveChanges();

                    lista2.Clear();

                    //FIM Apagar PMOC anterior-----------------------------------------------------------------------------------

                    lista = (from i in dc.local.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.nomeSistema != null).ToList()
                             select new checklisthistoricocivil
                             {
                                 anoMes = anoMes,
                                 autonumeroCliente = autonumeroCliente,
                                 autonumeroLocal = i.autonumero,
                                 nomeLocal = i.nome.Trim(),
                                 autonumeroPredio = i.autonumeroPredio,
                                 autonumeroSetor = i.autonumeroSetor,
                                 autonumeroSubSistema = i.autonumeroSubSistema,
                                 autonumeroSistema = i.autonumeroSistema,
                                 cancelado = "N",
                                 fechado = "N",
                                 nomeCliente = i.nomeCliente,
                                 nome = i.nome,
                                 nomePredio = i.nomePredio,
                                 nomeSetor = i.nomeSetor,
                                 nomeSubSistema = i.nomeSubSistema,
                                 nomeSistema = i.nomeSistema,
                                 autonumeroFuncionario = 0,
                                 nomeFuncionario = "",
                                 autonumeroProfissao = 0,
                                 nomeProfissao = "",
                                 dataInicio = null,
                                 dataFim = null,
                                 totalHoras = TimeSpan.Parse("00:00"),
                                 contadorPmocCivil = i.contadorPmocCivil,
                                 obs = ""
                                


                             }).ToList();


                    //manutEntities context = new manutEntities();
                    //foreach (var e in lista)
                    //{
                    //    context.checklisthistorico.Add(e);
                    //}

                    //context.SaveChanges();

                    var c1 = lista.Count();

                    //dc.checklisthistoricocivil.AddRange(lista);



                    manutEntities context = new manutEntities();
                    foreach (var e in lista)
                    {

                        Debug.WriteLine("000000" + e.nomeLocal + "11111");
                        context.checklisthistoricocivil.Add(e);
                    }

                    context.SaveChanges();


                    dc.SaveChanges();

                    var lista3 = (from i in dc.checklisthistoricocivil.Where(i => i.autonumeroCliente == autonumeroCliente && i.cancelado != "S" && i.anoMes == anoMes && i.autonumeroSubSistema > 0) select i).ToList();
                    c1 = lista3.Count();
                    var lista4 = (from i in dc.checklist.Where(i => i.autonumeroContrato == autonumeroCliente) select i).ToList();
                    c1 = lista4.Count();
                    var lista5 = (from k in lista3
                                  join i in lista4 on k.autonumeroSubSistema equals i.autonumeroSubsistema

                                  select new checklisthistoricocivilitem
                                  {
                                      anoMes = anoMes,
                                      autonumeroCliente = autonumeroCliente,
                                      autonumeroLocal = k.autonumeroLocal,
                                      nomeLocal = k.nomeLocal.Trim(),
                                      autonumeroHistoricoCivil = k.autonumero,
                                      autonumeroSubSistema = k.autonumeroSubSistema,
                                      nomeSubSistema = k.nomeSubSistema,
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
                                      item = i.item,
                                      autonumeroPredio = k.autonumeroPredio,
                                      autonumeroSetor = k.autonumeroSetor,
                                      nomeCliente = k.nomeCliente,
                                      nomePredio = k.nomePredio


                                  }).ToList();


                    lista3.Clear();
                    lista4.Clear();

                    //manutEntities context = new manutEntities();
                    //foreach (var e in lista5)
                    //{
                    //    context.checklisthistitem.Add(e);
                    //}

                    //context.SaveChanges();
                    c1 = lista5.Count();
                    dc.checklisthistoricocivilitem.AddRange(lista5);
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
