using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_doctor_appointment_service.Models
{
    public class Bookappointment
    {

        public int userid { get; set; }
        public int doctorid { get; set; }

        public int timeslotid { get; set; }

        
        public DateTime? Date { get; set; }
    }
}