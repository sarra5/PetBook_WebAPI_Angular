export class RequestBreed {
    constructor(
       public  senderPetName:string,
       public receiverPetName:string ,
       public ownersenderName :string ,
       public ownerreceiverName :string ,
       public petIDSender:number ,
       public petIDReceiver :number ,
       public pair :number
    ){}
}
