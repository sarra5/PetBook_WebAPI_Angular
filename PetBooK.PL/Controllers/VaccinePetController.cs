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
    public class VaccinePetController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public VaccinePetController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAllVaccinePet()
        {
            try
            {
                List<Vaccine_Pet> vaccinePets = unit.vaccine_PetRepository.SelectAll();
                if (vaccinePets == null || !vaccinePets.Any())
                    return NotFound("No Data");

                List<VaccinePetDTO> vaccinePetDTOs = mapper.Map<List<VaccinePetDTO>>(vaccinePets);
                return Ok(vaccinePetDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving reservations.");
            }
        }

        [HttpGet("pet/{PetId}")]
        public ActionResult GetVaccinePetByPetId(int PetId)
        {
            try
            {
                List<Vaccine_Pet> vaccinePets = unit.vaccine_PetRepository.FindBy(p => p.PetID == PetId);

                if (vaccinePets == null || !vaccinePets.Any())
                    return NotFound($"Vaccine Pet with Pet ID {PetId} not found.");

                List<VaccinePetDTO> vaccinePetDTOs = mapper.Map<List<VaccinePetDTO>>(vaccinePets);
                return Ok(vaccinePetDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("vaccine/{VaccineId}")]
        public ActionResult GetVaccinePetByVaccineId(int VaccineId)
        {
            try
            {
                List<Vaccine_Pet> vaccinePets = unit.vaccine_PetRepository.FindBy(p => p.VaccineID == VaccineId);

                if (vaccinePets == null || !vaccinePets.Any())
                    return NotFound($"Vaccine Pet with Vaccine ID {VaccineId} not found.");

                List<VaccinePetDTO> vaccinePetDTOs = mapper.Map<List<VaccinePetDTO>>(vaccinePets);
                return Ok(vaccinePetDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("{VaccineId:int}/{PetID:int}")]
        public ActionResult GetVaccinePetByVaccineIdAndPetId(int VaccineId, int PetID)
        {
            try
            {
                var vaccinePet = unit.vaccine_PetRepository.FirstOrDefault(c => c.VaccineID == VaccineId && c.PetID == PetID);

                if (vaccinePet == null)
                    return NotFound($"Vaccine Pet with Vaccine ID {VaccineId} and Pet ID {PetID} not found.");

                var vaccinePetDTO = mapper.Map<VaccinePetDTO>(vaccinePet);
                return Ok(vaccinePetDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpPost]
        public ActionResult AddVaccinePet(VaccinePetDTO vaccinePetDTO)
        {
            try
            {
                if (vaccinePetDTO == null)
                    return BadRequest("Vaccine Pet data is null");

                var existingVaccinePet = unit.vaccine_PetRepository
                .FirstOrDefault(c => c.VaccineID == vaccinePetDTO.VaccineID && c.PetID == vaccinePetDTO.PetID);

                if (existingVaccinePet != null)
                    return BadRequest("Vaccine Pet already exists");

                Vaccine_Pet vaccinePet = mapper.Map<Vaccine_Pet>(vaccinePetDTO);
                unit.vaccine_PetRepository.add(vaccinePet);
                unit.SaveChanges();
                return Ok("Successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public ActionResult UpdateVaccinePet(VaccinePetDTO vaccinePetDTO)
        {
            try
            {
                if (vaccinePetDTO == null)
                    return BadRequest("Vaccine Pet data is null");

                var existingVaccinePet = unit.vaccine_PetRepository
                .FirstOrDefault(c => c.VaccineID == vaccinePetDTO.VaccineID && c.PetID == vaccinePetDTO.PetID);

                if (existingVaccinePet == null)
                    return NotFound("Vaccine Pet not found");

                mapper.Map(vaccinePetDTO, existingVaccinePet);

                unit.vaccine_PetRepository.update(existingVaccinePet);
                unit.SaveChanges();
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete]
        public ActionResult DeleteVaccinePet(int VaccineId, int PetID)
        {
            try
            {
                if (PetID == null || VaccineId == null)
                    return BadRequest("Vaccine Pet data is null");

                var vaccinePet = unit.vaccine_PetRepository.FirstOrDefault(c => c.VaccineID == VaccineId && c.PetID == PetID);

                if (vaccinePet == null)
                    return NotFound("No Data to delete");

                unit.vaccine_PetRepository.deleteEntity(vaccinePet);
                unit.SaveChanges();
                return Ok("Successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
