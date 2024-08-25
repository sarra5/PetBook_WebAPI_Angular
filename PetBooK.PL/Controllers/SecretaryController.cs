using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBooK.BL.UOW;
using PetBooK.BL.DTO;
using PetBooK.DAL.Models;
using AutoMapper;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretaryController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;
        public SecretaryController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var secretaries = unit.secretaryRepository.SelectAllWithIncludes(s => s.SecretaryNavigation, s => s.Clinic);

                var secrdto = mapper.Map<List<SecretaryDTO>>(secretaries);

                return Ok(secrdto);
            }
            catch (Exception ex)
            {
                

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                var secretary = unit.secretaryRepository.SelectByIDInclude(id, "SecretaryID", s => s.SecretaryNavigation, s => s.Clinic);

                if (secretary == null)
                {
                    return NotFound();
                }
                var sc = mapper.Map<SecretaryDTO>(secretary);

                return Ok(sc);
            }
            catch (Exception ex)
            {


                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("getallinfoaboutThisClinicBySID/{id}")]
        public ActionResult GetAllInfoById(int id)
        {
            try
            {
                var secretary = unit.secretaryRepository.SelectByIDInclude(id, "SecretaryID", s => s.SecretaryNavigation, s => s.Clinic );

                if (secretary == null)
                {
                    return NotFound();
                }
                var sc = mapper.Map<SecretaryDTO>(secretary);

                return Ok(sc);
            }
            catch (Exception ex)
            {


                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var secretary = unit.secretaryRepository.SelectByIDInclude(id, "SecretaryID", s => s.SecretaryNavigation);
                if (secretary == null)
                {
                    return NotFound();
                }

                var secretaryDTO = mapper.Map<SecretaryDTO>(secretary);

             
                unit.secretaryRepository.delete(id);
                if (secretary.SecretaryNavigation != null)
                {
                    unit.userRepository.delete(secretary.SecretaryNavigation.UserID);
                }
                unit.SaveChanges();

               

                return NoContent();
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SecretaryDTO secretaryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var secretary = unit.secretaryRepository.SelectByIDInclude(id, "SecretaryID", s => s.SecretaryNavigation, s => s.Clinic);
                if (secretary == null)
                {
                    return NotFound();
                }
                mapper.Map(secretaryDTO, secretary);

                unit.secretaryRepository.update(secretary);
                unit.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost]
        public ActionResult Addsecretary(SecretaryDTO secretaryaddDTO)
        {
            if (secretaryaddDTO == null)
                return BadRequest();
            else
            {
                var secr = mapper.Map<Secretary>(secretaryaddDTO);
                unit.secretaryRepository.add(secr);
                unit.SaveChanges();
                return Ok(secretaryaddDTO);
            }
        }




    }
}
