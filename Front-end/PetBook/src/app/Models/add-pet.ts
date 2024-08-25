export class AddPet {
    constructor(
    public  name: string,
    public photo: File|string|null,
    public ageInMonth: number | null,
    public  sex: string,
    public  idNoteBookImage: File|string|null,
    public  userID: number,
    public  readyForBreeding: boolean | null,
    public  type: string,
    public other: string|null){}    
}