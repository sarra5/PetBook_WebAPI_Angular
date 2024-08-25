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
    public class VaccineController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;
        public VaccineController(UnitOfWork _unit, IMapper _mapper)
        {
            unit = _unit;
            mapper = _mapper;
        }

        [HttpGet]
        //public IActionResult GetAllVaccines()
        //{
        //    try
        //    {
        //        List<Vaccine> Vaccines = unit.vaccineRepository.selectall();
        //        List<VaccineDTO> VaccinesDTO = mapper.Map<List<VaccineDTO>>(Vaccines);
        //        if(VaccinesDTO.Count > 0)
        //        {
        //            return Ok(VaccinesDTO);
        //        }
        //        else
        //        {
        //            return NotFound("There's no vaccines");
        //        }

        //    }
        //    catch
        //    {
        //        return StatusCode(500, "An error occurred while processing your request");

        //    }
        //}
        ////////////////////////edit to pagination 
        public IActionResult GetAllVaccines(int pageNumber = 1, int pageSize = 4)
        {
            try
            {
                List<Vaccine> Vaccines = unit.vaccineRepository.selectall();
                List<VaccineDTO> VaccinesDTO = mapper.Map<List<VaccineDTO>>(Vaccines);
                int Total = VaccinesDTO.Count();
                if (VaccinesDTO.Count > 0)
                {
                    List<VaccineDTO> VaccinesDTO2 = VaccinesDTO.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    var response = new
                    {
                        Data = VaccinesDTO2,
                        AllData = VaccinesDTO,
                        TotalItems = Total
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound("There's no vaccines");
                }

            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");

            }
        }


        [HttpGet("VaccineNames")]
        public IActionResult GetAllVaccinesName()
        {
            try
            {
                List<Vaccine> Vaccines = unit.vaccineRepository.selectall();
                List<VaccineNames> VaccinesDTO = mapper.Map<List<VaccineNames>>(Vaccines);
                if (VaccinesDTO.Count > 0)
                {
                    return Ok(VaccinesDTO);
                }
                else
                {
                    return NotFound("There's no vaccines");
                }

            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");

            }
        }


        [HttpGet("{id}")]
        public IActionResult GetVaccineByID(int id)
        {
            try
            {
                Vaccine vaccine = unit.vaccineRepository.selectbyid(id);
                if(vaccine == null)
                {
                    return NotFound("This Vaccine is not found");
                }
                VaccineDTO vaccineDTO = mapper.Map<VaccineDTO>(vaccine);
                return Ok(vaccineDTO);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("vaccineName/{name}")]
        public IActionResult GetVaccineByName(String name)
        {
            try
            {
                Vaccine vaccine = unit.vaccineRepository.FirstOrDefault(x => x.Name == name);
                if (vaccine == null)
                {
                    return NotFound("This Vaccine is not found");
                }
                VaccineDTO vaccineDTO = mapper.Map<VaccineDTO>(vaccine);
                return Ok(vaccineDTO);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }



        [HttpPost]
        public IActionResult AddVaccine(VaccinePostDTO NewVaccineDTO)
        {
            try
            {
                if (NewVaccineDTO == null)
                {
                    return BadRequest();
                }
                Vaccine vaccine = mapper.Map<Vaccine>(NewVaccineDTO);
                unit.vaccineRepository.add(vaccine);
                unit.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


        [HttpPut]
        public IActionResult EditVaccine(VaccineDTO vaccineDTO)
        {
            try
            {
                if (vaccineDTO == null)
                {
                    return BadRequest("Please enter the required data");
                }
                Vaccine vaccine = mapper.Map<Vaccine>(vaccineDTO);
                unit.vaccineRepository.update(vaccine);
                unit.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteVaccine(int id)
        {
            try
            {
                List<Vaccine_Clinic> VC= unit.vaccine_ClinicRepository.FindBy(s=>s.VaccineID==id);
                unit.vaccine_ClinicRepository.DeleteEntities(VC);

                List<Vaccine_Pet> VP = unit.vaccine_PetRepository.FindBy(s=>s.VaccineID==id);
                unit.vaccine_PetRepository.DeleteEntities(VP);

                List<Reservation_For_Vaccine> RFV = unit.reservation_For_VaccineRepository.FindBy(s => s.VaccineID == id);
                unit.reservation_For_VaccineRepository.DeleteEntities(RFV);

                unit.vaccineRepository.delete(id);
                unit.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
