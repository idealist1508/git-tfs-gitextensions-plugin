using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public static class GitUICommandsExtensions
    {
        public static bool StartGitTfsCommandProcessDialog(this IGitUICommands commands, params string[] args)
        {
            var commandResult = commands.StartGitCommandProcessDialog("tfs " + string.Join(" ", args));
            commands.RepoChangedNotifier.Notify();
            return commandResult;
        }
    }
}
