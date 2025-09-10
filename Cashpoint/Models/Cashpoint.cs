using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cashpoint.Models
{
    public class Cashpoint
    {
        public int TapeCount { get; set; }
        public List<int> TapeNominals { get; set; }
        public List<int> NominalsCount { get; set; }
        public List<bool> TapeStatuses { get; set; }
    }
    
    public class CashpointQuery : Cashpoint
    {
        public int Cash { get; set; }
    }
}