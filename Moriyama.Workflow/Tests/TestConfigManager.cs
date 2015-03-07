﻿using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Moriyama.Workflow.Application;
using Moriyama.Workflow.Infrastructure;
using Moriyama.Workflow.Interfaces.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moriyama.Workflow.Tests.Classes;

namespace Moriyama.Workflow.Tests
{
    [TestClass]
    public class TestConfigManager
    {
        public TestConfigManager()
        {
            WorkflowConfigurationRepository.Instance.Storage = TempFileStorage.Instance;
            WorkflowConfigurationService.Instance.ConfigurationRepository = WorkflowConfigurationRepository.Instance;
 
        }

        [TestMethod]
        public void TestCreateConfig()
        {

            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration("This is a workflow", "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration("This is a workflow too.", "Moriyama.Workflow.Umbraco.Domain.UmbracoWorkflowConfiguration, Moriyama.Workflow.Umbraco.Domain");


           

        }


        [TestMethod]
        public void TestSetConfigProperties()
        {
            var props = new Dictionary<string, object>
                            {
                                {"IsConfigurationActive", true}, 
                                {"Name", "Spooky"}
                            };

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                WorkflowConfigurationService.Instance.SetConfigurationProperties(config.Id, props);
                WorkflowConfigurationService.Instance.SaveChanges(config.Id);
            }
        }

        [TestMethod]
        public void TestAddTask()
        {
            var configName = "TEST - " + DateTime.Now.ToString();
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration(configName, "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                if (config.Name == configName)
                {
                    IWorkflowTask t = new DummyWorkflowTask();
                    var taskId = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);
                    
                    Console.WriteLine(taskId);

                    WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                    var x = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                    x = WorkflowConfigurationRepository.Instance.RestoreState(x);

                    var s = new JavaScriptSerializer().Serialize(x);
                    Console.WriteLine(s);

                    break;
                }
            }

        }

        [TestMethod]
        public void TestAddTaskTransition()
        {
            var configName = "TEST - " + DateTime.Now.ToString();
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration(configName, "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                if (config.Name != configName) continue;

                IWorkflowTask t = new DummyWorkflowTask();
                var taskId = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);
                var taskId2 = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);

                WorkflowConfigurationService.Instance.AddTransition(config.Id, taskId, taskId2, "approve");
                
                
                WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                var x = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                x = WorkflowConfigurationRepository.Instance.RestoreState(x);

                var s = new JavaScriptSerializer().Serialize(x);
                Console.WriteLine(s);

                break;
            }
        }

        [TestMethod]
        public void TestRemoveTaskTransition()
        {
            var configName = "TEST - " + DateTime.Now.ToString();
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration(configName, "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                if (config.Name != configName) continue;

                IWorkflowTask t = new DummyWorkflowTask();
                var taskId = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);
                var taskId2 = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);

                WorkflowConfigurationService.Instance.AddTransition(config.Id, taskId, taskId2, "approve");


                WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                var x = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                x = WorkflowConfigurationRepository.Instance.RestoreState(x);

                var s = new JavaScriptSerializer().Serialize(x);
                Console.WriteLine(s);

                WorkflowConfigurationService.Instance.RemoveTransition(config.Id, taskId, "approve");
                WorkflowConfigurationService.Instance.SaveChanges(config.Id);


                var y = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                y = WorkflowConfigurationRepository.Instance.RestoreState(y);

                s = new JavaScriptSerializer().Serialize(y);
                Console.WriteLine(s);

                break;
            }
        }

        [TestMethod]
        public void TestRemoveTask()
        {
            var configName = "TEST - " + DateTime.Now.ToString();
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration(configName, "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                if (config.Name == configName)
                {
                    IWorkflowTask t = new DummyWorkflowTask();
                    var taskId = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);

                    Console.WriteLine(taskId);

                    WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                    var x = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                    x = WorkflowConfigurationRepository.Instance.RestoreState(x);

                    var s = new JavaScriptSerializer().Serialize(x);
                    Console.WriteLine(s);


                    WorkflowConfigurationService.Instance.RemoveTask(config.Id, taskId);
                    WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                    var y = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                    y = WorkflowConfigurationRepository.Instance.RestoreState(y);


                    Console.WriteLine(new JavaScriptSerializer().Serialize(y));

                    break;
                }

            }

        }


        [TestMethod]
        public void TestSetTaskProperties()
        {
            var configName = "TEST - " + DateTime.Now.ToString();
            WorkflowConfigurationService.Instance.CreateWorkflowConfiguration(configName, "Moriyama.Workflow.Domain.WorkflowConfiguration, Moriyama.Workflow.Domain");

            foreach (var config in WorkflowConfigurationService.Instance.ConfigurationRepository.List())
            {
                if (config.Name == configName)
                {
                    IWorkflowTask t = new DummyWorkflowTask();
                    var taskId = WorkflowConfigurationService.Instance.AddTask(config.Id, t.GetType().AssemblyQualifiedName, null);

                    Console.WriteLine(taskId);

                    WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                    var x = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                    x = WorkflowConfigurationRepository.Instance.RestoreState(x);

                    var s = new JavaScriptSerializer().Serialize(x);
                    Console.WriteLine(s);

                    var props = new Dictionary<string, object>
                            {
                                {"Description", "Woohoo"}, 
                                {"Name", "Spooky"}
                            };

                    WorkflowConfigurationService.Instance.SetTaskProperties(config.Id, taskId, props);
                    WorkflowConfigurationService.Instance.SaveChanges(config.Id);

                    var y = WorkflowConfigurationRepository.Instance.GetById(config.Id);
                    y = WorkflowConfigurationRepository.Instance.RestoreState(y);

                    s = new JavaScriptSerializer().Serialize(y);
                    Console.WriteLine(s);


                    break;
                }

            }

        }


    }
}
