using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using LitJson;
using AssemblyCSharp;
//结构定义
//网络验证
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
public struct TCP_Validate
{
    public char[] szValidateKey; //验证字符
}

//网络内核
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
public struct TCP_Info
{
    public byte cbDataKind; //数据类型
    public byte cbCheckCode; //效验字段
    public ushort wPacketSize; //数据大小
}

//网络命令
//网络包头
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
public struct TCP_Head
{
    public TCP_Info TCPInfo; //基础结构
    public TCP_Command CommandInfo; //命令信息
}

//网络缓冲
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
public struct TCP_Buffer
{
    public TCP_Head Head; //数据包头

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NetUtil.SOCKET_TCP_BUFFER)]
    public byte[] cbBuffer; //数据缓冲,SOCKET_TCP_PACKET
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
public struct TCP_Command
{
    public ushort wMainCmdID;
    public ushort wSubCmdID;
}

public enum CMD_TYPE
{
    MDM_KN_COMMAND = 0, //内核命令
    SUB_KN_DETECT_SOCKET = 1, //检测命令
    SUB_KN_VALIDATE_SOCKET = 2 //验证命令
}

public class NetUtil : MonoBehaviour
{
    //无效数值
    public static readonly byte INVALID_BYTE = 0xff; //无效数值
    public static readonly ushort INVALID_USHORT = 0xffff; //无效数值

    public static readonly uint INVALID_UINT = 0xffffffff; //无效数值

    //数据类型
    public static readonly byte DK_MAPPED = 0X01; //映射类型
    public static readonly byte DK_ENCRYPT = 0X02; //加密类型
    public static readonly byte DK_COMPRESS = 0X04; //压缩类型  
    public static readonly int MDM_KN_COMMAND; //内核命令
    public static readonly int SUB_KN_DETECT_SOCKET = 1; //检测命令
    public static readonly int SUB_KN_VALIDATE_SOCKET = 2; //验证命令
    public const int SOCKET_TCP_BUFFER = 16384;

    public static readonly int SOCKET_TCP_PACKET = SOCKET_TCP_BUFFER - Marshal.SizeOf(typeof(TCP_Head));

    //发送密钥
    public static readonly byte[] ENCODE_MAP = new byte[256]
    {
        0x70, 0x2F, 0x40, 0x5F, 0x44, 0x8E, 0x6E, 0x45, 0x7E, 0xAB, 0x2C, 0x1F, 0xB4, 0xAC, 0x9D, 0x91,
        0x0D, 0x36, 0x9B, 0x0B, 0xD4, 0xC4, 0x39, 0x74, 0xBF, 0x23, 0x16, 0x14, 0x06, 0xEB, 0x04, 0x3E,
        0x12, 0x5C, 0x8B, 0xBC, 0x61, 0x63, 0xF6, 0xA5, 0xE1, 0x65, 0xD8, 0xF5, 0x5A, 0x07, 0xF0, 0x13,
        0xF2, 0x20, 0x6B, 0x4A, 0x24, 0x59, 0x89, 0x64, 0xD7, 0x42, 0x6A, 0x5E, 0x3D, 0x0A, 0x77, 0xE0,
        0x80, 0x27, 0xB8, 0xC5, 0x8C, 0x0E, 0xFA, 0x8A, 0xD5, 0x29, 0x56, 0x57, 0x6C, 0x53, 0x67, 0x41,
        0xE8, 0x00, 0x1A, 0xCE, 0x86, 0x83, 0xB0, 0x22, 0x28, 0x4D, 0x3F, 0x26, 0x46, 0x4F, 0x6F, 0x2B,
        0x72, 0x3A, 0xF1, 0x8D, 0x97, 0x95, 0x49, 0x84, 0xE5, 0xE3, 0x79, 0x8F, 0x51, 0x10, 0xA8, 0x82,
        0xC6, 0xDD, 0xFF, 0xFC, 0xE4, 0xCF, 0xB3, 0x09, 0x5D, 0xEA, 0x9C, 0x34, 0xF9, 0x17, 0x9F, 0xDA,
        0x87, 0xF8, 0x15, 0x05, 0x3C, 0xD3, 0xA4, 0x85, 0x2E, 0xFB, 0xEE, 0x47, 0x3B, 0xEF, 0x37, 0x7F,
        0x93, 0xAF, 0x69, 0x0C, 0x71, 0x31, 0xDE, 0x21, 0x75, 0xA0, 0xAA, 0xBA, 0x7C, 0x38, 0x02, 0xB7,
        0x81, 0x01, 0xFD, 0xE7, 0x1D, 0xCC, 0xCD, 0xBD, 0x1B, 0x7A, 0x2A, 0xAD, 0x66, 0xBE, 0x55, 0x33,
        0x03, 0xDB, 0x88, 0xB2, 0x1E, 0x4E, 0xB9, 0xE6, 0xC2, 0xF7, 0xCB, 0x7D, 0xC9, 0x62, 0xC3, 0xA6,
        0xDC, 0xA7, 0x50, 0xB5, 0x4B, 0x94, 0xC0, 0x92, 0x4C, 0x11, 0x5B, 0x78, 0xD9, 0xB1, 0xED, 0x19,
        0xE9, 0xA1, 0x1C, 0xB6, 0x32, 0x99, 0xA3, 0x76, 0x9E, 0x7B, 0x6D, 0x9A, 0x30, 0xD6, 0xA9, 0x25,
        0xC7, 0xAE, 0x96, 0x35, 0xD0, 0xBB, 0xD2, 0xC8, 0xA2, 0x08, 0xF3, 0xD1, 0x73, 0xF4, 0x48, 0x2D,
        0x90, 0xCA, 0xE2, 0x58, 0xC1, 0x18, 0x52, 0xFE, 0xDF, 0x68, 0x98, 0x54, 0xEC, 0x60, 0x43, 0x0F
    };

