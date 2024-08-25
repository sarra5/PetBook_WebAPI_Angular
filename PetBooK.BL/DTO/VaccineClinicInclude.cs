using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class VaccineClinicInclude
    {
        public int VaccineID { get; set; }
        public int ClinicID { get; set; }
        public string Name { get; set; }
        public int? Rate { get; set; }


        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
