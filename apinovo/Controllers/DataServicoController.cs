using System;
using System.Collections.Generic;
//using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class servico
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

    public class DataServicoController : ApiController
    {
        //db.Configuration.ProxyCreationEnabled = false;
        [HttpGet]
        public List<servico> GetServicoNome2(string pesq1, string pesq2, string fonte, string anoMes, DateTime? dataContrato)
        {
            //var percBdi = (1 + bdi / 100);

            List<servico> l1 = null;
            List<servico> l2 = null;
            List<servico> l3 = null;
            List<servico> l4 = null;
            List<servico> l5 = null;
            List<servico> l6 = null;
            List<servico> l7 = null;

            var p1 = string.Empty;
            var p2 = string.Empty;

            if (!string.IsNullOrEmpty(pesq1))
            { p1 = pesq1.Trim(); }

            if (!string.IsNullOrEmpty(pesq2))
            { p2 = pesq2.Trim(); }

            using (var dc = new manutEntities())
            {
                if (fonte.Contains("anexoIV"))
                {
                    if (string.IsNullOrEmpty(p2))
                    {

                        l1 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("anexoIV")
                              select new servico
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
                        l1 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("anexoIV")
                              select new servico
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
                        l1 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("anexoIV")
                              select new servico
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


                if (fonte.Contains("anexoV"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l2 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("anexoV")
                              select new servico
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
                        l2 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("anexoV")
                              select new servico
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
                        l2 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("anexoV")
                              select new servico
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
                }
                if (fonte.Contains("M3"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l3 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("M3")
                              select new servico
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
                        l3 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("M3")
                              select new servico
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
                        l3 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("M3")
                              select new servico
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

                }

                if (fonte.Contains("EMOP"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l4 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("EMOP")
                              select new servico
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
                        l4 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("EMOP")
                              select new servico
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
                        l4 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("EMOP")
                              select new servico
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

                }

                if (fonte.Contains("SINAPI"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l5 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("SINAPI")
                              select new servico
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
                        l5 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("SINAPI")
                              select new servico
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
                        l5 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("SINAPI")
                              select new servico
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

                }


                if (fonte.Contains("SCO"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l6 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("SCO")
                              select new servico
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
                        l6 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("SCO")
                              select new servico
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
                        l6 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("SCO")
                              select new servico
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

                }


                if (fonte.Contains("SINAP-ND"))
                {

                    if (string.IsNullOrEmpty(p2))
                    {

                        l7 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.fonte.Contains("SINAP-ND")
                              select new servico
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
                        l7 = (from p in dc.tb_servico
                              where p.nome.Contains(p2) && p.fonte.Contains("SINAP-ND")
                              select new servico
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
                        l7 = (from p in dc.tb_servico
                              where p.nome.Contains(p1) && p.nome.Contains(p2) && p.fonte.Contains("SINAP-ND")
                              select new servico
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

                }





                List<servico> newList = new List<servico>();
                List<servico> newList2 = new List<servico>();

                if (l1 != null)
                {
                    newList.AddRange(l1);
                }

                if (l2 != null)
                {
                    newList.AddRange(l2);
                }
                if (l3 != null)
                {
                    newList.AddRange(l3);
                }
                if (l4 != null)
                {
                    newList.AddRange(l4);
                }
                if (l5 != null)
                {
                    newList.AddRange(l5);
                }
                if (l6 != null)
                {
                    newList.AddRange(l6);
                }
                if (l7 != null)
                {
                    newList.AddRange(l7);
                }

                 if (!fonte.Contains("M3"))
                {
                    newList2 = (from x in newList
                                where x.anoMes.Contains(anoMes)
                                select x).ToList();

                    return newList2.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
                }
                else
                {
                    newList2 = newList;
                    if (fonte.Contains("M3"))
                    {
                        newList2 = (from x in newList
                                    where x.fonte == "M3" && x.data >= dataContrato
                                    select x).ToList();
                    }
                    return newList2.OrderBy(x => x.fonte).ThenByDescending(x => x.preco).ToList();
                }


            }

        }

        [HttpGet]
        public IEnumerable<tb_servico> GetAllServicoCodigo(string fonte, string codigo)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_servico.Where((a => (a.fonte == fonte && a.codigo == codigo))).OrderByDescending(p => p.preco) select p;
                return user.ToList();
            }

        }

        [HttpGet]
        public IEnumerable<tb_servico> GetAllServicoM3()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_servico.Where((a => (a.fonte == "M3"))).OrderBy(p => p.nome) select p;
                return user.ToList();
            }

        }

        //  Obter dados por categoria agrupada EXEMPLO --------------------------------------------------------------------
        //[HttpGet]
        //public List<categoria> GetAllCategoria()
        //{
        //        using (var dc = new manutEntities())
        //        {
        //            var user = (from c in dc.tb_servico
        //                        group c by c.categoria into myGroupTemp
        //                        select new categoria { nome = myGroupTemp.Key }).ToList();

        //            return user;
        //        }
        //}

        //[HttpGet]
        //public List<categoria> GetAllfonte()
        //{
        //    using (var dc = new manutEntities())
        //    {
        //        var user = (from c in dc.tb_servico
        //                    group c by c.fonte into myGroupTemp
        //                    select new categoria { nome = myGroupTemp.Key }).ToList();

        //        return user;
        //    }
        //}

        //  FIM - Obter dados por categoria agrupada EXEMPLO --------------------------------------------------------------------


        [HttpPost]
        public string IncluirServico()
        {
            var data = DateTime.Now;
            using (var dc = new manutEntities())
            {
                var k = new tb_servico
                {
                    anoMes = "",
                    codigo = "",
                    fonte = "M3",
                    nome = HttpContext.Current.Request.Form["nome"].ToString().Trim(),
                    preco = Convert.ToDouble(HttpContext.Current.Request.Form["preco"].ToString()),
                    unidade = HttpContext.Current.Request.Form["unidade"].ToString().Trim(),
                    data = data.Date
                };

                dc.tb_servico.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);
                var codigo = "M3" + auto.ToString("########0");
                k.codigo = codigo;
                dc.SaveChanges();

                return auto.ToString("#######0");
            }
        }

        [HttpDelete]
        public string RemoverServico()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_servico.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.tb_servico.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }



        [HttpGet]
        public List<servico> GetServicoNome(string pesq1, string pesq2, string fonte, string anoMes, DateTime? dataContrato)
        {

            List<servico> l1 = null;

            var xwer = pesq2;

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

                    l1 = (from p in dc.tb_servico
                          where p.nome.Contains(p1) && fonte.Contains(p.fonte)
                          select new servico
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
                    l1 = (from p in dc.tb_servico
                          where p.nome.Contains(p2) && fonte.Contains(p.fonte)
                          select new servico
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
                    l1 = (from p in dc.tb_servico
                          where p.nome.Contains(p1) && p.nome.Contains(p2) && fonte.Contains(p.fonte)
                          select new servico
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



            }


            List<servico> newList = new List<servico>();

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
        public List<servico> GetServicoCodigo(string pesq1, string pesq2, string fonte, string anoMes, DateTime dataContrato)
        {

            List<servico> l1 = null;


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

                    l1 = (from p in dc.tb_servico
                          where p.codigo.Contains(p1) && fonte.Contains(p.fonte)
                          select new servico
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,

                          }).ToList(); ;
                }

                if (string.IsNullOrEmpty(p1))
                {
                    l1 = (from p in dc.tb_servico
                          where p.codigo.Contains(p2) && fonte.Contains(p.fonte)
                          select new servico
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                          }).ToList(); ;
                }

                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                {
                    l1 = (from p in dc.tb_servico
                          where p.codigo.Contains(p1) && p.codigo.Contains(p2) && fonte.Contains(p.fonte)
                          select new servico
                          {
                              anoMes = p.anoMes,
                              autonumero = p.autonumero,
                              codigo = p.codigo,
                              fonte = p.fonte,
                              nome = p.nome,
                              preco = p.preco,
                              unidade = p.unidade,
                          }).ToList(); ;
                }



            }

            List<servico> newList = new List<servico>();

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


    }

}

public class categoria
{
    public string nome { get; set; }
}

