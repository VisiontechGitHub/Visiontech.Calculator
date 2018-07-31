using Org.Visiontech.Compute;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalcolatoreXamarin.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AnalizedLensPage : CarouselPage
	{

        public readonly computeLensRequestDTO computeLensRequest;
        public readonly computeLensResponseDTO computeLensResponse;
        public AnalizedLensPage (computeLensRequestDTO computeLensRequest, computeLensResponseDTO computeLensResponse)
		{
            InitializeComponent ();

            this.computeLensRequest = computeLensRequest;
            this.computeLensResponse = computeLensResponse;
        }

        private void LeftCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            PaintSurface(sender, args, computeLensRequest.left, computeLensResponse.left);
        }

        private void RightCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            PaintSurface(sender, args, computeLensRequest.right, computeLensResponse.right);
        }

        private void PaintSurface(object sender, SKPaintSurfaceEventArgs args, computeLensRequestSideDTO computeLensRequestSideDTO, computeLensResponseSideDTO computeLensResponseSideDTO)
        {

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint blackSKPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Black.ToSKColor(),
            };

            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), blackSKPaint);

            if (computeLensResponseSideDTO.points != null & computeLensResponseSideDTO.points.Any()) {

                ICollection<analyzedPointDTO> points = computeLensResponseSideDTO.points.Where(point => point is analyzedPointDTO && Math.Abs(point.x) <= computeLensRequestSideDTO.horizontalDiameter / 2.0 && Math.Abs(point.y) <= computeLensRequestSideDTO.verticalDiameter / 2.0).Select(point => point as analyzedPointDTO).ToList();

                double maxCylinder = points.Select(point => point.cylinderMap).Max();
                double minCylinder = points.Select(point => point.cylinderMap).Min();
                double cylinderRange = (maxCylinder - minCylinder) / 6.0;

                if (cylinderRange > 0)
                {

                    double doubleCylinderRange = cylinderRange * 2;
                    double tripleCylinderRange = cylinderRange * 3;
                    double fiveCylinderRange = cylinderRange * 5;

                    double centerX = info.Width / 2.0;
                    double centerY = info.Height / 2.0;

                    int sqrt = Convert.ToInt32(Math.Sqrt(points.Count));
                    int side = (sqrt - 1) / 2;

                    double bitmapSide = Math.Min(info.Width, info.Height) / 2.0;

                    double sqrtRange = sqrt / 6.0;
                    double doubleSqrtRange = sqrtRange * 2;
                    double tripleSqrtRange = sqrtRange * 3;
                    double fiveSqrtRange = sqrtRange * 5;

                    using (SKBitmap bitmap = new SKBitmap(1, sqrt))
                    {

                        for (int i = sqrt - 1; i >= 0; i--)
                        {

                            SKColor color = Color.Black.ToSKColor();

                            switch (Math.Floor(i / sqrtRange))
                            {

                                case 0:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round(i / sqrtRange * 255)), Convert.ToInt32(Math.Round(i / sqrtRange * 255)), 255).ToSKColor();
                                    break;
                                case 1:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round((doubleSqrtRange - i) / sqrtRange * 255)), 255, 255).ToSKColor();
                                    break;
                                case 2:
                                    color = Color.FromRgb(0, 255, Convert.ToInt32(Math.Round((tripleSqrtRange - i) / sqrtRange * 255))).ToSKColor();
                                    break;
                                case 3:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round((i - tripleSqrtRange) / sqrtRange * 255)), 255, 0).ToSKColor();
                                    break;
                                case 4:
                                    color = Color.FromRgb(255, Convert.ToInt32(Math.Round((fiveSqrtRange - i) / sqrtRange * 255)), 0).ToSKColor();
                                    break;
                                default:
                                    color = Color.FromRgb(255, 0, Convert.ToInt32(Math.Round((i - fiveSqrtRange) / sqrtRange * 255))).ToSKColor();
                                    break;

                            }

                            bitmap.SetPixel(0, i, color);

                        }

                        canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(bitmapSide / 6), Convert.ToInt32(bitmapSide)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(info.Width - bitmapSide / 6), 0, Convert.ToSingle(info.Width), info.Height));

                    }

                    blackSKPaint.TextSize = info.Height / 32;

                    float textRange = (info.Height - blackSKPaint.TextSize * 3) / 6;

                    canvas.DrawText(string.Format("{0:N2}", minCylinder), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 2), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 2, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 3), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 3, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 4), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 4, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 5), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 5, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", maxCylinder), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 6, blackSKPaint);

                    using (SKBitmap bitmap = new SKBitmap(sqrt, sqrt))
                    {

                        foreach (analyzedPointDTO point in points)
                        {

                            SKColor color = Color.Black.ToSKColor();

                            double value = point.cylinderMap - minCylinder;

                            switch (Math.Floor(value / cylinderRange))
                            {

                                case 0:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round(value / cylinderRange * 255)), Convert.ToInt32(Math.Round(value / cylinderRange * 255)), 255).ToSKColor();
                                    break;
                                case 1:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round((doubleCylinderRange - value) / cylinderRange * 255)), 255, 255).ToSKColor();
                                    break;
                                case 2:
                                    color = Color.FromRgb(0, 255, Convert.ToInt32(Math.Round((tripleCylinderRange - value) / cylinderRange * 255))).ToSKColor();
                                    break;
                                case 3:
                                    color = Color.FromRgb(Convert.ToInt32(Math.Round((value - tripleCylinderRange) / cylinderRange * 255)), 255, 0).ToSKColor();
                                    break;
                                case 4:
                                    color = Color.FromRgb(255, Convert.ToInt32(Math.Round((fiveCylinderRange - value) / cylinderRange * 255)), 0).ToSKColor();
                                    break;
                                default:
                                    color = Color.FromRgb(255, 0, Convert.ToInt32(Math.Round((value - fiveCylinderRange) / cylinderRange * 255))).ToSKColor();
                                    break;

                            }

                            bitmap.SetPixel(Convert.ToInt32(side - point.x), Convert.ToInt32(side - point.y), color);

                        }

                        using (SKPath path = new SKPath())
                        {
                            path.AddCircle(Convert.ToSingle(centerX), Convert.ToSingle(centerY), Convert.ToSingle(bitmapSide * 5 / 6));
                            canvas.ClipPath(path, SKClipOperation.Intersect);
                        }

                        canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(bitmapSide * 5 / 6), Convert.ToInt32(bitmapSide * 5 / 6)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(centerX - bitmapSide * 5 / 6), Convert.ToSingle(centerY - bitmapSide * 5 / 6), Convert.ToSingle(centerX + bitmapSide * 5 / 6), Convert.ToSingle(centerY + bitmapSide * 5 / 6)));

                    }

                }

            }

        }

        private void ComputeChart(computeLensRequestSideDTO computeLensRequestSideDTO, computeLensResponseSideDTO computeLensResponseSideDTO, Func<threeDimensionalPointDTO, double> Mapping)
        {

            ICollection<threeDimensionalPointDTO> points = computeLensResponseSideDTO.points.Where(point => point is analyzedPointDTO && Math.Abs(point.x) <= computeLensRequestSideDTO.horizontalDiameter / 2.0 && Math.Abs(point.y) <= computeLensRequestSideDTO.verticalDiameter / 2.0).ToList();

            Navigation.PushAsync(new ThreeDimensionalLensPage(points, Mapping));

        }

        private void Left_Toolbar_3D_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.left, computeLensResponse.left, ToZ);
        }
        private void Right_Toolbar_3D_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.right, computeLensResponse.right, ToZ);
        }
        private void Left_Toolbar_CylinderMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.left, computeLensResponse.left, ToCylinderMap);
        }
        private void Right_Toolbar_CylinderMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.right, computeLensResponse.right, ToCylinderMap);
        }
        private void Left_Toolbar_PowerMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.left, computeLensResponse.left, ToPowerMap);
        }
        private void Right_Toolbar_PowerMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(computeLensRequest.right, computeLensResponse.right, ToPowerMap);
        }

        private double ToZ(threeDimensionalPointDTO point)
        {
            return point.z;
        }
        private double ToCylinderMap(threeDimensionalPointDTO point) {
            return (point as analyzedPointDTO).cylinderMap;
        }
        private double ToPowerMap(threeDimensionalPointDTO point)
        {
            return (point as analyzedPointDTO).powerMap;
        }

    }
}