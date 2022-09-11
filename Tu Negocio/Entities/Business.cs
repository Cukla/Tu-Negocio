using System;
using System.Collections.Generic;

namespace Tu_Negocio.Entities
{
    public class Business
    {
        public BusinessData Data { get; set; }
        public List<Client> Clients { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Product> Inventory { get; set; }
    }
}