    public static readonly byte[] DECODE_MAP = new byte[256]
    {
        0x51, 0xA1, 0x9E, 0xB0, 0x1E, 0x83, 0x1C, 0x2D, 0xE9, 0x77, 0x3D, 0x13, 0x93, 0x10, 0x45, 0xFF,
        0x6D, 0xC9, 0x20, 0x2F, 0x1B, 0x82, 0x1A, 0x7D, 0xF5, 0xCF, 0x52, 0xA8, 0xD2, 0xA4, 0xB4, 0x0B,
        0x31, 0x97, 0x57, 0x19, 0x34, 0xDF, 0x5B, 0x41, 0x58, 0x49, 0xAA, 0x5F, 0x0A, 0xEF, 0x88, 0x01,
        0xDC, 0x95, 0xD4, 0xAF, 0x7B, 0xE3, 0x11, 0x8E, 0x9D, 0x16, 0x61, 0x8C, 0x84, 0x3C, 0x1F, 0x5A,
        0x02, 0x4F, 0x39, 0xFE, 0x04, 0x07, 0x5C, 0x8B, 0xEE, 0x66, 0x33, 0xC4, 0xC8, 0x59, 0xB5, 0x5D,
        0xC2, 0x6C, 0xF6, 0x4D, 0xFB, 0xAE, 0x4A, 0x4B, 0xF3, 0x35, 0x2C, 0xCA, 0x21, 0x78, 0x3B, 0x03,
        0xFD, 0x24, 0xBD, 0x25, 0x37, 0x29, 0xAC, 0x4E, 0xF9, 0x92, 0x3A, 0x32, 0x4C, 0xDA, 0x06, 0x5E,
        0x00, 0x94, 0x60, 0xEC, 0x17, 0x98, 0xD7, 0x3E, 0xCB, 0x6A, 0xA9, 0xD9, 0x9C, 0xBB, 0x08, 0x8F,
        0x40, 0xA0, 0x6F, 0x55, 0x67, 0x87, 0x54, 0x80, 0xB2, 0x36, 0x47, 0x22, 0x44, 0x63, 0x05, 0x6B,
        0xF0, 0x0F, 0xC7, 0x90, 0xC5, 0x65, 0xE2, 0x64, 0xFA, 0xD5, 0xDB, 0x12, 0x7A, 0x0E, 0xD8, 0x7E,
        0x99, 0xD1, 0xE8, 0xD6, 0x86, 0x27, 0xBF, 0xC1, 0x6E, 0xDE, 0x9A, 0x09, 0x0D, 0xAB, 0xE1, 0x91,
        0x56, 0xCD, 0xB3, 0x76, 0x0C, 0xC3, 0xD3, 0x9F, 0x42, 0xB6, 0x9B, 0xE5, 0x23, 0xA7, 0xAD, 0x18,
        0xC6, 0xF4, 0xB8, 0xBE, 0x15, 0x43, 0x70, 0xE0, 0xE7, 0xBC, 0xF1, 0xBA, 0xA5, 0xA6, 0x53, 0x75,
        0xE4, 0xEB, 0xE6, 0x85, 0x14, 0x48, 0xDD, 0x38, 0x2A, 0xCC, 0x7F, 0xB1, 0xC0, 0x71, 0x96, 0xF8,
        0x3F, 0x28, 0xF2, 0x69, 0x74, 0x68, 0xB7, 0xA3, 0x50, 0xD0, 0x79, 0x1D, 0xFC, 0xCE, 0x8A, 0x8D,
        0x2E, 0x62, 0x30, 0xEA, 0xED, 0x2B, 0x26, 0xB9, 0x81, 0x7C, 0x46, 0x89, 0x73, 0xA2, 0xF7, 0x72
    };

