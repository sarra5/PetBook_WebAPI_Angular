using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedController : ControllerBase
    {
        UnitOfWork unitOfWork;
        IMapper mapper;


        public BreedController(UnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
       
        //------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult Get()
        {
            List<Breed> breeds = unitOfWork.breedRepository.selectall();
            if (breeds == null)
            {
                return NotFound();
            }

            List<BreedGetDTO> BreedDTO = mapper.Map<List<BreedGetDTO>>(breeds);   //mayaaaaa   //foreach (PackageUser PackageUser in packageUsers)

            return Ok(BreedDTO);
        }

        //------------------------------------------------------------------------------------------------

        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            Breed breed =unitOfWork.breedRepository.selectbyid(id);
            if(breed == null) return NotFound();

            BreedGetDTO breedDto= mapper.Map<BreedGetDTO>(breed);
            return Ok(breedDto);
        }

        //-------------------------------------------------------------------------------------------------------

        [HttpPost]

        public IActionResult addBreed(BreedAddDTO newBreed)
        {
            if (newBreed == null) { return BadRequest(); }
            Breed breed = mapper.Map<Breed>(newBreed);
            unitOfWork.breedRepository.add(breed);
            unitOfWork.SaveChanges();
            return Ok(newBreed);

        }

        //------------------------------------------------------------------------------------------------------------

        [HttpPut]

        public IActionResult EditBreed(BreedGetDTO newBreed)
        {
            if (newBreed == null) { BadRequest(); }
            Breed breed = mapper.Map<Breed>(newBreed);
            unitOfWork.breedRepository.update(breed);
            unitOfWork.SaveChanges();
            return Ok(newBreed);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpDelete]

        public IActionResult deleteBreed(int id)
        {
            List<Pet_Breed> PetBreeds = unitOfWork.pet_BreedRepository.FindBy(p => p.BreedID== id);
            foreach (var item in PetBreeds)
            {
                unitOfWork.pet_BreedRepository.deleteEntity(item);
            }
            unitOfWork.SaveChanges();
            unitOfWork.breedRepository.delete(id);
            unitOfWork.SaveChanges();
            return Ok();

        }

        //----------------------------------------------------------------------------------------------------------

        [HttpGet("GetBreedByName/{name}")]
        public IActionResult GetBreedByName(string name)
        {
            List<Breed> petBreed = unitOfWork.breedRepository
                   .SelectAll(p => p.Pet_Breeds)  // Include the navigation property
                   .Where(p => p.Breed1 == name)  // Filter by the breed name
                   .ToList();

            if (petBreed == null)
            {
                return NotFound(new { Message = $"Breed with name '{name}' not found." });
            }

           List<BreedWithPetDTO>  breedPets = mapper.Map<List<BreedWithPetDTO>>(petBreed);

            return Ok(breedPets);
        }

        //---------------------------------------------------------------------------------------------------------

    }
}
