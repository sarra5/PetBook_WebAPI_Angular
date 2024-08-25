using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PetBooK.BL.DTO;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using PetBooK.PL.Hubs;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace PetBooK.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBreedController : ControllerBase
    {
        UnitOfWork unit;
        IMapper mapper;

        // For SignalR to broadcast for frontend
        IHubContext<PetHub> hubContext;

        public RequestBreedController(UnitOfWork unit, IMapper mapper, IHubContext<PetHub> hubContext)
        {
            this.unit = unit;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        //-------Get All Requests of breed--------
        [HttpGet]
        public IActionResult getAllRequestsOfBreed()
        {
            List<Request_For_Breed> allRequestsOfBreed = unit.request_For_BreedRepository.SelectAll(rb => rb.PetIDSenderNavigation, rb => rb.PetIDReceiverNavigation);


            if (allRequestsOfBreed.Count < 0)
            {
                return NotFound("No Data Found");
            }
            else
            {
                List<RequestBreedDTO> requestsOfBreedDTO = mapper.Map<List<RequestBreedDTO>>(allRequestsOfBreed);
                return Ok(requestsOfBreedDTO);
            }
        }

        //-------Get Request of breed by id--------
        [HttpGet("{Sid}/{Rid}")]
        public IActionResult getRequestBreedByID(int Sid, int Rid)
        {
            Request_For_Breed requestOfBreed = unit.request_For_BreedRepository.SelectByCompositeKeyInclude("PetIDSender", Sid, "PetIDReceiver", Rid, rb => rb.PetIDSenderNavigation, rb => rb.PetIDReceiverNavigation);
            if (requestOfBreed == null)
            {
                return NotFound("the data required is not found");
            }
            else
            {
                RequestBreedDTO requestOfBreedDTO = mapper.Map<RequestBreedDTO>(requestOfBreed);
                return Ok(requestOfBreedDTO);
            }
        }
        ////-------Get Request of breed by Sender id--------
        [HttpGet("SenderID/{Sid}")]
        public IActionResult getRequestBreedBySenderID(int Sid)
        {
            List<Request_For_Breed> requestOfBreed = unit.request_For_BreedRepository.FindByInclude(s => s.PetIDSender == Sid, s => s.PetIDSenderNavigation, s => s.PetIDReceiverNavigation);
            if (requestOfBreed == null)
            {
                return NotFound("the data required is not found");
            }
            else
            {
                List<RequestBreedDTO> requestOfBreedDTO = mapper.Map<List<RequestBreedDTO>>(requestOfBreed);
                foreach (var item in requestOfBreedDTO)
                {
                    var pet = unit.petRepository.SelectByIDInclude(Sid, "PetID", s => s.User);
                    var user = unit.userRepository.selectbyid(pet.User.ClientID);
                    item.OwnersenderName = user.Name;

                    var pet2 = unit.petRepository.SelectByIDInclude(item.PetIDReceiver, "PetID", s => s.User);
                    var user2 = unit.userRepository.selectbyid(pet2.User.ClientID);
                    item.OwnerreceiverName = user2.Name;
                }
                return Ok(requestOfBreedDTO);
            }
        }

        ////-------Get Request of breed by Sender id--------
        [HttpGet("UserSenderID/{USERid}")]
        public IActionResult getRequestBreedByUserSenderID(int USERid)
        {
            List<Pet> pets = unit.petRepository.FindBy(s => s.UserID == USERid);

            if (pets == null || !pets.Any())
            {
                return NotFound("No pets found for the given user.");
            }

            List<RequestBreedDTO> allRequests = new List<RequestBreedDTO>();

            foreach (var pet in pets)
            {
                List<Request_For_Breed> requestOfBreed = unit.request_For_BreedRepository.FindByInclude(
                    s => s.PetIDSender == pet.PetID && s.Pair == false,
                    s => s.PetIDSenderNavigation,
                    s => s.PetIDReceiverNavigation
                );

                if (requestOfBreed == null || !requestOfBreed.Any())
                {
                    continue; // No requests found for this pet, continue to the next pet
                }

                List<RequestBreedDTO> requestOfBreedDTO = mapper.Map<List<RequestBreedDTO>>(requestOfBreed);

                foreach (var request in requestOfBreedDTO)
                {
                    var petSender = unit.petRepository.SelectByIDInclude(request.PetIDSender, "PetID", p => p.User);
                    var userSender = unit.userRepository.selectbyid(petSender.User.ClientID);
                    request.OwnersenderName = userSender.Name;

                    var petReceiver = unit.petRepository.SelectByIDInclude(request.PetIDReceiver, "PetID", p => p.User);
                    var userReceiver = unit.userRepository.selectbyid(petReceiver.User.ClientID);
                    request.OwnerreceiverName = userReceiver.Name;
                }

                allRequests.AddRange(requestOfBreedDTO);
            }

            if (!allRequests.Any())
            {
                return NotFound("No breed requests found for the given user's pets.");
            }

            return Ok(allRequests);
        }



        //////-------Get Request of breed by Receiver id--------

        [HttpGet("ReceiverID/{Rid}")]
        public IActionResult getRequestBreedByReceiverID(int Rid)
        {
            List<Request_For_Breed> requestOfBreed = unit.request_For_BreedRepository.FindByInclude(s => s.PetIDReceiver == Rid, s => s.PetIDSenderNavigation, s => s.PetIDReceiverNavigation);
            if (requestOfBreed == null)
            {
                return NotFound("the data required is not found");
            }
            else
            {
                List<RequestBreedDTO> requestOfBreedDTO = mapper.Map<List<RequestBreedDTO>>(requestOfBreed);

                foreach (var item in requestOfBreedDTO)
                {
                    var pet = unit.petRepository.SelectByIDInclude(item.PetIDSender, "PetID", s => s.User);
                    var user = unit.userRepository.selectbyid(pet.User.ClientID);
                    item.OwnerreceiverName = user.Name;

                    var pet2 = unit.petRepository.SelectByIDInclude(Rid, "PetID", s => s.User);
                    var user2 = unit.userRepository.selectbyid(pet2.User.ClientID);
                    item.OwnersenderName = user2.Name;
                }
                return Ok(requestOfBreedDTO);
            }
        }


        ////-------Get Request of breed by Sender id--------
        [HttpGet("UserReceiverID/{USERid}")]
        public IActionResult getRequestBreedByUserReceiverID(int USERid)
        {
            List<Pet> pets = unit.petRepository.FindBy(s => s.UserID == USERid);

            if (pets == null || !pets.Any())
            {
                return NotFound("No pets found for the given user.");
            }

            List<RequestBreedDTO> allRequests = new List<RequestBreedDTO>();

            foreach (var pet in pets)
            {
                List<Request_For_Breed> requestOfBreed = unit.request_For_BreedRepository.FindByInclude(
                    s => s.PetIDReceiver == pet.PetID && s.Pair == false,
                    s => s.PetIDSenderNavigation,
                    s => s.PetIDReceiverNavigation
                );

                if (requestOfBreed == null || !requestOfBreed.Any())
                {
                    continue; // No requests found for this pet, continue to the next pet
                }

                List<RequestBreedDTO> requestOfBreedDTO = mapper.Map<List<RequestBreedDTO>>(requestOfBreed);

                foreach (var request in requestOfBreedDTO)
                {
                    var petSender = unit.petRepository.SelectByIDInclude(request.PetIDSender, "PetID", p => p.User);
                    var userSender = unit.userRepository.selectbyid(petSender.User.ClientID);
                    request.OwnersenderName = userSender.Name;

                    var petReceiver = unit.petRepository.SelectByIDInclude(request.PetIDReceiver, "PetID", p => p.User);
                    var userReceiver = unit.userRepository.selectbyid(petReceiver.User.ClientID);
                    request.OwnerreceiverName = userReceiver.Name;
                }

                allRequests.AddRange(requestOfBreedDTO);
            }

            if (!allRequests.Any())
            {
                return NotFound("No breed requests found for the given user's pets.");
            }

            return Ok(allRequests);
        }

        //-------Add Request of breed--------
        [HttpPost]
        public IActionResult addNewRequestBreed(RequestBreedAddDTO newRequestBreedDTO)
        {
            if (newRequestBreedDTO == null)
            {
                return BadRequest("The data you sent is null");
            }

            else
            {
                Request_For_Breed requestOfBreed = mapper.Map<Request_For_Breed>(newRequestBreedDTO);
                requestOfBreed.Pair = false;
                unit.request_For_BreedRepository.add(requestOfBreed);
                unit.SaveChanges();
                return Ok(requestOfBreed);
            }
        }

        //-------update Request of breed--------
        [HttpPut]
        public IActionResult updateRequestOfBreed(RequestBreedUpdateDTO updatedrequestBreedDTO)
        {
            try
            {
                if (updatedrequestBreedDTO == null)
                {
                    return BadRequest("Your Updated request data is null");
                }
                else
                {
                    Request_For_Breed updatedRequestBreed = mapper.Map<Request_For_Breed>(updatedrequestBreedDTO);
                    unit.request_For_BreedRepository.update(updatedRequestBreed);
                    unit.SaveChanges();
                    return Ok(updatedRequestBreed);
                }
            }
            catch
            {
                return StatusCode(500, "check if the ids of the request of breed are exist in pets table or any thing else");
            }
            
        }
        //-------delete Request of breed--------
        [HttpDelete("{SID}/{RID}")]
        public IActionResult deleteRequestBreed(int SID,int RID)
        {
            Request_For_Breed deletedRequestOfBreed = unit.request_For_BreedRepository.SelectByCompositeKeyInclude("PetIDSender", SID, "PetIDReceiver", RID, rb => rb.PetIDSenderNavigation, rb => rb.PetIDReceiverNavigation);
            if (deletedRequestOfBreed==null)
            {
                return NotFound("the request you want to delete is not found");
            }
            else
            {
                unit.request_For_BreedRepository.deleteEntity(deletedRequestOfBreed);
                unit.SaveChanges();
                return Ok("deleted");
            }
        }


        //----------------delete pet who just married from requests-------------------//

        [HttpDelete("deletePetFromRequestsAndPendingR/{ID}")]
        public IActionResult deletePetFromRequests(int ID)
        {
            List<Request_For_Breed> deletedRequestOfBreed = unit.request_For_BreedRepository.FindBy(s=>s.PetIDReceiver==ID && s.Pair == false || s.PetIDSender==ID && s.Pair==false);
            if (deletedRequestOfBreed == null)
            {
                return Ok("the request you want to delete is not found");
            }
            else
            {
                foreach (var item in deletedRequestOfBreed)
                {
                        unit.request_For_BreedRepository.deleteEntity(item);
                        unit.SaveChanges();
                }
                return Ok("deleted");
            }
        }



        //--------------check if this pet on date or not-------------//
        [HttpGet("Turnthispettobenotavailable/{id}")]
        public IActionResult MakethisPetNotAvailableforbreading(int id)
        {
            Pet pet = unit.petRepository.selectbyid(id);
            if (pet == null)
            {
                Console.WriteLine($"Pet with ID {id} not found");
                return NotFound();
            }

            pet.ReadyForBreeding = false;
            unit.petRepository.update(pet);
            unit.SaveChanges();
            hubContext.Clients.All.SendAsync("PetWithReadyForBreedingFalse", pet);
            Console.WriteLine($"Pet with ID {id} marked as not ready for breeding");
            return Ok();
        }

        [HttpGet("Turnthispettobeavailable/{id}")]
        public IActionResult MakethisPetAvailableforbreading(int id)
        {
            Pet pet = unit.petRepository.selectbyid(id);
            if (pet == null)
            {
                Console.WriteLine($"Pet with ID {id} not found");
                return NotFound();
            }

            pet.ReadyForBreeding = true;
            unit.petRepository.update(pet);
            unit.SaveChanges();
            hubContext.Clients.All.SendAsync("PetWithReadyForBreedingTrue", pet);
            Console.WriteLine($"Pet with ID {id} marked as ready for breeding");
            return Ok();
        }

       
        //------------------------------------------------------//
        [HttpGet("retunIfItPaired/{id}")]
        public IActionResult IsSenderPaired(int id)
        {
           List<Request_For_Breed> requestBreed = unit.request_For_BreedRepository.FindByForeignKeyInclude(rb => rb.PetIDSender == id || rb.PetIDReceiver== id, rb=>rb.PetIDSenderNavigation,rb=>rb.PetIDReceiverNavigation).Where(rb => rb.Pair == true).ToList();
            if (requestBreed.Count==0)
            {
                return Ok("this pet is not paired");
            }
            else
            {
                List<RequestBreedDTO> requestBreedDTOs = mapper.Map<List<RequestBreedDTO>>(requestBreed);
                return Ok(requestBreedDTOs[0]);
            }

        }
        //--------------Delete pairs when user click unPair---------------//
        [HttpDelete("deletePiar/{ID}")]
        public IActionResult deletePair(int ID)
        {
            List<Request_For_Breed> deletedRequestOfBreed = unit.request_For_BreedRepository.FindBy(s => s.PetIDReceiver == ID || s.PetIDSender == ID);
            if (deletedRequestOfBreed == null)
            {
                return NotFound("the request you want to delete is not found");
            }
            else
            {
                foreach (var item in deletedRequestOfBreed)
                {
                    if (item.Pair == true)
                    {
                        unit.request_For_BreedRepository.deleteEntity(item);
                        unit.SaveChanges();
                    }
                }
                return Ok("deleted");
            }
        }
    }
}
