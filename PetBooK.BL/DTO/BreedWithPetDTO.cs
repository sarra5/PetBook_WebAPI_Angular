using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public  class BreedWithPetDTO
    {
        public int BreedID { get; set; }
        public string Breed1 { get; set; }
        public List<int> PetID { get; set; }

    }
}
