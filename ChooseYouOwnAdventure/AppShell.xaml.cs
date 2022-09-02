using ChooseYouOwnAdventure.View;

namespace ChooseYouOwnAdventure;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(StoryView), typeof(StoryView));
	}
}
