using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;
public class EmployeeDetailsVM
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public decimal Salary { get; set; }
    public string DepartmentName { get; set; }
    public string StreetAddress { get; set; }
    public string CountryName { get; set; }
    public string RegionName { get; set; }

    public override string ToString()
    {
        return $"{Id} - {FullName} - {Email} - {Phone} - {Salary} - {DepartmentName} - {StreetAddress} - {CountryName} - {RegionName}";
    }
}
