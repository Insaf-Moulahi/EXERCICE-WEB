using EXERCICEWEB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXERCICEWEB.Controllers
{
    public class PersonneController : Controller
    {
        PersonneDAL cmBusiness = new PersonneDAL();
        // GET: Home
        public ActionResult Index()
        {
            ModelState.Clear();
            Personne model = new Personne();
            model.personnes = cmBusiness.GetListPersonne();
            return View(model);
        }
        [HttpGet]
        public ActionResult Index(string search)
        {
            Personne model = new Personne();
            model.personnes = cmBusiness.GetListPersonne();
            if (search != "" && search != null)
            {
                model.personnes = cmBusiness.GetListPersonne2(search);
                return View(model);
            }
            else { return View(model); }
        }
        [HttpPost]
        public ActionResult InsertPersonne(Personne objModel)
        {
            bool IsProductNameExist = cmBusiness.GetListPersonne().Any
        (x => x.nom == objModel.nom );
            if (IsProductNameExist == true)
            {
                ViewBag.Message = String.Format("Personne existe déja");
                ModelState.AddModelError("nom", "Personne existe déja");
            }
            
                try
                {
                    int result = cmBusiness.InsertPersonne(objModel);
                    if (result == 1)
                    {
                        ViewBag.Message = "Personne ajouté avec succés";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "Echec";
                        ModelState.Clear();
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    throw;
                }
           
        }
        public JsonResult IsPersonneExists(string nom)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(cmBusiness.GetListPersonne().Any(x => x.nom == nom), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditPersonne(int? id)
        {
            var PERSONNE = cmBusiness.GetListPersonne().Find(x => x.ID.Equals(id));
            return Json(PERSONNE, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePersonne(Personne objModel)
        {
            try
            {
                int result = cmBusiness.UpdatePersonne(objModel);
                if (result == 1)
                {
                    ViewBag.Message = "Modification effectuée avec succés";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Message = "Echec";
                    ModelState.Clear();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult DeletePersonne(int id)
        {
            try
            {
                int result = cmBusiness.DeletePersonne(id);
                if (result == 1)
                {
                    ViewBag.Message = "Suppression effectuée avec succés";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Message = "Echec";
                    ModelState.Clear();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

    }
}
