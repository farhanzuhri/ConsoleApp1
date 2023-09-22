using System;
using System.Collections.Generic;
using System.Linq; 
using ConsoleApp1.Views;
using ConsoleApp1.Models;

namespace ConsoleApp1.Controllers
{
    public class RegionController
    {
        private Region _region;
        private RegionView _regionView;

        public RegionController(Region region, RegionView regionView)
        {
            _region = region;
            _regionView = regionView;
        }

        public void GetAll()
        {
            
            var results = _region.GetAll();
            if (!results.Any())
            {
                Console.WriteLine("No data found");
            }
            else
            {
                GeneralView.List(results, "regions"); 
            }
        }

        public void Insert()
        {
            
            string input = _regionView.InsertInput();
            var result = _region.Insert(input);
            GeneralView.Transaction(result); 
        }

        public void Update()
        {
            
            var region = _regionView.UpdateRegion();
            var result = _region.Update(region.Id, region.Name);    
            GeneralView.Transaction(result); 
        }
    }
}
