using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeIdeas.TIProData;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// ImageTest form allows the user to type the code of an alert(or filter) in
    /// the text box. Then upon clicking the appropriate button that particular alert
    /// or filter will be displayed in the lower image panel.
    /// </summary>
    public partial class ImageTest : Form
    {
        private readonly ImageCacheManager _imageCacheManager;
        /// <summary>
        /// ImageTest constructor
        /// </summary>
        /// <param name="imageCacheManager"> ImageCacheManager object</param>
        public ImageTest(ImageCacheManager imageCacheManager)
        {
            _imageCacheManager = imageCacheManager;
            InitializeComponent();
            if (!IsHandleCreated)
                // This is required because the callbacks can come in any thread.  InvokeRequired
                // does not work correctly before the handle is created.  Note that there is only
                // one ImageCache and only one callback shared by all windows.  So we might get
                // a callback at any time, even before someone hits a button on this window.
                CreateHandle();
            imageCacheManager.CachedImageAvailable +=
                new CachedImageAvailable(_imageCacheManager_CachedImageAvailable);
        }

        private delegate void DoUpdate();

        void  _imageCacheManager_CachedImageAvailable()
        {
         	if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)_imageCacheManager_CachedImageAvailable);
            }
            else
            {
                if (IsDisposed)
                    return;
                ReDisplayImages();
            }
        }

        private void ReDisplayImages()
        {
            imagePanel.SuspendLayout();
            int top = 0;
            foreach (Control control in imagePanel.Controls)
            {
                DoUpdate doUpdate = control.Tag as DoUpdate;
                if (null != doUpdate)
                    doUpdate();
                control.Top = top;
                top += control.Height;
            }
            imagePanel.ResumeLayout();
        }

        private void ImageTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            _imageCacheManager.CachedImageAvailable -= _imageCacheManager_CachedImageAvailable;
        }

        private void alertButton_Click(object sender, EventArgs e)
        {
            foreach (string name in additionsTextBox.Lines)
            {
                string internalName = name.Trim();
                if (internalName != "")
                    if (preloadCheckBox.Checked)
                        _imageCacheManager.GetAlert(internalName, true);
                    else
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Visible = true;
                        // The size should always be the same.  In the config window we take advantage of the
                        // fixed size to help us with the layout.
                        //pictureBox.Height = ImageCacheManager.ICON_HEIGHT;
                        //pictureBox.Width = ImageCacheManager.ICON_WIDTH;
                        // I commented out the hight and width because there is no obvious way to set the
                        // CLIENT height & width.  That should be done better in the real code.
                        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        // The alert grid sometimes changes the size of the icon.  This works surprisingly
                        // well, espeically when we are shrinking.  Keep the aspect ratio fixed!
                        //pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        // In the config window the resessed border makes the icons look better.  In the grid
                        // there is no border.
                        pictureBox.BorderStyle = BorderStyle.Fixed3D;
                        pictureBox.Tag = (DoUpdate)delegate { pictureBox.Image = _imageCacheManager.GetAlert(internalName); };
                        imagePanel.Controls.Add(pictureBox);
                    }
            }
            if (!preloadCheckBox.Checked)
                ReDisplayImages();
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            foreach (string name in additionsTextBox.Lines)
            {
                string internalName = name.Trim();
                if (internalName != "")
                    if (preloadCheckBox.Checked)
                        _imageCacheManager.GetAlert(internalName, true);
                    else
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Visible = true;
                        pictureBox.Height = ImageCacheManager.ICON_HEIGHT;
                        pictureBox.Width = ImageCacheManager.ICON_WIDTH;
                        pictureBox.SizeMode = PictureBoxSizeMode.Normal;
                        // In some places you want to add extra space and center the image, so these will
                        // take the same space as the alert icons.
                        pictureBox.Tag = (DoUpdate)delegate { pictureBox.Image = _imageCacheManager.GetFilter(internalName); };
                        imagePanel.Controls.Add(pictureBox);
                    }
            }
            if (!preloadCheckBox.Checked)
                ReDisplayImages();
        }

        private void textureButton_Click(object sender, EventArgs e)
        {
            foreach (string name in additionsTextBox.Lines)
            {
                string fileName = name.Trim();
                if (fileName != "")
                    if (preloadCheckBox.Checked)
                        _imageCacheManager.GetAlert(fileName, true);
                    else
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = _imageCacheManager.Get(ImageCacheManager.TEXTURES, fileName);
                        pictureBox.Visible = true;
                        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        pictureBox.Tag = (DoUpdate)delegate { pictureBox.Image = _imageCacheManager.Get(ImageCacheManager.TEXTURES, fileName); };
                        imagePanel.Controls.Add(pictureBox);
                    }
            }
            if (!preloadCheckBox.Checked)
                ReDisplayImages();
        }
    }
}
