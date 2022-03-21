using System.ComponentModel;

namespace Book_doctor_appointment_service.Models
{
    public class Alldoctor
    {

        public int Id { get; set; }

        [DisplayName("Name")]
        public string Doctorname { get; set; }
        public string Speciality { get; set; }
        public string Location { get; set; }
    }
}