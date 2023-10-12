using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CYLee.Revit.Entry
{
    [Transaction(TransactionMode.Manual)]
    public class AddTab : IExternalApplication
    {
        private UIControlledApplication mApp;
        private const string tabName = " CYLee ";
        private string panelName = "";
        private string assembly = "";
        private string currentAssembly = "";
        private string currentDirectory = "";
        private RibbonPanel CreatePanel(string tab, string panel)
        {
            foreach (RibbonPanel rp in mApp.GetRibbonPanels(tab))
            {
                if (rp.Name.Equals(panel))
                {
                    return rp;
                }
            }
            return mApp.CreateRibbonPanel(tab, panel); ;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            mApp = application;
            RibbonPanel panel = null;

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Oops!!!", ex.Message);
            }

            currentAssembly = Assembly.GetAssembly(GetType()).Location;
            currentDirectory = Path.GetDirectoryName(currentAssembly);

            #region ProjectInfo
            assembly = "CYLee.Revit.ProjectInfo.dll";
            panelName = "專案資訊";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 專案資訊
                // get image
                img16 = Util.GetImageSource(Resources.ProjectInfo16);
                img32 = Util.GetImageSource(Resources.ProjectInfo32);

                // create button
                pbd = new PushButtonData("cmdStartSetupProjectInfo", "專案資訊", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ProjectInfo.StartSetupProjectInfo")
                {
                    ToolTip = "設定專案資訊",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            #region AreaTools
            assembly = "CYLee.Revit.AreaTools.dll";
            panelName = "面積工具";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButtonData pbd1 = null;
                PushButtonData pbd2 = null;
                PushButtonData pbd3 = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 面積表
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdGenerateAreaSummarySheet", "面積表", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.GenerateAreaSummarySheet")
                {
                    ToolTip = "檢視面積表",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 套用房間屬性
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdSetRoomProperties", "套用房間屬性", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.SetRoomProperties")
                {
                    ToolTip = "所有房間套用公設比、分戶陽台面積",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 複製房間屬性
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd1 = new PushButtonData("cmdRoomMatcher", "複製房間屬性", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomMatcher")
                {
                    ToolTip = "複製房間屬性到選取的房間",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 清除柱邊界
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd2 = new PushButtonData("cmdClearColumnRoomBounding", "清除柱邊界", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.ClearColumnRoomBounding")
                {
                    ToolTip = "清除柱房間邊界設定",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 房間面積加總
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd3 = new PushButtonData("cmdSumRoomArea", "房間面積加總", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.SumRoomArea")
                {
                    ToolTip = "選擇房間後計算總面積",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                // add button to ribbon
                var ribbonItems = panel.AddStackedItems(pbd1, pbd2, pbd3);

                #region 清除房間
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd1 = new PushButtonData("cmdRoomCleaner", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner")
                {
                    ToolTip = "清除未放置或面積為 0 的房間",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pbd2 = new PushButtonData("cmdRoomCleanerx1", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner");
                pbd3 = new PushButtonData("cmdRoomCleanerx2", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner");

                ribbonItems = panel.AddStackedItems(pbd1, pbd2, pbd3);
                ribbonItems[1].Visible = false;
                ribbonItems[2].Visible = false;
                #endregion
            }
            #endregion

            #region EscapePath
            assembly = "CYLee.Revit.EscapePath.dll";
            panelName = "步行距離";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd1 = null;
                PushButtonData pbd2 = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 繪製步距線段
                // get image
                img16 = Util.GetImageSource(Resources.EscapePathDraw16);
                img32 = Util.GetImageSource(Resources.EscapePathDraw32);

                // create button
                pbd1 = new PushButtonData("cmdDrawPath", "繪製步距線段", Path.Combine(currentDirectory, assembly), "CYLee.Revit.EscapePath.DrawPath")
                {
                    ToolTip = "繪製步行距離檢討線段",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 建立步距報告
                // get image
                img16 = Util.GetImageSource(Resources.EscapePathReport16);
                img32 = Util.GetImageSource(Resources.EscapePathReport32);

                // create button
                pbd2 = new PushButtonData("cmdEscapePathReport", "建立步距報告", Path.Combine(currentDirectory, assembly), "CYLee.Revit.EscapePath.EscapePathReport")
                {
                    ToolTip = "建立步行距離檢討報告",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                // add split button to ribbon
                SplitButtonData sb1 = new SplitButtonData("splitButton1", "Split");
                SplitButton sb = panel.AddItem(sb1) as SplitButton;
                pb = sb.AddPushButton(pbd1);
                sb.AddPushButton(pbd2);
            }
            #endregion

            #region ParkingTools
            assembly = "CYLee.Revit.ParkingTools.dll";
            panelName = "停車工具";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 車位編號
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdParkingNumbering", "車位編號", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.ParkingNumbering")
                {
                    ToolTip = "進行車位編號",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 編號標籤
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdTagParking", "編號標籤", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.TagParking")
                {
                    ToolTip = "車位編號上標籤",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 還原翻轉
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdRevertMirroredParkingElements", "還原翻轉", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.RevertMirroredParkingElements")
                {
                    ToolTip = "將被翻轉過的車位翻轉回來",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            #region RampTools
            assembly = "CYLee.Revit.RampTools.dll";
            panelName = "坡道工具";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 建立坡道
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdStartSetupCreateRamp", "建立坡道", Path.Combine(currentDirectory, assembly), "CYLee.Revit.RampTools.StartSetupCreateRamp")
                {
                    ToolTip = "選定中心線後，依指定寬度建立坡道",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            #region MiscTools
            assembly = "CYLee.Revit.MiscTools.dll";
            panelName = "其它工具";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 複製視圖裁切範圍
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdCopyCropRegion", "複製視圖" + Environment.NewLine + "裁切範圍", Path.Combine(currentDirectory, assembly), "CYLee.Revit.MiscTools.CopyCropRegion")
                {
                    ToolTip = "將目前視圖裁切範圍套用至所有平面視圖",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region Schedule 轉出 Excel
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdExportScheduleToExcel", "Schedule" + Environment.NewLine + "轉出 Excel", Path.Combine(currentDirectory, assembly), "CYLee.Revit.MiscTools.ExportScheduleToExcel")
                {
                    ToolTip = "將目前 Schedule 匯出為 Excel 檔",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;

                #endregion

                #region 線段長度加總
                // get image
                img16 = Util.GetImageSource(Resources.Code16);
                img32 = Util.GetImageSource(Resources.Code32);

                // create button
                pbd = new PushButtonData("cmdSumLength", "線段長度加總", Path.Combine(currentDirectory, assembly), "CYLee.Revit.MiscTools.SumLength")
                {
                    ToolTip = "選擇線段後計算總長度",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            #region Entry About
            assembly = "CYLee.Revit.Entry.dll";
            panelName = "說明";

            if (File.Exists(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);
                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 關於
                // get image
                img16 = Util.GetImageSource(Resources.Info16);
                img32 = Util.GetImageSource(Resources.Info32);

                // create button
                pbd = new PushButtonData("cmdAboutCYLee", "關於", Path.Combine(currentDirectory, assembly), "CYLee.Revit.Entry.StartAbout")
                {
                    ToolTip = "",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
    public class Util
    {

        public static ImageSource GetImageSource(Image img)
        {
            BitmapImage bmp = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {

                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;
                bmp.EndInit();
            }

            return bmp;
        }
    }
}