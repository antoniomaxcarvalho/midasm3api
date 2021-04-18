using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataFeriadoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_feriado> GetFeriado(Int32 autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_feriado.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }
        }


        public IEnumerable<tb_feriado> GetAllFeriado(int ano)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_feriado.Where(a => (a.data.Value.Year == ano)) orderby p.data select p;
                return user.ToList(); ;
            }
        }

        public static List<DiaUtil> GetDiasUteisMes(int ano, int mes, int nroMaxDiasUteis)
        {

            using (var dc = new manutEntities())
            {

                var listaDiaUtil = new List<DiaUtil>();

                var data = DateTime.Now;
                int ultimoDiaDoMes = DateTime.DaysInMonth(ano, mes);

                //DateTime com o último dia do mês
                DateTime ultimoDataDoMes = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));


                var feriadosDoMes = from p in dc.tb_feriado.Where(a => (a.data.Value.Month == mes)) orderby p.data select p;

                var cont = 1;
                for (int diaDomes = 1; diaDomes <= ultimoDiaDoMes; diaDomes++)
                {
                    var dataP1 = ano.ToString("####") + "-" + mes.ToString("0#") + "-" + diaDomes.ToString("0#");
                    var dataPrevista = DateTime.Parse(dataP1, new CultureInfo("en-US", true));
                    var pularData = false;

                    if (dataPrevista.DayOfWeek == DayOfWeek.Sunday)
                    {
                        pularData = true;
                    }
                    else
                    {
                        if (dataPrevista.DayOfWeek == DayOfWeek.Saturday)
                        {
                            pularData = true;
                        }
                    }

                    if (!pularData)
                    {
                        foreach (var valueFeriado in feriadosDoMes)
                        {
                            if (Convert.ToDateTime(valueFeriado.data) == dataPrevista)
                            {
                                pularData = true;
                                break;
                            }
                        }
                        if (!pularData)
                        {

                            listaDiaUtil.Add(new DiaUtil { diaUtil = cont, data = dataPrevista });
                            cont++;
                            if (cont > nroMaxDiasUteis) break;
                        }
                    }


                }

                return listaDiaUtil.ToList(); ;
            }



        }



        public static List<DiaUtil> GetDiasUteisIntervalo(DateTime dataInicio, DateTime dataFim, int nroMaxDiasUteis)
        {

            using (var dc = new manutEntities())
            {

                var listaDiaUtil = new List<DiaUtil>();



                //  INICIO  Mês INCIAL  ----------------------------------------------------------

                var ano = dataInicio.Year;
                var mes = dataInicio.Month;
                var diaInicial = dataInicio.Day;

                var data = DateTime.Now;
                int ultimoDiaDoMes = DateTime.DaysInMonth(ano, mes);

                //DateTime com o último dia do mês
                DateTime ultimoDataDoMes = new DateTime(ano, mes, ultimoDiaDoMes);


                var feriadosDoMes = from p in dc.tb_feriado.Where(a => (a.data.Value.Month == mes)) orderby p.data select p;

                var cont = 1;
                for (int diaDomes = diaInicial; diaDomes <= ultimoDiaDoMes; diaDomes++)
                {
                    var dataP1 = ano.ToString("####") + "-" + mes.ToString("0#") + "-" + diaDomes.ToString("0#");
                    var dataPrevista = DateTime.Parse(dataP1, new CultureInfo("en-US", true));
                    var pularData = false;

                    if (dataPrevista.DayOfWeek == DayOfWeek.Sunday)
                    {
                        pularData = true;
                    }
                    else
                    {
                        if (dataPrevista.DayOfWeek == DayOfWeek.Saturday)
                        {
                            pularData = true;
                        }
                    }

                    if (!pularData)
                    {
                        foreach (var valueFeriado in feriadosDoMes)
                        {
                            if (Convert.ToDateTime(valueFeriado.data) == dataPrevista)
                            {
                                pularData = true;
                                break;
                            }
                        }
                        if (!pularData)
                        {

                            listaDiaUtil.Add(new DiaUtil { diaUtil = cont, data = dataPrevista });
                            cont++;
                            if (cont > nroMaxDiasUteis) break;
                        }
                    }


                }

                //  FIM  Mês INCIAL  ----------------------------------------------------------

                //  INICIO SEGUNDO Mês   ----------------------------------------------------------

                ano = dataFim.Year;
                mes = dataFim.Month;
                diaInicial = 1;
                ultimoDiaDoMes = dataFim.Day;
                feriadosDoMes = from p in dc.tb_feriado.Where(a => (a.data.Value.Month == mes)) orderby p.data select p;

                for (int diaDomes = diaInicial; diaDomes <= ultimoDiaDoMes; diaDomes++)
                {
                    var dataP1 = ano.ToString("####") + "-" + mes.ToString("0#") + "-" + diaDomes.ToString("0#");
                    var dataPrevista = DateTime.Parse(dataP1, new CultureInfo("en-US", true));
                    var pularData = false;

                    if (dataPrevista.DayOfWeek == DayOfWeek.Sunday)
                    {
                        pularData = true;
                    }
                    else
                    {
                        if (dataPrevista.DayOfWeek == DayOfWeek.Saturday)
                        {
                            pularData = true;
                        }
                    }

                    if (!pularData)
                    {
                        foreach (var valueFeriado in feriadosDoMes)
                        {
                            if (Convert.ToDateTime(valueFeriado.data) == dataPrevista)
                            {
                                pularData = true;
                                break;
                            }
                        }
                        if (!pularData)
                        {

                            listaDiaUtil.Add(new DiaUtil { diaUtil = cont, data = dataPrevista });
                            cont++;
                            if (cont > nroMaxDiasUteis) break;
                        }
                    }


                }

                return listaDiaUtil.ToList(); ;
            }



        }



        [HttpDelete]
        public string CancelarFeriado()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_feriado.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {

                    dc.tb_feriado.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;
                }
            }

            return message;
        }


        [HttpPost]

        public void AtualizarFeriado()
        {
            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

                DateTime? dataFeriado = null;

                if (DataEquipamentoController.IsDate(HttpContext.Current.Request.Form["data"]))
                {
                    dataFeriado = Convert.ToDateTime(HttpContext.Current.Request.Form["data"]);
                }

                if (autonumero == 0)
                {

                    var linha = new tb_feriado();
                    linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                    linha.data = dataFeriado;
                    dc.tb_feriado.Add(linha);
                    dc.SaveChanges();
                }
                else
                {
                    var linha = dc.tb_feriado.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null)
                    {
                        linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                        linha.data = dataFeriado;
                        dc.tb_feriado.AddOrUpdate(linha);
                        dc.SaveChanges();
                    }
                }


            }

        }



    }
    public class DiaUtil
    {
        public int diaUtil { get; set; } // This is an automatic property
        public DateTime data { get; set; }
    }
}
