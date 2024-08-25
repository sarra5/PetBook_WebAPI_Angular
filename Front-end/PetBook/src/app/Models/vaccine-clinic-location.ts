export class VaccineClinicLocation {
    constructor(
        public clinicID :number,
        public  name :string,
        public location :string,
        public rate :number,
        public price : number,
        public  Quantity :number ,
        public latitude: number, // Optional latitude property
        public longitude: number, // Optional longitude property
        public distance :number
      ){}
}
