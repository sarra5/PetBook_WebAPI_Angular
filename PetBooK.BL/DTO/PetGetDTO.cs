using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class PetGetDTO
    {
        public int PetID { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public int? AgeInMonth { get; set; }

        public string Sex { get; set; }

        public string IDNoteBookImage { get; set; }

        public int UserID { get; set; }

        public bool ReadyForBreeding { get; set; }
        public string BreedName { get; set; }
        public string Type { get; set; }

        public string Other { get; set; }
    }
}
