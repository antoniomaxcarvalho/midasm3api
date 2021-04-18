using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;



namespace apinovo.Controllers
{
    public class DataSubSistemaController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_subsistema> GetAllSubSistemaSistema(int autonumeroSistema)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_subsistema.Where(a => a.autonumeroSistema == autonumeroSistema && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_subsistema> GetSubSistemaAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_subsistema.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_subsistema> GetAllSubSistema()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_subsistema where p.cancelado != "S" orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public string IncluirAlterarSubSistema()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);

            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
            var obsPmoc = HttpContext.Current.Request.Form["obsPmoc"].ToString().Trim();

            if (autonumero == 0)
            {
                var autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString());
                var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();


                using (var dc = new manutEntities())
                {
                    var k = new tb_subsistema
                    {
                        nome = nome,
                        autonumeroSistema = autonumeroSistema,
                        nomeSistema = nomeSistema,
                        obsPmoc = obsPmoc,
                        cancelado = "N",
                        anual  ="",
                        autonumeroEquipe = 0,
                        autonumeroEquipe2 = 0,
                        chkAno = 0,
                        chkSemestre = 0,
                        chkTodoMes = 1,
                        chkTrimestre = 0,
                        mesesParaCalcular = "",
                        nomeEquipe = "",
                        nomeEquipe2 = "",
                        qtdeAtendidaEquipePorDia = 0,
                        qtdePorGrupoRelatorio = 0,
                        semestre = "",
                        trimestre = ""
                    };

                    dc.tb_subsistema.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.tb_subsistema.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroSubSistema = linha.autonumero;
                        linha.nome = nome;
                        linha.obsPmoc = obsPmoc;

                        dc.tb_subsistema.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.checklist.Where(x => x.autonumeroSubsistema == autonumeroSubSistema && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSubSistema = nome;
                        });
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarSubSistema()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_subsistema.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.tb_subsistema.AddOrUpdate(linha);
                    dc.SaveChanges();

                    dc.checklist.Where(x => x.autonumeroSubsistema == autonumero && x.cancelado != "S").ToList().ForEach(x =>
                    {
                        x.cancelado = "S";
                    });
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

    }
}
