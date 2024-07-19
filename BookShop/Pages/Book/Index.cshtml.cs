using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BookShop.Pages.Book
{
    public class IndexModel : PageModel
    {
        public List<BookInfo> BooksLists = new List<BookInfo>();
        public void OnGet()
        {
            try
            {
                String connectionSring = "Data Source=DELL\\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionSring))
                {
                    string sql = "Select * from BookShopTbl order by BookID asc";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookInfo bookInfo = new BookInfo();
                                bookInfo.id = reader.GetInt32(0);
                                bookInfo.BookID = reader.GetInt32(1);
                                bookInfo.Title = reader.GetString(2);
                                bookInfo.Author = reader.GetString(3);
                                bookInfo.year = reader.GetInt32(4);

                                BooksLists.Add(bookInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public class BookInfo
        {
            //public int id;
            //public int BookID;
            //public string Title;
            //public string Author;
            //public int year;

            public int id { get; set; }
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int year { get; set; }
        }
    }
}
