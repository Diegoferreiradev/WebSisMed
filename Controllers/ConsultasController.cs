using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSisMed.Models.Contexts;
using WebSisMed.Models.Entities;
using WebSisMed.Models.Enums;
using WebSisMed.ViewModels.Consultas;
using WebSisMed.ViewModels.Pacientes;

namespace WebSisMed.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly SisMedContext _context;
        private readonly IValidator<AdicionarConsultaViewModel> _adicionarConsultarValidator;
        private readonly IValidator<EditarConsultaViewModel> _editarConsultarValidator;
        private const int TAMANHO_PAGINA = 10;

        public ConsultasController(
            SisMedContext context, 
            IValidator<AdicionarConsultaViewModel> adicionarConsultarValidator, 
            IValidator<EditarConsultaViewModel> editarConsultarValidator)
        {
            _context = context;
            _adicionarConsultarValidator = adicionarConsultarValidator;
            _editarConsultarValidator = editarConsultarValidator;
        }

        public IActionResult Index(string filtro, int pagina = 1)
        {
            var consultas = _context.Consultas.Include(c => c.Paciente)
                                              .Include(c => c.Medico)
                                              .Where(c => c.Paciente.Nome.Contains(filtro) || c.Medico.Nome.Contains(filtro))
                                              .Select(c => new ListarConsultaViewModel
                                              {
                                                  Id = c.Id,
                                                  Paciente = c.Paciente.Nome,
                                                  Medico = c.Medico.Nome,
                                                  Data = c.Data,
                                                  Tipo = c.Tipo == TipoConsulta.Eletiva ? "Eletiva" : "Urgência"
                                              });

            ViewBag.NumeroPagina = pagina;
            ViewBag.TotalPaginas = Math.Ceiling((decimal)consultas.Count() / TAMANHO_PAGINA);


            return View(consultas.Skip((pagina - 1) * TAMANHO_PAGINA));
        }

        public IActionResult Adicionar()
        {
            ViewBag.TiposConsulta = new[] {
                new SelectListItem { Text = "Eletiva", Value = TipoConsulta.Eletiva.ToString() },
                new SelectListItem { Text = "Urgência", Value = TipoConsulta.Urgencia.ToString() }
            };

            ViewBag.Medicos = _context.Medicos.OrderBy(c => c.Nome)
                                              .Select(c => new SelectListItem { Text = c.Nome, Value = c.Id.ToString() });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(AdicionarConsultaViewModel dados)
        {
            var validacao = _adicionarConsultarValidator.Validate(dados);

            if (!validacao.IsValid)
            {
                ViewBag.TiposConsulta = new[] {
                new SelectListItem { Text = "Eletiva", Value = TipoConsulta.Eletiva.ToString() },
                new SelectListItem { Text = "Urgência", Value = TipoConsulta.Urgencia.ToString() }
                };

                ViewBag.Medicos = _context.Medicos.OrderBy(c => c.Nome).Select(c => new SelectListItem { Text = c.Nome, Value = c.Id.ToString() });
                validacao.AddToModelState(ModelState, string.Empty);
                return View(dados);
            }

            var consulta = new Consulta
            {
                Data = dados.Data,
                Tipo = dados.Tipo,
                IdPaciente = dados.IdPaciente,
                IdMedico = dados.IdMedico
            };

            _context.Consultas.Add(consulta);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var consulta = _context.Consultas.Include(x => x.Paciente).FirstOrDefault(x => x.Id == id);

            if (consulta != null)
            {
                ViewBag.TiposConsulta = new[] {
                new SelectListItem { Text = "Eletiva", Value = TipoConsulta.Eletiva.ToString() },
                new SelectListItem { Text = "Urgência", Value = TipoConsulta.Urgencia.ToString() }
                };

                ViewBag.Medicos = _context.Medicos.OrderBy(c => c.Nome).Select(c => new SelectListItem { Text = c.Nome, Value = c.Id.ToString() });

                return View(new EditarConsultaViewModel 
                { 
                    IdMedico = consulta.IdMedico,
                    IdPaciente = consulta.IdPaciente,
                    NomePaciente = consulta.Paciente.Nome,
                    Data = consulta.Data,
                    Tipo = consulta.Tipo
                });
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, EditarConsultaViewModel dados)
        {
            var validacao = _editarConsultarValidator.Validate(dados);

            if (!validacao.IsValid)
            {
                validacao.AddToModelState(ModelState, string.Empty);
                return View(dados);
            }

            var consulta = _context.Consultas.Find(id);

            if (consulta != null)
            {
                ViewBag.TiposConsulta = new[] {
                new SelectListItem { Text = "Eletiva", Value = TipoConsulta.Eletiva.ToString() },
                new SelectListItem { Text = "Urgência", Value = TipoConsulta.Urgencia.ToString() }
                };

                ViewBag.Medicos = _context.Medicos.OrderBy(c => c.Nome).Select(c => new SelectListItem { Text = c.Nome, Value = c.Id.ToString() });

                consulta.Data = consulta.Data;
                consulta.Tipo = consulta.Tipo;
                consulta.IdMedico = dados.IdMedico;
                consulta.IdPaciente = dados.IdPaciente;

                _context.Update(consulta);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }


        public IActionResult Buscar(string filtro)
        {
            var pacientes = _context.Pacientes.Where(x => x.Nome.Contains(filtro) || x.CPF.Contains(filtro))
                                              .Take(10)
                                              .Select(x => new ListarPacienteViewModel
                                              {
                                                  Id = x.Id,
                                                  Nome = x.Nome,
                                                  CPF = x.CPF
                                              });
            return Json(pacientes);
        }
    }
}
