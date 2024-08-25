using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ReservationForVaccineAddDTO
    {
        public int PetID { get; set; }
        public int ClinicID { get; set; }
        public int VaccineID { get; set; }
        public DateOnly Date { get; set; }

    }
}
