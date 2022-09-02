usingChooseYourOwnAdventure.View;

namespaceChooseYourOwnAdventure;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(StoryView), typeof(StoryView));
	}
}
