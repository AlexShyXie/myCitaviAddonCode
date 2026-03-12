using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Reflection;

namespace SingleLineScrollGrid
{
    /// <summary>
    /// 为 MainForm 中的 Grid 控件添加鼠标滚轮单行滚动功能。
    /// </summary>
    public class SingleLineScrollGrid : CitaviAddOn<MainForm>
    {
        // 用于记录已经绑定过事件的 Grid 控件，防止重复绑定
        private readonly HashSet<Control> _processedGrids = new HashSet<Control>();

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            // MainForm 中需要处理的三个 Grid 列表
            var targetGrids = new List<Control>();

            // 1. Reference 界面的 Grid (属性名: ReferenceEditorNavigationGrid)
            if (mainForm.ReferenceEditorNavigationGrid != null)
            {
                targetGrids.Add(mainForm.ReferenceEditorNavigationGrid);
            }

            // 2. Knowledge 界面的 Grid (属性名: KnowledgeOrganizerGrid)
            if (mainForm.KnowledgeOrganizerGrid != null)
            {
                targetGrids.Add(mainForm.KnowledgeOrganizerGrid);
            }

            // 3. Task 界面的 Grid (属性名: TaskPlannerGrid)
            if (mainForm.TaskPlannerGrid != null)
            {
                targetGrids.Add(mainForm.TaskPlannerGrid);
            }

            // 统一绑定事件
            foreach (var grid in targetGrids)
            {
                if (!_processedGrids.Contains(grid))
                {
                    _processedGrids.Add(grid);
                    grid.MouseWheel -= MainGrid_MouseWheel;
                    grid.MouseWheel += MainGrid_MouseWheel;
                }
            }
        }

        private void MainGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (sender is Control gridControl)
            {
                // 1. 标记事件已处理，阻止默认的滚动行为
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
                        // 获取 ScrollBarInfo (这是 OnScroll 的第一个参数)
                        var scrollBarInfoProperty = region.GetType().GetProperty("ScrollBarInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (scrollBarInfoProperty == null) break;
                        var scrollBarInfo = scrollBarInfoProperty.GetValue(region);
                        if (scrollBarInfo == null) break;

                        // 获取 ScrollEventType 类型
                        var scrollEventTypeType = typeof(ScrollEventType);

                        // 确定 ScrollEventType 值
                        // Delta > 0: 滚轮向上 -> 对应点击滚动条的"向上按钮" -> SmallDecrement
                        // Delta < 0: 滚轮向下 -> 对应点击滚动条的"向下按钮" -> SmallIncrement
                        var scrollEventTypeValue = e.Delta > 0 ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement;

                        // 获取 OnScroll 方法
                        var onScrollMethod = region.GetType().GetMethod("OnScroll", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { scrollBarInfo.GetType(), typeof(ScrollEventType) }, null);

                        if (onScrollMethod != null)
                        {
                            // 调用 OnScroll
                            onScrollMethod.Invoke(region, new object[] { scrollBarInfo, scrollEventTypeValue });
                        }

                        break; // 通常只处理第一个区域
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
