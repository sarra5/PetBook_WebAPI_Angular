using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using PetBooK.DAL.Services;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UnitOfWork unitOfWork;
        IMapper mapper;
        IFileService fileService;

        public UserController(UnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileService = fileService;
        }
        //------------------------------------------Get----------------------------------------
        //------------------GetAll-----------
        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> Users = unitOfWork.userRepository.selectall();
            if (Users == null)
            {
                return NotFound();
            }

            List<UserDTO> UserDTO = mapper.Map<List<UserDTO>>(Users);   //mayaaaaa   

            return Ok(UserDTO);
        }
        //--------------------------GetById-------------------------
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            User User = unitOfWork.userRepository.selectbyid(id);
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                UserDTO userDTO = mapper.Map<UserDTO>(User);
                return Ok(userDTO);
            }
        }

        //-----------------------------GetByLoc-----------------------
        [HttpGet("Loc")]
        public IActionResult GetByLoc(string Loc)
        {
            List<User> users = unitOfWork.userRepository.FindBy(l => l.Location == Loc);
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                List<UserDTO> UserDTO = mapper.Map<List<UserDTO>>(users);   //mayaaaaa   

                return Ok(UserDTO);
            }
        }



        //-------------------------------ADD------------------------------//Edit By Rana & Amira
        [HttpPost]
        public async Task<ActionResult> AddUserAsync(UserAddDTO useraddDTO)
        {
            string[] allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (useraddDTO == null)
            {
                return BadRequest();
            }

            string createdImageName = await fileService.SaveFileAsync(useraddDTO.Photo, allowedFileExtensions);

            User user = new User
            {
                Name = useraddDTO.Name,
                Location = useraddDTO.Location,
                Email = useraddDTO.Email,
                Password = useraddDTO.Password,
                Photo = createdImageName,
                UserName = useraddDTO.Name,
                Phone = useraddDTO.Phone,
                Age = useraddDTO.Age,
                Sex = useraddDTO.Sex,
                RoleID = useraddDTO.RoleID,
            };

            unitOfWork.userRepository.add(user);
            unitOfWork.SaveChanges(); 

            switch (useraddDTO.RoleID)
            {


                case 1: // Doctor
                    Doctor doctor = new Doctor
                    {
                        DoctorID = user.UserID,
                    };
                    unitOfWork.doctorRepository.add(doctor);
                    break;
              
                case 2: //Client
                    Client client = new Client
                    {
                        ClientID = user.UserID,
                    };
                    unitOfWork.clientRepository.add(client);
                    break;

                case 3: // Secretary

                    Secretary secretary = new Secretary
                    {
                        SecretaryID = user.UserID,
                    };
                    unitOfWork.secretaryRepository.add(secretary);
                    break;

                default:
                    return BadRequest("Invalid RoleID");
            }

            unitOfWork.SaveChanges();

            return Ok(useraddDTO);
        }

        //-------------------------Update------------------------------//Edit By Amira
        [HttpPut("id")]
        public async Task<ActionResult> UpdateUser(int id, [FromForm] UserUpdateDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            var existingUser = unitOfWork.userRepository.selectbyid(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (userDTO.Photo == null)
            {
                ModelState.Remove(nameof(userDTO.Photo));
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (userDTO.Photo != null)
            {
                string[] allowedFileExtentions = { ".jpg", ".jpeg", ".png", ".webp" };
                string createdImageName = await fileService.SaveFileAsync(userDTO.Photo, allowedFileExtentions);
                string oldImage = existingUser.Photo;
                existingUser.Photo = createdImageName;

                if (!string.IsNullOrEmpty(oldImage))
                {
                    fileService.DeleteFile(oldImage);
                }
            }

            existingUser.UserID = userDTO.Id;
            existingUser.Name = userDTO.Name;
            existingUser.UserName = userDTO.UserName;
            existingUser.Email = userDTO.Email;
            existingUser.Password = userDTO.Password;
            existingUser.Age = userDTO.Age;
            existingUser.Sex = userDTO.Sex;
            existingUser.Location = userDTO.Location;
            existingUser.Phone = userDTO.Phone;
            existingUser.RoleID = userDTO.RoleID;

            unitOfWork.userRepository.update(existingUser);
            unitOfWork.SaveChanges();

            return Ok(userDTO);
        }

        //--------------------------------Delete----------------------

        //--------------------------------delete relations------------------------
        [HttpDelete("id")]
        public IActionResult DeleteUser(int id)

        {
            //before deleting user ,flow secretary=>then user

            //delete secertary

            List<Secretary> secertarys = unitOfWork.secretaryRepository.FindBy(p => p.SecretaryID == id);
            foreach (var item in secertarys)
            {
                unitOfWork.secretaryRepository.deleteEntity(item);
            }
            unitOfWork.SaveChanges();
            //----------------------------delete doctor----------------------
            //delete clinic_doctor first from clinic_doctor before deleting doctor himselfs

            List<Clinic_Doctor> doctors = unitOfWork.clinic_DoctorRepository.FindBy(p => p.DoctorID == id);
            foreach (var item in doctors)
            {
                unitOfWork.clinic_DoctorRepository.deleteEntity(item);

            }
            unitOfWork.SaveChanges();

            Doctor doctorss = unitOfWork.doctorRepository.FirstOrDefault(p => p.DoctorID == id);
              unitOfWork.doctorRepository.deleteEntity(doctorss);
            
            unitOfWork.SaveChanges();


            //-------------------delete pet for deleting client-------------------------



            //before deleting client, flow petbreed, vaccinepet , reservation , reservation vaccine ==> then delete pet
            //delete client

            //delete PetBreed
            List<Pet> pets = unitOfWork.petRepository.FindBy(p => p.UserID == id);
            foreach (var items in pets)
            {
                List<Pet_Breed> petBreeds = unitOfWork.pet_BreedRepository.FindBy(p => p.PetID == items.PetID);
                foreach (var item in petBreeds)
                {
                    unitOfWork.pet_BreedRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Vaccine-Pet
                List<Vaccine_Pet> vaccine_Pets = unitOfWork.vaccine_PetRepository.FindBy(p => p.PetID == items.PetID);
                foreach (var item in vaccine_Pets)
                {
                    unitOfWork.vaccine_PetRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Reservation For Vaccine

                List<Reservation_For_Vaccine> reservation_For_Vaccines = unitOfWork.reservation_For_VaccineRepository.FindBy(p => p.PetID == items.PetID);
                foreach (var item in reservation_For_Vaccines)
                {
                    unitOfWork.reservation_For_VaccineRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete Reservation 

                List<Reservation> reservations = unitOfWork.reservationRepository.FindBy(p => p.PetID == items.PetID);
                foreach (var item in reservations)
                {
                    unitOfWork.reservationRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete RequestForBreed

                List<Request_For_Breed> request_For_Breeds = unitOfWork.request_For_BreedRepository.FindBy(p => p.PetIDSender == items.PetID || p.PetIDReceiver == items.PetID);
                foreach (var item in request_For_Breeds)
                {
                    unitOfWork.request_For_BreedRepository.deleteEntity(item);
                }
                unitOfWork.SaveChanges();

                //delete pet 

                Pet pet = unitOfWork.petRepository.selectbyid(items.PetID);
                unitOfWork.petRepository.deleteEntity(pet);
                unitOfWork.SaveChanges();
            }
            //andddd -------------------------delete clienttt-------------------
            Client client= unitOfWork.clientRepository.FirstOrDefault(p => p.ClientID == id);     
                unitOfWork.clientRepository.deleteEntity(client);
            unitOfWork.SaveChanges();




            //-------------------------------delete user--------------------------------------

            User user = unitOfWork.userRepository.selectbyid(id);
            unitOfWork.userRepository.deleteEntity(user);
            fileService.DeleteFile(user.Photo);
            unitOfWork.SaveChanges();
            return Ok();



        }

        [HttpGet("{userId}/Pets")]
        public IActionResult GetUserPets(int userId)
        {
            try
            {
                var pets = unitOfWork.petRepository.GetPetsByUser(userId);
                List<PetGetDTO> petDTO = mapper.Map<List<PetGetDTO>>(pets);
                return Ok(petDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching user pets: {ex.Message}");
            }
        }

    }

}

