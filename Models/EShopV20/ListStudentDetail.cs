using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class ListStudentDetail
{
    public Guid StudentID { get; set; }
    public string Name { get; set; }

    public List<StudentDetail> Details { get; set; }
}