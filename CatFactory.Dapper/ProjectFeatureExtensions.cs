﻿using CatFactory.CodeFactory.Scaffolding;

namespace CatFactory.Dapper
{
    public static class ProjectFeatureExtensions
    {
        public static DapperProject GetDapperProject(this ProjectFeature<DapperProjectSettings> projectFeature)
            => projectFeature.Project as DapperProject;
    }
}
