using Microsoft.Extensions.Logging;

namespace FE4ColCal_MAUI_TDD;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	//FE4ColCal_Test用の.net7.0ターゲットでビルドを通すためだけの、ダミーのエントリポイント
#if !IOS && !ANDROID && !MACCATALYST
	static void Main(string[] args)
	{

	}
#endif
}

