using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Reflection;

namespace SingleLineScrollGrid
{
    /// <summary>
    /// 为 MainForm 中的 Grid 控件添加鼠标滚轮单行滚动功能，支持分界面独立配置。
    /// </summary>
    public partial class SingleLineScrollGridMainform : CitaviAddOn<MainForm>
    {
        #region OnHostingFormLoaded - 初始化和绑定

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            // 1. 加载保存的设置
            LoadSettings();

            // 2. 注册窗体关闭事件（用于清理）
            mainForm.FormClosed += MainForm_FormClosed;

            // 3. 映射 Grid 控件到类型
            if (mainForm.ReferenceEditorNavigationGrid != null)
            {
                _gridTypeMap[mainForm.ReferenceEditorNavigationGrid] = GridType.Reference;
            }

            if (mainForm.KnowledgeOrganizerGrid != null)
            {
                _gridTypeMap[mainForm.KnowledgeOrganizerGrid] = GridType.Knowledge;
            }

            if (mainForm.TaskPlannerGrid != null)
            {
                _gridTypeMap[mainForm.TaskPlannerGrid] = GridType.Task;
            }

            // 4. 统一绑定鼠标滚轮事件
            foreach (var kvp in _gridTypeMap)
            {
                var grid = kvp.Key;
                if (!_processedGrids.Contains(grid))
                {
                    _processedGrids.Add(grid);
                    grid.MouseWheel -= MainGrid_MouseWheel;
                    grid.MouseWheel += MainGrid_MouseWheel;
                }
            }

            // 5. 添加设置菜单按钮
            mainForm.GetMainCommandbarManager().GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu).GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References).InsertCommandbarButton(15, COMMAND_KEY, "Grid滚动设置", CommandbarItemStyle.ImageAndText, SwissAcademic.Citavi.Shell.Properties.Resources.EditAnnotation);
            mainForm.GetReferenceEditorNavigationCommandbarManager().GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar).GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Tools).AddCommandbarButton(COMMAND_KEY, "Grid滚动设置", CommandbarItemStyle.ImageAndText, SwissAcademic.Citavi.Shell.Properties.Resources.EditAnnotation);

        }

        #endregion

        #region OnBeforePerformingCommand - 处理菜单命令

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(COMMAND_KEY, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                using (var dialog = new GridScrollSettingsDialog(
                    mainForm,
                    _refGridEnabled, _refGridLines,
                    _knowGridEnabled, _knowGridLines,
                    _taskGridEnabled, _taskGridLines))
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    // 更新字段
                    _refGridEnabled = dialog.RefGridEnabled;
                    _refGridLines = dialog.RefGridLines;
                    _knowGridEnabled = dialog.KnowGridEnabled;
                    _knowGridLines = dialog.KnowGridLines;
                    _taskGridEnabled = dialog.TaskGridEnabled;
                    _taskGridLines = dialog.TaskGridLines;

                    // 保存设置
                    SaveSettings();
                }
            }
        }

        #endregion

        #region 滚轮事件处理 - 核心逻辑

        private void MainGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (sender is Control gridControl && _gridTypeMap.TryGetValue(gridControl, out GridType gridType))
            {
                // 1. 根据类型获取当前配置
                bool isEnabled = false;
                int scrollLines = 1;

                switch (gridType)
                {
                    case GridType.Reference:
                        isEnabled = _refGridEnabled;
                        scrollLines = _refGridLines;
                        break;
                    case GridType.Knowledge:
                        isEnabled = _knowGridEnabled;
                        scrollLines = _knowGridLines;
                        break;
                    case GridType.Task:
                        isEnabled = _taskGridEnabled;
                        scrollLines = _taskGridLines;
                        break;
                }

                // 2. 如果未启用，不处理（使用系统默认行为）
                if (!isEnabled) return;

                // 3. 标记事件已处理
                ((HandledMouseEventArgs)e).Handled = true;

                try
                {
                    // 4. 获取 DisplayLayout
                    var displayLayoutProperty = gridControl.GetType().GetProperty("DisplayLayout");
                    if (displayLayoutProperty == null) return;
                    var displayLayout = displayLayoutProperty.GetValue(gridControl);
                    if (displayLayout == null) return;

                    // 5. 获取 RowScrollRegions
                    var rowScrollRegionsProperty = displayLayout.GetType().GetProperty("RowScrollRegions");
                    if (rowScrollRegionsProperty == null) return;
                    var rowScrollRegions = rowScrollRegionsProperty.GetValue(displayLayout);
                    if (rowScrollRegions == null) return;

                    var regionsList = rowScrollRegions as System.Collections.IEnumerable;
                    if (regionsList == null) return;

                    // 6. 循环调用 OnScroll 实现多行滚动
                    for (int i = 0; i < scrollLines; i++)
                    {
                        foreach (var region in regionsList)
                        {
                            if (region == null) continue;

                            // 获取 ScrollBarInfo
                            var scrollBarInfoProperty = region.GetType().GetProperty(
                                "ScrollBarInfo",
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                            );
                            if (scrollBarInfoProperty == null) break;

                            var scrollBarInfo = scrollBarInfoProperty.GetValue(region);
                            if (scrollBarInfo == null) break;

                            // 确定 ScrollEventType
                            var scrollEventTypeValue = e.Delta > 0
                                ? ScrollEventType.SmallDecrement
                                : ScrollEventType.SmallIncrement;

                            // 获取并调用 OnScroll 方法
                            var onScrollMethod = region.GetType().GetMethod(
                                "OnScroll",
                                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                null,
                                new Type[] { scrollBarInfo.GetType(), typeof(ScrollEventType) },
                                null
                            );

                            if (onScrollMethod != null)
                            {
                                onScrollMethod.Invoke(region, new object[] { scrollBarInfo, scrollEventTypeValue });
                            }

                            break; // 通常只处理第一个区域
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Grid滚动控制出错: {ex.Message}");
                }
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 窗体关闭时清理事件
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                mainForm.FormClosed -= MainForm_FormClosed;

                foreach (var grid in _processedGrids)
                {
                    grid.MouseWheel -= MainGrid_MouseWheel;
                }

                _processedGrids.Clear();
                _gridTypeMap.Clear();
            }
        }

        #endregion
    }
}
