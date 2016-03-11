﻿using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace JAGLibrary.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Admin()
        {
            if (Session["user"] == null)
                    return Redirect("/Home/Login/");
            else 
            {
                LoginData user = (LoginData)Session["user"];
                if (user._level == "1")
                    return Redirect("/Borrower/Borrower/");
                else if (user._level == "2")
                {
                    Session["name"] = "Admin";
                    return View();
                }
                else return Redirect("/Home/Login/");
            }
        }

        public ActionResult AddAuthor()
        {
            return View("AddAuthor", "_StandardLayout");
        }

        public ActionResult ListAuthors()
        {
            var model = new ListAuthor();
            model._authList = Service.Services.AuthorServices.getAuthorList();

            return View("ListAuthors", "_StandardLayout", model);
        }

        public ActionResult EditAuthor(int aid)
        {
            var model = Service.Services.AuthorServices.getAuthorFromAid(aid);

            return View("EditAuthor", "_StandardLayout", model);
        }

        private string getSalt(int maxLength)
        {
            var salt = new byte[maxLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public string getHash(string str, string salt)
        {
            string hash = str + salt;
            return hash.GetHashCode().ToString(); ;
        }


        private void saveBorrower(Common.Models.Borrower m)
        {
            int saltLenght = 32;
            LoginData ld = new LoginData();

            ld._salt = getSalt(saltLenght);
            ld._password = getHash(m._password, ld._salt); ;
            ld._username = m._pid;
            ld._level = "1";
            //ld._hash = getHash(m._password, ld._salt);

            //Add to database.
            Borrower person = new Borrower();
            person._pid = m._pid;
            person._firstname = m._firstname;
            person._lastname = m._lastname;
            person._address = m._address;
            person._phoneno = m._phoneno;
            person._catId = m._catId;

            //add person to database
            Service.Services.BorrowerService.addBorrowerToDb(person);
            Service.Services.LoginService.addUserToDb(ld);
        } 

        
        public ActionResult AddBorrower()
        {
            List<SelectListItem> categoryId = new List<SelectListItem>();
            categoryId.Add(new SelectListItem { Text = "Extern", Value = "1" });
            categoryId.Add(new SelectListItem { Text = "Staff", Value = "2" });
            categoryId.Add(new SelectListItem { Text = "Student", Value = "3" });
            categoryId.Add(new SelectListItem { Text = "Child", Value = "4" });
            ViewData["Select Category"] = categoryId;

            var model = new Borrower();
            return View("AddBorrower", "_StandardLayout", model);
        }


        //[HttpGet]
        public ActionResult AddBorrowerForm(Common.Models.Borrower m) 
        {
           
            var model = new Borrower();
            m = model;
            List<Borrower> borrowerList = Service.Services.BorrowerService.getBorrowerList();
            
            if (!borrowerList.Exists(x => x._pid == m._pid))
            {
                saveBorrower(m);
                ConfirmationAdmin(null, null, model);
            }
         
            return Redirect("AddBorrower");
        }

        public ActionResult EditBorrower(string bid)
        {
            var model = Service.Services.BorrowerService.getBorrower(bid);

            return View("EditBorrower", "_StandardLayout", model);
        }

        public ActionResult ListBorrowers()
        {
            var model = new ListBorrower();
            model._borrList = Service.Services.BorrowerService.getBorrowerList();

            return View("ListBorrowers", "_StandardLayout", model);
        }

        public ActionResult AddBook()
        {
            return View("AddBook", "_StandardLayout");
        }

        public ActionResult EditBook(string s)
        {
            var model = Service.Services.BookServices.getBookFromISBN(s);

            return View("EditBook", "_StandardLayout", model);
        }

        public ActionResult ListBooks()
        {
            var model = new ListBook();
            model._bookList = Service.Services.BookServices.getBookList();

            return View("ListBooks", "_StandardLayout", model);
        }

        public ActionResult Remove(int aid, string isbn, string bid)
        {
            var model = new Remove();
            if (aid != 0) {
                model._cat = 1;
                model._author = Service.Services.AuthorServices.getAuthorFromAid(aid);
                return View("Remove", "_StandardLayout", model);
            } else if (isbn != "0") {
                model._cat = 2;
                model._book = Service.Services.BookServices.getBookFromISBN(isbn);
                return View("Remove", "_StandardLayout", model);
            } else {
                model._cat = 3;
                model._borrower = Service.Services.BorrowerService.getBorrower(bid);
                return View("Remove", "_StandardLayout", model);
            } 
        }

        //[HttpGet]
        public ActionResult AddAuthorForm(Common.Models.Author m)
        {            
            Service.Services.AuthorServices.addAuthorToDb(m);
            var model = new Author();
            m = model;
            ConfirmationAdmin(model, null,null);

            return View("AddAuthor", "_StandardLayout", m);
        }

        public ActionResult ConfirmationAdmin(Common.Models.Author authorData, Common.Models.Book bookData, Common.Models.Borrower borrowerData)
        {
            var conf = new ConfirmationAdmin();

            if(authorData != null)
            {
                conf._firstName = authorData._firstname;
                conf._lastName = authorData._lastname;
            }
            else if (bookData != null)
            {
                conf._title = bookData._title;
            }
            else
            {
                conf._firstName = borrowerData._firstname;
                conf._lastName = borrowerData._lastname;
            }
            
            return View("ConfirmationAdmin", "_StandardLayout", conf);
        }

        //[HttpGet]
        public ActionResult EditAuthorForm(Common.Models.Author m)
        {
            return View("AddAuthor", "_StandardLayout", m);
        }

        //[HttpGet]
        public ActionResult AddBookForm(Common.Models.Book m)
        {
            Service.Services.BookServices.addBookToDb(m);
            var model = new Book();
            m = model;
            ConfirmationAdmin(null, model, null);

            return View("AddBook", "_StandardLayout", m);
        }

        public ActionResult RemoveThis(int cat, int aid, string isbn, string bid)
        {
            if (cat == 1) {
                Service.Services.AuthorServices.Remove(aid);
            } else if (cat == 2) {
                
            } else {
                
            }

            return View("Admin", "_StandardLayout");
        }
    }
}