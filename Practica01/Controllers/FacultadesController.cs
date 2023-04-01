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
    public class FacultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public FacultadesController(equiposContext equipoContext)
        {
            _equiposContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<Facultades> listadoFacultades = _equiposContext.facultades.ToList();

            if (listadoFacultades.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoFacultades);
        }

      
        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] Facultades facultad)
        {

            try
            {
                _equiposContext.facultades.Add(facultad);
                _equiposContext.SaveChanges();

                return Ok(facultad);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] Facultades facultad)
        {
            Facultades? facultadExistente = _equiposContext.facultades.Find(id);

            if (facultadExistente == null)
            {
                return NotFound();
            }

            facultadExistente.nombre_facultad = facultad.nombre_facultad;

            _equiposContext.Entry(facultadExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(facultadExistente);

        }

    
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarMarca(int id)
        {
            Facultades? facultadExistente = _equiposContext.facultades.Find(id);

            if (facultadExistente == null) return NotFound();

            _equiposContext.Entry(facultadExistente).State = EntityState.Deleted;
            _equiposContext.SaveChanges();

            return Ok(facultadExistente);

        }
       
    }
}
