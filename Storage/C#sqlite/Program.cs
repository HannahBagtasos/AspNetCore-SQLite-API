using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using NUnit.Framework;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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
            string timestamp_pattern = @"Timestamp\s+>=\s+datetime\((\d+-\d+-\d+\s+\d+:\d+)\)";
            string level_pattern = @"Level\s+==\s+""(\w+)""";
            string limit_pattern = @"limit\s+(\d+)";

            foreach (string kql_query in queries)
            {

                Match timestamp_match = Regex.Match(kql_query, timestamp_pattern);
                Match level_match = Regex.Match(kql_query, level_pattern);
                Match limit_match = Regex.Match(kql_query, limit_pattern);


                string sql_query = string.Format("SELECT * FROM Logs WHERE Timestamp >= '{0}' AND Level = '{1}' LIMIT {2};",
                                                 timestamp_match.Groups[1].Value,
                                                 level_match.Groups[1].Value,
                                                 limit_match.Groups[1].Value);

                Console.WriteLine(sql_query);

            }

            Console.WriteLine("Hello");



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
