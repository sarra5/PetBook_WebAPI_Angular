using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ReservationGetDTO
    {
        public int PetID { get; set; }

        public int ClinicID { get; set; }

        public DateTime? Date { get; set; }

        public string ClinicName { get; set; }

        public string PetName { get; set; }
    }
}
