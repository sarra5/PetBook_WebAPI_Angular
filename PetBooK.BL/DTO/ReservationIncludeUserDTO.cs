using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class ReservationIncludeUserDTO
    {
        public int PetID { get; set; }

        public int ClinicID { get; set; }
        public int UserID { get; set; }

        public DateTime? Date { get; set; }
        public string PetName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
