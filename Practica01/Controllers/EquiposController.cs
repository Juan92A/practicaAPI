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
        [Route("getAll")]
        public IActionResult ObtenerEquipos()
        {
            List<equipos> listadoEquipo = (from db in _equiposContext.equipos
                                           where db.estado == "A"
                                           select db).ToList();

            if (listadoEquipo.Count == 0) { return NotFound(); }

            return Ok(listadoEquipo);
        }

        ///localhost:puerto/api/equipos/getbyID/21/
        /// <summary>
        /// Obtiene un registro correspondiente al id ingresado
        /// </summary>
        /// <param name='id'></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbyID/(id)")]
        public IActionResult get(int id)
        {
             equipos? unEquipo = (from e in _equiposContext.equipos
                          where e.id_equipos == id && e.estado == "A"
                                  select e).FirstOrDefault();
            if (unEquipo==null)
            {
                return NotFound();

            }
            return Ok(unEquipo);

        }

        /// <summary>
        /// Obtiene un registro correspondiente al id ingresado
        /// </summary>
        /// <param name='id'></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find/(filtro)")]
        public IActionResult buscar(string filtro)
        {
            List<equipos> listadoEquipo = (from e in _equiposContext.equipos
                                           where e.descripcion.Contains(filtro) && e.estado == "A"
                                          //  || 
                                           select e).ToList();
            //if (listadoEquipo.Count() == 0)
            //{
            //    return NotFound();

            //}
            if (listadoEquipo.Any())
            {
             return Ok(listadoEquipo);

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
        public IActionResult Crear([FromBody] equipos equipoNuevo)
        {

            try
            {
                equipoNuevo.estado = "A";
                _equiposContext.equipos.Add(equipoNuevo);
                _equiposContext.SaveChanges();
                return Ok(equipoNuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// EndPoint encargado de actualizar un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("actualizar(id)")]
        public IActionResult update(int id,[FromBody] equipos equipoNuevo)
        {
            equipos? equipoExiste = (from e in _equiposContext.equipos
                                    where e.id_equipos ==id && e.estado=="A"
                                    select e).FirstOrDefault();

            if (equipoExiste == null)
            {
                return NotFound();
            }
            equipoExiste.nombre = equipoNuevo.nombre;
            equipoExiste.descripcion = equipoNuevo.descripcion;
            _equiposContext.Entry(equipoExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipoExiste);
        }

        /// <summary>
        /// EndPoint encargado de "eliminar" un registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete(id)")]
        public IActionResult eliminar (int id)
        {
            equipos? equipoExiste = (from e in _equiposContext.equipos
                                     where e.id_equipos == id && e.estado == "A"
                                     select e).FirstOrDefault();
            if (equipoExiste == null)
            {
                return NotFound();
            }
            equipoExiste.estado = "I";
            //actualizar el estado del registro por integridad de datos
            _equiposContext.Entry(equipoExiste).State = EntityState.Modified;
            //_equiposContext.equipos.Attach(equipoExiste);
            //_equiposContext.equipos.Remove(equipoExiste);
            _equiposContext.SaveChanges();
            return Ok();
        }
    }
}
