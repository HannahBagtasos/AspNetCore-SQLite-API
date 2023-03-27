using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using System.Reflection.Emit;
using System;


namespace Csharpsqlite
{
    class parse

    {
        static void Main(string[] args)
        {
            // parse only
            var query = "T | project a = a + b | where a > 10.0";
            var code = KustoCode.Parse(query);

            //connect to a database
            var sqlGenerator = new SqlGenerator();
            var sqlQuery = sqlGenerator.Generate(code.Syntax);

            Console.WriteLine(sqlQuery);



        }
    }

}
