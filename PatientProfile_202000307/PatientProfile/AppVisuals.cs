using Spotfire.Dxp.Application;
using Spotfire.Dxp.Application.Visuals;
using Spotfire.Dxp.Data;
using System.Linq;
using PerkinElmer.Signals.Analytics.Components;

namespace PatientProfile
{
    public partial class App : BaseComponentsApp
    {
        //3.2.0 SelectSubject 步骤中的 SelectSubject01 图
        [VisualizationName("SelectSubject01")]
        public Visual CreatePatientProfileZero()
        {
            var tablePlot = Page.Visuals.AddNew<TablePlot>();
            tablePlot.AutoConfigure();

            tablePlot.Title = "Select Subjects";
            tablePlot.Data.DataTableReference = App.Document.Data.Tables["dm"];
            tablePlot.Data.Filterings.Add(App.Document.Data.Markings["Population Subjects"]);
            tablePlot.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowAll;
            tablePlot.Data.MarkingReference = App.Document.Data.Markings["PatientProfile00_App"];
            tablePlot.TableColumns.Clear();
            tablePlot.TableColumns.Add(App.Document.Data.Tables["dm"].Columns["Subject name or identifier"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["dm"].Columns["Sex"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["dm"].Columns["Age"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["dm"].Columns["Race"]);
            tablePlot.Legend.Visible = false;
            return tablePlot.Visual;
        }


        //3.2.1 PatientProfile 步骤中的 PatientProfile 01 条形图
        [VisualizationName("PatientProfile01")]
        public Visual CreatePatientProfileFirst()
        {
            var scatterPlot = Page.Visuals.AddNew<ScatterPlot>();
            scatterPlot.AutoConfigure();

            scatterPlot.Title = "Patient Profile";
            scatterPlot.Data.Filterings.Add(App.Document.Data.Markings["PatientProfile00_App"]);
            scatterPlot.Data.MarkingReference = App.Document.Data.Markings["PatientProfile01_App"];
            scatterPlot.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowAll;
            scatterPlot.Data.DataTableReference = App.Document.Data.Tables["PatientProfile"];
            scatterPlot.XAxis.Expression = "[Day in Study]";
            scatterPlot.YAxis.Expression = "<[Event Type] NEST [TmpEventNew] as [Tm] NEST [Event_new]>";
            scatterPlot.ColorAxis.Expression = "<[Color Flag]>";
            scatterPlot.ColorAxis.Coloring.Apply("PP");

           
       

            scatterPlot.ShapeAxis.Expression = "<[Shape Flag]>";
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("End")] = new MarkerShape(MarkerType.TriangleLeft);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("Lab :")] = new MarkerShape(MarkerType.StarFour);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("Lab H")] = new MarkerShape(MarkerType.TriangleUp);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("Lab L")] = new MarkerShape(MarkerType.TriangleDown);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("Lab N")] = new MarkerShape(MarkerType.StarFour);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey("Start")] = new MarkerShape(MarkerType.TriangleRight);
            scatterPlot.ShapeAxis.ShapeMap[new CategoryKey(null)] = new MarkerShape(MarkerType.StarFive);
            //y轴上用utl图像来隐藏
            scatterPlot.YAxis.ScaleLabels.SetLevelSettings("Tm", Spotfire.Dxp.Application.Visuals.ValueRenderers.ValueRendererTypeIdentifiers.ImageFromUrlRenderer);
            scatterPlot.YAxis.ScaleLabels["Tm"].Size=1;
            
            scatterPlot.LineConnection.ConnectionAxis.Expression = "<[Event Type] NEST [Join ID]>";
            scatterPlot.XAxis.ManualZoom = true;
            scatterPlot.YAxis.ManualZoom = true;
            scatterPlot.Legend.Visible = true;

            foreach( var eachL in scatterPlot.Legend.Items)
            {
                if (eachL.Title != "颜色依据")
                {
                    eachL.Visible = false;
                }
            }

            return scatterPlot.Visual;
        }


        private void DrawingColor(ScatterPlot sp, string colorName, string columnName)
        {
            var colorP = sp.ColorAxis.Coloring.AddCategoricalColorRule();
            System.Drawing.Color colour = System.Drawing.ColorTranslator.FromHtml(colorName);
            colorP[columnName] = colour;

        }

        //3.2.2 PatientProfile 步骤中的 PatientProfile 02条形图
        [VisualizationName("PatientProfile02")]
        public Visual CreatePatientProfileSecond()
        {
            var tablePlot = Page.Visuals.AddNew<TablePlot>();
            tablePlot.AutoConfigure();

            tablePlot.Title = "Subject Details";
            tablePlot.Data.DataTableReference = App.Document.Data.Tables["PatientProfile"];
            tablePlot.Data.Filterings.Add(App.Document.Data.Markings["PatientProfile01_App"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Join ID"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Event_new"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Event Detail"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Unique Subject Identifier"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Event Type"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Day in Study"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["Start Date"]);
            tablePlot.TableColumns.Add(App.Document.Data.Tables["PatientProfile"].Columns["End Date"]);
            tablePlot.Legend.Visible = false;
            return tablePlot.Visual;
        }

        //3.2.3 PatientProfile 步骤中的 PatientProfile 03条形图
        [VisualizationName("PatientProfile03")]
        public Visual CreatePatientProfileThird()
        {
            var scatterPlot = Page.Visuals.AddNew<ScatterPlot>();
            scatterPlot.AutoConfigure();

            scatterPlot.Title = "AEGradeChangeDetail";
            scatterPlot.Data.Filterings.Add(App.Document.Data.Markings["PatientProfile01_App"]);
            scatterPlot.Data.DataTableReference = App.Document.Data.Tables["AEGradeChangeDetailCro"];
            scatterPlot.Data.WhereClauseExpression = "[CTCAE Grade at the start of AE] is not null";
            scatterPlot.Data.MarkingCombinationMethod = DataSelectionCombinationMethod.Union;
            scatterPlot.XAxis.Expression = "[Day]";
            scatterPlot.YAxis.Expression = "<[CTCAE Grade at the start of AE]>";
            scatterPlot.ColorAxis.Expression = "<[CTCAE Grade at the start of AE]>";
            scatterPlot.ColorAxis.Coloring.AddCategoricalColorRule();
            scatterPlot.SizeAxis.Expression = "Integer(Split([CTCAE Grade at the start of AE],\" \",2))";
            scatterPlot.LineConnection.ConnectionAxis.Expression = "<[Subject name or identifier]>";
            scatterPlot.ShapeAxis.Expression = "<[DaysClass]>";
            scatterPlot.Legend.Visible = false;
            scatterPlot.Trellis.TrellisMode = TrellisMode.Panels;
            scatterPlot.Trellis.PanelAxis.Expression = "<[AdverseEventTerm]>";
            scatterPlot.Trellis.ManualRowCount = 1;
            scatterPlot.Trellis.ManualColumnCount = 1;
            
            //scatterPlot.Trellis.ManualLayout = 
            return scatterPlot.Visual;
        }
    }
}
