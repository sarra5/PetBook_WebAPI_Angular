using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
   public class ClinicPhoneUpdateDTO
    {
        public int ClinicID { get; set; }
        public string OldPhone { get; set; }
        public string NewPhone { get; set; }
    }
}
