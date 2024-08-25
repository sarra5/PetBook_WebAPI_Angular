using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineClinicController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public VaccineClinicController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAllVaccineClinic()
        {
            try
            {
                List<Vaccine_Clinic> vaccineClinics = unit.vaccine_ClinicRepository.FindBy(s => s.Quantity >= 1);
                if (vaccineClinics == null || !vaccineClinics.Any())
                    return NotFound("No Data");

                List<VaccineClinicDTO> vaccineClinicDTOs = mapper.Map<List<VaccineClinicDTO>>(vaccineClinics);
                return Ok(vaccineClinicDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving reservations.");
            }
        }

        [HttpGet("clinic/{ClinicId}")]
        public ActionResult GetVaccineClinictByClinicId(int ClinicId)
        {
            try
            {
                List<Vaccine_Clinic> vaccineClinics = unit.vaccine_ClinicRepository.FindBy(p => p.ClinicID == ClinicId);

                if (vaccineClinics == null || !vaccineClinics.Any())
                    return NotFound($"Vaccine Clinic with Clinic ID {ClinicId} not found.");

                List<VaccineClinicDTO> vaccineClinicDTOs = mapper.Map<List<VaccineClinicDTO>>(vaccineClinics);
                return Ok(vaccineClinicDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("vaccine/{VaccineId}")]
        public ActionResult GetVaccineClinicByVaccineId(int VaccineId)
        {
            try
            {
                List<Vaccine_Clinic> vaccineClinics = unit.vaccine_ClinicRepository.FindBy(p => p.VaccineID == VaccineId);

                if (vaccineClinics == null || !vaccineClinics.Any())
                    return NotFound($"Vaccine Clinic with Vaccine ID {VaccineId} not found.");

                List<VaccineClinicDTO> vaccineClinicDTOs = mapper.Map<List<VaccineClinicDTO>>(vaccineClinics);
                return Ok(vaccineClinicDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("vaccineClinicInclude/{VaccineId}")]
        public ActionResult GetVaccineClinicByVaccineIdImclude(int VaccineId)
        {
            try
            {
                List<Vaccine_Clinic> vaccineClinics = unit.vaccine_ClinicRepository.FindByInclude(p => p.VaccineID == VaccineId, p => p.Clinic);

                if (vaccineClinics == null || !vaccineClinics.Any())
                    return NotFound($"Vaccine Clinic with Vaccine ID {VaccineId} not found.");

                List<VaccineClinicInclude> vaccineClinicDTOs = mapper.Map<List<VaccineClinicInclude>>(vaccineClinics);
                return Ok(vaccineClinicDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }






        [HttpGet("{VaccineId:int}/{ClinicID:int}")]
        public ActionResult GetVaccineClinicByVaccineIdAndClinicId(int VaccineId, int ClinicID)
        {
            try
            {
                var vaccineClinic = unit.vaccine_ClinicRepository.FirstOrDefault(c => c.VaccineID == VaccineId && c.ClinicID == ClinicID);

                if (vaccineClinic == null)
                    return NotFound($"Vaccine Clinic with Vaccine ID {VaccineId} and Clinic ID {ClinicID} not found.");

                var vaccineClinicDTO = mapper.Map<VaccineClinicDTO>(vaccineClinic);
                return Ok(vaccineClinicDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpPost]
        public ActionResult AddVaccinePClinic(VaccineClinicDTO vaccineClinicDTO)
        {
            try
            {
                if (vaccineClinicDTO == null)
                    return BadRequest("Vaccine Clinic data is null");

                var existingVaccineClinic = unit.vaccine_ClinicRepository
                .FirstOrDefault(c => c.VaccineID == vaccineClinicDTO.VaccineID && c.ClinicID == vaccineClinicDTO.ClinicID);

                if (existingVaccineClinic != null)
                    return BadRequest("Vaccine Clinic already exists");

                Vaccine_Clinic vaccineClinic = mapper.Map<Vaccine_Clinic>(vaccineClinicDTO);
                unit.vaccine_ClinicRepository.add(vaccineClinic);
                unit.SaveChanges();
                return Ok("Successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public ActionResult UpdateVaccineClinic(VaccineClinicDTO vaccineClinicDTO)
        {
            try
            {
                if (vaccineClinicDTO == null)
                    return BadRequest("Vaccine Clinic data is null");

                var existingVaccineClinic = unit.vaccine_ClinicRepository
                .FirstOrDefault(c => c.VaccineID == vaccineClinicDTO.VaccineID && c.ClinicID == vaccineClinicDTO.ClinicID);

                if (existingVaccineClinic == null)
                    return NotFound("Vaccine Clinic not found");

                mapper.Map(vaccineClinicDTO, existingVaccineClinic);

                unit.vaccine_ClinicRepository.update(existingVaccineClinic);
                unit.SaveChanges();
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("decreasetheNumberOfVaccine")]
        public ActionResult UpdateVaccineClinicQuantity(int VaccineId, int ClinicID)
        {
            try
            {
                var existingVaccineClinic = unit.vaccine_ClinicRepository
                .FirstOrDefault(c => c.VaccineID == VaccineId && c.ClinicID == ClinicID);

                if (existingVaccineClinic == null)
                    return NotFound("Vaccine Clinic not found");

                existingVaccineClinic.Quantity--;

                unit.vaccine_ClinicRepository.update(existingVaccineClinic);
                unit.SaveChanges();
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }




        [HttpDelete("{VaccineId}/{ClinicID}")]
        public ActionResult DeleteVaccineClinic(int VaccineId, int ClinicID)
        {
            
                if (ClinicID == null || VaccineId == null)
                    return BadRequest("Vaccine Clinic data is null");

                var vaccineClinic = unit.vaccine_ClinicRepository.FirstOrDefault(c => c.VaccineID == VaccineId && c.ClinicID == ClinicID);

                if (vaccineClinic == null)
                    return NotFound("No Data to delete");

                unit.vaccine_ClinicRepository.deleteEntity(vaccineClinic);
                unit.SaveChanges();
                return Ok();
            
            
        }

        [HttpGet("clinicc/{ClinicId}")]
        public ActionResult GetVaccineClinictbyClinicId(int ClinicId)
        {
            try
            {
                var vaccineClinics = unit.vaccine_ClinicRepository.FindByIdInclude(ClinicId, "ClinicID", vc => vc.Vaccine);

                if (vaccineClinics == null || !vaccineClinics.Any())
                    return NotFound($"Vaccine Clinic with Clinic ID {ClinicId} not found.");

                var vaccineCliniccDTOs = vaccineClinics.Select(vc => new VaccineCliniccDTO
                {
                    VaccineID = vc.VaccineID,
                    ClinicID = vc.ClinicID,
                    Name = vc.Vaccine.Name,
                    Price = vc.Price,
                    Quantity = vc.Quantity,
                    Description = vc.Vaccine.Description
                }).ToList();

                return Ok(vaccineCliniccDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the vaccine clinics.");
            }
        }


        [HttpPut("updateVaccine")]
        public IActionResult UpdateVaccine([FromBody] VaccineCliniccDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vaccine = unit.vaccineRepository.SelectByIDInclude(dto.VaccineID, "VaccineID", v => v.Vaccine_Clinics);



            if (vaccine == null)
            {
                return NotFound($"Vaccine with ID {dto.VaccineID} not found.");
            }
            vaccine.Name = dto.Name; // Update Vaccine entity
            vaccine.Description = dto.Description;


            var vaccineClinic = vaccine.Vaccine_Clinics.FirstOrDefault(vc => vc.ClinicID == dto.ClinicID);   // Retrieve the existing Vaccine_Clinic entity

            if (vaccineClinic == null)
            {
                return NotFound($"Vaccine_Clinic with VaccineID {dto.VaccineID} and ClinicID {dto.ClinicID} not found.");
            }
            vaccineClinic.Price = dto.Price;// Update Vaccine_Clinic entity
            vaccineClinic.Quantity = dto.Quantity;
            unit.SaveChanges();

            return Ok();
        }

        [HttpPost("addVaccine")]
        public IActionResult AddVaccine([FromBody] VaccineAddDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vaccine = new Vaccine
            {
                Name = dto.Name,
                Description = dto.Description
            };
            var vaccineClinic = new Vaccine_Clinic
            {
                ClinicID = dto.ClinicID,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Vaccine = vaccine
            };
            unit.vaccineRepository.add(vaccine);
            unit.vaccine_ClinicRepository.add(vaccineClinic);
            unit.SaveChanges();
            return Ok();
        }

    }
}
