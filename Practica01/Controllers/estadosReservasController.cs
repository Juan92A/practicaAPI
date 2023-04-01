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
    public class estadosReservasController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public estadosReservasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

       
        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<estadosReserva> listadoEstadoReserva = _equiposContext.estados_reserva.ToList();

            if (listadoEstadoReserva.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEstadoReserva);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] estadosReserva estadosReserva)
        {

            try
            {
                _equiposContext.estados_reserva.Add(estadosReserva);
                _equiposContext.SaveChanges();

                return Ok(estadosReserva);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
 

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] estadosReserva estadosReserva)
        {
            estadosReserva? estadoReservaExistente = _equiposContext.estados_reserva.Find(id);

            if (estadoReservaExistente == null)
            {
                return NotFound();
            }

            estadoReservaExistente.estado = estadosReserva.estado;

            _equiposContext.Entry(estadoReservaExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadoReservaExistente);

        }

        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarMarca(int id)
        {
            estadosReserva? estadoReservaExistente = _equiposContext.estados_reserva.Find(id);

            if (estadoReservaExistente == null) return NotFound();


            _equiposContext.Entry(estadoReservaExistente).State = EntityState.Deleted;
            _equiposContext.SaveChanges();

            return Ok(estadoReservaExistente);

        }
     
    }
}
