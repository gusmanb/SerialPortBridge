#!/bin/bash

chmod 777 SerialLinuxHost
echo -e "#!/bin/bash\n\ncd $PWD\nsudo ./SerialLinuxHost daemon &" > daemon
chmod 777 daemon
sed  -i "/^exit 0/ i $PWD\/daemon\n" /etc/rc.local 
