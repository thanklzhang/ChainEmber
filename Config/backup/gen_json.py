# -*- coding: utf-8 -*-
import gen
import os


def main():
    #find all file
    list_dirs = os.walk('data_table')
    res_file_name = "ResourcePath"  #资源管理表格文件名称
    client_project_path = "../JekoClient/Assets"
    res_out_path = client_project_path + '\Script\Data\TableData' #导出资源枚举 cs 文件 地址
    for root, dirs, files in list_dirs:
        # print(root)
        # for d in dirs:
        #     print(os.path.join(root, d))
        for f in files:
            # print(os.path.join(root, f))
            splitStr = os.path.splitext(f)
            if splitStr[1] == '.xlsx' and not '~$' in splitStr[0]:
                #generate json
                gen.gen(os.path.join(root, f),client_project_path + '/Resources/Config/ConfigData','json',res_file_name,res_out_path)
                    
if __name__ == "__main__":
    main()
