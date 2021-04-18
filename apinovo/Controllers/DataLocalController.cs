using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataLocalController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_local> GetLocal(Int32 autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_local.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }
        }


        public IEnumerable<tb_local> GetAllLocal()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_local.Where((a => a.cancelado != "S")) orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarLocal()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_local.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_local.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]

        public string IncluirLocal()
        {
            using (var dc = new manutEntities())
            {
                var k = new tb_local
                {
                    nome = ""
                };

                dc.tb_local.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }
        [HttpPost]

        public void AtualizarLocal()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

                var linha = dc.tb_local.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    dc.tb_local.AddOrUpdate(linha);
                    dc.SaveChanges();
                }
            }

        }

        //-------------------------------------------------------------------------------------------------


        [HttpGet]
        public IEnumerable<tb_local_fisico> GetLocalFisico(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_local_fisico.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }
        }


        [HttpDelete]
        public string CancelarLocalFisico()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_local_fisico.Find(autonumero, autonumeroCliente); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_local_fisico.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        //[HttpPost]

        //public string IncluirLocalFisico()
        //{
        //    using (var dc = new manutEntities())
        //    {
        //        var k = new tb_local_fisico
        //        {
        //            nome = "",
        //            autonumeroCliente = 0
        //        };

        //        dc.tb_local_fisico.Add(k);
        //        dc.SaveChanges();
        //        var auto = Convert.ToInt32(k.autonumero);

        //        return auto.ToString("#######0");
        //    }
        //}
        [HttpPost]
        public void AtualizarLocalFisico()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"]);

                if (autonumero == 0)
                {
                    var linha = new tb_local_fisico();
                    linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    linha.autonumeroCliente = autonumeroCliente;
                    linha.cancelado = "N";
                    dc.tb_local_fisico.Add(linha);
                    dc.SaveChanges();
                }
                else
                {
                    var linha = dc.tb_local_fisico.Find(autonumero, autonumeroCliente); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var nomeLocalFisico = HttpContext.Current.Request.Form["nome"].ToString();
                        linha.nome = nomeLocalFisico.Trim();
                        dc.tb_local_fisico.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.tb_os.Where(x => x.autonumeroLocalFisico == autonumero).ToList().ForEach(x =>
                        {
                            x.nomeLocalFisico = nomeLocalFisico;
                        });
                        dc.SaveChanges();
                    }
                }
            }

        }
        public IEnumerable<tb_local_fisico> GetAllLocalFisico(Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_local_fisico.Where((a => a.cancelado != "S" && a.autonumeroCliente == autonumeroCliente)) orderby p.nome select p;
                return user.ToList(); ;
            }
        }


        [HttpPost]
        public IEnumerable<tb_local_fisico> IncluirLocalFisicoDireto()
        {
            using (var dc = new manutEntities())
            {
                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString().Trim());
                var k = new tb_local_fisico
                {
                    nome = nome,
                    autonumeroCliente = autonumeroCliente,
                    cancelado = "N",
                    
                };

                dc.tb_local_fisico.Add(k);
                dc.SaveChanges();
                var user = from p in dc.tb_local_fisico.Where((a => a.cancelado != "S" && a.autonumeroCliente == autonumeroCliente)) orderby p.nome select p;
                return user.ToList(); ;
            }
        }


        [HttpPost]
        public IEnumerable<tb_subsistema> IncluirSubSistemaDireto()
        {

            using (var dc = new manutEntities())
            {
                var autonumeroSistema = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroSistema"].ToString().Trim());
                var nomeSistema = HttpContext.Current.Request.Form["nomeSistema"].ToString().Trim();
                var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();

                var k = new tb_subsistema
                {
                    nomeSistema = nomeSistema,
                    autonumeroSistema = autonumeroSistema,
                    nome = nome,
                    autonumeroEquipe = 1,
                    nomeEquipe = "1pr",
                    qtdePorGrupoRelatorio = 0,
                    qtdeAtendidaEquipePorDia = 0,
                    autonumeroEquipe2 = 0,
                    nomeEquipe2 = "",
                    anual = "",
                    semestre = "",
                    trimestre = "",
                    chkTodoMes = 0,
                    mesesParaCalcular = "",
                    chkAno = 0,
                    chkSemestre = 0,
                    chkTrimestre = 0
                };

                dc.tb_subsistema.Add(k);
                dc.SaveChanges();
                var user = from p in dc.tb_subsistema orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        //-------------------------------------------------------------------------------------------------

        [HttpGet]
        public IEnumerable<tb_local_atendido> GetAllLocalAtendido(long autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_local_atendido.Where((a => a.cancelado != "S" && a.autonumeroCliente == autonumeroCliente)) orderby p.nome select p;
                return user.ToList(); ;
            }


        }


        public void AtualizarLocalAtendido()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

                if (autonumero == 0)
                {
                    var linha = new tb_local_atendido();
                    linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    linha.autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"]);
                    linha.nomeCliente = HttpContext.Current.Request.Form["nomeCliente"];
                    linha.cancelado = "N";
                    dc.tb_local_atendido.Add(linha);
                    dc.SaveChanges();
                }
                else
                {
                    var linha = dc.tb_local_atendido.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                        dc.tb_local_atendido.AddOrUpdate(linha);
                        dc.SaveChanges();
                    }
                }
            }

        }

        [HttpDelete]
        public string CancelarLocalAtendido()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_local_atendido.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_local_atendido.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

    }
}
