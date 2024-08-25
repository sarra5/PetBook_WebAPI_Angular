using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using System.Collections.Generic;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public ReservationController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAllReservations()
        {
            try
            {
                List<Reservation> reservations = unit.reservationRepository.SelectAll(r => r.Clinic, r => r.Pet);
                if (reservations == null || !reservations.Any())
                    return NotFound("No Data");

                List<ReservationGetDTO> reservationDTOs = mapper.Map<List<ReservationGetDTO>>(reservations);
                return Ok(reservationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving reservations.");
            }
        }

        [HttpGet("pet/{PetId}")]
        public ActionResult GetReservationByPetId(int PetId)
        {
            try
            {
                List<Reservation> reservations = unit.reservationRepository.FindBy(p => p.PetID == PetId);

                if (reservations == null || !reservations.Any())
                    return NotFound($"Reservation with Pet ID {PetId} not found.");

                List<ReservationPostDTO> reservationDTOs = mapper.Map<List<ReservationPostDTO>>(reservations);
                return Ok(reservationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("clinic/{ClinicId}")]
        public ActionResult GetReservationByClinicId(int ClinicId)
        {
            try
            {
                List<Reservation> reservations = unit.reservationRepository.FindBy(c => c.ClinicID == ClinicId);

                if (reservations == null || !reservations.Any())
                    return NotFound($"Reservation with Clinic ID {ClinicId} not found.");

                List<ReservationPostDTO> reservationDTOs = mapper.Map<List<ReservationPostDTO>>(reservations);
                return Ok(reservationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("clinicIncludeUserInfo/{ClinicId}")]
        public ActionResult GetReservationByClinicIdInclude(int ClinicId)
        {
            try
            {
                List<Reservation> reservations = unit.reservationRepository.FindByInclude(c => c.ClinicID == ClinicId, s => s.Pet.User);

                if (reservations == null || !reservations.Any())
                    return NotFound($"Reservation with Clinic ID {ClinicId} not found.");

                List<ReservationIncludeUserDTO> reservationDTOs = mapper.Map<List<ReservationIncludeUserDTO>>(reservations);
                foreach (var item in reservationDTOs)
                {
                    Pet pet = unit.petRepository.selectbyid(item.PetID);
                    User us = unit.userRepository.selectbyid(pet.UserID);
                    item.Phone = us.Phone;
                    item.Name = us.Name;
                    item.UserID= us.UserID;

                }
                return Ok(reservationDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpGet("{ClinicId:int}/{PetID:int}")]
        public ActionResult GetReservationByClinicAndPetId(int ClinicId, int PetID)
        {
            try
            {
                var reservation = unit.reservationRepository.FirstOrDefault(c => c.ClinicID == ClinicId && c.PetID == PetID);

                if (reservation == null)
                    return NotFound($"Reservation with Clinic ID {ClinicId} and Pet ID {PetID} not found.");

                var reservationDTO = mapper.Map<ReservationPostDTO>(reservation);
                return Ok(reservationDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the reservation.");
            }
        }

        [HttpPost]
        public ActionResult AddReservation(ReservationPostDTO reservationDTO)
        {
            try
            {
                if (reservationDTO == null)
                    return BadRequest("Reservation data is null");

                var existingReservation = unit.reservationRepository
                    .FirstOrDefault(c => c.ClinicID == reservationDTO.ClinicID && c.PetID == reservationDTO.PetID);

                if (existingReservation != null)
                    return BadRequest("Reservation already exists");

                Reservation reservation = mapper.Map<Reservation>(reservationDTO);
                unit.reservationRepository.add(reservation);
                unit.SaveChanges();
                return Ok(new { message = "Successfully added" }); // Returning JSON
            }
            catch (Exception ex)
            {
                // Log the exception message for debugging purposes
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { error = "An unexpected error occurred." }); // Returning JSON
            }
        }



        [HttpPut]
        public ActionResult UpdateReservation(ReservationPostDTO reservationDTO)
        {
            try
            {
                if (reservationDTO == null)
                    return BadRequest("Reservation data is null");

                var existingReservation = unit.reservationRepository
                .FirstOrDefault(c => c.ClinicID == reservationDTO.ClinicID && c.PetID == reservationDTO.PetID);

                if (existingReservation == null)
                    return NotFound("Reservation not found");

                mapper.Map(reservationDTO, existingReservation);

                unit.reservationRepository.update(existingReservation);
                unit.SaveChanges();
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{PetID:int}/{ClinicId:int}")]
        public ActionResult DeleteReservation(int PetID, int ClinicID)
        {
            try
            {
                if (PetID == null || ClinicID == null)
                    return BadRequest("Reservation data is null");

                var reservation = unit.reservationRepository.FirstOrDefault(c => c.ClinicID == ClinicID && c.PetID == PetID);
                
                if (reservation == null)
                    return NotFound("No Data to delete");

                unit.reservationRepository.deleteEntity(reservation);
                unit.SaveChanges();
                return Ok(new { message = "Successfully Deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetReservationForClinicbypetID/{id}")]
        public IActionResult GetReservationsForClinicByPetId(int id)
        {
            List<Reservation> reservationForVaccine = unit.reservationRepository.FindByInclude(s => s.PetID == id, s => s.Clinic,s=>s.Pet);
            if (reservationForVaccine == null)
            {
                return Ok("there is no reservation for this pet");
            }
            else
            {
                List<ReservationGetDTO> reservationForClinicDTO = mapper.Map<List<ReservationGetDTO>>(reservationForVaccine);
                
                return Ok(reservationForClinicDTO);
            }
        }


    }

}

