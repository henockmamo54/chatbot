namespace WindowsService1
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FoodAndLibraryInstallerProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.FoodAndLibraryInfoService = new System.ServiceProcess.ServiceInstaller();
            // 
            // FoodAndLibraryInstallerProcessInstaller1
            // 
            this.FoodAndLibraryInstallerProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.FoodAndLibraryInstallerProcessInstaller1.Password = null;
            this.FoodAndLibraryInstallerProcessInstaller1.Username = null;
            // 
            // FoodAndLibraryInfoService
            // 
            this.FoodAndLibraryInfoService.ServiceName = "FoodAndLibraryInfoService";
            this.FoodAndLibraryInfoService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FoodAndLibraryInstallerProcessInstaller1,
            this.FoodAndLibraryInfoService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FoodAndLibraryInstallerProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller FoodAndLibraryInfoService;
    }
}