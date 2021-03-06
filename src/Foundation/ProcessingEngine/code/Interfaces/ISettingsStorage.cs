using Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Storage;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Interfaces
{
    public interface ISettingsStorage
    {
        void Save(Model data);
        Model Load();
    }
}