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
    public class RoleController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;
        public RoleController(UnitOfWork _unit, IMapper _mapper)
        {
            unit = _unit;
            mapper = _mapper;
        }


        [HttpGet]
        public IActionResult GetAllRoles()
        {
            try
            {
                List<Role> roles = unit.roleRepository.selectall();
                List<RoleDTO> rolesDTO = mapper.Map<List<RoleDTO>>(roles);
                if (rolesDTO.Count > 0)
                {
                    return Ok(rolesDTO);
                }
                else
                {
                    return NotFound("There's no roles");
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRoleByID(int id)
        {
            try
            {
                Role role = unit.roleRepository.selectbyid(id);
                if(role == null)
                {
                    return BadRequest("This id is not found");
                }
                RoleDTO roleDTO = mapper.Map<RoleDTO>(role);
                return Ok(roleDTO);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


        [HttpPost]
        public IActionResult AddNewRole(RolePostDTO roleDTO)
        {
            try
            {
                if (roleDTO == null)    
                {
                    return BadRequest("You must enter a data");
                }
                Role role = mapper.Map<Role>(roleDTO);
                unit.roleRepository.add(role);
                unit.SaveChanges();  //??
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPut]
        public IActionResult EditRole(RoleDTO roleDTO)
        {
            try
            {
                if (roleDTO == null)
                {
                    return BadRequest("Please enter the required data");
                }
                //Role role = unit.roleRepository.selectbyid(roleDTO.RoleID);
                //if (role == null)
                //{
                //    return NotFound("This Id is not found");
                //}
                else
                {
                    Role RoleToSave = mapper.Map<Role>(roleDTO);
                    unit.roleRepository.update(RoleToSave);
                    unit.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                unit.userRepository.FindByAndSetForeignKeyToNull(user => user.RoleID == id,user => user.RoleID);

                unit.roleRepository.delete(id);
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
