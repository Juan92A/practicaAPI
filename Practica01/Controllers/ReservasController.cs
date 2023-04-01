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
    public class ReservasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public ReservasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoReserva = (from reserva in _equiposContext.reservas
                                  join equipo in _equiposContext.equipos on reserva.equipo_id equals equipo.id_equipos
                                  join estado in _equiposContext.estados_reserva on reserva.reserva_id equals estado.estado_res_id
                                  join usuario in _equiposContext.usuarios on reserva.usuario_id equals usuario.usuario_id
                                  select new
                                  {
                                      nombreEquipo = equipo.nombre,
                                      nombreUsuario = usuario.nombre,
                                      reserva.fecha_salida,
                                      reserva.hora_salida,
                                      reserva.tiempo_reserva,
                                      estado.estado,
                                      reserva.fecha_retorno,
                                      reserva.hora_retorno
                                  }).ToList();

            if (listadoReserva.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoReserva);
        }

        
        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] Reserva _reserva)
        {

            try
            {
                _equiposContext.reservas.Add(_reserva);
                _equiposContext.SaveChanges();

                return Ok(_reserva);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] Reserva reserva)
        {
            Reserva? reservaExistente = _equiposContext.reservas.Find(id);

            if (reservaExistente == null)
            {
                return NotFound();
            }

            reservaExistente.equipo_id = reserva.equipo_id;
            reservaExistente.usuario_id = reserva.usuario_id;
            reservaExistente.fecha_salida = reserva.fecha_salida;
            reservaExistente.hora_salida = reserva.hora_salida;
            reservaExistente.tiempo_reserva = reserva.tiempo_reserva;
            reservaExistente.estado_reserva_id = reserva.estado_reserva_id;
            reservaExistente.fecha_retorno = reserva.fecha_retorno;
            reservaExistente.hora_retorno = reserva.hora_retorno;


            _equiposContext.Entry(reservaExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reservaExistente);

        }

      
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarMarca(int id)
        {
            Reserva? reservaExistente = _equiposContext.reservas.Find(id);

            if (reservaExistente == null) return NotFound();

            _equiposContext.Entry(reservaExistente).State = EntityState.Deleted;
            _equiposContext.SaveChanges();

            return Ok(reservaExistente);

        }
      
    }
}
