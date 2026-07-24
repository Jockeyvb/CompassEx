using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommLib
{
    /// <summary>
    /// 通用动态类型 API 返回结果实体类。
    /// </summary>
    /// <remarks>
    /// 本类用于在 Web 接口层进行初级报文拦截或多层网关转发，通过统一的动态契约封装返回结构。
    /// </remarks>
    public class ApiDynamicResult
    {
        /// <summary>
        /// 获取或设置 HTTP 或业务自定义状态码。
        /// </summary>
        /// <value>整型状态码，通常 200 代表成功，4xx/5xx 代表各级业务或系统异常。</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// 获取或设置动态 JSON 令牌内容。
        /// </summary>
        /// <value>声明为万能的 <see cref="JToken"/>，可完美兼容并解析一切 {}（对象）和 []（数组）等复杂的 JSON 拓扑结构。</value>
        public JToken Data { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示本次 API 请求业务层是否成功执行。
        /// </summary>
        /// <value>若成功执行则为 <see langword="true"/>；否则为 <see langword="false"/>。</value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 获取或设置接口发生错误时的异常或错误详情描述对象。
        /// </summary>
        /// <value>通常表现为标准字典、字符串数组或自定义错误对象结构。</value>
        public object Errors { get; set; }

        /// <summary>
        /// 获取或设置接口返回的附加扩展信息对象。
        /// </summary>
        /// <value>用于传递分页元数据、性能耗时等不需要合并入核心 Data 节点的附加指标。</value>
        public object Extras { get; set; }

        /// <summary>
        /// 获取或设置服务器响应时的 Unix 时间戳（毫秒级）。
        /// </summary>
        /// <value>自 1970-01-01 00:00:00 UTC 起算的累计毫秒数，常用于多端时区对齐或接口幂等指纹校验。</value>
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 通用强类型 API 返回结果外壳类。
    /// </summary>
    /// <typeparam name="T">期望最终转换或绑定的业务层真实强类型数据源类型。</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 获取或设置 HTTP 或业务自定义状态码。
        /// </summary>
        /// <value>整型状态码，保持与原始动态返回内容完全一致。</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// 获取或设置强类型的业务核心结果数据。
        /// </summary>
        /// <value>由泛型 <typeparamref name="T"/> 约束的强类型口袋，在客户端或下游消费时可免去手动转型的烦恼。</value>
        public T Data { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示本次 API 请求业务层是否成功执行。
        /// </summary>
        /// <value>成功则为 <see langword="true"/>；否则为 <see langword="false"/>。</value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 获取或设置接口发生错误时的异常或错误详情描述对象。
        /// </summary>
        /// <value>通常表现为标准字典、字符串数组或自定义错误对象结构。</value>
        public object Errors { get; set; }

        /// <summary>
        /// 获取或设置接口返回的附加扩展信息对象。
        /// </summary>
        /// <value>保持与动态包装结构中的扩展段完全相同的透传数据。</value>
        public object Extras { get; set; }

        /// <summary>
        /// 获取或设置服务器响应时的 Unix 时间戳（毫秒级）。
        /// </summary>
        /// <value>自 1970-01-01 00:00:00 UTC 起算的累计毫秒数。</value>
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 针对原先 <see cref="ApiDynamicResult"/> 的动态高级转换链式扩展工具类。
    /// </summary>
    public static class ApiResultExtensions
    {
        /// <summary>
        /// 将万能的动态 <see cref="JToken"/> 瞬间强转映射为指定的任意强类型泛型包装对象。
        /// </summary>
        /// <typeparam name="T">期望转换并填充的目标强类型（例如实体列表或布尔值）。</typeparam>
        /// <param name="dynamicResult">当前正在执行链式调用的原始动态 API 结果实例。</param>
        /// <returns>返回装配完毕、带有强类型业务数据的全新 <see cref="ApiResult{T}"/> 外壳实例。</returns>
        /// <remarks>
        /// 1. 若传入的动态源为空（<see langword="null"/>），则直接安全断言并返回空。<br/>
        /// 2. 方法会自动校验业务状态标识 <see cref="ApiDynamicResult.Succeeded"/>；若接口判定失败、或者核心数据节点无有效内容，则直接截断后续的反射开销，安全赋予泛型默认值（<see langword="default"/>）。<br/>
        /// 3. 反序列化底层采用 Newtonsoft 的元数据逆向对账机制（<see cref="JToken.ToObject{T}()"/>），支持自动抹平因大小写差异或非对等结构导致的微小错配。
        /// </remarks>
        public static ApiResult<T> ToResult<T>(this ApiDynamicResult dynamicResult)
        {
            if (dynamicResult == null) return null;

            var finalResult = new ApiResult<T>
            {
                StatusCode = dynamicResult.StatusCode,
                Succeeded = dynamicResult.Succeeded,
                Errors = dynamicResult.Errors,
                Extras = dynamicResult.Extras,
                Timestamp = dynamicResult.Timestamp
            };

            // 如果接口本身失败了，或者 Data 为空，直接返回壳，不浪费性能去转换
            if (!dynamicResult.Succeeded || dynamicResult.Data == null || dynamicResult.Data.Type == JTokenType.Null)
            {
                finalResult.Data = default;
                return finalResult;
            }

            // 利用 Newtonsoft 强大的 ToObject，自动抹平大小写差异和结构错配
            finalResult.Data = dynamicResult.Data.ToObject<T>();

            return finalResult;
        }
    }

}
