using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ClinicLocationUpdateDTO
    {
        public int ClinicID { get; set; }
        public string OldLocation { get; set; }
        public string NewLocation { get; set; }
    }
}
