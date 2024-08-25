using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
   public class SecretaryDTO
    {

        public int SecretaryID { get; set; }
        public decimal Salary { get; set; }
        public DateOnly HiringDate { get; set; }
        public int? ClinicID { get; set; }
        public string ?ClinicName { get; set; }
        public string BankAccount { get; set; }
        public string Name { get; set; } 
        public string Location { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; }
    }

   
}
