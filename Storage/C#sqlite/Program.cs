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
            string tableReg = @"(?<table>Logs)\s";
            string timestampReg = @"Timestamp\s+>=\s+datetime\((\d+-\d+-\d+\s+\d+:\d+)\)";
            string levelReg = @"Level\s+==\s+""(\w+)""";
            string limitReg = @"limit\s+(\d+)";
            

            foreach (string kql_query in queries)
            {

                Match table = Regex.Match(kql_query, tableReg);
                Match timestamp = Regex.Match(kql_query, timestampReg);
                Match level= Regex.Match(kql_query, levelReg);
                Match limit = Regex.Match(kql_query, limitReg);


                string sqlQuery = string.Format("SELECT * FROM '{0}' WHERE Timestamp >= '{1}' AND Level = '{2}' LIMIT {3};",
                                                 table.Groups[1].Value,
                                                 timestamp.Groups[1].Value,
                                                 level.Groups[1].Value,
                                                 limit.Groups[1].Value);

                Console.WriteLine(sqlQuery);

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
