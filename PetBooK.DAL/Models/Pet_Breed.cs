using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.DAL.Models
{
    public  class Pet_Breed
    {

        [ForeignKey("Pet")]
        public int PetID { get; set; }
        public Pet Pet { get; set; }

        [ForeignKey("Breed")]
        public int BreedID { get; set; }
        public Breed Breed { get; set; }


    }
}
