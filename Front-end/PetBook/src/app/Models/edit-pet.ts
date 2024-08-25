export class EditPet {
    constructor(
        public PetID: number,
        public  name: string,
        public photo: File|string|null,
        public ageInMonth: number,
        public  sex: string,
        public  idNoteBookImage: File|string|null,
        public  userID: number,
        public  readyForBreeding: boolean,
        public  type: string,
        public other: string){}   
}
