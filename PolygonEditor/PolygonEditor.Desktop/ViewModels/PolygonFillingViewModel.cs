using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PolygonEditor.Desktop.Helpers;
using PolygonEditor.Desktop.Models.Filler;
using Color = System.Windows.Media.Color;

namespace PolygonEditor.Desktop.ViewModels
{
    public class PolygonFillingViewModel : ObservableObject
    {

        private Bitmap objectTexture;
        private Bitmap normalMap;
        private Bitmap heightMap;

        public bool UseFixedObjectColor { get; set; } = true;
        public bool UseFixedLightSource { get; set; } = true;
        public bool UseEmptyDisturbanceVector { get; set; } = true;
        public bool UseFixedNormalVector { get; set; } = true;
        public bool UseMouseFollowNormalVector { get; set; }

        public float Factor { get; set; } = 1;
        public int Radius { get; set; } = 5;
        public Color ObjectColor { get; private set; } = Color.FromRgb(100, 100, 100);
        public Color LightColor { get; private set; } = Color.FromRgb(255, 255, 255);
        public ImageSource ObjectTexture { get; private set; }
        public ImageSource NormalMap { get; private set; }
        public ImageSource HeightMap { get; private set; }


        public PolygonFillingViewModel()
        {
            objectTexture = Properties.Resources.texture;
            ObjectTexture = objectTexture.ConvertToBitmapImage();
            RaisePropertyChanged("ObjectTexture");

            normalMap = Properties.Resources.normal_map;
            NormalMap = normalMap.ConvertToBitmapImage();
            RaisePropertyChanged("NormalMap");

            heightMap = Properties.Resources.brick_heightmap;
            HeightMap = heightMap.ConvertToBitmapImage();
            RaisePropertyChanged("HeightMap");
        }

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
                objectTexture = bmp;
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
                normalMap = bmp;
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
                heightMap = bmp;
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

        public PolygonFillingSettings GetFillingSettings()
        {
            return new PolygonFillingSettings()
            {
                UseEmptyDisturbanceVector = UseEmptyDisturbanceVector,
                ObjectTexture = objectTexture?.ConvertToBitmapWrapper(),
                NormalMap = normalMap?.ConvertToBitmapWrapper(),
                HeightMap = heightMap?.ConvertToBitmapWrapper(),
                LightColor = new Vector3((float)LightColor.R/255, (float)LightColor.G/255, (float)LightColor.B/255),
                ObjectColor = ObjectColor,
                Radius = Radius,
                UseFixedLightSource = UseFixedLightSource,
                UseFixedNormalVector = UseFixedNormalVector,
                UseFixedObjectColor = UseFixedObjectColor,
                Factor = Factor,
                UseMouseFollowNormalVector = UseMouseFollowNormalVector
            };
        }
    }
}
