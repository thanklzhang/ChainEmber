# -*- coding: utf-8 -*-
#!/usr/bin/python

import gen_logic
import sys

import os
import shutil

################ 配置
# table 文件输入目录
config_input_dir = 'data_config'

# 客户端 cs define 文件输出目录
client_cs_out_dir = "../Assets/Script/Config/ConfigDefine"
# 客户端 json 文件输出目录
# client_json_out_dir = "../001_GameFramework_Client/Assets/BuildRes/TableData"

# 服务端 cs define 文件输出目录
#server_cs_out_dir = "../001_GameFramework_Server/GameServer/Common/Config/Table/Define"
# 服务端 json 文件输出目录
# server_json_out_dir = "../001_GameFramework_Server/GameServer/netcoreapp3.1/Resource/Table"
#服务器端有可能在 IDE 中调试 所以在每个工程下都复制下所有 json
# battle_self_table_path = "../001_GameFramework_Server/GameServer/BattleServer/bin/Debug/netcoreapp3.1/Resource/Table"

#纯战斗逻辑 config cs define 目录
#pure_battle_logic_table_path = "../001_GameFramework_Battle/BattleProject/Common/Table"
#纯战斗逻辑 config json 目录
# pure_battle_logic_table_json_path = "../001_GameFramework_Battle/BattleProject/bin/Debug/netcoreapp3.1/Resource/Table"

# 战斗配置接口定义 cs define 文件输出目录
battle_config_define_cs_out_dir = "../Client/Assets/Script/Battle/LocalBattleLogic/BattleLogic/BattleCore/Config/ConfigDefine"

# 战斗配置接口定义(客户端实现) cs define 文件输出目录
battle_config_client_impl_define_cs_out_dir = "../Client/Assets/Script/Battle/LocalBattleLogic/Executer/Impl/ConfigImpl/ConfigDefineImpl"

# config 数据 前端用
common_json_out_dir = "../Client/Assets/BuildRes/Config"

#资源 id 生成目录
res_id_out_dir = "../Client/Assets/Scripts/Common/Define"

#################

def main(argv):

    #client
    args = {}
    args['op_type'] = gen_logic.OpType.cs
    args['in_path'] = config_input_dir
    args['out_path'] = client_cs_out_dir
    args['battle_config_cs_path'] = battle_config_define_cs_out_dir
    args['battle_config_impl_cs_path'] = battle_config_client_impl_define_cs_out_dir
    
    gen_logic.gen(args)
    
    #server
    #args = {}
    #args['op_type'] = gen_logic.OpType.cs
    #args['in_path'] = config_input_dir
    #args['out_path'] = server_cs_out_dir
    #gen_logic.gen(args)
    
    #copy
    #Copy(server_cs_out_dir,pure_battle_logic_table_path)
    
    #json
    args = {}
    args['op_type'] = gen_logic.OpType.json
    args['in_path'] = config_input_dir
    args['out_path'] = common_json_out_dir
    args['res_out_path'] = res_id_out_dir
    gen_logic.gen(args)


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

##################
if __name__ == "__main__":
    main(sys.argv)
