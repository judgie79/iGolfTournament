using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class MapViewModel
    {
        private const string APIKEY = "AIzaSyDP3UjjVX0CSS_zoYB5-axnBf-ZGvIc1BA";

        public MapViewModel(Address address)
            : this(address, APIKEY)
        {
            
        }

        public MapViewModel(string apiKey)
            : this(new Address(), apiKey)
        {

        }

        public MapViewModel(Address address, string apiKey)
        {
            this.ApiKey = apiKey;
            this.Address = address;

            ShowMap = Address != null && (!string.IsNullOrWhiteSpace(Address.City) || !string.IsNullOrWhiteSpace(Address.Zip));
        }

        public MapViewModel()
            : this(new Address(), APIKEY)
        {

        }

        public Address Address { get; set; }

        public string ApiKey { get; set; }

        public bool ShowMap { get; set; }
    }
}