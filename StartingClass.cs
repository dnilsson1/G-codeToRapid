using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Windows.Forms;



//	To activate this Add-In you have to install it in the RobotStudio Add-In directory,
//  typically C:\Program Files\Common Files\ABB Industrial IT\Robotics IT\RobotStudio\AddIns
//  There are two alternatives: copy SupportSampleRobotStudioAddin.dll to the Add-In directory, or
//  copy SupportSampleRobotStudioAddin.rsaddin to the Add-In directory.
// 
//  The rsaddin file provides additional information to the RobotStudio host before the
//  Add-In assembly has been loaded, as specified in RobotStudioAddin.xsd. It also
//  allows more flexibility in locating the Add-In assembly, which can be useful
//	during development. 
namespace GcodeImporter
{

    public class CoordinateFileRibbon
    {
        #region Fields
        private static RibbonGroup rbnGroupCoordinateFile;
        private static CommandBarButton btnCoordinateFileImport;
        private static ToolWindow windowCoordinateFileImport;
        #endregion

        #region Entry Point function
        /// <summary>
        /// This is the entry point for the add-in
        /// </summary>
        public static void AddinMain()
        {
            //if rbnGroupCF instance refereces to null then add ribbon group
            if (rbnGroupCoordinateFile == null)
                AddRibbonGroup();

            Project.ActiveProjectChanged += new EventHandler(Project_ActiveProjectChanged);

        }
        #endregion
        #region Add ribbon controls
        /// <summary>
        ///  Add Ribbon Group
        /// </summary>
        public static void AddRibbonGroup()
        {
            rbnGroupCoordinateFile = new RibbonGroup("rgCF", "File Import");
            btnCoordinateFileImport = new CommandBarButton("CFI_button", "G-Code Importer");

            // btnCoordinateFileImport.Image = new System.Drawing.Bitmap("TestLogo.bmp");

            btnCoordinateFileImport.DefaultEnabled = true;
            btnCoordinateFileImport.HelpText = "For converting various coordinate files to RobotStudio paths.";
            btnCoordinateFileImport.ExecuteCommand += new ExecuteCommandEventHandler(btnCoordinateFileImport_ExecuteCommand);
            rbnGroupCoordinateFile.Controls.Add(btnCoordinateFileImport);
            UIEnvironment.RibbonTabs["AddIns"].Groups.Add(rbnGroupCoordinateFile);
        }
        static void btnCoordinateFileImport_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
        {
            try
            {
                windowCoordinateFileImport = new ToolWindow();
                windowCoordinateFileImport.PreferredSize = new System.Drawing.Size(180, 100);
                UserInterface uc = new UserInterface();
                windowCoordinateFileImport.Control = uc;
                windowCoordinateFileImport.Caption = "Coordinate File Import";
                UIEnvironment.Windows.AddDocked(windowCoordinateFileImport, DockStyle.Right);
                windowCoordinateFileImport.VisibleChanged += new EventHandler(windowCoordinateFileImport_VisibleChanged);
                btnCoordinateFileImport.DefaultEnabled = false;
            }
            catch (Exception ex)
            {
                Logger.AddMessage(new LogMessage(ex.StackTrace, LogMessageSeverity.Error));
            }
        }
        static void windowCoordinateFileImport_VisibleChanged(object sender, EventArgs e)
        {
            if (windowCoordinateFileImport != null)
            {
                if (windowCoordinateFileImport.Visible)
                    btnCoordinateFileImport.DefaultEnabled = false;
                else
                    btnCoordinateFileImport.DefaultEnabled = true;
            }
        }
        static void Project_ActiveProjectChanged(object sender, EventArgs e)
        {
            if (Project.ActiveProject != null)
            {
                Station station = Project.ActiveProject as Station;
                station.Closed += new EventHandler(station_Closed);
                if (rbnGroupCoordinateFile != null)
                {
                    btnCoordinateFileImport.DefaultEnabled = true;
                }
                else
                {
                    AddRibbonGroup();
                }
            }
            else
                btnCoordinateFileImport.DefaultEnabled = false;
        }
        /// <summary>
        /// Listen post closed station event
        /// </summary>
        static void station_Closed(object sender, EventArgs e)
        {
            if (windowCoordinateFileImport != null)
                windowCoordinateFileImport.Close();
            btnCoordinateFileImport.DefaultEnabled = false;
        }
        #endregion
    }
}















