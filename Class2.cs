using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace GcodeImporter
{
    public class Read
    {
        public RsTask ActiveTask { get; set; } // Setting the active task
        public static bool EndOfDocument = false;

        public Double[] arrPositions; // Stores the current Position data
        public Double[] AbsPos; // Stores the incremental Position change
        public Double[] lastPos; // Stores the previous position Data
        public Double[] incrPos; // Stores the incremental Position change
        public static bool Absolute; // Activates or Deactivates if the function for absolute value translation should be executed or not. 
        public static int pathnumber = 0;
        public static RsWorkObject WorkObj;
        public static RsToolData toolDat;
        public static string spd;
        public static string res;
        public static string xrot;


        public void ReadGcode(string FilePath, string fileName, Double[] Positions,
    RsToolData toolData, RsWorkObject workObject, string speed, string zone, string resolution, string Xrot, string Yrot, string Zrot)
        {
            arrPositions = new Double[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            lastPos = new Double[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            AbsPos = new Double[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            spd = speed;
            res = resolution;
            xrot = Xrot;

            for (int t = 0; t < 8;)
            {
                arrPositions[t] = 0;
                t++;
            };

            // Initiates a new streamreader
            StreamReader sr = new StreamReader(FilePath);
            //foreach (string line in File.ReadLines(@"C:\Users\dani0007\Documents\Gcode.txt", Encoding.UTF8))
            // string Saved;
            string input;
            // The pattern searches for the letters [ngxyzijkf] followed by a + or - if they exist followed by one or more digits. then if zero or more dots followed by one or more digits, this to exclude the number from ending with a dot. 

            string Pattern = @"[ngxyzijkfabc][+-]?(\d+)(.\d+)*";
            if (sr.Peek() <= 0)
            {
                EndOfDocument = true;
            }
            while (sr.Peek() >= 0)
            {
                input = sr.ReadLine();

                Regex rgx = new Regex(Pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(input);

                if (matches.Count > 0)
                {


                    string[] substrings = Regex.Split(input, @"/n");

                    foreach (string match in substrings)
                    {
                        //Console.WriteLine("Splittar: " + "'{0}'", match);

                    }

                    //Console.WriteLine("{");
                    for (int ctr = 0; ctr < substrings.Length; ctr++)
                    {

                        //Console.WriteLine(substrings[ctr]);


                        Regex rgx2 = new Regex(Pattern, RegexOptions.IgnoreCase);
                        MatchCollection matches2 = rgx.Matches(substrings[ctr]);

                        #region  Loop that Sorts the values to differnt locations in the Array arrPositions[]
                        foreach (Match match in matches2)
                        {
                            if (Absolute == false) { Logger.AddMessage(new LogMessage("Value is false")); }
                            if (Absolute == true) { Logger.AddMessage(new LogMessage("Value is true")); }

                            if (match.Value == "G02" | match.Value == "G2" | match.Value == "G03" | match.Value == "G3")
                            {
                                arrPositions[4] = 0;
                                arrPositions[5] = 0;
                                arrPositions[6] = 0;
                            }

                            if (Absolute == false && (match.Value == "G0" | match.Value == "G01" | match.Value == "G1" | match.Value == "G02" | match.Value == "G2" | match.Value == "G03" | match.Value == "G3"))
                            {
                                arrPositions[1] = 0;
                                arrPositions[2] = 0;
                                arrPositions[3] = 0;

                            }

                            if (match.Value == "G0" | match.Value == "G01" | match.Value == "G1" | match.Value == "G02" | match.Value == "G2" | match.Value == "G03" | match.Value == "G3" | match.Value == "G90" | match.Value == "G91")
                            {
                                int StartG = match.Index + 1;
                                int LengthG = match.Length;
                                int EndG = (match.Index + match.Length);
                                string ValueG = match.Value.Remove(0, 1);
                                Double DoubleG = Double.Parse(ValueG, CultureInfo.InvariantCulture);
                                arrPositions[0] = DoubleG;
                                if (Absolute == true) { Logger.AddMessage(new LogMessage("Value: " + match.Value)); }
                            }


                            if (match.Value.StartsWith("X") | match.Value.StartsWith("x"))
                            {
                                int StartX = match.Index + 1;
                                int LengthX = match.Length;
                                int EndX = (match.Index + match.Length);
                                string ValueX = match.Value.Remove(0, 1);
                                Double DoubleX = Double.Parse(ValueX, CultureInfo.InvariantCulture);

                                arrPositions[1] = DoubleX;

                            }


                            if (match.Value.StartsWith("Y") | match.Value.StartsWith("y"))
                            {
                                int StartY = match.Index + 1;
                                int LengthY = match.Length;
                                int EndY = (match.Index + match.Length);
                                string ValueY = match.Value.Remove(0, 1);
                                Double DoubleY = Double.Parse(ValueY, CultureInfo.InvariantCulture);

                                arrPositions[2] = DoubleY;
                            }


                            if (match.Value.StartsWith("Z") | match.Value.StartsWith("z"))
                            {
                                int StartZ = match.Index + 1;
                                int LengthZ = match.Length;
                                int EndZ = (match.Index + match.Length);
                                string ValueZ = match.Value.Remove(0, 1);
                                Double DoubleZ = Double.Parse(ValueZ, CultureInfo.InvariantCulture);

                                arrPositions[3] = DoubleZ;
                            }


                            if (match.Value.StartsWith("I") | match.Value.StartsWith("i"))
                            {
                                int StartI = match.Index + 1;
                                int LengthI = match.Length;
                                int EndI = (match.Index + match.Length);
                                string ValueI = match.Value.Remove(0, 1);
                                Double DoubleI = Double.Parse(ValueI, CultureInfo.InvariantCulture);

                                arrPositions[4] = DoubleI;
                            }


                            if (match.Value.StartsWith("J") | match.Value.StartsWith("j"))
                            {
                                int StartJ = match.Index + 1;
                                int LengthJ = match.Length;
                                int EndJ = (match.Index + match.Length);
                                string ValueJ = match.Value.Remove(0, 1);
                                Double DoubleJ = Double.Parse(ValueJ, CultureInfo.InvariantCulture);

                                arrPositions[5] = DoubleJ;
                            }


                            if (match.Value.StartsWith("K") | match.Value.StartsWith("k"))
                            {
                                int StartK = match.Index + 1;
                                int LengthK = match.Length;
                                int EndK = (match.Index + match.Length);
                                string ValueK = match.Value.Remove(0, 1);
                                Double DoubleK = Double.Parse(ValueK, CultureInfo.InvariantCulture);

                                arrPositions[6] = DoubleK;
                            }

                            if (match.Value.StartsWith("F") | match.Value.StartsWith("f"))
                            {
                                int StartF = match.Index + 1;
                                int LengthF = match.Length;
                                int EndF = (match.Index + match.Length);
                                string ValueF = match.Value.Remove(0, 1);
                                Double DoubleF = Double.Parse(ValueF, CultureInfo.InvariantCulture);

                                arrPositions[7] = DoubleF;

                            }

                            if (match.Value.StartsWith("A") | match.Value.StartsWith("a"))
                            {
                                /* int StartF = match.Index + 1;
                                 int LengthF = match.Length;
                                 int EndF = (match.Index + match.Length);
                                 string ValueA = match.Value.Remove(0, 1);
                                 Double DoubleA = Double.Parse(ValueA, CultureInfo.InvariantCulture);

                                 arrPositions[8] = DoubleA;*/

                            }

                            if (match.Value.StartsWith("B") | match.Value.StartsWith("b"))
                            {
                                /*int StartF = match.Index + 1;
                                int LengthF = match.Length;
                                int EndF = (match.Index + match.Length);
                                string ValueB = match.Value.Remove(0, 1);
                                Double DoubleB = Double.Parse(ValueB, CultureInfo.InvariantCulture);

                                arrPositions[9] = DoubleB;*/


                            }

                            if (match.Value.StartsWith("C") | match.Value.StartsWith("c"))
                            {
                                /*int StartF = match.Index + 1;
                                int LengthF = match.Length;
                                int EndF = (match.Index + match.Length);
                                string ValueC = match.Value.Remove(0, 1);
                                Double DoubleC = Double.Parse(ValueC, CultureInfo.InvariantCulture);

                                arrPositions[10] = DoubleC;*/


                            }
                        }
                        #endregion

                        if (Xrot != "0" | Yrot != "0" | Zrot != "0")
                        {
                            arrPositions[8] = Double.Parse(Xrot, CultureInfo.InvariantCulture);
                            arrPositions[9] = Double.Parse(Yrot, CultureInfo.InvariantCulture);
                            arrPositions[10] = Double.Parse(Zrot, CultureInfo.InvariantCulture);
                        }

                        if (arrPositions[0] == 90) // Sets absoulte coordintes to true
                        {
                            Absolute = true;

                        }
                        else if (arrPositions[0] == 91) // Sets incremental coordinates to true
                        {
                            Absolute = false;

                        }
                        Globals.positions = arrPositions;
                        #region Handles Absolut or incremental Coordinate system switching

                        if (Absolute == false && (arrPositions[0] == 00 | arrPositions[0] == 0 | arrPositions[0] == 01 | arrPositions[0] == 1 | arrPositions[0] == 02 | arrPositions[0] == 2 | arrPositions[0] == 03 | arrPositions[0] == 3))
                        {

                            arrPositions[1] = arrPositions[1] + lastPos[1];
                            arrPositions[2] = arrPositions[2] + lastPos[2];
                            arrPositions[3] = arrPositions[3] + lastPos[3];

                            //Globals.positions = arrPositions;
                            if (arrPositions[0] == 00 | arrPositions[0] == 0 | arrPositions[0] == 01 | arrPositions[0] == 1)
                            {
                                CreateTargets(arrPositions);
                                for (int i = 0; i < arrPositions.Length; i++)
                                {
                                    lastPos[i] = arrPositions[i];
                                }
                            }
                            else
                            {
                                arrPositions[4] = arrPositions[4] + lastPos[4];
                                arrPositions[5] = arrPositions[5] + lastPos[5];
                                arrPositions[6] = arrPositions[6] + lastPos[6];
                                Parameterize(arrPositions, lastPos, resolution);
                                for (int i = 0; i < arrPositions.Length; i++)
                                {
                                    lastPos[i] = arrPositions[i];
                                }
                            }
                        }



                        //Logger.AddMessage(new LogMessage("if(abs=false & G00/G01) ")); 


                        else
                        {
                            Globals.positions = arrPositions;
                            if (arrPositions[0] == 00 | arrPositions[0] == 0 | arrPositions[0] == 01 | arrPositions[0] == 1)
                            {
                                CreateTargets(arrPositions);
                                for (int i = 0; i < arrPositions.Length; i++)
                                {
                                    lastPos[i] = arrPositions[i];
                                }
                            }
                            else
                            {
                                Parameterize(arrPositions, lastPos, resolution);
                                for (int i = 0; i < arrPositions.Length; i++)
                                {
                                    lastPos[i] = arrPositions[i];
                                }
                            }
                        }
                        #endregion

                    }
                }
            }
            //a++;
            sr.Close();
            CreatePath(toolData, workObject, speed, zone);
        }

        public double[] AbsoluteValues(double[] prevpos)
        {
            double[] AbsoluteVal = new double[11];

            for (int i = 0; i < AbsoluteVal.Length; i++)
            {
                AbsoluteVal[i] = 0;
            }
            return AbsoluteVal;
        }

        //public static Vector3 GetOrientation(Double dblRX, Double dblRY, Double dblRZ)
        //{
        //    Vector3 vecActOri = new Vector3();
        //    try
        //    {
        //        if (dblRX!= 0 | dblRY != 0 | dblRZ != 0)
        //        {
        //            //Calculating the orientation.        
        //            //.cls and .nc files
        //            double dblRR = System.Math.Sqrt(dblRX * dblRX + dblRY * dblRY);
        //            vecActOri.x = 0;
        //            vecActOri.y = System.Math.Atan2(dblRR, dblRZ);
        //            vecActOri.z = System.Math.Atan2(dblRY, dblRX);
        //        }
        //        else
        //        {
        //            vecActOri.x = 0;
        //            vecActOri.y = 0;
        //            vecActOri.z = 0;
        //        }
        //        return vecActOri;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddMessage(new LogMessage(ex.StackTrace, LogMessageSeverity.Error));
        //        return vecActOri;
        //    }
        //}

        static void Parameterize(double[] input, double[] lastinput, string resolution)
        {
            //Begin UndoStep
            Project.UndoContext.BeginUndoStep("CreateTarget");

            try
            {

                //The function calculates the paremeterization of a curve. It calculates points along a curve between two vectors with the specified resulotion "res"



                double res = Double.Parse(resolution, CultureInfo.InvariantCulture); // Converts the resolution string to a double
                double endVal = 1;
                double stepSize = endVal / (res - 1);
                double iaprox = 0;


                double[] A = new double[3] { 0, 0, 0 };
                double[] Center = new double[3] { 0, 0, 0 };
                double[] B = new double[3] { 0, 0, 0 };
                double[] AO = new double[3] { 0, 0, 0 };
                double[] varS = new double[3] { 0, 0, 0 };

                #region Calculates the center arc point for MoveC
                // Logger.AddMessage(new LogMessage("Targets: " + Globals.positions[1] + "Y" + Globals.positions[2] + " Z" + Globals.positions[3] + "I" + Globals.positions[4]) + "J" + Globals.positions[5] + "K" + Globals.positions[6]);
                //Logger.AddMessage(new LogMessage("LastTargets: " + Globals.lastPos[1] + "Y" + Globals.lastPos[2] + " Z" + Globals.lastPos[3] + "I" + Globals.lastPos[4]) + "J" + Globals.lastPos[5] + "K" + Globals.lastPos[6]);   
                //converts the current point values into millimeters

                B[0] = input[1] / 1000;
                B[1] = input[2] / 1000;
                B[2] = input[3] / 1000;

                // Collects the previous points values and converts them into millimeters
                A[0] = lastinput[1] / 1000;
                A[1] = lastinput[2] / 1000;
                A[2] = lastinput[3] / 1000;

                // Calculates the centerpoint of the arc
                Center[0] = (lastinput[1] + input[4]) / 1000;
                Center[1] = (lastinput[2] + input[5]) / 1000;
                Center[2] = (lastinput[3] + input[6]) / 1000;
                // Logger.AddMessage(new LogMessage("Targets: " + input[1] + "Y" + input[2] + " Z" + input[3] + "I" + input[4]) + "J" + input[5] + "K" + input[6]);             

                #endregion
                //Calculating the distance between StartPoint and SteeringPoint
                AO[0] = A[0] - Center[0];
                AO[1] = A[1] - Center[1];
                AO[2] = A[2] - Center[2];

                // Normalizing the vector AO
                double normAO = Math.Sqrt((AO[0] * AO[0]) + (AO[1] * AO[1]) + (AO[2] * AO[2]));

                while (iaprox <= endVal)
                {


                    // Stepping from Start point to end point
                    varS[0] = ((iaprox * (B[0] - Center[0])) + ((1 - iaprox) * (AO[0])));
                    varS[1] = ((iaprox * (B[1] - Center[1])) + ((1 - iaprox) * (AO[1])));
                    varS[2] = ((iaprox * (B[2] - Center[2])) + ((1 - iaprox) * (AO[2])));

                    //normalizing the vector to get the direction 
                    double normS = Math.Sqrt((varS[0] * varS[0]) + (varS[1] * varS[1]) + (varS[2] * varS[2]));

                    //Multiplying the normalized vector with the radius
                    double cPointX = (Center[0] + ((varS[0] / normS) * normAO));
                    double cPointY = (Center[1] + ((varS[1] / normS) * normAO));
                    double cPointZ = (Center[2] + ((varS[2] / normS) * normAO));

                    // Solves the problem of zero by zero division resulting in NaN
                    if (double.IsNaN(cPointX))
                    {
                        cPointX = 0;
                    }
                    if (double.IsNaN(cPointY))
                    {
                        cPointY = 0;
                    }
                    if (double.IsNaN(cPointZ))
                    {
                        cPointZ = 0;
                    }

                    Vector3 cPos = new Vector3((cPointX), (cPointY), (cPointZ));

                    /*   
                        Logger.AddMessage(new LogMessage("TargetsX: " + cPointX));
                        Logger.AddMessage(new LogMessage("TargetsY: " + cPointY));
                        Logger.AddMessage(new LogMessage("TargetsZ: " + cPointZ));
                        Logger.AddMessage(new LogMessage("pos: " + input[2]));
                        Logger.AddMessage(new LogMessage("lastpos: " + lastinput[2]));
                   */

                    #region
                    //Setting target orientation
                    Vector3 TargetOrientation = new Vector3(Convert.ToDouble(input[8]), Convert.ToDouble(input[9]), // <<---------------------------------------------- Needs input fields for 3 axis machines
                         Convert.ToDouble(input[10]));

                    #endregion
                    // Create the first robotstudio target.
                    ShowTarget(cPos, TargetOrientation);
                    iaprox = iaprox + stepSize;


                }

            }
            catch (Exception exception)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
            finally
            {
                //End UndoStep
                Project.UndoContext.EndUndoStep();
            }



        }


        private static void CreateTargets(Double[] positions)
        {
            //Begin UndoStep
            Project.UndoContext.BeginUndoStep("CreateTarget");

            try
            {
                double PosX = positions[1] / 1000;
                double PosY = positions[2] / 1000;
                double PosZ = positions[3] / 1000;

                Vector3 pos = new Vector3(PosX, PosY, PosZ);

                #region
                //Setting target orientation
                Vector3 TargetOrientation = new Vector3(Convert.ToDouble(positions[8]), Convert.ToDouble(positions[9]), // <<---------------------------------------------- Needs input fields for 3 axis machines
                             Convert.ToDouble(positions[10]));



                #endregion
                // Create the first robotstudio target.

                ShowTarget(pos, TargetOrientation);


            }
            catch (Exception exception)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
            finally
            {
                //End UndoStep
                Project.UndoContext.EndUndoStep();
            }
        }



        private static void ShowTarget(Vector3 position, Vector3 orientation)
        {
            try
            {
                //get the active station
                Station station = Project.ActiveProject as Station;

                //create robtarget
                RsRobTarget robTarget = new RsRobTarget();
                robTarget.Name = station.ActiveTask.GetValidRapidName("Target", "_", 10);

                //translation
                robTarget.Frame.Translation = position;

                // Otrienting the targets and converts the orientation to radians 
                robTarget.Frame.RX = (orientation.x * (Math.PI / 180));
                robTarget.Frame.RY = (orientation.y * (Math.PI / 180));
                robTarget.Frame.RZ = (orientation.z * (Math.PI / 180));

                //add robtargets to datadeclaration
                station.ActiveTask.DataDeclarations.Add(robTarget);

                //create target
                RsTarget target = new RsTarget(station.ActiveTask.ActiveWorkObject, robTarget);
                target.Name = robTarget.Name;
                target.Attributes.Add(target.Name, true);

                //add targets to active task
                station.ActiveTask.Targets.Add(target);




            }
            catch (Exception exception)
            {
                Logger.AddMessage(new LogMessage(exception.Message.ToString()));
            }
        }

        private static void CreatePath(RsToolData SelectedToolData, RsWorkObject SelectedWorkObject, string speed, string zone)
        {
            Project.UndoContext.BeginUndoStep("RsPathProcedure Create");

            try
            {
                //Get the active Station
                Station station = Project.ActiveProject as Station;
                // Create a PathProcedure.

                RsPathProcedure myPath = new RsPathProcedure("AutoPath" + pathnumber);

                // Add the path to the ActiveTask.
                station.ActiveTask.PathProcedures.Add(myPath);
                myPath.ModuleName = "module1";
                myPath.ShowName = true;
                myPath.Synchronize = true;
                myPath.Visible = true;

                //Make the path procedure as active path procedure
                station.ActiveTask.ActivePathProcedure = myPath;
                int i = 0;
                //Create Path 
                foreach (RsTarget target in station.ActiveTask.Targets)
                {
                    if (i == 0)
                    {

                        // new RsMoveInstruction(station.ActiveTask, "Move", "Default",
                        //MotionType.Joint, station.ActiveTask.ActiveWorkObject.Name, target.Name, station.ActiveTask.ActiveTool.Name);

                        RsMoveInstruction moveInstruction =
                            new RsMoveInstruction(station.ActiveTask, "Move", "Default",
                            MotionType.Joint, SelectedWorkObject.Name, target.Name, SelectedToolData.Name);
                        moveInstruction.InstructionArguments["Speed"].Value = speed;
                        moveInstruction.InstructionArguments["Zone"].Value = zone;

                        myPath.Instructions.Add(moveInstruction);
                    }
                    else
                    {

                        RsMoveInstruction moveInstruction =
                            new RsMoveInstruction(station.ActiveTask, "Move", "Default", MotionType.Linear, SelectedWorkObject.Name,
                            target.Name, SelectedToolData.Name);
                        moveInstruction.InstructionArguments["Speed"].Value = speed;
                        moveInstruction.InstructionArguments["Zone"].Value = zone;
                        myPath.Instructions.Add(moveInstruction);
                    }
                    i++;
                    pathnumber++;
                }
            }
            catch (Exception ex)
            {
                Project.UndoContext.CancelUndoStep(CancelUndoStepType.Rollback);
                Logger.AddMessage(new LogMessage(ex.Message.ToString()));
            }
            finally
            {
                Project.UndoContext.EndUndoStep();
            }
        }
    }
}






