using System.Configuration;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    public class CustomerDal
    {
        OrderDal Orders = new OrderDal();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthWindDB"].ConnectionString);
        public bool AddCustomer(string ID, string CompName, string CustName, string CustTitle, string Address)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address) values (@ID, @CompName, @CustName, @CustTitle, @Address)", con);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("compname", CompName);
            cmd.Parameters.AddWithValue("CustName", CustName);
            cmd.Parameters.AddWithValue("@CustTitle", CustTitle);
            cmd.Parameters.AddWithValue("@Address", Address);
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        public bool EditCustomer(string ID, string CompName, string CustName, string CustTitle, string Address)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Customers set CompanyName=@compname, ContactName=@CustName, ContactTitle=@CustTitle, Address=@Address where CustomerID=@ID", con);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("compname", CompName);
            cmd.Parameters.AddWithValue("CustName", CustName);
            cmd.Parameters.AddWithValue("@CustTitle", CustTitle);
            cmd.Parameters.AddWithValue("@Address", Address);
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }



        public bool DeleteCust(string ID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete Customers where CustomerID=@id", con);
            cmd.Parameters.AddWithValue("@id", ID);
            try
            {
                Orders.DeleteOrderDetails(ID);
                Orders.DeleteOrder(ID);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        public Customer NextCustomer(string ID)
        {
            con.Open();
            var Customer_next = new Customer();
            SqlCommand cmd = new SqlCommand("select top 1 CustomerID,CompanyName, ContactName,ContactTitle,Address from Customers where CustomerID>@id Order By CustomerID ASC", con);
            cmd.Parameters.AddWithValue("@id", ID);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    Customer_next.CustomerId = rd.GetString(rd.GetOrdinal("CustomerID"));
                    Customer_next.CompanyName = rd.GetString(rd.GetOrdinal("CompanyName"));
                    Customer_next.CustomerName = rd.GetString(rd.GetOrdinal("ContactName"));
                    Customer_next.CustomerTitle = rd.GetString(rd.GetOrdinal("ContactTitle"));
                    Customer_next.Address = rd.GetString(rd.GetOrdinal("Address"));
                }
                con.Close();
                rd.Close();
            }
            catch(SqlException ex)
            {
                throw ex;
            }

            return Customer_next;
        }

        public Customer PrevCustomer(string ID)
        {
            con.Open();
            var CustomerPrevious = new Customer();
            SqlCommand cmd = new SqlCommand("select top 1 CustomerID,CompanyName, ContactName,ContactTitle,Address from Customers where CustomerID<@id Order By CustomerID DESC", con);
            cmd.Parameters.AddWithValue("@id", ID);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    CustomerPrevious.CustomerId = rd.GetString(rd.GetOrdinal("CustomerID"));
                    CustomerPrevious.CompanyName = rd.GetString(rd.GetOrdinal("CompanyName"));
                    CustomerPrevious.CustomerName = rd.GetString(rd.GetOrdinal("ContactName"));
                    CustomerPrevious.CustomerTitle = rd.GetString(rd.GetOrdinal("ContactTitle"));
                    CustomerPrevious.Address = rd.GetString(rd.GetOrdinal("Address"));
                }
                con.Close();
                rd.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return CustomerPrevious;
        }

        public Customer FirstCustomer()
        {
            con.Open();
            var FirstCust = new Customer();
            var cmd = new SqlCommand("select top 1 CustomerID,CompanyName, ContactName,ContactTitle,Address from Customers Order by CustomerID ASC", con);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    FirstCust.CustomerId = rd.GetString(rd.GetOrdinal("CustomerID"));
                    FirstCust.CompanyName = rd.GetString(rd.GetOrdinal("CompanyName"));
                    FirstCust.CustomerName = rd.GetString(rd.GetOrdinal("ContactName"));
                    FirstCust.CustomerTitle = rd.GetString(rd.GetOrdinal("ContactTitle"));
                    FirstCust.Address = rd.GetString(rd.GetOrdinal("Address"));
                }
                con.Close();
                rd.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return FirstCust;
        }

        public Customer LastCustomer()
        {
            con.Open();
            var LastCust = new Customer();
            var cmd = new SqlCommand("select top 1 CustomerID,CompanyName, ContactName,ContactTitle,Address from Customers Order by CustomerID DESC", con);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    LastCust.CustomerId = rd.GetString(rd.GetOrdinal("CustomerID"));
                    LastCust.CompanyName = rd.GetString(rd.GetOrdinal("CompanyName"));
                    LastCust.CustomerName = rd.GetString(rd.GetOrdinal("ContactName"));
                    LastCust.CustomerTitle = rd.GetString(rd.GetOrdinal("ContactTitle"));
                    LastCust.Address = rd.GetString(rd.GetOrdinal("Address"));
                }
                con.Close();
                rd.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return LastCust;
        }

        /*public List<Customer> GetCustomer()
        {
            List<Customer> List_customer = new List<Customer>();

            con.Open();
            SqlCommand cmd = new SqlCommand("select CustomerID, CompanyName, ContactName, ContactTitle, Address from Customers", con);
            try
            {
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Customer Temp = new Customer();
                    Temp.CustomerId = rd.GetString(rd.GetOrdinal("CustomerID"));
                    Temp.CompanyName = rd.GetString(rd.GetOrdinal("CompanyName"));
                    Temp.CustomerName = rd.GetString(rd.GetOrdinal("ContactName"));
                    Temp.CustomerTitle = rd.GetString(rd.GetOrdinal("ContactTitle"));
                    Temp.Address = rd.GetString(rd.GetOrdinal("Address"));
                    List_customer.Add(Temp);
                }
                rd.Close();
                con.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return List_customer;
        }*/

        
    }
}