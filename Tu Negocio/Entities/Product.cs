using Microsoft.UI.Xaml.Controls;

namespace Tu_Negocio.Entities
{
    public struct Product
    {
        public Image Image { get; set; }
        public int ID { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Atributes { get; set; }
        public int Amount { get; set; }
        public string FileName
        {
            get
            {
                return $"{Name}_{ID}.txt";
            }
        }
    }
}
