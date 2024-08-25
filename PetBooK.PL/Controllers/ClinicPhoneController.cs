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
    public class ClinicPhoneController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;
        public ClinicPhoneController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult GetAllClinicPhone()
        {
            try
            {
                List<Clinic_Phone> clinicPhones = unit.clinic_PhoneRepository.SelectAll();
                if (clinicPhones == null || clinicPhones.Count == 0)
                    return NotFound("No clinic phones found");

                
                var clinicPhoneDTOs = mapper.Map<List<ClinicPhoneDTO>>(clinicPhones);


                return Ok(clinicPhoneDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving clinic phones.");
            }
        }

        [HttpGet("clinic/{ClinicId}")]
        public ActionResult GetClinicPhonesByClinicId(int ClinicId)
        {
            try
            {
                List<Clinic_Phone> clinicPhones = unit.clinic_PhoneRepository.FindBy(cp => cp.ClinicID == ClinicId).ToList();

                if (clinicPhones == null || clinicPhones.Count == 0)
                    return NotFound($"Phones for Clinic ID {ClinicId} not found.");

                var clinicPhoneDTOs = mapper.Map<List<ClinicPhoneDTO>>(clinicPhones);

                return Ok(clinicPhoneDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the clinic phones.");
            }
        }

        [HttpGet("{ClinicID:int}/{PhoneNumber}")]
        public ActionResult GetClinicPhoneByClinicIdAndPhoneNumber(int ClinicID, string PhoneNumber)
        {
            try
            {
                var clinicPhone = unit.clinic_PhoneRepository.FirstOrDefault(cp => cp.ClinicID == ClinicID && cp.Phone == PhoneNumber);

                if (clinicPhone == null)
                    return NotFound($"Clinic Phone with Clinic ID {ClinicID} and Phone Number {PhoneNumber} not found.");

                var clinicPhoneDTO = mapper.Map<ClinicPhoneDTO>(clinicPhone);
                return Ok(clinicPhoneDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the clinic phone.");
            }
        }

        [HttpPost]
        public ActionResult AddPhone(ClinicPhoneDTO phoneDTO)
        {
            try
            {
                if (phoneDTO == null)
                    return BadRequest("Phone data is null");

                var existingPhone = unit.clinic_PhoneRepository.FirstOrDefault(p => p.ClinicID == phoneDTO.ClinicID && p.Phone == phoneDTO.PhoneNumber);

                if (existingPhone != null)
                    return BadRequest("Phone already exists");

                var phone = new Clinic_Phone
                {
                    ClinicID = phoneDTO.ClinicID,
                    Phone = phoneDTO.PhoneNumber,
                };

                unit.clinic_PhoneRepository.add(phone);
                unit.SaveChanges();

                return Ok("Phone successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public ActionResult UpdatePhone(ClinicPhoneUpdateDTO phoneDTO)
        {
            try                                                             
            {
                var existingPhone = unit.clinic_PhoneRepository.FirstOrDefault(p => p.ClinicID == phoneDTO.ClinicID && p.Phone == phoneDTO.OldPhone);

                if (existingPhone == null)
                    return NotFound("Phone not found");


                mapper.Map(existingPhone, phoneDTO);

                unit.clinic_PhoneRepository.update(existingPhone);
                unit.SaveChanges();

                return Ok("Phone successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Error: {ex.Message}");
            }
        }

        [HttpDelete("{ClinicID:int}/{PhoneNumber}")]
        public ActionResult DeletePhone(int ClinicID, string PhoneNumber)
        {
            try
            {
                if (ClinicID == null || PhoneNumber == null)
                    return BadRequest("Phone data is null");

                var phone = unit.clinic_PhoneRepository.FirstOrDefault(p => p.ClinicID == ClinicID && p.Phone == PhoneNumber);

                if (phone == null)
                    return NotFound("Phone not found");

                unit.clinic_PhoneRepository.deleteEntity(phone);
                unit.SaveChanges();

                return Ok("Phone successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Error: {ex.Message}");
            }
        }
    }
}
