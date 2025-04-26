using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IFingerPrint;
using libzkfpcsharp;
using Sample;
using System.Security.Cryptography;
using Dal;
using System.Data.SqlClient;
using Employee;
using FingerServices;

namespace FingerTest
{
    public partial class Form1 : Form
    {
        private readonly IFingerLib fingerLib;
        private readonly IFingerDevice fingerDevice;
        private readonly IFingerDB fingerDB;
        private readonly IFingerManage fingerManage;
        private readonly IFingerUtility fingerUtility;

        private readonly Service service;

        //用户信息
        Person person = null;
        //所有用户的指纹信息
        private List<Person> personList = new List<Person>();
        //当前用户
        Person currentPerson = null;

        //键值对存储各个等级密钥
        private Dictionary<PersonnelLevel, string> levelKeys = 
            new Dictionary<PersonnelLevel, string>();
        //密钥默认匹配等级
        PersonnelLevel matchedLevel = PersonnelLevel.Level_0;

        //设备句柄
        IntPtr mDevHandle = IntPtr.Zero;
        //数据库句柄
        IntPtr mDBHandle = IntPtr.Zero;
        //窗口句柄
        IntPtr FormHandle = IntPtr.Zero;

        //CTS
        private CancellationTokenSource cts;

        // 注册状态信号量
        private SemaphoreSlim registrationSemaphore = new SemaphoreSlim(1, 1);
        // 是否已注册信号量
        private SemaphoreSlim registeredSemaphore = new SemaphoreSlim(0, 1);
        // 指纹存在信号量
        private SemaphoreSlim fpExistSemaphore = new SemaphoreSlim(0, 1);

        bool bIdentify = true;

        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 3;

        //保存三个模板
        byte[][] RegTmps = new byte[3][];
        //保存三合一最终输出模板
        byte[] RegTmp = new byte[2048];
        //保存线程捕获的指纹模板
        byte[] CapTmp = new byte[2048];
        //保存线程捕获的指纹模板大小
        int cbCapTmp = 2048;
        //最终输出模板大小
        int cbRegTmp = 0;
        //数据库有ID，初始ID被占用
        int iFid = 1;

        //指纹注册状态
        bool bRegistering = false;

        //Task替换线程
        private Task captureTask = null;
        Thread captureThread = null;

        //图像尺寸
        private int mfpWidth = 0;
        private int mfpHeight = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public Form1(
            IFingerLib fingerLib,
            IFingerDevice fingerDevice,
            IFingerDB fingerDB,
            IFingerManage fingerManage,
            IFingerUtility fingerUtility)
        {
            InitializeComponent();

            this.fingerLib = fingerLib;
            this.fingerDevice = fingerDevice;
            this.fingerDB = fingerDB;
            this.fingerManage = fingerManage;
            this.fingerUtility = fingerUtility;

            service = new Service(levelKeys);
        }

        private void bnVerify_Click(object sender, EventArgs e)
        {
            if (registeredSemaphore.CurrentCount == 1)
            {
                MessageBox.Show("本次未注册，无法验证!", "提示");
                return;
            }
            if (bIdentify)
            {
                bIdentify = false;
                textRes.Text = "请输入指纹!";
            }
        }

        private void InitFinger()
        {
            cmbIdx.Items.Clear();
            int ret = zkfperrdef.ZKFP_ERR_OK;

            if ((ret = fingerLib.Initialize()) == zkfperrdef.ZKFP_ERR_OK)
            {
                int nCount = fingerDevice.GetDeviceCount();
                if (nCount > 0)
                {
                    for (int i = 0; i < nCount; i++)
                    {
                        cmbIdx.Items.Add(i.ToString());
                    }
                    cmbIdx.SelectedIndex = 0;

                    textRes.Text = "初始化成功!";
                }
                else
                {
                    fingerLib.Terminate();
                    MessageBox.Show("无设备连接!");
                }
            }
            else
            {
                MessageBox.Show("初始化失败, 错误码=" + ret + " !");
            }
        }



