using System;
using System.Reflection;

namespace Rs.App.DotNet.WebApi.Delegates.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}