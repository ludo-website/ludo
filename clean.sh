#!/bin/bash
docker stack rm kong
docker stack rm portainer
docker swarm leave --force
docker network prune -f
# docker image prune -a