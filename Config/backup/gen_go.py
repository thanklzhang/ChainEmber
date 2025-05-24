# -*- coding: utf-8 -*-
import gen
import os
import traceback

def main():
    #find all file
    list_dirs = os.walk('data_table')
    #gen.gen_enum('data/Enum.xlsx','../client/Assets/Scripts/Config')
    for root, dirs, files in list_dirs:
        # print(root)
        # for d in dirs:
        #     print(os.path.join(root, d))
        for f in files:
            # print(os.path.splitext(f)[1])
            if os.path.splitext(f)[1] == '.xlsx':
                #generate json
                    try:
                        gen.gen(os.path.join(root, f),'..\JekoServer\config_data\model','go')
                    except Exception as e:
                        print('error file:'+os.path.join(root, f))
                        msg = traceback.format_exc()
                        print(msg)
                

    
if __name__ == "__main__":
    main()
