﻿using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDependency.Core;
using Common.Logging;
using Moriyama.Workflow.Interfaces.Application;
using Moriyama.Workflow.Umbraco6.Web.Extensions;

[assembly: WebResource("Moriyama.Workflow.Umbraco6.Web.Workflow.Css.Grid.css", "text/css")]
[assembly: WebResource("Moriyama.Workflow.Umbraco6.Web.Workflow.Js.Util.js", "text/javascript")]
namespace Moriyama.Workflow.Umbraco6.Web.Workflow
{
    public partial class InstantiationCriteria : UserControl
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IGlobalisationService TheGlobalisationService { get; set; }
        public IWorkflowInstantiationCriteriaService TheWorkflowInstantiationCriteriaService { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            /* if (Validator.IsTrial() || Validator.IsInvalid())
            {
                TrialLiteral.Visible = true;
                TrialLiteral.Text = string.Format("<p class='trialMode'>{0}</p>", TheGlobalisationService.GetString("trial_mode"));
            } */

            this.AddResourceToClientDependency("Moriyama.Workflow.Umbraco6.Web.Workflow.Css.Grid.css", ClientDependencyType.Css);
            this.AddResourceToClientDependency("Moriyama.Workflow.Umbraco6.Web.Workflow.Js.Util.js", ClientDependencyType.Javascript);

            CreateCriteriaButton.Text = TheGlobalisationService.GetString("create_new_criteria");
            ((ButtonField)CriteriaGridView.Columns[1]).Text = TheGlobalisationService.GetString("delete");
            
        }

        protected void WorklowCriteriaRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;
            e.Row.Cells[0].Text = TheGlobalisationService.GetString("name");
        }

        protected void CreateCriteriaButtonClick(object sender, EventArgs e)
        {

            //if (Validator.IsTrial() || Validator.IsInvalid())
            //{
            //    var numConfigs = TheWorkflowInstantiationCriteriaService.List().Count;
            //    if (numConfigs > 0)
            //    {
            //        throw new Exception(TheGlobalisationService.GetString("only_one_config_in_trial"));
            //    }
            //}

            var workflowName = TheGlobalisationService.GetString("new_instantiation_criteria");
            Log.Debug(string.Format("Creating new workflow instantiation criteria '{0}'", workflowName));
            TheWorkflowInstantiationCriteriaService.CreateWorkflowInstantiationCriteria(workflowName);
        }

        protected void WorklowCriteriaRowDeleting(object sender, GridViewDeleteEventArgs eventArgs)
        {
        }

        protected void WorklowCriteriaRowCommand(object sender, GridViewCommandEventArgs eventArgs)
        {
            var commandArgument = Convert.ToInt32(eventArgs.CommandArgument);
            var id = (int)((GridView)sender).DataKeys[commandArgument].Value;

            var cmd = eventArgs.CommandName;

            if (cmd.ToLower() != "delete") return;

            Log.Debug(string.Format("Deleting workflow criteria {0}", id));
            TheWorkflowInstantiationCriteriaService.Delete(id);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            CriteriaGridView.DataSource = TheWorkflowInstantiationCriteriaService.List();

            CriteriaGridView.DataBind();

            ((ButtonField)CriteriaGridView.Columns[1]).Text = TheGlobalisationService.GetString("delete");
        }
        
    }
}