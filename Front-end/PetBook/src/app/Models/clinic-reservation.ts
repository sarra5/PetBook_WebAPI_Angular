import { ClinicPhones } from "./clinic-phones";

export class ClinicReservation {
    constructor(
    public petID: number,
    public clinicID : number,
    public clinicName: string,
    public petName: string,
    public clinicPhones: ClinicPhones[],
    public date :Date 
    )
    {}
    }
    
