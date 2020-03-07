using System;
using System.Runtime.Serialization;
using PerkinElmer.Signals.Analytics.AppCommon;
using PerkinElmer.Signals.Analytics.Components;
using Spotfire.Dxp.Application.Visuals;
using Spotfire.Dxp.Framework.Persistence;
using Spotfire.Dxp.Application;
using Spotfire.Dxp.Data;

namespace PatientProfile
{
    [Serializable]
    [PersistenceVersion(1, 0)]
    [AppMetadata("PatientProfile", "PatientProfile")]
    public partial class App : BaseComponentsApp
    {

        #region Constructors
        public App(string name, string jsonAppStore)
            : base(name, jsonAppStore)
        {

        }
        protected App(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion

        #region UserMethods
        public override void ViewController()
        {
            // Configure here all the watchers you need for the app

            Scope["visualizationLayout"] = "PatientProfileLayout";

            Scope.Watch("FilterLab.scope.value", (oldValue, newValue)=>
            {
                if (oldValue == newValue)
                    return;

                if (Scope["FilterLab.scope.value"] == true)
                {
                    App.Document.Properties["PatientProfileReports"] = "Filter Out Normal Lab Values";
                }
                else
                {
                    App.Document.Properties["PatientProfileReports"] = "Show All Lab Values	Show All Lab Values";
                }
            });

            Scope.Watch("ColumnValue.value", (oldValue, newValue) =>
            {
                if (oldValue == newValue)
                    return;

                foreach (var v in Page.Visuals)
                {
                    if (v.Title == "Patient Profile")
                    {
                        var Vis = v.As<ScatterPlot>();
                        if (Scope["ColumnValue"].ToString() !="") {
                            Vis.Data.WhereClauseExpression = "Integer([Unique Subject Identifier])=" + Scope["ColumnValue"].ToString();
                        }
                        else
                        {
                            Vis.Data.WhereClauseExpression = "";
                        }
                    }
                }

            });

            // 监听PatientProfile 中添加筛选的按钮
            Scope.On("ShowPPFilter.scope.click", (argument) =>
            {
                OnAddPPFilter();
            });
            // 监听PatientProfile 中收起筛选的按钮
            Scope.On("RollPPFilter.scope.click", (argument) =>
            {
                OnRollPPFilter();
            });

        }
        #endregion

        // 监听PatientProfile 中添加筛选的按钮
        private void OnAddPPFilter()
        {
            Scope["DesignFormFilter.scope.show"] = true;
            Scope["ShowPPFilter.scope.show"] = false;
            Scope["RollPPFilter.scope.show"] = true;
        }
        // 监听PatientProfile 中收起筛选的按钮
        private void OnRollPPFilter()
        {
            Scope["DesignFormFilter.scope.show"] = false;
            Scope["ShowPPFilter.scope.show"] = true;
            Scope["RollPPFilter.scope.show"] = false;
        }

    }
}
