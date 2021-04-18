using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DatachecklisthistoricocivilitemCivilController : ApiController
    {
        [HttpGet]
        public IEnumerable GetCheckListItemLocal(long autonumeroHistoricoCivil)
        {

            using (var dc = new manutEntities())
            {

                var c = 1;

                var user = (from p in dc.checklisthistoricocivilitem.Where(a => a.autonumeroHistoricoCivil == autonumeroHistoricoCivil).OrderBy(p => p.autonumero).ToList()
                            select new
                            {
                                p.autonumero,
                                p.item,
                                p.nome,
                                p.d,   // necessita de acao "S"
                                p.e,
                                p.q,   // necessita de acao "N"
                                p.m,   // necessita de acao "NA"
                                p.b,
                                p.t,
                                p.s,
                                p.a,
                                checkSim = 0,
                                checkNao = 0,
                                checkNA = 0,
                            }).ToList();
                return user.ToList();
            }
        }

        [HttpPost]
        public string IncluirAlterarCheckListCivilMensal()
        {
            var c = 1;
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt64(auto2);

            var checkSim = HttpContext.Current.Request.Form["checkSim"].ToString();
            if (string.IsNullOrEmpty(checkSim))
            {
                checkSim = "N";
            }
            var checkNao = HttpContext.Current.Request.Form["checkNao"].ToString();
            if (string.IsNullOrEmpty(checkNao))
            {
                checkNao = "N";
            }
            var checkNA = HttpContext.Current.Request.Form["checkNA"].ToString();
            if (string.IsNullOrEmpty(checkNA))
            {
                checkNA = "N";
            }

            using (var dc = new manutEntities())
            {

                var linha = dc.checklisthistoricocivilitem.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.d = checkSim;
                    linha.q = checkNao;
                    linha.m = checkNA;
                    dc.SaveChanges();
                }

            }
            return "";
        }

        [HttpPost]
        public string ConfirmaTarefaCivil()
        {
            var c = 1;
            var autonumeroHistoricoCivil = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroHistoricoCivil"].ToString());
            var ano = HttpContext.Current.Request.Form["ano"].ToString();
            var mes = HttpContext.Current.Request.Form["mes"].ToString().Trim().PadLeft(2, '0');
            var tarefa = HttpContext.Current.Request.Form["tarefa"].ToString();
            var anoMes = string.Concat(ano.ToString(), mes.ToString().PadLeft(2, '0'));


            using (var dc = new manutEntities())
            {

                dc.checklisthistoricocivilitem.Where(p => p.autonumeroHistoricoCivil == autonumeroHistoricoCivil &&
                p.anoMes == anoMes).ToList().ForEach(x =>
                {
                    if (tarefa == "S")
                    {
                        if (x.d == "S")
                        {
                            x.d = "N";
                        }
                        else
                        {
                            x.d = "S";
                            x.q = "N";
                            x.m = "N";
                        }

                    }
                    if (tarefa == "N")
                    {
                        if (x.q == "S")
                        {
                            x.q = "N";
                        }
                        else
                        {
                            x.q = "S";
                            x.d = "N";
                            x.m = "N";
                        }
                    }
                    if (tarefa == "NA")
                    {
                        if (x.m == "S")
                        {
                            x.m = "N";
                        }
                        else
                        {
                            x.m = "S";
                            x.q = "N";
                            x.d = "N";
                        }
                    }

                });
                dc.SaveChanges();

            }
            return "";
        }


    }
}
