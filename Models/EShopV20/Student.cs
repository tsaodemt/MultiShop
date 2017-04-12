using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Student
{
    public Guid StudentID { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string Blood { get; set; }

    public DateTime DateOfBirth { get; set; }

    public System.DateTime JoinDate { get; set; }
    public string Note { get; set; }
    public DateTime NoteDate { get; set; }
    public Guid NoteBy { get; set; }
    public DateTime PassDate { get; set; }
    public string Hometown { get; set; }
    public string Passport { get; set; }
    public string Syndication { get; set; }
    public string ReceivingCompany { get; set; }
    public DateTime ExitDate { get; set; }
    public bool Status { get; set; }
}