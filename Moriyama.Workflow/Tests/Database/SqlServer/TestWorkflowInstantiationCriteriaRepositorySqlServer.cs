﻿using System;
using Moriyama.Workflow.Domain;
using Moriyama.Workflow.Infrastructure;
using Moriyama.Workflow.Infrastructure.DatabaseHelper;
using Moriyama.Workflow.Infrastructure.DatabaseHelper.Factory;
using Moriyama.Workflow.Interfaces.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moriyama.Workflow.Tests.Database.SqlServer
{
    [TestClass]
    public class TestWorkflowInstantiationCriteriaRepositorySqlServer
    {
        public TestWorkflowInstantiationCriteriaRepositorySqlServer()
        {
            WorkflowConfigurationRepository.Instance.Storage = TempFileStorage.Instance;
            WorkflowConfigurationRepository.Instance.DatabaseHelper =
                DatabaseHelperFactory.Instance.CreateDatabaseHelper(
                    "Moriyama.Workflow.Infrastructure.DatabaseHelper.SqlServerDatabaseHelper, Moriyama.Workflow.Infrastructure");

            ((SqlServerDatabaseHelper)WorkflowConfigurationRepository.Instance.DatabaseHelper).ConnectionStringProvider
                = new SqlServerConnectionStringProvider();
        }

        [TestMethod]
        public void TestCreate()
        {
            var r = WorkflowInstantiationCriteriaRepository.Instance;

            var a = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "What the fuck!" };
            
            r.Create(a);
        }

        [TestMethod]
        public void TestRetrieve()
        {
            var r = WorkflowInstantiationCriteriaRepository.Instance;

            var a = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "What the fuck!" };

            r.Create(a);
            var id = a.Id;
            a = null;

            var b = r.GetById(id);
            b = r.RestoreState(b);

            Assert.IsNotNull(b);
        }

        [TestMethod]
        public void TestList()
        {
            var r = WorkflowInstantiationCriteriaRepository.Instance;
            var a = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "Get Me" };

            var b = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "Get Me Too" };
            // b.StartTask = new EndWorkflowTask();

            //r.Save(a);
            //r.Save(b);

            var id = a.Id;
            a = null;

            var y = r.List();
            foreach (var item in y)
            {
                Console.WriteLine(item.Name);
            }
            Assert.IsNotNull(y);
        }

        [TestMethod]
        public void TestDelete()
        {
            var r = WorkflowInstantiationCriteriaRepository.Instance;
            var a = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "Delete me" };

            r.Create(a);

            r.Delete(a);
        }


        [TestMethod]
        public void TestSave()
        {
            var r = WorkflowInstantiationCriteriaRepository.Instance;
            var a = (IWorkflowInstantiationCriteria)new WorkflowInstantiationCriteria { Name = "What the fuck!" };

            r.Create(a);

            a.Name = "the bomb";
            r.Update(a);

            
            r.Update(a);
        }

    }
}
