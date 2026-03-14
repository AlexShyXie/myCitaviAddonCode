using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleLineScrollGrid
{
    partial class SingleLineScrollGridMainform
    {
        /// <summary>
        /// 从 Settings 字典加载所有设置
        /// </summary>
        private void LoadSettings()
        {
            // 加载 Reference 界面设置
            if (Settings.TryGetValue(SETTINGS_KEY_REF_ENABLED, out string refEnabledStr) &&
                bool.TryParse(refEnabledStr, out bool refEnabled))
            {
                _refGridEnabled = refEnabled;
            }

            if (Settings.TryGetValue(SETTINGS_KEY_REF_LINES, out string refLinesStr) &&
                int.TryParse(refLinesStr, out int refLines) && refLines > 0)
            {
                _refGridLines = refLines;
            }

            // 加载 Knowledge 界面设置
            if (Settings.TryGetValue(SETTINGS_KEY_KNOW_ENABLED, out string knowEnabledStr) &&
                bool.TryParse(knowEnabledStr, out bool knowEnabled))
            {
                _knowGridEnabled = knowEnabled;
            }

            if (Settings.TryGetValue(SETTINGS_KEY_KNOW_LINES, out string knowLinesStr) &&
                int.TryParse(knowLinesStr, out int knowLines) && knowLines > 0)
            {
                _knowGridLines = knowLines;
            }

            // 加载 Task 界面设置
            if (Settings.TryGetValue(SETTINGS_KEY_TASK_ENABLED, out string taskEnabledStr) &&
                bool.TryParse(taskEnabledStr, out bool taskEnabled))
            {
                _taskGridEnabled = taskEnabled;
            }

            if (Settings.TryGetValue(SETTINGS_KEY_TASK_LINES, out string taskLinesStr) &&
                int.TryParse(taskLinesStr, out int taskLines) && taskLines > 0)
            {
                _taskGridLines = taskLines;
            }
        }

        /// <summary>
        /// 保存所有设置到 Settings 字典
        /// </summary>
        private void SaveSettings()
        {
            // 保存 Reference 设置
            Settings[SETTINGS_KEY_REF_ENABLED] = _refGridEnabled.ToString();
            Settings[SETTINGS_KEY_REF_LINES] = _refGridLines.ToString();

            // 保存 Knowledge 设置
            Settings[SETTINGS_KEY_KNOW_ENABLED] = _knowGridEnabled.ToString();
            Settings[SETTINGS_KEY_KNOW_LINES] = _knowGridLines.ToString();

            // 保存 Task 设置
            Settings[SETTINGS_KEY_TASK_ENABLED] = _taskGridEnabled.ToString();
            Settings[SETTINGS_KEY_TASK_LINES] = _taskGridLines.ToString();
        }
    }
}

