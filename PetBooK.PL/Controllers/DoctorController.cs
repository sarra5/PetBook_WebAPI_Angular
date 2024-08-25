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
    public class DoctorController : ControllerBase
    {
            UnitOfWork unitOfWork;
            IMapper mapper;
            IFileService fileService;

        public DoctorController(UnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
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
                List<Doctor> Doctors = unitOfWork.doctorRepository.selectall();
                if (Doctors == null)
                {
                    return NotFound();
                }

                List<DoctorDTO> DoctorDTO = mapper.Map<List<DoctorDTO>>(Doctors);    

                return Ok(DoctorDTO);
            }
        //--------------------------GetById-------------------------
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            Doctor doctor = unitOfWork.doctorRepository.SelectByIDInclude(id, "DoctorID", s => s.DoctorNavigation, s => s.Clinic_Doctors);

            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                DoctorDTO DoctorDTO = mapper.Map<DoctorDTO>(doctor);
                return Ok(DoctorDTO);
            }


        }

            //-----------------------------GetByLoc-----------------------
            [HttpGet("Degree")]
            public IActionResult GetByLoc(string Degree)
            {
                List<Doctor> doctors = unitOfWork.doctorRepository.FindBy(d => d.Degree == Degree);
                if (doctors == null)
                {
                    return NotFound();
                }
                else
                {
                    List<DoctorDTO> DoctorDTO = mapper.Map<List<DoctorDTO>>(doctors);   //mayaaaaa   

                    return Ok(DoctorDTO);
                }
            }



        //-------------------------------ADD------------------------------
        [HttpPost]
        public ActionResult AddDoctor(DoctorAddDTO doctoraddDTO)
        {
            if (doctoraddDTO == null)
                return BadRequest();
            else
            {
                Doctor doctor = mapper.Map<Doctor>(doctoraddDTO);
                unitOfWork.doctorRepository.add(doctor);
                unitOfWork.SaveChanges();
                return Ok(doctoraddDTO);
            }
        }


        [HttpPost("adduserfirstthenadddoctor")]
        public async Task<ActionResult> AddUserThenDoctor(int ClinicId, [FromForm]DoctorUser doctoraddDTO)
        {
            if (doctoraddDTO == null)
                return BadRequest();
            else
            {

                User existUser = unitOfWork.userRepository.FirstOrDefault(u => u.UserName == doctoraddDTO.UserName && u.Email == doctoraddDTO.Email);
                if (existUser == null) // if this user does not eist before 
                {
                    string[] allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".webp" };
                    string createdImageName = await fileService.SaveFileAsync(doctoraddDTO.Photo, allowedFileExtensions);


                    // ADD this user in user table 
                    User us = mapper.Map<User>(doctoraddDTO);
                    us.RoleID = 1;
                    us.Photo = createdImageName;
                    unitOfWork.userRepository.add(us);
                    unitOfWork.SaveChanges();
                    User user = unitOfWork.userRepository.FirstOrDefault(u => u.UserName == doctoraddDTO.UserName);


                    // then in doctor table
                    Doctor doctor = mapper.Map<Doctor>(doctoraddDTO);
                    doctor.DoctorID = user.UserID;
                    unitOfWork.doctorRepository.add(doctor);
                    unitOfWork.SaveChanges();
                }

                // Add this doctor in this clinic in clinic octor table
                User userr = unitOfWork.userRepository.FirstOrDefault(u => u.UserName == doctoraddDTO.UserName);
                Clinic_Doctor cD = new Clinic_Doctor();
                cD.ClinicID = ClinicId;
                cD.DoctorID = userr.UserID;
                unitOfWork.clinic_DoctorRepository.add(cD);
                unitOfWork.SaveChanges();
                return Ok(doctoraddDTO);
            }
        }


        //-------------------------Update------------------------------
        [HttpPut]
            public ActionResult UpdateDoctor(DoctorDTO doctorDTO)
            {
                if (doctorDTO == null)
                    return BadRequest();
                else
                {

                    Doctor doctor = mapper.Map<Doctor>(doctorDTO);
                    User us=unitOfWork.userRepository.selectbyid(doctor.DoctorID);
                    us.Name = doctorDTO.Name;
                    unitOfWork.userRepository.update(us);
                    unitOfWork.doctorRepository.update(doctor);
                    unitOfWork.SaveChanges();
                    return Ok(doctorDTO);
                }
            }

        //--------------------------------Delete----------------------
        [HttpDelete("id")]
        public IActionResult DeleteDoctorr(int id)
        {
            List<Clinic_Doctor> doctors = unitOfWork.clinic_DoctorRepository.FindBy(p => p.DoctorID == id);
            foreach (var item in doctors)
            {
                unitOfWork.clinic_DoctorRepository.deleteEntity(item);

            }
            unitOfWork.SaveChanges();

            Doctor doctor = unitOfWork.doctorRepository.FirstOrDefault(p => p.DoctorID == id);
            unitOfWork.doctorRepository.deleteEntity(doctor);

            unitOfWork.SaveChanges();
            return Ok("Doctor Has been deleted Successfully deleted");

        }


        
        [HttpGet("{clinicId}/doctors")]
        public IActionResult GetDoctorsByClinicId(int clinicId)
        {
            try
            {
                // Fetch doctors along with the related User entity to get the Name
                var clinicDoctors = unitOfWork.clinic_DoctorRepository
                    .Where(cd => cd.ClinicID == clinicId)
                    .Include(cd => cd.doctor.DoctorNavigation) // Ensure related User entity is included
                    .Select(cd => cd.doctor)
                    .ToList();

                if (clinicDoctors.Count == 0)
                {
                    return NotFound("No doctors found for the given clinic ID.");
                }

                // Map to DTOs, including the Name from the related User entity
                var doctorDTOs = clinicDoctors.Select(doctor => new DoctorDTO
                {
                    DoctorID = doctor.DoctorID,
                    Degree = doctor.Degree,
                    Name = doctor.DoctorNavigation?.Name,
                    HiringDate = doctor.HiringDate
                }).ToList();

                return Ok(doctorDTOs);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }

}

