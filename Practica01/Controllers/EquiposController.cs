using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica01.Models;
using Microsoft.EntityFrameworkCore;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public EquiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        /// <summary>
        /// EndPoint encargado de obtener y retornar todos los registros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoEquipos = (from equipo in _equiposContext.equipos
                                  join estado in _equiposContext.estados_equipo on equipo.estado_equipo_id equals estado.id_estados_equipo
                                  join tipoEquipo in _equiposContext.tipo_equipo on equipo.tipo_equipo_id equals tipoEquipo.id_tipo_equipo
                                  join marca in _equiposContext.Marcas on equipo.marca_id equals marca.id_marcas
                                  select new
                                  {
                                      equipo.nombre,
                                      equipo.descripcion,
                                      descripcionTipo = tipoEquipo.descripcion,
                                      marca.nombre_marca,
                                      equipo.modelo,
                                      equipo.anio_compra,
                                      equipo.costo,
                                      equipo.vida_util,
                                      estadoEquipo = estado.descripcion
                                  }).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);
        }
        ///localhost:puerto/api/equipos/getbyID/21/
        /// <summary>
        /// Obtiene un registro correspondiente al id ingresado
        /// </summary>
        /// <param name='id'></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public ActionResult GetById(int id)
        {
            equipos? equipo = _equiposContext.equipos.Find(id);

            if (equipo == null) return NotFound();

            return Ok(equipo);
        }

        /// <summary>
        /// Obtiene un registro correspondiente al id ingresado
        /// </summary>
        /// <param name='id'></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Find")]
        public ActionResult Find(string filtro)
        {
            List<equipos>? equiposList = _equiposContext.equipos
                                       .Where((x => (x.nombre.Contains(filtro) || x.descripcion.Contains(filtro)) && x.estado == "A"))
                                       .ToList();

            if (equiposList.Any())
            {
                return Ok(equiposList);

            }

            return NotFound();
        }
        /// <summary>
        /// EndPoint encargado de Almacenar un nuevo registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IActionResult crear([FromBody] equipos equipo)
        {

            try
            {
                _equiposContext.equipos.Add(equipo);
                _equiposContext.SaveChanges();

                return Ok(equipo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// EndPoint encargado de actualizar un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("actualizar(id)")]
        public IActionResult actualizar(int id, [FromBody] equipos equipo)
        {
            equipos? equipoExistente = _equiposContext.equipos.Find(id);

            if (equipoExistente == null)
            {
                return NotFound();
            }

            equipoExistente.nombre = equipo.nombre;
            equipoExistente.descripcion = equipo.descripcion;

            _equiposContext.Entry(equipoExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(equipoExistente);

        }

        /// <summary>
        /// EndPoint encargado de "eliminar" un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete(id)")]
        public IActionResult eliminarEquipo(int id)
        {
            equipos? equipoExiste = _equiposContext.equipos.Find(id);

            if (equipoExiste == null) return NotFound();

            equipoExiste.estado = "C";

            _equiposContext.Entry(equipoExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(equipoExiste);

        }
    }
}