    //加密数据
    protected static byte m_cbSendRound; //字节映射
    protected static byte m_cbRecvRound; //字节映射
    protected static uint m_dwSendXorKey; //发送密钥

    public static uint m_dwRecvXorKey; //接收密钥

    //计数变量
    protected static int m_dwSendPacketCount; //发送计数
    protected static int m_dwRecvPacketCount; //接受计数
    static DateTime DateStart = new DateTime(1970, 1, 1, 0, 0, 0);

    public static void Init()
    {
        m_cbSendRound = 0;
        m_cbRecvRound = 0;
        m_dwSendXorKey = 0;
        m_dwRecvXorKey = 0;
        m_dwSendPacketCount = 0;
        m_dwRecvPacketCount = 0;
    }

    public static bool mappedBuffer(byte[] buffer, int wDataSize)
    {
        //变量定义
        var pInfo = new TCP_Info();
        var cbCheckCode = 0;
        //映射数据
        for (var i = Marshal.SizeOf(pInfo); i < wDataSize; i++)
        {
            cbCheckCode += buffer[i];
            buffer[i] = ENCODE_MAP[buffer[i]];
        }

        //设置数据
        pInfo.cbCheckCode = (byte)(~cbCheckCode + 1);
        pInfo.wPacketSize = (ushort)wDataSize;
        pInfo.cbDataKind = 0x05;
        var bInfo = StructToBytes(pInfo);
        Array.Copy(bInfo, buffer, bInfo.Length);
        return true;
    }

    public static int EncryptBuffer(byte[] buffer, int wDataSize)
    {
        MyDebug.Log("EncryptBuffer!!");
        int wEncryptSize = wDataSize - Marshal.SizeOf(typeof(TCP_Command));
        int wSnapCount = 0;
        if ((wEncryptSize % Marshal.SizeOf(typeof(int)) != 0))
        {
            wSnapCount = Marshal.SizeOf(typeof(int)) - wEncryptSize % Marshal.SizeOf(typeof(int));

        }


        //映射数据
        for (var i = Marshal.SizeOf(typeof(TCP_Info)); i < wDataSize; i++)
        {
            var bt = buffer[i];
            buffer[i] = ENCODE_MAP[bt];
        }


        ////创建密钥
        //var dwXorKey = m_dwSendXorKey;
        //if (m_dwSendPacketCount == 0)
        //{
        //    //随机映射种子
        //    dwXorKey = 111;
        //    dwXorKey = SeedRandMap((ushort) dwXorKey);
        //    dwXorKey |= (SeedRandMap((ushort) (dwXorKey >> 16))) << 16;
        //    dwXorKey ^= g_dwPacketKey;
        //    m_dwSendXorKey = dwXorKey;
        //    m_dwRecvXorKey = dwXorKey;
        //}

        ////加密数据
        //var wEncrypCount = (wEncryptSize + wSnapCount) / Marshal.SizeOf(typeof(int));
        //var byte4 = new byte[4];
        //var byte2 = new byte[2];
        ////插入密钥
        //if (m_dwSendPacketCount == 0)
        //{
        //    Array.Copy(buffer, Marshal.SizeOf(typeof(TCP_Head)), buffer,
        //        Marshal.SizeOf(typeof(TCP_Head)) + Marshal.SizeOf(typeof(int)),
        //        wDataSize - Marshal.SizeOf(typeof(TCP_Head)));
        //    var sendBye = StructToBytes(m_dwSendXorKey);
        //    Array.Copy(sendBye, 0, buffer, Marshal.SizeOf(typeof(TCP_Head)), Marshal.SizeOf(typeof(int)));
        //    wDataSize += Marshal.SizeOf(typeof(int));
        //}

        ////设置变量
        //m_dwSendPacketCount++;
        //m_dwRecvPacketCount++;
        //m_dwSendXorKey = dwXorKey;
        //MyDebug.Log("EncryptBuffer!!ENd");
        return wDataSize;
    }

