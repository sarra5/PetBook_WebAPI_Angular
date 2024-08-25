using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.DTO
{
    public class PetAddDTO
    {
        public string Name { get; set; }

        public IFormFile Photo { get; set; }

        public int? AgeInMonth { get; set; }

        public string Sex { get; set; }
        public IFormFile? IDNoteBookImage { get; set; }

        public int UserID { get; set; }

        public bool ReadyForBreeding { get; set; }

        public string Type { get; set; }

        public string Other { get; set; }
    }
}
