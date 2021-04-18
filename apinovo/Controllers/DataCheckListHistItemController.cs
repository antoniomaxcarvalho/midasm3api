using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace apinovo.Controllers
{
    public class DataCheckListHistItemController : ApiController
    {
        [HttpGet]
        public IEnumerable<checklisthistitem> GetAllCheckListItemSubSistema(int autonumeroContrato, long autonumeroEquipamento)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklisthistitem.Where(a => a.autonumeroContrato == autonumeroContrato && a.autonumeroEquipamento == autonumeroEquipamento).OrderBy(c => c.anoMes).ThenBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }

        [HttpGet]
        public IEnumerable GetCheckListItemSubSistema(int autonumeroContrato, long autonumeroEquipamento, string ano, string mes)
        {
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));

            using (var dc = new manutEntities())
            {

                var user = (from p in dc.checklisthistitem.Where(a => a.autonumeroContrato == autonumeroContrato && a.autonumeroEquipamento == autonumeroEquipamento && a.anoMes == anoMes).OrderBy(c => c.autonumero).ToList()
                            select new
                            {
                                p.autonumero,
                                p.item,
                                p.nome,
                                p.d,
                                p.e,
                                p.q,
                                p.m,
                                p.b,
                                p.t,
                                p.s,
                                p.a,
                                p.executou,
                                check = 0
                            }).ToList();
                return user.ToList();
            }
        }

        //[HttpPost]
        //public string IncluirAlterarCheckListMensal()
        //{
        //    var c = 1;
        //    var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
        //    if (string.IsNullOrEmpty(auto2))
        //    {
        //        auto2 = "0";
        //    }
        //    var autonumero = Convert.ToInt64(auto2);

        //    var executou = HttpContext.Current.Request.Form["executou"].ToString();
        //    if (string.IsNullOrEmpty(executou))
        //    {
        //        executou = "N";
        //    }

        //    using (var dc = new manutEntities())
        //    {

        //        var linha = dc.checklisthistitem.Find(autonumero); // sempre irá procurar pela chave primaria
        //        if (linha != null)
        //        {
        //            linha.executou = executou;
        //            dc.SaveChanges();
        //        }

        //    }
        //    return "";
        //}

        [HttpPost]
        public string IncluirAlterarCheckListMensal()
        {
            var c = 1;
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt64(auto2);

            var nroFolha = Convert.ToInt64(HttpContext.Current.Request.Form["nroFolha"].ToString());

            var executou = HttpContext.Current.Request.Form["executou"].ToString();
            if (string.IsNullOrEmpty(executou))
            {
                executou = "N";
            }

            using (var dc = new manutEntities())
            {

                long? autonumeroEquipamento = 0;
                int? autonumeroCheckList = 0;

                var linha = dc.checklisthistitem.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    autonumeroEquipamento = linha.autonumeroEquipamento;
                    autonumeroCheckList = linha.autonumeroCheckList;
                    linha.executou = executou;
                    dc.SaveChanges();
                }



                var linha2 = (from p in dc.checklisthistitemnrofolha
                              where p.contadorPmocEquipamento == nroFolha && p.autonumeroCheckList == autonumeroCheckList
                              select p).FirstOrDefault();

                var folha = (from p in dc.checklisthistoriconrofolha
                             where p.contadorPmocEquipamento == nroFolha
                             select p).FirstOrDefault();


                if (folha.equip1 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip1 = "";
                    }
                    else
                    {
                        linha2.equip1 = "X";
                    }
                }
                if (folha.equip2 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip2 = "";
                    }
                    else
                    {
                        linha2.equip2 = "X";
                    }
                }
                if (folha.equip3 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip3 = "";
                    }
                    else
                    {
                        linha2.equip3 = "X";
                    }
                }

                if (folha.equip4 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip4 = "";
                    }
                    else
                    {
                        linha2.equip4 = "X";
                    }
                }
                if (folha.equip5 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip5 = "";
                    }
                    else
                    {
                        linha2.equip5 = "X";
                    }
                }

                if (folha.equip6 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip6 = "";
                    }
                    else
                    {
                        linha2.equip6 = "X";
                    }
                }
                if (folha.equip7 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip7 = "";
                    }
                    else
                    {
                        linha2.equip7 = "X";
                    }
                }

                if (folha.equip8 == autonumeroEquipamento)
                {
                    if (executou == "N")
                    {
                        linha2.equip8 = "";
                    }
                    else
                    {
                        linha2.equip8 = "X";
                    }
                }

                dc.checklisthistitemnrofolha.AddOrUpdate(linha2);
                dc.SaveChanges();


            }
            return "";
        }


        [HttpPost]
        public string ConfirmaTarefa()
        {
            var c = 1;
            var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var autonumeroEquipamento = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroEquipamento"].ToString());
            var ano = HttpContext.Current.Request.Form["ano"].ToString();
            var mes = HttpContext.Current.Request.Form["mes"].ToString().Trim().PadLeft(2, '0');
            var tarefa = HttpContext.Current.Request.Form["tarefa"].ToString();
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));


            var nroFolha = Convert.ToInt64(HttpContext.Current.Request.Form["nroFolha"].ToString());


            using (var dc = new manutEntities())
            {

                //dc.checklist.Where(x => x.autonumeroSistema == autonumeroSistema && x.cancelado != "S").ToList().ForEach(x =>
                //{
                //    x.nomeSistema = nome;
                //});
                //dc.SaveChanges();


                dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroContrato &&
                p.autonumeroEquipamento == autonumeroEquipamento && p.anoMes == anoMes).ToList().ForEach(x =>
                {
                    if (tarefa == "D" && x.d == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }

                    }
                    if (tarefa == "E" && x.e == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }

                    }
                    if (tarefa == "Q" && x.q == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }
                    if (tarefa == "M" && x.m == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }
                    if (tarefa == "B" && x.b == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }
                    if (tarefa == "T" && x.t == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }
                    if (tarefa == "S" && x.s == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }
                    if (tarefa == "A" && x.a == "S")
                    {
                        if (x.executou == "S")
                        {
                            x.executou = "N";
                        }
                        else
                        {
                            x.executou = "S";
                        }
                    }

                });
                dc.SaveChanges();


                var folha = (from p in dc.checklisthistoriconrofolha
                             where p.contadorPmocEquipamento == nroFolha
                             select p).FirstOrDefault();


                dc.checklisthistitem.Where(p => p.autonumeroContrato == autonumeroContrato &&
                p.autonumeroEquipamento == autonumeroEquipamento && p.anoMes == anoMes).ToList().ForEach(x =>
                {


                    var linha2 = (from p in dc.checklisthistitemnrofolha
                                  where p.contadorPmocEquipamento == nroFolha && p.autonumeroCheckList == x.autonumeroCheckList
                                  select p).FirstOrDefault();


                    if (folha.equip1 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip1 = "";
                        }
                        else
                        {
                            linha2.equip1 = "X";
                        }
                    }
                    if (folha.equip2 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip2 = "";
                        }
                        else
                        {
                            linha2.equip2 = "X";
                        }
                    }
                    if (folha.equip3 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip3 = "";
                        }
                        else
                        {
                            linha2.equip3 = "X";
                        }
                    }

                    if (folha.equip4 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip4 = "";
                        }
                        else
                        {
                            linha2.equip4 = "X";
                        }
                    }
                    if (folha.equip5 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip5 = "";
                        }
                        else
                        {
                            linha2.equip5 = "X";
                        }
                    }

                    if (folha.equip6 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip6 = "";
                        }
                        else
                        {
                            linha2.equip6 = "X";
                        }
                    }
                    if (folha.equip7 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip7 = "";
                        }
                        else
                        {
                            linha2.equip7 = "X";
                        }
                    }

                    if (folha.equip8 == autonumeroEquipamento)
                    {
                        if (x.executou == "N")
                        {
                            linha2.equip8 = "";
                        }
                        else
                        {
                            linha2.equip8 = "X";
                        }
                    }

                    dc.checklisthistitemnrofolha.AddOrUpdate(linha2);
                    dc.SaveChanges();



                });





            }
            return "";
        }



    }
}
