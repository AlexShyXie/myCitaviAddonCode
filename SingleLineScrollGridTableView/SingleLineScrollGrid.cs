using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Reflection;

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

                try
                {
                    // 2. 获取 DisplayLayout
                    var displayLayoutProperty = gridControl.GetType().GetProperty("DisplayLayout");
                    if (displayLayoutProperty == null) return;
                    var displayLayout = displayLayoutProperty.GetValue(gridControl);
                    if (displayLayout == null) return;

                    // 3. 获取 RowScrollRegions
                    var rowScrollRegionsProperty = displayLayout.GetType().GetProperty("RowScrollRegions");
                    if (rowScrollRegionsProperty == null) return;
                    var rowScrollRegions = rowScrollRegionsProperty.GetValue(displayLayout);
                    if (rowScrollRegions == null) return;

                    var regionsList = rowScrollRegions as System.Collections.IEnumerable;
                    if (regionsList == null) return;

                    foreach (var region in regionsList)
                    {
                        if (region == null) continue;

                        // 4. 调用 OnScroll 方法模拟点击滚动条按钮
                        // 关键：使用 ScrollEventType.SmallDecrement (向上) 或 SmallIncrement (向下)
                        // 这完全模拟了点击滚动条上下箭头按钮的行为，Grid 会自动处理边界！

                        // 获取 ScrollBarInfo (这是 OnScroll 的第一个参数)
                        var scrollBarInfoProperty = region.GetType().GetProperty("ScrollBarInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (scrollBarInfoProperty == null) break;
                        var scrollBarInfo = scrollBarInfoProperty.GetValue(region);
                        if (scrollBarInfo == null) break;

                        // 获取 ScrollEventType 类型 (System.Windows.Forms 命名空间)
                        var scrollEventTypeType = typeof(ScrollEventType);

                        // 确定 ScrollEventType 值
                        // Delta > 0: 滚轮向上 -> 对应点击滚动条的"向上按钮" -> SmallDecrement
                        // Delta < 0: 滚轮向下 -> 对应点击滚动条的"向下按钮" -> SmallIncrement
                        var scrollEventTypeValue = e.Delta > 0 ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement;

                        // 获取 OnScroll 方法
                        // 参数列表: void OnScroll(ScrollBarInfo scrollBar, ScrollEventType scrollType)

                        // 注意：OnScroll 是 protected 方法，必须用 NonPublic 找到它
                        var onScrollMethod = region.GetType().GetMethod("OnScroll", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { scrollBarInfo.GetType(), typeof(ScrollEventType) }, null);

                        if (onScrollMethod != null)
                        {
                            // 调用 OnScroll
                            onScrollMethod.Invoke(region, new object[] { scrollBarInfo, scrollEventTypeValue });
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
