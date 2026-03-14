using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleLineScrollGrid
{
    partial class SingleLineScrollGrid
    {
        // ==================== 常量定义 ====================
        // 设置键名 - 确保全局唯一
        private const string SETTINGS_KEY_REF_ENABLED = "SingleLineScrollGrid.Reference.Enabled";
        private const string SETTINGS_KEY_REF_LINES = "SingleLineScrollGrid.Reference.Lines";

        private const string SETTINGS_KEY_KNOW_ENABLED = "SingleLineScrollGrid.Knowledge.Enabled";
        private const string SETTINGS_KEY_KNOW_LINES = "SingleLineScrollGrid.Knowledge.Lines";

        private const string SETTINGS_KEY_TASK_ENABLED = "SingleLineScrollGrid.Task.Enabled";
        private const string SETTINGS_KEY_TASK_LINES = "SingleLineScrollGrid.Task.Lines";

        private const string COMMAND_KEY = "SingleLineScrollGrid.Settings.Button";

        // 默认值
        private const int DEFAULT_SCROLL_LINES = 1;
        private const bool DEFAULT_ENABLED = true;

        // ==================== 字段定义 ====================
        // 三个界面的配置
        private bool _refGridEnabled = DEFAULT_ENABLED;
        private int _refGridLines = DEFAULT_SCROLL_LINES;

        private bool _knowGridEnabled = DEFAULT_ENABLED;
        private int _knowGridLines = DEFAULT_SCROLL_LINES;

        private bool _taskGridEnabled = DEFAULT_ENABLED;
        private int _taskGridLines = DEFAULT_SCROLL_LINES;

        // 用于记录 Grid 和其类型的映射
        private readonly System.Collections.Generic.Dictionary<System.Windows.Forms.Control, GridType> _gridTypeMap
            = new System.Collections.Generic.Dictionary<System.Windows.Forms.Control, GridType>();

        // 用于记录已经绑定过事件的 Grid 控件，防止重复绑定
        private readonly System.Collections.Generic.HashSet<System.Windows.Forms.Control> _processedGrids
            = new System.Collections.Generic.HashSet<System.Windows.Forms.Control>();

        // ==================== 枚举定义 ====================
        private enum GridType
        {
            Reference,
            Knowledge,
            Task
        }
    }
}

