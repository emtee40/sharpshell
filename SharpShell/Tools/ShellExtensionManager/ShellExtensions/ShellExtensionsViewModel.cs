﻿using System.Collections.ObjectModel;
using Apex.MVVM;
using SharpShell.ServerRegistration;

namespace ShellExtensionManager.ShellExtensions
{
    [ViewModel]
    public class ShellExtensionsViewModel : ViewModel
    {
        public ShellExtensionsViewModel()
        {
            //  Create the RefreshExtensions Asynchronous Command.
            RefreshExtensionsCommand = new AsynchronousCommand(DoRefreshExtensionsCommand);

            RefreshExtensionsCommand.DoExecute();
        }

        /// <summary>
        /// Performs the  command.
        /// </summary>
        /// <param name="parameter">The RefreshExtensions command parameter.</param>
        private void DoRefreshExtensionsCommand(object parameter)
        {
            //  Get all servers.
            var servers = ServerRegistrationManager.EnumerateRegisteredExtensions(RegistrationScope.OS64Bit);
            foreach (var server in servers)
            {
                var installation =
                    ServerRegistrationManager.GetExtensionInstallationInfo(server.ServerClassId,
                        RegistrationScope.OS64Bit);
                var sharpServer = installation?.GetSharpShellServerInformation();

                if (sharpServer != null)
                {
                    var extensionViewModel = new ExtensionViewModel();
                    extensionViewModel.DisplayName = sharpServer.DisplayName;
                    extensionViewModel.ShellExtensionType = sharpServer.ShellExtensionType;
                    foreach (var classReg in server.Associations)
                        extensionViewModel.ClassRegistrations.Add(classReg);
                    RefreshExtensionsCommand.ReportProgress(() => Extensions.Add(extensionViewModel));
                }
            }
        }

        /// <summary>
        /// Gets the RefreshExtensions command.
        /// </summary>
        /// <value>The value of .</value>
        public AsynchronousCommand RefreshExtensionsCommand
        {
            get;
            private set;
        }

        
        /// <summary>
        /// The Extensions observable collection.
        /// </summary>
        private readonly ObservableCollection<ExtensionViewModel> ExtensionsProperty =
          new ObservableCollection<ExtensionViewModel>();

        /// <summary>
        /// Gets the Extensions observable collection.
        /// </summary>
        /// <value>The Extensions observable collection.</value>
        public ObservableCollection<ExtensionViewModel> Extensions
        {
            get { return ExtensionsProperty; }
        }

        
        /// <summary>
        /// The NotifyingProperty for the SelectedExtension property.
        /// </summary>
        private readonly NotifyingProperty SelectedExtensionProperty =
          new NotifyingProperty("SelectedExtension", typeof(ExtensionViewModel), default(ExtensionViewModel));

        /// <summary>
        /// Gets or sets SelectedExtension.
        /// </summary>
        /// <value>The value of SelectedExtension.</value>
        public ExtensionViewModel SelectedExtension
        {
            get { return (ExtensionViewModel)GetValue(SelectedExtensionProperty); }
            set { SetValue(SelectedExtensionProperty, value); }
        }
    }
}
