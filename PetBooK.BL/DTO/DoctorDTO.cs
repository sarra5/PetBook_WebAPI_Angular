using PetBooK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class DoctorDTO
    {
        public int DoctorID { get; set; }

        public string Name { get; set; }
        public string Degree { get; set; }

        public DateOnly HiringDate { get; set; }
        
        
    }
}
