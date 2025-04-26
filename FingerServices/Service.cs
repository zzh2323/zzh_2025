using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Employee;

namespace FingerServices
{
    public class Service
    {
        private readonly Dictionary<PersonnelLevel, string> levelKeys;
        private PersonnelLevel matchedLevel;
        private bool IsRegister;
        private int RegisterCount;
        private int cbRegTmp;

        public Service(Dictionary<PersonnelLevel, string> levelKeys)
        {
            this.levelKeys = levelKeys;
            this.matchedLevel = PersonnelLevel.Level_0;
            this.IsRegister = false;
            this.RegisterCount = 0;
            this.cbRegTmp = 0;
        }

        public string Enroll(string name, string levelPwd, out bool isRegisterStarted)
        {
            // 检查姓名是否为空
            if (string.IsNullOrWhiteSpace(name))
            {
                isRegisterStarted = false;
                return "姓名不能为空!";
            }

            // 检查密钥是否为空
            if (string.IsNullOrWhiteSpace(levelPwd))
            {
                isRegisterStarted = false;
                return "密钥不能为空!";
            }

            // 检查姓名长度
            if (name.Length < 1 || name.Length > 10)
            {
                isRegisterStarted = false;
                return "姓名长度必须在1到10个字符之间!";
            }

            // 验证密钥是否匹配
            bool isKeyValid = false;
            foreach (var kvp in levelKeys)
            {
                if (kvp.Value == levelPwd)
                {
                    isKeyValid = true;
                    matchedLevel = kvp.Key;
                    break;
                }
            }

            if (!isKeyValid)
            {
                matchedLevel = PersonnelLevel.Level_0;
                isRegisterStarted = false;
                return "请注意！密钥不正确!注册等级为0!";
            }

            // 开始注册流程
            if (!IsRegister)
            {
                IsRegister = true;
                RegisterCount = 0;
                cbRegTmp = 0;
                isRegisterStarted = true;
                return "请输入指纹三次!";
            }

            isRegisterStarted = false;
            return string.Empty;
        }
    }
}
