using PetBooK.BL.Reo;
using PetBooK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.UOW
{
    public class UnitOfWork
    {
        PetBookContext db;
        GenericRepo<Breed> BreedRepository;
        GenericRepo<Client> ClientRepository;
        GenericRepo<Clinic> ClinicRepository;
        GenericRepo<Clinic_Doctor> Clinic_DoctorRepository;
        GenericRepo<Clinic_Phone> Clinic_PhoneRepository;
        GenericRepo<Doctor> DoctorRepository;
        GenericRepo<Pet> PetRepository;
        GenericRepo<Pet_Breed> Pet_BreedRepository;
        GenericRepo<Request_For_Breed> Request_For_BreedRepository;
        GenericRepo<Reservation> ReservationRepository;
        GenericRepo<Reservation_For_Vaccine> Reservation_For_VaccineRepository;
        GenericRepo<Role> RoleRepository;
        GenericRepo<Secretary> SecretaryRepository;
        GenericRepo<User> UserRepository;
        GenericRepo<Vaccine> VaccineRepository;
        GenericRepo<Vaccine_Clinic> Vaccine_ClinicRepository;
        GenericRepo<Vaccine_Pet> Vaccine_PetRepository;
        GenericRepo<Clinic_Location> Clinic_LocationRepository;






        public UnitOfWork(PetBookContext db)
        {
            this.db = db;
        }




        public GenericRepo<Breed> breedRepository
        {
            get
            {
                if (BreedRepository == null)
                {
                    BreedRepository = new GenericRepo<Breed>(db);
                }
                return BreedRepository;
            }
        }

        public GenericRepo<Client> clientRepository
        {
            get
            {
                if (ClientRepository == null)
                {
                    ClientRepository = new GenericRepo<Client>(db);
                }
                return ClientRepository;
            }
        }

        public GenericRepo<Clinic> clinicRepository
        {
            get
            {
                if (ClinicRepository == null)
                {
                    ClinicRepository = new GenericRepo<Clinic>(db);
                }
                return ClinicRepository;
            }
        }


        public GenericRepo<Clinic_Doctor> clinic_DoctorRepository
        {
            get
            {
                if (Clinic_DoctorRepository == null)
                {
                    Clinic_DoctorRepository = new GenericRepo<Clinic_Doctor>(db);
                }
                return Clinic_DoctorRepository;
            }
        }

        public GenericRepo<Clinic_Phone> clinic_PhoneRepository
        {
            get
            {
                if (Clinic_PhoneRepository == null)
                {
                    Clinic_PhoneRepository = new GenericRepo<Clinic_Phone>(db);
                }
                return Clinic_PhoneRepository;
            }
        }

        public GenericRepo<Doctor> doctorRepository
        {
            get
            {
                if (DoctorRepository == null)
                {
                    DoctorRepository = new GenericRepo<Doctor>(db);
                }
                return DoctorRepository;
            }
        }

        public GenericRepo<Pet> petRepository
        {
            get
            {
                if (PetRepository == null)
                {
                    PetRepository = new GenericRepo<Pet>(db);
                }
                return PetRepository;
            }
        }

        public GenericRepo<Pet_Breed> pet_BreedRepository
        {
            get
            {
                if (Pet_BreedRepository == null)
                {
                    Pet_BreedRepository = new GenericRepo<Pet_Breed>(db);
                }
                return Pet_BreedRepository;
            }
        }
        public GenericRepo<Request_For_Breed> request_For_BreedRepository
        {
            get
            {
                if (Request_For_BreedRepository == null)
                {
                    Request_For_BreedRepository = new GenericRepo<Request_For_Breed>(db);
                }
                return Request_For_BreedRepository;
            }
        }

        public GenericRepo<Reservation> reservationRepository
        {
            get
            {
                if (ReservationRepository == null)
                {
                    ReservationRepository = new GenericRepo<Reservation>(db);
                }
                return ReservationRepository;
            }
        }

        public GenericRepo<Reservation_For_Vaccine> reservation_For_VaccineRepository
        {
            get
            {
                if (Reservation_For_VaccineRepository == null)
                {
                    Reservation_For_VaccineRepository = new GenericRepo<Reservation_For_Vaccine>(db);
                }
                return Reservation_For_VaccineRepository;
            }
        }

        public GenericRepo<Role> roleRepository
        {
            get
            {
                if (RoleRepository == null)
                {
                    RoleRepository = new GenericRepo<Role>(db);
                }
                return RoleRepository;
            }
        }

        public GenericRepo<Secretary> secretaryRepository
        {
            get
            {
                if (SecretaryRepository == null)
                {
                    SecretaryRepository = new GenericRepo<Secretary>(db);
                }
                return SecretaryRepository;
            }
        }

        public GenericRepo<User> userRepository
        {
            get
            {
                if (UserRepository == null)
                {
                    UserRepository = new GenericRepo<User>(db);
                }
                return UserRepository;
            }
        }

        public GenericRepo<Vaccine> vaccineRepository
        {
            get
            {
                if (VaccineRepository == null)
                {
                    VaccineRepository = new GenericRepo<Vaccine>(db);
                }
                return VaccineRepository;
            }
        }

        public GenericRepo<Vaccine_Clinic> vaccine_ClinicRepository
        {
            get
            {
                if (Vaccine_ClinicRepository == null)
                {
                    Vaccine_ClinicRepository = new GenericRepo<Vaccine_Clinic>(db);
                }
                return Vaccine_ClinicRepository;
            }
        }

        public GenericRepo<Vaccine_Pet> vaccine_PetRepository
        {
            get
            {
                if (Vaccine_PetRepository == null)
                {
                    Vaccine_PetRepository = new GenericRepo<Vaccine_Pet>(db);
                }
                return Vaccine_PetRepository;
            }
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public GenericRepo<Clinic_Location> clinic_LocationRepository
        {
            get
            {
                if (Clinic_LocationRepository == null)
                {
                    Clinic_LocationRepository = new GenericRepo<Clinic_Location>(db);
                }
                return Clinic_LocationRepository;
            }
        }

    }
}
