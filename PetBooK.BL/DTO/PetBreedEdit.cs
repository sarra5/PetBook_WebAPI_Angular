using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class PetBreedEdit
    {
        public int PetID { get; set; }
        public int OldBreedID { get; set; }
        public int NewBreedID { get; set; }

    }
}
