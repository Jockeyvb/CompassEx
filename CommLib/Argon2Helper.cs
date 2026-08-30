
using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;
namespace CommLib;

public class Argon2Helper
{
    /// <summary>
    /// 生成 Argon2id 哈希
    /// </summary>
    /// <param name="password">密码明文</param>
    /// <returns></returns>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8, // 线程数
            Iterations = 4,          // 迭代次数
            MemorySize = 65536       // 内存开销 (单位 KB，即 64MB)
        };

        byte[] hash = argon2.GetBytes(32); // 生成 32 字节哈希

        // 拼接 Salt 和 Hash 用于持久化存储
        byte[] hashBytes = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password"></param>
    /// <param name="base64Hash"></param>
    /// <returns></returns>
    public static bool VerifyPassword(string password, string base64Hash)
    {
        byte[] hashBytes = Convert.FromBase64String(base64Hash);

        // 提取 Salt (前 16 字节)
        byte[] salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

        // 提取原本的 Hash 长度
        int storedHashLength = hashBytes.Length - 16;
        byte[] storedHash = new byte[storedHashLength];
        Buffer.BlockCopy(hashBytes, 16, storedHash, 0, storedHashLength);

        // 重新计算输入密码的哈希
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 65536
        };

        byte[] computedHash = argon2.GetBytes(storedHashLength);

        // 恒定时间比较（防止时序攻击）
        return SafeEquals(computedHash, storedHash);
    }
    // 替代 FixedTimeEquals 的安全比较方法（防止时序攻击）
    private static bool SafeEquals(byte[] a, byte[] b)
    {
        if (a == null || b == null)
        {
            return false;
        }

        if (a.Length != b.Length)
        {
            return false;
        }

        int diff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];
        }
        return diff == 0;
    }
}