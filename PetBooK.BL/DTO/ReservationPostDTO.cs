using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ReservationPostDTO
    {
        public int PetID { get; set; }

        public int ClinicID { get; set; }

        public DateTime? Date { get; set; }
    }
}
