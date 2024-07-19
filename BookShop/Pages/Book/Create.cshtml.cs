using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static BookShop.Pages.Book.IndexModel;

namespace BookShop.Pages.Book
{
    public class CreateModel : PageModel
    {
        public BookInfo bookInfo = new BookInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";
        //[BindProperty]
        //public BookInfo bookInfo { get; set; } = new BookInfo();
        //public string ErrorMessage { get; set; } = "";
        //public string SuccessMessage { get; set; } = "";


        public void OnGet()
        {
        }

        public void OnPost()
        {
            bookInfo.BookID = Convert.ToInt32(Request.Form["bookId"]);
            bookInfo.Title = Request.Form["bookTitle"];
            bookInfo.Author = Request.Form["authorName"];
            bookInfo.year = Convert.ToInt32(Request.Form["year"]);

            //if (bookInfo.BookID == 0 || bookInfo.Title.Length == 0 ||
            //    bookInfo.Author.Length == 0 || bookInfo.year == 0)

            if (bookInfo.BookID == 0 || string.IsNullOrEmpty(bookInfo.Title) ||
           string.IsNullOrEmpty(bookInfo.Author) || bookInfo.year == 0)
            {
                ErrorMessage = "All the Fields are Required";
                return;
            }

            // Save the new book in the database

            try
            {
                String connectionSring = "Data Source=DELL\\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True;TrustServerCertificate=True";
                using(SqlConnection connection = new SqlConnection(connectionSring))
                {
                    connection.Open();
                    String sql = "insert into BookShopTbl" + 
                        "(BookID, Title, Author, Year) values" +
                        "(@BookID, @Title, @Author, @Year);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookInfo.BookID);
                        command.Parameters.AddWithValue("@Title", bookInfo.Title);
                        command.Parameters.AddWithValue("@Author", bookInfo.Author);
                        command.Parameters.AddWithValue("@Year", bookInfo.year);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            bookInfo.BookID = 0; 
            bookInfo.Title = ""; 
            bookInfo.Author = ""; 
            bookInfo.year = 0;
            SuccessMessage = "New Book Added Successfully";

            Response.Redirect("/Book/Index");
        }
    }
}
