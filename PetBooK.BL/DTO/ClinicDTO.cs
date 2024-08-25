using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ClinicDTO
    {
        public int ClinicID { get; set; }
        public string Name { get; set; }
        public int? Rate { get; set; }
        public string BankAccount { get; set; }
        public List<string> Locations { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
