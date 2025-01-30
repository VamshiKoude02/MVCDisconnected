using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace MVCDisconnected.Models
{
    public class CSDBDataContext
    {
        DataTable dt;
        SqlConnection Con;
        SqlDataAdapter da;
        string msg;
        public CSDBDataContext()
        {
            dt = new DataTable();
            Con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCon"].ConnectionString);
            da = new SqlDataAdapter("select * from employee", Con);
        }

        public List<Employee> GetEmployees()
        {
            dt.Clear();
            List<Employee> emps = new List<Employee>();            
            try
            {
                using (da)
                {
                    da.Fill(dt);
                };
                foreach(DataRow dr in dt.Rows)
                {
                    emps.Add(new Employee
                    {
                        Eno = Convert.ToInt32(dr["Eno"]),
                        Ename = Convert.ToString(dr["Ename"]),
                        Job = Convert.ToString(dr["Job"]),
                        Salary = Convert.ToDouble(dr["Salary"]),
                        Dname = Convert.ToString(dr["Dname"])
                    });
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return emps;
        }
        public void InsertEmployee(Employee emp)
        {
            try
            {
                using(da)
                {
                    da.Fill(dt);
                    DataRow dr = dt.NewRow();
                    dr["Eno"] = emp.Eno;
                    dr["Ename"] = emp.Ename;
                    dr["Job"] = emp.Job;
                    dr["Salary"] = emp.Salary;
                    dr["Dname"] = emp.Dname;
                    dt.Rows.Add(dr);
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(dt);
                };
            }
            catch(Exception e)
            {
                msg = e.Message;
            }
        }
        public void DeleteEmployee(int eno)
        {
            try
            {
                using (da)
                {
                    dt.Clear();
                    da.Fill(dt);
                    DataRow dr = dt.AsEnumerable().FirstOrDefault(x => x.Field<int>("Eno") == eno);
                    dr.Delete();
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(dt);
                };
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
        public void UpdateEmployee(Employee emp)
        {
            try
            {
                using (da = new SqlDataAdapter($"select * from Employee where Eno = {emp.Eno}", Con))
                {
                    dt.Clear();
                    da.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    dr["Ename"]= emp.Ename;
                    dr["Job"]= emp.Job;
                    dr["Salary"]= emp.Salary;
                    dr["Dname"]= emp.Dname;
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(dt);
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}