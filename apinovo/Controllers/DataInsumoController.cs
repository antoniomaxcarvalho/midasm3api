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
    public class DataInsumoController : ApiController
    {

        public class insumo
        {
            public int autonumero { get; set; }
            public string nome { get; set; }
            public string unidade { get; set; }
            public Nullable<double> preco { get; set; }
            public string codigo { get; set; }
            public string anoMes { get; set; }
            public string fonte { get; set; }
            public DateTime? data { get; set; }
        }

        [HttpGet]
        public IEnumerable<tb_insumo> GetAllInsumo()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_insumo orderby p.nome select p;
                return user.ToList();
            }

        }

        [HttpGet]
        public List<insumo> GetInsumoNome(string pesq1, string pesq2, string fonte, string anoMes,DateTime? dataContrato)
        {

            List<insumo> l1 = null;


            var p1 = string.Empty;
            var p2 = string.Empty;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = pesq1; }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = pesq2; }

            using (var dc = new manutEntities())
            {
                if (string.IsNullOrEmpty(p2))
                {

                    l1 = (from p in dc.tb_insumo
                          where p.nome.Contains(p1) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data

                          }).ToList();
                }

                if (string.IsNullOrEmpty(p1))
                {
                    l1 = (from p in dc.tb_insumo
                          where p.nome.Contains(p2) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data
                          }).ToList();
                }

                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                {
                    l1 = (from p in dc.tb_insumo
                          where p.nome.Contains(p1) && p.nome.Contains(p2) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data
                          }).ToList(); ;
                }

            }

            List<insumo> newList = new List<insumo>();

             if (!fonte.Contains("M3"))
            {

                    newList = (from x in l1
                               where x.anoMes.Contains(anoMes)
                               select x).ToList();
                    return newList.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
            }
            else
            {
                newList = l1;
                if (fonte.Contains("M3"))
                {
                    newList = (from x in l1
                               where x.fonte == "M3" && x.data >= dataContrato
                               select x).ToList();
                }
                return newList.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
            }


        }


        [HttpPost]
        public string IncluirInsumo()
        {

            var data = DateTime.Now;
            using (var dc = new manutEntities())
            {
                var k = new tb_insumo
                {
                    anoMes = "",
                    codigo = "",
                    fonte = "M3",
                    nome = HttpContext.Current.Request.Form["nome"].ToString().Trim(),
                    preco = Convert.ToDouble(HttpContext.Current.Request.Form["preco"].ToString()),
                    unidade = HttpContext.Current.Request.Form["unidade"].ToString().Trim(),
                    data = data.Date
                };

                dc.tb_insumo.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);
                var codigo = "M3" + auto.ToString("########0");
                k.codigo = codigo;
                dc.SaveChanges();

                return auto.ToString("#######0");
            }
        }

        [HttpDelete]
        public string RemoverInsumo()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_insumo.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.tb_insumo.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }

        [HttpGet]
        public IEnumerable<tb_insumo> GetAllInsumoM3()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_insumo.Where((a => (a.fonte == "M3"))).OrderBy(p => p.nome) select p;
                return user.ToList();
            }

        }

        [HttpGet]
        public List<insumo> GetInsumoCodigo(string pesq1, string pesq2, string fonte, string anoMes,DateTime dataContrato)
        {

            List<insumo> l1 = null;


            var p1 = string.Empty;
            var p2 = string.Empty;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = pesq1.Trim(); }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = pesq2.Trim(); }

            using (var dc = new manutEntities())
            {

                if (string.IsNullOrEmpty(p2))
                {

                    l1 = (from p in dc.tb_insumo
                          where p.codigo.Contains(p1) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data

                          }).ToList(); ;
                }

                if (string.IsNullOrEmpty(p1))
                {
                    l1 = (from p in dc.tb_insumo
                          where p.codigo.Contains(p2) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data
                          }).ToList(); ;
                }

                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                {
                    l1 = (from p in dc.tb_insumo
                          where p.codigo.Contains(p1) && p.codigo.Contains(p2) && fonte.Contains(p.fonte)
                          select new insumo
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                              data = p.data
                          }).ToList(); ;
                }

            }

            List<insumo> newList = new List<insumo>();

             if (!fonte.Contains("M3"))
            {

                newList = (from x in l1
                           where x.anoMes.Contains(anoMes)
                           select x).ToList();
                return newList.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
            }
            else
            {
                newList = l1;
                if (fonte.Contains("M3"))
                {
                    newList = (from x in l1
                               where x.fonte == "M3" && x.data >= dataContrato
                               select x).ToList();
                }
                return newList.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
            }


        }


        [HttpGet]
        public List<string> GetAllTabelas()
        {
           
            using (var dc = new manutEntities())
            {

                //return (from ta in dc.tb_servico select ta.fonte).Distinct().Union(from tb in dc.tb_insumo select tb.fonte).Distinct().Union(from tb in dc.tb_planilhafechada select tb.fonte).Distinct().ToList();
                return (from ta in dc.tb_servico select ta.fonte).Distinct().Union(from tb in dc.tb_insumo select tb.fonte).Distinct().Distinct().ToList();

            }

        }


        [HttpGet]
        public List<insumo> GetInsumoPreco(string pesq1, string pesq2, string fonte)
        {

            List<insumo> l1 = null;


            var p1 = 0.0;
            var p2 = 0.0;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = Convert.ToDouble(pesq1.Trim()); }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = Convert.ToDouble(pesq2.Trim()); }

            using (var dc = new manutEntities())
            {
                l1 = (from p in dc.tb_insumo
                      where p.preco >= p1 && p.preco <= p2 && fonte.Contains(p.fonte)
                      select new insumo
                      {
                          anoMes = p.anoMes,
                          autonumero = p.autonumero,
                          codigo = p.codigo,
                          fonte = p.fonte,
                          nome = p.nome,
                          preco = p.preco,
                          unidade = p.unidade,
                          data = p.data

                      }).Take(500).ToList(); 
            }

            return l1.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();

        }


    }


}
