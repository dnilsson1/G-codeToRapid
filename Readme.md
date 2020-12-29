
## G-code to Rapid translator for RobotStudio

~~This is the original code from my master thesis in 2016.~~  
#### This code has been now been updated and has been verified to run on RobotStudio 2020.4




[Link to the paper][df1]

### Todos

 - Needs to have an updated controller.connect() method.
 - The program needs to implement threading in order to run much more efficiently. Small G-code files is generated ok. 
   But files with loads of instructions like 3D-Prints causes the application to freeze for quite some time (several minutes).
 - There is generally plenty of room for optimization in the code. The code was mostly done just to proof that it worked.
 - No functionality to handle Splindle/extruder instructions. It basically filters out only move instructions as it is now.


#### Prerequisits
- Visual Studio
- Download and install RobotStudio SDK from https://developercenter.robotstudio.com/ for the current RobotStudio version.


### Setup
- Add the references to the assemblies as stated in the documentation: https://developercenter.robotstudio.com/api/robotstudio/articles/Introduction/Installation.html#assemblies
- Edit the paths hightlighted below in the rsaddin-file:
  
   - **xsi:schemaLocation="urn:abb-robotics-robotstudio-addin file:///C:\Program%20Files%20(x86)\ABB\SDK\RobotStudio%202020%20SDK\RobotStudioAddin.xsd"**
   - **Path>C:\Users\sedanil1\source\repos\G-codeToRapid3\bin\Debug</Path**
   - **Path>C:\Users\sedanil1\source\repos\G-codeToRapid3\bin\Release</Path**

----

[df1]: <http://www.diva-portal.org/smash/record.jsf?pid=diva2%3A1034182&dswid=4335>
