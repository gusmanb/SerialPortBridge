#!/bin/bash

chmod 777 SerialLinuxHost
echo -e "#!/bin/bash\n\ncd $PWD\nsudo ./SerialLinuxHost daemon &" > daemon
chmod 777 daemon
sed  "/exit 0/ { N; s/exit 0\n/$PWD/SerialLinuxHost\n&/ }" /etc/rc.local
