namespace Catel.Examples.UWP.SavableModel.Models
{
    using Catel.Data;

    public class Person : SavableModelBase<Person>
    {
        #region Properties
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
        #endregion
    }
}
