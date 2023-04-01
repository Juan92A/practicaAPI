using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEquiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public TipoEquiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

      
        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<TipoEquipos> listadoTipoEquipo = _equiposContext.tipo_equipo.Where(x => x.estado == "A").ToList();

            if (listadoTipoEquipo.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoTipoEquipo);
        }
     
        [HttpPost]
        [Route("Add")]
        public IActionResult Crear([FromBody] TipoEquipos tipoEquipo)
        {

            try
            {
                _equiposContext.tipo_equipo.Add(tipoEquipo);
                _equiposContext.SaveChanges();

                return Ok(tipoEquipo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] TipoEquipos tipoEquipo)
        {
            TipoEquipos? tipoEquipoExistente = _equiposContext.tipo_equipo.Find(id);

            if (tipoEquipoExistente == null)
            {
                return NotFound();
            }

            tipoEquipoExistente.descripcion = tipoEquipo.descripcion;
            tipoEquipoExistente.estado = tipoEquipo.estado;

            _equiposContext.Entry(tipoEquipoExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(tipoEquipoExistente);

        }

      
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarTipoEquipo(int id)
        {
            TipoEquipos? tipoEquipoExistente = _equiposContext.tipo_equipo.Find(id);

            if (tipoEquipoExistente == null) return NotFound();

            tipoEquipoExistente.estado = "C";

            _equiposContext.Entry(tipoEquipoExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(tipoEquipoExistente);

        }
    
    }
}
