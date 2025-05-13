using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(100)]
    public string Department { get; set; }

    public decimal Salary { get; set; }
}