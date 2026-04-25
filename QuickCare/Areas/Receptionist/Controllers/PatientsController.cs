using Common_QuickCare.Controllers;
using Common_QuickCare.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickCare.Areas.Receptionist.Controllers.ViewModals;
using QuickCare.Areas.Receptionist.Repositories;

namespace QuickCare.Areas.Receptionist.Controllers
{
    [Authorize]
    [Area("Receptionist")]
    public class PatientsController : BaseController
    {
        private readonly PatientRepository _repo;
        public PatientsController(CommonRepository c, PatientRepository repo) : base(c) { _repo = repo; }

        [Authorize(Roles = "Receptionist")]
        public IActionResult Index() => View(new PatientViewModal());

        [Authorize(Roles = "Receptionist")]
        public IActionResult GetPatientList(string search = "", int page = 1)
        {
            var patients = _repo.GetPatients(OrganizationId, search, page);
            return PartialView("_PartialPatientList", patients);
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<JsonResult> SavePatient(PatientVM patient)
        {
            patient.OrganizationId = OrganizationId;
            await _repo.UpsertPatient(patient, UserId);
            return Json(new { success = true });
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<JsonResult> DeletePatient(int id)
        {
            await _repo.DeletePatient(id);
            return Json(new { success = true });
        }
    }
}
