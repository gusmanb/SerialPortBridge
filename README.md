# SerialPortBridge

Small set of tools to connect serial ports from a Raspberry to a Windows machine. I created this program because I wanted to use my raspberry as 3D printer / CNC server and no software worked as I wanted.

 ### Compilation:

1- Open solution in VS, compile the SerialClient.
2- Open a command line console and execute the `publish.bat` file under SerialLinuxHost, it will create the publication of the server for the Raspberry.
3- Copy `BaudSetter.c` wherever you have installed the server publication
4- Compile with `gcc -o BaudSetter BaudSetter.c`

Ready!

### Run as daemon

Create a file called `daemon` where you have installed the server
Add this script:

```
#!/bin/bash

cd (path_where_server_is_installed)
sudo ./SerialLinuxHost daemon &
```

Change permissions as executable: `chmod 777 daemon`

Edit rc.local and add `(path_where_server_is_installed)/daemon` before `exit 0`

Reboot
