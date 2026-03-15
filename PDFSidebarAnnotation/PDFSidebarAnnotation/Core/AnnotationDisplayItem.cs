using SwissAcademic.Citavi;
using System.Windows.Media;

namespace PDFSidebarAnnotation
{
    public class AnnotationDisplayItem
    {
        public Annotation CitaviAnnotation { get; set; }
        public int PageNumber { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public Color DisplayColor { get; set; }

        public string ToolTipText
        {
            get
            {
                return $"类型: {Type}\n页码: {PageNumber}\n内容: {Content}";
            }
        }
    }
}
