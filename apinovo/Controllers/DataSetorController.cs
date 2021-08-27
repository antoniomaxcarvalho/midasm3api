using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataSetorController : ApiController
    {
        [HttpGet]
        public IEnumerable<setor> GetAllSetorPredio(int autonumeroPredio)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.setor.Where(a => a.autonumeroPredio == autonumeroPredio && a.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }


        [HttpGet]
        public IEnumerable<tb_setor> GetSetor(Int32 autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_setor.Where(a => a.autonumero == autonumero  && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_setor> getAllSetorCompartilhado()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_setor.Where(a => a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_setor> GetAllSetor(Int64 autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_setor.Where((a => a.cancelado != "S" &&  a.autonumeroCliente == autonumeroCliente )) orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpDelete]
        public string CancelarSetor()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_setor.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_setor.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        [HttpPost]

        public string IncluirSetor()
        {
            using (var dc = new manutEntities())
            {
                var k = new tb_setor
                {
                    nome = "",
                    autonumeroCliente = 0
                };

                dc.tb_setor.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }
        [HttpPost]

        public void AtualizarSetor()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

                var linha = dc.tb_setor.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    var nomeSetor = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    linha.nome = nomeSetor.Trim(); ;
                    linha.autonumeroCliente = Convert.ToInt64(   HttpContext.Current.Request.Form["autonumeroCliente"].ToString().Trim());
                    dc.tb_setor.AddOrUpdate(linha);
                    dc.SaveChanges();
                    dc.tb_os.Where(x => x.autonumeroSetor == autonumero).ToList().ForEach(x =>
                    {
                        x.nomeSetor = nomeSetor;
                    });
                    dc.SaveChanges();
                }
            }

        }

        [HttpPost]
        public string IncluirAlterarSetor()
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
                var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
                var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString());
                var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();

                using (var dc = new manutEntities())
                {
                    var k = new setor
                    {
                        nome = nome,
                        autonumeroCliente = autonumeroCliente,
                        nomeCliente = nomeCliente,
                        autonumeroPredio = autonumeroPredio,
                        nomePredio = nomePredio,
                        cancelado = "N",
                        contadorPmocCivil = 0,

                    };

                    dc.setor.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    return auto.ToString("#######0");
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.setor.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        var autonumeroSetor = linha.autonumero;
                        linha.nome = nome;
                        dc.setor.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.tb_os.Where(x => x.autonumeroSetor == autonumeroSetor && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSetor = nome;
                        });
                        dc.SaveChanges();

                        dc.local.Where(x => x.autonumeroSetor == autonumeroSetor && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSetor = nome;
                        });
                        dc.SaveChanges();

                        dc.tb_cadastro.Where(x => x.autonumeroSetor == autonumeroSetor && x.cancelado != "S").ToList().ForEach(x =>
                        {
                            x.nomeSetor = nome;
                        });
                        dc.SaveChanges();

                        return "0";

                    }
                }
            }
            return "0";
        }



        [HttpPost]
        public IEnumerable<tb_setor> IncluirSetorDireto()
        {
            using (var dc = new manutEntities())
            {
                var nomeSetor = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                var autonumeroCliente = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroCliente"].ToString().Trim());
                var k = new tb_setor
                {
                    
                    nome = nomeSetor,
                    autonumeroCliente = autonumeroCliente,
                    cancelado = "N",
                };

                dc.tb_setor.Add(k);
                dc.SaveChanges();

                var user = from p in dc.tb_setor.Where((a => a.cancelado != "S" && a.autonumeroCliente == autonumeroCliente)) orderby p.nome select p;
                return user.ToList(); ;
            }
        }


        [HttpDelete]
        public string CancelarSetorPredio()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.setor.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.setor.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }



    }
}
