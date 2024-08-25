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
    public class ClinicController : ControllerBase
    {

        UnitOfWork unitOfWork;
        IMapper mapper;
        public ClinicController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //------------------------------------------Get----------------------------------------
        //------------------GetAll-----------
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Clinic> clinics = unitOfWork.clinicRepository.selectall();
            if (clinics == null)
            {
                return NotFound();
            }

            List<ClinicccDTO> clinicDTO = mapper.Map<List<ClinicccDTO>>(clinics);   //mayaaaaa   

            return Ok(clinicDTO);
        }
        //--------------------------GetById-------------------------
        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            Clinic clinic = unitOfWork.clinicRepository.selectbyid(id);
            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                ClinicccDTO clinicDTO = mapper.Map<ClinicccDTO>(clinic);
                return Ok(clinicDTO);
            }
        }

        //-----------------------------GetByName-----------------------
        [HttpGet("Name")]
        public IActionResult GetByName(string name)
        {
            List<Clinic> clinics = unitOfWork.clinicRepository.FindBy(l => l.Name == name);
            if (clinics == null || clinics.Count == 0 )
            {
                return NotFound(new { message = "Not Found" });
            }
            else
            {
                List<ClinicccDTO> clinicDTO = mapper.Map<List<ClinicccDTO>>(clinics);   //mayaaaaa   

                return Ok(clinicDTO);
            }
        }

        //-----------------------------GetByRate-----------------------
        [HttpGet("Rate")]
        public IActionResult GetByRate(int rate)
        {
            List<Clinic> clinics = unitOfWork.clinicRepository.FindBy(l => l.Rate == rate);
            if (clinics == null)
            {
                return NotFound();
            }
            else
            {
                List<ClinicccDTO> clinicDTO = mapper.Map<List<ClinicccDTO>>(clinics);

                return Ok(clinicDTO);
            }
        }


        //-------------------------------ADD------------------------------
        [HttpPost]
        public ActionResult AddClinic(ClinicAddDTO clinicDTO)
        {
            if (clinicDTO == null)
                return BadRequest();
            else
            {
                Clinic clinic = mapper.Map<Clinic>(clinicDTO);
                unitOfWork.clinicRepository.add(clinic);
                unitOfWork.SaveChanges();
                return Ok(clinicDTO);
            }
        }

        //-------------------------Update------------------------------
        [HttpPut]
        public ActionResult UpdateClinic(ClinicccDTO clinicDTO)
        {
            if (clinicDTO == null)
                return BadRequest();
            else
            {

                Clinic clinic = mapper.Map<Clinic>(clinicDTO);
                unitOfWork.clinicRepository.update(clinic);
                unitOfWork.SaveChanges();
                return Ok(clinicDTO);
            }
        }



        //--------------------------------Delete----------------------
        [HttpDelete]
        public IActionResult DeleteClinic(int id)
        { //--delete_secr
               Secretary secr = unitOfWork.secretaryRepository.FirstOrDefault(p => p.ClinicID == id);
                unitOfWork.secretaryRepository.deleteEntity(secr);
                unitOfWork.SaveChanges();




            //-------------delete clinic_doc-----------

            List<Clinic_Doctor> clinics = unitOfWork.clinic_DoctorRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in clinics)
            {
                unitOfWork.clinic_DoctorRepository.deleteEntity(items);
                }
                unitOfWork.SaveChanges();

            //---------delete clinic_phone------------------------

            List<Clinic_Phone> phones = unitOfWork.clinic_PhoneRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in phones)
            {
                unitOfWork.clinic_PhoneRepository.deleteEntity(items);
            }
            unitOfWork.SaveChanges();
            //---vaccine_clinic------------

            List<Vaccine_Clinic> VC = unitOfWork.vaccine_ClinicRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in VC)
            {
                unitOfWork.vaccine_ClinicRepository.deleteEntity(items);
            }
            unitOfWork.SaveChanges();

            //---------------------delete clinicloc--------------------
            List < Clinic_Location> loc = unitOfWork.clinic_LocationRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in phones)
            {
                unitOfWork.clinic_PhoneRepository.deleteEntity(items);
            }
            unitOfWork.SaveChanges();

            //-----------------------delete reservation for vaccine ---------------


            List<Reservation_For_Vaccine> res = unitOfWork.reservation_For_VaccineRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in res)
            {
                unitOfWork.reservation_For_VaccineRepository.deleteEntity(items);
            }
            unitOfWork.SaveChanges();

            //--------resevation------------------

            List<Reservation> resv = unitOfWork.reservationRepository.FindBy(p => p.ClinicID == id);
            foreach (var items in res)
            {
                unitOfWork.reservation_For_VaccineRepository.deleteEntity(items);
            }
            unitOfWork.SaveChanges();

           
            //-------------clinic------------------


                unitOfWork.clinicRepository.delete(id);
                unitOfWork.SaveChanges();
                return Ok("clinic has Successfully been deleted");
        }

        //--------------------------------GetClinicByIt'sLocations----------------------
        [HttpGet("Clinics")]
        public IActionResult GetAllClinicsWithLocation(int pageNumber = 1, int pageSize = 4)
        {
            try
            {
                List<Clinic> clinics = unitOfWork.clinicRepository.SelectAllIncludePagination(s => s.Clinic_Locations);
                List<ClinicByLocationsDTO> clinicByLocation = new List<ClinicByLocationsDTO>();
                foreach (var clinic in clinics)
                {
                    foreach (var location in clinic.Clinic_Locations)
                    {
                        var Loc = new ClinicByLocationsDTO
                        {
                            ClinicID = clinic.ClinicID,
                            Name = clinic.Name,
                            Location = location.Location,
                            Rate = clinic.Rate,
                            BankAccount = clinic.BankAccount
                        };
                        clinicByLocation.Add(Loc);
                    }
                }
                int Total = clinicByLocation.Count();
                List<ClinicByLocationsDTO> clinicByLocation2 = clinicByLocation.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var response = new
                {
                    Data = clinicByLocation2,
                    AllData = clinicByLocation,
                    TotalItems = Total
                };
                return Ok(response);
            }
            catch
            {
                return StatusCode(500, "error while retrievong data");
            }
        }

    }

}

