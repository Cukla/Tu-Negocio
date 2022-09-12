using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tu_Negocio.Entities;

namespace Tu_Negocio
{
    public class AppViewModel : BaseBind
    {
        public AppViewModel()
        {
            // ...
        }

        // All common app data
        private Business selectedBusiness = new Business { Data = new BusinessData(), Inventory = new List<Product>() };
        public Business SelectedBusiness
        {
            get { return selectedBusiness; }
            set { SetProperty(ref selectedBusiness, value); }
        }

        private Client selectedClient = new Client();
        public Client SelectedClient
        {
            get { return selectedClient; }
            set { SetProperty(ref selectedClient, value); }
        }
        /*
        public Business DerivedSelectedBusiness => "return something based on SampleCommonString";
        
                public String SampleDerivedProperty2
                {
                    get
                    {
                        //<< evaluate SampleCommonString >>
                        return "Same thing as SampleDerivedProperty1, but more explicit";
                    }
                }
        */
        // This is a property that you can use for functions and internal logic… but it CAN'T be binded
        public String SampleNOTBindableProperty { get; set; }

        public void SampleFunction()
        {
            // Insert code here.

            // The function has to be with NO parameters, in order to work with simple {x:Bind} markup.
            // If your function has to access some specific data, you can create a new bindable (or non) property, just as the ones above, and memorize the data there.
        }
    }
}
