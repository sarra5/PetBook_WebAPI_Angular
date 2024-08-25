export class UserPetInfo {
    constructor(
    
        public ageInMonth: number,
        public idNoteBookImage: string,
        public name: string,
        public other: string,
        public petID : number,
        public photo: string,
        public readyForBreeding: boolean,
        public sex: string,
        public type:string ,
        public userID: number,
        public pairWith: any,
        public isReadyForBreeding?: boolean,
        public PairedWithPetID?:number
    ){};
}
