import { ClinicPhones } from "./clinic-phones";

export class VaccineReservation {
    constructor(
        public  petID: number,
        public clinicID : number,
        public vaccineID : number,
        public date: Date,
        public clinicName: string,
        public petName: string,
        public vaccineName: string,
        public Phones: ClinicPhones[],

    ){}
}