    //加密密钥
    const uint g_dwPacketKey = 0xA;

    //随机映射
    static uint SeedRandMap(ushort wSeed)
    {
        uint dwHold = wSeed;
        return ((dwHold = dwHold * 241103 + 2533101) >> 16);
    }

    public static short CrevasseBuffer(TCP_Info tInfo, byte[] pcbDataBuffer, int wDataSize)
    {
        var wSnapCount = 0;
        if ((wDataSize % sizeof(int)) != 0)
        {
            wSnapCount = sizeof(int) - wDataSize % sizeof(int);
        }



        //数据包长度错误
        if (wDataSize < (Marshal.SizeOf(typeof(TCP_Command))))
            return 0;
        //映射     
        for (var i = 0; i < wDataSize; i++)
        {
            var bt = DECODE_MAP[pcbDataBuffer[i]];
            pcbDataBuffer[i] = bt;
        }
        return (short)wDataSize;
    }

    public static bool UnMappedBuffer(byte[] buffer, int size)
    {
        var Tcp_Info = typeof(TCP_Info);
        var pInfo = (TCP_Info)BytesToStruct(buffer, Tcp_Info, Marshal.SizeOf(Tcp_Info));
        //映射
        if ((pInfo.cbDataKind & DK_MAPPED) != 0)
        {
            byte cbCheckCode = pInfo.cbCheckCode;
            for (var i = Marshal.SizeOf(pInfo); i < size; i++)
            {
                cbCheckCode += DECODE_MAP[buffer[i]];
                buffer[i] = DECODE_MAP[buffer[i]];
            }

            //效验
            if (cbCheckCode != 0)
                return false;
        }

        return true;
    }

    public static long GetTimeStamp()
    {
        var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    //结构体转字节数组
    //public static byte[] StructToBytes(object structObj, int size = 0)
    //{
    //    if (size == 0)
    //    {
    //        size = Marshal.SizeOf(structObj);
    //    }
    //    IntPtr buffer = Marshal.AllocHGlobal(size);
    //    try
    //    {
    //        Marshal.StructureToPtr(structObj, buffer, false);
    //        byte[] bytes = new byte[size];
    //        Marshal.Copy(buffer, bytes, 0, size);
    //        return bytes;
    //    }
    //    catch (Exception ex)
    //    {
    //        MyDebug.LogWarning("struct to bytes error:" + ex);
    //        return null;
    //    }
    //    finally
    //    {
    //        Marshal.FreeHGlobal(buffer);
    //    }
    //}
    //字节数组转结构体
    public static object BytesToStruct(byte[] bytes, Type strcutType, int nSize, int index = 0)
    {
        if (bytes == null)
        {
            MyDebug.LogWarning("null bytes!!!!!!!!!!!!!");
        }

        var size = Marshal.SizeOf(strcutType);
        var buffer = Marshal.AllocHGlobal(nSize);
        try
        {
            Marshal.Copy(bytes, index, buffer, nSize);
            return Marshal.PtrToStructure(buffer, strcutType);
        }
        catch (Exception ex)
        {
            MyDebug.LogWarning("Type: " + strcutType.ToString() + "---TypeSize:" + size + "----packetSize:" + nSize);
            return null;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public static bool CheckStructSize<T>(int size)
    {
        return true;
        var structSize = Marshal.SizeOf(typeof(T));
        if (size != structSize)
            throw new Exception("---CheckStructSize failed!!! nSize(" + size + ")!=structSize(" + structSize + ")...");
        return true;
    }
    public static string GetStringByBytes(byte[] bytes)
    {
        int len = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] > 0)
                len++;
        }

        MyDebug.Log("########################Len:" + len);
        byte[] blist = new byte[len];
        Array.Copy(bytes, blist, len);
        return NetUtil.BytesToString(blist);
    }
    public static byte[] RechargeBytes(byte[] bytes)
    {
        byte[] blist = new byte[10];
        return blist;
    }
    public static byte[] StringToBytes(string str)
    {
        return Encoding.Unicode.GetBytes(str);
    }

