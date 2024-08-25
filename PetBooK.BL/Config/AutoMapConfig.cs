using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using PetBooK.BL.DTO;
using PetBooK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.Config
{
    public class AutoMapConfig : Profile
    {
        public AutoMapConfig() 
        {


            CreateMap<BreedGetDTO, Breed>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserAddDTO, User>();
            CreateMap<DoctorDTO, Doctor>();
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<Doctor, DoctorAddDTO>();
            CreateMap<DoctorAddDTO,Doctor>();
            CreateMap<Breed, BreedGetDTO>();
            CreateMap<BreedGetDTO,Breed >();
            CreateMap<BreedAddDTO, Breed>();
            CreateMap<Pet_Breed, PetBreedAddDTO>();
            CreateMap<PetBreedAddDTO , Pet_Breed >();
            CreateMap<Pet, PetGetDTO>();
            CreateMap<PetGetDTO, Pet>();
            CreateMap<Pet, PetAddDTO>();
            CreateMap<PetAddDTO, Pet>();
            CreateMap<ClinicccDTO, Clinic>();
            CreateMap<Clinic, ClinicccDTO>();
            CreateMap<Clinic, ClinicAddDTO>();
            CreateMap<ClinicAddDTO, Clinic>();
            CreateMap<VaccineClinicInclude, Vaccine_Clinic>();
            CreateMap<Vaccine_Clinic, VaccineClinicInclude>()
                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Clinic.Name))
                      .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Clinic.Rate));


            CreateMap<Vaccine, VaccineNames>();
            CreateMap<VaccineNames, Vaccine>();


            ///Mapping Reservations
            CreateMap<Reservation, ReservationGetDTO>()
            .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic.Name))
            .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.Pet.Name));
            
            CreateMap<ReservationPostDTO, Reservation> ();


            //Mapping Role
            CreateMap<Role, RoleDTO>();  //src,dest
            CreateMap<RoleDTO, Role>();
            CreateMap<RolePostDTO, Role>();



            //Mapping Vaccine
            CreateMap<Vaccine, VaccineDTO>();
            CreateMap<VaccineDTO, Vaccine>();
            CreateMap<VaccinePostDTO, Vaccine>();
            CreateMap<Reservation, ReservationPostDTO>();

            //Mapping Clinic_Doctor
           
            CreateMap<Clinic_Doctor, ClinicDoctorDTO>()
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.doctor.DoctorNavigation.Name))
            .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.clinic.Name));

            CreateMap<ClinicDoctorDTO, Clinic_Doctor>();
            CreateMap < ClinicDoctorPostDTO,Clinic_Doctor> (); 




            CreateMap<Breed, BreedWithPetDTO>()
             .ForMember(dest => dest.PetID, opt => opt.MapFrom(src => src.Pet_Breeds.Select(p => p.PetID).ToList()));

           

            CreateMap<VaccinePetDTO, Vaccine_Pet>();
            CreateMap<Vaccine_Pet, VaccinePetDTO>();

            CreateMap<VaccineClinicDTO, Vaccine_Clinic>();
            CreateMap<Vaccine_Clinic, VaccineClinicDTO>();
            CreateMap<Secretary, SecretaryDTO>();
            CreateMap<Secretary, SecretaryDTO>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecretaryNavigation.Name))
             .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.SecretaryNavigation.Age))
             .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.SecretaryNavigation.Phone))
             .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.SecretaryNavigation.Location))
             .ForMember(dest => dest.ClinicID, opt => opt.MapFrom(src => src.Clinic.ClinicID))
             .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic.Name))
             .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.Clinic.BankAccount));
            

            CreateMap<Clinic_Phone, ClinicPhoneDTO>();
            CreateMap<Clinic_Phone, ClinicPhoneUpdateDTO>();
            CreateMap<Clinic_Location, ClinicLocationDTO>();
            CreateMap<ClinicPhoneUpdateDTO,Clinic_Phone>()
           .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.NewPhone));
            CreateMap<Clinic_Phone, ClinicPhoneDTO>()
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
            CreateMap<ClinicPhoneDTO, Clinic_Phone>();
            CreateMap<Clinic_Location, ClinicLocationUpdateDTO>();
            CreateMap<Clinic_Location, ClinicLocationUpdateDTO>()
           .ForMember(dest => dest.NewLocation, opt => opt.MapFrom(src => src.Location))
           .ForMember(dest => dest.ClinicID, opt => opt.MapFrom(src => src.ClinicID));

            //Mapping Client:
            CreateMap<Client, ClientDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ClientNavigation.Name))
            .ForMember(dest => dest.ClientID, opt => opt.MapFrom(src => src.ClientID))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.ClientNavigation.Sex))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.ClientNavigation.Password))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.ClientNavigation.Age))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ClientNavigation.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ClientNavigation.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.ClientNavigation.Phone))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.ClientNavigation.Photo))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.ClientNavigation.Location))
            .ForMember(dest => dest.petsNames, opt => opt.MapFrom(src => src.Pets
            .Where(p => p.UserID == src.ClientNavigation.UserID) // Filter pets belonging to the client
            .Select(p => p.Name)
            .ToList()));

            CreateMap<ClientDTO, Client>();

            //Mapping Request For Breed:
            CreateMap<Request_For_Breed, RequestBreedDTO>()
                .ForMember(dest => dest.senderPetName, opt => opt.MapFrom(src => src.PetIDSenderNavigation.Name))
                .ForMember(dest => dest.receiverPetName, opt => opt.MapFrom(src => src.PetIDReceiverNavigation.Name));

            CreateMap<RequestBreedAddDTO, Request_For_Breed>();

            CreateMap<RequestBreedUpdateDTO, Request_For_Breed>();

            //Mapping Reservation For Vaccine:
            CreateMap<Reservation_For_Vaccine, ReservationForVaccineDTO>();
            CreateMap<ReservationForVaccineAddDTO, Reservation_For_Vaccine
                >();
            CreateMap<Doctor, DoctorDTO>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DoctorNavigation.Name));



            CreateMap<Clinic_Location, ClinicLocationInclude>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Clinic.Name))
          .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Clinic.Rate))
          .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.Clinic.BankAccount));


            CreateMap<Reservation, ReservationIncludeUserDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Pet.User.ClientNavigation.Name))
            .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Pet.User.ClientNavigation.UserID))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Pet.User.ClientNavigation.Phone))
            .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.Pet.Name));


            CreateMap<User, DoctorUser>();
            CreateMap<DoctorUser,User>();
            CreateMap<Doctor, DoctorUser>();
            CreateMap<DoctorUser, Doctor>();


            

           

            CreateMap<Reservation_For_Vaccine, ReservationFoeVaccineInclude>().ReverseMap();

            CreateMap<Clinic_Doctor, ClinicDoctorssDTO>()
            .ForMember(dest => dest.ClinicID, opt => opt.MapFrom(src => src.ClinicID))
            .ForMember(dest => dest.DoctorID, opt => opt.MapFrom(src => src.DoctorID))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.doctor.DoctorNavigation.Name))
            .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.doctor.Degree))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.doctor.DoctorNavigation.Phone))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.doctor.DoctorNavigation.Photo));

            CreateMap<Pet, PetGetDTO>()
               .ForMember(dest => dest.BreedName, opt => opt.MapFrom(src => src.Pet_Breeds.FirstOrDefault().Breed.Breed1));


            



        }





    }
}
