﻿using CatFactory.CodeFactory;
using CatFactory.DotNetCore;

namespace CatFactory.Dapper
{
    public static class ProjectFeatureExtensions
    {
        private static ICodeNamingConvention namingConvention;

        static ProjectFeatureExtensions()
        {
            namingConvention = new DotNetNamingConvention();
        }

        public static string GetInterfaceRepositoryName(this ProjectFeature projectFeature)
            => namingConvention.GetInterfaceName(string.Format("{0}Repository", projectFeature.Name));

        public static string GetClassRepositoryName(this ProjectFeature projectFeature)
            => namingConvention.GetClassName(string.Format("{0}Repository", projectFeature.Name));

        public static DapperProject GetDapperProject(this ProjectFeature projectFeature)
            => projectFeature.Project as DapperProject;
    }
}
