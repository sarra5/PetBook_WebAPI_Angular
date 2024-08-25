using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class RequestBreedDTO
    {
        public string senderPetName { get; set; }
        public string receiverPetName { get; set; }

        public string OwnersenderName { get; set; }

        public string OwnerreceiverName { get; set; }

        public int PetIDSender { get; set; }

        public int PetIDReceiver { get; set; }

        public bool Pair { get; set; }

       
    }
}