        private void OpenDevice()
        {
            int ret = zkfp.ZKFP_ERR_OK;
            if (IntPtr.Zero == (mDevHandle = fingerDevice.OpenDevice(cmbIdx.SelectedIndex)))
            {
                MessageBox.Show("连接设备失败!");
                return;
            }
            if (IntPtr.Zero == (mDBHandle = fingerDB.DBInit()))
            {
                MessageBox.Show("初始化算法库失败!");
                fingerDevice.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
                return;
            }
            bnEnroll.Enabled = true;
            bnVerify.Enabled = true;
            bnIdentify.Enabled = true;
            RegisterCount = 0;
            cbRegTmp = 0;
            iFid = 1;
            for (int i = 0; i < 3; i++)
            {
                RegTmps[i] = new byte[2048];
            }
            byte[] paramValue = new byte[4];
            int size = 4;
            fingerUtility.GetParameters(mDevHandle, 1, paramValue, ref size);
            fingerUtility.ByteArray2Int(paramValue, ref mfpWidth);

            size = 4;
            fingerUtility.GetParameters(mDevHandle, 2, paramValue, ref size);
            fingerUtility.ByteArray2Int(paramValue, ref mfpHeight);

            FPBuffer = new byte[mfpWidth * mfpHeight];

            //初始化CTS
            cts = new CancellationTokenSource();
            captureTask = Task.Run(() => DoCapture(cts.Token));
            
            textRes.Text = "连接设备成功!";
        }

