﻿using Wpf;
using Xamarin.Forms.Platform.WPF;
using SKFormsView = SkiaSharp.Views.Forms.SKCanvasView;
using SKNativeView = SkiaSharp.Views.WPF.SKElement;

[assembly: ExportRenderer(typeof(SKFormsView), typeof(SKCanvasViewRenderer))]

namespace Wpf
{
    public class SKCanvasViewRenderer: SKCanvasViewRendererBase<SKFormsView, SKNativeView>
    {
        
    }
}