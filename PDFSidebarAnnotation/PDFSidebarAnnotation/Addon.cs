using SwissAcademic.Citavi.Controls.Wpf;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PDFSidebarAnnotation
{
    public class Addon : CitaviAddOn<MainForm>
    {
        private AnnotationListControl _annotationControl;

        #region Methods
        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            AddTabPageToSideBar(mainForm);
            Observe(mainForm, true);
            base.OnHostingFormLoaded(mainForm);
        }

        void Observe(MainForm mainForm, bool observe)
        {
            if (observe)
            {
                mainForm.FormClosed += MainForm_FormClosed;
                var viewer = mainForm.PreviewControl.GetPdfViewControl();
                if (viewer != null)
                {
                    // 只监听文档变化事件，用于刷新列表
                    viewer.DocumentChanged += Viewer_DocumentChanged;
                    viewer.DocumentClosing += Viewer_DocumentClosing;
                }
            }
            else
            {
                mainForm.FormClosed -= MainForm_FormClosed;
                var viewer = mainForm.PreviewControl.GetPdfViewControl();
                if (viewer != null)
                {
                    viewer.DocumentChanged -= Viewer_DocumentChanged;
                    viewer.DocumentClosing -= Viewer_DocumentClosing;
                }
            }
        }

        void AddTabPageToSideBar(MainForm mainForm)
        {
            if (mainForm.PreviewControl.GetPdfViewControl().GetSideBar() is System.Windows.Controls.TabControl tabControl)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/PDFSidebarAnnotation;component/Resources/AnnotationIcon.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();

                var translationControl = new AnnotationListControl(mainForm);

                var tabPage = new TabItem
                {
                    Tag = "AnnotationListAddon",
                    Header = new Image { Height = 16, Source = bitmapImage },
                    Content = translationControl
                };

                tabControl.Items.Add(tabPage);
            }
        }

        #endregion

        #region EventHandlers
        private void Viewer_DocumentChanged(object sender, EventArgs e)
        {
            // 当文档切换时，刷新注解列表
            if (_annotationControl != null)
            {
                _annotationControl.RefreshAnnotations();
            }
        }

        private void Viewer_DocumentClosing(object sender, DocumentClosingArgs args)
        {
            // 当文档关闭时，清空列表
            if (_annotationControl != null)
            {
                _annotationControl.AnnotationListBox.ItemsSource = null;
            }
        }

        private void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                Observe(mainForm, false);
            }
        }
        #endregion
    }
}
