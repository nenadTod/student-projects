using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models
{
    public class SearchFilter
    {
        VehicleFilterModel vehicleFilter;
        public SearchFilter(VehicleFilterModel vehicleFilter)
        {
            this.vehicleFilter = vehicleFilter;
        }
        public bool CheckVehicle(Vehicle vehicle)
        {
            return
                (vehicleFilter.Manufacturer == "" || vehicleFilter.Manufacturer == vehicle.Manufacturer) &&
                (vehicleFilter.Model == "" || vehicleFilter.Model == vehicle.Model) &&
                (vehicleFilter.YearOfProduction == 0 || vehicleFilter.YearOfProduction == vehicle.YearOfProduction) &&
                (vehicleFilter.TypeId == 0 || vehicleFilter.TypeId == vehicle.TypeId);
        }

        public bool CheckVehiclePrice(VehicleDTO vehicle)
        {
            return
                (vehicleFilter.MaxPrice == 0 || vehicleFilter.MaxPrice >= vehicle.PricePerHour) &&
                (vehicleFilter.MinPrice == 0 || vehicleFilter.MinPrice <= vehicle.PricePerHour);
        }
    }
}