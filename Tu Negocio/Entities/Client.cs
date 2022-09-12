using System;

namespace Tu_Negocio.Entities
{
    public struct Client
    {
        public string Name { get; set; }
        public string TelNum { get; set; }
        public string Dir { get; set; }
        public string City { get; set; }
        public int DNI { get; set; }
        public string Cuit { get; set; }
        public string Adi { get; set; }
        public DateTime Added { get; set; }

    }
}
