using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;


namespace storageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        [HttpPost("query")]
        public ActionResult query([FromBody] JObject requestBody)
        {
            System.Data.SQLite.SQLiteConnection.CreateFile("storage.db");
            string sqlQuery = requestBody.GetValue("query").ToString();

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("data source=storage.db"))
            {
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(conn))
                {
                    conn.Open();
                    cmd.CommandText = sqlQuery;

                    using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<List<string>> results = new List<List<string>>();
                        while (reader.Read())
                        {
                            List<string> row = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i].ToString());
                            }
                            results.Add(row);
                        }
                        conn.Close();
                        return new OkObjectResult(new Result { Data = results });
                    }
                }
            }


        }
        public class Result
        {
            public List<List<string>> Data { get; set; }
        }
    }
}
