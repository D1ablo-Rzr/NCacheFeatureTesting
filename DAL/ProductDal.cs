using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Models;
using System.Configuration;

namespace DAL
{
    public class ProductDal
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthWindDB"].ConnectionString);
        public List<Products> AddProducts(int ProdID)
        {
            List<Products> ProductList = new List<Products>();
            con.Open();
            var cmd = new SqlCommand("select Top 5 ProductID,ProductName,UnitPrice from Products where ProductID>@id Order By ProductID ASC", con);
            cmd.Parameters.AddWithValue("@id", ProdID);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Products Temp = new Products();
                    Temp.ProductID = rd.GetInt32(rd.GetOrdinal("ProductID"));
                    Temp.ProductName = rd.GetString(rd.GetOrdinal("ProductName"));
                    Temp.UnitPrice = rd.GetDecimal(rd.GetOrdinal("UnitPrice"));
                    ProductList.Add(Temp);
                }
                con.Close();
                rd.Close();
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            //AddtoCache();

            return ProductList;
        }

        public List<Employees> AddEmployees(int EmpID)
        {
            con.Open();
            List<Employees> EmployeeList = new List<Employees>();
            var cmd1 = new SqlCommand("select Top 5 EmployeeId,FirstName from Employees where EmployeeID>@id Order By EmployeeID ASC", con);
            cmd1.Parameters.AddWithValue("@id", EmpID);
            try
            {
                SqlDataReader rd1 = cmd1.ExecuteReader();
                while (rd1.Read())
                {
                    Employees Temp = new Employees();
                    Temp.EmployeeID = rd1.GetInt32(rd1.GetOrdinal("EmployeeId"));
                    Temp.FirstName = rd1.GetString(rd1.GetOrdinal("FirstName"));
                    EmployeeList.Add(Temp);
                }
                con.Close();
                rd1.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return EmployeeList;
        }
    }
}
