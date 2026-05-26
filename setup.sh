#!/bin/bash
docker swarm init
sleep 10
docker stack deploy -c docker-stack-kong.yml kong
docker stack deploy -c docker-stack-portainer.yml portainer
