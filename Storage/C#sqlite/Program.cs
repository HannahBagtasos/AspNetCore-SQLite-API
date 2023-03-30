using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;


namespace Csharpsqlite
{
    class Program

    {
        static void Main(string[] args)
        {
            //test merge

            List<string> queries = new List<string>()

                {
                 "Logs | where Timestamp >= datetime(2015-08-22 05:00) and Timestamp < datetime(2015-08-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 10",
                 "Logs | where Timestamp >= datetime(2015-08-22 07:00) and Timestamp < datetime(2015-08-22 08:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 11",
                 "Logs | where Timestamp >= datetime(2015-09-22 05:00) and Timestamp < datetime(2015-09-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 12",
                 "Logs | where Timestamp >= datetime(2015-10-22 05:00) and Timestamp < datetime(2015-10-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 13",
                 "Logs | where Timestamp >= datetime(2015-10-22 08:00) and Timestamp < datetime(2015-10-22 09:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 14",
                 "Logs | where Timestamp >= datetime(2015-11-22 05:00) and Timestamp < datetime(2015-11-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 15",
                 "Logs | where Timestamp >= datetime(2015-12-22 05:00) and Timestamp < datetime(2015-12-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 16",
                 "Logs | where Timestamp >= datetime(2016-09-22 05:00) and Timestamp < datetime(2016-09-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 17",
                 "Logs | where Timestamp >= datetime(2016-09-22 05:00) and Timestamp < datetime(2016-10-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 18",
                 "Logs | where Timestamp >= datetime(2016-10-22 05:00) and Timestamp < datetime(2016-10-22 06:00) | where Level == \"e\" and Service == \"Inferences.UnusualEvents_Main\" | project Level, Timestamp, Message | limit 19",
                };


            //All occurrences of the name "a" in the C# code represented are found 
            //and returned as a collection of NameReference nodes in the referencesToA variable.

            foreach (string query in queries)
            {
                KustoCode code = KustoCode.Parse(query);
                var referencesToA = code.Syntax.GetDescendants<NameReference>(n => n.SimpleName == "a");
                Console.WriteLine("References to 'a' in query '{0}': {1}", query, referencesToA.Count());
                //Console.WriteLine(code.ToString()); // or code.Syntax.ToString() for just the syntax tree
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
