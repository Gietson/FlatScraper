#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Publishing application
echo ./scripts/publish.sh
echo Building and pushing Docker images
echo ./scripts/docker-publish.sh