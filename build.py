#!/usr/bin/env python3
import sys
import os
import shutil

def ensureDir(relativePath):
    dir_path = os.path.join(os.getcwd(), relativePath)
    if os.path.exists(dir_path) is False:
        os.mkdir(dir_path)

def rmDir(relativePath):
    dir_path = os.path.join(os.getcwd(), relativePath)
    if os.path.exists(dir_path) is False:
        return
        shutil.rmtree(dir_path)

def main():
    module = 'MediaHuis.Notifications.Api'
    service = sys.argv[1]
    module = sys.argv[2]
    version = sys.argv[3]
    ensureDir(f'{service}/.dist')
    rmDir(f'{service}/.dist/{module}')
    os.system(f'dotnet build {service}/{module} -p:Version="{version}" -c Release -o {service}/.dist/{module}')

main()
