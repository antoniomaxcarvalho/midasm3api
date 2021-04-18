using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataSistemaController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_sistema> GetSistema(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_sistema.Where(a => a.autonumero == autonumero && a.cancelado != "S") orderby p.nome select p;
                return user.ToList();
            }
        }


        [HttpGet]
        public IEnumerable GetAllSistema()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_sistema.Where(a => a.cancelado != "S") orderby p.nome select p;
                return user.ToList();

            }

        }



        [HttpPost]
        public string IncluirAlterarSistema()
        {
            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();

            if (autonumero == 0)
            {

                using (var dc = new manutEntities())
                {
                    var k = new tb_sistema
                    {
                        nome = nome,
                        cancelado = "N"
                    };


                    dc.tb_sistema.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.tb_sistema.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroSistema = linha.autonumero;

                        linha.nome = nome;
                        dc.tb_sistema.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.tb_subsistema.Where(x => x.autonumeroSistema == autonumeroSistema && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSistema = nome;
                        });
                        dc.SaveChanges();

                        dc.tb_os.Where(x => x.autonumeroSistema == autonumeroSistema && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSistema = nome;
                        });
                        dc.SaveChanges();

                        dc.checklist.Where(x => x.autonumeroSistema == autonumeroSistema && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSistema = nome;
                        });
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }


        [HttpDelete]
        public string CancelarSistema()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_sistema.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.tb_sistema.AddOrUpdate(linha);
                    dc.SaveChanges();


                    dc.tb_subsistema.Where(x => x.autonumeroSistema == autonumero && x.cancelado != "S").ToList().ForEach(x =>
                    {
                        x.cancelado = "S";
                    });
                    dc.SaveChanges();

                    dc.checklist.Where(x => x.autonumeroSistema == autonumero && x.cancelado != "S").ToList().ForEach(x =>
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
