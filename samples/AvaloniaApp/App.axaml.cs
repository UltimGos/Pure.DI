using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AvaloniaApp;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (Resources[nameof(Composition)] is Composition composition)
        {
            // Assigns the main window/view
            switch (ApplicationLifetime)
            {
                case IClassicDesktopStyleApplicationLifetime desktop:
                    desktop.MainWindow = composition.MainWindow;
                    break;

                case ISingleViewApplicationLifetime singleViewPlatform:
                    singleViewPlatform.MainView = composition.MainWindow;
                    break;
            }

            // Handles disposables
            if (ApplicationLifetime is IControlledApplicationLifetime controlledApplicationLifetime)
            {
                controlledApplicationLifetime.Exit += (_, _) => composition.Dispose();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}