using System;
using System.Collections.Generic;

public partial class Student
{
    public Guid StudentID { get; set; }
    public string StudentCode { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public System.DateTime DateOfBirth { get; set; }
    public System.DateTime JoinDate { get; set; }
    public string Note { get; set; }
    public System.DateTime NoteDate { get; set; }
    public Guid NoteBy { get; set; }
}