    public static string BytesToString(byte[] bytes)
    {
        
            return ChatsToString(bytes); //Encoding.Unicode.GetString(bytes);
      
    } 
    public static string ChatsToString(byte[] bytes)
    {               
        Char[] c = Encoding.Unicode.GetChars(bytes);
        return new string(c);    
    }

    public static byte[] StructToBytes<T>(T obj)
    {
        var size = Marshal.SizeOf(obj);
        var bytes = new byte[size];
        var arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
        Marshal.StructureToPtr(obj, arrPtr, true);
        return bytes;
    }

    public static string ObjToJson<T>(T cl)
    {
        var mes = JsonMapper.ToJson(cl);
        return mes;
    }

    public static T JsonToObj<T>(string mes)
    {
        return JsonMapper.ToObject<T>(mes);
    }

    public static T BytesToStruct<T>(byte[] bytes)
    {
        var arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
        return (T)Marshal.PtrToStructure(arrPtr, typeof(T));
    }

    public static string GetServerLog(byte[] bytes)
    {
        MyDebug.Log("GetServerLog");
        // byte[] buffer = Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, bytes);
        // return Encoding.GetEncoding("GBK").GetString(bytes);
        return Encoding.UTF8.GetString(bytes);
    }
    private static string path;

    public static void ShotSceneTexture()
    {
        path = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(path);
        MyDebug.Log(path);
     //   UnityPhoneManager.Instance.ShareTimelineImage(Application.persistentDataPath + "/" + path);
        UIManager.instance.Show(UIType.UIScreenshot, (go) => { go.GetComponent<UIPanel_Screenshot>().Init(path); });
    }
    public static int PuCardChange(PU_KE pk)
    {
        int pkid = 0;
        switch (pk)
        {
            case PU_KE.F1:
                pkid = 1;
                break;
            case PU_KE.F2:
                pkid = 2;
                break;
            case PU_KE.F3:
                pkid = 3;
                break;
            case PU_KE.F4:
                pkid = 4;
                break;
            case PU_KE.F5:
                pkid = 5;
                break;
            case PU_KE.F6:
                pkid = 6;
                break;
            case PU_KE.F7:
                pkid = 7;
                break;
            case PU_KE.F8:
                pkid = 8;
                break;
            case PU_KE.F9:
                pkid = 9;
                break;
            case PU_KE.F10:
                pkid = 10;
                break;
            case PU_KE.F11:
                pkid = 11;
                break;
            case PU_KE.F12:
                pkid = 12;
                break;
            case PU_KE.F13:
                pkid = 13;
                break;
            case PU_KE.M1:
                pkid = 14;
                break;
            case PU_KE.M2:
                pkid = 15;
                break;
            case PU_KE.M3:
                pkid = 16;
                break;
            case PU_KE.M4:
                pkid = 17;
                break;
            case PU_KE.M5:
                pkid = 18;
                break;
            case PU_KE.M6:
                pkid = 19;
                break;
            case PU_KE.M7:
                pkid = 20;
                break;
            case PU_KE.M8:
                pkid =21;
                break;
            case PU_KE.M9:
                pkid = 22;
                break;
            case PU_KE.M10:
                pkid = 23;
                break;
            case PU_KE.M11:
                pkid = 24;
                break;
            case PU_KE.M12:
                pkid = 25;
                break;
            case PU_KE.M13:
                pkid = 26;
                break;
            case PU_KE.HONG1:
                pkid = 27;
                break;
            case PU_KE.HONG2:
                pkid = 28;
                break;
            case PU_KE.HONG3:
                pkid = 29;
                break;
            case PU_KE.HONG4:
                pkid = 30;
                break;
            case PU_KE.HONG5:
                pkid = 31;
                break;
            case PU_KE.HONG6:
                pkid = 32;
                break;
            case PU_KE.HONG7:
                pkid = 33;
                break;
            case PU_KE.HONG8:
                pkid = 34;
                break;
            case PU_KE.HONG9:
                pkid = 35;
                break;
            case PU_KE.HONG10:
                pkid = 36;
                break;
            case PU_KE.HONG11:
                pkid = 37;
                break;
            case PU_KE.HONG12:
                pkid = 38;
                break;
            case PU_KE.HONG13:
                pkid = 39;
                break;
            case PU_KE.HENG1:
                pkid = 40;
                break;
            case PU_KE.HENG2:
                pkid = 41;
                break;
            case PU_KE.HENG3:
                pkid = 42;
                break;
            case PU_KE.HENG4:
                pkid = 43;
                break;
            case PU_KE.HENG5:
                pkid = 44;
                break;
            case PU_KE.HENG6:
                pkid = 45;
                break;
            case PU_KE.HENG7:
                pkid = 46;
                break;
            case PU_KE.HENG8:
                pkid = 47;
                break;
            case PU_KE.HENG9:
                pkid = 48;
                break;
            case PU_KE.HENG10:
                pkid = 49;
                break;
            case PU_KE.HENG11:
                pkid = 50;
                break;
            case PU_KE.HENG12:
                pkid = 51;
                break;
            case PU_KE.HENG13:
                pkid = 52;
                break;
            case PU_KE.DAWANG:
                pkid = 54;
                break;
            case PU_KE.XIAOWAGN:
                pkid = 53;
                break;
            default:
                break;
        }
        return pkid;
    }
    public static byte PuCardChange(int pk)
    {
        PU_KE type =PU_KE.F1;
        switch (pk)
        {
            case 1:
                type = PU_KE.F1;
                break;
            case 2:
                type = PU_KE.F2;
                break;
            case 3:
                type = PU_KE.F3;
                break;
            case 4:
                type = PU_KE.F4;
                break;
            case 5:
                type = PU_KE.F5;
                break;
            case 6:
                type = PU_KE.F6;
                break;
            case 7:
                type = PU_KE.F7;
                break;
            case 8:
                type = PU_KE.F8;
                break;
            case 9:
                type = PU_KE.F9;
                break;
            case 10:
                type = PU_KE.F10;
                break;
            case 11:
                type = PU_KE.F11;
                break;
            case 12:
                type = PU_KE.F12;
                break;
            case 13:
                type = PU_KE.F13;
                break;
            case 14:
                type = PU_KE.M1;
                break;
            case 15:
                type = PU_KE.M2;
                break;
            case 16:
                type = PU_KE.M3;
                break;
            case 17:
                type = PU_KE.M4;
                break;
            case 18:
                type = PU_KE.M5;
                break;
            case 19:
                type = PU_KE.M6;
                break;
            case 20:
                type = PU_KE.M7;
                break;
            case 21:
                type = PU_KE.M8;
                break;
            case 22:
                type = PU_KE.M9;
                break;
            case 23:
                type = PU_KE.M10;
                break;
            case 24:
                type = PU_KE.M11;
                break;
            case 25:
                type = PU_KE.M12;
                break;
            case 26:
                type = PU_KE.M13;
                break;
            case 27:
                type = PU_KE.HONG1;
                break;
            case 28:
                type = PU_KE.HONG2;
                break;
            case 29:
                type = PU_KE.HONG3;
                break;
            case 30:
                type = PU_KE.HONG4;
                break;
            case 31:
                type = PU_KE.HONG5;
                break;
            case 32:
                type = PU_KE.HONG6;
                break;
            case 33:
                type = PU_KE.HONG7;
                break;
            case 34:
                type = PU_KE.HONG8;
                break;
            case 35:
                type = PU_KE.HONG9;
                break;
            case 36:
                type = PU_KE.HONG10;
                break;
            case 37:
                type = PU_KE.HONG11;
                break;
            case 38:
                type = PU_KE.HONG12;
                break;
            case 39:
                type = PU_KE.HONG13;
                break;
            case 40:
                type = PU_KE.HENG1;
                break;
            case 41:
                type = PU_KE.HENG2;
                break;
            case 42:
                type = PU_KE.HENG3;
                break;
            case 43:
                type = PU_KE.HENG4;
                break;
            case 44:
                type = PU_KE.HENG5;
                break;
            case 45:
                type = PU_KE.HENG6;
                break;
            case 46:
                type = PU_KE.HENG7;
                break;
            case 47:
                type = PU_KE.HENG8;
                break;
            case 48:
                type = PU_KE.HENG9;
                break;
            case 49:
                type = PU_KE.HENG10;
                break;
            case 50:
                type = PU_KE.HENG11;
                break;
            case 51:
                type = PU_KE.HENG12;
                break;
            case 52:
                type = PU_KE.HENG13;
                break;
            case 53:
                type = PU_KE.XIAOWAGN;
                break;
            case 54:
                type = PU_KE.DAWANG;
                break;               
        }
        return (byte)type;
    }
}