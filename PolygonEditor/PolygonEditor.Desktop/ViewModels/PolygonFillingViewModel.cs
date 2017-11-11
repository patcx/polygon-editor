using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PolygonEditor.Desktop.Helpers;
using Color = System.Windows.Media.Color;

namespace PolygonEditor.Desktop.ViewModels
{
    public class PolygonFillingViewModel : ObservableObject
    {
        public bool UseFixedObjectColor { get; set; } = true;
        public bool UseFixedLightSource { get; set; } = true;
        public bool UseDisturbanceVector { get; set; } = true;
        public bool UseFixedNormalVector { get; set; } = true;

        public int Radius { get; set; } = 5;
        public Color ObjectColor { get; private set; } = Color.FromRgb(0, 0, 0);
        public Color LightColor { get; private set; } = Color.FromRgb(255, 255, 255);
        public ImageSource ObjectTexture { get; private set; }
        public ImageSource NormalMap { get; private set; }
        public ImageSource HeightMap { get; private set; }

        public ICommand ObjectColorPicker => new RelayCommand(() =>
        {
            Color? color;
            SetColor(out color);
            if (color != null)
            {
                ObjectColor = color.Value;
                RaisePropertyChanged("ObjectColor");
            }
        });

        public ICommand LightColorPicker => new RelayCommand(() =>
        {
            Color? color;
            SetColor(out color);
            if (color != null)
            { 
                LightColor = color.Value;
                RaisePropertyChanged("LightColor");
            }

        });

        public ICommand SetObjectTexture => new RelayCommand(() =>
        {
            Bitmap bmp;
            LoadTexture(out bmp);
            if (bmp != null)
            {
                ObjectTexture = bmp.ConvertToBitmapImage();
                RaisePropertyChanged("ObjectTexture");
            }
        });

        public ICommand SetNormalMap => new RelayCommand(() =>
        {
            Bitmap bmp;
            LoadTexture(out bmp);
            if (bmp != null)
            {
                NormalMap = bmp.ConvertToBitmapImage();
                RaisePropertyChanged("NormalMap");
            }
        });

        public ICommand SetHeightMap => new RelayCommand(() =>
        {
            Bitmap bmp;
            LoadTexture(out bmp);
            if (bmp != null)
            {
                HeightMap = bmp.ConvertToBitmapImage();
                RaisePropertyChanged("HeightMap");
            }
        });

        private void LoadTexture(out Bitmap bmp)
        {
            bmp = null;
            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    bmp = (Bitmap)Image.FromFile(dialog.FileName);
                }
            }
        }

        private void SetColor(out Color? color)
        {
            color = null;
            ColorDialog dialog = new ColorDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                color = Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B);
            }
        }
    }
}
