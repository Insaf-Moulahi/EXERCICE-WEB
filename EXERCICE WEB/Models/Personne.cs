using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EXERCICEWEB.Models
{
    public class Personne
    {
        public int ID { get; set; }       
        public string nom { get; set; }
        public string prénom { get; set; }      
        public DateTime? Date_Naissance { get; set; }
        public int Age { get; set; }
        
        public List<Personne> personnes { get; set; }
    }

}