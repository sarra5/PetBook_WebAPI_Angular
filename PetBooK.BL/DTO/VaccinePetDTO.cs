using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class VaccinePetDTO
    {
        public int VaccineID { get; set; }
        public int PetID { get; set; }
        public double? Dose { get; set; }
        public DateTime? Time { get; set; }
        public bool IsVaccinated { get; set; }
    }
}
