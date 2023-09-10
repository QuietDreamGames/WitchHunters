using System.Collections.Generic;
using Features.SaveSystems.Configs;
using Features.SaveSystems.Interfaces;
using Features.SaveSystems.Modules.PathBuilder;

namespace Features.SaveSystems.Core
{
    public class SavableData
    {
        public List<ISavableCollector> Collectors { get; }
        public SavableStorage Storage { get; }

        public SavableData(
            ISavableSerializer serializer, 
            ISavablePathBuilder pathBuilder)
        {
            Collectors = new List<ISavableCollector>();
            Storage = new SavableStorage(serializer, pathBuilder);
        }

        public void AddCollector(ISavableCollector collector)
        {
            Collectors.Add(collector);
        }
        
        public void RemoveCollector(SavableCollector collector)
        {
            Collectors.Remove(collector);
        }

        public void Save()
        {
            for (var i = 0; i < Collectors.Count; i++)
            {
                var collector = Collectors[i];
                var data = collector.Save();
                Storage.Save(collector.ID, data);
            }
        }
        
        public void Load()
        {
            for (var i = 0; i < Collectors.Count; i++)
            {
                var collector = Collectors[i];
                var data = Storage.Load(collector.ID);
                if (data == null)
                {
                    continue;
                }
                
                collector.Load(data);
            }
        }
        
        public void SaveToDisk(SaveCategory saveCategory)
        {
            Storage.SaveToDisk(saveCategory);
        }
        
        public void LoadFromDisk(SaveCategory saveCategory)
        {
            Storage.LoadFromDisk(saveCategory);
        }
        
        public void Clear(SaveCategory saveCategory)
        {
            Storage.Clear(saveCategory);
        }
    }
}