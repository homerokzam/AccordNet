using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Accord.Video;
using Accord.Video.DirectShow;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;

namespace AccordNet
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      Init(this.pictureBox1);
    }

    public void Init(PictureBox pb)
    {
      FilterInfoCollection VideoCaptuerDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
      VideoCaptureDevice FinalVideoSource = new VideoCaptureDevice(VideoCaptuerDevices[0].MonikerString);

      FinalVideoSource.NewFrame += new NewFrameEventHandler((sender, eventArgs) =>
      {
        Bitmap image = (Bitmap)eventArgs.Frame.Clone();

        FaceHaarCascade cascade = new FaceHaarCascade();
        HaarObjectDetector detector = new HaarObjectDetector(cascade, 80);

        detector.UseParallelProcessing = true;

        Rectangle[] faces = detector.ProcessFrame(image);

        Graphics g = Graphics.FromImage(image);
        foreach (var face in faces)
        {
          Pen p = new Pen(Color.Red, 10f);
          g.DrawRectangle(p, face);
        }
        g.Dispose();

        pb.Image = image;
      });

      FinalVideoSource.DesiredFrameRate = 1;
      FinalVideoSource.DesiredFrameSize = new Size(1, 500);
      FinalVideoSource.Start();
    }
  }
}
