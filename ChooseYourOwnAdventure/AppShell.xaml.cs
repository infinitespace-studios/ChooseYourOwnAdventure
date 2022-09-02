using ChooseYourOwnAdventure.View;

namespace ChooseYourOwnAdventure;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(StoryView), typeof(StoryView));
	}
}
