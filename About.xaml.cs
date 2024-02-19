using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CYLee.Revit.Entry
{
    public partial class About : Window
    {
        private string currentDirectory { get; set; }

        public About()
        {
            string currentAssembly = Assembly.GetAssembly(GetType()).Location;
            currentDirectory = Path.GetDirectoryName(currentAssembly);
            string assembly = "";
            InitializeComponent();
            vVersion.Content = "軟體版本：" + GetAssembleVersion("CYLee.Revit.Entry.dll");
            vLogo.Source = Util.GetImageSource(CYLee.Revit.Entry.Resources.CYLeeLogo);
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            assembly = "CYLee.Revit.ProjectInfo.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("ProjectInfo", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.AreaTools.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("AreaTools", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.ParkingTools.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("ParkingTools", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.RampTools.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("RampTools", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.MiscTools.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("MiscTools", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.EscapePath.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("EscapePath", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.Core.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("Core", GetAssembleVersion(assembly));

            assembly = "CYLee.Revit.Utilities.dll";
            if (File.Exists(Path.Combine(currentDirectory, assembly)))
                dictionary.Add("Utilities", GetAssembleVersion(assembly));

            vModuleList.ItemsSource = dictionary;
        }
        private string GetAssembleVersion(string file)
        {
            if (File.Exists(Path.Combine(currentDirectory, file)))
            {
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(Path.Combine(currentDirectory, file));
                return fvi.FileVersion;
            }
            return string.Empty;
        }
    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class StartAbout : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            new About().ShowDialog();
            return Result.Succeeded;
        }
    }
}
