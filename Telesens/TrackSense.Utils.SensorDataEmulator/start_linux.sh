#!/bin/bash
echo "Installing dependencies...";
pip install -r requirements.txt;
echo "Starting Hypercorn server...";
hypercorn main:app --bind 0.0.0.0:8000;
