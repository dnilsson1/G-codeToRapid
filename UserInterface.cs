using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.Controllers;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.Controllers.Discovery;

namespace GcodeImporter
{
    public partial class UserInterface : Form
    {
        #region fields
        //default value of the declared variables
        string Tool = "tool0";                           //tool0 is a default for strTool string object reference
        string WorkObj = "wobj0";                           //Default : Wobj0   
        string Zone = "fine";                            //Default : fine
        string Speed = "v5";                             //Default : v5
        string Resolution = "5";
        string RotXvar = "0";
        string RotYvar = "0";
        string RotZvar = "0";

        Controller controller = default(Controller);                              //reference to virtual controller of active system in RobotStudio
        #endregion

        public UserInterface()
        {
            InitializeComponent();
            controller = GetVirtualController();            //get value of virtual controller

            UpdateToolList();
            comboBoxTool.SelectedIndex = 0;
            UpdateWobjList();
            comboBoxWorkObj.SelectedIndex = 0;
            UpdateSpeedList();
            if (comboBoxSpeed.Items.Count > 0)
            {
                comboBoxSpeed.SelectedIndex = 0;
            }
            UpdateZoneList();
            if (comboBoxZone.Items.Count > 0)
            {
                comboBoxZone.SelectedIndex = 0;
            }
            UpdateResolutionList();
            if (comboBoxResolution.Items.Count > 0)
            {
                comboBoxResolution.SelectedIndex = 0;
            }

            Station station = Project.ActiveProject as Station;

            station.ActiveTask.ActiveWorkObjectChanged += new ProjectObjectPropertyChangedEventHandler(ActiveTask_ActiveWorkObjectChanged);
            station.ActiveTask.ActiveToolChanged += new ProjectObjectPropertyChangedEventHandler(ActiveTask_ActiveToolChanged);
            station.ActiveTaskChanged += new ProjectObjectPropertyChangedEventHandler(station_ActiveTaskChanged);
        }

