using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioMutant.UserData
{
    public class Address
    {
        public string Street { get; set; }
        
        public string Suite { get; set; }
        
        public string City { get; set; }
       
        public string Zipcode { get; set; }
        
        public Geo Geo { get; set; }
    }
}
