# SerialPortBridge

Small set of tools to connect serial ports from a Raspberry to a Windows machine. I created this program because I wanted to use my raspberry as 3D printer / CNC server and no software worked as I wanted.

![Screenshot](https://raw.githubusercontent.com/gusmanb/SerialPortBridge/master/SerialManager/Images/Screenshot.png)

 ### Compilation:

1. Open solution in VS, compile the SerialClient (Windows app).
2. Open a command line console and execute the `publish.bat` file under SerialLinuxHost, it will create the publication of the server for the Raspberry.
3. Copy `BaudSetter.c` wherever you have installed the server publication
4. Compile with `gcc -o BaudSetter BaudSetter.c`
5. Ready!

### Windows requirements

You must install [com0com](https://sourceforge.net/projects/com0com/).
If you did a full installation the `Com0com path` setting can be left empty and the client will locate it

### Run as daemon in Raspberry/Raspbian

1. Copy the `install_daemon.sh` script to the folder where you have installed the serial host
2. Change it's permissions to executable: `sudo chmod 777 install_daemon.sh`
3. Execute: `sudo install_daemon.sh`
4. Reboot

### Usage

1. Enter at least the server address (and port 9025 if you haven't changed it in the server code) and save the configuration
2. Create as many pairs as ports you want to connect, the input port will be used by your target app, the output port will be used by the serial manager. No need to specify baud rate.
3. Create as many bridges as ports you want to connect selecting the output you want to connect to a remote port. You must specify the speed of the remote port.
4. Ready!

An usage example

- Connected a CNC to the Raspberry, it creates the ttyUSB0 port.
- Configured server as 192.168.1.10 9025
- Created pair COM2<->COM3
- Created bridge ttyUSB0<->COM3 with 115200 bauds
- On GrblControl CNC is configured to use COM2 at 115200 bauds

Now GrblControl will control the remote CNC.
