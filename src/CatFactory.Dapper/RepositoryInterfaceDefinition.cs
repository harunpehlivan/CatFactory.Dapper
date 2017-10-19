﻿using CatFactory.DotNetCore;

namespace CatFactory.Dapper
{
    public static class RepositoryInterfaceDefinition
    {
        public static CSharpInterfaceDefinition GetRepositoryInterfaceDefinition(this DapperProject project)
        {
            return new CSharpInterfaceDefinition
            {
                Namespace = project.GetDataLayerContractsNamespace(),
                Name = "IRepository"
            };
        }
    }
}
