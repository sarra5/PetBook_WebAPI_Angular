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
    public class ClinicDoctorController : ControllerBase
    {
        IMapper mapper;
        UnitOfWork unit;
        public ClinicDoctorController(IMapper _imapper, UnitOfWork _unit)
        {
            mapper = _imapper;
            unit = _unit;
        }
        [HttpGet]
        public IActionResult GetAllClinicDoctors()
        {
            try
            {
                List<Clinic_Doctor> ClinicDoctors = unit.clinic_DoctorRepository.SelectAll(r=>r.clinic,d=>d.doctor,d=>d.doctor.DoctorNavigation);
                if(ClinicDoctors.Count  > 0)
                {
                    List<ClinicDoctorDTO> ClinicDoctorsDTO = mapper.Map<List<ClinicDoctorDTO>>(ClinicDoctors);
                    return Ok(ClinicDoctorsDTO);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpGet("{DCid}/{CLid}")]
        public IActionResult GetClinicDoctorByDoctor(int DCid,int CLid) 
        {
            try
            {
                Clinic_Doctor CD = unit.clinic_DoctorRepository.SelectByCompositeKeyInclude("ClinicID", CLid, "DoctorID",DCid,r => r.clinic, d => d.doctor,d=>d.doctor.DoctorNavigation);
                if(CD == null)
                {
                    return NotFound();
                }
                ClinicDoctorDTO CDDTO = mapper.Map<ClinicDoctorDTO>(CD);
                return Ok(CDDTO);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public IActionResult AddNewClinicDoctor(ClinicDoctorPostDTO NewClinicDoctor)
        {
            try
            {
                if(NewClinicDoctor == null)
                {
                    return BadRequest();
                }
                Clinic_Doctor CD = unit.clinic_DoctorRepository.FirstOrDefault(s => s.ClinicID == NewClinicDoctor.ClinicID && s.DoctorID == NewClinicDoctor.DoctorID);
                if(CD != null)
                {
                    return BadRequest("This doctor already exist in this clinic");
                }
                Clinic_Doctor CDMapped = mapper.Map<Clinic_Doctor>(NewClinicDoctor);
                unit.clinic_DoctorRepository.add(CDMapped);
                unit.SaveChanges();
                return Ok(NewClinicDoctor);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{CLid}/{DCid}")]
        public IActionResult DeleteClinicDoctor(int CLid, int DCid)
        {
            try
            {
                Clinic_Doctor CD = unit.clinic_DoctorRepository.FirstOrDefault(s => s.ClinicID == CLid && s.DoctorID == DCid);
                if(CD == null)
                {
                    return NotFound();
                }
                unit.clinic_DoctorRepository.deleteEntity(CD);
                unit.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{cid}")]
        public IActionResult GetDoctorsByClinicId(int cid)
        {
            List<Clinic_Doctor> ClinicDoctors = unit.clinic_DoctorRepository.FindByInclude(s => s.ClinicID == cid,
                s => s.doctor.DoctorNavigation, s => s.clinic);
            if (ClinicDoctors == null)
            {
                return Ok("there is no Doctors in this clinic");
            }
            else
            {
                List<ClinicDoctorssDTO> ClinicDoctorsDTO = mapper.Map<List<ClinicDoctorssDTO>>(ClinicDoctors);
                return Ok(ClinicDoctorsDTO);
            }
        }

    }
}
