using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataTramitacaoController : ApiController
    {
        [HttpGet]
        public IEnumerable<tramitacao> GetAllTramitacao(int autonumeroGrupoTramitacao, int autonumeroCliente)
        {
            using (var dc = new manutEntities())
            {
                if (autonumeroCliente == 0)
                {
                    if (autonumeroGrupoTramitacao > 0)
                    {
                        var user = from p in dc.tramitacao.Where(a => a.autonumeroGrupoTramitacao == autonumeroGrupoTramitacao && a.cancelado != "S").OrderBy(p => p.nomeGrupoTramitacao).ThenBy(p => p.nomeCliente).ThenBy(p => p.intervaloMedicao) select p;
                        return user.ToList();
                    }
                    else
                    {
                        var user = from p in dc.tramitacao.Where(a => a.cancelado != "S").OrderBy(p => p.nomeGrupoTramitacao).ThenBy(p => p.nomeCliente).ThenBy(p => p.intervaloMedicao) select p;

                        return user.ToList();
                    }
                }
                else
                {
                    if (autonumeroGrupoTramitacao > 0)
                    {
                        var user = from p in dc.tramitacao.Where(a => a.autonumeroCliente == autonumeroCliente && a.autonumeroGrupoTramitacao == autonumeroGrupoTramitacao && a.cancelado != "S").OrderBy(p => p.nomeGrupoTramitacao).ThenBy(p => p.nomeCliente).ThenBy(p => p.intervaloMedicao) select p;
                        return user.ToList();
                    }
                    else
                    {
                        var user = from p in dc.tramitacao.Where(a => a.autonumeroCliente == autonumeroCliente && a.cancelado != "S").OrderBy(p => p.nomeGrupoTramitacao).ThenBy(p => p.nomeCliente).ThenBy(p => p.intervaloMedicao) select p;

                        return user.ToList();
                    }
                }
            }
        }

        [HttpPost]
        public string IncluirTramitacao()
        {
            var autonumeroGrupoTramitacao = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroGrupoTramitacao"].ToString());
            var nomeGrupoTramitacao = HttpContext.Current.Request.Form["nomeGrupoTramitacao"].ToString().Trim();
            var valor = Convert.ToDecimal(HttpContext.Current.Request.Form["valor"].ToString());
            var contrato = HttpContext.Current.Request.Form["contrato"].ToString();
            var destino = HttpContext.Current.Request.Form["destino"].ToString();
            var intervaloMedicao = HttpContext.Current.Request.Form["intervaloMedicao"].ToString();
            var nomeCliente = HttpContext.Current.Request.Form["nomeCliente"].ToString();
            var autonumeroCliente = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroCliente"].ToString());
            var nroProcessoPagamento = HttpContext.Current.Request.Form["nroProcessoPagamento"].ToString();


            var ultimaTramitacao = DateTime.Now;
            if (DataClienteController.IsDate(HttpContext.Current.Request.Form["ultimaTramitacao"].ToString()))
            {
                ultimaTramitacao = Convert.ToDateTime(HttpContext.Current.Request.Form["ultimaTramitacao"].ToString());

            }
            else
            {
                throw new ArgumentException("#Erro Data Inválida");
            }

            if (string.IsNullOrEmpty(nroProcessoPagamento))
            {
                nroProcessoPagamento = "-";
            }

            using (var dc = new manutEntities())
            {
                var k = new tramitacao
                {
                    nomeGrupoTramitacao = nomeGrupoTramitacao,
                    autonumeroGrupoTramitacao = autonumeroGrupoTramitacao,
                    contrato = contrato,
                    destino = destino,
                    intervaloMedicao = intervaloMedicao,
                    nomeCliente = nomeCliente,
                    autonumeroCliente = autonumeroCliente,
                    ultimaTramitacao = ultimaTramitacao,
                    valor = valor,
                    cancelado = "N",
                    nroProcessoPagamento = nroProcessoPagamento
                };

                dc.tramitacao.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }


        [HttpPost]
        public string AlterarNroProcesso()
        {
            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"].ToString());
            var nomeAlterarProcesso = HttpContext.Current.Request.Form["nomeAlterarProcesso"].ToString().Trim();


            using (var dc = new manutEntities())
            {
                var linha = dc.tramitacao.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.nroProcessoPagamento = nomeAlterarProcesso.Trim();

                }
                dc.tramitacao.AddOrUpdate(linha);
                dc.SaveChanges();

                return "";

            }
        }




        [HttpDelete]
        public string CancelarTramitacao()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tramitacao.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.tramitacao.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }

        [HttpGet]
        public IEnumerable<grupotramitacao> GetAllGrupoTramitacao()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.grupotramitacao.Where(p => p.cancelado != "S").OrderBy(p => p.nome) select p;
                return user.ToList();
            }
        }

        public string IncluirGrupoTramitacao()
        {
            //var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"].ToString());
            var nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();

            using (var dc = new manutEntities())
            {
                var k = new grupotramitacao
                {
                    nome = nome,
                    cancelado = "N"
                };

                dc.grupotramitacao.Add(k);
                dc.SaveChanges();
                var auto = Convert.ToInt32(k.autonumero);

                return auto.ToString("#######0");
            }
        }

        [HttpDelete]
        public string CancelarGrupoTramitacao()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt32(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.grupotramitacao.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {
                    linha.cancelado = "S";
                    dc.grupotramitacao.AddOrUpdate(linha);
                    dc.SaveChanges();
                    return string.Empty;
                }
            }

            return message;
        }



    }
}

