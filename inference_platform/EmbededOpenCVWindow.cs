using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inference_platform
{
    internal class EmbededOpenCVWindow
    {
        #region OpenCVWindow 嵌入窗体控件

        #region 引入dll
        //FindWindow用来查找窗体。
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strclassName, string strWindowText);

        //SetParent用来设置窗体的父窗体。
        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        //MoveWindow用来改变窗体大小。
        [DllImport("user32.dll")]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        //DestroyWindow用来关闭并窗体。
        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);


        private const int GWL_STYLE = (-16);
        public const int WS_CAPTION = 0xC00000;

        //GetWindowLong 获取index
        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //SetWindowLong 隐藏标题栏
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion 引入dll

        /// <summary>
        /// 嵌入OpenCV的窗体
        /// </summary>
        /// <param name="windowName">窗体名称</param>
        /// <param name="control">嵌入的容器</param>
        /// <returns></returns>
        public static IntPtr embeded_openCV_window(string windowName, Control control)
        {
            //窗体嵌入
            IntPtr ptr = new IntPtr(0);
            ptr = FindWindow(null, windowName);
            //判断这个窗体是否有效 
            if (ptr != IntPtr.Zero)
            {
                //MessageBox.Show("找到窗口");
                SetParent(ptr, control.Handle);
                control.Tag = ptr;

                SetWindowLong(ptr, GWL_STYLE, GetWindowLong(ptr, GWL_STYLE) & ~WS_CAPTION);
                MoveWindow(ptr, 0, 0, control.ClientSize.Width, control.ClientSize.Height, true);
            }
            return ptr;
        }

        #endregion OpenCVWindow 嵌入窗体控件


    }
}
