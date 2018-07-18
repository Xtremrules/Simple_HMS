using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple_HMS.Interface;
using System.Web.Mvc;
using Simple_HMS.Entity;
using System.Web.Routing;

namespace Simple_HMS.Controllers
{
    [Authorize]
    public class DeskController : Controller
    {
        private readonly IPatientRepository _repository;

        public DeskController(IPatientRepository repository)
        {
            _repository = repository;
        }

        // GET: desk
        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult search(string regno)
        {
            ViewBag.regno = regno;
            var patients = _repository.GetPatient(regno);

            if (patients?.Count() == 0)
            {
                TempData["error"] = $"No patients with {regno} was found";
                return RedirectToAction("index");
            }
            else
            {
                if (patients.Count() == 1)
                {
                    ViewBag.records = _repository.GetMedRecordSummaryRange(regno, null, null);
                    return View(nameof(patients), patients.FirstOrDefault());
                }
                return View(patients);
            }
        }

        [HttpPost, ActionName("advance-search")]
        public ActionResult AdvanceSearch(string search_term)
        {
            IEnumerable<IPatient> patients;

            ViewBag.regno = search_term;
            var terms = search_term.Trim().Split(new char[] { ' ' }, 2);
            if (terms.Length == 2)
                patients = _repository.SearchPatient(terms[0], terms[1]);
            else
                patients = _repository.SearchPatient(search_term, null);

            if (patients?.Count() == 0)
            {
                TempData["error"] = $"No patients with search term {search_term} was found";
                return RedirectToAction("index");
            }
            else
            {
                return View("search", patients);
            }
        }

        public ActionResult patients(string regno)
        {
            var patients = _repository.GetPatient(regno).FirstOrDefault();
            if (patients == null)
                return RedirectToAction("index");
            ViewBag.records = _repository.GetMedRecordSummaryRange(regno, null, null);
            return View(patients);
        }

        [ActionName("add-patients")]
        public ActionResult AddPatients()
        {
            var RegNo = "NHMS" + _repository.GetPatientCount().Value.ToString();
            return View("AddPatients", new Patients() { patients_regno = RegNo });
        }

        [HttpPost, ActionName("add-patients")]
        public ActionResult AddPatients(Patients model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Check your values";
                return View(nameof(AddPatients), model);
            }

            try
            {
                _repository.AddPatient(model);
                TempData["success"] = $"Patients created success fully. Regno: {model.patients_regno}";
                return RedirectToAction(nameof(patients), new { regno = model.patients_regno });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return View("AddPatients", model);
        }

        [ActionName("add-med-record")]
        public ActionResult AddMedRecord(string regno)
        {
            if(string.IsNullOrWhiteSpace(regno))
            {
                TempData["error"] = "Enter patients regno";
                return RedirectToAction("index");
            }

            var patients = _repository.GetPatient(regno).FirstOrDefault();
            if(patients == null)
            {
                TempData["error"] = "Patient not found";
                return RedirectToAction("index");
            }
            ViewBag.patients = patients;
            return View(nameof(AddMedRecord), new MedRecords { patients_regno = patients.patients_regno});
        }

        [ActionName("add-med-record"), HttpPost]
        public ActionResult AddMedRecord(MedRecords model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Data not valid");
                return View(nameof(AddMedRecord), model);
            }

            try
            {
                _repository.AddMedRecord(model);
                TempData["success"] = "Record added successfully";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(nameof(AddMedRecord), model);
            }
            return RedirectToAction("patients", new { regno = model.patients_regno });
        }

        [ActionName("med-rec-summary"), HttpPost]
        public ActionResult MedRecSummary(string regno, DateTime start_date, DateTime end_date)
        {
            if (string.IsNullOrEmpty(regno))
                return RedirectToAction("index");
            var summary = _repository.GetMedRecordSummaryRange(regno, start_date.ToString("yyyy-MM-dd"), end_date.ToString("yyyy-MM-dd"));
            if (summary == null)
            {
                TempData["error"] = "Summary not found";
                return RedirectToAction("index");
            }
            return View(nameof(MedRecSummary), summary);
        }
    }
}