using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Reflection;
using Infragistics.Win.UltraWinGrid;

namespace SingleLineScrollGrid
{
    public class SingleLineScrollGrid : CitaviAddOn<ReferenceGridForm>
    {
        private readonly HashSet<Control> _processedGrids = new HashSet<Control>();

        public override void OnHostingFormLoaded(ReferenceGridForm referenceGridForm)
        {
            var mainGridField = referenceGridForm.GetType()
                .GetField("mainGrid", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (mainGridField != null)
            {
                var mainGrid = mainGridField.GetValue(referenceGridForm) as Control;

                if (mainGrid != null && !_processedGrids.Contains(mainGrid))
                {
                    _processedGrids.Add(mainGrid);
                    mainGrid.MouseWheel -= MainGrid_MouseWheel;
                    mainGrid.MouseWheel += MainGrid_MouseWheel;
                }
            }
        }

        private void MainGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (sender is Control gridControl)
            {
                // 1. 标记事件已处理
                ((HandledMouseEventArgs)e).Handled = true;

                // 2. 计算滚动方向
                int scrollDirection = e.Delta > 0 ? -1 : 1;

                try
                {
                    // 3. 获取 DisplayLayout
                    var displayLayoutProperty = gridControl.GetType().GetProperty("DisplayLayout");
                    if (displayLayoutProperty == null) return;
                    var displayLayout = displayLayoutProperty.GetValue(gridControl);
                    if (displayLayout == null) return;

                    // 4. 获取 RowScrollRegions
                    var rowScrollRegionsProperty = displayLayout.GetType().GetProperty("RowScrollRegions");
                    if (rowScrollRegionsProperty == null) return;
                    var rowScrollRegions = rowScrollRegionsProperty.GetValue(displayLayout);
                    if (rowScrollRegions == null) return;

                    var regionsList = rowScrollRegions as System.Collections.IEnumerable;
                    if (regionsList == null) return;

                    foreach (var region in regionsList)
                    {
                        if (region == null) continue;

                        // === 方案：使用 ScrollBarInfo 获取精确边界 ===
                        // 这个属性是 public 的，且包含准确的 Maximum
                        var scrollBarInfoProperty = region.GetType().GetProperty("ScrollBarInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        if (scrollBarInfoProperty != null)
                        {
                            var scrollBarInfo = scrollBarInfoProperty.GetValue(region);
                            if (scrollBarInfo != null)
                            {
                                // 获取当前值、最大值、最小值
                                var valueProp = scrollBarInfo.GetType().GetProperty("Value");
                                var maxProp = scrollBarInfo.GetType().GetProperty("Maximum");
                                var minProp = scrollBarInfo.GetType().GetProperty("Minimum");

                                if (valueProp != null && maxProp != null)
                                {
                                    int currentValue = (int)valueProp.GetValue(scrollBarInfo);
                                    int maximum = (int)maxProp.GetValue(scrollBarInfo);
                                    int minimum = minProp != null ? (int)minProp.GetValue(scrollBarInfo) : 0;

                                    // 计算新位置
                                    int newPosition = currentValue + scrollDirection;

                                    // 关键：边界检查，确保在 [Minimum, Maximum] 范围内
                                    // 这样到底或到顶时就动不了了
                                    if (newPosition >= minimum && newPosition <= maximum)
                                    {
                                        // 获取 ScrollPosition 属性并设置新值
                                        var scrollPositionProperty = region.GetType().GetProperty("ScrollPosition");
                                        if (scrollPositionProperty != null)
                                        {
                                            scrollPositionProperty.SetValue(region, newPosition);
                                        }
                                    }
                                }
                            }
                        }

                        break; // 只处理第一个区域
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"滚动控制出错: {ex.Message}");
                }
            }
        }
    }
}
