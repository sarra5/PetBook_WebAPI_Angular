﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PetBooK.DAL.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Type { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}