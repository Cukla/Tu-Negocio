using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tu_Negocio.Entities;
using File = System.IO.File;

namespace Tu_Negocio.Json
{
    internal class StorageManager
    {
        public AppViewModel ViewModel => MainWindow.Current.ViewModel;

        string BussinesName = String.Empty;
        string BusinessPath = String.Empty;
        string DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string ClientsPath = String.Empty;
        string SupplierPath = String.Empty;
        string StoragePath = String.Empty;
        public StorageManager()
        {

        }

        public StorageManager(string BussinesName)
        {
            this.BussinesName = BussinesName;
            BusinessPath = DocPath + $@"\Tu Negocio\Business\";
            ClientsPath = DocPath + $@"\Tu Negocio\{BussinesName}\Clients\";
            SupplierPath = DocPath + $@"\Tu Negocio\{BussinesName}\Suppliers\";
            StoragePath = DocPath + $@"\Tu Negocio\{BussinesName}\Storage\";
        }

        public void SaveClient(Client cli)
        {
            string output = JsonConvert.SerializeObject(cli);
            CheckIfFolderExist(ClientsPath);
            File.WriteAllText($"{ClientsPath}{cli.DNI}.json", output);
        }
        public void SaveSupplier(Supplier sup)
        {
            string output = JsonConvert.SerializeObject(sup);
            CheckIfFolderExist(SupplierPath);
            File.WriteAllText($"{SupplierPath}{sup.DNI}.json", output);
        }

        public void SaveBussines()
        {
            string output = JsonConvert.SerializeObject(ViewModel.SelectedBusiness.Data);
            CheckIfFolderExist(BusinessPath);
            File.WriteAllText($@"{BusinessPath}{ViewModel.SelectedBusiness.Data.Name}.json", output);
        }
        #region Business

        public void GetSelectedBusiness(string SelectedBus)
        {
            ViewModel.SelectedBusiness.Data = new BusinessData();
            if (SelectedBus != "No Business Selected")
            {
                string jsonContent = File.ReadAllText($"{BusinessPath}/{SelectedBus}.json");
                ViewModel.SelectedBusiness.Data = JsonConvert.DeserializeObject<BusinessData>(jsonContent);
                ViewModel.SelectedBusiness.Clients = GetClientsInBusiness();
                ViewModel.SelectedBusiness.Suppliers = GetSuppliersInBusiness();
                ViewModel.SelectedBusiness.Inventory = LoadAllProducts();
            }
        }

        public List<BusinessData> ReadAllBusiness()
        {
            List<BusinessData> business = new List<BusinessData>();
            foreach (string file in GetAllFilesInDir(BusinessPath, "*.json"))
            {
                string jsonContent = File.ReadAllText(file);
                BusinessData data = JsonConvert.DeserializeObject<BusinessData>(jsonContent);
                business.Add(data);
            }
            return business;
        }

        public List<Client> GetClientsInBusiness()
        {
            List<Client> clients = new List<Client>();
            foreach (string file in GetAllFilesInDir(ClientsPath, "*.json"))
            {
                string jsonContent = File.ReadAllText(file);
                Client data = JsonConvert.DeserializeObject<Client>(jsonContent);
                clients.Add(data);
            }
            return clients;
        }

        public List<Supplier> GetSuppliersInBusiness()
        {
            List<Supplier> suppliers = new List<Supplier>();
            foreach (string file in GetAllFilesInDir(SupplierPath, "*.json"))
            {
                string jsonContent = File.ReadAllText(file);
                Supplier data = JsonConvert.DeserializeObject<Supplier>(jsonContent);
                suppliers.Add(data);
            }
            return suppliers;
        }
        #region Storage
        public void CreateStorageFile(Product pro)
        {
            CheckIfFolderExist(StoragePath);
            string content = $"{pro.ID},{pro.Name},{pro.Price},{pro.Cost},{pro.Barcode},{pro.Description},{pro.Atributes},{pro.Amount}";
            File.WriteAllText($@"{StoragePath}\{pro.Name}_{pro.ID}.csv", content);
            File.WriteAllText($@"{StoragePath}\{pro.Name}_{pro.ID}.txt", pro.Amount.ToString());
        }

        public void ModifyStorage(Product pro)
        {
            CheckIfFolderExist(StoragePath);
            File.WriteAllText($@"{StoragePath}\{pro.Name}_{pro.ID}.txt", pro.Amount.ToString());
        }

        public int LoadStorage(Product pro)
        {
            CheckIfFolderExist(StoragePath);
            return int.Parse(File.ReadAllText($@"{StoragePath}\{pro.Name}_{pro.ID}.txt"));
        }

        public List<Product> LoadAllProducts()
        {
            List<Product> pros = new List<Product>();
            foreach (string file in GetAllFilesInDirShortName(StoragePath, "*.csv"))
            {
                pros.Add(LoadProduct(file));
            }
            return pros;
        }

        public Product LoadProduct(string proName)
        {
            CheckIfFolderExist(StoragePath);
            Product pro = new Product();
            using (var reader = new StreamReader($@"{StoragePath}\{proName}"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    pro = new Product
                    {
                        ID = int.Parse(values[0]),
                        Name = values[1],
                        Price = decimal.Parse(values[2]),
                        Cost = decimal.Parse(values[3]),
                        Barcode = values[4],
                        Description = values[5],
                        Atributes = values[6]
                    };
                }

            }
            pro.Amount = int.Parse(File.ReadAllText($@"{StoragePath}\{proName.Split(".")[0]}.txt"));
            return pro;
        }
        #endregion

        #endregion
        #region Settings file
        public void CreateSettingsFile()
        {
            Settings set = new Settings { SelectedBusinessName = "No Business Selected", Categories = new List<string>() };
            string output = JsonConvert.SerializeObject(set);
            CheckIfFolderExist($"{DocPath}/Tu Negocio/");
            File.WriteAllText($"{DocPath}/Tu Negocio/settings.json", output);
        }
        public bool ReadSettingsFile(ref Settings settings)
        {
            try
            {
                string jsonContent = File.ReadAllText($"{DocPath}/Tu Negocio/settings.json");
                settings = JsonConvert.DeserializeObject<Settings>(jsonContent);
                settings.Categories = ReadCategories();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void ModifySettingsFile(ref Settings settings)
        {
            string output = JsonConvert.SerializeObject(settings);
            CheckIfFolderExist($"{DocPath}/Tu Negocio/");
            File.WriteAllText($"{DocPath}/Tu Negocio/settings.json", output);
        }

        public void WriteCategories(List<string> categories)
        {
            File.WriteAllLines($"{DocPath}/Tu Negocio/categories.txt", categories);
        }

        public List<string> ReadCategories()
        {
            return File.ReadAllLines($"{DocPath}/Tu Negocio/categories.txt").ToList();
        }
        #endregion
        #region Utility functions
        private void CheckIfFolderExist(string path)
        {
            // If directory does not exist, create it
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private List<string> GetAllFilesInDir(string path, string type)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                List<string> FileNames = new List<string>();
                FileInfo[] Files = d.GetFiles(type);

                foreach (FileInfo file in Files)
                    FileNames.Add(file.FullName);

                return FileNames;
            }
            catch { return new List<string>(); }
        }

        private List<string> GetAllFilesInDirShortName(string path, string type)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                List<string> FileNames = new List<string>();
                FileInfo[] Files = d.GetFiles(type);

                foreach (FileInfo file in Files)
                    FileNames.Add(file.Name);

                return FileNames;
            }
            catch { return new List<string>(); }
        }

        #endregion
    }
}
