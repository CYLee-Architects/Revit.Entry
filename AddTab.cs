using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

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
        private List<PushButton> planPushButtons = new List<PushButton>();
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

            currentAssembly = Assembly.GetAssembly(GetType()).Location;
            currentDirectory = Path.GetDirectoryName(currentAssembly);

            int assemblyErrorCount = 0;
            StringBuilder assemblyErrorMsg = new StringBuilder();
            assemblyErrorMsg.AppendLine("CYLEE 輔助工具");

            assembly = "CYLee.Revit.Core.dll";

            if (!Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                assemblyErrorCount++;
                assemblyErrorMsg.AppendLine(assembly);
            }

            assembly = "CYLee.Revit.Utilities.dll";

            if (!Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                assemblyErrorCount++;
                assemblyErrorMsg.AppendLine(assembly);
            }

            if (assemblyErrorCount > 0)
            {
                assemblyErrorMsg.AppendLine("檔案損毁，請重新安裝");
                TaskDialog.Show("Error", assemblyErrorMsg.ToString());
                return Result.Failed;
            }

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Oops!!!", ex.Message);
            }

            #region ProjectInfo
            assembly = "CYLee.Revit.ProjectInfo.dll";
            panelName = "專案資訊";

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 專案資訊
                // get image
                img16 = Util.GetImageSource(Resources.ProjectInfo_16);
                img32 = Util.GetImageSource(Resources.ProjectInfo_32);

                // create button
                pbd = new PushButtonData("btnSetProjectInfoCmd", "專案資訊", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ProjectInfo.SetProjectInfoCmd")
                {
                    ToolTip = "設定專案資訊",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 專案資訊
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnSetLevelCmd", "樓層設定", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ProjectInfo.SetLevelCmd")
                {
                    ToolTip = "設定專案樓層",
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

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                PushButtonData pbd = null;
                PushButtonData pbd1 = null;
                PushButtonData pbd2 = null;
                PushButtonData pbd3 = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 檢視全棟面積概算
                // get image
                img16 = Util.GetImageSource(Resources.AreaSummaryAll_32);
                img32 = Util.GetImageSource(Resources.AreaSummaryAll_32);

                // create button
                pbd1 = new PushButtonData("btnAreaSummaryCmd", "檢視全棟" + Environment.NewLine + "面積概算", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.AreaSummaryAllCmd")
                {
                    ToolTip = "檢視全棟面積概算",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 匯出全棟面積概算
                // get image
                img16 = Util.GetImageSource(Resources.AreaToExcel_32);
                img32 = Util.GetImageSource(Resources.AreaToExcel_32);

                // create button
                pbd2 = new PushButtonData("btnExportAreaSummaryCmd", "匯出全棟" + Environment.NewLine + "面積概算", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.ExportAreaSummaryAllCmd")
                {
                    ToolTip = "匯出全棟面積概算",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                // add split button to ribbon
                SplitButtonData sb1 = new SplitButtonData("splitButton1", "Split");
                SplitButton sb = panel.AddItem(sb1) as SplitButton;
                pb = sb.AddPushButton(pbd1);
                pb = sb.AddPushButton(pbd2);


                #region 檢視當層面積概算
                img16 = Util.GetImageSource(Resources.AreaSummaryLV_32);
                img32 = Util.GetImageSource(Resources.AreaSummaryLV_32);

                pbd = new PushButtonData("btnAreaSummaryCurrentLevelCmd", "檢視當層" + Environment.NewLine + "面積概算", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.AreaSummaryCurrentLevelCmd")
                {
                    ToolTip = "檢視當層面積概算",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pbd.AvailabilityClassName = "CYLee.Revit.AreaTools.AreaSummaryCurrentLevelCmd";

                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 套用房間屬性
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnSetRoomProperties", "套用房間屬性", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.SetRoomProperties")
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
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd1 = new PushButtonData("btnRoomMatcher", "複製房間屬性", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomMatcher")
                {
                    ToolTip = "複製房間屬性到選取的房間",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 清除柱邊界
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd2 = new PushButtonData("btnClearColumnRoomBounding", "清除柱邊界", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.ClearColumnRoomBounding")
                {
                    ToolTip = "清除柱房間邊界設定",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };
                #endregion

                #region 房間面積加總
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd3 = new PushButtonData("btnSumRoomArea", "房間面積加總", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.SumRoomArea")
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
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd1 = new PushButtonData("btnRoomCleaner", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner")
                {
                    ToolTip = "清除未放置或面積為 0 的房間",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pbd2 = new PushButtonData("btnRoomCleanerx1", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner");
                pbd3 = new PushButtonData("btnRoomCleanerx2", "清除房間", Path.Combine(currentDirectory, assembly), "CYLee.Revit.AreaTools.RoomCleaner");

                ribbonItems = panel.AddStackedItems(pbd1, pbd2, pbd3);
                ribbonItems[1].Visible = false;
                ribbonItems[2].Visible = false;
                #endregion
            }
            #endregion

            #region EscapePath
            assembly = "CYLee.Revit.EscapePath.dll";
            panelName = "步行距離";

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 繪製步距線段
                // get image
                img16 = Util.GetImageSource(Resources.EscapePathDraw_16);
                img32 = Util.GetImageSource(Resources.EscapePathDraw_32);

                // create button
                pbd = new PushButtonData("btnDrawPath", "繪製步距線段", Path.Combine(currentDirectory, assembly), "CYLee.Revit.EscapePath.DrawPath")
                {
                    ToolTip = "繪製步行距離檢討線段",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pb = panel.AddItem(pbd) as PushButton;
                planPushButtons.Add(pb);
                #endregion

                #region 建立步距報告
                // get image
                img16 = Util.GetImageSource(Resources.EscapePathReport_16);
                img32 = Util.GetImageSource(Resources.EscapePathReport_32);

                // create button
                pbd = new PushButtonData("btnEscapePathReport", "建立步距報告", Path.Combine(currentDirectory, assembly), "CYLee.Revit.EscapePath.EscapePathReport")
                {
                    ToolTip = "建立步行距離檢討報告",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pb = panel.AddItem(pbd) as PushButton;
                planPushButtons.Add(pb);
                #endregion
            }
            #endregion

            #region ParkingTools
            assembly = "CYLee.Revit.ParkingTools.dll";
            panelName = "停車工具";

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 車位編號
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnParkingNumbering", "車位編號", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.ParkingNumbering")
                {
                    ToolTip = "進行車位編號",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                planPushButtons.Add(pb);
                #endregion

                #region 編號標籤
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnTagParking", "編號標籤", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.TagParking")
                {
                    ToolTip = "車位編號上標籤",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                planPushButtons.Add(pb);
                #endregion

                #region 還原翻轉
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnRevertMirroredParkingElements", "還原翻轉", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ParkingTools.RevertMirroredParkingElements")
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

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 建立坡道
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnStartSetupCreateRampV2", "建立坡道", Path.Combine(currentDirectory, assembly), "CYLee.Revit.RampTools.StartSetupCreateRampV2")
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

            #region ViewTools
            assembly = "CYLee.Revit.ViewTools.dll";
            panelName = "視圖工具";

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 建立裁切範圍
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnSetupCropRegionCmd", "建立" + Environment.NewLine + "裁切範圍", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ViewTools.SetupCropRegionCmd")
                {
                    ToolTip = "依指定的圖紙尺寸、方向自動建立裁切範圍",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion

                #region 複製視圖裁切範圍
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnCopyCropRegion", "複製視圖" + Environment.NewLine + "裁切範圍", Path.Combine(currentDirectory, assembly), "CYLee.Revit.ViewTools.CopyCropRegionCmd")
                {
                    ToolTip = "將目前視圖裁切範圍套用至所有平面視圖",
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

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);

                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region Schedule 轉出 Excel
                // get image
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnExportScheduleToExcel", "Schedule" + Environment.NewLine + "轉出 Excel", Path.Combine(currentDirectory, assembly), "CYLee.Revit.MiscTools.ExportScheduleToExcel")
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
                img16 = Util.GetImageSource(Resources.Code_16);
                img32 = Util.GetImageSource(Resources.Code_32);

                // create button
                pbd = new PushButtonData("btnSumLength", "線段長度加總", Path.Combine(currentDirectory, assembly), "CYLee.Revit.MiscTools.SumLength")
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

            if (Core.AssemblyValidator.Validate(Path.Combine(currentDirectory, assembly)))
            {
                panel = CreatePanel(tabName, panelName);
                // init
                PushButtonData pbd = null;
                PushButton pb = null;
                ImageSource img16 = null;
                ImageSource img32 = null;

                #region 關於
                // get image
                img16 = Util.GetImageSource(Resources.Info_16);
                img32 = Util.GetImageSource(Resources.Info_32);

                // create button
                pbd = new PushButtonData("btnAboutCYLee", "關於", Path.Combine(currentDirectory, assembly), "CYLee.Revit.Entry.StartAbout")
                {
                    ToolTip = "",
                    LongDescription = "",
                    Image = img16,
                    LargeImage = img32,
                };

                pbd.AvailabilityClassName = "CYLee.Revit.Entry.StartAbout";

                // add button to ribbon
                pb = panel.AddItem(pbd) as PushButton;
                #endregion
            }
            #endregion

            application.ViewActivated += OnViewActivated;
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            application.ViewActivated -= OnViewActivated;
            return Result.Succeeded;
        }
        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // 取得當前視圖
            View currentView = e.CurrentActiveView;

            // 檢查視圖類型是否為平面視圖（例如：Floor Plan）
            if (currentView.ViewType.Equals(ViewType.FloorPlan) || currentView.ViewType.Equals(ViewType.AreaPlan))
            {
                // 啟用或顯示你的Ribbon按鈕
                EnableRibbonButton();
            }
            else
            {
                // 禁用或隱藏你的Ribbon按鈕
                DisableRibbonButton();
            }
        }
        private void EnableRibbonButton()
        {
            if (planPushButtons.Count > 0)
            {
                foreach (var button in planPushButtons)
                {
                    button.Enabled = true;
                }
            }
        }
        private void DisableRibbonButton()
        {
            if (planPushButtons.Count > 0)
            {
                foreach (var button in planPushButtons)
                {
                    button.Enabled = false;
                }
            }
        }
    }
}
