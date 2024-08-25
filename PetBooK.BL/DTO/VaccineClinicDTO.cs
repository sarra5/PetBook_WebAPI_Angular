using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class VaccineClinicDTO
    {
        public int VaccineID { get; set; }
        public int ClinicID { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
