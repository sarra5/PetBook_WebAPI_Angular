using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.DAL.Models
{
    public  class Clinic_Doctor
    {
        [ForeignKey("Clinic")]
        public int ClinicID { get; set; }
        public Clinic clinic { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor doctor { get; set; }


    }
}
