#!/bin/bash

docker build -t ludo-api -f Ludo.Api.Dockerfile .
docker tag ludo-api silviaalex/ludo-api
docker push silviaalex/ludo-api

docker build -t ludo-auth -f Ludo.Auth.Dockerfile .
docker tag ludo-auth silviaalex/ludo-auth
docker push silviaalex/ludo-auth

docker build -t ludo-io -f Ludo.Io.Dockerfile .
docker tag ludo-io silviaalex/ludo-io
docker push silviaalex/ludo-io
