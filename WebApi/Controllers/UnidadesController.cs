using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UnidadesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Models.Request.ReqUnidades> lst = new List<Models.Request.ReqUnidades>();
            using (Models.InventarioEntities1 db = new Models.InventarioEntities1())
            {
                lst = (from data in db.Unidades
                       select new Models.Request.ReqUnidades
                       {
                           codigo = data.coduni,
                           descripcion = data.desuni,
                           estado = data.status

                       }).ToList();
            }
            return Ok(lst);
        }

        [HttpPost]
        public IHttpActionResult Add(Models.Request.ReqUnidades model)
        {
            using (Models.InventarioEntities1 db = new Models.InventarioEntities1())
            {
                var oUnidades = new Models.Unidades();
                oUnidades.coduni = model.codigo;
                oUnidades.desuni = model.descripcion;
                oUnidades.status = model.estado;
                db.Unidades.Add(oUnidades);
                db.SaveChanges();
            }

            return Ok("Post con exito");
        }
    }
}
