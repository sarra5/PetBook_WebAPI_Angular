﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetBooK.DAL.Models;

[Table("Vaccine")]
public partial class Vaccine
{
    [Key]
    public int VaccineID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string Description { get; set; }

    [InverseProperty("Vaccine")]
    public virtual ICollection<Reservation_For_Vaccine> Reservation_For_Vaccines { get; set; } = new List<Reservation_For_Vaccine>();

    [InverseProperty("Vaccine")]
    public virtual ICollection<Vaccine_Clinic> Vaccine_Clinics { get; set; } = new List<Vaccine_Clinic>();

    [InverseProperty("Vaccine")]
    public virtual ICollection<Vaccine_Pet> Vaccine_Pets { get; set; } = new List<Vaccine_Pet>();
}