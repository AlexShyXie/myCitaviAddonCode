using SwissAcademic.Citavi;
using SwissAcademic.Pdf.Analysis;
using System.Collections.Generic;
using System.Linq;

namespace PDFSidebarAnnotation
{
    public class AnnotationComparer : IComparer<Annotation>
    {
        public int Compare(Annotation x, Annotation y)
        {
            if (x == null || y == null) return 0;
            if (x.Quads == null || !x.Quads.Any()) return 1;
            if (y.Quads == null || !y.Quads.Any()) return -1;

            // 1. 比较页码
            int xPage = x.Quads.First().PageIndex;
            int yPage = y.Quads.First().PageIndex;
            if (xPage != yPage) return xPage.CompareTo(yPage);

            // 2. 比较Y坐标 (取最大的Y值，即最上面的点，注意PDF坐标系Y轴向上)
            // QuotationsToolbox 逻辑：取 -Y1 比较，越靠上 Y1 越大，-Y1 越小
            double xTop = x.Quads.Max(q => q.MaxY);
            double yTop = y.Quads.Max(q => q.MaxY);

            // 这里我们简单处理：按照从上到下排序 (Y值大的在前)
            // 如果希望从上到下，应该降序排列 Y 值
            int yCompare = yTop.CompareTo(xTop);
            if (yCompare != 0) return yCompare;

            // 3. 比较X坐标 (从左到右)
            double xLeft = x.Quads.Min(q => q.MinX);
            double yLeft = y.Quads.Min(q => q.MinX);

            return xLeft.CompareTo(yLeft);
        }
    }
}
