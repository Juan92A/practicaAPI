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
    public class estadosEquiposController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public estadosEquiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }
       
        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<estadosEquipos> listadoEstadoEquipos = _equiposContext.estados_equipo.Where(x => x.estado == "A").ToList();

            if (listadoEstadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEstadoEquipos);
        }
      

        [HttpPost]
        [Route("Add")]
        public IActionResult Crear([FromBody] estadosEquipos estadoEquipo)
        {

            try
            {
                _equiposContext.estados_equipo.Add(estadoEquipo);
                _equiposContext.SaveChanges();

                return Ok(estadoEquipo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
 

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] estadosEquipos estadosEquipos)
        {
            estadosEquipos? estadosEquipoExistente = _equiposContext.estados_equipo.Find(id);

            if (estadosEquipoExistente == null)
            {
                return NotFound();
            }

            estadosEquipoExistente.descripcion = estadosEquipos.descripcion;
            estadosEquipoExistente.estado = estadosEquipos.estado;

            _equiposContext.Entry(estadosEquipoExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadosEquipoExistente);

        }
       
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarTipoEquipo(int id)
        {
            estadosEquipos? estadoEquipoExistente = _equiposContext.estados_equipo.Find(id);

            if (estadoEquipoExistente == null) return NotFound();

            estadoEquipoExistente.estado = "C";

            _equiposContext.Entry(estadoEquipoExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadoEquipoExistente);

        }
       
    }
}