        #region methods This part is mainly borrowed from ABB's example code
        /// <summary>
        /// Get instance of virtual controller of active system in RobotStudio
        /// </summary>
        /// <returns>Virtual controller</returns>
        private static Controller GetVirtualController()
        {
            //variable used to capture the active controller
            Controller controller = default(Controller);
            Station station = Station.ActiveProject as Station;

            //get active task controller
            RsIrc5Controller rsActiveController = station.ActiveTask.Parent as RsIrc5Controller;

            if (rsActiveController != null)
            {
                //Create an object of networkscanner to scan the network for controllers
                //controllers can either be a virtual or real
                NetworkScanner scanner = null;
                if (scanner == null)
                    scanner = new NetworkScanner();
                scanner.Scan();

                //Get virtual controllers from scanner object
                ControllerInfo[] arrControllerInfo = scanner.GetControllers(NetworkScannerSearchCriterias.Virtual);

                if (arrControllerInfo.Length > 0)
                {
                    for (int i = 0; i < arrControllerInfo.Length; i++)
                    {
                        string virtSystemId = string.Concat("{", arrControllerInfo[i].SystemId.ToString().ToUpper(), "}");

                        //Compares the SystemId of virtual controller and RobotStudio Controller
                        if (arrControllerInfo[i].BaseDirectory != null && (virtSystemId.ToUpper().Equals(
                        rsActiveController.SystemId, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            controller = ControllerFactory.CreateFrom(arrControllerInfo[i]);
                            return controller;
                        }
                    }
                }
            }
            return controller;
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Add tools to cmbTool items list
        /// </summary>
        private void UpdateToolList()
        {
            try
            {
                Station station = Project.ActiveProject as Station;
                comboBoxTool.Items.Clear();

                foreach (RsDataDeclaration rsd in station.ActiveTask.DataDeclarations)
                {
                    if (rsd is RsToolData)
                        comboBoxTool.Items.Add(rsd.Name);
                }
            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.StackTrace, LogMessageSeverity.Error));
            }
        }
        /// <summary>
        /// Add workobjects to cmbWobj items list
        /// </summary>
        private void UpdateWobjList()
        {
            try
            {
                Station station = Project.ActiveProject as Station;
                comboBoxWorkObj.Items.Clear();

                foreach (RsDataDeclaration rsd in station.ActiveTask.DataDeclarations)
                {
                    if (rsd is RsWorkObject)
                        comboBoxWorkObj.Items.Add(rsd.Name);
                }
            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.StackTrace, LogMessageSeverity.Error));
            }
        }
        /// <summary>
        /// Update and sort the list of Speed
        /// </summary>
        private void UpdateSpeedList()
        {
            try
            {
                List<string> lstSpeed = new List<string>();

                //Gets all the constant speed from rapid symbols and add to arraylist

                comboBoxSpeed.Items.Clear();
                int i = 0;
                int speedVals = 0;
                while (speedVals <= 2000)
                {
                    comboBoxSpeed.Items.Add("v" + speedVals);
                    speedVals = speedVals + 200;
                    i++;
                }

            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.StackTrace, LogMessageSeverity.Error));
            }
        }
        /// <summary>
        /// Update and sort the list of Zones
        /// </summary>
        private void UpdateZoneList()
        {
            try
            {
                comboBoxZone.Items.Clear();
                int i = 0;
                while (i <= 100)
                {
                    comboBoxZone.Items.Add("z" + i);
                    i = i + 10;
                }

            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.StackTrace, LogMessageSeverity.Error));
            }
        }
        #endregion

        #region update the resolution ComboBox
        private void UpdateResolutionList()
        {
            try
            {
                List<string> lstResolution = new List<string>();

                comboBoxResolution.Items.Clear();
                int i = 0;
                int Res = 5;
                while (Res <= 25)
                {
                    comboBoxResolution.Items.Add(Res);
                    Res = Res + 2;
                    i++;
                }
            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.StackTrace, LogMessageSeverity.Error));
            }
        }
        #endregion

        #region event handlers
        /// <summary>
        /// Event handler which listen the active task changed event of station
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">ProjectObjectPropertyChangedEventArgs object</param>
        void station_ActiveTaskChanged(object sender, ProjectObjectPropertyChangedEventArgs e)
        {
            UpdateToolList();

            UpdateWobjList();
        }
        /// <summary>
        /// Event handler used to listen the active tool changed event of active task
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">ProjectObjectPropertyChangedEventArgs object</param>
        void ActiveTask_ActiveToolChanged(object sender, ProjectObjectPropertyChangedEventArgs e)
        {
            UpdateToolList();
        }
        /// <summary>
        /// Event handler used to listen the active workobject changed event of active station
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">ProjectObjectPropertyChangedEventArgs object</param>
        void ActiveTask_ActiveWorkObjectChanged(object sender, ProjectObjectPropertyChangedEventArgs e)
        {
            UpdateWobjList();
        }
        /// <summary>
        /// Event handler used to listen the user click action on the Open or Save button of file dialog box.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">CancelEventArgs object</param>
        /// <summary>
        /// Launches open file dialogbox to get input from user 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  Event handler used to read the comboBoxWorkObj click action
        /// </summary>
        private void comboBoxWorkObj_Click(object sender, System.EventArgs e)
        {
            UpdateWobjList();
        }
        /// <summary>
        ///  Event handler used to read the comboBoxTool click action
        /// </summary>
        private void comboBoxTool_Click(object sender, System.EventArgs e)
        {
            UpdateToolList();
        }
        /// <summary>
        /// <summary>
        /// Event handler used to listen the comboBoxSpeed click action
        /// </summary>
        private void comboBoxSpeed_Click(object sender, System.EventArgs e)
        {
            UpdateSpeedList();
        }
        /// <summary>
        ///  Event handler used to listen the comboBoxZone click action
        /// </summary>
        private void comboBoxZone_Click(object sender, System.EventArgs e)
        {
            UpdateZoneList();
        }
        /// <summary>
        /// Event handler used to listen the comboBoxWobj selected index changed action
        /// </summary>
        private void comboBoxWobj_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkObj = comboBoxWorkObj.SelectedItem.ToString();
        }
        /// <summary>
        /// Event handler used to listen the comboBoxTool selected index changed action
        /// </summary>
        private void comboBoxTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tool = comboBoxTool.SelectedItem.ToString();
        }
        /// <summary>
        /// Event handler used to listen the comboBoxSpeed selected index changed action
        /// </summary>
        private void comboBoxSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            Speed = comboBoxSpeed.SelectedItem.ToString();
        }
        /// <summary>
        /// Event handler used to listen the comboBoxZone selected index changed action
        /// </summary>

        private void comboBoxZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            Zone = comboBoxZone.SelectedItem.ToString();
        }

        /// <summary>
        /// Event handler used to listen the comboBoxResolution selected index changed action
        /// </summary>

        private void comboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            Resolution = comboBoxResolution.SelectedItem.ToString();
        }

        #endregion
        /// <summary>
        /// Event handler used to listen the buttonBrowse click action
        /// </summary>
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void ShowDialog(object sender, CancelEventArgs e)
        {
            Logger.AddMessage(new LogMessage("Your import of file: " + openFileDialog1.FileName + " has started.", LogMessageSeverity.Information));


            Cursor.Current = Cursors.WaitCursor;
            Station station = Project.ActiveProject as Station;

            string fileExtension = System.IO.Path.GetExtension(openFileDialog1.FileName);
            string Path = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
            string Name = System.IO.Path.GetFileName(openFileDialog1.FileName);
            if (fileExtension == ".nc" | fileExtension == ".gcode" | fileExtension == ".cnc")
            {
                Zone = comboBoxZone.SelectedItem.ToString();
                Speed = comboBoxSpeed.SelectedItem.ToString();
                Resolution = comboBoxResolution.SelectedItem.ToString();
                RotXvar = RotX.Text;
                RotYvar = RotY.Text;
                RotZvar = RotZ.Text;

                RsToolData robotToolData = new RsToolData();
                RsWorkObject robotWorkObj = new RsWorkObject();



                foreach (RsDataDeclaration rsd in station.ActiveTask.DataDeclarations)
                {
                    if (rsd is RsToolData)
                        if (rsd.Name == Tool)
                            robotToolData = rsd as RsToolData;
                    station.ActiveTask.ActiveTool = robotToolData;
                    if (rsd is RsWorkObject)
                        if (rsd.Name == WorkObj)
                        {
                            robotWorkObj = rsd as RsWorkObject;
                            station.ActiveTask.ActiveWorkObject = robotWorkObj;
                        }
                }

                Read read1 = new Read();
                read1.ReadGcode(Path, Name, Globals.positions, robotToolData, robotWorkObj, Speed, Zone, Resolution, RotXvar, RotYvar, RotZvar);
                // read1.CreatePath(Name, Globals.positions, robotToolData, robotWorkObj, Speed, Zone);
                // THen do the import
                Logger.AddMessage(new LogMessage("Enjoy your new auto generated targets and paths." + "\n" + "They are now ready!"));
            }

            else
            {
                Logger.AddMessage(new LogMessage("Ooops.. The file import failed due to file problems... Please check that the fileformat is correct and that the file isn't corrupted"));
                //Show message not correct file...
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                comboBoxSpeed.Visible = false;
                comboBoxZone.Visible = false;
                comboBoxResolution.Visible = false;
                RotX.Visible = false;
                RotY.Visible = false;
                RotZ.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;

                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;

            }

            else if (checkBox1.Checked == true)
            {
                comboBoxSpeed.Visible = true;
                comboBoxZone.Visible = true;
                comboBoxResolution.Visible = true;
                RotX.Visible = true;
                RotY.Visible = true;
                RotZ.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;

            }
        }






        internal static void checkBox1_CheckedChanged()
        {
            throw new NotImplementedException();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void RotX_TextChanged(object sender, EventArgs e)
        {
            RotXvar = RotX.Text;
        }

        private void RotY_TextChanged(object sender, EventArgs e)
        {
            RotYvar = RotY.Text;
        }

        private void RotZ_TextChanged(object sender, EventArgs e)
        {
            RotZvar = RotZ.Text;
        }


    }
}





