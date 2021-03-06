using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Interfaces;
using ProtoBuf;
using System.IO;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Storage
{
    public class SettingsStorage : ISettingsStorage
    {
        private const string DIR = "~/App_Data/Hackathon.NaN.MLBox";
        private const string MODEL = "model.bin";
        
        public void Save(Model data)
        {
            using (var file = File.Create(string.Format($"{DIR}/{MODEL}")))
            {
                Serializer.Serialize(file, data);
            }
        }

        public Model Load()
        {
            Model model = null;
            using (var file = File.OpenRead(string.Format($"{DIR}/{MODEL}")))
            {
                model = Serializer.Deserialize<Model>(file);
            }

            return model;
        }
    }
}
