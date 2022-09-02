using Microsoft.Extensions.DependencyInjection.Extensions;
using ChooseYouOwnAdventure.Service;
using ChooseYouOwnAdventure.ViewModel;
using ChooseYouOwnAdventure.View;
using CommunityToolkit.Maui;

namespace ChooseYouOwnAdventure;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
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

