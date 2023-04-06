using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storageApp.DTO
{
    public class queryResultReadListDTO
    {
        public List<queryResultReadDTO> data { get; set; }
    }

    public class queryResultReadDTO
    {
        public String value1 { get; set; }
        public String value2 { get; set; }
    }
}