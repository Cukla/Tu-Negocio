using System;

namespace Tu_Negocio.Entities
{
    public struct Sale
    {
        public Client Client { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Name { get; set; }
        public PaymentMethod PM { get; set; }
    }
}
