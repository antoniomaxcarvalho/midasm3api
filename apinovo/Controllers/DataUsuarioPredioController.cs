using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataUsuarioPredioController : ApiController
    {

        [HttpGet]
        public IEnumerable<usuariopredio> GetUsuarioPredioAutonumero(int autonumero)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.usuariopredio.Where(a => a.autonumero == autonumero) select p;
                return user.ToList(); ;
            }

        }


        [HttpGet]
        public IEnumerable<usuariopredio> GetTodosUsuarioPredio()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                var user = from p in dc.usuariopredio select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<usuariopredio> GetAllUsuarioPredio( int autonumeroUsuario)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.usuariopredio.Where(a => a.autonumeroUsuario == autonumeroUsuario) orderby p.nomePredio select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<usuariopredio> GetAllPredioUsuario(int autonumeroPredio)
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.usuariopredio.Where(a => a.autonumeroPredio == autonumeroPredio) orderby p.nomeUsuario select p;
                return user.ToList(); ;
            }

        }


        [HttpDelete]
        public string CancelarUsuarioPredio()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.usuariopredio.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null)
                {

                    dc.usuariopredio.Remove(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }

        [HttpPost]
        public void IncluirUsuarioPredio()
        {

            using (var dc = new manutEntities())
            {

                var autonumeroUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroUsuario"].ToString().Trim());
                var nomeUsuario = HttpContext.Current.Request.Form["nomeUsuario"].ToString().Trim();
                var nomePredio = HttpContext.Current.Request.Form["nomePredio"].ToString().Trim();
                var autonumeroPredio = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroPredio"].ToString().Trim());


                var Funcionario = new usuariopredio
                {
                    autonumeroPredio = autonumeroPredio,
                    nomePredio = nomePredio,
                    autonumeroUsuario = autonumeroUsuario,
                    nomeUsuario = nomeUsuario

                };

                dc.usuariopredio.Add(Funcionario);
                dc.SaveChanges();
                var auto = Convert.ToInt32(Funcionario.autonumero);



            }

        }


    }
}
