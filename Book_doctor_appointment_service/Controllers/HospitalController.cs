using Book_Doctor_appointment.Models;
using Book_doctor_appointment_service.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Book_doctor_appointment_service.Controllers
{
    public class HospitalController : ApiController
    {
        public IHttpActionResult Login([FromBody] loginuser li)
        {
            Searchuser ss = new Book_Doctor_appointment.Models.Searchuser();
            string pass = ss.searchk(li);
            if (pass == li.Userpassword)
            {
                GetUserid s = new Book_Doctor_appointment.Models.GetUserid();
                int id = s.Searchbyusername(li.Emailid);
                return Ok(id);
            }
            else
            {
                return NotFound();
            }
        }
        public IHttpActionResult LoginDoctor([FromBody] Logindoctor li)
        {
            Searchuser ss = new Book_Doctor_appointment.Models.Searchuser();
            Searchdoctor d = new Searchdoctor();
            string pass = d.searchk(li);
            if (pass == li.Userpassword)
            {
                GetDoctorid s = new Models.GetDoctorid();
                int id = s.Searchbyusername(li.Emailid);
                return Ok(id);
            }
            else
            {
                return NotFound();
            }
        }
        public IHttpActionResult Signup([FromBody] loginuser li)
        {
            Enterintotable en = new Book_Doctor_appointment.Models.Enterintotable();
            en.InsertUser(li);
            return Ok();

        }

        public IHttpActionResult GetAllDctor()
        {
            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<Alldoctor> listuser = new List<Alldoctor>();
            var listofdoctor = (from p in entities.ListofDoctors
                                join q in entities.Specialties on p.Specialityid equals q.id
                                join r in entities.Hospital_location on p.locid equals r.id
                                select new Alldoctor
                                {
                                    Id = p.id,
                                    Doctorname = p.Doctorname,
                                    Speciality = q.Specialty1,
                                    Location = r.Hospital_Location1

                                }).ToList();
            listuser = listofdoctor.ToList();
            return Ok(listuser);

        }
        public IHttpActionResult GetLocations()
        {

            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<System.Web.Mvc.SelectListItem> locationlist = new List<System.Web.Mvc.SelectListItem>();
            locationlist = (from p in entities.Hospital_location.AsEnumerable()
                            select new System.Web.Mvc.SelectListItem
                            {
                                Text = p.Hospital_Location1,
                                Value = p.id.ToString()
                            }).ToList();
            locationlist.Insert(0, new System.Web.Mvc.SelectListItem { Text = "--Select Hospital--", Value = "" });

            return Ok(locationlist);
        }

        public IHttpActionResult GetSpecialties()
        {

            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<System.Web.Mvc.SelectListItem> Specialtylist = (from p in entities.Specialties.AsEnumerable()
                                                                 select new System.Web.Mvc.SelectListItem
                                                                 {
                                                                     Text = p.Specialty1,
                                                                     Value = p.id.ToString()
                                                                 }).ToList();
            Specialtylist.Insert(0, new System.Web.Mvc.SelectListItem { Text = "--Select Specialty--", Value = "" });
            return Ok(Specialtylist);

        }


        public IHttpActionResult Gettimeslotbyavailability(int doctorid, DateTime Date)
        {
            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            var data1 = (from p in entities.Timeslots
                         join q in entities.Appointments on p.id equals q.Timeslot
                         where q.Date_of_Appointment == Date && q.Doctorid == doctorid
                         select new
                         {
                             p.id,
                             p.timeslot1
                         });
            var data2 = (from p in entities.Timeslots
                         join q in entities.ListofDoctors on p.Doctorid equals q.id
                         where p.Doctorid == doctorid
                         select new
                         {
                             p.id,
                             p.timeslot1
                         });

            var data3 = (from p in data2 select p).Except(data1);
            List<System.Web.Mvc.SelectListItem> availabletimeslots = (from p in data3.AsEnumerable()

                                                                      select new System.Web.Mvc.SelectListItem
                                                                      {
                                                                          Text = p.timeslot1,
                                                                          Value = p.id.ToString()
                                                                      }).ToList();


            return Ok(availabletimeslots);

        }


        public IHttpActionResult GetListofDoctorforselected(int locid, int spid, int? userid)
        {
            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<Alldoctor> listuser = new List<Alldoctor>();
            var listofdoctor = (from p in entities.ListofDoctors
                                join q in entities.Specialties on p.Specialityid equals q.id
                                join r in entities.Hospital_location on p.locid equals r.id
                                where p.Specialityid == spid && r.id == locid
                                select new Alldoctor
                                {
                                    Id = p.id,
                                    Doctorname = p.Doctorname,
                                    Speciality = q.Specialty1,
                                    Location = r.Hospital_Location1

                                }).ToList();
            listuser = listofdoctor.ToList();
            return Ok(listuser);

        }

        public IHttpActionResult GetBookappointment(int id)
        {
            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<Appointments> listofdoctor = new List<Appointments>();
            listofdoctor = (from p in entities.ListofDoctors
                            join q in entities.Specialties on p.Specialityid equals q.id
                            join r in entities.Hospital_location on p.locid equals r.id
                            where p.id == id
                            select new Appointments
                            {
                                DoctorName = p.Doctorname,
                                Speciality = q.Specialty1,
                                Location = r.Hospital_Location1

                            }).ToList();
            return Ok(listofdoctor);

        }

        public IHttpActionResult GetMyappointments(int id)
        {

            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<Myappointments> my_pp = new List<Myappointments>();
            my_pp = (from a in entities.Appointments
                     join b in entities.registers on a.Userid equals b.id
                     join c in entities.ListofDoctors on a.Doctorid equals c.id
                     join d in entities.Hospital_location on c.locid equals d.id
                     join e in entities.Specialties on c.Specialityid equals e.id
                     join f in entities.Timeslots on a.Timeslot equals f.id
                     where b.id == id
                     select new Myappointments
                     {
                         Id = a.id,
                         DoctorName = c.Doctorname,
                         Speciality = e.Specialty1,
                         Location = d.Hospital_Location1,
                         Dateofappointment = a.Date_of_Appointment,
                         Timeslot = f.timeslot1,
                         Booked_Date = a.Booked_appointment_date
                     }).ToList();
            return Ok(my_pp);
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult BookappointmentSucess(ArrayList b)
        {

            HospitalApplicationEntities db = new HospitalApplicationEntities();
            if (b.Count > 0)
            {
                Bookappointment bs = JsonConvert.DeserializeObject<Bookappointment>(b[0].ToString());
                Appointment rs = new Appointment();

                rs.Date_of_Appointment = bs.Date;
                rs.Userid = bs.userid;
                rs.Doctorid = bs.doctorid;
                rs.Booked_appointment_date = DateTime.Now;
                rs.Timeslot = bs.timeslotid;
                db.Appointments.Add(rs);
                db.SaveChanges();
                return Ok();
            }
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult CancelAppointment(int id)
        {

            HospitalApplicationEntities db = new HospitalApplicationEntities();
            var x = (from y in db.Appointments
                     where y.id == id
                     select y).FirstOrDefault();
            db.Appointments.Remove(x);
            db.SaveChanges();
            return Ok();
        }
        public IHttpActionResult GetPatientappointments(int id)
        {

            HospitalApplicationEntities entities = new HospitalApplicationEntities();
            List<Myappointments> my_pp = new List<Myappointments>();
            my_pp = (from a in entities.Appointments
                     join b in entities.registers on a.Userid equals b.id
                     join c in entities.ListofDoctors on a.Doctorid equals c.id
                     join d in entities.Hospital_location on c.locid equals d.id
                     join e in entities.Specialties on c.Specialityid equals e.id
                     join f in entities.Timeslots on a.Timeslot equals f.id
                     where c.id == id
                     select new Myappointments
                     {
                         Id = a.id,
                         Name = b.name,
                         Emailid = b.emailid,
                         Location = d.Hospital_Location1,
                         Dateofappointment = a.Date_of_Appointment,
                         Timeslot = f.timeslot1,
                         Booked_Date = a.Booked_appointment_date
                     }).ToList();
            return Ok(my_pp);
        }

    }
}