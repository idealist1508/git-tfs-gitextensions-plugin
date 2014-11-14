using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public partial class GitTfsDialog : Form
    {
        private readonly IGitUICommands _commands;
        private readonly PluginSettings _settings;
        private readonly ISettingsSource _source;

        public GitTfsDialog(IGitUICommands commands, PluginSettings settings, ISettingsSource source, IEnumerable<string> tfsRemotes)
        {
            _commands = commands;
            _settings = settings;
            _source = source;

            InitializeComponent();
            TfsRemoteComboBox.DataSource = tfsRemotes.ToList();
            InitializeFromSettings();
        }

        private void InitializeFromSettings()
        {
            InitializeTfsRemotes();
            InitializePull();
            InitializePush();
        }

        private void InitializeTfsRemotes()
        {
            TfsRemoteComboBox.Text = _settings.TfsRemote[_source];
        }

        private void InitializePull()
        {
            switch(_settings.PullBehaviour)
            {
                case PullBehaviour.Pull:
                    PullRadioButton.Checked = true;
                    break;
                case PullBehaviour.Rebase:
                    RebaseRadioButton.Checked = true;
                    break;
                case PullBehaviour.Fetch:
                    FetchRadioButton.Checked = true;
                    break;
            }

            SetPullButtonEnabledState();
        }

        private void MergeOptionCheckedChanged(object sender, EventArgs e)
        {
            SetPullButtonEnabledState();
        }

        private void SetPullButtonEnabledState()
        {
            PullButton.Enabled = PullRadioButton.Checked || RebaseRadioButton.Checked || FetchRadioButton.Checked;
        }

        private void PullButtonClick(object sender, EventArgs e)
        {
            if (PullRadioButton.Checked)
            {
                _settings.PullBehaviour = PullBehaviour.Pull;
                if (!_commands.StartGitTfsCommandProcessDialog("pull", "--remote " + TfsRemoteComboBox.Text))
                {
                    _commands.StartResolveConflictsDialog();
                }
            }
            else if (RebaseRadioButton.Checked)
            {
                _settings.PullBehaviour = PullBehaviour.Rebase;
                _commands.StartGitTfsCommandProcessDialog("fetch", "--remote " + TfsRemoteComboBox.Text);
                _commands.StartRebaseDialog("tfs/" + TfsRemoteComboBox.Text);
            }
            else if (FetchRadioButton.Checked)
            {
                _settings.PullBehaviour = PullBehaviour.Fetch;
                _commands.StartGitTfsCommandProcessDialog("fetch", "--remote " + TfsRemoteComboBox.Text);
            }
        }

        private void InitializePush()
        {
            switch (_settings.PushBehaviour)
            {
                case PushBehaviour.Checkin:
                    CheckinRadioButton.Checked = true;
                    break;
                case PushBehaviour.Shelve:
                    ShelveRadioButton.Checked = true;
                    break;
                case PushBehaviour.RCheckin:
                    RCheckinRadioButton.Checked = true;
                    break;
            }

            SetPushButtonEnabledState();
        }

        private void PushOptionCheckedChanged(object sender, EventArgs e)
        {
            SetPushButtonEnabledState();
        }

        private void SetPushButtonEnabledState()
        {
            PushButton.Enabled = CheckinRadioButton.Checked || ShelveRadioButton.Checked || RCheckinRadioButton.Checked;
        }

        private void PushButtonClick(object sender, EventArgs e)
        {
            if (CheckinRadioButton.Checked)
            {
                _settings.PushBehaviour = PushBehaviour.Checkin;
                _commands.StartGitTfsCommandProcessDialog("checkintool", "--remote " + TfsRemoteComboBox.Text);
            }
            else if (ShelveRadioButton.Checked)
            {
                _settings.PushBehaviour = PushBehaviour.Shelve;
                new ShelveDialog(_commands, _settings, _source).ShowDialog();
            }
            else if (RCheckinRadioButton.Checked)
            {
                _settings.PushBehaviour = PushBehaviour.RCheckin;
                _commands.StartGitTfsCommandProcessDialog("rcheckin", "--remote " + TfsRemoteComboBox.Text);
            }
            this.Close();
        }

        private void TfsRemoteComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.TfsRemote[_source] = TfsRemoteComboBox.Text;
        }
    }
}
