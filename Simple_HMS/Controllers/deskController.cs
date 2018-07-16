using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple_HMS.Interface;
using System.Web.Mvc;
using Simple_HMS.Entity;

namespace Simple_HMS.Controllers
{
    public class deskController : Controller
    {
        private readonly IPatientRepository _repository;

        public deskController(IPatientRepository repository)
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
                    return View(nameof(patients), patients.FirstOrDefault());
                return View(patients);
            }
        }

        [HttpPost, ActionName("advance-search")]
        public ActionResult AdvanceSearch(string search_term)
        {
            ViewBag.search_term = search_term;
            var terms = search_term.Split(new char[] { ' ' }, 2);
            var patients = _repository.SearchPatient(terms[0], terms[1]);

            if (patients?.Count() == 0)
            {
                TempData["error"] = $"No patients with search term {search_term} was found";
                return RedirectToAction("index");
            }
            else
            {
                return View("AdvanceSearch", patients);
            }
        }

        public ActionResult patients(string regno)
        {
            var patients = _repository.GetPatient(regno).FirstOrDefault();
            if (patients == null)
                return RedirectToAction("index");
            ViewBag.records = _repository.GetMedRecordSummaryRange(regno, "", "");
            return View(patients);
        }

        public ActionResult AddPatients()
        {
            return View(new Patients());
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
                var RegNo = "SHMS" + _repository.GetPatientCount().Value.ToString();
                model.patients_regno = RegNo;
                _repository.AddPatient(model);

                TempData["success"] = $"Patients created success fully. Regno: {RegNo}";
                return RedirectToAction(nameof(patients), new { regno = RegNo });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return View("AddPatients", model);
        }

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

            return View(nameof(AddMedRecord), patients);
        }

        [ActionName("med-rec-summary")]
        public ActionResult MedRecSummary(string regno, DateTime start_date, DateTime end_date)
        {
            if (string.IsNullOrEmpty(regno))
                return RedirectToAction("index");
            var summary = _repository.GetMedRecordSummaryRange(regno, start_date.Date.ToString(), end_date.Date.ToString());
            if(summary == null)
            {
                TempData["error"] = "Summary not found";
                return RedirectToAction("index");
            }
            return View(nameof(MedRecSummary), summary);
        }
    }
}