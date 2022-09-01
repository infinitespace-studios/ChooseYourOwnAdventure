using Microsoft.Extensions.DependencyInjection.Extensions;
using ChoseYouOwnAdventure.Service;
using ChoseYouOwnAdventure.ViewModel;
using ChoseYouOwnAdventure.View;

namespace ChoseYouOwnAdventure;

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

		builder.Services.AddSingleton<StoryService>();

		// Main Page Setup
		builder.Services.AddSingleton<StoriesViewModel>();
		builder.Services.AddSingleton<MainPage>();

		// Details Page Setup
		builder.Services.AddTransient<StoryViewModel>();
		builder.Services.AddTransient<StoryView>();

		return builder.Build();
	}
}

