using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using System.Security.Cryptography;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationForVaccineController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public ReservationForVaccineController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        //------Get All Reservations for Vaccine-------
        [HttpGet]
        public IActionResult getAllReservationForVaccine()
        {
            List<Reservation_For_Vaccine> allReservationOfVaccine = unit.reservation_For_VaccineRepository.SelectAll(rv => rv.Clinic, rv => rv.Pet, rv => rv.Vaccine);
            if (allReservationOfVaccine.Count < 0)
            {
                return NotFound("reservations for vaccines are not found");
            }
            else
            {
                List<ReservationForVaccineDTO> allReservationForVaccineDTO = mapper.Map<List<ReservationForVaccineDTO>>(allReservationOfVaccine);
                return Ok(allReservationForVaccineDTO);
            }
        }
        //------Get Reservation for Vaccine by comp id-------
        [HttpGet("{Vid}/{Cid}/{Pid}")]
        public IActionResult getReservationForVaccine(int Vid, int Cid, int Pid)
        {
            Reservation_For_Vaccine reservationForVaccine = unit.reservation_For_VaccineRepository.SelectBy3CompositeKeyInclude("VaccineID", Vid, "ClinicID", Cid, "PetID", Pid, rv => rv.Clinic, rv => rv.Pet, rv => rv.Vaccine);
            if (reservationForVaccine == null)
            {
                return NotFound("Requested Reservation For Vaccine is not found");
            }

            else
            {
                ReservationForVaccineDTO reservationForVaccineDTO = mapper.Map<ReservationForVaccineDTO>(reservationForVaccine);
                return Ok(reservationForVaccineDTO);
            }
        }
        //------add Reservation for Vaccine-------
        [HttpPost]
        public IActionResult addNewReservationForVaccine(ReservationForVaccineAddDTO newReservationForVaccineDTO)
        {

            if (newReservationForVaccineDTO == null)
            {
                return BadRequest("The data of new reservation for vaccine is null");
            }
            else
            {
                Reservation_For_Vaccine newReservationForVaccine = mapper.Map<Reservation_For_Vaccine>(newReservationForVaccineDTO);
                unit.reservation_For_VaccineRepository.add(newReservationForVaccine);
                unit.SaveChanges();
                return Ok(newReservationForVaccine);
            }

        }
        //------update Reservation for Vaccine-------
        [HttpPut]
        public IActionResult updateReservationForVaccine(ReservationForVaccineAddDTO updatedReservationForVaccineDTO)
        {
            if (updatedReservationForVaccineDTO == null)
            {
                return NotFound("The data of reservation for vaccine you want to update is null");
            }
            else
            {
                Reservation_For_Vaccine updatedReservationForVaccine = mapper.Map<Reservation_For_Vaccine>(updatedReservationForVaccineDTO);
                unit.reservation_For_VaccineRepository.update(updatedReservationForVaccine);
                unit.SaveChanges();
                return Ok(updatedReservationForVaccine);
            }
        }
        //-----Delete Reservation for Vaccine-------
        [HttpDelete("{Vid}/{Cid}/{Pid}")]
        public IActionResult deleteReservationForVaccine(int Vid, int Cid, int Pid)
        {
            Reservation_For_Vaccine deletedReservationForVaccine = unit.reservation_For_VaccineRepository.SelectBy3CompositeKeyInclude("VaccineID", Vid, "ClinicID", Cid, "PetID", Pid, rv => rv.Clinic, rv => rv.Pet, rv => rv.Vaccine);

            if (deletedReservationForVaccine == null)
            {
                return NotFound("The request you want to delete is not found");

            }

            else
            {
                unit.reservation_For_VaccineRepository.deleteEntity(deletedReservationForVaccine);
                unit.SaveChanges();
                return Ok(new { message = "Successfully deleted" });
            }
        }


        //---------------------------------------------------------------------------------

        [HttpGet("reservation_For_VaccineRepository/{Cid}")]
        public IActionResult getByClinicId(int Cid)
        {
            List<Reservation_For_Vaccine> reservationForVaccine = unit.reservation_For_VaccineRepository.FindByInclude(s => s.ClinicID == Cid, s => s.Clinic, s => s.Vaccine, s => s.Pet);
            if (reservationForVaccine == null)
            {
                return NotFound("Requested Reservation For Vaccine is not found");
            }

            else
            {
                List<ReservationFoeVaccineInclude> reservationForVaccineDTO = mapper.Map<List<ReservationFoeVaccineInclude>>(reservationForVaccine);
                foreach (var item in reservationForVaccineDTO)
                {
                    Pet pet = unit.petRepository.selectbyid(item.PetID);
                    User us = unit.userRepository.selectbyid(pet.UserID);
                    item.Phone = us.Phone;
                    item.Name = us.Name;

                }
                return Ok(reservationForVaccineDTO);
            }
        }

        //--------Get Reservation for Vaccine by petID--------
        [HttpGet ("GetReservationforVaccinebypetID/{id}")]
       public IActionResult GetReservationsForVaccByPetId(int id)
        {
            List<Reservation_For_Vaccine> reservationForVaccine = unit.reservation_For_VaccineRepository.FindByInclude(s => s.PetID == id, s => s.Clinic, s => s.Vaccine, s => s.Pet);
            if (reservationForVaccine == null)
            {
                return Ok("there is no reservation for this pet");
            }
            else
            {
                List<ReservationFoeVaccineInclude> reservationForVaccineDTO = mapper.Map<List<ReservationFoeVaccineInclude>>(reservationForVaccine);
                foreach (var item in reservationForVaccineDTO)
                {
                    Pet pet = unit.petRepository.selectbyid(item.PetID);
                    User us = unit.userRepository.selectbyid(pet.UserID);
                    item.Phone = us.Phone;
                    item.Name = us.Name;

                }
                return Ok(reservationForVaccineDTO);
            }
        } 


    }

}
