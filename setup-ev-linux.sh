#!/bin/bash

# set environment variable
## Postgres db
export MyWeb_PostgresDbSettings__Username=admin
export MyWeb_PostgresDbSettings__Password=admin
## Email
export MyWeb_EmailSettings__Password=Test@1234

# start docker-compose
docker-compose up -d


