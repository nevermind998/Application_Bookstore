using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LoboPraksa_Zadatak1
{
    
    public class ZipMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ContentEncodingHeader = "Content-Encoding";
        private const string ContentEncodingGzip = "gzip";
        private const string ContentEncodingDeflate = "deflate";

        public ZipMiddleware(RequestDelegate next)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(ContentEncodingHeader) && (context.Request.Headers[ContentEncodingHeader] == ContentEncodingGzip || context.Request.Headers[ContentEncodingHeader] == ContentEncodingDeflate))
            {
                var contentEncoding = context.Request.Headers[ContentEncodingHeader];
                var decompressor = contentEncoding == ContentEncodingGzip ? (Stream)new GZipStream(context.Request.Body, CompressionMode.Decompress, true) : (Stream)new DeflateStream(context.Request.Body, CompressionMode.Decompress, true);
                context.Request.Body = decompressor;
            }
            await _next(context);
        }

    }

    public static class ZipMiddlewareExtensions
    {
        public static IApplicationBuilder UseZipMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ZipMiddleware>();
        }
    }
}
