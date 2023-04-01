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
    public class MarcasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public MarcasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


     
        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<Marca> listadoMarcas = _equiposContext.Marcas.Where(x => x.estados == "A").ToList();

            if (listadoMarcas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMarcas);
        }

       
        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] Marca marca)
        {

            try
            {
                _equiposContext.Marcas.Add(marca);
                _equiposContext.SaveChanges();

                return Ok(marca);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] Marca marca)
        {
            Marca? marcaExistente = _equiposContext.Marcas.Find(id);

            if (marcaExistente == null)
            {
                return NotFound();
            }

            marcaExistente.nombre_marca = marca.nombre_marca;
            marcaExistente.estados = marca.estados;

            _equiposContext.Entry(marcaExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(marcaExistente);

        }

  
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarMarca(int id)
        {
            Marca? marcaExistente = _equiposContext.Marcas.Find(id);

            if (marcaExistente == null) return NotFound();

            marcaExistente.estados = "C";

            _equiposContext.Entry(marcaExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(marcaExistente);

        }
    
    }
}
