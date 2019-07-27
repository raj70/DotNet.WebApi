using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rs.App.DotNet.WebApi.DelegateHandler.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}