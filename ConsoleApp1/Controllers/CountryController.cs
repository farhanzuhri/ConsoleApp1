
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Views;
using ConsoleApp1.Models;


namespace ConsoleApp1.Controllers
{
    public class CountryController
    {
        private Countries _country;
        private CountryView _countryView;

        public CountryController(Countries country, CountryView countryView)
        {
            _country = country;
            _countryView = countryView;
        }

        public void GetAll()
        {
            var results = _country.GetAll();
            if (!results.Any())
            {
                Console.WriteLine("No data found");
            }
            else
            {
                GeneralView.List(results, "countries");
            }
        }

        public void Insert()
        {
            Countries input = _countryView.InsertInput(); ;


            var result = _country.Insert(input);

            GeneralView.Transaction(result);
        }

        public void Update()
        {
            Countries country = _countryView.UpdateInput();

            var result = _country.Update(country);
            GeneralView.Transaction(result);
        }

        public void Delete()
        {
            string input = _countryView.DeleteInput();
            var result = _country.Delete(input);

            GeneralView.Transaction(result);

        }
    }
}