        private async Task DoCapture(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                cbCapTmp = 2048;
                int ret = fingerManage.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                }
                await Task.Delay(200);
            }
        }

        private void CloseAndFree()
        {
            CloseDevice();
            RegisterCount = 0;

            fingerLib.Terminate();
            if (IntPtr.Zero != mDBHandle)
            {
                fingerDB.DBFree(mDBHandle);
                mDBHandle = IntPtr.Zero;
            }

            cbRegTmp = 0;
            // 使用Invoke方法更新UI控件
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    bnEnroll.Enabled = false;
                    bnVerify.Enabled = false;
                    bnIdentify.Enabled = false;
                    textRes.Text = "设备已断开!";
                    picFPImg.Image = null;
                    cmbIdx.SelectedIndex = -1;
                    cmbIdx.Items.Clear();
                }));
            }
            else
            {
                bnEnroll.Enabled = false;
                bnVerify.Enabled = false;
                bnIdentify.Enabled = false;
                textRes.Text = "设备已断开!";
                picFPImg.Image = null;
                cmbIdx.SelectedIndex = -1;
                cmbIdx.Items.Clear();
            }

        }

        private async void CloseDevice()
        {
            if (cts != null)
            {
                cts.Cancel();
                if (captureTask!= null)
                {
                    await captureTask;
                }
                cts.Dispose();
            }

            if (IntPtr.Zero != mDevHandle)
            {
                fingerDevice.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
            }
        }

        private void bnIdentify_Click(object sender, EventArgs e)
        {
            textRes.Text = "请输入指纹!";

            if (!bIdentify)
            {
                bIdentify = true;
            }
        }

        private void bnEnroll_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string levelPwd = txtLevelPwd.Text;

            bool isRegisterStarted;
            string result = service.Enroll(name, levelPwd, out isRegisterStarted);

            if (!string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result, "提示");
            }

            if (isRegisterStarted)
            {
                textRes.Text = "请输入指纹三次!";

                //进入注册状态
                registrationSemaphore.Wait();
            }
        }
        

        //消息捕获
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        MemoryStream ms = new MemoryStream();
                        BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                        Bitmap bmp = new Bitmap(ms);
                        this.picFPImg.Image = bmp;
                        if (registeredSemaphore.CurrentCount == 0)
                        {
                            int ret = zkfp.ZKFP_ERR_OK;
                            int fid = 0, score = 0;
                            ret = fingerDB.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                                textRes.Text = "该指纹已注册, 指纹ID= " + fid + "!";
                                return;
                            }
                            if (RegisterCount > 0 && fingerDB.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                textRes.Text = "请输入同一手指的指纹三次!";
                                return;
                            }
                            Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                            String strBase64 = fingerUtility.BlobToBase64(CapTmp, cbCapTmp);
                            byte[] blob = fingerUtility.Base64ToBlob(strBase64);
                            RegisterCount++;
                            if (RegisterCount >= REGISTER_FINGER_COUNT)
                            {
                                RegisterCount = 0;
                                if (zkfp.ZKFP_ERR_OK == (ret = fingerDB.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)))
                                {
                                    textRes.Text = "指纹扫描完成!";
                                    if (zkfp.ZKFP_ERR_OK == (ret = fingerDB.DBAdd(mDBHandle, iFid, RegTmp)))
                                    {
                                        textRes.Text = "指纹添加成功!";
                                        if (SaveFingerInfo(RegTmp, cbRegTmp, iFid))
                                        {
                                            iFid++;
                                            textRes.Text = "注册成功!";
                                            registeredSemaphore.Release();
                                            registrationSemaphore.Release();
                                            txtName.Text = "";
                                            txtLevelPwd.Text = "";
                                            fingerDB.DBClear(mDBHandle);
                                            GetFingerInfo();
                                        }
                                        else
                                        {
                                            textRes.Text = "注册失败!";
                                            registrationSemaphore.Release();
                                        }
                                    }
                                    else
                                    {
                                        textRes.Text = "指纹添加失败, 错误码= " + ret;
                                        registrationSemaphore.Release();
                                    }
                                }
                                else
                                {
                                    textRes.Text = "模板扫描失败, 错误码= " + ret;
                                    registrationSemaphore.Release();
                                }
                                
                                return;
                            }
                            else
                            {
                                textRes.Text = "您仍需要输入 " + (REGISTER_FINGER_COUNT - RegisterCount) + " 次指纹!";
                            }
                        }
                        else
                        {
                            if (fpExistSemaphore.CurrentCount == 1)
                            {
                                textRes.Text = "请先注册指纹!";
                                return;
                            }
                            if (bIdentify)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = fingerDB.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                
                                if (zkfp.ZKFP_ERR_OK == ret)
                                {
                                    currentPerson = GetPersonByFid(fid);
                                    textRes.Text = "识别成功, 指纹ID= " + fid + ", 姓名= " + currentPerson.Name + ", 等级= " + currentPerson.Level + " ! 对比分数=" + score + "!";
                                    return;
                                }
                                else
                                {
                                    textRes.Text = "识别失败, 错误码= " + ret;
                                    return;
                                } 
                            }
                            else if (registeredSemaphore.CurrentCount == 1)
                            {
                                int ret = fingerDB.DBMatch(mDBHandle, CapTmp, RegTmp);
                                if (0 < ret)
                                {
                                    textRes.Text = "匹配指纹成功, 对比分数=" + ret + "!";
                                    return;
                                }
                                else
                                {
                                    textRes.Text = "匹配失败，错误码= " + ret;
                                    return;
                                }
                            }
                            else
                            {
                                textRes.Text = "仅可在注册后进行验证!";
                                return;
                            }
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }
        /// <summary>
        /// 加密并保存指纹信息到数据库
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="length"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool SaveFingerInfo(byte[] byteData, int length, int fid)
        {
            //加密指纹信息
            String fingerPrint = fingerUtility.BlobToBase64(byteData, length);
            AesEncryption aes = new AesEncryption();
            var (keyValue, ivValue) = aes.GenerateKeyAndIV();
            //encryptionKey = key;
            //encryptionIV = iv;
            string encryptedFingerPrint = aes.UseAes(fingerPrint, true, keyValue, ivValue);
            byte[] encryptedFingerPrintBytes = Convert.FromBase64String(encryptedFingerPrint);

            //保存到数据库
            string sql = "INSERT INTO FingerPrint(Fid, Name, Levels, Info, KeyValue, IvValue) VALUES(@Fid, @Name, @Levels, @Info, @KeyValue, @IvValue)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Fid", fid),
                new SqlParameter("@Name", txtName.Text),
                new SqlParameter("@Levels", matchedLevel),
                new SqlParameter("@Info", encryptedFingerPrintBytes),
                new SqlParameter("@KeyValue", keyValue),
                new SqlParameter("@IvValue", ivValue)
            };
            try
            {
                return SqlHelper.Update(sql, parameters) > 0;
            }
            catch (SqlException ex)
            {
                SqlHelper.LogError(ex);
                throw new Exception("数据库操作出现异常，原因：" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                SqlHelper.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据库中所有指纹信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool GetFingerInfo()
        {
            string sql = "SELECT Fid, Name, Levels, Info, KeyValue, IvValue FROM FingerPrint";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                MessageBox.Show("数据库中没有指纹信息!", "提示");
                return false;
            }
            else
            {
                if (fpExistSemaphore.CurrentCount == 0)
                {
                    fpExistSemaphore.Release();
                }
            }
            Dictionary<int, string> fingerPrintDict = new Dictionary<int, string>();
            personList.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                int fid = (int)dr["Fid"];
                string name = dr["Name"].ToString();
                int levelValue = (int)dr["Levels"];
                if (Enum.IsDefined(typeof(PersonnelLevel), levelValue))
                {
                    PersonnelLevel level = (PersonnelLevel)levelValue;
                    person = new Person(name, level, fid);
                    personList.Add(person);
                }
                byte[] encryptedFingerPrintBytes = (byte[])dr["Info"];
                byte[] keyValue = (byte[])dr["KeyValue"];
                byte[] ivValue = (byte[])dr["IvValue"];
                AesEncryption aes = new AesEncryption();
                string decryptedFingerPrint = aes.UseAes(Convert.ToBase64String(encryptedFingerPrintBytes), false, keyValue, ivValue);
                fingerPrintDict.Add(fid, decryptedFingerPrint);
            }

            // 将指纹信息添加到内存 
            foreach (var kvp in fingerPrintDict)
            {
                int fid = kvp.Key;
                byte[] fingerPrintBytes = Convert.FromBase64String(kvp.Value);
                int ret = fingerDB.DBAdd(mDBHandle, fid, fingerPrintBytes);
                iFid++;
                if (ret != zkfp.ZKFP_ERR_OK)
                {
                    MessageBox.Show($"指纹ID {fid} 添加到内存失败, 错误码= {ret}");
                }
            }
            
            return true;

        }

        //获取数据库等级密钥
        private void GetLevelKey()
        {
            string sql = "SELECT LevelValue, LevelKey FROM LevelInfo";
            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                MessageBox.Show("数据库中没有等级信息!");
                return;
            }

            levelKeys.Clear();
            foreach (DataRow row in dt.Rows)
            {
                int levelValue = Convert.ToInt32(row["LevelValue"]);
                string levelKey = row["LevelKey"].ToString();
                if (Enum.IsDefined(typeof(PersonnelLevel), levelValue))
                {
                    PersonnelLevel level = (PersonnelLevel)levelValue;
                    levelKeys[level] = levelKey;
                }
            }
        }
        //获取当前用户
        private Person GetPersonByFid(int fid)
        {
            return personList.FirstOrDefault(p => p.ID == fid);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormHandle = this.Handle;
            InitFinger();
            OpenDevice();
            GetFingerInfo();
            GetLevelKey();
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 显示确认对话框
            DialogResult result = MessageBox.Show("确定要关闭窗口吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // 如果用户选择 "No"，则取消关闭操作
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            // 异步执行 CloseAndFree 方法
            await Task.Run(() => CloseAndFree());
        }

        private void bnCancel_Click(object sender, EventArgs e)
        {
            if (registrationSemaphore.CurrentCount == 1)
            {
                MessageBox.Show("没有可以取消的注册操作!", "提示");
            }
            else
            {
                
                DialogResult result = MessageBox.Show("确定要取消注册吗？", "警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ResetRegistration();
                }
            }
        }

        private void ResetRegistration()
        {
            if (registrationSemaphore.CurrentCount == 0)
            {
                registrationSemaphore.Release();
            }
            RegisterCount = 0;
            cbRegTmp = 0;
            cbCapTmp = 2048; // 重置捕获模板大小
            textRes.Text = "注册已取消!";
            txtName.Text = "";
            txtLevelPwd.Text = "";

            // 清空保存模板的交错数组
            for (int i = 0; i < RegTmps.Length; i++)
            {
                RegTmps[i] = new byte[2048];
            }

            // 清空捕获的指纹数据
            Array.Clear(CapTmp, 0, CapTmp.Length);
        }
    }
}
