using ConsoleApp1.Views;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Controllers
{
    public class LocationController
    {
        private Locations _location;
        private LocationView _locationView;

        public LocationController(Locations location, LocationView locationView)
        {
            _location = location;
            _locationView = locationView;
        }

        public void GetAll()
        {
            var results = _location.GetAll();
            if (!results.Any())
            {
                Console.WriteLine("No data found");
            }
            else
            {
                _locationView.List(results, "regions");
            }
        }

        public void Insert()
        {
            Locations input = _locationView.InsertInput();
            var result = _location.Insert(input);

            _locationView.Transaction(result);
        }

        public void Update()
        {
            Locations location = _locationView.UpdateInput();

            var result = _location.Update(location);
            _locationView.Transaction(result);
        }

        public void Delete()
        {
            int input = 0;
            var isTrue = true;
            while (isTrue)
            {
                try
                {
                    input = _locationView.DeleteInput();
                    isTrue = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            var result = _location.Delete(input);

            _locationView.Transaction(result);

        }
    }
}
