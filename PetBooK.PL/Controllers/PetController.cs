using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using PetBooK.DAL.Services;
using PetBooK.PL.Hubs;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        UnitOfWork unitOfWork;
        IMapper mapper;
        IFileService fileService;

        // For SignalR to broadcast for frontend
        IHubContext<PetHub> hubContext;

        public PetController(UnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IHubContext<PetHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileService = fileService;
            this.hubContext = hubContext;
        }

        //------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult GetAllPets()
        {
            List<Pet> pets = unitOfWork.petRepository.selectall();
            if (pets == null) { return BadRequest(); }

            List<PetGetDTO> PetDTO = mapper.Map<List<PetGetDTO>>(pets);
            return Ok(PetDTO);
        }

        //---------------------------------------------------------------------------------------------------
        //get all pets when are ready for breeding

        [HttpGet("RequestPet")]
        public IActionResult GetAllTypesWithoutFilterWhenRequestIsTrue()
        {
            List<Pet> pets = unitOfWork.petRepository.FindBy(p => p.ReadyForBreeding == true);
            if (pets == null) { return BadRequest(); }


            List<PetGetDTO> PetDTO = mapper.Map<List<PetGetDTO>>(pets);
            return Ok(PetDTO);
        }
        //-----------------------------------search breedname-------------
        [HttpGet("SearchBreedNameOfPetsReadyForBreeding")]
        public IActionResult BreedNameOfPetsReadyForBreeding()
        {
            try
            {
                var targetBreed = unitOfWork.breedRepository.selectall();
               
                List<string> Names = new List<string>();
                foreach (var item in targetBreed)
                {
                    Names.Add(item.Breed1);
                }
               
                return Ok(Names);
                
            }
            catch
            {
                return StatusCode(500, "error while retrieving data");
            }
            //var petBreedRelationships = unitOfWork.pet_BreedRepository.FindBy(pb => pb.BreedID == targetBreed.BreedID).ToList();
            //var petIds = petBreedRelationships.Select(pb => pb.PetID).ToList();

            //var pets = unitOfWork.petRepository.FindBy(p => p.ReadyForBreeding == true && petIds.Contains(p.PetID)).ToList();
            //if (!pets.Any())
            //{
            //    return NotFound("No pets ready for breeding found.");
            //}

            //var petDTOs = mapper.Map<List<PetGetDTO>>(pets);
            //return Ok(petDTOs);

        }
        /////////Lookkkk
        [HttpGet("SearchPetsReadyForBreeding")]
        public IActionResult GetAllPetsReadyForBreeding(string type = "", string sex = "", string search = "", int pageNumber = 1, int pageSize = 4)
        {
            var petsQuery = unitOfWork.petRepository.FindByIncludeThenInclude(
                p => p.ReadyForBreeding &&
                     (string.IsNullOrEmpty(type) || p.Type == type) &&
                     (string.IsNullOrEmpty(sex) || p.Sex == sex) &&
                     (string.IsNullOrEmpty(search) || p.Pet_Breeds.Any(pb => pb.Breed.Breed1.Contains(search))),
                p => p.Pet_Breeds,
                pb => pb.Breed);


            var petDTOs = mapper.Map<List<PetGetDTO>>(petsQuery);

            int total = petDTOs.Count();
            List<PetGetDTO> petDTOs2 = petDTOs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var response = new
            {
                Data = petDTOs2,
                AllData = petDTOs,
                TotalItems = total
            };
            return Ok(response);
        }

        
        //--------------------------------------------------------------------------------------
        [HttpPost] //Edit By Amira
        public async Task<IActionResult> PostPet([FromForm] PetAddDTO NewPet)
        {
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png", ".webp"];

            string createdImageName = await fileService.SaveFileAsync(NewPet.Photo, allowedFileExtentions);
            string createdImageIDBook = await fileService.SaveFileAsync(NewPet.IDNoteBookImage, allowedFileExtentions);

            if (NewPet == null)
            {
                return BadRequest();
            }

            else
            {
                Pet pet = new Pet
                {
                    Name = NewPet.Name,
                    Photo = createdImageName,
                    AgeInMonth = NewPet.AgeInMonth,
                    Sex = NewPet.Sex,
                    IDNoteBookImage = createdImageIDBook,
                    ReadyForBreeding = NewPet.ReadyForBreeding,
                    UserID = NewPet.UserID,
                    Type = NewPet.Type,
                    Other = NewPet.Other,
                };

                unitOfWork.petRepository.add(pet);
                unitOfWork.SaveChanges();

                if (pet.ReadyForBreeding == true)
                {
                    hubContext.Clients.All.SendAsync("PetWithReadyForBreedingTrue", pet);
                }

                return Ok(pet);
            }
        }
        //------------------------------------------------------------------------------------------------


        [HttpGet("{id}")]
            public IActionResult GetId(int id)
            {
                Pet pet = unitOfWork.petRepository.selectbyPetid(id);
                if (pet == null) { return BadRequest(); }
                PetGetDTO petDTO = mapper.Map<PetGetDTO>(pet);
                return Ok(petDTO);
            }

            //------------------------------------------------------------------------------------------------

            [HttpGet("GetByUserID/{userId}")]
            public IActionResult GetPetByUserId(int userId)
            {
                List<Pet> pets = unitOfWork.petRepository.FindByForeignKey(p => p.UserID == userId);
                if (pets == null) { return BadRequest(); }
                List<PetGetDTO> petDTO = mapper.Map<List<PetGetDTO>>(pets);
                return Ok(petDTO);
            }
            //-----------------------------------------------------------------------------------------------------




            [HttpPut]

            public async Task<IActionResult> Edit([FromForm] PetUpdateDTO NewPet)
            {
 
                var existingPet = unitOfWork.petRepository.selectbyid(NewPet.PetID);
                if (existingPet == null)
                {
                    return NotFound();
                }

                // Separate photo handling for PetPhoto
                if (NewPet.Photo == null)
                {
                    ModelState.Remove(nameof(NewPet.Photo));
                }

                // Separate photo handling for IDNoteBookImage
                if (NewPet.IDNoteBookImage == null)
                {
                    ModelState.Remove(nameof(NewPet.IDNoteBookImage));
                }

                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                try
                {
                    // Process PetPhoto
                    if (NewPet.Photo != null)
                    {
                        string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                        string createdPetImageName = await fileService.SaveFileAsync(NewPet.Photo, allowedFileExtensions);
                        string oldPetImage = existingPet.Photo;
                        existingPet.Photo = createdPetImageName;

                        if (!string.IsNullOrEmpty(oldPetImage))
                        {
                            fileService.DeleteFile(oldPetImage);
                        }
                    }

                    // Process IDNoteBookImage
                    if (NewPet.IDNoteBookImage != null)
                    {
                        string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                        string createdIDNoteBookImageName = await fileService.SaveFileAsync(NewPet.IDNoteBookImage, allowedFileExtensions);
                        string oldIDNoteBookImage = existingPet.IDNoteBookImage;
                        existingPet.IDNoteBookImage = createdIDNoteBookImageName;

                        if (!string.IsNullOrEmpty(oldIDNoteBookImage))
                        {
                            fileService.DeleteFile(oldIDNoteBookImage);
                        }
                    }

                    existingPet.Name = NewPet.Name;
                    existingPet.AgeInMonth = NewPet.AgeInMonth;
                    existingPet.Sex = NewPet.Sex;
                    existingPet.ReadyForBreeding = NewPet.ReadyForBreeding;
                    existingPet.UserID = NewPet.UserID;
                    existingPet.Type = NewPet.Type;
                    existingPet.Other = NewPet.Other;
                    unitOfWork.petRepository.update(existingPet);
                    unitOfWork.SaveChanges();

                    if (existingPet.ReadyForBreeding == true)
                    {
                        hubContext.Clients.All.SendAsync("PetWithReadyForBreedingTrue", existingPet);
                    }
                    else if (existingPet.ReadyForBreeding == false)
                    {
                        hubContext.Clients.All.SendAsync("PetWithReadyForBreedingFalse", existingPet);
                    }
                     
                return Ok(NewPet);
                }
                catch (Exception ex)
                {
                    // Log the exception (you can replace this with your preferred logging framework)
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Internal server error");
                }
            }

            //-----------------------------------------------------------------------------------------------------------
            [HttpGet("GetAllPetswhosReadyToDate")]
            public IActionResult GetAllPetswhosReadyToDate()
            {
                List<Pet> pet = unitOfWork.petRepository.FindBy(p => p.ReadyForBreeding == true);

                if (pet == null)
                {
                    return NotFound();
                }

                List<PetGetDTO> petDTO = mapper.Map<List<PetGetDTO>>(pet);

                return Ok(petDTO);
            }


            //----------------------------------------------------------------------------------------------------

            [HttpDelete("{id}")]

            public IActionResult DeleteById(int id)
            {
                //delete PetBreed

                List<Pet_Breed> petBreeds = unitOfWork.pet_BreedRepository.FindBy(p => p.PetID == id);
                foreach (var item in petBreeds)
                {
                    unitOfWork.pet_BreedRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Vaccine-Pet

                List<Vaccine_Pet> vaccine_Pets = unitOfWork.vaccine_PetRepository.FindBy(p => p.PetID == id);
                foreach (var item in vaccine_Pets)
                {
                    unitOfWork.vaccine_PetRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Reservation For Vaccine

                List<Reservation_For_Vaccine> reservation_For_Vaccines = unitOfWork.reservation_For_VaccineRepository.FindBy(p => p.PetID == id);
                foreach (var item in reservation_For_Vaccines)
                {
                    unitOfWork.reservation_For_VaccineRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Reservation 

                List<Reservation> reservations = unitOfWork.reservationRepository.FindBy(p => p.PetID == id);
                foreach (var item in reservations)
                {
                    unitOfWork.reservationRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete RequestForBreed

                List<Request_For_Breed> request_For_Breeds = unitOfWork.request_For_BreedRepository.FindBy(p => p.PetIDSender == id || p.PetIDReceiver == id);
                foreach (var item in request_For_Breeds)
                {
                    unitOfWork.request_For_BreedRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete pet 

                Pet pet = unitOfWork.petRepository.selectbyid(id);
                if (pet.ReadyForBreeding == true)
                {
                    pet.ReadyForBreeding = false;
                    hubContext.Clients.All.SendAsync("PetWithReadyForBreedingFalse", pet);
                }
                unitOfWork.petRepository.deleteEntity(pet);
                fileService.DeleteFile(pet.Photo); //Edit By Amira
                unitOfWork.SaveChanges();
                return Ok();
            }

            [HttpGet("user/{userId}")]
            public ActionResult GetPetsByUserId(int userId)
            {
                var pets = unitOfWork.petRepository.GetEntitiesByUserId(userId, "UserID");
                if (pets == null || !pets.Any())
                {
                    return NotFound();
                }
                List<PetGetDTO> petDTO = mapper.Map<List<PetGetDTO>>(pets);
                return Ok(petDTO);
            }

            [HttpGet("{petId}/breed")]
            public ActionResult<string> GetBreedTypeByPetId(int petId)
            {
                var breedType = unitOfWork.petRepository.GetBreedTypeByPetId(petId);
                if (breedType == null)
                {
                    return NotFound();
                }
                return Ok(breedType);
            }

        [HttpPost("{petId}/pair")]
        public IActionResult PairPets(int petId, [FromBody] PairRequestDTO pairRequest)
        {
            var success = unitOfWork.petRepository.PairPets(petId, pairRequest.SelectedPetId, pairRequest.UserId);
            if (success)
            {
                return Ok(true);
            }
            var currentPet = unitOfWork.petRepository.selectbyid(petId);
            if (currentPet != null && !currentPet.ReadyForBreeding)
            {
                return Ok(false);
            }

            return BadRequest(false);
        }


        

        




    }
}