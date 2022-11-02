using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace apinovo.Controllers
{
    public class DataUsuarioController : ApiController
    {
        static DateTime dummy;

        [HttpGet]
        public IEnumerable<tb_usuario> GetUsuarioSenha(string login, string senha)
        {
            var c = 1;


            //You this logig within your method.
            if (dummy != DateTime.Today)
            {
                dummy = DateTime.Today;

                var k = new DataOsController();
                k.AcertarOSDesabilitado();
           

            }


            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_usuario.Where((a => (a.login == login) && a.senha == senha && a.cancelado != "S")) select p;
                return user.ToList(); ;
            }

        }


        public IEnumerable<tb_usuario> GetAllUsuario()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_usuario.Where((a => (a.cancelado != "S"))) orderby p.nome select p;
                return user.ToList(); ;
            }

        }

        public IEnumerable<tb_tipo_usuario> GetAllTipoUsuario()
        {

            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_tipo_usuario orderby p.tipoUsuario select p;
                return user.ToList(); ;
            }

        }


        [HttpDelete]
        public string CancelarUsuario()
        {
            var message = "* Erro Não foi possível atualizar o banco de dados";

            var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

            using (var dc = new manutEntities())
            {
                var linha = dc.tb_usuario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.cancelado = "S";
                    dc.tb_usuario.AddOrUpdate(linha);
                    dc.SaveChanges();

                    return string.Empty;

                }
            }

            return message;

        }


        [HttpPost]
        //public string AddPedido(Int64 autonumeroProduto, string codBarra, string nomeProduto,
        //    int qtdePedida, double precoItem, Int64 autonumeroCliente, string nomeCliente, string cpfCnpj)
        public string IncluirUsuario()
        {
            using (var dc = new manutEntities())
            {
                var usuario = new tb_usuario
                {
                    login = "",
                    senha = "",
                    tipoUsuario = "Usuario final",
                    nome = "",
                    email = "",
                    telefone = "",
                    matricula = "",
                    cancelado = "N",
                    cliente = "",
                    autonumeroTipoUsuario = 4

                };

                dc.tb_usuario.Add(usuario);                
                dc.SaveChanges();
                var auto = Convert.ToInt32(usuario.autonumero);

                return auto.ToString("#######0");
            }
        }

        [HttpPost]
        public void AtualizarUsuario()
        {
            var c = 1;
            using (var dc = new manutEntities())
            {
                try
                {
                    var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

                    var linha = dc.tb_usuario.Find(autonumero); // sempre irá procurar pela chave primaria
                    if (linha != null && linha.cancelado != "S")
                    {
                        linha.login = HttpContext.Current.Request.Form["login"].ToString().Trim();
                        linha.senha = HttpContext.Current.Request.Form["senha"].ToString().Trim();
                        linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                        linha.tipoUsuario = HttpContext.Current.Request.Form["tipoUsuario"].ToString().Trim();
                        linha.nome = HttpContext.Current.Request.Form["nome"].ToString().Trim();
                        linha.email = HttpContext.Current.Request.Form["email"].ToString().Trim();
                        linha.matricula = HttpContext.Current.Request.Form["matricula"].ToString().Trim();
                        linha.telefone = HttpContext.Current.Request.Form["telefone"].ToString().Trim();
                        linha.telefone = HttpContext.Current.Request.Form["telefone"].ToString().Trim();
                        linha.cliente = HttpContext.Current.Request.Form["cliente"].ToString().Trim();
                        linha.autonumeroTipoUsuario = Convert.ToInt32(HttpContext.Current.Request.Form["autonumeroTipoUsuario"].ToString().Trim());

                        dc.tb_usuario.AddOrUpdate(linha);
                        dc.SaveChanges();

                        dc.tb_os.Where(x => x.autonumeroUsuario == autonumero).ToList().ForEach(x =>
                        {
                            x.nomeUsuario = linha.nome;
                        });
                        dc.SaveChanges();

                    }
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException != null
                        ? ex.InnerException.ToString().Substring(0, 130) + " - DataprodutoController SaveFilesFoto"
                        : ex.Message + " - DataprodutoController SaveFilesFoto";

                }

            }

        }

        [HttpPost]
        public void AtualizarSenha()
        {

            using (var dc = new manutEntities())
            {
                var autonumero = Convert.ToInt64(HttpContext.Current.Request.Form["autonumero"]);

                var linha = dc.tb_usuario.Find(autonumero); // sempre irá procurar pela chave primaria
                if (linha != null && linha.cancelado != "S")
                {
                    linha.senha = HttpContext.Current.Request.Form["senha"].ToString().Trim();

                    dc.tb_usuario.AddOrUpdate(linha);
                    dc.SaveChanges();

                }
            }

        }

        [HttpGet]
        public IEnumerable<tb_usuario> GetUsuarioCliente(string siglaCliente)
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_usuario.Where(a => (a.cliente.Contains(siglaCliente) && (a.tipoUsuario.Contains("Medio") || a.tipoUsuario.Contains("Assistente")) )) select p;
                return user.ToList(); ;
            }
        }



    }
}
