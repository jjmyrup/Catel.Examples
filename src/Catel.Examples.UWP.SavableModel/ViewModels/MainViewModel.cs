
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

        public string StorageFilePath { get; private set; }
        public TaskCommand SaveCommand { get; private set; }

        public MainViewModel(ISerializer serializer)
        {
            Argument.IsNotNull(() => serializer);

            _serializer = serializer;

            SaveCommand = new TaskCommand(
                async () =>
                {
                    try
                    {
                        using (var stream = await _storagefile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            Person.Save(stream, serializer: _serializer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Status = ex.ToString();
                    }
                },
                () => Person != null && Person.IsDirty);
        }

        protected override async Task InitializeAsync()
        {
            try
            {
                StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

                _storagefile =
                    await storageFolder.CreateFileAsync("person", CreationCollisionOption.OpenIfExists);

                StorageFilePath = _storagefile.Path;

                using (var stream = await _storagefile.OpenAsync(FileAccessMode.Read))
                {
                        Person = Person.Load(stream, serializer: _serializer);
                }
            }
            catch (Exception ex)
            {
                Person = new Person();

                Status = ex.ToString();
            }
        }

        [Model]
        [Expose("FirstName", IsReadOnly = false)]
        [Expose("MiddleName", IsReadOnly = false)]
        [Expose("LastName", IsReadOnly = false)]
        public Person Person { get; set; }
        public string Status { get; private set; }
    }
}
