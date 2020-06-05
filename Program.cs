using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;



namespace FileReader
{
    
    class FileReader
    {
          static void Main(string[] args)
            {
                
            Console.WriteLine("Press ESC to stop");
            int a = 0;
           // float gn = 0;
            double g=0, x=0, y=0, z=0;
           
            do
            {
                while (!Console.KeyAvailable && a < 1)
                {
                    String[] arrPositions = new String[8]; // order (0-7) --> N,G,X,Y,Z,I,J,K,F
                    
                    // Do something
                    StreamReader sr = new StreamReader(@"C:\Users\dani0007\Documents\Gcode.txt");
                    //foreach (string line in File.ReadLines(@"C:\Users\dani0007\Documents\Gcode.txt", Encoding.UTF8))
                    string Saved;
                    string input;
                    // The pattern searches for the letters [ngxyzf] followed by a + or - if they exist followed by one or more digits. then if zero or more dots followed by one or more digits, this to exclude the number from ending with a dot. 
                   
                    
                    String Pattern = @"[ngxyzf][+-]?(\d+)(.\d+)*";
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

                            Console.WriteLine("{");
                            for (int ctr = 0; ctr < substrings.Length; ctr++)
                            {

                                Console.WriteLine(substrings[ctr]);
                                

                                Regex rgx2 = new Regex(Pattern, RegexOptions.IgnoreCase);
                                MatchCollection matches2 = rgx.Matches(substrings[ctr]);

                                foreach (Match match in matches2)
                                {

                                    if (match.Value == "G0" | match.Value == "G01" | match.Value == "G1" | match.Value == "G02" | match.Value == "G2" | match.Value == "G03" | match.Value == "G3")
                                    {
                                        int StartG = match.Index + 1;
                                        int LengthG = match.Length;
                                        int EndG = (match.Index + match.Length);
                                        String ValueG = match.Value.Remove(0, 1);
                                        Double DoubleG = Double.Parse(ValueG, CultureInfo.InvariantCulture);
                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med G!");
                                        Console.Write("Start of G:" + StartG);
                                        Console.Write(" Lenght Of G:" + LengthG);
                                        Console.Write(" End of G:" + EndG);
                                        Console.WriteLine(DoubleG);
                                        arrPositions[0] = ValueG;
                                    }


                                    if (match.Value.StartsWith("X"))
                                    {
                                        int StartX = match.Index + 1;
                                        int LengthX = match.Length;
                                        int EndX = (match.Index + match.Length);
                                        String ValueX = match.Value.Remove(0, 1);
                                        Double DoubleX = Double.Parse(ValueX, CultureInfo.InvariantCulture);
                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med X!");
                                        Console.WriteLine("Start of X:" + StartX);
                                        Console.Write(" Lenght Of X:" + LengthX);
                                        Console.Write(" End of X:" + EndX);
                                        Console.Write(" Double X:"+DoubleX);
                                        arrPositions[1] = ValueX;
                                    }


                                    if (match.Value.StartsWith("Y"))
                                    {
                                        int StartY = match.Index + 1;
                                        int LengthY = match.Length;
                                        int EndY = (match.Index + match.Length);
                                        String ValueY = match.Value.Remove(0, 1);
                                        Double DoubleY = Double.Parse(ValueY, CultureInfo.InvariantCulture);
                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med Y!");
                                        Console.WriteLine("Start of Y:" + StartY);
                                        Console.Write(" Lenght Of Y:" + LengthY);
                                        Console.Write(" End of Y:" + EndY);
                                        Console.Write("Double Y: " + DoubleY);
                                        arrPositions[2] = ValueY;
                                    }


                                    if (match.Value.StartsWith("Z"))
                                    {
                                        int StartZ = match.Index + 1;
                                        int LengthZ = match.Length;
                                        int EndZ = (match.Index + match.Length);
                                        String ValueZ = match.Value.Remove(0, 1);
                                        Double DoubleZ = Double.Parse(ValueZ, CultureInfo.InvariantCulture);

                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med Z!");
                                        Console.Write("Start of Z:" + StartZ);
                                        Console.Write(" Lenght Of Z:" + LengthZ);
                                        Console.Write(" End of Z:" + EndZ);
                                        Console.Write(" Double Z:" + DoubleZ);
                                        arrPositions[3] = ValueZ;
                                    }


                                    if (match.Value.StartsWith("I"))
                                    {
                                        int StartI = match.Index + 1;
                                        int LengthI = match.Length;
                                        int EndI = (match.Index + match.Length);
                                        String ValueI = match.Value.Remove(0, 1);
                                        Double DoubleI = Double.Parse(ValueI, CultureInfo.InvariantCulture);

                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med I!");
                                        Console.Write("Start of I:" + StartI);
                                        Console.Write(" Lenght Of I:" + LengthI);
                                        Console.Write(" End of I:" + EndI);
                                        Console.Write("Double: "+ DoubleI);
                                        arrPositions[4] = ValueI;
                                    }


                                    if (match.Value.StartsWith("J"))
                                    {
                                        int StartJ = match.Index + 1;
                                        int LengthJ = match.Length;
                                        int EndJ = (match.Index + match.Length);
                                        String ValueJ = match.Value.Remove(0, 1);
                                        Double DoubleJ = Double.Parse(ValueJ, CultureInfo.InvariantCulture);

                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med J!");
                                        Console.Write("Start of J:" + StartJ);
                                        Console.Write(" Lenght Of J:" + LengthJ);
                                        Console.Write(" End of J:" + EndJ);
                                        Console.Write("Double: "+DoubleJ);
                                        arrPositions[5] = ValueJ;
                                    }


                                    if (match.Value.StartsWith("K"))
                                    {
                                        int StartK = match.Index + 1;
                                        int LengthK = match.Length;
                                        int EndK = (match.Index + match.Length);
                                        String ValueK = match.Value.Remove(0, 1);
                                        Double DoubleK = Double.Parse(ValueK, CultureInfo.InvariantCulture);

                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med K!");
                                        Console.Write("Start of K:" + StartK);
                                        Console.Write(" Lenght Of K:" + LengthK);
                                        Console.Write(" End of K:" + EndK);
                                        Console.Write("Double: "+DoubleK);
                                        arrPositions[6] = ValueK;
                                    }

                                    if (match.Value.StartsWith("F"))
                                    {
                                        int StartF = match.Index + 1;
                                        int LengthF = match.Length;
                                        int EndF = (match.Index + match.Length);
                                        String ValueF = match.Value.Remove(0, 1);
                                        Double DoubleF = Double.Parse(ValueF, CultureInfo.InvariantCulture);

                                        Console.WriteLine("");
                                        Console.WriteLine("Börjar med F!");
                                        Console.Write("Start of F:" + StartF);
                                        Console.Write(" Lenght Of F:" + LengthF);
                                        Console.Write(" End of F:" + EndF);
                                        Console.Write("Double: "+DoubleF);
                                        arrPositions[7] = ValueF;
                                    }

                                    if (ctr < substrings.Length - 1)

                                        Console.Write(", ");
                                        }
                                   
                                Console.WriteLine("");
                                Console.WriteLine("}");
                                Console.WriteLine("");
                            }
                        }
                            
                                //Console.WriteLine("{0}", linebuf);
                                /*foreach (Match n in matches)
                                {
                                    //Console.WriteLine("Test=" + n.Value);
                                    if (n.Value.StartsWith("G"))
                                        //g = Convert.ToInt32(n.Value.Remove(0, 1));
                                        Console.WriteLine("Command G=" + n.Value);

                                    if (n.Value.StartsWith("X"))
                                        // x = Convert.ToInt32(n.Value.Remove(0,1));
                                        Console.WriteLine("Command X= " + n.Value);

                                    if (n.Value.StartsWith("Y"))
                                        // y = Convert.ToDouble(line.Remove(3,100));
                                        Console.WriteLine("Command Y= " + n.Value);

                                    if (n.Value.StartsWith("Z"))
                                        //z = Convert.ToInt32(n.Value.Remove(0, 1));
                                        Console.WriteLine("Command Z= " + n.Value);
            

                                }
                            }

                        }

                        */    
                    }
                        a++;
                        sr.Close();    
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                }

              
             
          

        class Program
        {      
            }
        }
    }

