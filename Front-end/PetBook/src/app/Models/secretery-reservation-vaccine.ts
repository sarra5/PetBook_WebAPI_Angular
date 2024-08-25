export class SecreteryReservationVaccine {
    constructor( 
        public petID: number,
        public clinicID: number,
        public date: Date,
        public  petName : string,
        public  name :string,
        public  phone :string,
        public  vaccineID:number,
        public vaccineName:number
    ){}
}
