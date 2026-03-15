using SwissAcademic.Citavi.Controls.Wpf;
using SwissAcademic.Citavi.Shell.Controls.Preview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissAcademic.Citavi;
using Color = System.Windows.Media.Color;
using pdftron.PDF;
using System.Text;
using System.Windows.Forms;
using SwissAcademic.Controls;

namespace PDFSidebarAnnotation
{
    public static class CitaviHelper
    {
        static readonly BindingFlags Flags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic;

        internal static PdfViewControl GetPdfViewControl(this PreviewControl previewControl)
        {
            return previewControl?
                .GetType()
                .GetProperty("PdfViewControl", Flags, null, typeof(PdfViewControl), new Type[0], null)?
                .GetValue(previewControl, Flags, null, null, null) as PdfViewControl;
        }

        // 使用完全限定名 System.Windows.Controls.TabControl
        internal static System.Windows.Controls.TabControl GetSideBar(this PdfViewControl pdfViewControl)
        {
            if (pdfViewControl == null) return null;
            // 这里也要使用完全限定名
            return WPFHelper.FindChild<System.Windows.Controls.TabControl>(pdfViewControl);
        }

        /// <summary>
        /// 获取当前PDF的所有Annotation并转换为显示列表
        /// </summary>
        public static List<AnnotationDisplayItem> GetCurrentPdfAnnotations(this PreviewControl previewControl)
        {
            var resultList = new List<AnnotationDisplayItem>();
            Location location = previewControl.ActiveLocation;
            if (location == null) return resultList;

            var annotations = location.Annotations.ToList();
            annotations.Sort(new AnnotationComparer());

            foreach (var annotation in annotations)
            {
                var quadsList = annotation.Quads.ToList();
                var item = new AnnotationDisplayItem
                {
                    CitaviAnnotation = annotation,
                    PageNumber = quadsList.Count > 0 ? quadsList[0].PageIndex : 1,
                    DisplayColor = Color.FromArgb(
                        annotation.OriginalColor.A,
                        annotation.OriginalColor.R,
                        annotation.OriginalColor.G,
                        annotation.OriginalColor.B)
                };

                // 1. 首先检查是否有知识条目关联
                var knowledgeItem = GetKnowledgeFromAnnotation(annotation);
                if (knowledgeItem != null)
                {
                    // 有知识条目关联
                    item.Type = GetTypeString(knowledgeItem.QuotationType);
                    if (item.Type == "HiY")
                    {
                        if (knowledgeItem.Relevance.ToString() != "0")
                        {
                            item.Type = "HiG";
                            item.Content = "HighGray With Invisible Knowledge";
                        }
                        else
                        {
                            item.Content = "HighYellow With Invisible Knowledge";
                        }
                        
                    }
                    else if (item.Type == "None")
                    {
                        item.Content = "HighGray With Invisible Knowledge";
                    }
                    else
                    {
                        item.Content = !string.IsNullOrEmpty(knowledgeItem.CoreStatement) ?
                                   knowledgeItem.CoreStatement :
                                   knowledgeItem.Text;
                    }
                    
                }
                else
                {
                    if (!annotation.Visible)  // 没有Knowledge，然后visible是False的，就是完全被隐藏的，需要用5_检测隐藏的Annotation并删除.cs处理
                    {
                        item.Type = "Hid";
                        item.Content = "NoKnowledge Invisible Data";
                    }
                    else
                    {
                        // 2. 没有知识条目关联，visible是True的，就是灰色高亮
                        item.Type = "HiU";
                        item.Content = "NoKnowledge Underline";
                    }

                }

                resultList.Add(item);
            }

            return resultList;
        }

        private static string GetTypeString(QuotationType type)
        {
            switch (type)
            {
                case QuotationType.DirectQuotation:
                    return "Dir";
                case QuotationType.IndirectQuotation:
                    return "Ind";
                case QuotationType.QuickReference:
                    return "HiR"; // 实际上Highlight in red是QuickReference，然后visible是False
                case QuotationType.Comment:
                    return "Cmt";
                case QuotationType.Summary:
                    return "Sum";
                case QuotationType.Highlight:
                    return "HiY";  // 实际上Highlight in yellow是没有Knowledge，然后visible是True
                default:
                    return "None";
            }
        }


        // 您之前的辅助方法
        public static KnowledgeItem GetKnowledgeFromAnnotation(Annotation annotation)
        {
            if (annotation == null || annotation.EntityLinks == null) return null;
            var targetLink = annotation.EntityLinks
                .Where(e => e.Indication == EntityLink.PdfKnowledgeItemIndication)
                .FirstOrDefault();
            if (targetLink != null && targetLink.Source is KnowledgeItem)
            {
                return (KnowledgeItem)targetLink.Source;
            }
            return null;
        }


        // 跳转逻辑
        public static void NavigateToAnnotation(this PreviewControl previewControl, Annotation annotation)
        {
            if (annotation == null) return;
            try
            {
                var pdfViewControl = previewControl.GetPdfViewControl();
                if (pdfViewControl == null) return;

                var goToAnnotationMethod = pdfViewControl.GetType().GetMethod("GoToAnnotation",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (goToAnnotationMethod != null)
                {
                    goToAnnotationMethod.Invoke(pdfViewControl, new object[] { annotation, null });
                }
            }
            catch { }
        }
    }
}
