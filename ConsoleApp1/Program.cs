using ConsoleApp1.Controllers;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using ConsoleApp1.Views;
using ConsoleApp1.Models;

namespace ConsoleApp1;

public class Program
{
    private static void Main()
    {
        var choice = true;
        while (choice)
        {
            Console.WriteLine("1. List all regions");
            Console.WriteLine("2. List all countries");
            Console.WriteLine("3. List all locations");
            Console.WriteLine("4. List regions with Where");
            Console.WriteLine("5. Join tables regions and countries and locations");
            Console.WriteLine("6. Data Employee");
            Console.WriteLine("7. Department Info");
            Console.WriteLine("10. Exit");
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();
            choice = Menu(input);
        }
    }

    public static bool Menu(string input)
    {
        switch (input)
        {
            case "1":
                var region = new Region();
                var regions = region.GetAll();
                GeneralView.List(regions, "regions");
                break;
            case "2":
                var country = new Countries();
                var countries = country.GetAll();
                GeneralView.List(countries, "countries");
                break;
            case "3":
                var location = new Locations();
                var locations = location.GetAll();
                GeneralView.List(locations, "locations");
                break;
            case "4":
                var region2 = new Region();
                string input2 = Console.ReadLine();
                var result = region2.GetAll().Where(r => r.Name.Contains(input2)).ToList();
                GeneralView.List(result, "regions");
                break;
            case "5":
                var country3 = new Countries();
                var region3 = new Region();
                var location3 = new Locations();

                var getCountry = country3.GetAll();
                var getRegion = region3.GetAll();
                var getLocation = location3.GetAll();

                var resultJoin = (from r in getRegion
                                  join c in getCountry on r.Id equals c.RegionId
                                  join l in getLocation on c.Id equals l.CountryId
                                  select new RegionAndCountryVM
                                  {
                                      CountryId = c.Id,
                                      CountryName = c.Name,
                                      RegionId = r.Id,
                                      RegionName = r.Name,
                                      City = l.City
                                  }).ToList();

                var resultJoin2 = getRegion.Join(getCountry,
                                                 r => r.Id,
                                                 c => c.RegionId,
                                                 (r, c) => new { r, c })
                                           .Join(getLocation,
                                                 rc => rc.c.Id,
                                                 l => l.CountryId,
                                                 (rc, l) => new RegionAndCountryVM
                                                 {
                                                     CountryId = rc.c.Id,
                                                     CountryName = rc.c.Name,
                                                     RegionId = rc.r.Id,
                                                     RegionName = rc.r.Name,
                                                     City = l.City
                                                 }).ToList();

                /*foreach (var item in resultJoin2)
                {
                    Console.WriteLine($"{item.Id} - {item.NameRegion} - {item.NameCountry} - {item.RegionId}");
                }*/

                GeneralView.List(resultJoin2, "regions and countries");
                break;
            case "6":

                var Employee4 = new Employees();
                var Department4 = new Departments();
                var Location4 = new Locations();
                var Country4 = new Countries();
                var Region4 = new Region();

                //menyimpan list dari return method getAll ke variabel tiap tabel
                var getEmployee4 = Employee4.GetAll();
                var getDepartment4 = Department4.GetAll();
                var getLocation4 = Location4.GetAll();
                var getCountry4 = Country4.GetAll();
                var getRegion4 = Region4.GetAll();

                //menyimpan list hasil join antar tabel ke variabel dataemployee
                var employeeDetails = (from e in getEmployee4
                                       join d in getDepartment4 on e.DepartmentId equals d.Id
                                       join l in getLocation4 on d.LocationId equals l.Id
                                       join c in getCountry4 on l.CountryId equals c.Id
                                       join r in getRegion4 on c.RegionId equals r.Id
                                       select new EmployeeDetailsVM
                                       {
                                           Id = e.Id,
                                           FullName = e.FirstName + " " + e.LastName,
                                           Email = e.Email,
                                           Phone = e.PhoneNumber,
                                           DepartmentName = d.Name,
                                           StreetAddress = l.StreetAddress,
                                           CountryName = c.Name,
                                           RegionName = r.Name
                                       }).ToList();
                //menampilkan ke layar konsol menggunakan method list dari kelas generalmenu
                GeneralView.List(employeeDetails, "Data Employee");

                break;
            case "7":
                //menyimpan list dari return method getAll ke variabel tiap tabel
                var getEmployee5 = new Employees().GetAll();
                var getDepartment5 = new Departments().GetAll();

                //menyimpan list hasil join dan group by antar tabel ke variabel departmentInfo
                var departmentInfo = (from e in getEmployee5
                                      join d in getDepartment5 on e.DepartmentId equals d.Id
                                      group e by new { d.Name, e.DepartmentId } into groupED
                                      //implementasi View model 
                                      select new DataDepartmentVM
                                      {
                                          DepartmentName = groupED.Key.Name,
                                          TotalEmployee = groupED.Count(),
                                          //mencari min max salary menggunakan lambda expression 
                                          MinSalary = groupED.Min(e => e.Salary),
                                          MaxSalary = groupED.Max(e => e.Salary)
                                      }).ToList();
                //menampilkan ke layar konsol menggunakan method list dari kelas generalmenu
                GeneralView.List(departmentInfo, "Department Info");
                break;
            case "10":
                return false;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }

        return true;

    }

        public static void RegionMenu()
        {
            var region = new Region();
            var regionView = new RegionView();

            var regionController = new RegionController(region, regionView);

            var isLoop = true;
            while (isLoop)
            {
                Console.WriteLine("1. List all regions");
                Console.WriteLine("2. Insert new region");
                Console.WriteLine("3. Update region");
                Console.WriteLine("4. Delete region");
                Console.WriteLine("10. Back");
                Console.Write("Enter your choice: ");
                var input2 = Console.ReadLine();
                switch (input2)
                {
                    case "1":
                        regionController.GetAll();
                        break;
                    case "2":
                        regionController.Insert();
                        break;
                    case "3":
                        regionController.Update();
                        break;
                    case "10":
                        isLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }




