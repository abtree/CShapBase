using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;

namespace CommittableTransaction_
{
    [Serializable]
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
    class CommittableTransaction_
    {
        public void DisplayTransactionInfo(string title, TransactionInformation ti)
        {
            if(ti == null)
            {
                Console.WriteLine("{0} TransactionInformation is null", title);
                return;
            }

            Console.WriteLine(title);
            Console.WriteLine("Creation Time: {0:T}", ti.CreationTime);
            Console.WriteLine("Status: {0}", ti.Status);
            Console.WriteLine("Local ID: {0}", ti.LocalIdentifier);
            Console.WriteLine("Distributed ID: {0}", ti.DistributedIdentifier);
            Console.WriteLine();
        }
        public async Task AddStudentAsync(Student student, CommittableTransaction tx, SqlConnection connection)
        {
            //var connection = new SqlConnection("ConnStr");
            //await connection.OpenAsync();
            //var tx = connection.BeginTransaction();

           // try
           // {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO `students`(firstname, lastname, company) VALUES(@FName, @LName, @Company)";

                connection.EnlistTransaction(tx);
                //command.Transaction = tx;
                command.Parameters.AddWithValue("@FName", student.FirstName);
                command.Parameters.AddWithValue("@LName", student.LastName);
                command.Parameters.AddWithValue("@Company", student.Company);
                
                await command.ExecuteNonQueryAsync();

                //tx.Commit();
           // }
           // catch
           // {
                //tx.Rollback();
           // }
           // finally
           // {
           //     connection.Close();
           // }
        }

        public async Task CommittableTransactionAsync()
        {
            var tx = new CommittableTransaction();
            DisplayTransactionInfo("TX created", tx.TransactionInformation);
            var connection = new SqlConnection("ConnStr");

            try
            {
                await connection.OpenAsync();

                //注意 这里如果每个学生建立不同的数据库连接 事务会自动升级为分布式事务
                var s1 = new Student
                {
                    FirstName = "FName",
                    LastName = "LName",
                    Company = "CN innovation"
                };
                await AddStudentAsync(s1, tx, connection);

                var s2 = new Student
                {
                    FirstName = "FName",
                    LastName = "LName",
                    Company = "CN innovation"
                };
                await AddStudentAsync(s2, tx, connection);

                tx.Commit();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx.Rollback();
            }
            finally
            {
                connection.Close();
            }

            DisplayTransactionInfo("TX completed", tx.TransactionInformation);
        }
    }
}
