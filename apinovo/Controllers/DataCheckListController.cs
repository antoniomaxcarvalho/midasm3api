using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataCheckListController : ApiController
    {



        [HttpGet]
        public IEnumerable<checklist> GetCheckListAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklist.Where(a => a.autonumero == autonumero).OrderBy(p => p.item) select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<checklist> GetAllCheckListSistema(int autonumeroContrato, int autonumeroSistema)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklist.Where(a => a.autonumeroContrato == autonumeroContrato && a.autonumeroSistema == autonumeroSistema && a.cancelado != "S").OrderBy(p => p.autonumero) select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<checklist> GetAllCheckListSubSistema(int autonumeroContrato, int autonumeroSubsistema)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklist.Where(a => a.autonumeroContrato == autonumeroContrato && a.autonumeroSubsistema == autonumeroSubsistema && a.cancelado != "S").OrderBy(c => c.autonumero) select p;
                return user.ToList();
            }
        }
        [HttpGet]
        public IEnumerable<checklist> GetAllCheckList()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.checklist.Where(a => a.cancelado != "S").OrderBy(p => p.autonumeroContrato).ThenBy(p => p.item) select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable GetAllCheckListExistente( long autonumeroContrato)
        {
            var c = 1;
            using (var dc = new manutEntities())
            {

                var user = (from o in dc.checklist
                            where o.cancelado != "S" && o.autonumeroContrato == autonumeroContrato
                            select new
                            {
                                o.nomeSistema,
                                o.nomeSubSistema,
                            }).Distinct().OrderBy(o => o.nomeSistema).ThenBy(o => o.nomeSubSistema);
                return user.ToList();
            }
        }


        [HttpPost]
        public string IncluirAlterarCheckList()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);
            var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var nomeContrato = HttpContext.Current.Request.Form["nomeContrato"].ToString().Trim();
            var autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
            var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
            var autonumeroSubsistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubsistema"].ToString());
            var nomeSubSistema = HttpContext.Current.Request.Form["nomeSubSistema"].ToString().Trim();
            var item = HttpContext.Current.Request.Form["item"].ToString().Trim();
            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
            var d = HttpContext.Current.Request.Form["d"].ToString().Trim();
            var e = HttpContext.Current.Request.Form["e"].ToString().Trim();
            var q = HttpContext.Current.Request.Form["q"].ToString().Trim();
            var m = HttpContext.Current.Request.Form["m"].ToString().Trim();
            var b = HttpContext.Current.Request.Form["b"].ToString().Trim();
            var t = HttpContext.Current.Request.Form["t"].ToString().Trim();
            var s = HttpContext.Current.Request.Form["s"].ToString().Trim();
            var a = HttpContext.Current.Request.Form["a"].ToString().Trim();


            if (string.IsNullOrEmpty(d)) d = "N";
            if (string.IsNullOrEmpty(e)) e = "N";
            if (string.IsNullOrEmpty(q)) q = "N";
            if (string.IsNullOrEmpty(m)) m = "N";
            if (string.IsNullOrEmpty(b)) b = "N";
            if (string.IsNullOrEmpty(t)) t = "N";
            if (string.IsNullOrEmpty(s)) s = "N";
            if (string.IsNullOrEmpty(a)) a = "N";

            using (var dc = new manutEntities())
            {
                if (autonumero == 0)
                {

                    var k = new checklist
                    {
                        autonumeroContrato = autonumeroContrato,
                        nomeContrato = nomeContrato,
                        autonumeroSistema = autonumeroSistema,
                        nomeSistema = nomeSistema,
                        autonumeroSubsistema = autonumeroSubsistema,
                        nomeSubSistema = nomeSubSistema,
                        item = item,
                        nome = nome,
                        d = d,
                        e = e,
                        q = q,
                        m = m,
                        b = b,
                        t = t,
                        s = s,
                        a = a,
                        cancelado = "N"

                    };

                    dc.checklist.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
                else
                {
                    var linha = dc.checklist.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.nome = nome;
                        linha.item = item;
                        linha.d = d;
                        linha.q = q;
                        linha.m = m;
                        linha.b = b;
                        linha.t = t;
                        linha.s = s;
                        linha.a = a;
                        dc.checklist.AddOrUpdate(linha);
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }
        [HttpDelete]
        public string CancelarCheckList()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.checklist.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.checklist.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]
        public string CopiarCheckList()
        {
            var c = 1;
            var autonumeroContrato = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroContrato"].ToString());
            var autonumeroSubSistemaOrigem = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistemaOrigem"].ToString());
            var autonumeroSubSistemaDestino = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSubSistemaDestino"].ToString());

            using (var dc = new manutEntities())
            {
                var cList = (from p in dc.checklist.Where(a => a.autonumeroContrato == autonumeroContrato && a.autonumeroSubsistema == autonumeroSubSistemaOrigem && a.cancelado != "S").OrderBy(p => p.autonumero) select p).ToList();

                cList.ToList().ForEach(x =>
                {
                    x.autonumeroSubsistema = autonumeroSubSistemaDestino;
                });

                foreach (var e in cList)
                {
                    dc.checklist.Add(e);
                }

                dc.SaveChanges();

                return "";

            }

        }




    }
}
