using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace Csharpsqlite
{
    class Program

    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello, World!");

            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                  [storage] (
                                  [Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP,
                                  [Level] NVARCHAR(2048) NULL,
                                  [Service] NVARCHAR(2048) NULL,
                                  ";
            //database file
            System.Data.SQLite.SQLiteConnection.CreateFile("storage.db");
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("data source=storage.db"))
            {
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(conn))
                {
                    conn.Open();
                    cmd.CommandText = createQuery;
                    cmd.ExecuteNonQuery();

                    //just test inputs
                    cmd.CommandText = "INSERT INTO storage (Level,Service) values ('e' , 'Inferences.UnusualEvents_Main')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO storage (Level,Service) values ('a' , 'Inferences.UnusualEvents_Main')";
                    cmd.ExecuteNonQuery();


                    //query
                    cmd.CommandText = "SELECT * from storage";
                    using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Level"] + ":" + reader["Service"]);
                        }
                        conn.Close();

                    }

                }
            }
            Console.ReadLine();

            //using (var connection = new SqliteConnection("Data Source=storage.db"))
            //{
               // connection.Open();

              //  var command = connection.CreateCommand();
               // command.CommandText =
                //@"
               //      SELECT name
                //     FROM user
                //     WHERE id = $id
                //";
                //command.Parameters.AddWithValue("$id", id);

               // using (var reader = command.ExecuteReader())
               // {
                   // while (reader.Read())
                    //{
                      //  var name = reader.GetString(0);

                      //  Console.WriteLine($"Hello, {name}!");
                    //}
               // }
           // }
        }
    }

}
