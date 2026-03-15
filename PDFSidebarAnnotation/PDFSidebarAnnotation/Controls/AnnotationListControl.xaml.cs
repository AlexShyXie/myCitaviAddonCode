using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SwissAcademic.Citavi.Shell;

namespace PDFSidebarAnnotation
{
    public partial class AnnotationListControl : UserControl
    {
        private readonly MainForm _mainForm;

        public AnnotationListControl(MainForm mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }

        // 公开方法，供 Addon 调用以刷新列表
        public void RefreshAnnotations()
        {
            var annotations = _mainForm.PreviewControl.GetCurrentPdfAnnotations();
            AnnotationListBox.ItemsSource = annotations;
        }

        // 点击列表项时，跳转到对应的 Annotation
        private void AnnotationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnnotationListBox.SelectedItem is AnnotationDisplayItem item)
            {
                _mainForm.PreviewControl.NavigateToAnnotation(item.CitaviAnnotation);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshAnnotations();
        }
    }
}
