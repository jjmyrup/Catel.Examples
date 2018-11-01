
namespace Catel.Examples.UWP.SavableModel.ViewModels
{
    using global::Windows.Storage;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel.Examples.UWP.SavableModel.Models;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Runtime.Serialization;

    public class MainViewModel : ViewModelBase
    {
        private readonly ISerializer _serializer;
        private StorageFile _storagefile;

        public TaskCommand SaveCommand { get; private set; }

        public MainViewModel(ISerializer serializer)
        {
            _serializer = serializer;

            SaveCommand = new TaskCommand(
                async () =>
                {
                    using (var stream = await _storagefile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        Person.Save(stream, _serializer);
                    }
                },
                () => Person != null && Person.IsDirty);
        }

        protected override async Task InitializeAsync()
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            _storagefile =
                await storageFolder.CreateFileAsync("person", CreationCollisionOption.OpenIfExists);

            using (var stream = await _storagefile.OpenAsync(FileAccessMode.Read))
            {
                try
                {
                    Person = Person.Load(stream, _serializer);
                }
                catch
                {
                    Person = new Person();
                }
            }
        }

        [Model]
        [Expose("FirstName", IsReadOnly = false)]
        [Expose("MiddleName", IsReadOnly = false)]
        [Expose("LastName", IsReadOnly = false)]
        public Person Person { get; set; }
    }
}
