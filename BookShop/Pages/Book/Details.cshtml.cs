using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static BookShop.Pages.Book.IndexModel;

namespace BookShop.Pages.Book
{
    public class DetailsModel : PageModel
    {
        public BookInfo bookInfo = new BookInfo();

        public void OnGet()
        {
            string id = Request.Query["id"];
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
            }
        }
    }
}
