#!/bin/bash

dc=${1}

if [ ! -z ${dc} ] && [ -f ${dc} ]; then
  echo "Saving docker images from file ${dc}..."
  images=`grep image: ${dc} | awk '{print $2}'`
  docker save ${images} | gzip > docker-images.gz
  echo "Success!"
else
  echo "ERROR! You must set path to docker-compose.yml as argument!"
fi