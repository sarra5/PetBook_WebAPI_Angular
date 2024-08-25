export class DoctorUser {
    constructor(
        public name: string,
        public email: string,
        public password: string,
        public phone: string,
        public userName: string,
        public location: string,
        public age: number|null,
        public sex: string,
        public degree: string,
        public hiringDate: Date,
        public photo: File|null

          )
          {}
      }
      
