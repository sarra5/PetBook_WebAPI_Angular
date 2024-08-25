using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicLocationController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public ClinicLocationController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }
        [HttpGet("search/{query}")]
        public IActionResult SearchClinicByNameOrLocation(string query)
        {
            var clinics = unit.clinicRepository
                .FindByInclude(
                    c => c.Name == query || c.Clinic_Locations.Any(cl => cl.Location == query),
                    c => c.Clinic_Phones,
                    c => c.Clinic_Locations
                );

            if (clinics == null || !clinics.Any())
            {
                return NotFound();
            }

            var clinicDTOs = clinics.Select(c => new ClinicDTO
            {
                ClinicID = c.ClinicID,
                Name = c.Name,
                Rate = c.Rate,
                BankAccount = c.BankAccount,
                PhoneNumbers = c.Clinic_Phones.Select(cp => cp.Phone).ToList(),
                Locations = c.Clinic_Locations.Select(cp => cp.Location).ToList()
            }).ToList();

            return Ok(clinicDTOs);
        }


        [HttpGet]
        public ActionResult GetAllClinicLocation()
        {
            try
            {
                List<Clinic_Location> clinicLocations = unit.clinic_LocationRepository.selectall();
                if (clinicLocations == null || !clinicLocations.Any())
                    return NotFound("No Data");

                var clinicLocationDTOs = mapper.Map<List<ClinicLocationDTO>>(clinicLocations);


                return Ok(clinicLocationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving clinic locations.");
            }
        }



        [HttpGet("location/{ClinicId}")]
        public ActionResult GetClinicLocationsByClinicId(int ClinicId)
        {
            try
            {
                List<Clinic_Location> clinicLocations = unit.clinic_LocationRepository.FindBy(cl => cl.ClinicID == ClinicId).ToList();

                if (clinicLocations == null || !clinicLocations.Any())
                    return NotFound($"Locations for Clinic ID {ClinicId} not found.");

                var clinicLocationDTOs = mapper.Map<List<ClinicLocationDTO>>(clinicLocations);

                return Ok(clinicLocationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the clinic locations.");
            }


        }


        [HttpGet("locationinclude/{ClinicId:int}")]
        public ActionResult GetClinicLocationsByClinicincludeId(int ClinicId)
        {
            try
            {
                List<Clinic_Location> clinicLocations = unit.clinic_LocationRepository.FindByInclude(cl => cl.ClinicID == ClinicId,cl=>cl.Clinic);

                if (clinicLocations == null || !clinicLocations.Any())
                    return NotFound($"Locations for Clinic ID {ClinicId} not found.");

                List< ClinicLocationInclude> clinicLocationDTOs = mapper.Map<List<ClinicLocationInclude>>(clinicLocations);

                return Ok(clinicLocationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the clinic locations.");
            }
        }





        [HttpGet("{ClinicID:int}/{Location}")]
        public ActionResult GetClinicLocationByClinicIdAndLocation(int ClinicID, string Location)
        {
            try
            {
                var clinicLocation = unit.clinic_LocationRepository.FirstOrDefault(cl => cl.ClinicID == ClinicID && cl.Location == Location);

                if (clinicLocation == null)
                    return NotFound($"Clinic Location with Clinic ID {ClinicID} and Location {Location} not found.");

                var clinicLocationDTO = mapper.Map<ClinicLocationDTO>(clinicLocation);
                return Ok(clinicLocationDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the clinic location.");
            }
        }

        [HttpPost]
        public ActionResult AddLocation(ClinicLocationDTO locationDTO)
        {
            try
            {
                if (locationDTO == null)
                    return BadRequest("Location data is null");

                var existingLocation = unit.clinic_LocationRepository
                    .FirstOrDefault(l => l.ClinicID == locationDTO.ClinicID && l.Location == locationDTO.Location);

                if (existingLocation != null)
                    return BadRequest("Location already exists");


                var location = new Clinic_Location
                {
                    ClinicID = locationDTO.ClinicID,
                    Location = locationDTO.Location,

                };

                
                unit.clinic_LocationRepository.add(location);
                unit.SaveChanges();

                return Ok("Location successfully added");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public ActionResult UpdateLocation( ClinicLocationUpdateDTO locationDTO)
        {
            try
            {
                if (locationDTO == null)
                    return BadRequest("Location data is null");

                var existingLocation = unit.clinic_LocationRepository
                    .FirstOrDefault(l => l.ClinicID == locationDTO.ClinicID && l.Location == locationDTO.OldLocation);

                if (existingLocation == null)
                    return NotFound("Location not found");


                mapper.Map(existingLocation, locationDTO);


                unit.clinic_LocationRepository.update(existingLocation);
                unit.SaveChanges();

                return Ok("Location successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpDelete]
        public ActionResult DeleteLocation(int ClinicID, string Location)
        {
            try
            {
                if (ClinicID == null || Location == null)
                    return BadRequest("Location data is null");

                var location = unit.clinic_LocationRepository.FirstOrDefault(l => l.ClinicID == ClinicID && l.Location == Location);

                if (location == null)
                    return NotFound("Location not found");

                unit.clinic_LocationRepository.deleteEntity(location);
                unit.SaveChanges();

                return Ok("Location successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }



    }
}
