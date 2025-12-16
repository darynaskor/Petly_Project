using ObjCRuntime;
using UIKit;
using Petly.Maui.Views;

namespace Petly.Maui;

public class Program
{
    static void Main(string[] args)
    {
        // Це стандартний виклик, який запускає AppDelegate, 
        // що, своєю чергою, викликає MauiProgram.CreateMauiApp().
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}