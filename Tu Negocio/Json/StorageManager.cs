using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Tu_Negocio.Entities;

namespace Tu_Negocio.Json
{
    internal class StorageManager
    {
        string BussinesName = String.Empty;
        string BusinessPath = String.Empty;
        string DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string ClientsPath = String.Empty;
        string SupplierPath = String.Empty;
        JsonSerializer serializer = new JsonSerializer();

        public StorageManager(string BussinesName)
        {
            this.BussinesName = BussinesName;
            BusinessPath = DocPath + $@"\Tu Negocio\Business\";
            ClientsPath = DocPath + $@"\Tu Negocio\{BussinesName}\Clients\";
            SupplierPath = DocPath + $@"\Tu Negocio\{BussinesName}\Suppliers\";
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

        public void SaveBussines(BusinessData bus)
        {
            string output = JsonConvert.SerializeObject(bus);
            CheckIfFolderExist(BusinessPath);
            File.WriteAllText($@"{BusinessPath}{bus.Name}.json", output);
        }
        #region Business
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
            DirectoryInfo d = new DirectoryInfo(path);
            List<string> FileNames = new List<string>();
            FileInfo[] Files = d.GetFiles(type);

            foreach (FileInfo file in Files)
                FileNames.Add(file.FullName);

            return FileNames;
        }

        #endregion
    }
}
