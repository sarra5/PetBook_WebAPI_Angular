using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class RequestBreedUpdateDTO
    {
        public int PetIDSender { get; set; }

        public int PetIDReceiver { get; set; }
        public bool Pair { get; set; }
    }
}
