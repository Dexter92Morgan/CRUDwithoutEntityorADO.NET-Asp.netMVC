using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using CRUDwithoutEntity.Models;

namespace CRUDwithoutEntity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            List<CustomerModel> customers = new List<CustomerModel>();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            string query = "SELECT * FROM Customers";
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
                            customers.Add(new CustomerModel
                            {
                                CustomerId = Convert.ToInt32(sdr["CustomerId"]),
                                Name = Convert.ToString(sdr["Name"]),
                                Country = Convert.ToString(sdr["Country"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            if (customers.Count == 0)
            {
                customers.Add(new CustomerModel());
            }
            return View(customers);
        }

        [HttpPost]
        public JsonResult InsertCustomer(CustomerModel customer)
        {
            string query = "INSERT INTO Customers VALUES(@Name, @Country)";
            query += "SELECT SCOPE_IDENTITY()";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Country", customer.Country);
                    cmd.Connection = con;
                    con.Open();
                    customer.CustomerId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }

            return Json(customer);
        }


        [HttpPost]
        public ActionResult UpdateCustomer(CustomerModel customer)
        {
            string query = "UPDATE Customers SET Name=@Name, Country=@Country WHERE CustomerId=@CustomerId";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Country", customer.Country);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM Customers WHERE CustomerId=@CustomerId";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return new EmptyResult();
        }
    }
}