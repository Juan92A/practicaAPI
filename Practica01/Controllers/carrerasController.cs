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
    public class carrerasController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public carrerasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoCarreras = (from carreara in _equiposContext.carreras
                                   join facultad in _equiposContext.facultades on carreara.facultad_id equals facultad.facultad_id
                                   select new
                                   {
                                       carreara.nombre_carrera,
                                       facultad.nombre_facultad
                                   }).ToList();

            if (listadoCarreras.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoCarreras);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] carreras carreras)
        {

            try
            {
                _equiposContext.carreras.Add(carreras);
                _equiposContext.SaveChanges();

                return Ok(carreras);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
  
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] carreras carreras)
        {
            carreras? carreraExistente = _equiposContext.carreras.Find(id);

            if (carreraExistente == null)
            {
                return NotFound();
            }

            carreraExistente.nombre_carrera = carreras.nombre_carrera;
            carreraExistente.facultad_id = carreras.facultad_id;

            _equiposContext.Entry(carreraExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(carreraExistente);

        }

       
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarCarrera(int id)
        {
            carreras? carreraExistente = _equiposContext.carreras.Find(id);

            if (carreraExistente == null) return NotFound();

            _equiposContext.Entry(carreraExistente).State = EntityState.Deleted;
            _equiposContext.SaveChanges();

            return Ok(carreraExistente);

        }

    }
}
