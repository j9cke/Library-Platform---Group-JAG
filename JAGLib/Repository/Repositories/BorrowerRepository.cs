﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.EntityModel;
using System.Data.SqlClient;

namespace Repository.Repositories
{
    public class BorrowerRepository
    {
        static public borrower dbGetBorrower(string id)
        {
            borrower _brwObj = null;
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM borrower WHERE PersonId = '" + id + "';", con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    _brwObj = new borrower();
                    _brwObj._pid = dar["PersonId"] as string;
                    _brwObj._firstname = dar["FirstName"] as string;
                    _brwObj._lastname = dar["LastName"] as string;
                    _brwObj._address = dar["Address"] as string;
                    _brwObj._phoneno = dar["Telno"] as string;
                    _brwObj._catId = (int)dar["CategoryId"];
                }
            }
            catch (Exception eObj)
            {
                throw eObj;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return _brwObj;
        }

        static private List<borrower> dbGetBorrowerList(string query)
        {
            List<borrower> _brwList = null;
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar != null)
                {
                    _brwList = new List<borrower>();
                    while (dar.Read())
                    {
                        borrower _brwObj = new borrower();
                        _brwObj._pid = dar["PersonId"] as string;
                        _brwObj._firstname = dar["FirstName"] as string;
                        _brwObj._lastname = dar["LastName"] as string;
                        _brwObj._address = dar["Address"] as string;
                        _brwObj._phoneno = dar["Telno"] as string;
                        string cat = dar["CategoryId"] as string;
                        if (cat == null)
                            _brwObj._catId = 0;
                        else
                            _brwObj._catId = Convert.ToInt32(cat);
                        _brwList.Add(_brwObj);
                    }
                }
            }
            catch (Exception eObj)
            {
                throw eObj;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return _brwList;
        }

        static private void dbInsert(string query)
        {
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }

            catch (Exception eObj) { throw eObj; }
            finally { if (con != null) con.Close(); }
        }

        static private void dbRemoveOrEdit(string query)
        {
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }

            catch (Exception eObj) { throw eObj; }
            finally { if (con != null) con.Close(); }
        }

        static public List<borrower> dbGetAllBorrowerList()
        {
            return dbGetBorrowerList("SELECT * FROM borrower;");
        }

        static public List<borrower> dbGetDepartmentEmployeeList(int catId)
        {
            return dbGetBorrowerList("SELECT * FROM borrower WHERE aid = " + Convert.ToString(catId) + ";");
        }

        static public void dbAddBorrower(borrower b)
        {
            dbInsert("INSERT INTO BORROWER VALUES ('" + b._pid + "', '" + b._firstname + "', '" + b._lastname + "', '" + b._address + "', '" + b._phoneno + "', '" + b._catId + "');");
        }

        /***************  GET BORROW FOR A BORROWER  ******************/
        static private List<borrow> dbGetBorrowListForPerson(string query)
        {
            List<borrow> _brwList = null;
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar != null)
                {
                    _brwList = new List<borrow>();
                    while (dar.Read())
                    {
                        //borrow _brwObj = new borrow();
                        //_brwObj.pid = dar["PersonId"] as string;
                        //_brwObj.barcode = dar["Barcode"] as string;
                        //_brwObj.returnDate = dar["ReturnDate"] as string;
                        //_brwObj.borrowDate = dar["BorrowDate"] as string;
                        //_brwObj.toBeReturnedDate = dar["ToBeReturnedDate"] as string;
                       
                        //_brwList.Add(_brwObj);
                    }
                }
            }
            catch (Exception eObj)
            {
                throw eObj;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return _brwList;
        }
        static public List<borrow> dbGetAllBorrowList(string pId)
        {
            return dbGetBorrowListForPerson("SELECT * FROM borrower WHERE PersonId = '" + pId + "';");
        }

        static private category dbGetCategoryforCatId(string query)
        {
            category cat = new category();
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar.Read())
                {
                    cat.categoryId = (int)dar["CategoryId"]; // as string;
                    cat.period = (int)dar["Period"]; //as string;
                    cat.categoryt = dar["Category"] as string;
                    cat.penaltyperday = (int)dar["Penaltyperday"];
                }
            }
            catch (Exception eObj)
            {
                throw eObj;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }



            return cat;
        }



        static private borrowerdetails dbGetBorrowerDetailsforpid(string query)
        {
            borrowerdetails brwd = new borrowerdetails();;
            string _connectionString = DataSource.getConnectionString("projectmanager");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                SqlDataReader dar = cmd.ExecuteReader();
                if (dar != null)
                {
                    while (dar.Read())
                    {
                        brwd._pid = dar["PersonId"] as string;
                        brwd._firstname = dar["FirstName"] as string;
                        brwd._lastname = dar["LastName"] as string;
                        brwd._address = dar["Address"] as string;
                        brwd._phoneno = dar["Telno"] as string;
                        brwd._catId = (int)dar["CategoryId"];
                        
                        borrow _brwObj = new borrow();
                        _brwObj.barcode = dar["Barcode"] as string;

                        DateTime temp;
                        if (DateTime.TryParse(dar["ReturnDate"].ToString(), out temp))
                            _brwObj.returnDate = temp;
                        if (DateTime.TryParse(dar["BorrowDate"].ToString(), out temp))
                            _brwObj.borrowDate = temp;
                        if (DateTime.TryParse(dar["ToBeReturnedDate"].ToString(), out temp))
                            _brwObj.toBeReturnedDate = temp;


                        if (_brwObj.returnDate == null && _brwObj.toBeReturnedDate > DateTime.Now)
                        {
                            category cat = dbGetCategory(brwd._catId.ToString());
                            _brwObj.penalty = ((DateTime.Today - _brwObj.toBeReturnedDate.Date).TotalDays * cat.penaltyperday).ToString();
                        }


                        _brwObj.pid = dar["PersonId"] as string;

                        _brwObj.book = new book();
                        _brwObj.book._isbn = dar["ISBN"] as string;
                        _brwObj.book._title = dar["Title"] as string;
                        _brwObj.book._signId = (int)dar["SignId"];
                        _brwObj.book._publicationYear = dar["PublicationYear"] as string;
                        _brwObj.book._publicationInfo = dar["publicationinfo"] as string;
                        _brwObj.book._pages = (Int16)dar["pages"];

                        
                        brwd._borrowlist.Add(_brwObj);
                    }
                }
            }
            catch (Exception eObj)
            {
                throw eObj;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return brwd;
        }

        static public borrowerdetails dbGetBorrowerDetails(string pid)
        {
            return dbGetBorrowerDetailsforpid("SELECT * FROM BORROW INNER JOIN BORROWER ON BORROW.PersonId = BORROWER.PersonId INNER JOIN COPY ON BORROW.Barcode = COPY.Barcode INNER JOIN BOOK ON COPY.ISBN = BOOK.ISBN WHERE BORROWER.PersonId LIKE '" + pid + "';");

        }

        static public void dbEditBorrower(borrower b)
        {
            dbRemoveOrEdit("UPDATE BORROWER SET FirstName='" + b._firstname + "', LastName='" + b._lastname + "', Address='" + b._address + "', Telno='" + b._phoneno + "', CategoryId='" + b._catId + "' WHERE PersonId='" + b._pid + "';");
        }


        static public category dbGetCategory(string catID)
        {
            return dbGetCategoryforCatId("SELECT * FROM CATEGORY WHERE CategoryId = '" + catID + "';");
        }

   }
}