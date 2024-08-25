using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ClinicLocationInclude
    {
        public int ClinicID { get; set; }
        public string Location { get; set; }

        public string Name { get; set; }
        public int? Rate { get; set; }
        public string BankAccount { get; set; }

    }
}
