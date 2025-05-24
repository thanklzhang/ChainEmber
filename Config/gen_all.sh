#!/bin/bash
cd "$(dirname "$0")"  


# 检查 python 命令是否指向 Python 3  
if python --version 2>&1 | grep -q "Python 3"; then  
    # 如果是 Python 3，使用 python 命令  
    PYTHON_CMD="python"  
else  
    # 否则，尝试使用 python3 命令  
    if command -v python3 >/dev/null 2>&1; then  
        PYTHON_CMD="python3"  
    else  
        # 如果既没有 Python 3 的 python 命令，也没有 python3 命令，则输出错误消息并退出  
        echo "Error: Python 3 is not installed and python3 command is not found."  
        exit 1  
    fi  
fi  
  
# 运行 Python 脚本  
$PYTHON_CMD gen_all.py

