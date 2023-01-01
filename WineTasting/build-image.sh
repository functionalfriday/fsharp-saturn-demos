#!/bin/sh
docker build --pull --build-arg=Dev -f Dockerfile -t winetasting:latest .
