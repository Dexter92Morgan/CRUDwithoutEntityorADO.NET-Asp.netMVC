using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDwithoutEntity.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;

namespace CRUDwithoutEntity.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        // 1. *************show the list of Patient in the Clinic ******************
       
        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString; 
        
        public ActionResult Index()
        {
            List<PatientModelVeiw> Patients = new List<PatientModelVeiw>();
            string query = "SELECT * FROM TblPatient";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Patients.Add(new PatientModelVeiw
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                PatientName = Convert.ToString(sdr["PatientName"]),
                                PatientNumber = Convert.ToString(sdr["PatientNumber"]),
                                PatientEmail = Convert.ToString(sdr["PatientEmail"]),
                                Address = Convert.ToString(sdr["Address"]),
                                BloodGroup = Convert.ToString(sdr["BloodGroup"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            if (Patients.Count == 0)
            {
                Patients.Add(new PatientModelVeiw());
            }
            return View(Patients);
        }


        // GET: Patient/PatientDetails/5
        public ActionResult PatientDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientModelVeiw Patient = new PatientModelVeiw();
            string query = "SELECT * FROM TblPatient where Id=" + id;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Patient = new PatientModelVeiw
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                PatientName = Convert.ToString(sdr["PatientName"]),
                                PatientNumber = Convert.ToString(sdr["PatientNumber"]),
                                PatientEmail = Convert.ToString(sdr["PatientEmail"]),
                                Address = Convert.ToString(sdr["Address"]),
                                BloodGroup = Convert.ToString(sdr["BloodGroup"])
                            };
                        }
                    }
                    con.Close();
                }
            }
            if (Patient == null)
            {
                return HttpNotFound();
            }
            return View(Patient);
        }

        // 2. *************ADD NEW Patient in the Clinic ******************
        // GET: Home/CreatePatient
        public ActionResult CreatePatient()
        {
            return View();
        }

        // POST: Patient/CreatePatient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatient([Bind(Include = "Id,PatientName,PatientNumber,PatientEmail,Address,BloodGroup")] PatientModelVeiw patientModelVeiw)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        //inserting Patient data into database
                        string query = "insert into TblPatient values (@PatientName, @PatientNumber, @PatientEmail,@Address,@BloodGroup)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@PatientName", patientModelVeiw.PatientName);
                            cmd.Parameters.AddWithValue("@PatientNumber", patientModelVeiw.PatientNumber);
                            cmd.Parameters.AddWithValue("@PatientEmail", patientModelVeiw.PatientEmail);
                           cmd.Parameters.AddWithValue("@Address", patientModelVeiw.Address);
                            cmd.Parameters.AddWithValue("@BloodGroup", patientModelVeiw.BloodGroup);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }
            return View(patientModelVeiw);
        }

        // 3. *************Update Patient Detail in the Clinic ******************

        // GET: Patient/UpdatePatient/5
        public ActionResult UpdatePatient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientModelVeiw Patient = new PatientModelVeiw();
            string query = "SELECT * FROM TblPatient where Id=" + id;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Patient = new PatientModelVeiw
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                PatientName = Convert.ToString(sdr["PatientName"]),
                                PatientNumber = Convert.ToString(sdr["PatientNumber"]),
                                PatientEmail = Convert.ToString(sdr["PatientEmail"]),
                                Address = Convert.ToString(sdr["Address"]),
                                BloodGroup = Convert.ToString(sdr["BloodGroup"])
                            };
                        }
                    }
                    con.Close();
                }
            }
            if (Patient == null)
            {
                return HttpNotFound();
            }
            return View(Patient);
        }

        // POST: Patient/UpdatePatient/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePatient([Bind(Include = "Id,PatientName,PatientNumber,PatientEmail,Address,BloodGroup")] PatientModelVeiw patientModelVeiw)
        {
            if (ModelState.IsValid)
            {
                string query = "UPDATE TblPatient SET PatientName = @PatientName,PatientNumber=@PatientNumber, PatientEmail = @PatientEmail,Address=@Address,BloodGroup=@BloodGroup Where Id =@Id";
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@PatientName", patientModelVeiw.PatientName);
                        cmd.Parameters.AddWithValue("@PatientNumber", patientModelVeiw.PatientNumber);
                        cmd.Parameters.AddWithValue("@PatientEmail", patientModelVeiw.PatientEmail);
                        cmd.Parameters.AddWithValue("@Address", patientModelVeiw.Address);
                        cmd.Parameters.AddWithValue("@BloodGroup", patientModelVeiw.BloodGroup);
                        cmd.Parameters.AddWithValue("@Id", patientModelVeiw.Id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(patientModelVeiw);
        }


        // 3. *************Deelete Patient in the Clinic ******************

        // GET: Patient/DeletePatient/5
        public ActionResult DeletePatient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientModelVeiw Patient = new PatientModelVeiw();
            string query = "SELECT * FROM TblPatient where Id=" + id;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Patient = new PatientModelVeiw
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                PatientName = Convert.ToString(sdr["PatientName"]),
                                PatientNumber = Convert.ToString(sdr["PatientNumber"]),
                                PatientEmail = Convert.ToString(sdr["PatientEmail"]),
                                Address = Convert.ToString(sdr["Address"]),
                                BloodGroup = Convert.ToString(sdr["BloodGroup"])
                            };
                        }
                    }
                    con.Close();
                }
            }
            return View(Patient);
        }

        // POST: Patient/DeletePatient/5
        [HttpPost, ActionName("DeletePatient")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePatient(int id)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "Delete FROM TblPatient where Id='" + id + "'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return RedirectToAction("Index");
        }

    }
}