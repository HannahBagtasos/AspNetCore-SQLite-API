using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using NUnit.Framework;
using System.Reflection.Emit;


namespace Csharpsqlite
{
    class Program

    {
        static void Main(string[] args)
        {
            //test merge2

            List<string> queries = new List<string>()

                {
               
                 "Logs | where Timestamp >= datetime(2015-08-22 05:00) | where Level == \"e\" | limit 10",
                 "Logs | where Timestamp >= datetime(2015-08-22 07:00) | where Level == \"e\" | limit 11",
                 "Logs | where Timestamp >= datetime(2015-09-22 05:00) | where Level == \"e\" | limit 12",
                 "Logs | where Timestamp >= datetime(2015-10-22 05:00) | where Level == \"e\" | limit 13",
                 "Logs | where Timestamp >= datetime(2015-10-22 08:00) | where Level == \"e\" | limit 14",
                 "Logs | where Timestamp >= datetime(2015-11-22 05:00) | where Level == \"e\" | limit 15",
                 "Logs | where Timestamp >= datetime(2015-12-22 05:00) | where Level == \"e\" | limit 16",
                 "Logs | where Timestamp >= datetime(2016-09-22 05:00) | where Level == \"e\" | limit 17",
                 "Logs | where Timestamp >= datetime(2016-09-22 05:00) | where Level == \"e\" | limit 18",
                 "Logs | where Timestamp >= datetime(2016-10-22 05:00) | where Level == \"e\" | limit 19",
                };


            //All occurrences of the name "a" in the C# code represented are found 
            //and returned as a collection of NameReference nodes in the referencesToA variable.

            foreach (string query in queries)
            {
                KustoCode code = KustoCode.Parse(query);
                //var references = code.Syntax.GetDescendants<NameReference>(n => n.SimpleName == "a");
                var references = code.Syntax.GetDescendants<NameReference>(n => n.SimpleName == "Level" || n.SimpleName == "Timestamp" || n.SimpleName == "Logs" || n.SimpleName == "Service");
                foreach (var reference in references)
                {
                    if (reference.SimpleName == "timestamp" || reference.SimpleName == "logs" || reference.SimpleName == "service")
                    {
                        Console.WriteLine("Taking the QL in query '{0}': {1}", query, reference.SimpleName);
                    }
                }
                //Console.WriteLine("Taking the QL in query '{0}': {1}", query, references.Count());
                //Console.WriteLine(code.ToString()); 
                //SELECT Level, Timestamp, Message
                //FROM Logs
                //WHERE Timestamp >= '2015-08-22 05:00' AND Timestamp< '2015-08-22 06:00'
                // Level = 'e' AND Service = 'Inferences.UnusualEvents_Main'
                //LIMIT 10;
                // level, timestamo, logs, service
              


            }




            string createQuery = @"CREATE TABLE IF NOT EXISTS
                                  [storage] (
                                  [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                  [Timestamp] DATETIME DEFAULT CURRENT_TIMESTAMP,
                                  [Level] NVARCHAR(2048) NULL,
                                  [Service] NVARCHAR(2048) NULL
                                  )";


            //database file
            System.Data.SQLite.SQLiteConnection.CreateFile("storage.db");
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("data source=storage.db"))
            {
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(conn))
                {
                    conn.Open();
                    cmd.CommandText = createQuery;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO storage(Level,Service) values('e' , 'Inferences.UnusualEvents_Main')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO storage(Level,Service) values('a' , 'Inferences.UnusualEvents_Main')";
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

        }

    }

}
