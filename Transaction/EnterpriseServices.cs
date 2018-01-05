using System;
using System.EnterpriseServices;
using System.Data.SqlClient;

namespace EnterpriseService
{
    public struct Course
    {
        public int Number { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// 这里只添加了注解 系统会自动使用事务
    /// </summary>
    [Transaction(TransactionOption.Required)]
    public class EnterpriseService : ServicedComponent
    {
        [AutoComplete]
        public void AddCourse(Course course)
        {
            var connection = new SqlConnection("ConnStr");
            SqlCommand courseCommand = connection.CreateCommand();
            courseCommand.CommandText = "INSERT INTO courses(Number, Title) VALUES(@Number, @Title)";
            connection.Open();
            try
            {
                courseCommand.Parameters.AddWithValue("@Number", course.Number);
                courseCommand.Parameters.AddWithValue("@Title", course.Title);
                courseCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
