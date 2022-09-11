using System;

namespace Tu_Negocio.Entities
{
    public struct Expenditure
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Name { get; set; }
        public Supplier supplier { get; set; }
        public PaymentMethod PM { get; set; }
    }
}
