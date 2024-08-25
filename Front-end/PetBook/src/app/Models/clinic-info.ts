export class ClinicInfo {
    constructor( 
        public clinicID: number,
    public name: string,
   public  rate: number,
    public bankAccount: string,
    public locations: string[],
    public phoneNumbers: string[]){}
}
