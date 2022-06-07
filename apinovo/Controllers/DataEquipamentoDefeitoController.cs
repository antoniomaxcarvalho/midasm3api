using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataEquipamentoDefeitoController : ApiController
    {
        [HttpGet]
        public IEnumerable<equipamentodefeito> GetEquipamentoDefeitoEquipamento(long autonumeroEquipamento)
        {
            using (var dc = new manutEntities())
            {
                var user = (from p in dc.equipamentodefeito.Where(a => a.autonumeroEquipamento == autonumeroEquipamento && a.cancelado != "S") select p).OrderBy(p => p.dataInicio).ThenBy(p => p.horaInicio);
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<equipamentodefeito> GetEquipamentoDefeitoAutonumero(int autonumero)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.equipamentodefeito.Where(a => a.autonumero == autonumero && a.cancelado != "S") select p;
                return user.ToList(); ;
            }
        }

        [HttpPost]
        public tb_cadastro IncluirAlterarEquipamentoDefeito()
        {
            var c = 1;
            var ListResposta = new List<ResposatEquipamentoDefeito>();
            var resp = new ResposatEquipamentoDefeito();


            var auto2 = HttpContext.Current.Request.Form["autonumero"].ToString();
            if (string.IsNullOrEmpty(auto2))
            {
                auto2 = "0";
            }
            var autonumero = Convert.ToInt32(auto2);


            var autonumeroEquipamentoDefeito = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroEquipamentoDefeito"].ToString());

            DateTime dataInicio = DateTime.Now;
          ////  Debug.WriteLine("------------------");
            if (IsDate(HttpContext.Current.Request.Form["dataInicio"].ToString()))
            {
                dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
            }
            else
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Ocorrência com problemas"));
            }
          ////  Debug.WriteLine(dataInicio);
            DateTime dataFim = DateTime.Now;
            if (IsDate(HttpContext.Current.Request.Form["dataFim"].ToString()))
            {
                dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());
            }
            else
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Final com problemas"));
            }


            //var dataInstalacao = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString().PadLeft(4, '0');

            var dataInstalacao = dataInicio;

            if (string.IsNullOrEmpty(HttpContext.Current.Request.Form["dataInstalacao"].ToString()))
            {
                using (var dc = new manutEntities())
                {
                    var linha = dc.tb_cadastro.Find(autonumero); // sempre irá procurar pela chave primaria

                    if (linha != null)
                    {
                        if (IsDate(linha.dataInstalacao.ToString()))
                        {
                            dataInstalacao = (DateTime) linha.dataInstalacao;
                        }

                    }
                }
            }


            if (dataInstalacao != null)
            {
                if (IsDate(HttpContext.Current.Request.Form["dataInstalacao"].ToString()))
                {
                    dataInstalacao =  Convert.ToDateTime( HttpContext.Current.Request.Form["dataInstalacao"].ToString());
                }
            }
            else
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Instalação com problemas"));
            }

          ////  Debug.WriteLine(dataInicio);
            if (dataInicio > dataFim)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data informada errada"));
            }


            var hhoraInicio = HttpContext.Current.Request.Form["horaInicio"].ToString();
            var hhoraFim = HttpContext.Current.Request.Form["horaFim"].ToString();


            string input = hhoraInicio.ToString().Trim();
            var parts = input.Split(':');
            var hours = Int32.Parse(parts[0]);
            var minutes = Int32.Parse(parts[1]);

            var hInicio = new TimeSpan(hours, minutes, 0);

            input = hhoraFim.ToString().Trim();
            parts = input.Split(':');
            hours = Int32.Parse(parts[0]);
            minutes = Int32.Parse(parts[1]);

            var hFim = new TimeSpan(hours, minutes, 0);


            var valorEquipamentoNovo = Convert.ToDecimal(HttpContext.Current.Request.Form["valorEquipamentoNovo"].ToString());


          ////  Debug.WriteLine(dataInicio);
            var totHParado = CalcularTotalHoras(dataInicio, dataFim, hhoraInicio, hhoraFim);

            var secondsParado = (long)TimeSpan.Parse(totHParado.ToString().Trim()).TotalSeconds;



            // Calcular Tempo FUNCIONANDO ---------------------------------------------

            using (var dc = new manutEntities())
            {
                var ultimoRegistro = (from p in dc.equipamentodefeito.Where(a => a.autonumeroEquipamento == autonumero && a.cancelado != "S") select p)
                                .OrderByDescending(p => p.dataInicio).ThenBy(p => p.horaFim).FirstOrDefault();

                if (ultimoRegistro == null)
                {

                    dataFim = dataInicio;
                    hhoraFim = hhoraInicio;

                    dataInicio = dataInstalacao;
                    //dataInicio = Convert.ToDateTime(dataInstalacao);
                    //hhoraInicio = "00:00";

                }
                else
                {


                    if (ultimoRegistro.dataFim > dataInicio)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data informada inferior a última 3m ocorrência"));
                    }

                    dataFim = dataInicio;
                    hhoraFim = hhoraInicio;

                    dataInicio = (DateTime)ultimoRegistro.dataFim;
                    hhoraInicio = ((TimeSpan)ultimoRegistro.horaFim).ToString(@"hh\:mm");

                }
            }

            //Debug.WriteLine(dataInicio);
            //Debug.WriteLine(hhoraInicio);
            //Debug.WriteLine(dataFim);
            //Debug.WriteLine(hhoraFim);
           
          ////  Debug.WriteLine(dataInicio);
            var totHFuncionando = CalcularTotalHoras(dataInicio, dataFim, hhoraInicio, hhoraFim);

            var seconds = (long)TimeSpan.Parse(totHFuncionando.ToString().Trim()).TotalSeconds;

            // FIM - Calcular Tempo FUNCIONANDO ---------------------------------------------

            if (autonumeroEquipamentoDefeito == 0)
            {
                using (var dc = new manutEntities())
                {
                    var k = new equipamentodefeito
                    {
                        autonumeroEquipamento = autonumero,
                        totalHorasFuncionando = (decimal)totHFuncionando.TotalHours,
                        totalSegundosFuncionando = seconds,
                        totalHorasParado = (decimal)totHParado.TotalHours,
                        totalSegundosParado = secondsParado,
                        cancelado = "N",
                        horaInicio = hInicio,
                        horaFim = hFim,
                        dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString()),
                        dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString()),

                    };

                    dc.equipamentodefeito.Add(k);
                    dc.SaveChanges();
                    var auto = Convert.ToInt32(k.autonumero);

                    autonumeroEquipamentoDefeito = auto;
                }
            }
            else
            {
                using (var dc = new manutEntities())
                {

                    var linha = dc.equipamentodefeito.Find(autonumeroEquipamentoDefeito); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.horaFim = hFim;
                        linha.horaInicio = hInicio;
                        linha.dataFim = Convert.ToDateTime(HttpContext.Current.Request.Form["dataFim"].ToString());
                        linha.dataInicio = Convert.ToDateTime(HttpContext.Current.Request.Form["dataInicio"].ToString());
                        linha.totalHorasFuncionando = (decimal)totHFuncionando.TotalHours;
                        linha.totalSegundosFuncionando = seconds;
                        linha.totalHorasParado = (decimal)totHParado.TotalHours;
                        linha.totalSegundosParado = secondsParado;
                        dc.equipamentodefeito.AddOrUpdate(linha);
                        dc.SaveChanges();

                    }
                }
            }

            //CalcularIndices(autonumero, valorEquipamentoNovo);

            //resp.autonumeroEquipamentoDefeito = autonumeroEquipamentoDefeito;
            //resp.dataInicio = dataInicio;
            //resp.horaInicio = hInicio;
            //resp.dataFim = dataFim;
            //resp.horaFim = hFim;
            //resp.totalHorasFuncionando = totHFuncionando;
            //resp.totalHorasParado = totHParado;

            //ListResposta.Add(resp);

            return CalcularIndices(autonumero, valorEquipamentoNovo); ;

        }

        [HttpDelete]
        public tb_cadastro CancelarEquipamentoDefeitoUltimoLancamento()
        {

            var autonumeroEquipamento = Convert.ToInt64(HttpContext.Current.Request.Form["autonumeroEquipamento"]);
            var valorEquipamentoNovo = Convert.ToDecimal(HttpContext.Current.Request.Form["valorEquipamentoNovo"].ToString());

            using (var dc = new manutEntities())
            {
                var ultimoRegistro = (from p in dc.equipamentodefeito.Where(a => a.autonumeroEquipamento == autonumeroEquipamento && a.cancelado != "S") select p)
                                .OrderByDescending(p => p.dataInicio).ThenBy(p => p.horaFim).FirstOrDefault();

                if (ultimoRegistro != null)
                {
                    var linha = dc.equipamentodefeito.Find(ultimoRegistro.autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.cancelado = "S";
                        dc.equipamentodefeito.AddOrUpdate(linha);
                        dc.SaveChanges();

                    }
                }

                return CalcularIndices(autonumeroEquipamento, valorEquipamentoNovo); ;
            }
        }

        public TimeSpan CalcularTotalHoras(DateTime dataInicio, DateTime dataFim, string hhoraInicio, string hhoraFim)
        {
          ////  Debug.WriteLine(dataInicio);
          ////  Debug.WriteLine(dataFim);
          ////  Debug.WriteLine(hhoraInicio);
          ////  Debug.WriteLine(hhoraFim);
            var totalHoras = new TimeSpan(0, 0, 0);

            // Pegar Horas maior que 24H ----------------------------------------------------------
            string input = totalHoras.ToString().Trim();
            var parts = input.Split(':');
            var hours = Int32.Parse(parts[0]);
            var minutes = Int32.Parse(parts[1]);



            // Fim - Pegar Horas maior que 24H ----------------------------------------------------------


            var horaInicio = new TimeSpan(0, 0, 0);
            var horaFim = new TimeSpan(0, 0, 0);

            var hInicio = new TimeSpan(0, 0, 0);
            var hFim = new TimeSpan(0, 0, 0);


            if (!string.IsNullOrEmpty(hhoraInicio) && !string.IsNullOrEmpty(hhoraFim) && dataInicio == dataFim)
            {

                // Pegar Horas maior que 24H ----------------------------------------------------------
                input = hhoraInicio;
                parts = input.Split(':');
                hours = Int32.Parse(parts[0]);
                minutes = Int32.Parse(parts[1]);
                horaInicio = new TimeSpan(hours, minutes, 0);
                // Fim - Pegar Horas maior que 24H ----------------------------------------------------------

                // Pegar Horas maior que 24H ----------------------------------------------------------
                input = hhoraFim;
                parts = input.Split(':');
                hours = Int32.Parse(parts[0]);
                minutes = Int32.Parse(parts[1]);
                horaFim = new TimeSpan(hours, minutes, 0);
                // Fim - Pegar Horas maior que 24H ----------------------------------------------------------

                hInicio = horaInicio;
                hFim = horaFim;

                totalHoras = horaFim - horaInicio;

            }
            else
            {

                input = hhoraInicio;
                parts = input.Split(':');
                hours = Int32.Parse(parts[0]);
                minutes = Int32.Parse(parts[1]);

                var ano = dataInicio.Year;
                var mes = dataInicio.Month;
                var dia = dataInicio.Day;

                var dt1 = new DateTime(ano, mes, dia, hours, minutes, 00);

                input = hhoraFim;
                parts = input.Split(':');
                hours = Int32.Parse(parts[0]);
                minutes = Int32.Parse(parts[1]);

                ano = dataFim.Year;
                mes = dataFim.Month;
                dia = dataFim.Day;

                var dt2 = new DateTime(ano, mes, dia, hours, minutes, 00);

              ////  Debug.WriteLine(dt1);
              ////  Debug.WriteLine(dt2);

                TimeSpan ts = dt2 - dt1;
              ////  Debug.WriteLine(ts);

                totalHoras = ts;

            }

            return totalHoras;

        }


        public tb_cadastro CalcularIndices(long autonumeroEquipamento, decimal valorEquipamentoNovo)
        {
            var c = 1;
            using (var dc = new manutEntities())
            {

                var e = dc.equipamentodefeito.Where(a => a.autonumeroEquipamento == autonumeroEquipamento && a.cancelado != "S").ToList();

                var qtde = e.Count();
                var somatorioFuncionando = (double)e.Sum(x => x.totalSegundosFuncionando);
                var somatorioParado = (double)e.Sum(x => x.totalSegundosParado);

                TimeSpan t = TimeSpan.FromSeconds(somatorioFuncionando);
                var totalHorasFuncionando = (decimal)t.TotalHours;

                t = TimeSpan.FromSeconds(somatorioParado);
                var totalHorasParado = (decimal)t.TotalHours;

                // MTBF:  Mean Time Between Failures - O MTBF  tempo médio entre falhas - ótima forma de mensurar a confiabilidade da máquina.
                // O recomendado é calcular 70% do tempo médio de falhas para realizar essa inspeção. Ou seja, 
                // se o motor elétrico apresenta um MTBF de 181,6 horas, a cada 127,1 horas (181,6 x 0,7) deve-se realizar a inspeção no equipamento.
                // MTBF = tempo de atividade operacional total entre falhas / número total de falhas

                //Principais erros cometidos:

                //Somar o MTBF de todos os equipamentos para encontrar a média global;
                //Calcular o MTBF em equipamentos irreparáveis;
                //Zerar o MTBF a cada mês(é preciso somá-lo).


                var MTBF = 0m;
                if (qtde > 0)
                {
                    MTBF = totalHorasFuncionando / qtde;
                }

                // MTTR: Mean Time To Repair - MTTR indica o tempo médio para reparo. Ao contrário do MTBF, quanto menor o MTTR, melhor, portanto devemos trabalhar para mantê-lo baixo.
                // MTTR: é a quantidade média de tempo necessária para reparar um sistema e restaurá - lo à funcionalidade total
                // MTTR= tempo total gasto em reparos durante um determinado período/número de reparos
                var MTTR = 0m;
                if (qtde > 0)
                {
                    MTTR = totalHorasParado / qtde;
                }

                // Tempo médio de falha - Calculado apenas para equipamento que NÃO PODEM SER REPARADOS
                // MTTF = total de horas de operação / número total de falhas
                //var MTTF = 0m;
                //if (qtde > 0)
                //{
                //    var tempoTotal = (totalHorasParado + totalHorasFuncionando);
                //    MTTF = (tempoTotal) / qtde;
                //}



                // Disponibilidade: refere-se à capacidade de um item de estar em condições de executar uma certa função em um dado instante ou durante um intervalo de tempo determinado.
                // uma boa disponibilidade é aquela acima de 90%. 
                var disponibilidade = 0m;
                if (MTBF + MTTR > 0)
                {
                    disponibilidade = (MTBF / (MTBF + MTTR)) * 100;
                }


                // Confiabilidade: é a probabilidade de um item desempenhar sua função especificada no projeto de acordo com as 
                // condições de operação e durante um intervalo específico de tempo.
                // — isto é, a probabilidade de ele operar normalmente sem falhar
                var confiabilidade = 0.0; 
                if (MTBF > 0)
                {
                    var taxaDeFalhas = 1 / MTBF;
                    var seteDiasEmHoras = 7 * 24; // Confiabilidade do equipamento PARA 1 SEMANA de bom funcionamento
                    var k = (double)taxaDeFalhas * seteDiasEmHoras * (-1);

                    confiabilidade = Math.Exp(k) * 100;  // (Log Nperiano a bbase é 2,7182818285)
                }

                //  CPMV: Custo de manutenção sobre valor de reposição -  identificar se seria mais vantajoso manter o ativo ou comprar um novo
                //  O valor máximo aceitável para esse indicador é 6% num período de um ano. No entanto, o limite pode depender da análise do equipamento 
                //  em alguns casos, 2.5% já é o bastante. Se encontrarmos um número maior, significa que será mais vantajoso comprar um novo equipamento do que continuar mantendo o antigo.
                var valorGastoAtéHojeComManutencao = dc.tb_os.Where(a => a.codigoEquipamento == autonumeroEquipamento && a.cancelado != "S").Sum(x => x.valor) ?? 0;
                var CPMV = 0m;
                if (valorEquipamentoNovo > 0)
                {
                    CPMV = (valorGastoAtéHojeComManutencao / valorEquipamentoNovo) * 100;
                }

                var linha = dc.tb_cadastro.Find(autonumeroEquipamento); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
     
                    linha.mtbf = MTBF;
                    linha.mttr = MTTR;
                    linha.cpmv = CPMV;
                    linha.confiabilidade = (decimal)confiabilidade;
                    linha.disponibilidade = disponibilidade;
                    dc.tb_cadastro.AddOrUpdate(linha);
                    dc.SaveChanges();
                }

                return linha;

            }

        }

        public class ResposatEquipamentoDefeito
        {
            public long autonumeroEquipamentoDefeito { get; set; }
            public DateTime? dataInicio { get; set; }
            public DateTime? dataFim { get; set; }
            public TimeSpan? horaInicio { get; set; }
            public TimeSpan? horaFim { get; set; }
            public TimeSpan? totalHorasFuncionando { get; set; }
            public TimeSpan? totalHorasParado { get; set; }
        }

        public static bool IsDate(string MyString)
        {
            try
            {
                DateTime.Parse(MyString); // or Double.Parse if you want to use double
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
