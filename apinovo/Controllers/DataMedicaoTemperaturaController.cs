using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataMedicaoTemperaturaController : ApiController
    {

        [HttpGet]
        public IEnumerable<medicao> GetAllMedicaoCliente(int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.medicao.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S") orderby p.id select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public string ExisteMedicaoClienteID(string id)
        {
            using (var dc = new manutEntities())
            {
                var qtde = (from p in dc.medicao.Where(a => a.id == id && a.cancelado != "S") select p).ToList().Count();
                if (qtde > 0)
                {
                    return "S";
                }
                else
                {
                    return "N";
                }

            }
        }

        [HttpGet]
        public IEnumerable<medicao> GetAllMedicaoClienteData(int autonumeroCliente, string d1, string d2)
        {
            var data1 = Convert.ToDateTime(d1);
            var data2 = Convert.ToDateTime(d2).AddDays(1);

            using (var dc = new manutEntities())
            {
                var user = from p in dc.medicao.Where(a => a.autonumeroCliente == autonumeroCliente && a.data >= data1 && a.data < data2) orderby p.id select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<medicao> GetAllMedicao()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.medicao orderby p.id select p;
                return user.ToList(); ;
            }
        }
        [HttpPost]
        public string IncluirMedicaox()
        {
            var autonumeroLocalAtendido = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroLocalAtendido"].ToString());
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString());
            var id = HttpContext.Current.Request.Form["id"].ToString().Trim();

            DateTime? dataMedicao = null;
            if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["dataMedicao"]))
            {
                dataMedicao = Convert.ToDateTime(HttpContext.Current.Request.Form["data"]);
            }

            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString().Trim();
            var nomeLocalAtendido = HttpContext.Current.Request.Form["nomeLocalAtendido"].ToString().Trim();
            var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();

            var temperatura = Convert.ToDecimal(HttpContext.Current.Request.Form["temperatura"].ToString());
            var umidade = Convert.ToDecimal(HttpContext.Current.Request.Form["umidade"].ToString());

            using (var dc = new manutEntities())
            {
                var soma = (from p in dc.medicao where p.id == id && p.cancelado != "S" select p).ToList();
                if (soma.Count > 0)
                {
                    return "";
                }

                var k = new medicao
                {
                    autonumeroLocalAtendido = autonumeroLocalAtendido,
                    autonumeroCliente = autonumeroCliente,
                    autonumeroUsuario = autonumeroUsuario,
                    data = dataMedicao,
                    id = id,
                    nomeCliente = nomeCliente,
                    nomeLocalAtendido = nomeLocalAtendido,
                    nomeUsuario = nomeUsuario,
                    temperatura = temperatura,
                    umidade = umidade,
                    cancelado = "N",
                };

                dc.medicao.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return "";
            }
        }
        [HttpDelete]
        public string CancelarMedicao()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.medicao.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    dc.medicao.Remove(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }



        public class med
        {
            public long autonumero { get; set; }
            public Nullable<int> autonumeroUsuario { get; set; }
            public string nomeUsuario { get; set; }
            public string id { get; set; }
            public Nullable<int> autonumeroCliente { get; set; }
            public string nomeCliente { get; set; }
            public Nullable<int> autonumeroLocalAtendido { get; set; }
            public string nomeLocalAtendido { get; set; }
            public string data { get; set; }
            public Nullable<decimal> temperatura { get; set; }
            public Nullable<decimal> umidade { get; set; }
            public string cancelado { get; set; }
        }


        [HttpPost]
        public string IncluirMedicao(List<med> itensJson)
        {
           using (var dc = new manutEntities())
            {
                foreach (var value in itensJson)
                {

                    try
                    {

                        var soma = (from p in dc.medicao where p.id == value.id && p.cancelado != "S" select p).ToList();
                        if (soma.Count == 0)
                        {
                            if (!string.IsNullOrEmpty(value.id))
                            {
                                DateTime? dataMedicao = null;
                                if (DataEquipamentoController.IsDate(value.data.ToString()))
                                {
                                    dataMedicao = Convert.ToDateTime(value.data);
                                }
                                else
                                {
                                    throw new ArgumentException("#Erro Medição: " + value.id);
                                }

                                Debug.WriteLine(value.data.ToString());
                                Debug.WriteLine(dataMedicao);


                                var k = new medicao
                                {
                                    autonumeroLocalAtendido = value.autonumeroLocalAtendido,
                                    autonumeroCliente = value.autonumeroCliente,
                                    autonumeroUsuario = value.autonumeroUsuario,
                                    data = dataMedicao,
                                    id = value.id,
                                    nomeCliente = value.nomeCliente,
                                    nomeLocalAtendido = value.nomeLocalAtendido,
                                    nomeUsuario = value.nomeUsuario,
                                    temperatura = value.temperatura,
                                    umidade = value.umidade,
                                    cancelado = "N",
                                };

                                Debug.WriteLine(k.data);

                                dc.medicao.Add(k);
                                dc.SaveChanges();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        var c2 = 1;
                        // MESMO COM ERRO, CONTINUE O LOOP
                    }

                }

                return null;
            }

        }





    }
}
