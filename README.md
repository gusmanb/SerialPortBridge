# SerialPortBridge

Small set of tools to connect serial ports from a Raspberry to a Windows machine. I created this program because I wanted to use my raspberry as 3D printer / CNC server and no software worked as I wanted.

 ### Compilation:

1. Open solution in VS, compile the SerialClient (Windows app).
2. Open a command line console and execute the `publish.bat` file under SerialLinuxHost, it will create the publication of the server for the Raspberry.
3. Copy `BaudSetter.c` wherever you have installed the server publication
4. Compile with `gcc -o BaudSetter BaudSetter.c`

Ready!

### Windows requirements

You must install [com0com](https://sourceforge.net/projects/com0com/).
If you did a full installation the `Com0com path` setting can be left empty and the client will locate it

### Run as daemon in Raspberry/Raspbian

1. Copy the `install_daemon.sh` script to the folder where you have installed the serial host
2. Change it's permissions to executable: `sudo chmod 777 install_daemon.sh`
3. Execute: `sudo install_daemon.sh`
4. Reboot
