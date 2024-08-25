export class RequestForBreed {
    constructor(

    public senderPetName: string,
    public receiverPetName: string,
    public petIDSender: number,
    public petIDReceiver: number,
    public pair: boolean
    ){}
}
