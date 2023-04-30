using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebSisMed.Models.Contexts;
using WebSisMed.Models.Entities;
using WebSisMed.ViewModels.Pacientes;

namespace WebSisMed.Controllers
{
    public class PacientesController : Controller
    {
        private readonly SisMedContext _context;
        private readonly IValidator<AdicionarPacienteViewModel> _adicionarPacienteValidator;
        private readonly IValidator<EditarPacienteViewModel> _editarPacienteValidator;
        private const int TAMANHO_PAGINA = 10;

        public PacientesController(
            SisMedContext context,
            IValidator<AdicionarPacienteViewModel> adicionarPacienteValidator,
            IValidator<EditarPacienteViewModel> editarPacienteViewModel)
        {
            _context = context;
            _adicionarPacienteValidator = adicionarPacienteValidator;
            _editarPacienteValidator = editarPacienteViewModel;
        }

        public IActionResult Index(string filtro, int pagina = 1)
        {
            var pacientes = _context.Pacientes.Where(x => x.Nome.Contains(filtro) || x.CPF.Contains(filtro))
                                          .Select(x => new ListarPacienteViewModel
                                          {
                                              Id = x.Id,
                                              CPF = x.CPF,
                                              Nome = x.Nome
                                          });
            ViewBag.Filtro = filtro;
            ViewBag.NumeroPagina = pagina;
            ViewBag.TotalPaginas = Math.Ceiling((decimal)pacientes.Count() / TAMANHO_PAGINA);
            return View(pacientes.Skip((pagina - 1) * TAMANHO_PAGINA).Take(TAMANHO_PAGINA));
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(AdicionarPacienteViewModel dados)
        {
            var validacao = _adicionarPacienteValidator.Validate(dados);

            if (!validacao.IsValid)
            {
                validacao.AddToModelState(ModelState, string.Empty);
                return View(validacao);
            }

            var pacientes = new Paciente
            {
                Nome = dados.Nome,
                CPF = Regex.Replace(dados.CPF, "[^0-9]", ""),
                DataNascimento = dados.DataNascimento
            };

            _context.Pacientes.Add(pacientes);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var paciente = _context.Pacientes.Find(id);

            if (paciente != null)
            {
                var informacoesComplementares = _context.InformacoesComplementares.FirstOrDefault(x => x.IdPaciente == id);

                return View(new EditarPacienteViewModel
                {
                    Id = paciente.Id,
                    CPF = paciente.CPF,
                    Nome = paciente.Nome,
                    DataNascimento = paciente.DataNascimento,
                    Alergias = informacoesComplementares?.Alergias,
                    MedicamentoEmUso = informacoesComplementares?.MedicamentoEmUso,
                    CirurgiasRealizadas = informacoesComplementares?.CirurgiasRealizadas
                });
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, EditarPacienteViewModel dados)
        {
            var validacao = _editarPacienteValidator.Validate(dados);

            if (!validacao.IsValid)
            {
                validacao.AddToModelState(ModelState, string.Empty);
                return View(dados);
            }

            var paciente = _context.Pacientes.Find(id);

            if (paciente != null) 
            {
                paciente.Nome = dados.Nome;
                paciente.CPF = Regex.Replace(dados.CPF, "[^0-9]", "");
                paciente.DataNascimento = dados.DataNascimento;

                var informacoesComplementares = _context.InformacoesComplementares.FirstOrDefault(x => x.IdPaciente == id);

                if (informacoesComplementares == null)
                        informacoesComplementares = new InformacoesComplementaresPaciente();

                    informacoesComplementares.Alergias = dados.Alergias;
                    informacoesComplementares.MedicamentoEmUso = dados.MedicamentoEmUso;
                    informacoesComplementares.CirurgiasRealizadas = dados.CirurgiasRealizadas;
                    informacoesComplementares.IdPaciente = id;

                if (informacoesComplementares.Id > 0)
                    _context.InformacoesComplementares.Update(informacoesComplementares);
                else
                    _context.InformacoesComplementares.Add(informacoesComplementares);

                _context.Update(paciente);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public IActionResult Excluir(int id)
        {
            var paciente = _context.Pacientes.Find(id);

            if (paciente != null) 
            {
                return View(new EditarPacienteViewModel
                {
                    Id = paciente.Id,
                    CPF = paciente.CPF,
                    Nome = paciente.Nome,
                    DataNascimento = paciente.DataNascimento
                });
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id, EditarPacienteViewModel dados)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente != null)
            {
                _context.Remove(paciente);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return NoContent();
        }
    }
}
