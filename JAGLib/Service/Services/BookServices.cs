﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Repository.Repositories;
using Repository.EntityModel;

namespace Service.Services
{
    public class BookServices
    {
        static private List<Book> _bookList = null;
        static private List<BookDetails> _bkdtList = null;
        

        // Hämtar alla böcker
        static public List<Book> getBookList()
        {
            _bookList = new List<Book>();
            List<book> bList = BookRepository.dbGetAllBookList();

            foreach (book bookObj in bList)
                _bookList.Add(MapBook(bookObj));

            return _bookList;
        }

        // Hämtar böcker som börjar på angiven bokstav
        static public List<Book> getBookListOnFirstLetter(string c)
        {
            _bookList = new List<Book>();
            List<book> bList = BookRepository.dbGetBookListOnFirstLetter(c);

            foreach (book bookObj in bList)
                _bookList.Add(MapBook(bookObj));

            return _bookList;
        }

        static public List<BookDetails> getBookDetailsFromIsbn(string isbn)
        {
            _bkdtList = new List<BookDetails>();
            List<bookdetails> bList = BookRepository.dbGetDetailsOfBook(isbn);
           

            foreach (bookdetails item in bList)
            {
                _bkdtList.Add(MapBookDetails(item));
            }

            _bkdtList[0]._copyList = mapCopy(BookRepository.dbGetCopyFromISBN(isbn));

            return _bkdtList;
        }

        /*static public List<BookDetails> getCopyFromISBN(string isbn)
        {
            _copyList = new List<BookDetails>();
            List<Copy> cList = mapCopy(BookRepository.dbGetCopyFromISBN(isbn));
                
            foreach(Copy item in cList)
                _copyList.Add(mapCopy(item));

            return _copyList;
        }*/

        static public Book getBookFromISBN(string isbn)
        {
            book bookObj = BookRepository.dbGetBookFromISBN(isbn);

            return MapBook(bookObj);
        }

        // Lägger till angiven Author till databasen
        static public void addBookToDb(Book m)
        {
            BookRepository.dbAddBook(deMapBook(m));
            AuthorRepository.dbAddBookAuthor(m._isbn, m._authorid);
        }

        // Editera author med värdena som kommer in
        static public void EditBook(Book b)
        {
            BookRepository.dbEditBook(deMapBook(b));
        }

        // Tar bort en Book på ISBN & bookens copies
        static public void Remove(string isbn)
        {
            AuthorRepository.dbRemoveBookAuthor(isbn);          // Ta bort author ur book_author

            List<copy> barcodsToRemove = BookRepository.dbGetCopyFromISBN(isbn);
            foreach (copy c in barcodsToRemove)
                BookRepository.dbRemoveBorrows(c.copy_barcode); // Ta bort copyn ur borrows
            
            BookRepository.dbRemoveCopies(isbn);                // Ta bort copies
            BookRepository.dbRemoveBook(isbn);                  // Ta bort bok
        }

        static public bool haveCopysOnLoan(string isbn)
        {
            return BookRepository.dbHaveCopysOnLoan(isbn);
        }

        static private Book MapBook(book bookObj)
        {
            Book theBook = new Book();
            theBook._isbn = bookObj._isbn;
            theBook._title = bookObj._title;
            theBook._signId = bookObj._signId;
            theBook._publicationYear = bookObj._publicationYear;
            theBook._publicationInfo = bookObj._publicationInfo;
            theBook._pages = bookObj._pages;
            return theBook;
        }

        static public Book MapBookPublic(book book)
        {
            return MapBook(book);
        }

        static private book deMapBook(Book bookObj)
        {
            book theBook = new book();
         
            theBook._isbn = bookObj._isbn;
            theBook._title = bookObj._title;
            theBook._signId = bookObj._signId;
            theBook._publicationYear = bookObj._publicationYear;
            theBook._publicationInfo = bookObj._publicationInfo;
            theBook._pages = bookObj._pages;

            
            
            return theBook;
        }

        static private List<Copy> mapCopy(List<copy> c)
        {
            List<Copy> theCopy = new List<Copy>();
            foreach (copy copobj in c)
            {
                Copy cop = new Copy();
                cop._isbn = copobj.copy_isbn;
                cop._barcode = copobj.copy_barcode;
                cop._library = copobj.copy_library;
                cop._status = copobj.copy_status;
                cop._location = copobj.copy_location;
                theCopy.Add(cop);
            }
            return theCopy;
        }

        static private BookDetails MapBookDetails(bookdetails bkdtObj)
        {
            BookDetails theBookDts = new BookDetails();
            theBookDts.book_isbn = bkdtObj.book_isbn;
            theBookDts.book_title = bkdtObj.book_title;
            theBookDts.book_signId = bkdtObj.book_signId;
            theBookDts.book_publicationYear = bkdtObj.book_publicationYear;
            theBookDts.book_publicationInfo = bkdtObj.book_publicationInfo;
            theBookDts.book_pages = bkdtObj.book_pages;
            theBookDts.author_firstname = bkdtObj.author_firstname;
            theBookDts.author_lastname = bkdtObj.author_lastname;
            return theBookDts;
        }
    }
}
