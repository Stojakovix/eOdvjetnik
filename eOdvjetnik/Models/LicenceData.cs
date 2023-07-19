using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class Device
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string hwid { get; set; }
        public int licence_active { get; set; }
        public int device_type_id { get; set; }
        public string opis { get; set; }
    }

    public class DevicesHasLicence
    {
        public int devices_id { get; set; }
        public int licences_id { get; set; }
    }

    public class Licence
    {
        public int id { get; set; }
        public int licence_type_id { get; set; }
        public DateTime expire_date { get; set; }
        public DateTime activation_date { get; set; }
        public int company_id { get; set; }
        public object automatic_renewal { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public string OIB { get; set; }
        public string adresa { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string fax { get; set; }
        public string mobitel { get; set; }
        public int user_id { get; set; }
    }

    public class LicenceType
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public int cijena { get; set; }
    }

    public class RootObject
    {
        public List<Device> Devices { get; set; }
        public List<DevicesHasLicence> DevicesHasLicences { get; set; }
        public List<Licence> Licences { get; set; }
        public List<Company> Companies { get; set; }
        public List<LicenceType> LicenceTypes { get; set; }
    }
}
