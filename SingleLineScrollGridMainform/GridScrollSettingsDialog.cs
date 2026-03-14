using System;
using System.Windows.Forms;
using SwissAcademic.Controls;

namespace SingleLineScrollGrid
{
    /// <summary>
    /// Grid 滚动设置对话框 - 逻辑代码
    /// </summary>
    public partial class GridScrollSettingsDialog : FormBase
    {
        #region 公共属性 - 用于返回设置结果
        
        public bool RefGridEnabled { get; private set; }
        public int RefGridLines { get; private set; }
        
        public bool KnowGridEnabled { get; private set; }
        public int KnowGridLines { get; private set; }
        
        public bool TaskGridEnabled { get; private set; }
        public int TaskGridLines { get; private set; }
        
        #endregion

        #region 构造函数
        
        public GridScrollSettingsDialog(
            Form owner, 
            bool refEnabled, int refLines,
            bool knowEnabled, int knowLines,
            bool taskEnabled, int taskLines) : base(owner)
        {
            InitializeComponent(); // 调用 Designer.cs 中的初始化方法
            
            // 传入当前设置值
            RefGridEnabled = refEnabled;
            RefGridLines = refLines;
            KnowGridEnabled = knowEnabled;
            KnowGridLines = knowLines;
            TaskGridEnabled = taskEnabled;
            TaskGridLines = taskLines;
            
            // 初始化控件的值
            InitializeValues();
        }
        
        #endregion

        #region 初始化控件值
        
        private void InitializeValues()
        {
            // 设置控件的初始值
            chkRefEnabled.Checked = RefGridEnabled;
            numRefLines.Value = RefGridLines;
            
            chkKnowEnabled.Checked = KnowGridEnabled;
            numKnowLines.Value = KnowGridLines;
            
            chkTaskEnabled.Checked = TaskGridEnabled;
            numTaskLines.Value = TaskGridLines;
        }
        
        #endregion

        #region 事件处理
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            // 更新属性值
            RefGridEnabled = chkRefEnabled.Checked;
            RefGridLines = (int)numRefLines.Value;
            
            KnowGridEnabled = chkKnowEnabled.Checked;
            KnowGridLines = (int)numKnowLines.Value;
            
            TaskGridEnabled = chkTaskEnabled.Checked;
            TaskGridLines = (int)numTaskLines.Value;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            // 重置为默认值
            chkRefEnabled.Checked = true;
            numRefLines.Value = 1;
            
            chkKnowEnabled.Checked = true;
            numKnowLines.Value = 1;
            
            chkTaskEnabled.Checked = true;
            numTaskLines.Value = 1;
        }
        
        #endregion
    }
}
