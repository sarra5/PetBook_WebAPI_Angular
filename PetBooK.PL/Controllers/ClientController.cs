using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBooK.BL.DTO;
using PetBooK.DAL.Models;
using AutoMapper;
using PetBooK.BL.UOW;
using Microsoft.EntityFrameworkCore;


namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        public ClientController(UnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        //----------Get all Clients----------//
        [HttpGet]
        public IActionResult getAllClients()
        {
            List<Client> allClients = unit.clientRepository.SelectAll(r => r.Pets, r => r.ClientNavigation);
            if (allClients.Count == 0)
            {
                return NotFound();
            }

            else
            {
                List<ClientDTO> clientDTOs = mapper.Map<List<ClientDTO>>(allClients);
                return Ok(clientDTOs);

            }
        }

        //----------Get Client by id----------//
        [HttpGet("{clientId}")]
        public IActionResult getClient(int clientId)
        {
            Client Client = unit.clientRepository.SelectByIDInclude(clientId, "ClientID", r => r.Pets, r => r.ClientNavigation);
            if (Client == null)
            {
                return NotFound();
            }
            else
            {
                ClientDTO clientDTO = mapper.Map<ClientDTO>(Client);
                return Ok(clientDTO);
            }
        }

        //----------Update Client----------//
        [HttpPut]
        public IActionResult UpdateClient(int id, string IdName,ClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var targtedClient = unit.clientRepository.SelectByIDInclude(id, IdName, c => c.ClientNavigation, c => c.Pets);
                if (targtedClient == null)
                {
                    return NotFound("The client you want to update is not found");
                }
                else
                {
                    Client client = mapper.Map<Client>(clientDTO);
                    //mapper.Map(clientDTO, targtedClient);
                    unit.clientRepository.update(client);
                    unit.SaveChanges();
                    return Ok(client);
                }
            }
        }
        //----------Delete Client----------//
        [HttpDelete]
        public IActionResult deleteClient(int id)
        {
            Client deletedClient = unit.clientRepository.selectbyid(id);
            if (deletedClient == null)
            {
                return NotFound("the client you want to delete is not found");

            }
            else
            {
              List<Pet> petsOfDeletedClient= unit.petRepository.FindBy(p=>p.UserID==id);
                if (petsOfDeletedClient == null)
                {
                    return BadRequest("there is no pet for this client");
                }
                else
                {
                    foreach(Pet pet in petsOfDeletedClient)
                    {
                        List<Reservation_For_Vaccine> reservation_For_Vaccines = unit.reservation_For_VaccineRepository.FindBy(rv => rv.PetID == pet.PetID);
                        unit.reservation_For_VaccineRepository.DeleteEntities(reservation_For_Vaccines);
                        //----------------------
                        List<Reservation> reservations = unit.reservationRepository.FindBy(r => r.PetID == pet.PetID);
                        unit.reservationRepository.DeleteEntities(reservations);
                        //-----------------------
                        List<Request_For_Breed> request_For_Breeds = unit.request_For_BreedRepository.FindBy(rb => rb.PetIDSender == pet.PetID || rb.PetIDReceiver == pet.PetID);
                        unit.request_For_BreedRepository.DeleteEntities(request_For_Breeds);
                        //------------------------
                        Pet_Breed petBreed = unit.pet_BreedRepository.FirstOrDefault(p=>p.PetID== pet.PetID);
                        unit.pet_BreedRepository.deleteEntity(petBreed);
                        //------------------
                        List<Vaccine_Pet> petVaccines = unit.vaccine_PetRepository.FindBy(p => p.PetID == pet.PetID);
                        unit.vaccine_PetRepository.DeleteEntities(petVaccines);
                    }
                    unit.petRepository.DeleteEntities(petsOfDeletedClient);
                    unit.clientRepository.deleteEntity(deletedClient);
                    unit.SaveChanges();
                    return Ok("Deleted");
                }
            }
        }
        //----------ADD Client----------//  
        //public void addClient(Client newClient)
        //{

        //}

     


    }
}

