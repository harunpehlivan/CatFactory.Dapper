﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatFactory.CodeFactory.Scaffolding;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.Dapper
{
    public static class DapperProjectExtensions
    {
        public static string GetEntityLayerDirectory(this DapperProject project)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.EntityLayer);

        public static string GetEntityLayerDirectory(this DapperProject project, string schema)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.EntityLayer, schema);

        public static string GetDataLayerDirectory(this DapperProject project)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.DataLayer);

        public static string GetDataLayerContractsDirectory(this DapperProject project)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.DataLayer, project.ProjectNamespaces.Contracts);

        public static string GetDataLayerDataContractsDirectory(this DapperProject project)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.DataLayer, project.ProjectNamespaces.DataContracts);

        public static string GetDataLayerRepositoriesDirectory(this DapperProject project)
            => Path.Combine(project.OutputDirectory, project.ProjectNamespaces.DataLayer, project.ProjectNamespaces.Repositories);

        public static IEnumerable<Column> GetInsertColumns(this DapperProject project, ITable table)
        {
            var selection = project.GetSelection(table);

            foreach (var column in table.Columns)
            {
                if (table.Identity?.Name == column.Name || selection.Settings.InsertExclusions.Contains(column.Name))
                    continue;

                yield return column;
            }
        }

        public static IEnumerable<Column> GetUpdateColumns(this DapperProject project, ITable table)
        {
            var selection = project.GetSelection(table);

            foreach (var column in table.GetColumnsWithNoPrimaryKey())
            {
                if (selection.Settings.UpdateExclusions.Contains(column.Name))
                    continue;

                yield return column;
            }
        }

        public static DapperProject GlobalSelection(this DapperProject project, Action<DapperProjectSettings> action = null)
        {
            var settings = new DapperProjectSettings();

            var selection = project.Selections.FirstOrDefault(item => item.IsGlobal);

            if (selection == null)
            {
                selection = new ProjectSelection<DapperProjectSettings>
                {
                    Pattern = ProjectSelection<DapperProjectSettings>.GlobalPattern,
                    Settings = settings
                };

                project.Selections.Add(selection);
            }
            else
            {
                settings = selection.Settings;
            }

            action?.Invoke(settings);

            return project;
        }

        public static ProjectSelection<DapperProjectSettings> GlobalSelection(this DapperProject project)
            => project.Selections.FirstOrDefault(item => item.IsGlobal);

        public static DapperProject Selection(this DapperProject project, string pattern, Action<DapperProjectSettings> action = null)
        {
            var selection = project.Selections.FirstOrDefault(item => item.Pattern == pattern);

            if (selection == null)
            {
                var globalSettings = project.GlobalSelection().Settings;

                selection = new ProjectSelection<DapperProjectSettings>
                {
                    Pattern = pattern,
                    Settings = new DapperProjectSettings
                    {
                        ForceOverwrite = globalSettings.ForceOverwrite,
                        SimplifyDataTypes = globalSettings.SimplifyDataTypes,
                        UseAutomaticPropertiesForEntities = globalSettings.UseAutomaticPropertiesForEntities,
                        EnableDataBindings = globalSettings.EnableDataBindings,
                        UseStringBuilderForQueries = globalSettings.UseStringBuilderForQueries,
                        InsertExclusions = globalSettings.InsertExclusions.Select(item => item).ToList(),
                        UpdateExclusions = globalSettings.UpdateExclusions.Select(item => item).ToList(),
                        AddPagingForGetAllOperation = globalSettings.AddPagingForGetAllOperation
                    }
                };

                project.Selections.Add(selection);
            }

            action?.Invoke(selection.Settings);

            return project;
        }

        [Obsolete("Use Selection method.")]
        public static DapperProject Select(this DapperProject project, string pattern, Action<DapperProjectSettings> action = null)
            => project.Select(pattern, action);
    }
}
