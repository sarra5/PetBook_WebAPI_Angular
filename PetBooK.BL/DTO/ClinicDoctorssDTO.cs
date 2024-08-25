using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ClinicDoctorssDTO
    {
        public int ClinicID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Photo { get; set; }
        public string Degree { get; set; }
        public string Phone { get; set; }
    }
}
