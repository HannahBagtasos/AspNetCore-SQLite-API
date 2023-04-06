using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storageApp.Models
{
    public class logData
    {
        public int Id { get; set;}
        public DateTime Timestamp {get; set;}
        public string? Level {get;set;}  //was giving me a warning so I made it nullable
    }
}