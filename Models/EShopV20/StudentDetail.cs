using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class StudentDetail
{
    public Guid Id { get; set; }
    public Guid StudentID { get; set; }
    public DateTime Date { get; set; }
    public string Study { get; set; }
    public string Violate { get; set; }
}