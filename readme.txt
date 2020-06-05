This sample is created for RobotStudio SDK Support. Usually to give an example to a specific support question.
To get the latest samples etc go to the RobotStudio User Forum - Developer Center section
http://forums.robotstudio.com/categories/developer-center
or browse your way from http://www.robotstudio.com/
or browse your way from http://new.abb.com/products/robotics/robotstudio

In this project we have set the ABB Robotics - References , to [Copy Local = TRUE] so the SDK dll files are distributed in the build folders.
That way if you load this sample but have a later version of the SDK you can still run it as we intended - this is a recommended distribution model as well.

XCopy
If you get the error
	The command "XCopy /y "<path>" "<path>"" exited with code 4.
In this sample we have a Post-Build event that copies the build into the common files - Addin folder of RobotStudio. Now since that folder is in the programs section, the post-build-script needs Administrator access. 
You should check the paths to see that they comply with your installation. If you do not want to run with Administrator privileges then remove the post-build-script completely, you can copy paste the files as usual.
1	Remove/Edit the XCopy script
	Right click the project "SupportSampleRobotStudioAddin" and select [Properties]
	[Build Events] > [Edit Post Build]
2	Start Visual Studio as Administrator
	In the Start menu, browse to Visual Studio then hold down the [Shift] key and right click the Visual Studio shortcut and select [Run as administrator]
	http://msdn.microsoft.com/en-us/library/jj662724.aspx
