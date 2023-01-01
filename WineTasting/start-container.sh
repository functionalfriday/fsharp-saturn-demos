#!/bin/sh
imageName=winetasting
containerName=wineTasting

#docker ps -q --filter "name=$containerName" | grep -q . && docker stop $containerName
#docker run -d --rm -it --name $containerName -p 5000:80 $imageName

#docker run --rm -it --entrypoint /bin/bash --name $containerName -p 5000:80 $imageName
docker run --rm -it --name $containerName -p 5000:80 $imageName
