using CoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 碰撞检测优化
{
    public class MapManager
    {
        private static Dictionary<Point, HashSet<BaseObject>> mapInfos = new Dictionary<Point, HashSet<BaseObject>>();
        private static Panel panel;
        private static int cols;
        private static int rows;
        private static int cellWidth;
        private static int cellHeight;
        public static void SetMapContainer(Panel _panel,int _cols,int _rows)
        {
            panel = _panel;
            cols = _cols;
            rows = _rows;
            cellWidth = panel.Width / cols;
            cellHeight = panel.Height / rows;
        }

        public static void AddObjectToMap(BaseObject obj)
        {
            if (obj == null) return;
            var cellIndex = CalculCellIndex(obj);
            if (!mapInfos.ContainsKey(cellIndex))
            {
                mapInfos[cellIndex] = new HashSet<BaseObject>();
            }
            mapInfos[cellIndex].Add(obj);
        }

        public static void AddObjectsToMap(IEnumerable<BaseObject> objs)
        {
            foreach (var obj in objs)
            {
                AddObjectToMap(obj);
            }
        }

        public static void UpdateObjectPosition(BaseObject obj)
        {
            AddObjectToMap(obj);
            //在周围的格子里查找是否存在相同的obj，如果有就删除
            var currCell = CalculCellIndex(obj);
            foreach (var item in GetSurroundObjects(obj))
            {
                if (item.Key == currCell)
                {
                    continue;
                }
                if (item.Value.Contains(obj))
                {
                    item.Value.Remove(obj);
                }
            }
        }

        public static void ClearObjectPostion(BaseObject obj)
        {
            var cellIndex = CalculCellIndex(obj);
            if (mapInfos.ContainsKey(cellIndex) && mapInfos[cellIndex].Contains(obj))
            {
                mapInfos[cellIndex].Remove(obj);
            }
        }
        /// <summary>
        /// 获取周围格子和自身格子里的对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<Point,HashSet<BaseObject>>> GetSurroundObjects(BaseObject obj)
        {
            // 获取obj所在的格子索引
            Point cellIndex = CalculCellIndex(obj);
            //获取周围最多八格里的对象列表，依次返回
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    Point neighborIndex = new Point(cellIndex.X + dx, cellIndex.Y + dy);
                    if (mapInfos.ContainsKey(neighborIndex))
                    {
                        yield return new KeyValuePair<Point, HashSet<BaseObject>>(neighborIndex, mapInfos[neighborIndex]);
                    }
                }
            }
        }

        public static Point CalculCellIndex(BaseObject obj)
        {
            Point objCenter = new Point(obj.Left + obj.Width / 2, obj.Top + obj.Height / 2);
            return new Point(objCenter.X / cellWidth, objCenter.Y / cellHeight);
        }

        public static Point CalculRealPosition(Point cellPos)
        {
            return new Point(cellPos.X * cellWidth, cellPos.Y * cellHeight);
        }
    }
}
