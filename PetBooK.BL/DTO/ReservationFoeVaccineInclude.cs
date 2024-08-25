using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ReservationFoeVaccineInclude
    {
        public int PetID { get; set; }
        public int ClinicID { get; set; }
        public int VaccineID { get; set; }
        public DateOnly Date { get; set; }
        public string ClinicName { get; set; }
        public string PetName { get; set; }
        public string vaccineName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
