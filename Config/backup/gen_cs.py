# -*- coding: utf-8 -*-
import gen
import os
import traceback

def main():
    #find all file
    config_names = []
    list_dirs = os.walk('data_table')
    #gen.gen_enum('data/Enum.xlsx','../client/Assets/Scripts/Config')
    client_project_path = "../JekoClient/Assets"
    out_dictionary = client_project_path + '\Script\Data\TableData'
    for root, dirs, files in list_dirs:
        # print(root)
        # for d in dirs:
        #     print(os.path.join(root, d))
        for f in files:
            # print(os.path.splitext(f)[1])
            splitStr = os.path.splitext(f)
            if splitStr[1] == '.xlsx' and not '~$' in splitStr[0]:
                #generate json
                try:
                    gen.gen(os.path.join(root, f),out_dictionary,'cs',None,None)
                    config_names.append(os.path.splitext(f)[0])
                except Exception as e:
                    print('error file:'+os.path.join(root, f))
                    msg = traceback.format_exc()
                    print(msg)

    gen.gen_auto_config_loader_cs_code(out_dictionary,config_names)
     
if __name__ == "__main__":
    main()
