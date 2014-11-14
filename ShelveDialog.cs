using System;
using System.Windows.Forms;
using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public partial class ShelveDialog : Form
    {
        private readonly IGitUICommands _commands;
        private readonly PluginSettings _settings;
        private readonly ISettingsSource _source;

        public ShelveDialog(IGitUICommands commands, PluginSettings settings, ISettingsSource source)
        {
            _commands = commands;
            _settings = settings;
            _source = source;

            InitializeComponent();
            InitializeFromSettings();
        }

        private void InitializeFromSettings()
        {
            NameTextBox.Text = _settings.ShelveSetName[_source];
            OverwriteCheckBox.Checked = _settings.Overwrite[_source] ?? false;
            SetShelveButtonEnabledState();
        }

        private void NameTextBoxTextChanged(object sender, EventArgs e)
        {
            SetShelveButtonEnabledState();
        }

        private void SetShelveButtonEnabledState()
        {
            ShelveButton.Enabled = !string.IsNullOrEmpty(NameTextBox.Text);
        }

        private void ShelveButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text)) return;

            _settings.ShelveSetName[_source] = NameTextBox.Text;
            _settings.Overwrite[_source] = OverwriteCheckBox.Checked;

            _commands.StartGitTfsCommandProcessDialog("shelve", OverwriteCheckBox.Checked ? "-f " : "", NameTextBox.Text);
            Close();
        }
    }
}
