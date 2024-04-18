using Microsoft.AspNetCore.Mvc;
using Practica2.AccesoDatos.Repositorio.IRepositorio;
using Practica2.Modelos;
using Practica2.Utilidades;

namespace Practica2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                //Crear nueva marca
                marca.Estado = true;
                return View(marca);
            }
            marca = await _unidadTrabajo.Marca.obtener(id.GetValueOrDefault()); //Nos aseguramos que la información llegue correctamente
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Evita que se pueda clonar 
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
					TempData[DS.Exitosa] = "Marca creada exitósamente";
				}
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
					TempData[DS.Exitosa] = "Marca actualizada exitósamente";
				}
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
			TempData[DS.Error] = "Error al guardar la marca";
			return View(marca);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDB = await _unidadTrabajo.Marca.obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar marca." });
            }
            _unidadTrabajo.Marca.Remover(marcaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitósamente." });
        }

        #region API 
        [HttpGet]
		public async Task<IActionResult> obtenerTodos()
		{
			var todos = await _unidadTrabajo.Marca.obtenerTodos();
			return Json(new { data = todos });  //data es el nombre que tiene que tener la tabla por defecto para crear el JSON
		}

		[ActionName("ValidarNombre")]
		public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
		{
			bool valor = false;
			var lista = await _unidadTrabajo.Marca.obtenerTodos();
			if (id == 0)
			{
				valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
			}
			else
			{
				valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
			}
			if (valor)
			{
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
		#endregion
	}
}
