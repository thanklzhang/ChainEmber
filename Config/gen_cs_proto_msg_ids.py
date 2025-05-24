# -*- coding: utf-8 -*-
#!usr/bin/python
# coding=gbk

import os
import string
import traceback
import openpyxl
import sys
import time
import re
import shutil

definestr = ""

baseDir = os.getcwd()

EnumName = "ProtoIDs"


def get_proto_file_list(dir, flist):
    print("\n")

    print("start get proto files ...")
    print("\n")
    for root, dirs, files in os.walk(dir, True):
        for name in files:
            nm = os.path.splitext(name)
            if (nm[1] == ".proto"):
                flist.append(name)
                print("find file : " + name)
                # tmpCmd=('(copy %s %s /y)' %(os.path.join(root, name),baseDir))
                # print(tmpCmd)
                # os.system(tmpCmd)


def WriteToFile(f, str):
    file_part = open(f, 'wb')
    file_part.write(str.encode('UTF-8'))
    file_part.close()

# 找到 proto 所有的消息 合并成一个自定义消息枚举


def GenMsgFile(dir, inputFiles, outFile):
    print("\n")
    optionStr = ""
    for currFile in inputFiles:
        fc = open(dir + "\\" + currFile, 'rb')
        print("currFile : " + currFile + "----------------------------------")
        line = fc.readline()
        content = []
        while line:
            content.append(line)
            line = fc.readline()
        optionStr += "\t//" + currFile.split('.')[0] + "\n"
        lineNum = 0
        start = -1
        isFinish = False

        msdIdIndex = 0
        rightIndex = -1
        for c in content:
            realContent = c
            if(not isFinish):
                currContent = realContent.decode('utf-8')
                # 找到 枚举名称
                msdIdIndex = currContent.find(EnumName)
                if(msdIdIndex > 0):
                    start = lineNum

                if(start >= 0):
                    rightIndex = currContent.find("}")
                    if(rightIndex >= 0):
                        isFinish = True
                    else:
                        currStrs = currContent.split('=')
                        if(len(currStrs) > 1):
                            # 消息
                            leftValue = currStrs[0].strip()
                            # 过滤无用枚举值
                            if(leftValue != "First" and leftValue != "Begin" and leftValue != "End"):
                                rightValue = currStrs[1].strip()
                                # print(leftValue + " = " + rightValue)
                                rightValue = rightValue.replace(";", ",")
                                optionStr += "\t" + leftValue + " = " + rightValue + "\n"
                        else:
                            # 检查注释内容
                            describeStr = currContent.split('//')
                            if(len(describeStr) > 1):
                                optionStr += currContent
                                # print(currContent)

                lineNum = lineNum + 1
        # print("\n")
        optionStr += "\n"
        fc.close()

    allStr = "//gen by tool\npublic enum " + \
        EnumName + "\n{\n" + optionStr + "}\n"

    WriteToFile(outFile, allStr)
    return

def Copy(source_path,target_path):
    print('start to copy files ... : ' + source_path + " -> " + target_path)
    if not os.path.exists(target_path):
        os.makedirs(target_path)
    if os.path.exists(source_path):
        # root 所指的是当前正在遍历的这个文件夹的本身的地址
        # dirs 是一个 list，内容是该文件夹中所有的目录的名字(不包括子目录)
        # files 同样是 list, 内容是该文件夹中所有的文件(不包括子目录)
        for root, dirs, files in os.walk(source_path):
            for file in files:
                src_file = os.path.join(root, file)
                shutil.copy(src_file, target_path)
                print(src_file)
    print('copy files finished!')
    
def main():
    try:
        
        inputPath = "proto"
        outFile = "" + EnumName + ".cs"

        # # 获取目录下所有 proto 文件
        # fileProtoList = []
        # get_proto_file_list(baseDir + "\\" + inputPath, fileProtoList)
        
        #proto cmd enum 转换成 cs ， 这样业务不依靠 protoBuf 而是依靠生成的自定义格式的 enum
        fileProtoList = []
        fileProtoList.append("Cmd" + ".proto")

        # 根据 proto 文件生成消息枚举文件
        GenMsgFile(inputPath, fileProtoList, outFile)

        protoIdsPath = "..\\Client\\Assets\\Script\\Common"
        shutil.copy("ProtoIDs.cs", protoIdsPath)
 
        os.system("pause")
    except:
        print("!!!!!!catch exception!!!!!!")
        traceback.print_exc()
        os.system("pause")

    # os.system("pause")
main()
# print("------------------")
