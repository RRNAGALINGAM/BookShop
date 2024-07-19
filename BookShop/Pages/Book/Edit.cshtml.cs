using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static BookShop.Pages.Book.IndexModel;

namespace BookShop.Pages.Book
{
    public class EditModel : PageModel
    {
        public BookInfo bookInfo = new BookInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                ErrorMessage = "BookID parameter is missing";
                return;
            }
            try
            {
                String connectionSring = "Data Source=DELL\\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionSring))
                {
                    connection.Open();
                    String sql = "Select * from BookShopTbl where id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookInfo.BookID = reader.GetInt32(1);
                                bookInfo.Title = reader.GetString(2);
                                bookInfo.Author = reader.GetString(3);
                                bookInfo.year = reader.GetInt32(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            bookInfo.BookID = Convert.ToInt32(Request.Form["bookId"]);
            bookInfo.Title = Request.Form["bookTitle"].ToString();
            bookInfo.Author = Request.Form["authorName"].ToString();
            bookInfo.year = Convert.ToInt32(Request.Form["year"]);

            if (bookInfo.BookID == 0 || string.IsNullOrEmpty(bookInfo.Title) ||
           string.IsNullOrEmpty(bookInfo.Author) || bookInfo.year == 0)
            {
                ErrorMessage = "All the Fields are Required";
                return;
            }
            try
            {
                String connectionSring = "Data Source=DELL\\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionSring))
                {
                    connection.Open();
                    string sql = "UPDATE BookShopTbl " +
             "SET BookID = @BookID, Title = @Title, Author = @Author, Year = @Year " +
             "WHERE BookID = @BookID";

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
            }
            Response.Redirect("/Book/Index");

        }
    }
}
