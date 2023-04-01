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
    public class UsuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public UsuariosController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoUsuarios = (from usuario in _equiposContext.usuarios
                                   join carrera in _equiposContext.carreras on usuario.carrera_id equals carrera.carrera_id
                                   select new
                                   {
                                       usuario.nombre,
                                       usuario.documento,
                                       usuario.tipo,
                                       usuario.carnet,
                                       carrera.nombre_carrera
                                   }).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }
      
        [HttpPost]
        [Route("Add")]
        public IActionResult Crear([FromBody] Usuario _usuario)
        {

            try
            {
                _equiposContext.usuarios.Add(_usuario);
                _equiposContext.SaveChanges();

                return Ok(_usuario);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
      

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] Usuario _usuario)
        {
            Usuario? usuarioExistente = _equiposContext.usuarios.Find(id);

            if (usuarioExistente == null)
            {
                return NotFound();
            }

            usuarioExistente.nombre = _usuario.nombre;
            usuarioExistente.documento = _usuario.documento;
            usuarioExistente.tipo = _usuario.tipo;
            usuarioExistente.carnet = _usuario.carnet;
            usuarioExistente.carrera_id = _usuario.carrera_id;

            _equiposContext.Entry(usuarioExistente).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(usuarioExistente);

        }

       
        [HttpDelete]
        [Route("detelet/{id}")]
        public IActionResult EliminarTipoEquipo(int id)
        {
            Usuario? usuarioExistente = _equiposContext.usuarios.Find(id);

            if (usuarioExistente == null) return NotFound();

            _equiposContext.Entry(usuarioExistente).State = EntityState.Deleted;
            _equiposContext.SaveChanges();

            return Ok(usuarioExistente);

        }
        

    }
}
