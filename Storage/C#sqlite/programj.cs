using Kusto.Language.Symbols;
using Kusto.Language;
using Kusto.Language.Syntax;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sqlite
{
    class Programj
    {
        static void Main(string[] args)
        {
            var globals = GlobalState.Default.WithDatabase(
                new DatabaseSymbol("db",
                    new TableSymbol("T", "(a: real, b: real)")));

            var query = "T | project a = a + b | where a > 10.0";
            var code = KustoCode.ParseAndAnalyze(query, globals);
            // search syntax tree for references to specific columns
            var columnA = globals.Database.Tables.First(t => t.Name == "db").GetColumn("a");
            var referencesToA = code.Syntax.GetDescendants<NameReference>(n => n.ReferencedSymbol == columnA);

            // there is only one reference to the column named "a" from the table "T"
            Assert.AreEqual(1, referencesToA.Count);
        }

       
    }
}
