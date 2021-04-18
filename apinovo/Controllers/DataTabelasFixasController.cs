using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace apinovo.Controllers
{
    public class DataTabelasFixasController : ApiController
    {
        [HttpGet]
        public IEnumerable<tb_sistema> GetAllSistema()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_sistema.Where(p => p.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_subsistema> GetAllSubSistema()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_subsistema.Where(p =>p.cancelado != "S") orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_setor> GetAllSetor()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_setor orderby p.nome select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<tb_prioridade> GetAllPrioridade()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_prioridade orderby p.nome select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<tb_tipo_servico> GetAllTipoServico()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_tipo_servico orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_status_os> GetAllStatus()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_status_os orderby p.nome select p;
                return user.ToList(); ;
            }
        }
        [HttpGet]
        public IEnumerable<tb_equipe> GetAllEquipe()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_equipe orderby p.autonumero select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_categoriaservico> GetAllCategorias()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_categoriaservico orderby p.nome select p;
                return user.ToList(); ;
            }
        }

        [HttpGet]
        public IEnumerable<tb_situacaoordemservico> GetAllSituacaoOrdemServico()
        {
            using (var dc = new manutEntities())
            {
                var user = from p in dc.tb_situacaoordemservico orderby p.nome select p;
                return user.ToList(); ;
            }
        }

    }
}
